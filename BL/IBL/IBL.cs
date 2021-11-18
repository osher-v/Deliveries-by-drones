using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IBL;

namespace IBL
{
    public interface IBL
    {
        //Adding functions.

        void AddStation(BaseStation newbaseStation);
        void AddDrone(DroneToList newdrone, int firstChargingStation);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newParcel);

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
        void ReleaseDroneFromCharging(int droneId, DateTime time);

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

        //

        BaseStation GetBaseStation(int idForDisplayObject);
        Drone GetDrone(int idForDisplayObject);
        Customer GetCustomer(int idForDisplayObject);
        Parcel GetParcel(int idForDisplayObject);
        
        //

        IEnumerable<BaseStationsToList> GetBaseStationList(Predicate<BaseStationsToList> predicate = null);
        IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null);
        IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null);
        IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate = null);
    }
}
