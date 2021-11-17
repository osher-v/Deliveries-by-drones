using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
using IDal;

namespace DalObject
{
    public partial class DalObject 
    {
        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
        public void SendingDroneforChargingAtBaseStation(int baseStationId, int droneId)
        {
            DataSource.DroneChargeList.Add(new DroneCharge() { StationId = baseStationId, DroneId = droneId });
            UpdateMinusChargeSlots(baseStationId);
        }

        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseDroneFromChargingAtBaseStation(int droneId)
        {
            //find the Station Id and remove from the DroneChargeList.
            int indexafordroneCharge = DataSource.DroneChargeList.FindIndex(x => x.DroneId == droneId);
            DroneCharge help2 = DataSource.DroneChargeList[indexafordroneCharge];
            int baseStationId = help2.StationId;
            DataSource.DroneChargeList.RemoveAt(DataSource.DroneChargeList.FindIndex(x => x.DroneId == droneId));

            UpdatePluseChargeSlots(baseStationId);
        }

        //public IEnumerable<DroneCharge> GetBaseChargeList()
        //{
        //    return DataSource.DroneChargeList.Take(DataSource.DroneChargeList.Count);
        //}

        /// <summary>
        /// the fanction get baseCharge List.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>return baseCharge List. </returns>
        public IEnumerable<DroneCharge> GetBaseChargeList(Predicate<DroneCharge> predicate = null)
        {
            return DataSource.DroneChargeList.FindAll(x => predicate == null ? true : predicate(x));
        }
    }
}
