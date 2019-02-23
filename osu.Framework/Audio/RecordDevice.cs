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

        //for combination that contains MaxBits and MaxChannels
        public int MaxFrequency;

        //for combination that contains MaxBits
        public int MaxChannels;

        public int MaxBits;

        internal RecordInfo AvailableOptionsInfo
        {
            set
            {
                switch(value.SupportedFormats)
                {//najpierw sortowanie(tych opcji) po ilosci bitow
                    //potem po ilosci kanałow
                    //potem po ilosci kHz
                    case RecordFormatFlags.WF96S16:
                        MaxBits = 16;
                        MaxChannels = 2;
                        MaxFrequency = 96000;
                        break;
                }
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
