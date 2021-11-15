using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    //public partial class BLcustomer
    public partial class BL
    {
        public void AddCustomer(Customer customer)
        {
            IDAL.DO.Customer newCustomer = new IDAL.DO.Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Longitude = customer.LocationOfCustomer.longitude,
                Latitude = customer.LocationOfCustomer.latitude
            };
            try
            {
                AccessIdal.AddCustomer(newCustomer);
            }
            catch { }
        }

        public void UpdateCustomer(int customerId, string customerName, string phoneNumber)
        {



        }
    }

}


