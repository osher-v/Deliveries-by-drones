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
        /// class of parcel.
        /// </summary>
        public class Parcel :ParcelParent
        {
            public CustomerInDelivery Sender { get; set; }
            public CustomerInDelivery Receiver { get; set; }
            public DroneInThePackage MyDrone { get; set; }
            public DateTime Requested { get; set; }
            public DateTime Assigned { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

             public override string ToString()
            {
                return base.ToString() + string.Format("sender: {0}receiver: {1}the drone in parcel: {2,-5}\n" +
                    "Request started in: {3,-12}\nAssigned in: {4,-12}\n" +
                    "pick up time at: {5,-12}\ndelivered time at: {6,-12}\n ~~~~~~~~~~", Sender, Receiver, MyDrone,
                    Requested, Assigned, PickedUp, Delivered);
            }
        }
    }
}
