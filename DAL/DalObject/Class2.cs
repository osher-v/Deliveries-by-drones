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
        public DalObject()
        {
            DataSource.Initialize();
        }
        
        public void SetStation(int ID, string name, int chargsSlots, double longitude, double latitude)
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
        
        public void SetDrone(int droneID, string model, int weightCategory, int status, double batterylevel)
        {
            DataSource.droneArr[DataSource.Config.indexOlderForDroneArr++] = new Drone()
            {
                Id = droneID,
                Model = model,
                MaxWeight = (WeightCategories)weightCategory,
                Battery = status,
                Status = (DroneStatuses)batterylevel
            };
        }
        public void SetCustomer(int customerID, string customerName, string PhoneNumber, double customerLongitude, double customerLatitude)
        {
            DataSource.customerArr[DataSource.Config.indexOlderForCustomerArr++] = new Customer()
            {
                Id = customerID,
                Name = customerName,
                PhoneNumber = PhoneNumber,
                Longitude = customerLongitude,
                Latitude = customerLatitude
            };
        }
        public void SetParcel(int parcelID, int senderId, int targetId, int weight, int priorities)
        {
            DataSource.parcelArr[DataSource.Config.indexOlderForParcelArr++] = new Parcel()
            {
                Id = parcelID,
                SenderId = senderId,
                TargetId = targetId,
                Weight = (WeightCategories)weight,
                Priority = (Priorities)priorities,
                Requested = DateTime.Now
            };
        }


        public BaseStation GetBaseStation(int ID)
        {
            BaseStation empty = new BaseStation();
            for (int i = 0; i < DataSource.Config.indexOlderForBaseStationArr; i++)
            {
                if (ID == DataSource.baseStationArr[i].Id)
                    return DataSource.baseStationArr[i];
            }
            return empty;
        }

        public Drone GetDrone(int ID)
        {
            Drone empty = new Drone();
            for (int i = 0; i < DataSource.Config.indexOlderForDroneArr; i++)
            {
                if (ID == DataSource.droneArr[i].Id)
                    return DataSource.droneArr[i];
            }
            return empty;
        }

        public Customer GetCustomer(int ID)
        {
            Customer empty = new Customer();
            for (int i = 0; i < DataSource.Config.indexOlderForCustomerArr; i++)
            {
                if (ID == DataSource.customerArr[i].Id)
                    return DataSource.customerArr[i];
            }
            return empty;
        }

        public Parcel GetParcel(int ID)
        {
            Parcel empty = new Parcel();
            for (int i = 0; i < DataSource.Config.indexOlderForParcelArr; i++)
            {
                if (ID == DataSource.parcelArr[i].Id)
                    return DataSource.parcelArr[i];
            }
            return empty;
        }


        public BaseStation[] GetBaseStationList()
        {
            return DataSource.baseStationArr.Take();

            //return DataSource.baseStationArr.Take(DataSource.Config.indexOlderForBaseStationArr)
        }
    }
}
