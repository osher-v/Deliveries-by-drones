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
        /// abstract class of drone.
        /// </summary>
        public abstract class DroneParent
        {
            public int Id { get; set; }

            public string Model { get; set; }

            public WeightCategories MaxWeight { get; set; }

            public double BatteryStatus { get; set; }

            public DroneStatuses Statuses { get; set; }

            public Location CurrentLocation { get; set; }

            public override string ToString()
            {
                return string.Format("the Id is {0,-5} \t the Model is {1,-5} \n" +
                    "the MaxWeight is {2,-5} \t the BatteryStatus is {3}%\t" +
                    "the Statuses is {4,-5} \n the CurrentLocation is {5,-5} "
                    , Id, Model, MaxWeight, BatteryStatus, Statuses, CurrentLocation);
            }
        }
    }  
}
