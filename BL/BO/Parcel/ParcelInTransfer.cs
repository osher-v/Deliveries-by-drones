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
        /// class of parcel in transfer.
        /// </summary>
        public class ParcelInTransfer :ParcelParent
        {
            public bool OnTheWayToTheDestination { get; set; } 

            public CustomerInDelivery Sender { get; set; }

            public CustomerInDelivery Receiver { get; set; }

            public Location SourceLocation { get; set; }

            public Location DestinationLocation { get; set; }

            public double TransportDistance { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("on the way to the destination: {0,-2}\n the customar in hold is:{1,-8} \t the reciver is:{2,8}\n" +
                    "the Source Location is:{3,-8} \t the Destination Location is:{4,-8} \n destiniton is:{5,-8} \n"
                    ,OnTheWayToTheDestination, Sender, Receiver, SourceLocation, DestinationLocation, TransportDistance);
            }
        }
    }
}
