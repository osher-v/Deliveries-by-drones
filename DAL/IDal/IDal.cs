using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public interface IDal
    {    
        #region Functions for insert options
        /// <summary>
        /// The function adds a station to the list of Basestations.
        /// </summary>
        /// <param name = "newbaseStation" ></ param >
        public void AddStation(BaseStation newbaseStation);

        /// <summary>
        /// The function adds a drone to the list of drones.
        /// </summary>
        /// <param name = "newDrone" ></ param >
        public void AddDrone(Drone newDrone);

        /// <summary>
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name = "newCustomer" ></ param >
        public void AddCustomer(Customer newCustomer);

        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        public int AddParcel(Parcel newParcel);
        #endregion Functions for insert options

        #region Functions for update options
        /// <summary>
        /// The function updates the number of charging stations at the base station by--
        /// </summary>
        /// <param name="baseStationId">baseStationId</param>
        public void UpdateMinusChargeSlots(int baseStationId);

        /// <summary>
        /// The function updates the number of charging stations at the base station by++
        /// </summary>
        /// <param name="baseStationId">baseStationId</param>
        public void UpdatePluseChargeSlots(int baseStationId);

        /// <summary>
        /// The function updates the drone object in the list
        /// </summary>
        /// <param name="newDrone">newDrone</param>
        public void UpdateDrone(Drone newDrone);

        /// <summary>
        /// The function updates the Customer object in the list
        /// </summary>
        /// <param name="newCustomer"></param>
        public void UpdateCustomer(Customer newCustomer);

        /// <summary>
        /// The function updates the BaseStation object in the list
        /// </summary>
        /// <param name="newBaseStation"></param>
        public void UpdateBaseStation(BaseStation newBaseStation);

        /// <summary>
        /// The function assigns a package to the drone.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        /// <param name="droneId">Id of drone</param>
        public void AssignPackageToDdrone(int ParcelId, int droneId);

        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        public void PickedUpPackageByTheDrone(int ParcelId);

        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        public void DeliveryPackageToTheCustomer(int ParcelId);

        /// <summary>
        /// sending drone for charging at BaseStation.
        /// </summary>
        /// <param name="baseStationId">Id of baseStation</param>
        /// <param name="droneId">Id of drone</param>
        public void SendingDroneforChargingAtBaseStation(int baseStationId, int droneId);

        /// <summary>
        /// release drone from charging at BaseStation.
        /// </summary>
        /// <param name="droneId">Id of drone</param>
        public void ReleaseDroneFromChargingAtBaseStation(int droneId);
        #endregion Functions for update options

        #region Functions for display options
        /// <summary>
        /// The function returns the selected base station.
        /// </summary>
        /// <param name = "ID" > Id of a selected BaseStation</param>
        /// <returns> return empty ubjact if its not there</returns>
        public BaseStation GetBaseStation(int ID);

        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="ID">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Drone GetDrone(int ID);

        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="ID">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Customer GetCustomer(int ID);

        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="ID">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Parcel GetParcel(int ID);

        /// <summary>
        /// The function returns the selected Base Charge.
        /// </summary>
        /// <param name="droneID">drone ID</param>
        /// <returns>the selected Base Charge</returns>
        public DroneCharge GetBaseCharge(int droneID);
        #endregion Functions for display options

        #region Functions for listing options
        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<BaseStation> GetBaseStationList(Predicate<BaseStation> predicate = null);

        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Drone> GetDroneList();

        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Customer> GetCustomerList();

        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> prdicat = null);

        /// <summary>
        /// The function returns an array of all drone charge.
        /// </summary>
        /// <param name="prdicat"></param>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<DroneCharge> GetBaseChargeList(Predicate<DroneCharge> prdicat = null);  
        #endregion Functions for listing options

        public double[] RequestPowerConsumptionByDrone();
    }
}
