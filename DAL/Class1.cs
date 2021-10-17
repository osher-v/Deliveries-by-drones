
using System;
using IDAL.DO;

namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
            public int Id { get; set; }
            public string StationName { get; set; }
            public int chargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return string.Format("id is: {0}\t name of the station is: {1}\t number of charge slots: {2}\t" +
                    "Longitude: {3}\t  Latitude: {4}\t ", Id, StationName, chargeSlots, Longitude, Latitude);
            }
        }

        public enum WeightCategories{light,medium,heavy }
        public enum DroneStatuses{ free, inMaintenance, busy }
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight  { get; set; }
            public double Battery { get; set; }
            public DroneStatuses Status { get; set; }
            public override string ToString()
            {
                return string.Format("id is: {0}\t model of the drone is: {1}\t drone type is: {2}\t" +
                    "Battery level is: {3}\t  drone status is: {4}\t " ,Id, Model, MaxWeight, Battery, Status);
            }
        }

        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return string.Format("id is: {0}\t Customer's name: {1}\t Customer's phone naumber: {2}\t" +
                    "Longitude location: {3}\t  Latitude location: {4}\t ", Id, Name, PhoneNumber, Longitude, Latitude);
            }
        }
     
        public enum Priorities { regular, fast,urgent}
        public struct Parcel
        {
            public int Id { get; set; }

            public int SenderId { get; set; }

            public int TargetId { get; set; }

            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }//
            public int DroneId { get; set; }

            public DateTime Requested { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }           
            public DateTime Delivered { get; set; }
            public override string ToString()
            {
                return string.Format("ID is: {0}\t sender ID is: {1}\t target ID: {2}\t" +
                    "parcel Weight: {3}\t  Priority: {4}\t  drone id: {5}\t" +
                    "Request started in: {6}\t  Scheduled: {7}\t" +
                    "pick up time at: {8}\t  arivel time at: {9}\t"
                    , Id, SenderId, TargetId, Weight, Priority, DroneId, Requested, Scheduled, PickedUp, Delivered);
            }
        }

        public struct DroneCharge
        {
            public int StationId { get; set; }
            public int DroneId { get; set; }
            public override string ToString()
            {
                return string.Format("Station id is: {0}\t DroneId: {1} ", StationId, DroneId);
            }
        }

    }

}
namespace DalObject
{
    public class DataSource
    {
        private static Random random = new Random(DateTime.Now.Millisecond);
        internal Drone[] droneArr = new Drone[10];
        internal static BaseStation[] baseStationArr = new BaseStation[10];
        internal Customer[] customerArr;
        internal Parcel[] parcelArr = new Parcel[10];

        internal class Config
        {
            public static int indexOlderForDroneArr = 0;
            public static int indexOlderForBaseStationArr = 0;
            public static int indexOlderForCustomerArr = 0;
            public static int indexOlderForParcelArr = 0;
        }
        public static void initialize() 
        {
           baseStationArr[0] = new BaseStation() 
               { 
                 chargeSlots = random.Next(5, 10)
               };

           baseStationArr[1] = new BaseStation()
               {
                  chargeSlots = random.Next(5, 10),
               };
        }
    }
}   





