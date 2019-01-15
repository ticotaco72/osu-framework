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
    public class RecordManager : AdjustableAudioComponent
    {
        /// <summary>
        /// The thread audio/record operations (mainly Bass calls) are ran on.
        /// </summary>
        internal readonly AudioThread Thread;

        private List<DeviceInfo> recordDevices = new List<DeviceInfo>();
        private List<string> recordDeviceNames = new List<string>();

        /// <summary>
        /// The names of all available audio devices.
        /// </summary>
        public IEnumerable<string> RecordDeviceNames => recordDeviceNames;

        /// <summary>
        /// Is fired whenever a new audio device is discovered and provides its name.
        /// </summary>
        public event Action<string> OnNewDevice;

        /// <summary>
        /// Is fired whenever an audio device is lost and provides its name.
        /// </summary>
        public event Action<string> OnLostDevice;

        /// <summary>
        /// The preferred audio device we should use. A value of
        /// <see cref="string.Empty"/> denotes the OS default.
        /// </summary>
        public readonly Bindable<string> RecordDevice = new Bindable<string>();

        private string currentRecordDevice;

        //make some use of it
        /// <summary>
        /// Volume of all tracks played game-wide.
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
        public RecordManager()
        {
            RecordDevice.ValueChanged += onDeviceChanged;

            Thread = new AudioThread(Update);
            Thread.Start();

            scheduler.Add(() =>
            {
                try
                {
                    setRecordDevice();
                }
                catch
                {
                }
            });

            scheduler.AddDelayed(delegate
            {
                updateAvailableRecordDevices();
                checkRecordDeviceChanged();
            }, 1000, true);
        }

        protected override void Dispose(bool disposing)
        {
            OnNewDevice = null;
            OnLostDevice = null;

            base.Dispose(disposing);
        }

        private void onDeviceChanged(string newDevice)
        {
            scheduler.Add(() => setRecordDevice(string.IsNullOrEmpty(newDevice) ? null : newDevice));
        }

        /// <summary>
        /// Returns a list of the names of recognized audio devices.
        /// </summary>
        /// <remarks>
        /// The No Sound device that is in the list of Audio Devices that are stored internally is not returned.
        /// Regarding the .Skip(1) as implementation for removing "No Sound", see http://bass.radio42.com/help/html/e5a666b4-1bdd-d1cb-555e-ce041997d52f.htm.
        /// </remarks>
        /// <returns>A list of the names of recognized audio devices.</returns>
        private IEnumerable<string> getDeviceNames(List<DeviceInfo> devices) => devices.Select(d => d.Name);

        private List<DeviceInfo> getAllDevices()
        {
            int deviceCount = Bass.RecordingDeviceCount;
            List<DeviceInfo> info = new List<DeviceInfo>();
            for (int i = 0; i < deviceCount; i++)
                info.Add(Bass.RecordGetDeviceInfo(i));

            return info;
        }

        private bool setRecordDevice(string preferredDevice = null)
        {
            updateAvailableRecordDevices();

            string oldDevice = currentRecordDevice;
            string newDevice = preferredDevice;

            if (string.IsNullOrEmpty(newDevice))
                newDevice = recordDevices.Find(df => df.IsDefault).Name;

            bool oldDeviceValid = Bass.CurrentRecordingDevice >= 0;
            if (oldDeviceValid)
            {
                DeviceInfo oldDeviceInfo = Bass.RecordGetDeviceInfo(Bass.CurrentRecordingDevice);
                oldDeviceValid &= oldDeviceInfo.IsEnabled && oldDeviceInfo.IsInitialized;
            }

            if (newDevice == oldDevice && oldDeviceValid)
                return true;

            if (string.IsNullOrEmpty(newDevice))
            {
                Logger.Log(@"BASS Initialization failed (no record device present)");
                return false;
            }

            int newDeviceIndex = recordDevices.FindIndex(df => df.Name == newDevice);

            DeviceInfo newDeviceInfo = new DeviceInfo();

            try
            {
                if (newDeviceIndex >= 0)
                    newDeviceInfo = Bass.RecordGetDeviceInfo(newDeviceIndex);
                //we may have previously initialised this device.
            }
            catch
            {
            }

            if (oldDeviceValid && (newDeviceInfo.Driver == null || !newDeviceInfo.IsEnabled))
            {
                //handles the case we are trying to load a user setting which is currently unavailable,
                //and we have already fallen back to a sane default.
                return true;
            }

            if (!Bass.Init(newDeviceIndex) && Bass.LastError != Errors.Already)
            {
                //the new device didn't go as planned. we need another option.

                if (preferredDevice == null)
                {
                    //we're fucked. the default device won't initialise.
                    currentRecordDevice = null;
                    return false;
                }

                //let's try again using the default device.
                return setRecordDevice();
            }

            if (Bass.LastError == Errors.Already)
            {
                // We check if the initialization error is that we already initialized the device
                // If it is, it means we can just tell Bass to use the already initialized device without much
                // other fuzz.
                Bass.CurrentDevice = newDeviceIndex;
                Bass.Free();
                Bass.Init(newDeviceIndex);
            }

            Trace.Assert(Bass.LastError == Errors.OK);

            Logger.Log($@"BASS Initialized
                          BASS Version:               {Bass.Version}
                          BASS FX Version:            {ManagedBass.Fx.BassFx.Version}
                          Device:                     {newDeviceInfo.Name}
                          Drive:                      {newDeviceInfo.Driver}");

            //we have successfully initialised a new device.
            currentRecordDevice = newDevice;

            Bass.RecordingBufferLength = 2000;

            return true;
        }

        private void updateAvailableRecordDevices()
        {
            var currentDeviceList = getAllDevices().Where(d => d.IsEnabled).ToList();
            var currentDeviceNames = getDeviceNames(currentDeviceList).ToList();

            var newDevices = currentDeviceNames.Except(recordDeviceNames).ToList();
            var lostDevices = recordDeviceNames.Except(currentDeviceNames).ToList();

            if (newDevices.Count > 0 || lostDevices.Count > 0)
            {
                eventScheduler.Add(delegate
                {
                    foreach (var d in newDevices)
                        OnNewDevice?.Invoke(d);
                    foreach (var d in lostDevices)
                        OnLostDevice?.Invoke(d);
                });
            }

            recordDevices = currentDeviceList;
            recordDeviceNames = currentDeviceNames;
        }

        private void checkRecordDeviceChanged()
        {
            try
            {
                if (RecordDevice.Value == string.Empty)
                {
                    // use default device
                    var device = Bass.RecordGetDeviceInfo(Bass.CurrentRecordingDevice);
                    if (!device.IsDefault && !setRecordDevice())
                    {
                        if (!device.IsEnabled || !setRecordDevice(device.Name))
                        {
                            foreach (var d in getAllDevices())
                            {
                                if (d.Name == device.Name || !d.IsEnabled)
                                    continue;

                                if (setRecordDevice(d.Name))
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    // use whatever is the preferred device
                    var device = Bass.RecordGetDeviceInfo(Bass.CurrentRecordingDevice);
                    if (device.Name == RecordDevice.Value)
                    {
                        if (!device.IsEnabled && !setRecordDevice())
                        {
                            foreach (var d in getAllDevices())
                            {
                                if (d.Name == device.Name || !d.IsEnabled)
                                    continue;

                                if (setRecordDevice(d.Name))
                                    break;
                            }
                        }
                    }
                    else
                    {
                        var preferredDevice = getAllDevices().SingleOrDefault(d => d.Name == RecordDevice.Value);
                        if (preferredDevice.Name == RecordDevice.Value && preferredDevice.IsEnabled)
                            setRecordDevice(preferredDevice.Name);
                        else if (!device.IsEnabled && !setRecordDevice())
                        {
                            foreach (var d in getAllDevices())
                            {
                                if (d.Name == device.Name || !d.IsEnabled)
                                    continue;

                                if (setRecordDevice(d.Name))
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public override string ToString() => $@"{GetType().ReadableName()} ({currentRecordDevice})";
    }
}
