using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        //משלוח אצל לקוח
        class DeliveryToACustomer
        {
            public int Id { get; set; }

            public WeightCategories Weight { get; set; }

            public Priorities Prior { get; set; }

            public DeliveryStatus Status { get; set; }

            public int MyProperty { get; set; }

            public override string ToString()
            {
                return string.Format("   ", Id, Weight, Prior, Status, MyProperty);
            }
        }
    } 
}
