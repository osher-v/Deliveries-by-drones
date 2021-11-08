using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
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
                return string.Format("the Id is {0,-5} \t the Model is {1,-5} \t" +
                    "the MaxWeight is {2,-5} \t the BatteryStatus is {3,-5}" +
                    "the Statuses is {4,-5} \t the CurrentLocation is {5,-5} "
                    , Id, Model, MaxWeight, BatteryStatus, Statuses, CurrentLocation);
            }
        }
    }  
}
