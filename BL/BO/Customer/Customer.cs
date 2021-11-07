using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Customer : CustomerParent
        {
            public Location location { get; set; }

            public List<DeliveryToACustomer> FromTheCustomer { get; set; }

            public List<DeliveryToACustomer> ToTheCustomer { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("location:{0,-8}\t ", location)
                   + "the list of delivery from the customer is:" + String.Join("\t", FromTheCustomer)
                   + "the list of delivery from the customer is:" + string.Join("\t,",ToTheCustomer);
            }
        }
    }   
}
