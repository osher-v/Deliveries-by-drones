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

            //לא ברור איך מדפיסים את הרימה דרך טו סטרינג
            //דרך פוראייצ
            public override string ToString()
            {
                return base.ToString() + string.Format("the location is {0,-6} \t" +
                    "the list of delivery from the customer is {1,-6} \t" +
                    " the list of delivery from the customer is {2,-6}"
                    , location, FromTheCustomer, ToTheCustomer);
            }
        }
    }   
}
