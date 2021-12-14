using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// class of customer.
    /// </summary>
    public class Customer : CustomerParent
    {
        public Location LocationOfCustomer { get; set; }

        public List<ParcelAtCustomer> ParcelFromTheCustomer { get; set; }

        public List<ParcelAtCustomer> ParcelToTheCustomer { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("location:{0,-8}\n ", LocationOfCustomer)
               + "the list of Parcel that are delivery from the customer is:\n " + string.Join("\t", ParcelFromTheCustomer)
               + "the list of Parcel that are delivery to the customer is:\n" + string.Join("*). ", ParcelToTheCustomer);
        }
    }
}

