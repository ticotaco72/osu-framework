using System;
using System.Collections.Generic;
using System.Text;
using ManagedBass;

namespace osu.Framework.Audio
{
    public class RecordDevice : RecordComponent
    {
        private RecordManager manager;

        public DeviceInfo Info;

        public double Volume;

        internal int BassIndex;

        //make an enum for this, we will use only master input of each device, chyba że możliwe jest jakiś surround?, ale to chyba nie jest konieczne/potrzebne
        public int InputType;

        //for combination that contains MaxBits
        public int MaxFrequency;

        //for combination that contains MaxBits and MaxFrequency
        public int MaxChannels;

        public int MaxBits;

        //this is struct!!!!!!!! add our version that is clas?? and device info too??
        private RecordInfo availableOptionsInfo;

        internal RecordInfo AvailableOptionsInfo
        {
            get { return availableOptionsInfo;  }
            set
            {
                switch(value.SupportedFormats)
                {//najpierw sortowanie(tych opcji) po ilosci bitow
                    //potem po ilosci kanałow
                    //potem po ilosci kHz
                    case RecordFormatFlags.WF96S16:
                        MaxBits = 16;
                        MaxFrequency = 96000;
                        MaxChannels = 2;
                        break;
                    case RecordFormatFlags.WF96M16:
                        MaxBits = 16;
                        MaxFrequency = 96000;
                        MaxChannels = 1;
                        break;
                    case RecordFormatFlags.WF48S16:
                        MaxBits = 16;
                        MaxFrequency = 48000;
                        MaxChannels = 2;
                        break;
                    case RecordFormatFlags.WF48M16:
                        MaxBits = 16;
                        MaxFrequency = 48000;
                        MaxChannels = 1;
                        break;
                    case RecordFormatFlags.WF4S16:
                        MaxBits = 16;
                        MaxFrequency = 44100;
                        MaxChannels = 2;
                        break;
                    case RecordFormatFlags.WF4M16:
                        MaxBits = 16;
                        MaxFrequency = 44100;
                        MaxChannels = 1;
                        break;
                    case RecordFormatFlags.WF96S08:
                        MaxBits = 8;
                        MaxFrequency = 96000;
                        MaxChannels = 2;
                        break;
                    case RecordFormatFlags.WF96M08:
                        MaxBits = 8;
                        MaxFrequency = 96000;
                        MaxChannels = 1;
                        break;
                    case RecordFormatFlags.WF48S08:
                        MaxBits = 8;
                        MaxFrequency = 48000;
                        MaxChannels = 2;
                        break;
                    case RecordFormatFlags.WF48M08:
                        MaxBits = 8;
                        MaxFrequency = 48000;
                        MaxChannels = 1;
                        break;
                    case RecordFormatFlags.WF4S08:
                        MaxBits = 8;
                        MaxFrequency = 44100;
                        MaxChannels = 2;
                        break;
                    case RecordFormatFlags.WF4M08:
                        MaxBits = 8;
                        MaxFrequency = 44100;
                        MaxChannels = 2;
                        break;
                    //add lower frequencies and default
                }

                availableOptionsInfo = value;
            }
        }

        //input stan

        //urządzenie stan

        //public initdevice??????// trgo tutaj nie będzie , wszystko ma się odbywać w starrt record

        internal int Handle;

        public bool StartRecord()//void
        {
            return true;
        }

        //delegate

        //this is the callback
        public bool ReceiveData(int Handle, IntPtr Buffer, int Length, IntPtr User)
        {
            return false;
        }

        //public RecordProcedure Receiver = new RecordProcedure(ReceiveData);

        public void StopRecord()
        {}

        //changevolume() albo bindable

        //bufer

        //isrecording????????????

        internal RecordDevice(RecordManager recordManager, DeviceInfo info, RecordInfo recordInfo, int index)//add input info oraz input type oraz volume oraz kreacje bufora
        {
            manager = recordManager;
            Info = info;
            AvailableOptionsInfo = recordInfo;
            BassIndex = index;
        }

        //dispose
    }
}
