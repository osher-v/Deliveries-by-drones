using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
    {
        public void AddParcel(Parcel newParcel)
        {
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel()
            {
                Id = newParcel.Id,
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Receiver.Id,
                Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                Priority = (IDAL.DO.Priorities)newParcel.Prior,
                Assigned = DateTime.Now,
                DroneId = 0 // לשאול את דן   
            };

            try
            {
                AccessIdal.AddParcel(parcel);
            }
            catch { }
        }

        public void AssignPackageToDdrone(int droneId)
        {
            DroneToList myDrone = DronesBL.Find(x => x.Id == droneId);

            if (myDrone.Statuses != DroneStatuses.free)
                throw new Exception();

            try
            {
                List<IDAL.DO.Parcel> highestPriority = highestPriorityList(myDrone);

                List<IDAL.DO.Parcel> highestWeight = highestWeightList(highestPriority, myDrone);

                IDAL.DO.Parcel theRightPackage = minDistance(highestWeight, myDrone.CurrentLocation);

                myDrone.Statuses = DroneStatuses.busy;
                myDrone.NumberOfLinkedParcel = theRightPackage.Id;

                AccessIdal.AssignPackageToDdrone(theRightPackage.Id, droneId);
            }
            catch
            {

            }
        }

        //********************* Auxiliary functions for the AssignPackageToDdrone function *****************************
        private List<IDAL.DO.Parcel> highestPriorityList(DroneToList myDrone)
        {
            List<IDAL.DO.Parcel> parcels = AccessIdal.GetParcelList(x => x.DroneId == 0).ToList();

            List<IDAL.DO.Parcel> parcelsWithHighestPriority = new List<IDAL.DO.Parcel>();
            List<IDAL.DO.Parcel> parcelsWithMediumPriority = new List<IDAL.DO.Parcel>();
            List<IDAL.DO.Parcel> parcelsWithRegulerPriority = new List<IDAL.DO.Parcel>();

            foreach (var item in parcels)
            {
                if (myDrone.MaxWeight >= (WeightCategories)item.Weight && possibleDistance(item, myDrone))
                {
                    switch ((Priorities)item.Priority)
                    {
                        case Priorities.regular:
                            parcelsWithRegulerPriority.Add(item);
                            break;

                        case Priorities.fast:
                            parcelsWithMediumPriority.Add(item);
                            break;

                        case Priorities.urgent:
                            parcelsWithHighestPriority.Add(item);
                            break;

                        default:
                            break;
                    }
                }
            }

            //if (parcelsWithHighestPriority.Any())
            //    return parcelsWithHighestPriority;
            //else if (parcelsWithMediumPriority.Any())
            //    return parcelsWithMediumPriority;
            //else
            //    return parcelsWithRegulerPriority;

            return (parcelsWithHighestPriority.Any() ? parcelsWithHighestPriority : (parcelsWithMediumPriority.Any() ?
                parcelsWithMediumPriority : parcelsWithRegulerPriority));
        }

        private List<IDAL.DO.Parcel> highestWeightList(List<IDAL.DO.Parcel> parcels, DroneToList myDrone)
        {
            List<IDAL.DO.Parcel> parcelsHeavy = new List<IDAL.DO.Parcel>();
            List<IDAL.DO.Parcel> parcelsMedium = new List<IDAL.DO.Parcel>();
            List<IDAL.DO.Parcel> parcelsLight = new List<IDAL.DO.Parcel>();

            foreach (var item in parcels)
            {
                switch ((WeightCategories)item.Weight)
                {
                    case WeightCategories.light:
                        parcelsLight.Add(item);
                        break;
                    case WeightCategories.medium:
                        parcelsMedium.Add(item);
                        break;
                    case WeightCategories.heavy:
                        parcelsHeavy.Add(item);
                        break;
                    default:
                        break;
                }
            }

            //if (parcelsWithHighestPriority.Any())
            //    return parcelsWithHighestPriority;
            //else if (parcelsWithMediumPriority.Any())
            //    return parcelsWithMediumPriority;
            //else
            //    return parcelsWithRegulerPriority;

            return (parcelsHeavy.Any() ? parcelsHeavy : (parcelsMedium.Any() ?
                parcelsMedium : parcelsLight));
        }

        private bool possibleDistance(IDAL.DO.Parcel parcel, DroneToList myDrone)
        {

            //Customer senderCustomer = GetCustomer(parcel.SenderId);
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

            //לזכור בעתיד לשנות את הפונקציה שמחזירה את המרחק הקצר ביותר לתחנה
            List<BaseStation> baseStationBL = new List<BaseStation>();
            List<IDAL.DO.BaseStation> holdDalBaseStation = AccessIdal.GetBaseStationList().ToList();
            foreach (var item in holdDalBaseStation)
            {
                Location LocationOfItem = new Location() { longitude = item.Longitude, latitude = item.Latitude };
                baseStationBL.Add(new BaseStation
                {
                    Id = item.Id,
                    Name = item.StationName,
                    FreeChargeSlots = item.FreeChargeSlots,
                    BaseStationLocation = LocationOfItem
                });
            }

            electricityUse += minDistanceBetweenBaseStationsAndLocation(baseStationBL, GetCustomer(parcel.TargetId).LocationOfCustomer).Item2 * Free;

            if (myDrone.BatteryStatus - electricityUse < 0)
                return false;
            return true;
        }

        private IDAL.DO.Parcel minDistance(List<IDAL.DO.Parcel> parcels, Location location)
        {
            List<double> listOfDistance = new List<double>();
            foreach (var obj in parcels)
            {
                Location locationOfSender = GetCustomer(obj.SenderId).LocationOfCustomer;
                listOfDistance.Add(GetDistance(location, locationOfSender));
            }
            return parcels[listOfDistance.FindIndex(x => x == listOfDistance.Min())];
        }
        //**************************************************************************************************************
 
        public void PickedUpPackageByTheDrone(int droneId)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);

            if (drone.NumberOfLinkedParcel == 0)//Deafult if he do not Assign to parcel.
                throw new Exception();

            IDAL.DO.Parcel parcelIDal = AccessIdal.GetParcel(drone.NumberOfLinkedParcel);

            if (parcelIDal.PickedUp != DateTime.MinValue)
                throw new Exception();

            Location locationOfSender = GetCustomer(parcelIDal.SenderId).LocationOfCustomer;
            drone.BatteryStatus -= GetDistance(drone.CurrentLocation, locationOfSender) * Free;
            drone.CurrentLocation = locationOfSender;

            AccessIdal.PickedUpPackageByTheDrone(parcelIDal.Id);
        }
    
        public void DeliveryPackageToTheCustomer(int droneId)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);

            if (drone.NumberOfLinkedParcel == 0)//Deafult if he do not Assign to parcel.
                throw new Exception();

            IDAL.DO.Parcel parcelIDal = AccessIdal.GetParcel(drone.NumberOfLinkedParcel);

            if (parcelIDal.PickedUp != DateTime.MinValue && parcelIDal.Delivered == DateTime.MinValue)
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
            else 
                throw new Exception();
        }

        public Parcel GetParcel(int idForDisplayObject)
        {
            IDAL.DO.Parcel printParcel = AccessIdal.GetParcel(idForDisplayObject);
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
            if (parcel.Assigned != DateTime.MinValue)
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
            List<ParcelToList> parcels = new List<ParcelToList>();
            List<IDAL.DO.Parcel> holdDalParcels = AccessIdal.GetParcelList().ToList();

            foreach (var item in holdDalParcels)
            {
                DeliveryStatus st;
                if (item.Delivered != DateTime.MinValue)
                    st = DeliveryStatus.Delivered;
                else if (item.PickedUp != DateTime.MinValue)
                    st = DeliveryStatus.PickedUp;
                else if (item.Assigned != DateTime.MinValue)
                    st = DeliveryStatus.Assigned;
                else
                    st = DeliveryStatus.created;

                parcels.Add(new ParcelToList {Id = item.Id, Weight = (WeightCategories)item.Weight,
                    Prior = (Priorities)item.Priority, CustomerSenderName = AccessIdal.GetCustomer(item.SenderId).Name,
                    CustomerReceiverName = AccessIdal.GetCustomer(item.TargetId).Name, Status = st
                });
            }

            return parcels.FindAll(x => predicate == null ? true : predicate(x));
        }
    }
}