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
        /// <summary>
        /// the function get ID for a drone that will be updated (name only)
        /// </summary>
        /// <param name="droneId">the reqsted drone</param>
        /// <param name="droneName">the new name </param>
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
        public Drone GetDrone(int idForDisplayObject)
        {

            //IDAL.DO.BaseStation printBase = AccessIdal.GetBaseStation(idForDisplayObject);
            //Location dalBaseLocation = new Location() { longitude = printBase.Longitude, latitude = printBase.Latitude };
            //BaseStation blBase = new BaseStation()
            //{
            //    Id = printBase.Id,
            //    Name = printBase.StationName,
            //    BaseStationLocation = dalBaseLocation,
            //    FreeChargeSlots = printBase.FreeChargeSlots
            //};

            //List<IDAL.DO.DroneCharge> droneInCharge = AccessIdal.GetBaseChargeList(i => i.StationId == idForDisplayObject).ToList();
            //foreach (var item in droneInCharge)
            //{
            //    blBase.DroneInChargsList.Add(new DroneInCharg { Id = item.DroneId, BatteryStatus = DronesBL.Find(x => x.Id == item.DroneId).BatteryStatus });
            //}
            Drone printDrone = new Drone() {Id= idForDisplayObject, BatteryStatus= DronesBL.Find(x => x.Id == idForDisplayObject).BatteryStatus, 
                CurrentLocation= DronesBL.Find(x => x.Id == idForDisplayObject).CurrentLocation,
                MaxWeight= DronesBL.Find(x => x.Id == idForDisplayObject).MaxWeight,
                Model= DronesBL.Find(x => x.Id == idForDisplayObject).Model,
                Statuses= DronesBL.Find(x => x.Id == idForDisplayObject).Statuses };
           if(DronesBL.Find(x => x.Id == idForDisplayObject).Statuses!= DroneStatuses.busy)
            {
                List<IDAL.DO.Parcel> holdDalParcels = AccessIdal.GetParcelList(i => i.DroneId != 0).ToList();
                printDrone.Delivery = holdDalParcels.Find( x=>x.DroneId== idForDisplayObject);//מההההה אתהההה רוצההה ממניייי מההההההההה
            }
            return printDrone;
        }

    }
}
