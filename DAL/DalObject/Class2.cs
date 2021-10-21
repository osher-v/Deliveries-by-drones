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
            DataSource.baseStationsList.Add(new BaseStation {
                Id = ID,
                StationName = name,
                FreeChargeSlots = chargsSlots,
                Longitude = longitude,
                Latitude = latitude
            });
        }
        
        public void SetDrone(int droneID, string model, int weightCategory, int status, double batterylevel)
        {
            DataSource.droneList.Add(new Drone() {
                Id = droneID,
                Model = model,
                MaxWeight = (WeightCategories)weightCategory,
                Battery = status,
                Status = (DroneStatuses)batterylevel
            });
        }
        public void SetCustomer(int customerID, string customerName, string PhoneNumber, double customerLongitude, double customerLatitude)
        {
            DataSource.customersList.Add( new Customer() {
                Id = customerID,
                Name = customerName,
                PhoneNumber = PhoneNumber,
                Longitude = customerLongitude,
                Latitude = customerLatitude
            });
        }
        public void SetParcel(int parcelID, int senderId, int targetId, int weight, int priorities)
        {
            DataSource.parcelsList.Add( new Parcel() {
                Id = parcelID,
                SenderId = senderId,
                TargetId = targetId,
                Weight = (WeightCategories)weight,
                Priority = (Priorities)priorities,
                Requested = DateTime.Now
            });
        }


        public BaseStation GetBaseStation(int ID)
        {
            BaseStation empty = new BaseStation();
            for (int i = 0; i < DataSource.baseStationsList.Count(); i++)
            {
                if (ID == DataSource.baseStationsList[i].Id)
                    return DataSource.baseStationsList[i];
            }
            return empty;
        }

        public Drone GetDrone(int ID)
        {
            Drone empty = new Drone();
            for (int i = 0; i < DataSource.droneList.Count(); i++)
            {
                if (ID == DataSource.droneList[i].Id)
                    return DataSource.droneList[i];
            }
            return empty;
        }

        public Customer GetCustomer(int ID)
        {
            Customer empty = new Customer();
            for (int i = 0; i < DataSource.customersList.Count(); i++)
            {
                if (ID == DataSource.customersList[i].Id)
                    return DataSource.customersList[i];
            }
            return empty;
        }

        public Parcel GetParcel(int ID)
        {
            Parcel empty = new Parcel();
            for (int i = 0; i < DataSource.parcelsList.Count(); i++)
            {
                if (ID == DataSource.parcelsList[i].Id)
                    return DataSource.parcelsList[i];
            }
            return empty;
        }


        public List<BaseStation> GetBaseStationList()
        {
            List<BaseStation> temp = new List<BaseStation>();
            for (int i = 0; i < DataSource.baseStationsList.Count(); i++)
            {
                temp.Add(DataSource.baseStationsList[i]);
            }
            return temp;
        }

        public List<Drone> GetDroneList()
        {
            List<Drone> temp = new List<Drone>();
            for (int i = 0; i < DataSource.droneList.Count(); i++)
            {
                temp.Add(DataSource.droneList[i]);
            }
            return temp;
        }

        public List<Customer> GetCustomerList()
        {
            List<Customer> temp = new List<Customer>();
            for (int i = 0; i < DataSource.droneList.Count(); i++)
            {
                temp.Add(DataSource.customersList[i]);
            }
            return temp;
        }

        public List<Parcel> GetParcelList()
        {
            List<Parcel> temp = new List<Parcel>();
            for (int i = 0; i < DataSource.parcelsList.Count(); i++)
            {
                temp.Add(DataSource.parcelsList[i]);
            }
            return temp;
        }

        public List<Parcel> GetParcelWithoutDrone()
        {
            List<Parcel> temp = new List<Parcel>();
            for (int i = 0; i < DataSource.parcelsList.Count(); i++)
            {
                if(DataSource.parcelsList[i].DroneId==0)
                    temp.Add(DataSource.parcelsList[i]);
            }
            return temp;
        }
        
        public List<BaseStation> GetBaseStationsWithFreeChargSlots()
        {
            List<BaseStation> temp = new List<BaseStation>();
            for (int i = 0; i < DataSource.baseStationsList.Count(); i++)
            {
                if (DataSource.baseStationsList[i].FreeChargeSlots > 0)
                    temp.Add(DataSource.baseStationsList[i]);
            }
            return temp;
        }
    }
}
