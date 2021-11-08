using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Customer : CustomerParent
        {
            public Location LocationOfCustomer { get; set; }

            public List<ParcelAtCustomer> ParcelFromTheCustomer { get; set; }

            public List<ParcelAtCustomer> ParcelToTheCustomer { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("location:{0,-8}\t ", LocationOfCustomer)
                   + "the list of delivery from the customer is:" + string.Join("\t", ParcelFromTheCustomer)
                   + "the list of delivery from the customer is:" + string.Join("\t,",ParcelToTheCustomer);
            }
        }
    }   
}
