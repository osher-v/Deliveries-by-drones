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
        public Customer GetCustomer(int idForDisplayObject)
        {
            IDAL.DO.Customer printCustomer = AccessIdal.GetCustomer(idForDisplayObject);
            Location dalCustomerLocation = new Location() { longitude = printCustomer.Longitude, latitude = printCustomer.Latitude };
            Customer blCustomer = new Customer(){Id = printCustomer.Id, Name = printCustomer.Name,PhoneNumber = printCustomer.PhoneNumber,
                LocationOfCustomer = dalCustomerLocation };
            //List<IDAL.DO.Parcel> parcels = new List<IDAL.DO.Parcel>();
            List<IDAL.DO.Parcel> holdDalParcels = AccessIdal.GetParcelList(i => i.SenderId == idForDisplayObject).ToList();

            foreach (var item in holdDalParcels)
            {
                CustomerInDelivery customerInDelivery = new CustomerInDelivery { Id = item.TargetId, Name = AccessIdal.GetCustomer(item.TargetId).Name };
                ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer() { Id = item.Id, Prior = (Priorities)item.Priority,
                    Weight = (WeightCategories)item.Weight, OtherCustomer= customerInDelivery};
                if (item.Requested != DateTime.MinValue && item.Assigned == DateTime.MinValue && item.PickedUp == DateTime.MinValue && item.Delivered == DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.created;
                else if (item.Requested != DateTime.MinValue && item.Assigned != DateTime.MinValue && item.PickedUp == DateTime.MinValue && item.Delivered == DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.Assigned;
                else if (item.Requested != DateTime.MinValue && item.Assigned != DateTime.MinValue && item.PickedUp != DateTime.MinValue && item.Delivered == DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.PickedUp;
                else if (item.Requested != DateTime.MinValue && item.Assigned != DateTime.MinValue && item.PickedUp != DateTime.MinValue && item.Delivered != DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.Delivered;
                else throw new Exception();
                blCustomer.ParcelFromTheCustomer.Add(parcelAtCustomer);
            }

            List<IDAL.DO.Parcel> holdDalSentParcels = AccessIdal.GetParcelList(i => i.TargetId == idForDisplayObject).ToList();

            foreach (var item in holdDalSentParcels)
            {
                CustomerInDelivery customerInDelivery = new CustomerInDelivery { Id = item.SenderId, Name = AccessIdal.GetCustomer(item.SenderId).Name };
                ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer()
                {
                    Id = item.Id,
                    Prior = (Priorities)item.Priority,
                    Weight = (WeightCategories)item.Weight,
                    OtherCustomer = customerInDelivery
                };
                if (item.Requested != DateTime.MinValue && item.Assigned == DateTime.MinValue && item.PickedUp == DateTime.MinValue && item.Delivered == DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.created;
                else if (item.Requested != DateTime.MinValue && item.Assigned != DateTime.MinValue && item.PickedUp == DateTime.MinValue && item.Delivered == DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.Assigned;
                else if (item.Requested != DateTime.MinValue && item.Assigned != DateTime.MinValue && item.PickedUp != DateTime.MinValue && item.Delivered == DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.PickedUp;
                else if (item.Requested != DateTime.MinValue && item.Assigned != DateTime.MinValue && item.PickedUp != DateTime.MinValue && item.Delivered != DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.Delivered;
                else throw new Exception();
                blCustomer.ParcelToTheCustomer.Add(parcelAtCustomer);
            }
            return blCustomer;
        }

    }

}


