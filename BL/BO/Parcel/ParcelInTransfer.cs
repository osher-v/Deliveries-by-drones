using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class ParcelInTransfer
        {
            public int Id { get; set; }

            public bool MyProperty { get; set; } //???????????????????????????????

            public Priorities Prior { get; set; }

            public WeightCategories Weight { get; set; }

            public CustomerInDelivery Sender { get; set; }

            public CustomerInDelivery Receiver { get; set; }

            public Location SourceLocation { get; set; }

            public Location DestinationLocation { get; set; }

            public double TransportDistance { get; set; }

            public override string ToString()
            {
                return string.Format("     " , Id, MyProperty, Prior, Weight, Sender, Receiver, SourceLocation, 
                    DestinationLocation, TransportDistance);
            }
        }
    }
}
