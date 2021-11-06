using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Drone : DroneParent
        {
            public ParcelInDelivery Delivery { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("the Delivery is: {0,-5}", Delivery);
            }
        }
    } 
}
