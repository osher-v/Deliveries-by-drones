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
            /*
            //drone update.
            int indexaforDrone = DataSource.DronesList.FindIndex(x => x.Id == droneId);
            Drone help = DataSource.DronesList[indexaforDrone];
            help.Status = (DroneStatuses)1; //inMaintenance
            DataSource.DronesList[indexaforDrone] = help;
            */

            DataSource.DroneChargeList.Add(new DroneCharge() { StationId = baseStationId, DroneId = droneId });

            //BaseStation update.
            int indexaforBaseStationId = DataSource.BaseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots--;
            DataSource.BaseStationsList[indexaforBaseStationId] = temp;

        }
        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseDroneFromChargingAtBaseStation(int droneId)
        {
            /*
            //Drone update.
            int indexaforDrone = DataSource.DronesList.FindIndex(x => x.Id == droneId);
            Drone help = DataSource.DronesList[indexaforDrone];
            help.Status = (DroneStatuses)0; //free
            DataSource.DronesList[indexaforDrone] = help;
            */

            //find the Station Id and remove from the DroneChargeList.
            int indexafordroneCharge = DataSource.DroneChargeList.FindIndex(x => x.DroneId == droneId);
            DroneCharge help2 = DataSource.DroneChargeList[indexafordroneCharge];
            int baseStationId = help2.StationId;
            DataSource.DroneChargeList.RemoveAt(DataSource.DroneChargeList.FindIndex(x => x.DroneId == droneId));

            //BaseStation update.
            int indexaforBaseStationId = DataSource.BaseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots++;
            DataSource.BaseStationsList[indexaforBaseStationId] = temp;
        }
        
    }
}
