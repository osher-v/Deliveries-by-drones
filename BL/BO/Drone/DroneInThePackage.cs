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
        /// class of drone in the package.
        /// </summary>
        public class DroneInThePackage : DroneInCharg
        {
            public Location CurrentLocation { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("the CurrentLocation is {0,-6}\n", CurrentLocation);
            }
        }
    }
}
