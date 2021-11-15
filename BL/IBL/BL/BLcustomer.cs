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
        /// <summary>
        /// The function adds a customer object.
        /// </summary>
        /// <param name="customer">customer</param>
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

        /// <summary>
        /// The function updates a customer object.
        /// </summary>
        /// <param name="customerId">customer ID</param>
        /// <param name="customerName">customer name</param>
        /// <param name="phoneNumber">phone number</param>
        public void UpdateCustomer(int customerId, string customerName, string phoneNumber)
        {
            try
            {
                IDAL.DO.Customer customer = AccessIdal.GetCustomer(customerId);
                if (customerName != "")
                    customer.Name = customerName;
                if (phoneNumber != "")
                    customer.PhoneNumber = phoneNumber;
                AccessIdal.UpdateCustomer(customer);
            }
            catch
            {

            }
        }
    }

}


