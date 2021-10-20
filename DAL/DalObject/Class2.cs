using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        DalObject()
        {
            DataSource.Initialize();
        }
        
        public static void SetStation(int ID, string name, int chargsSlots, double longitude, double latitude)
        {
            DataSource.baseStationArr[DataSource.Config.indexOlderForBaseStationArr++] = new BaseStation()
            {
                Id = ID,
                StationName = name,
                ChargeSlots = chargsSlots,
                Longitude = longitude,
                Latitude = latitude
            };
        }
        public static void SetDrone(int droneID, string model, int weightCategory, int status, double batterylevel)
        {


        }
        public static void SetCustomer(int customerID, string customerName, string PhoneNumber, double customerLongitude, double customerLatitude)
        {

        }
        public static void SetParcel(int parcelID, int SenderId, int TargetId, int Weight, int priorities)
        {

        }


        public static BaseStation GetBaseStation(int ID)
        {
            for (int i = 0; i < DataSource.Config.indexOlderForBaseStationArr; i++)
            {
                if ((ID == DataSource.baseStationArr[i].Id))
                    return DataSource.baseStationArr[i];
            }
            return DataSource.baseStationArr[1];
        }
    }
}
