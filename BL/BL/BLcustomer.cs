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
                LocationOfCustomer = dalCustomerLocation, ParcelFromTheCustomer=new List<ParcelAtCustomer>(),  ParcelToTheCustomer =new List<ParcelAtCustomer>() };

            List<DO.Parcel> holdDalParcels = AccessIdal.GetParcelList(i => i.SenderId == idForDisplayObject).ToList();
            // the for run on all the parcel list and put the coract info into it  
            foreach (var item in holdDalParcels)
            {
                CustomerInDelivery customerInDelivery = new CustomerInDelivery { Id = item.TargetId, Name = AccessIdal.GetCustomer(item.TargetId).Name };

                ParcelAtCustomer parcelAtCustomer = new ParcelAtCustomer() { Id = item.Id, Prior = (Priorities)item.Priority,
                    Weight = (WeightCategories)item.Weight, OtherCustomer= customerInDelivery};
                
                if (item.Delivered != null) 
                    parcelAtCustomer.Status = DeliveryStatus.Delivered;
                else if (item.PickedUp != null)
                    parcelAtCustomer.Status = DeliveryStatus.PickedUp;
                else if (item.Assigned != null)
                    parcelAtCustomer.Status = DeliveryStatus.Assigned;
                else
                    parcelAtCustomer.Status = DeliveryStatus.created;

                blCustomer.ParcelFromTheCustomer.Add(parcelAtCustomer);
            }

            List<DO.Parcel> holdDalSentParcels = AccessIdal.GetParcelList(i => i.TargetId == idForDisplayObject).ToList();
            // the for run on all the parcel list and put the coract info into it  
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
                if (item.Delivered != null)
                    parcelAtCustomer.Status = DeliveryStatus.Delivered;
                else if (item.PickedUp != null)
                    parcelAtCustomer.Status = DeliveryStatus.PickedUp;
                else if (item.Assigned != null)
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
            List<DO.Customer> holdDalCustomer = AccessIdal.GetCustomerList().ToList();
            // the for run on all the customer list and put the coract info into it  

            foreach (var item in holdDalCustomer)
            {
                CustomerBL.Add(new CustomerToList
                {
                    Id=item.Id, Name=item.Name ,PhoneNumber=item.PhoneNumber, 
                    NumberOfPackagesSentAndDelivered= AccessIdal.GetParcelList
                    (x => x.Delivered != null && x.SenderId==item.Id).ToList().Count, 

                     NumberOfPackagesSentAndNotYetDelivered= AccessIdal.GetParcelList
                    (x => x.PickedUp != null && x.Delivered == null && x.SenderId == item.Id).ToList().Count,
                        
                      NumberOfPackagesWhoReceived = AccessIdal.GetParcelList
                    (x => x.Delivered != null && x.TargetId == item.Id).ToList().Count,

                       NumberPackagesOnTheWayToTheCustomer = AccessIdal.GetParcelList
                    (x => x.PickedUp != null && x.Delivered == null && x.TargetId == item.Id).ToList().Count,
                });
            }

            return CustomerBL.FindAll(x => predicate == null ? true : predicate(x));
        }

        public void RemoveCustomer(int CustomerId)
        {
            try    // throw if the id is nonExsist
            {
                AccessIdal.RemoveCustomer(CustomerId);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("BaseStation");
            }
        }
    }
}


