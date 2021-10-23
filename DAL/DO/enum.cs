
using System;
using IDAL.DO;
using DalObject;

using System.ComponentModel;

namespace IDAL
{
    namespace DO
    {
     
        /// <summary>
        ///  Enum to identify the order of priority
        /// </summary>
        public enum Priorities { regular, fast, urgent }
        /// <summary>
        ///  Enum to identify the weight category
        /// </summary>
        public enum WeightCategories { light, medium, heavy }
        /// <summary>
        ///  Enum to know the status
        /// </summary>
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






