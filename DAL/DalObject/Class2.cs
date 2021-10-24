using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;

namespace DalObject
{
    /// <summary>
    /// matods that use from the main 
    /// </summary>
    public class DalObject
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="ID"> Id of baseStations </param>
        /// <param name="name">name of baseStations</param>
        /// <param name="chargsSlots">number of chargsSlots baseStations</param>
        /// <param name="longitude">longitude of baseStations</param>
        /// <param name="latitude">latitude of baseStations</param>
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

        /// <summary>
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="droneID">Id of Drone</param>
        /// <param name="model">model of Drone</param>
        /// <param name="weightCategory">weight of Drone</param>
        /// <param name="status">status of Drone</param>
        /// <param name="batterylevel">batterylevel of Drone</param>
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

        /// <summary>
        /// The function adds a customer to the list of sustomers.
        /// </summary>
        /// <param name="customerID">Id of customer</param>
        /// <param name="customerName">name of customer</param>
        /// <param name="PhoneNumber">PhoneNumber of customer</param>
        /// <param name="customerLongitude">Longitude of customer</param>
        /// <param name="customerLatitude">Latitude of customer</param>
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

        /// <summary>
        /// The function adds a Parcel to the list of Parcels
        /// </summary>
        /// <param name="senderId">The identity of the sender</param>
        /// <param name="targetId">Address identity</param>
        /// <param name="weight">weight of Parcel</param>
        /// <param name="priorities">The urgency of the package</param>
        /// <returns>
        /// </returns>
        public int SetParcel( int senderId, int targetId, int weight, int priorities)
        {
            DataSource.parcelsList.Add( new Parcel() {
                Id=DataSource.Config.CountIdPackage++,
                SenderId = senderId,
                TargetId = targetId,
                Weight = (WeightCategories)weight,
                Priority = (Priorities)priorities,
                Requested = DateTime.Now    
            });
            return DataSource.Config.CountIdPackage;
        }

        /// <summary>
        /// The function assigns a package to the drone.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        /// <param name="droneId">Id of drone</param>
        public void AssignPackageToDdrone(int ParcelId, int droneId)
        {
            //Update the package.
            int indexaforParcel = DataSource.parcelsList.FindIndex(x => x.Id == ParcelId);
            Parcel temp = DataSource.parcelsList[indexaforParcel];
            temp.DroneId = droneId;
            temp.Assigned = DateTime.Now;
            DataSource.parcelsList[indexaforParcel] = temp;

            //drone update.
            int indexaforDrone = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone help = DataSource.droneList[indexaforDrone];
            help.Status = (DroneStatuses)2; //busy
            DataSource.droneList[indexaforDrone] = help;
        }

        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        public void PickedUpPackageByTheDrone(int ParcelId)
        {
            //Update the package.
            int indexaforParcel = DataSource.parcelsList.FindIndex(x => x.Id == ParcelId);
            Parcel temp = DataSource.parcelsList[indexaforParcel];
            temp.PickedUp = DateTime.Now;
            DataSource.parcelsList[indexaforParcel] = temp;
        }

        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        public void DeliveryPackageToTheCustomer(int ParcelId)
        {
            int indexaforParcel = DataSource.parcelsList.FindIndex(x => x.Id == ParcelId);
            Parcel temp = DataSource.parcelsList[indexaforParcel];
            temp.Delivered = DateTime.Now;
            DataSource.parcelsList[indexaforParcel] = temp;
        }

        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
        public void SendingDroneforChargingAtBaseStation(int baseStationId ,int droneId)
        {
            //drone update.
            int indexaforDrone = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone help = DataSource.droneList[indexaforDrone];
            help.Status = (DroneStatuses)1; //inMaintenance
            DataSource.droneList[indexaforDrone] = help;

            DataSource.droneChargeList.Add(new DroneCharge() { StationId = baseStationId, DroneId = droneId });

            //BaseStation update.
            int indexaforBaseStationId = DataSource.baseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.baseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots--;
            DataSource.baseStationsList[indexaforBaseStationId] = temp;
        }

        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseDroneFromChargingAtBaseStation(int droneId)
        {
            //Drone update.
            int indexaforDrone = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone help = DataSource.droneList[indexaforDrone];
            help.Status = (DroneStatuses)0; //free
            DataSource.droneList[indexaforDrone] = help;

            //find the Station Id and remove from the droneChargeList.
            int indexafordroneCharge = DataSource.droneChargeList.FindIndex(x => x.DroneId == droneId);
            DroneCharge help2 = DataSource.droneChargeList[indexafordroneCharge];
            int baseStationId = help2.StationId;
            DataSource.droneChargeList.Remove(DataSource.droneChargeList.Find(x => x.DroneId == droneId));

            //BaseStation update.
            int indexaforBaseStationId = DataSource.baseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.baseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots++;
            DataSource.baseStationsList[indexaforBaseStationId] = temp;
        }

        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name="ID">Id of a selected BaseStation </param>
        /// <returns> return empty ubjact if its not there</returns>
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

        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="ID">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
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

        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="ID">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
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

        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="ID">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
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

        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<BaseStation> GetBaseStationList()
        {
            List<BaseStation> temp = new List<BaseStation>();
            for (int i = 0; i < DataSource.baseStationsList.Count(); i++)
            {
                temp.Add(DataSource.baseStationsList[i]);
            }
            return temp;
        }

        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Drone> GetDroneList()
        {
            List<Drone> temp = new List<Drone>();
            for (int i = 0; i < DataSource.droneList.Count(); i++)
            {
                temp.Add(DataSource.droneList[i]);
            }
            return temp;
        }

        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Customer> GetCustomerList()
        {
            List<Customer> temp = new List<Customer>();
            for (int i = 0; i < DataSource.customersList.Count(); i++)
            {
                temp.Add(DataSource.customersList[i]);
            }
            return temp;
        }

        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Parcel> GetParcelList()
        {
            List<Parcel> temp = new List<Parcel>();
            for (int i = 0; i < DataSource.parcelsList.Count(); i++)
            {
                temp.Add(DataSource.parcelsList[i]);
            }
            return temp;
        }

