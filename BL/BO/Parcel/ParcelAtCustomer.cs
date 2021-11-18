using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        //חבילה אצל לקוח
        /// <summary>
        /// class of parcel at customer.
        /// </summary>
        public class ParcelAtCustomer : ParcelParent
        {

            public DeliveryStatus Status { get; set; }

            public CustomerInDelivery OtherCustomer { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("status is: {0,-5} \t the other customer is: {1,-6}", Status, OtherCustomer);
            }
        }
    } 
}
