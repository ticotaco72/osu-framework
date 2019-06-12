// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using System.Collections.Generic;
using ManagedBass;
using osu.Framework.Configuration;
using osu.Framework.IO.Stores;
using osu.Framework.Threading;
using System.Linq;
using System.Diagnostics;
using osu.Framework.Extensions.TypeExtensions;
using osu.Framework.Logging;

namespace osu.Framework.Audio
{
    //add some abstarction about devices and reading from every single one; but first implement reading from one and providing it further
    public class RecordManager : RecordComponent
    {
        /// <summary>
        /// The thread audio/record operations (mainly Bass calls) are ran on.
        /// </summary>
        internal readonly AudioThread Thread;

        private List<RecordDevice> recordDevices = new List<RecordDevice>();
        private List<RecordDevice> readyDevices = new List<RecordDevice>();

        public List<RecordDevice> AllRecordDevices => recordDevices;
        public List<RecordDevice> NowRecordDevices => readyDevices;

        private RecordDevice currentOperationRecordDevice;

        //make some use of it; przenieść na poziom recorddevice
        /// <summary>
        /// Master(default) volume of all RecordDevices.
        /// </summary>
        public readonly BindableDouble VolumeRecord = new BindableDouble(1)
        {
            MinValue = 0,
            MaxValue = 1
        };

        private Scheduler scheduler => Thread.Scheduler;

        private Scheduler eventScheduler => EventScheduler ?? scheduler;

        /// <summary>
        /// The scheduler used for invoking publicly exposed delegate events.
        /// </summary>
        public Scheduler EventScheduler;

        /// <summary>
        /// Constructs a RecordManager given a thread.
        /// </summary>
        public RecordManager(AudioThread thread)
        {
            //RecordDevice.ValueChanged += onDeviceChanged;

            Thread = thread;

            scheduler.AddDelayed(delegate
            {
                updateAvailableRecordDevices();
                //checkRecordDeviceChanged();
            }, 1000, true);
        }

        protected override void Dispose(bool disposing)
        {
            //OnNewDevice = null;
            //OnLostDevice = null;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Returns a list of the names of recognized audio devices.
        /// </summary>
        /// <remarks>
        /// The No Sound device that is in the list of Audio Devices that are stored internally is not returned.
        /// Regarding the .Skip(1) as implementation for removing "No Sound", see http://bass.radio42.com/help/html/e5a666b4-1bdd-d1cb-555e-ce041997d52f.htm.
        /// </remarks>
        /// <returns>A list of the names of recognized audio devices.</returns>
        private IEnumerable<string> getDeviceNames(List<RecordDevice> devices) => devices.Select(d => d.Info.Name);

        private List<RecordDevice> getAllDevices()
        {
            int deviceCount = Bass.RecordingDeviceCount;
            List<RecordDevice> info = new List<RecordDevice>();
            for (int i = 0; i < deviceCount; i++)
                info.Add(new RecordDevice(this, Bass.RecordGetDeviceInfo(i), Bass.RecordingInfo, i));

            return info;
        }

        private void setCurrentOperationRecordDevice(RecordDevice recordDevice)
        {
            updateAvailableRecordDevices();
            if (recordDevice.Info.IsInitialized && recordDevice.Info.IsEnabled)
            {
                if (Bass.RecordGetDeviceInfo(Bass.CurrentRecordingDevice).Name != recordDevice.Info.Name)
                    Bass.CurrentRecordingDevice = recordDevices.FindIndex(df => df.Info.Name == recordDevice.Info.Name);
                else
                    return;
            }
            else
            {
                if (recordDevice.Info.IsEnabled)
                    initRecordDevice(recordDevice);
                else
                    //give there some normal exception
                    throw new Exception();
            }
        }

        private bool initRecordDevice(RecordDevice preferredDevice)
        {
            if (!Bass.RecordInit(preferredDevice.BassIndex) && Bass.LastError != Errors.Already)
            {
                throw new Exception();
            }

            if (Bass.LastError == Errors.Already)
            {
                // We check if the initialization error is that we already initialized the device
                // If it is, it means we can just tell Bass to use the already initialized device without much
                // other fuzz.
                Bass.CurrentRecordingDevice = preferredDevice.BassIndex;
                Bass.RecordFree();
                Bass.RecordInit(preferredDevice.BassIndex);
            }

            Trace.Assert(Bass.LastError == Errors.OK);

            Logger.Log($@"BASS Initialized
                          BASS Version:               {Bass.Version}
                          BASS FX Version:            {ManagedBass.Fx.BassFx.Version}
                          Device:                     {preferredDevice.Info.Name}
                          Drive:                      {preferredDevice.Info.Driver}");

            //we have successfully initialised a new device.
            //taka opcja prawdopodobnie tylko kopiuje wartosci, amy chcemy zeby to bylo reference do oryginału
            currentOperationRecordDevice = preferredDevice;

            //managedbass' default
            //think about it czy to jest koniecznne? tak, czy tutaj? Czy na pewno tutaj? czy zawsze ta sama wartość?
            Bass.RecordingBufferLength = 2000;

            return true;
        }

        public void StartRecording(RecordDevice device)
        {
            //add some conditionals for arguments of the statement
            if (device.MaxBits!=8)
                device.Handle = Bass.RecordStart(device.MaxFrequency, device.MaxChannels, BassFlags.Default, 100, device.ReceiveData);
            else
                device.Handle = Bass.RecordStart(device.MaxFrequency, device.MaxChannels, BassFlags.Byte, 100, device.ReceiveData);
            //add handling of errors
        }

        private void updateAvailableRecordDevices()
        {
            var currentDeviceList = getAllDevices().ToList();//.Where(d => d.Info.IsEnabled).ToList();

            var newDevices = currentDeviceList.Except(recordDevices).ToList();
            var lostDevices = recordDevices.Except(currentDeviceList).ToList();

            foreach (RecordDevice device in newDevices)
            {
                recordDevices.Add(device);
            }
            foreach (RecordDevice device in lostDevices)
            {
                recordDevices[recordDevices.FindIndex(df => df.Info.Name == device.Info.Name)].Dispose();//lub set bassindex to -2;
            }
            //add writing changes to existing devices
            for (int i=0; i < currentDeviceList.Count(); i++)
            {
                var index = recordDevices.FindIndex(df => df.Info.Name == currentDeviceList[i].Info.Name);
                recordDevices[index].BassIndex = i;
                recordDevices[index].Info = currentDeviceList[i].Info;
                //add changing other members(volume)
            }
        }

        public override string ToString() => $@"{GetType().ReadableName()} ({currentOperationRecordDevice.Info.Name})";
    }
}
