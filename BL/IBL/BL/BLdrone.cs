using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    //public partial class BLdrone
    public partial class BL
    {
        /// <summary>
        /// add a new drine ti both places suce as idal list and bl list 
        /// </summary>
        /// <param name="newDrone">the name of the new drone</param>
        /// <param name="firstChargingStation">the first to put the drone in for charg</param>
        public void AddDrone(DroneToList newDrone, int firstChargingStation)
        {
            IDAL.DO.Drone Drone = new IDAL.DO.Drone()
            {
                Id = newDrone.Id,
                MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight,
                Model = newDrone.Model
            };
            try
            {
                AccessIdal.AddDrone(Drone);
            }
            catch { }
            try
            {
                newDrone.BatteryStatus = random.Next(20, 41);
                newDrone.Statuses = DroneStatuses.inMaintenance;
                newDrone.CurrentLocation.longitude = AccessIdal.GetBaseStation(firstChargingStation).Longitude;
                newDrone.CurrentLocation.latitude = AccessIdal.GetBaseStation(firstChargingStation).Latitude;
                DronesBL.Add(newDrone);
                AccessIdal.SendingDroneforChargingAtBaseStation(firstChargingStation, newDrone.Id);// לשאול את דן 
            }
            catch { }
        }

        public void UpdateDroneName(int droneId, string droneName)
        {
            try
            {
                DronesBL.Find(x => x.Id == droneId).Model = droneName;
            }
            catch { }
            try 
            {
                IDAL.DO.Drone newDrone = AccessIdal.GetDrone(droneId);
                newDrone.Model = droneName;
                AccessIdal.UpdateDrone(newDrone);
            }
            catch { }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idForDisplayObject"></param>
        /// <returns></returns>
        public Drone GetDrone(int idForDisplayObject)
        {
            DroneToList droneToLIist = DronesBL.Find(x => x.Id == idForDisplayObject);

            Drone printDrone = new Drone() {Id= droneToLIist.Id, BatteryStatus= droneToLIist.BatteryStatus, 
                CurrentLocation= droneToLIist.CurrentLocation, MaxWeight= droneToLIist.MaxWeight,
                Model= droneToLIist.Model,  Statuses= droneToLIist.Statuses };

           if(droneToLIist.Statuses == DroneStatuses.busy)
            {    
                IDAL.DO.Parcel holdDalParcel = AccessIdal.GetParcel(droneToLIist.NumberOfLinkedParcel);
                IDAL.DO.Customer holdDalSender = AccessIdal.GetCustomer(holdDalParcel.SenderId);
                IDAL.DO.Customer holdDalReciver= AccessIdal.GetCustomer(holdDalParcel.TargetId);
                Location locationOfSender = new Location() { longitude= holdDalSender.Longitude, latitude=holdDalSender.Latitude };
                Location locationOfReciver = new Location() { longitude = holdDalReciver.Longitude, latitude = holdDalReciver.Latitude };

                // sender
                printDrone.Delivery.Sender.Id = holdDalParcel.SenderId;
                printDrone.Delivery.Sender.Name = holdDalSender.Name;
                printDrone.Delivery.SourceLocation = locationOfSender;
                // reciver
                printDrone.Delivery.Receiver.Id = holdDalReciver.Id;
                printDrone.Delivery.Receiver.Name = holdDalReciver.Name;
                printDrone.Delivery.DestinationLocation = locationOfReciver;

                printDrone.Delivery.TransportDistance = GetDistance(locationOfSender, locationOfReciver);

                printDrone.Delivery.Id = holdDalParcel.Id;
                printDrone.Delivery.Prior = (Priorities)holdDalParcel.Priority;
                printDrone.Delivery.Weight = (WeightCategories)holdDalParcel.Weight;
                if (holdDalParcel.PickedUp != DateTime.MinValue && holdDalParcel.Delivered==DateTime.MinValue)
                {
                    printDrone.Delivery.OnTheWayToTheDestination = true;
                }  
              else printDrone.Delivery.OnTheWayToTheDestination = false;
            }
            return printDrone;
        }

        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null)
        {
            return DronesBL.FindAll(x => predicate == null ? true : predicate(x));
        }
    }
}
