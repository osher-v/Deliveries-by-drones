using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// class of drone. 
    /// </summary>
    public class Drone : DroneParent
    {
        public ParcelInTransfer Delivery { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("the delivery details is: {0,-5}\n", Delivery);
        }
    }
}
