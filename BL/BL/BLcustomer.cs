using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    //public partial class BLcustomer
    partial class BL
    {
        public void AddCustomer(Customer customer)
        {
            DO.Customer newCustomer = new DO.Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
                Longitude = customer.LocationOfCustomer.longitude,
                Latitude = customer.LocationOfCustomer.latitude
            };
            // if Add An Existing Object throw Exception
            try
            {
                AccessIdal.AddCustomer(newCustomer);
            }
            catch (DO.AddAnExistingObjectException)
            {
                throw new AddAnExistingObjectException();
            }
        }

        public void UpdateCustomer(int customerId, string customerName, string phoneNumber)
        {
            // try if Non Existent Object Exception
            try
            {
                DO.Customer customer = AccessIdal.GetCustomer(customerId);
                if (customerName != "")
                    customer.Name = customerName;
                if (phoneNumber != "")
                    customer.PhoneNumber = phoneNumber;
                AccessIdal.UpdateCustomer(customer);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("customer");
            }
        }

        public Customer GetCustomer(int idForDisplayObject)
        {
            DO.Customer printCustomer = new DO.Customer();
            try
            {
                printCustomer = AccessIdal.GetCustomer(idForDisplayObject);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("Customer");
            }

            Location dalCustomerLocation = new Location() { longitude = printCustomer.Longitude, latitude = printCustomer.Latitude };

            Customer blCustomer = new Customer(){Id = printCustomer.Id, Name = printCustomer.Name,PhoneNumber = printCustomer.PhoneNumber,
                LocationOfCustomer = dalCustomerLocation};

            IEnumerable<DO.Parcel> holdDalParcels = AccessIdal.GetParcelList(i => i.SenderId == idForDisplayObject);

            blCustomer.ParcelFromTheCustomer = from item in holdDalParcels
                                               select new ParcelAtCustomer()
                                               {
                                                   Id = item.Id,
                                                   Prior = (Priorities)item.Priority,
                                                   Weight = (WeightCategories)item.Weight,
                                                   OtherCustomer = new CustomerInDelivery
                                                   {
                                                       Id = item.TargetId,
                                                       Name = AccessIdal.GetCustomer(item.TargetId).Name
                                                   },
                                                   Status = item.Delivered != null ? DeliveryStatus.Delivered : item.PickedUp != null ?
                                                   DeliveryStatus.PickedUp : item.Assigned != null ? DeliveryStatus.Assigned : DeliveryStatus.created
                                               };

            IEnumerable<DO.Parcel> holdDalSentParcels = AccessIdal.GetParcelList(i => i.TargetId == idForDisplayObject);

            blCustomer.ParcelToTheCustomer = from item in holdDalSentParcels
                                             select new ParcelAtCustomer()
                                               {
                                                   Id = item.Id,
                                                   Prior = (Priorities)item.Priority,
                                                   Weight = (WeightCategories)item.Weight,
                                                   OtherCustomer = new CustomerInDelivery
                                                   {
                                                       Id = item.SenderId,
                                                       Name = AccessIdal.GetCustomer(item.SenderId).Name
                                                   },
                                                   Status = item.Delivered != null ? DeliveryStatus.Delivered : item.PickedUp != null ?
                                                   DeliveryStatus.PickedUp : item.Assigned != null ? DeliveryStatus.Assigned : DeliveryStatus.created
                                               };

            return blCustomer;
        }

        public IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null)
        {

            IEnumerable<DO.Customer> holdDalCustomer = AccessIdal.GetCustomerList();
            IEnumerable<CustomerToList> CustomerBL = from item in holdDalCustomer
                                                     select new CustomerToList()
                                                     {
                                                         Id = item.Id,
                                                         Name = item.Name,
                                                         PhoneNumber = item.PhoneNumber,
                                                         NumberOfPackagesSentAndDelivered = AccessIdal.GetParcelList
                                                             (x => x.Delivered != null && x.SenderId == item.Id).Count(),
                                                         NumberOfPackagesSentAndNotYetDelivered = AccessIdal.GetParcelList
                                                             (x => x.PickedUp != null && x.Delivered == null && x.SenderId == item.Id).Count(),
                                                         NumberOfPackagesWhoReceived = AccessIdal.GetParcelList
                                                             (x => x.Delivered != null && x.TargetId == item.Id).Count(),
                                                         NumberPackagesOnTheWayToTheCustomer = AccessIdal.GetParcelList
                                                             (x => x.PickedUp != null && x.Delivered == null && x.TargetId == item.Id).Count(),
                                                     };
           return CustomerBL.Where(x => predicate == null ? true : predicate(x));
        }
    }
}


