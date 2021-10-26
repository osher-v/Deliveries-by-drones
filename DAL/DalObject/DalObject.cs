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

        #region Functions of insert options
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name="newbaseStation"></param>
        public void AddStation(BaseStation newbaseStation)
        {
            DataSource.baseStationsList.Add(newbaseStation);
        }

        /// <summary>
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name="newDrone"></param>
        public void AddDrone(Drone newDrone)
        {
            DataSource.droneList.Add(newDrone);
        }

        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(Customer newCustomer)
        {
            DataSource.customersList.Add(newCustomer);
        }

        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        public int AddParcel(Parcel newParcel)
        {
            newParcel.Id = DataSource.Config.CountIdPackage++;
            DataSource.parcelsList.Add(newParcel);
            return newParcel.Id; //Returns the id of the current Parcel.
        }
        #endregion Functions of insert options

        #region Functions of update options
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

            /*
            //drone update.
            int indexaforDrone = DataSource.droneList.FindIndex(x => x.Id == droneId);
            Drone help = DataSource.droneList[indexaforDrone];
            help.Status = (DroneStatuses)2; //busy
            DataSource.droneList[indexaforDrone] = help;
            */
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
            DataSource.droneChargeList.RemoveAt(DataSource.droneChargeList.FindIndex(x => x.DroneId == droneId));

            //BaseStation update.
            int indexaforBaseStationId = DataSource.baseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.baseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots++;
            DataSource.baseStationsList[indexaforBaseStationId] = temp;
        }
        #endregion Functions of update options

        #region Functions of display options
        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name="ID">Id of a selected BaseStation </param>
        /// <returns> return empty ubjact if its not there</returns>
        public BaseStation GetBaseStation(int ID)
        {
            return DataSource.baseStationsList.Find(x => x.Id == ID);
        }

        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="ID">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Drone GetDrone(int ID)
        {
            return DataSource.droneList.Find(x => x.Id == ID);
        }

        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="ID">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Customer GetCustomer(int ID)
        {
            return DataSource.customersList.Find(x => x.Id == ID);
        }

        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="ID">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Parcel GetParcel(int ID)
        {
            return DataSource.parcelsList.Find(x => x.Id == ID);
        }
        #endregion Functions of display options

        #region Functions of listing options
        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<BaseStation> GetBaseStationList()
        {
            return DataSource.baseStationsList.Take(DataSource.baseStationsList.Count).ToList();
        }

        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Drone> GetDroneList()
        {
            return DataSource.droneList.Take(DataSource.droneList.Count).ToList();
        }

        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Customer> GetCustomerList()
        {
            return DataSource.customersList.Take(DataSource.customersList.Count).ToList();
        }

        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Parcel> GetParcelList()
        {
            return DataSource.parcelsList.Take(DataSource.parcelsList.Count).ToList();
        }

        /// <summary>
        /// The function returns an array of all packages not associated with the Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<Parcel> GetParcelWithoutDrone()
        {
            return DataSource.parcelsList.TakeWhile(x =>x.DroneId == 0).ToList();
        }

        /// <summary>
        /// The function returns base stations with free charge positions.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public List<BaseStation> GetBaseStationsWithFreeChargSlots()
        { 
            return DataSource.baseStationsList.TakeWhile(x => x.FreeChargeSlots > 0).ToList();
        }
        #endregion Functions of listing options

        #region Convert decima to sexagesimal function 

        /// <summary>
        /// A function that converts from decimal to Sexagesimal
        /// </summary>
        /// <param name="decimalValueToConvert">The number to convert</param>
        /// <param name="side"></param>
        /// <returns>string that hold the convert location</returns>
        public static string ConvertDecimalDegreesToSexagesimal(double decimalValueToConvert,LongitudeAndLatitude side )
        {
            string daricton = null;
            switch (side)
            {
                case LongitudeAndLatitude.Longitude:
                    if (decimalValueToConvert >= 0)
                        daricton = "N";
                    else daricton = "S";
                    break;

                case LongitudeAndLatitude.Latitude:
                    if (decimalValueToConvert >= 0)//chack the number if its too east or weast
                        daricton = "E";
                    else daricton = "W";
                    break;

                default:
                    break;
            }

            int degrees = (int)decimalValueToConvert;// we lose the numbers affter the dot.
            int minutes = (int)((decimalValueToConvert - degrees) * 60);//we take the decimal number and we remove the number that we take before 
                                                                        //and multiplay by 60 (becuse we want minuts)
            float seconds = (float)((decimalValueToConvert - degrees - (minutes / 60)) * 3600);//and multiplay by 3600 (becuse we want seconds)

            return String.Format("{0}° {1}' {2}'' {3}", Math.Abs(degrees), Math.Abs(minutes), Math.Abs(seconds), daricton);// return the complited number
        }
       

        /*
        public static string ConvertLongitudDecimalDegreesToSexagesimal(double decimalValueToConvert)
        {

            int degrees = (int)decimalValueToConvert;// we lose the numbers affter the dot.
            int minutes = (int)((decimalValueToConvert - degrees) * 60);//we take the decimal number and we remove the number that we take before 
                                                                        //and multiplay by 60 (becuse we want minuts)
            float seconds = (float)((decimalValueToConvert - degrees - (minutes / 60)) * 3600);//and multiplay by 3600 (becuse we want seconds)

            string daricton;
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
        */
        #endregion Convert decima to sexagesimal function 

        #region Function of calculating distance between points
        /// <summary>
        /// A function that calculates the distance between points
        /// </summary>
        /// <param name="longitude">The new longitude</param>
        /// <param name="latitude">The new latitude</param>
        /// <param name="ID">the ID for the reqsted objact</param>
        /// <param name="yourChoise">the choise for the objact (to know the list that we need)</param>
        /// <returns>the distence between the points</returns>
        public static double GetDistance(double longitude, double latitude, int ID ,int yourChoise)
        {
            double otherLongitude, otherLatitude;
            int index;

            if (yourChoise == 1)//BaseStation point
            {
                index = DataSource.baseStationsList.FindIndex(x => x.Id == ID);
                otherLongitude = DataSource.baseStationsList[index].Longitude;
                otherLatitude = DataSource.baseStationsList[index].Latitude;
            }
            else if (yourChoise == 2)//customer point
            {
                index = DataSource.customersList.FindIndex(x => x.Id == ID);
                otherLongitude = DataSource.customersList[index].Longitude;
                otherLatitude = DataSource.customersList[index].Latitude;
            }
            else return -1;

            //For the calculation we calculate the earth into a circle (ellipse) Divide its 360 degrees by half
            //180 for each longitude / latitude and then make a pie on each half to calculate the radius for
            //the formula below
            var num1 = longitude * (Math.PI / 180.0); 
            var d1 = latitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0); //https://iw.waldorf-am-see.org/588999-calculating-distance-between-two-latitude-QPAAIP
                                                                                                                                   //We calculate the distance according to a formula that
                                                                                                                                   // also takes into account the curvature of the earth
            return (double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }
        #endregion Function of calculating distance between points
    }
}


