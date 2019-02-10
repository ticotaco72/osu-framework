using System;
using System.Collections.Generic;
using System.Text;
using ManagedBass;

namespace osu.Framework.Audio
{
    public class RecordDevice : AudioComponent
    {
        private RecordManager manager;

        public DeviceInfo Info;

        public double Volume;

        //make an enum for this, we will use only master input of each device, chyba że możliwe jest jakiś surround?, ale to chyba nie jest konieczne/potrzebne
        public int InputType;

        //input stan

        //urządzenie stan

        //public initdevice??????

        public bool StartRecord()
        {
            return true;
        }

        //this is the callback
        public bool ReceiveData()
        {
            return false;
        }

        public void StopRecord()
        {}

        //changevolume() albo bindable

        //bufer

        //isrecording????????????

        internal RecordDevice(RecordManager recordManager, DeviceInfo info)//add input info oraz input type oraz volume oraz kreacje bufora
        {
            manager = recordManager;
            Info = info;
        }
    }
}
