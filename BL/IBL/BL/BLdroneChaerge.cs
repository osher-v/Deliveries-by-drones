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
            if(drone == default)
                throw new NonExistentObjectException();

            if (drone.Statuses != DroneStatuses.free)
                throw new TheDroneCanNotBeSentForCharging("Error, Only a free drone can be sent for charging"); 

            List<IDAL.DO.BaseStation> dalListStations = AccessIdal.GetBaseStationList(x => x.FreeChargeSlots > 0).ToList();
            List<BaseStation> BLbaseStations = new List<BaseStation>();
            foreach (var item in dalListStations)
            {
                BLbaseStations.Add(new BaseStation{Id = item.Id,Name = item.StationName,FreeChargeSlots = item.FreeChargeSlots,
                    BaseStationLocation = new Location() { longitude = item.Longitude, latitude = item.Latitude } });
            }

            if (!BLbaseStations.Any()) //if the List is empty and is no Free charge slots in the all Base station.
                throw new TheDroneCanNotBeSentForCharging("Error, there are no free charging stations");

            double distence = minDistanceBetweenBaseStationsAndLocation(BLbaseStations, drone.CurrentLocation).Item2;
            if (drone.BatteryStatus - distence * Free < 0)
            {
                throw new TheDroneCanNotBeSentForCharging("Error, to the drone does not have enough battery to go to recharge at the nearest available station");
            }

            drone.BatteryStatus -= distence * Free;
            drone.CurrentLocation = minDistanceBetweenBaseStationsAndLocation(BLbaseStations, drone.CurrentLocation).Item1;
            drone.Statuses = DroneStatuses.inMaintenance;
            AccessIdal.UpdateMinusChargeSlots(BLbaseStations.Find(x => x.BaseStationLocation == drone.CurrentLocation).Id);
            AccessIdal.SendingDroneforChargingAtBaseStation(BLbaseStations.Find(x => x.BaseStationLocation == drone.CurrentLocation).Id, drone.Id);     
        }

        public void ReleaseDroneFromCharging(int droneId, DateTime time)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);
            if (drone == default)
                throw new NonExistentObjectException();

            if (drone.Statuses != DroneStatuses.inMaintenance)
            {
                throw new OnlyMaintenanceDroneWillBeAbleToBeReleasedFromCharging();
            }

            double horsnInCahrge = time.Hour + (time.Minute % 60) + (time.Second % 3600);

            double batrryCharge = horsnInCahrge * DroneLoadingRate + drone.BatteryStatus;
            if (batrryCharge > 100)
                batrryCharge = 100;
            drone.BatteryStatus = batrryCharge;
            drone.Statuses = DroneStatuses.free;

            AccessIdal.UpdatePluseChargeSlots(AccessIdal.GetBaseCharge(drone.Id).StationId);
            AccessIdal.ReleaseDroneFromChargingAtBaseStation(droneId);
        }
    }
}
