using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    partial class BL
    {
        public int AddParcel(Parcel newParcel)
        {
            try //Check if the customers exist in the system.
            {
                AccessIdal.GetCustomer(newParcel.Sender.Id);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("Erorr: is no Customer Sender id");
            }

            try //Check if the customers exist in the system.
            {
                AccessIdal.GetCustomer(newParcel.Receiver.Id);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("Erorr: is no Customer Receiver id");
            }

            DO.Parcel parcel = new DO.Parcel()
            {
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Receiver.Id,
                Weight = (DO.WeightCategories)newParcel.Weight,
                Priority = (DO.Priorities)newParcel.Prior,
                Requested = DateTime.Now,
                Assigned = null,
                PickedUp = null,
                Delivered = null,
                DroneId = 0
            };

            int IdOfParcel = AccessIdal.AddParcel(parcel);
            return IdOfParcel;
        }

        public void AssignPackageToDdrone(int droneId)
        {
            DroneToList myDrone = DronesBL.Find(x => x.Id == droneId);
            if (myDrone == default)
                throw new NonExistentObjectException();

            if (myDrone.Statuses != DroneStatuses.free)
                throw new DroneCantBeAssigend();

            IEnumerable<DO.Parcel> tempParcels = from item in AccessIdal.GetParcelList(x => x.DroneId == 0 &&
                                                      myDrone.MaxWeight >= (WeightCategories)x.Weight && possibleDistance(x, myDrone))
                                                 orderby item.Priority descending, item.Weight descending,
                                                         GetDistance(GetCustomer(item.SenderId).LocationOfCustomer, myDrone.CurrentLocation)
                                                 select item;

            if (!tempParcels.Any())
                throw new NoSuitablePsrcelWasFoundToBelongToTheDrone();

            DO.Parcel theRightParcel = tempParcels.First();//מביא את האיבר הראשון שהוא המתאים ביותר לאחר כל הסינונים והמיונים

            myDrone.Statuses = DroneStatuses.busy;
            myDrone.NumberOfLinkedParcel = theRightParcel.Id;

            AccessIdal.AssignPackageToDdrone(theRightParcel.Id, droneId);
        }

        /// <summary>
        /// The function calculates whether the drone can reach the parcel.
        /// </summary>
        /// <param name="parcel">list of the most urgent parcels</param>
        /// <param name="myDrone">drone object</param>
        /// <returns></returns>
        private bool possibleDistance(DO.Parcel parcel, DroneToList myDrone)
        {
            double electricityUse = GetDistance(myDrone.CurrentLocation, GetCustomer(parcel.SenderId).LocationOfCustomer) * Free;
            double distanceSenderToDestination = GetDistance(GetCustomer(parcel.SenderId).LocationOfCustomer, GetCustomer(parcel.TargetId).LocationOfCustomer);
            switch ((WeightCategories)parcel.Weight)
            {
                case WeightCategories.light:
                    electricityUse += distanceSenderToDestination * LightWeightCarrier;
                    break;
                case WeightCategories.medium:
                    electricityUse += distanceSenderToDestination * MediumWeightBearing;
                    break;
                case WeightCategories.heavy:
                    electricityUse += distanceSenderToDestination * CarriesHeavyWeight;
                    break;
                default:
                    break;
            }

            if (myDrone.BatteryStatus - electricityUse < 0)//if its lowest than zero no need to continue
                return false;

            IEnumerable<DO.BaseStation> holdDalBaseStation = AccessIdal.GetBaseStationList();
            IEnumerable<BaseStation> baseStationBL = from item in holdDalBaseStation
                                                     select new BaseStation()
                                                     {
                                                         Id = item.Id,
                                                         Name = item.StationName,
                                                         FreeChargeSlots = item.FreeChargeSlots,
                                                         BaseStationLocation = new Location()
                                                         {
                                                             longitude = item.Longitude,
                                                             latitude = item.Latitude
                                                         }
                                                     };
            electricityUse += minDistanceBetweenBaseStationsAndLocation(baseStationBL, GetCustomer(parcel.TargetId).LocationOfCustomer).Item2 * Free;

            if (myDrone.BatteryStatus - electricityUse < 0)
                return false;
            return true;
        }

        public void PickedUpPackageByTheDrone(int droneId)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);
            if (drone == default)
                throw new NonExistentObjectException();

            if (drone.NumberOfLinkedParcel == 0)//Deafult if he do not Assign to parcel.
                throw new UnableToCollectParcel("The drone is not associated with the package");

            DO.Parcel parcelIDal = AccessIdal.GetParcel(drone.NumberOfLinkedParcel);

            if (parcelIDal.PickedUp != null)
                throw new UnableToCollectParcel("The parcel has already been collected");

            Location locationOfSender = GetCustomer(parcelIDal.SenderId).LocationOfCustomer;
            drone.BatteryStatus -= GetDistance(drone.CurrentLocation, locationOfSender) * Free;
            drone.CurrentLocation = locationOfSender;

            AccessIdal.PickedUpPackageByTheDrone(parcelIDal.Id);
        }

        public void DeliveryPackageToTheCustomer(int droneId)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);
            if (drone == default)
                throw new NonExistentObjectException();

            if (drone.NumberOfLinkedParcel == 0)//Deafult if he do not Assign to parcel.
                throw new DeliveryCannotBeMade("The drone is not associated with the package");

            DO.Parcel parcelIDal = AccessIdal.GetParcel(drone.NumberOfLinkedParcel);

            if (parcelIDal.PickedUp != null && parcelIDal.Delivered == null)
            {
                Location locationOfTarget = GetCustomer(parcelIDal.TargetId).LocationOfCustomer;
                switch ((WeightCategories)parcelIDal.Weight)
                {
                    case WeightCategories.light:
                        drone.BatteryStatus -= GetDistance(locationOfTarget, drone.CurrentLocation) * LightWeightCarrier;
                        break;
                    case WeightCategories.medium:
                        drone.BatteryStatus -= GetDistance(locationOfTarget, drone.CurrentLocation) * MediumWeightBearing;
                        break;
                    case WeightCategories.heavy:
                        drone.BatteryStatus -= GetDistance(locationOfTarget, drone.CurrentLocation) * CarriesHeavyWeight;
                        break;
                    default:
                        break;
                }
                drone.CurrentLocation = locationOfTarget;
                drone.Statuses = DroneStatuses.free;
                drone.NumberOfLinkedParcel = 0; //importent.
                AccessIdal.DeliveryPackageToTheCustomer(parcelIDal.Id);
            }
            else if (parcelIDal.PickedUp == null)
                throw new DeliveryCannotBeMade("Error: parcel not yet collected");
            else if (parcelIDal.Delivered != null)
                throw new DeliveryCannotBeMade("Error: The parcel has already been delivered");
        }

        public Parcel GetParcel(int idForDisplayObject)
        {
            DO.Parcel printParcel = new DO.Parcel();
            try
            {
                printParcel = AccessIdal.GetParcel(idForDisplayObject);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("Parcel");
            }

            DroneToList droneToLIist = DronesBL.Find(x => x.Id == printParcel.DroneId);
            CustomerInDelivery senderInDelivery = new CustomerInDelivery() { Id = printParcel.SenderId, Name = AccessIdal.GetCustomer(printParcel.SenderId).Name };
            CustomerInDelivery reciverInDelivery = new CustomerInDelivery() { Id = printParcel.TargetId, Name = AccessIdal.GetCustomer(printParcel.TargetId).Name };
            Parcel parcel = new Parcel()
            {
                Id = printParcel.Id,
                Weight = (WeightCategories)printParcel.Weight,
                Prior = (Priorities)printParcel.Priority,
                Sender = senderInDelivery,
                Receiver = reciverInDelivery,
                Requested = printParcel.Requested,
                Assigned = printParcel.Assigned,
                PickedUp = printParcel.PickedUp,
                Delivered = printParcel.Delivered
            };

            if (parcel.Assigned != null && parcel.Delivered == null)//ברגע שחבילה סופקה אין רחפן בקשר
            {
                DroneInThePackage droneInThePackage = new DroneInThePackage()
                {
                    Id = droneToLIist.Id,
                    BatteryStatus = droneToLIist.BatteryStatus,
                    CurrentLocation = droneToLIist.CurrentLocation
                };
                parcel.MyDrone = droneInThePackage;
            }

            return parcel;
        }

        public IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate = null)
        {
            IEnumerable<ParcelToList> parcels = from item in AccessIdal.GetParcelList()
                                                select new ParcelToList()
                                                {
                                                    Id = item.Id,
                                                    Weight = (WeightCategories)item.Weight,
                                                    Prior = (Priorities)item.Priority,
                                                    CustomerSenderName = AccessIdal.GetCustomer(item.SenderId).Name,
                                                    CustomerReceiverName = AccessIdal.GetCustomer(item.TargetId).Name,
                                                    Status = item.Delivered != null ? DeliveryStatus.Delivered : item.PickedUp != null ?
                                                    DeliveryStatus.PickedUp : item.Assigned != null ? DeliveryStatus.Assigned : DeliveryStatus.created
                                                };

            return parcels.Where(x => predicate == null ? true : predicate(x));


            //List<ParcelToList> parcels = new List<ParcelToList>();
            //List<DO.Parcel> holdDalParcels = AccessIdal.GetParcelList().ToList();

            //foreach (var item in holdDalParcels)
            //{
            //    DeliveryStatus currentStatus;
            //    if (item.Delivered != null)
            //        currentStatus = DeliveryStatus.Delivered;
            //    else if (item.PickedUp != null)
            //        currentStatus = DeliveryStatus.PickedUp;
            //    else if (item.Assigned != null)
            //        currentStatus = DeliveryStatus.Assigned;
            //    else
            //        currentStatus = DeliveryStatus.created;

            //    parcels.Add(new ParcelToList
            //    {
            //        Id = item.Id,
            //        Weight = (WeightCategories)item.Weight,
            //        Prior = (Priorities)item.Priority,
            //        CustomerSenderName = AccessIdal.GetCustomer(item.SenderId).Name,
            //        CustomerReceiverName = AccessIdal.GetCustomer(item.TargetId).Name,
            //        Status = currentStatus
            //    });
            //}

            //return parcels.Where(x => predicate == null ? true : predicate(x));
        }

        public void RemoveParcel(int ParcelId)//להוסיף חריגה אם החבילה כבר משויכת
        {
            try    // throw if the id is nonExsist
            {
                AccessIdal.RemoveParcel(ParcelId);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("BaseStation");
            }
        }
    }
}