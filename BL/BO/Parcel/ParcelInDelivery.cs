using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        //חבילה בהעברה??????

        class ParcelInDelivery
        {
            public int Id { get; set; }
            public Priorities Prior { get; set; }
            public CustomerInDelivery Sender { get; set; }
            public CustomerInDelivery Receiver { get; set; }
            public override string ToString()
            {
                return string.Format("id is:{0,-8}\t prioritie is:{1,-8}\t {2}\t {3}\t",Id,Prior,Sender,Receiver);
            }

        }
    }
}
