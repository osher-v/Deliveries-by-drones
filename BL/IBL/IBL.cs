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
  
        void AddStation(BaseStation newbaseStation);
        void AddDrone(DroneToList newdrone, int firstChargingStation);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newParcel);


        void UpdateDroneName(int droneId, string droneName);
        void UpdateBaseStaison(int baseStationId, string baseName, int chargeslots);
        void UpdateCustomer(int customerId, string customerName, string phoneNumber);
        void SendingDroneforCharging(int droneId);
        void ReleaseDroneFromCharging(int droneId, DateTime time);
        void AssignPackageToDdrone(int droneId);
        void PickedUpPackageByTheDrone(int droneId);
        void DeliveryPackageToTheCustomer(int droneId);


        BaseStation GetBaseStation(int idForDisplayObject);
        Drone GetDrone(int idForDisplayObject);
        Customer GetCustomer(int idForDisplayObject);
        Parcel GetParcel(int idForDisplayObject);
        
       
        IEnumerable<BaseStationsToList> GetBaseStationList(Predicate<BaseStationsToList> predicate = null);
        IEnumerable<DroneToList> GetDroneList(Predicate<DroneToList> predicate = null);
        IEnumerable<CustomerToList> GetCustomerList(Predicate<CustomerToList> predicate = null);
        IEnumerable<ParcelToList> GetParcelList(Predicate<ParcelToList> predicate = null);
    }
}
