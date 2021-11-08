using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        //רחפן בחבילה
        public class DroneInThePackage
        {
            public int Id { get; set; }

            public double BatteryStatus { get; set; }

            public Location CurrentLocation { get; set; }

            public override string ToString()
            {
                return string.Format("the Id is {0,-6} \t the BatteryStatus is {1,-6} \t" +
                    "the CurrentLocation is {2,-6}", Id, BatteryStatus, CurrentLocation);
            }
        }
    }
}
