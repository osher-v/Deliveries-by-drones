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
        //internal static List<Drone> DronesList = new List<Drone>();
        ///// <summary> list of base stations </summary>
        //internal static List<BaseStationsToList> BaseStationsList = new List<BaseStationsToList>();
        ///// <summary>list of customers</summary>
        //internal static List<Customer> CustomersList = new List<Customer>();
        ///// <summary> list of parcels </summary>
        //internal static List<Parcel> ParcelsList = new List<Parcel>();
        ///// <summary> class that responsible for counters </summary>

        void AddStation(BaseStation newbaseStation);
        void AddDrone(DroneToList newdrone, int firstChargingStation);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newParcel);
        void UpdateDroneName(int droneId, string droneName);
        void UpdateBaseStaison(int baseStationId, string baseName, int chargeslots);
        void UpdateCustomer(int customerId, string customerName, string phoneNumber);
        void SendingDroneforCharging(int droneId);
        void ReleaseDroneFromCharging(int droneId, DateTime time);
        object GetBaseStation(int idForDisplayObject);
        object GetDrone(int idForDisplayObject);
        object GetCustomer(int idForDisplayObject);
        object GetParcel(int idForDisplayObject);
        object GetBaseStationList();
    }
}
