
using System;
using IDAL.DO;
using DalObject;

using System.ComponentModel;

namespace IDAL
{
    namespace DO
    {

        public enum Priorities { regular, fast, urgent }
        public enum WeightCategories { light, medium, heavy }
        public enum DroneStatuses { free, inMaintenance, busy }

        /*
        public enum Priorities
        {
            [Description("regular")]
            regular,
            [Description("fast")]
            fast,
            [Description("urgent")]
            urgent 
        }

        public enum WeightCategories
        {
            [Description("light")]
            light,
            [Description("medium")]
            medium,
            [Description("heavy")]
            heavy 
        }

        public enum DroneStatuses
        {
            [Description("free")]
            free,
            [Description("inMaintenance")]
            inMaintenance,
            [Description("busy")]
            busy 
        }
        */
    }

}






