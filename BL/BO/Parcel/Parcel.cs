using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
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
                return base.ToString() + string.Format("sender is: {0,-6} \t receiver is: {1,-6} \t the drone is: {2,-5}" +
                    "Request started in: { 3,-12}\t Assigned: { 4,-12}\t" +
                    "pick up time at: {5,-12}\t  arivel time at: {6,-12}\t", Sender, Receiver, MyDrone,
                    Requested, Assigned, PickedUp, Delivered);
            }
        }
    }
}
