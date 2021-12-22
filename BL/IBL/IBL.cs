using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    /// <summary>
    /// interface IBL
    /// </summary>
    public interface IBL
    {
        #region Adding functions

        //Adding functions.

        /// <summary>
        /// The function adds a base station.
        /// </summary>
        /// <param name="newbaseStation">newbaseStation object</param>
        void AddStation(BaseStation newbaseStation);

        /// <summary>
        /// The function adds a Drone.
        /// </summary>
        /// <param name="newdrone">newdrone object</param>
        /// <param name="firstChargingStation">first charging station</param>
        void AddDrone(DroneToList newdrone, int firstChargingStation);

        /// <summary>
        /// The function adds a Customer.
        /// </summary>
        /// <param name="newCustomer">newCustomer object</param>
        void AddCustomer(Customer newCustomer);

        /// <summary>
        /// The function adds a Parcel.
        /// </summary>
        /// <param name="newParcel">newParcel object</param>
        void AddParcel(Parcel newParcel);
        #endregion Adding functions

        #region Update functions

        //Update functions.

        /// <summary>
        /// the function get ID for a drone that will be updated (name only)
        /// </summary>
        /// <param name="droneId">the reqsted drone</param>
        /// <param name="droneName">the new name </param>
        void UpdateDroneName(int droneId, string droneName);

        /// <summary>
        /// The function updates the charge slots at the base station.
        /// </summary>
        /// <param name="baseStationId">baseStation Id</param>
        /// <param name="baseName">base name</param>
        /// <param name="chargeslots">total quantity charge slots</param>
        void UpdateBaseStaison(int baseStationId, string baseName, string chargeslots);

        /// <summary>
        /// The function updates a customer object.
        /// </summary>
        /// <param name="customerId">customer ID</param>
        /// <param name="customerName">customer name</param>
        /// <param name="phoneNumber">phone number</param>
        void UpdateCustomer(int customerId, string customerName, string phoneNumber);

        /// <summary>
        /// the fanction send drone to charge and update accordanly 
        /// </summary>
        /// <param name="droneId">drone Id</param>
        void SendingDroneforCharging(int droneId);

        /// <summary>
        /// The function frees the skimmer from charging.
        /// </summary>
        /// <param name="droneId">drone Id</param>
        /// <param name="time">Time the drone is charging</param>
        void ReleaseDroneFromCharging(int droneId); //, DateTime time);

        /// <summary>
        /// The function assigns a drone to the parcel.
        /// </summary>
        /// <param name="droneId">drone Id</param>
        void AssignPackageToDdrone(int droneId);

        /// <summary>
        /// The function updates the package pickedUp time.
        /// </summary>
        /// <param name="droneId">drone Id</param>
        void PickedUpPackageByTheDrone(int droneId);

        /// <summary>
        /// the fanction set delivere time and update all relvent data. 
        /// </summary>
        /// <param name="droneId">the reqsted drone</param>
        void DeliveryPackageToTheCustomer(int droneId);
        #endregion Update functions

        #region Functions that return object.

        //Functions that return object.

        /// <summary>
        /// The function returns a BaseStation.
        /// </summary>
        /// <param name="idForDisplayObject">Id for display object</param>
        /// <returns>a BaseStation.</returns>
        BaseStation GetBaseStation(int idForDisplayObject);

        /// <summary>
        /// The function returns a Drone.
        /// </summary>
        /// <param name="idForDisplayObject">Id for display object</param>
        /// <returns>a Drone</returns>
        Drone GetDrone(int idForDisplayObject);

        /// <summary>
        /// The function returns a Customer.
        /// </summary>
        /// <param name="idForDisplayObject">Id for display object</param>
        /// <returns>a Customer</returns>
        Customer GetCustomer(int idForDisplayObject);

        /// <summary>
        /// The function returns a Parcel.
        /// </summary>
        /// <param name="idForDisplayObject">Id for display object</param>
        /// <returns>a Parcel</returns>
        Parcel GetParcel(int idForDisplayObject);
        #endregion Functions that return object.

        #region Functions that return a list of objects

        //Functions that return a list of objects.

        /// <summary>
        /// The function returns a list of BaseStation by condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>a list of BaseStation by condition</returns>
        IEnumerable<BaseStationsToList> GetBaseStationList(Predicate<BaseStationsToList> predicate = null);

        /// <summary>
        /// The function returns a list of Drone by condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>a list of Drone by condition</returns>
        IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null);

        /// <summary>
        /// The function returns a list of Customer by condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>a list of Customer by condition</returns>
        IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null);

        /// <summary>
        /// The function returns a list of Parcel by condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>a list of Parcel by condition</returns>
        IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate = null);
        #endregion Functions that return a list of objects

        #region Functions for remove options

        /// <summary>
        /// The function removes the Station from the list.
        /// </summary>
        /// <param name="baseStationId"></param>
        public void RemoveStation(BaseStation baseStationId);

        /// <summary>
        /// The function removes the Drone from the list.
        /// </summary>
        /// <param name="DroneId"></param>
        public void RemoveDrone(int DroneId);

        /// <summary>
        /// The function removes the Customer from the list.
        /// </summary>
        /// <param name="CustomerId"></param>
        public void RemoveCustomer(int CustomerId);

        /// <summary>
        /// The function removes the Parcel from the list.
        /// </summary>
        /// <param name="ParcelId"></param>
        /// <returns></returns>
        public void RemoveParcel(int ParcelId);
        #endregion Functions for remove options
    }
}