        /// <summary>
        /// The function returns an array of all packages not associated with the Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
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

        /// <summary>
        /// The function returns base stations with free charge positions.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
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
        /// <summary>
        /// A function that converts from decimal to Sexagesimal
        /// </summary>
        /// <param name="decimalValueToConvert"> the lociton on decimal display</param>
        /// <returns>string that hold the convert location</returns>
        public static string ConvertLongitudDecimalDegreesToSexagesimal(double decimalValueToConvert)
        {

            int degrees = (int)decimalValueToConvert;// we lose the numbers affter the dot.
            int minutes = (int)((decimalValueToConvert - degrees) * 60);//we take the decimal number and we remove the number that we take before 
                                                                        //and multiplay by 60 (becuse we want minuts)
            float seconds = (float)((decimalValueToConvert - degrees - (minutes / 60)) * 3600);//and multiplay by 3600 (becuse we want seconds)
            string daricton = null;
            if (decimalValueToConvert >= 0)
                daricton = "N";
            else daricton = "S";
            return String.Format("{0}° {1}' {2}'' {3}", Math.Abs(degrees), Math.Abs(minutes), Math.Abs(seconds),daricton);// return the complited number
        }
        /// <summary>
        /// A function that converts from decimal to Sexagesimal
        /// </summary>
        /// <param name="decimalValueToConvert"> the lociton on decimal display</param>
        /// <returns>string that hold the convert location</returns>
        public static string ConvertLatitudDecimalDegreesToSexagesimal(double decimalValueToConvert)
        {

            int degrees = (int)decimalValueToConvert;// we lose the numbers affter the dot.
            int minutes = (int)((decimalValueToConvert - degrees) * 60);//we take the decimal number and we remove the number that we take before 
                                                                        //and multiplay by 60 (becuse we want minuts)
            float seconds = (float)((decimalValueToConvert - degrees - (minutes / 60)) * 3600);//and multiplay by 3600 (becuse we want seconds)
            string daricton = null;
            if (decimalValueToConvert >= 0)//chack the number if its too east or weast
                daricton = "E";
            else daricton = "W";
            return String.Format("{0}° {1}' {2}'' {3}", Math.Abs(degrees), Math.Abs(minutes), Math.Abs(seconds), daricton); // return the complited number
        }
    }
}
