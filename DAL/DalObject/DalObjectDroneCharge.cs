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
        //בשתי הפונקציות הבאות אלו חריגות בדיוק לעשות????

        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
        public void SendingDroneforChargingAtBaseStation(int baseStationId, int droneId)
        {
            DataSource.DroneChargeList.Add(new DroneCharge() { StationId = baseStationId, DroneId = droneId });
            updateMinusChargeSlots(baseStationId);
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

            updatePluseChargeSlots(baseStationId);
        }
        
    }
}
