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
        public void AddDrone(DroneToList newDrone,int firstChargingStation)
        {
            IDAL.DO.Drone Drone = new IDAL.DO.Drone()
            {
                Id = newDrone.Id,
                MaxWeight= (IDAL.DO.WeightCategories)newDrone.MaxWeight, 
                Model=newDrone.Model
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
    }
}
