using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    partial class BL
    {    
        public void SendingDroneforCharging(int droneId)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);
            if(drone == default)
                throw new NonExistentObjectException();

            if (drone.Statuses != DroneStatuses.free)
                throw new TheDroneCanNotBeSentForCharging("Error, Only a free drone can be sent for charging");

            IEnumerable<DO.BaseStation> dalListStations = AccessIdal.GetBaseStationList(x => x.FreeChargeSlots > 0);
            IEnumerable<BaseStation> BLbaseStations = from item in dalListStations
                                                            select new BaseStation()
                                                            {
                                                                Id = item.Id,
                                                                Name = item.StationName,
                                                                FreeChargeSlots = item.FreeChargeSlots,
                                                                BaseStationLocation = new Location() { longitude = item.Longitude, latitude = item.Latitude }
                                                            };

            if (!BLbaseStations.Any()) //if the List is empty and is no Free charge slots in the all Base station.
                throw new TheDroneCanNotBeSentForCharging("Error, there are no free charging stations");

            Location locationOfTheNearestStation = minDistanceBetweenBaseStationsAndLocation(BLbaseStations, drone.CurrentLocation).Item1;
            double minDistence = minDistanceBetweenBaseStationsAndLocation(BLbaseStations, drone.CurrentLocation).Item2;
            
            if (drone.BatteryStatus - minDistence * Free < 0)
            {
                throw new TheDroneCanNotBeSentForCharging("Error, to the drone does not have enough battery to go to recharge at the nearest available station");
            }

            // if all is good and we update the date 
            drone.BatteryStatus -= minDistence * Free;
            drone.CurrentLocation = locationOfTheNearestStation;
            drone.Statuses = DroneStatuses.inMaintenance;

            //foreach (var item in BLbaseStations)
            //{
            //    if (item.BaseStationLocation.longitude == locationOfTheNearestStation.longitude &&
            //        item.BaseStationLocation.latitude == locationOfTheNearestStation.latitude)
            //    {   
            //        AccessIdal.UpdateMinusChargeSlots(item.Id);
            //        AccessIdal.SendingDroneforChargingAtBaseStation(item.Id, drone.Id);
            //    }
            //}

            //AccessIdal.UpdateMinusChargeSlots(BLbaseStations.First(x => x.BaseStationLocation == locationOfTheNearestStation).Id);
            //AccessIdal.SendingDroneforChargingAtBaseStation(BLbaseStations.First(x => x.BaseStationLocation == locationOfTheNearestStation).Id, drone.Id);

            AccessIdal.UpdateMinusChargeSlots(BLbaseStations.First(item => item.BaseStationLocation.longitude == locationOfTheNearestStation.longitude &&
                    item.BaseStationLocation.latitude == locationOfTheNearestStation.latitude).Id);
            AccessIdal.SendingDroneforChargingAtBaseStation(BLbaseStations.First(item => item.BaseStationLocation.longitude == locationOfTheNearestStation.longitude &&
                    item.BaseStationLocation.latitude == locationOfTheNearestStation.latitude).Id, drone.Id);
        }

        public void ReleaseDroneFromCharging(int droneId)
        {
            DroneToList drone = DronesBL.Find(x => x.Id == droneId);
            if (drone == default)
                throw new NonExistentObjectException();

            if (drone.Statuses != DroneStatuses.inMaintenance)
            {
                throw new OnlyMaintenanceDroneWillBeAbleToBeReleasedFromCharging();
            }
    
            TimeSpan interval = DateTime.Now - AccessIdal.GetBaseCharge(droneId).StartChargeTime;

            double horsnInCahrge = interval.Hours + (((double)interval.Minutes) / 60) + (((double)interval.Seconds) / 3600);

            double batrryCharge = horsnInCahrge * DroneLoadingRate + drone.BatteryStatus;
            if (batrryCharge > 100)
                batrryCharge = 100;
            drone.BatteryStatus = batrryCharge;
            drone.Statuses = DroneStatuses.free;

            AccessIdal.UpdatePluseChargeSlots(AccessIdal.GetBaseCharge(drone.Id).StationId);
            AccessIdal.ReleaseDroneFromChargingAtBaseStation(droneId);
        }

        public int GetBaseCharge(int droneID)
        {
            try
            {
               DO.DroneCharge BaseCharge = AccessIdal.GetBaseCharge(droneID);
                return BaseCharge.StationId;
            }
            catch(DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException();
            }         
        }
    }
}
