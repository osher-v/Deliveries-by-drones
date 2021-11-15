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

    }
}
