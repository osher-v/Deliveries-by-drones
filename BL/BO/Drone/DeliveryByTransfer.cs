using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class DeliveryInTransfer
        {
            public int Id { get; set; }

            public WeightCategories Weight { get; set; }

            public Priorities Prior { get; set; }

            public bool MyProperty { get; set; }

            public Location SourceLocation { get; set; }

            public Location DestinationLocation { get; set; }

            public double TransportDistance { get; set; }

            public override string ToString()
            {
                return string.Format("     " , Id, Weight, Prior, MyProperty, SourceLocation, 
                    DestinationLocation, TransportDistance);
            }
        }
    }
}
