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
        public class ParcelAtCustomer : ParcelParent
        {

            public DeliveryStatus Status { get; set; }

            public CustomerInDelivery OtherCustomer { get; set; }

            public override string ToString()
            {
                return string.Format("status is: {3,-5} \t the InDelivery is: {4,-6}",Status, OtherCustomer);
            }
        }
    } 
}
