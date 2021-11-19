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
                return base.ToString() + string.Format("sender ID is: {0,-6} \t receiver ID is: {1,-6} \n the drone in parcel: {2,-5}" +
                    "Request started in: {3,-12}\n Assigned in: {4,-12}\t" +
                    "pick up time at: {5,-12}\t  delivered time at: {6,-12}\n", Sender, Receiver, MyDrone,
                    Requested, Assigned, PickedUp, Delivered);
            }
        }
    }
}
