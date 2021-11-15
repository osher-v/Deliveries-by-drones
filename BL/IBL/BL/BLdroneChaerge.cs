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
        /// <summary>
        /// the fanction send drone to charge and update accordanly 
        /// </summary>
        /// <param name="droneId"></param>
        public void SendingDroneforCharging(int droneId)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);

            if (drone.Statuses != DroneStatuses.free)
                throw new Exception(); //צריך לראות מהי החריגה
            List<IDAL.DO.BaseStation> dalListStations = AccessIdal.GetBaseStationList(x => x.FreeChargeSlots > 0).ToList();
            List<BaseStation> BLbaseStations = new List<BaseStation>();
            foreach (var item in dalListStations)
            {
                Location itemLocation = new Location { longitude = item.Longitude, latitude = item.Latitude };
                BLbaseStations.Add(new BaseStation
                {
                    Id = item.Id,
                    Name = item.StationName,
                    FreeChargeSlots = item.FreeChargeSlots,
                    BaseStationLocation = itemLocation
                });
            }
            double distence = minDistanceBetweenBaseStationsAndLocation(BLbaseStations, drone.CurrentLocation).Item2;
            if (drone.BatteryStatus - distence * Free < 0)
            {
                throw new Exception();
            }

            drone.BatteryStatus -= distence * Free;
            drone.CurrentLocation = minDistanceBetweenBaseStationsAndLocation(BLbaseStations, drone.CurrentLocation).Item1;
            drone.Statuses = DroneStatuses.inMaintenance;
            AccessIdal.SendingDroneforChargingAtBaseStation(BLbaseStations.Find(x => x.BaseStationLocation == drone.CurrentLocation).Id, drone.Id);
            // AccessIdal.UpdateMinusChargeSlots(BLbaseStations.Find(x => x.BaseStationLocation == drone.CurrentLocation).Id);
        }


        public void ReleaseDroneFromCharging(int droneId, DateTime time)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);
            if (drone.Statuses != DroneStatuses.inMaintenance)
            {
                throw new Exception();
            }

            double horsnInCahrge = time.Hour + (time.Minute/60) + (time.Second / 3600);

            double batrryCharge = horsnInCahrge * DroneLoadingRate+drone.BatteryStatus;
            if (batrryCharge > 100)
                batrryCharge = 100;
            drone.BatteryStatus = batrryCharge;
            drone.Statuses = DroneStatuses.free;

            AccessIdal.ReleaseDroneFromChargingAtBaseStation(droneId);

        }


    }
}
