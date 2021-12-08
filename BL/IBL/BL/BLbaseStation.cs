using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    //public partial class BLbaseStation
    public partial class BL
    {
        public void AddStation(BaseStation newbaseStation)
        {

            IDAL.DO.BaseStation newStation = new IDAL.DO.BaseStation()
            {
                Id = newbaseStation.Id,
                StationName = newbaseStation.Name,
                FreeChargeSlots = newbaseStation.FreeChargeSlots,
                Longitude = newbaseStation.BaseStationLocation.longitude,
                Latitude = newbaseStation.BaseStationLocation.latitude
            };
            try    // throw if the id is exsist
            {
                AccessIdal.AddStation(newStation);
            }
            catch (IDAL.DO.AddAnExistingObjectException)
            {
                throw new AddAnExistingObjectException();
            }
        }

        public void UpdateBaseStaison(int baseStationId, string baseName, string chargeslots)
        {
            IDAL.DO.BaseStation newbase = new IDAL.DO.BaseStation();
            try
            {
                newbase = AccessIdal.GetBaseStation(baseStationId);
                if (baseName != "") //if it is not empty.
                {
                    newbase.StationName = baseName;
                }
            }
            catch (IDAL.DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("BaseStation");
            }

            if (chargeslots != "") ////if it is not empty.
            {
                int totalQuantityChargeSlots;
                int.TryParse(chargeslots, out  totalQuantityChargeSlots);
                int numOfBuzeChargeslots = AccessIdal.GetBaseChargeList(x => x.StationId == baseStationId).ToList().Count;
                //chaeck if More Drone In Charging Than The Proposed Charging Stations
                if (totalQuantityChargeSlots - numOfBuzeChargeslots < 0)
                   throw new MoreDroneInChargingThanTheProposedChargingStations();
                newbase.FreeChargeSlots = totalQuantityChargeSlots - numOfBuzeChargeslots;
            }
            AccessIdal.UpdateBaseStation(newbase);
        }

        public BaseStation GetBaseStation(int idForDisplayObject)
        {
            IDAL.DO.BaseStation printBase = new IDAL.DO.BaseStation();
            // check if Non Existent BaseStation 
            try
            {
                printBase = AccessIdal.GetBaseStation(idForDisplayObject);
            }
            catch (IDAL.DO.NonExistentObjectException)
            {
                throw new NonExistentObjectException("BaseStation");
            }

            Location dalBaseLocation = new Location() { longitude = printBase.Longitude, latitude=printBase.Latitude };
            BaseStation blBase = new BaseStation() { Id = printBase.Id, Name=printBase.StationName, BaseStationLocation = dalBaseLocation,
                FreeChargeSlots=printBase.FreeChargeSlots, DroneInChargsList=new List<DroneInCharg>()};
           
            List<IDAL.DO.DroneCharge> droneInCharge = AccessIdal.GetBaseChargeList(i => i.StationId == idForDisplayObject).ToList();
            foreach (var item in droneInCharge)
            {
                blBase.DroneInChargsList.Add(new DroneInCharg { Id = item.DroneId,
                    BatteryStatus = DronesBL.Find(x => x.Id == item.DroneId).BatteryStatus });// put only the ones with the data we want 
            }
            return blBase;
        }

        public IEnumerable<BaseStationsToList> GetBaseStationList(Predicate<BaseStationsToList> predicate = null)
        {
            List<BaseStationsToList> baseStationBL = new List<BaseStationsToList>();
            List<IDAL.DO.BaseStation> holdDalBaseStation = AccessIdal.GetBaseStationList().ToList();
            foreach (var item in holdDalBaseStation)
            {
                baseStationBL.Add(new BaseStationsToList { Id = item.Id, Name = item.StationName, FreeChargeSlots = item.FreeChargeSlots,
                     BusyChargeSlots = AccessIdal.GetBaseChargeList(x => x.StationId == item.Id).ToList().Count });
            }

            return baseStationBL.FindAll(x => predicate == null ? true : predicate(x));
        }
    }
}
