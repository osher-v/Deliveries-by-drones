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
        /// class of baseStation.
        /// </summary>
        public class BaseStation : BaseStationParnt
        {
            public Location BaseStationLocation { get; set; }
            public List<DroneInCharg> DroneInChargsList { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("location:{0,-8}\t ", BaseStationLocation)
                    + "Drone in chargs: " + string.Join("\n", DroneInChargsList)+"\n";
            }

        }
    }
}
