
using System;
using IDAL.DO;
using DalObject;


namespace IDAL
{
    namespace DO
    {
        public enum Priorities { regular, fast, urgent }
        public enum WeightCategories { light, medium, heavy }
        public enum DroneStatuses { free, inMaintenance, busy }


    }

}

namespace DalObject
{
    public class DataSource
    {
        internal static Random random = new Random(DateTime.Now.Millisecond);
        internal Drone[] droneArr = new Drone[10];
        internal static BaseStation[] baseStationArr = new BaseStation[5];
        internal Customer[] customerArr=new Customer[100];
        internal Parcel[] parcelArr = new Parcel[1000];

        internal class Config
        {
            internal static int indexOlderForDroneArr = 0;
            internal static int indexOlderForBaseStationArr = 0;
            internal static int indexOlderForCustomerArr = 0;
            internal static int indexOlderForParcelArr = 0;
             
            
        }

        public static void initialize()
        {
            baseStationArr[0] = new BaseStation()
               {
                    Id = random.Next(9,15);
                    StationName = "alpa";
                     Longitude = 31.123456;
                   Latitude = 31.123457;
                  chargeSlots = random.Next(5, 10);
                     
                }

            baseStationArr[1] = new BaseStation()
            {
            Id = random.Next(9, 15);
            StationName = "bita";
            Longitude = 31.123456;
            Latitude = 31.123457;
            chargeSlots = random.Next(5, 10);
        }
        }
    }
}





