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
        public void AddDrone(DroneToList newDrone, int firstChargingStation)
        {
            if ((int)newDrone.MaxWeight < 0 || (int)newDrone.MaxWeight > 2)
                throw new NonExistentEnumException("(0 - 2)");

            try //
            {
                AccessIdal.GetBaseStation(firstChargingStation);
            }
            catch (IDAL.DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("BaseStation");
            }

            if(AccessIdal.GetBaseStation(firstChargingStation).FreeChargeSlots <= 0)
                throw new NoFreeChargingStations();

            IDAL.DO.Drone Drone = new IDAL.DO.Drone() { Id = newDrone.Id, MaxWeight = (IDAL.DO.WeightCategories)newDrone.MaxWeight,
                Model = newDrone.Model};

            try
            {
                AccessIdal.AddDrone(Drone);
            }
            catch (IDAL.DO.AddAnExistingObjectException)
            {
                throw new AddAnExistingObjectException();
            }

            newDrone.BatteryStatus = random.Next(20, 41);
            newDrone.Statuses = DroneStatuses.inMaintenance;

            Location location = new Location()
            {
                 longitude = AccessIdal.GetBaseStation(firstChargingStation).Longitude,
                 latitude = AccessIdal.GetBaseStation(firstChargingStation).Latitude
            };

            newDrone.CurrentLocation = location;

            AccessIdal.UpdateMinusChargeSlots(firstChargingStation);
            AccessIdal.SendingDroneforChargingAtBaseStation(firstChargingStation, newDrone.Id);

            DronesBL.Add(newDrone);
        }

        public void UpdateDroneName(int droneId, string droneName)
        {
            try 
            {
                IDAL.DO.Drone newDrone = AccessIdal.GetDrone(droneId);
                newDrone.Model = droneName;
                AccessIdal.UpdateDrone(newDrone);
            }
            catch (IDAL.DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("drone");
            }

            DronesBL.Find(x => x.Id == droneId).Model = droneName;

        }

        public Drone GetDrone(int idForDisplayObject)
        {
            DroneToList droneToLIist = DronesBL.Find(x => x.Id == idForDisplayObject);
            if (droneToLIist == default)
                throw new NonExistentObjectException("drone");

            Drone printDrone = new Drone() {Id= droneToLIist.Id, BatteryStatus= droneToLIist.BatteryStatus, 
                CurrentLocation= droneToLIist.CurrentLocation, MaxWeight= droneToLIist.MaxWeight,
                Model= droneToLIist.Model,  Statuses= droneToLIist.Statuses, Delivery = new ParcelInTransfer() };

           if(droneToLIist.Statuses == DroneStatuses.busy)
           {    
                IDAL.DO.Parcel holdDalParcel = AccessIdal.GetParcel(droneToLIist.NumberOfLinkedParcel);

                IDAL.DO.Customer holdDalSender = AccessIdal.GetCustomer(holdDalParcel.SenderId);
                IDAL.DO.Customer holdDalReciver= AccessIdal.GetCustomer(holdDalParcel.TargetId);

                Location locationOfSender = new Location() { longitude= holdDalSender.Longitude, latitude=holdDalSender.Latitude };
                Location locationOfReciver = new Location() { longitude = holdDalReciver.Longitude, latitude = holdDalReciver.Latitude };

                // sender
                //printDrone.Delivery.Sender.Id = holdDalParcel.SenderId;
                //printDrone.Delivery.Sender.Name = holdDalSender.Name;
                printDrone.Delivery.Sender = new CustomerInDelivery() { Id = holdDalParcel.SenderId, Name = holdDalSender.Name };
                printDrone.Delivery.SourceLocation = locationOfSender;

                // reciver
                //printDrone.Delivery.Receiver.Id = holdDalReciver.Id;
                //printDrone.Delivery.Receiver.Name = holdDalReciver.Name;
                printDrone.Delivery.Receiver = new CustomerInDelivery() { Id = holdDalReciver.Id, Name = holdDalReciver.Name };
                printDrone.Delivery.DestinationLocation = locationOfReciver;

                printDrone.Delivery.TransportDistance = GetDistance(locationOfSender, locationOfReciver);

                printDrone.Delivery.Id = holdDalParcel.Id;
                printDrone.Delivery.Prior = (Priorities)holdDalParcel.Priority;
                printDrone.Delivery.Weight = (WeightCategories)holdDalParcel.Weight;

                if (holdDalParcel.PickedUp != DateTime.MinValue) // && holdDalParcel.Delivered==DateTime.MinValue
                {
                    printDrone.Delivery.OnTheWayToTheDestination = true;
                }  
                else
                    printDrone.Delivery.OnTheWayToTheDestination = false;

           }

            return printDrone;
        }

        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null)
        {
            return DronesBL.FindAll(x => predicate == null ? true : predicate(x));
        }
    }
}
