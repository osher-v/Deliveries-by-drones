using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
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
                return string.Format(" {0,-2}\t the customar in hold is:{1,-8}\treciver ID:{2,8}\t  the Source Location is:{3,-8}\t" +
                    " the Destination Location is:{4,-8}\t destiniton is:{5,-8}\t  ", OnTheWayToTheDestination, Sender, Receiver, SourceLocation, 
                    DestinationLocation, TransportDistance);
            }
        }
    }
}
