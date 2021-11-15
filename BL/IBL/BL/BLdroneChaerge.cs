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

         //   AccessIdal.UpdateMinusChargeSlots(BLbaseStations.Find(x => x.BaseStationLocation == drone.CurrentLocation).Id);
        }


    }
}
