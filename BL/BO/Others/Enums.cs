using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
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

            /// <summary>
            /// Enum for delivert status
            /// </summary>
            public enum DeliveryStatus { created, Assigned, PickedUp, Delivered }


    }
}
