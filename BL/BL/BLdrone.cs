using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;

namespace BL
{
    //public partial class BLdrone
    partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(DroneToList newDrone, int firstChargingStation)
        {
            if ((int)newDrone.MaxWeight < 0 || (int)newDrone.MaxWeight > 2)
                throw new NonExistentEnumException("(0 - 2)");

            try //if the id is Non Existent throw Exception
            {
                AccessIdal.GetBaseStation(firstChargingStation);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("BaseStation");
            }

            if(AccessIdal.GetBaseStation(firstChargingStation).FreeChargeSlots <= 0)
                throw new NoFreeChargingStations();

            DO.Drone Drone = new DO.Drone() { Id = newDrone.Id, MaxWeight = (DO.WeightCategories)newDrone.MaxWeight,
                Model = newDrone.Model};

            try  // if Add An Existing Object throw Exception
            {
                AccessIdal.AddDrone(Drone);
            }
            catch (DO.AddAnExistingObjectException)
            {
                throw new AddAnExistingObjectException();
            }

            newDrone.BatteryStatus = random.Next(20, 41);
            newDrone.Statuses = DroneStatuses.inMaintenance;
            //set a location bo
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneName(int droneId, string droneName)
        {
            //if the its Non Existent throw Exception
            try 
            {
                DO.Drone newDrone = AccessIdal.GetDrone(droneId);
                newDrone.Model = droneName;
                AccessIdal.UpdateDrone(newDrone);
            }
            catch (DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("drone");
            }

            DronesBL.Find(x => x.Id == droneId).Model = droneName;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
                DO.Parcel holdDalParcel = AccessIdal.GetParcel(droneToLIist.NumberOfLinkedParcel);

                DO.Customer holdDalSender = AccessIdal.GetCustomer(holdDalParcel.SenderId);
                DO.Customer holdDalReciver= AccessIdal.GetCustomer(holdDalParcel.TargetId);

                Location locationOfSender = new Location() { longitude= holdDalSender.Longitude, latitude=holdDalSender.Latitude };
                Location locationOfReciver = new Location() { longitude = holdDalReciver.Longitude, latitude = holdDalReciver.Latitude };

                // sender      
                printDrone.Delivery.Sender = new CustomerInDelivery() { Id = holdDalParcel.SenderId, Name = holdDalSender.Name };
                printDrone.Delivery.SourceLocation = locationOfSender;

                // reciver     
                printDrone.Delivery.Receiver = new CustomerInDelivery() { Id = holdDalReciver.Id, Name = holdDalReciver.Name };
                printDrone.Delivery.DestinationLocation = locationOfReciver;

                if (holdDalParcel.PickedUp == null)//Assigned
                {
                    printDrone.Delivery.TransportDistance = GetDistance(printDrone.CurrentLocation, locationOfSender);
                }
                else//PickedUp
                {
                    printDrone.Delivery.TransportDistance = GetDistance(locationOfSender, locationOfReciver);
                }

                printDrone.Delivery.Id = holdDalParcel.Id;
                printDrone.Delivery.Prior = (Priorities)holdDalParcel.Priority;
                printDrone.Delivery.Weight = (WeightCategories)holdDalParcel.Weight;

                if (holdDalParcel.PickedUp != null) 
                {
                    printDrone.Delivery.OnTheWayToTheDestination = true;
                }  
                else
                    printDrone.Delivery.OnTheWayToTheDestination = false;

           }

            return printDrone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null)
        {
            return DronesBL.FindAll(x => predicate == null ? true : predicate(x));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void sim(int droneID, Action ReportProgressInSimultor, Func<bool> IsTimeRun)
        {
            new Simulator(this, droneID, ReportProgressInSimultor, IsTimeRun);
        }
    }
}
