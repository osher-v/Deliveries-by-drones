using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;

using DalApi;

namespace DalObject
{
     partial class DalObject 
    {
        public void SendingDroneforChargingAtBaseStation(int baseStationId, int droneId)
        {
            DataSource.DroneChargeList.Add(new DroneCharge() { StationId = baseStationId,
                DroneId = droneId, StartChargeTime =  DateTime.Now });
        }
  
        public void ReleaseDroneFromChargingAtBaseStation(int droneId)
        {       
            DataSource.DroneChargeList.RemoveAt(DataSource.DroneChargeList.FindIndex(x => x.DroneId == droneId));
        }

        public DroneCharge GetBaseCharge(int droneID)
        {
            if (!DataSource.DroneChargeList.Exists(x => x.DroneId == droneID))
            {
                throw new NonExistentObjectException();
            }
            return DataSource.DroneChargeList.Find(x => x.DroneId == droneID);
        }
   
        public IEnumerable<DroneCharge> GetBaseChargeList(Predicate<DroneCharge> predicate = null)
        {
            //return DataSource.DroneChargeList.FindAll(x => predicate == null ? true : predicate(x));
            return DataSource.DroneChargeList.Where(x => predicate == null ? true : predicate(x));
        }
    }
}
