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
            catch (IDAL.DO.AddAnExistingObjectException)
            {
                throw new AddAnExistingObjectException();
            }
        }

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
                LocationOfCustomer = dalCustomerLocation, ParcelFromTheCustomer=new List<ParcelAtCustomer>(),  ParcelToTheCustomer =new List<ParcelAtCustomer>() };

            List<IDAL.DO.Parcel> holdDalParcels = AccessIdal.GetParcelList(i => i.SenderId == idForDisplayObject).ToList();

            foreach (var item in holdDalParcels)
            {
                CustomerInDelivery customerInDelivery = new CustomerInDelivery { Id = item.TargetId, Name = AccessIdal.GetCustomer(item.TargetId).Name };

                ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer() { Id = item.Id, Prior = (Priorities)item.Priority,
                    Weight = (WeightCategories)item.Weight, OtherCustomer= customerInDelivery};
                
                if (item.Delivered != DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.Delivered;
                else if (item.PickedUp != DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.PickedUp;
                else if (item.Assigned != DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.Assigned;
                else
                    parcelAtCustomer.Status = DeliveryStatus.created;

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
                if (item.Delivered != DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.Delivered;
                else if (item.PickedUp != DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.PickedUp;
                else if (item.Assigned != DateTime.MinValue)
                    parcelAtCustomer.Status = DeliveryStatus.Assigned;
                else
                    parcelAtCustomer.Status = DeliveryStatus.created;

                blCustomer.ParcelToTheCustomer.Add(parcelAtCustomer);
            }
            return blCustomer;
        }

        public IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null)
        {
            List<CustomerToList> CustomerBL = new List<CustomerToList>();
            List<IDAL.DO.Customer> holdDalCustomer = AccessIdal.GetCustomerList().ToList();
            foreach (var item in holdDalCustomer)
            {
                CustomerBL.Add(new CustomerToList
                {  Id=item.Id, Name=item.Name ,PhoneNumber=item.PhoneNumber, 
                    NumberOfPackagesSentAndDelivered= AccessIdal.GetParcelList
                    (x => x.Delivered!=DateTime.MinValue && x.SenderId==item.Id).ToList().Count,

                     NumberOfPackagesSentAndNotYetDelivered= AccessIdal.GetParcelList
                    (x => x.Delivered == DateTime.MinValue && x.SenderId == item.Id).ToList().Count,
                        
                      NumberOfPackagesWhoReceived = AccessIdal.GetParcelList
                    (x => x.Delivered != DateTime.MinValue && x.TargetId == item.Id).ToList().Count,

                       NumberPackagesOnTheWayToTheCustomer = AccessIdal.GetParcelList
                    (x => x.Delivered == DateTime.MinValue && x.TargetId == item.Id).ToList().Count,
                });
            }

            return CustomerBL.FindAll(x => predicate == null ? true : predicate(x));
        }
    }
}


