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
        /// class of parcel to List.
        /// </summary>
        public class ParcelToList : ParcelParent
        {
            public string CustomerSenderName  { get; set; }

            public string CustomerReceiverName { get; set; }

            public DeliveryStatus Status { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("customer sender name is: {0} \n" +
                    "customer receiver name is:{1,-6}\nthe status: {2,-6}\n~~~~~~~", CustomerSenderName, CustomerReceiverName, Status);
            }
        }
    } 
}
