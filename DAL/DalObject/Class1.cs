using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
using System.ComponentModel;

namespace DalObject
{
    public class DataSource
    {
        internal static Random random = new Random(DateTime.Now.Millisecond);

        internal static Drone[] droneArr = new Drone[10];
        internal static BaseStation[] baseStationArr = new BaseStation[5];
        internal static Customer[] customerArr = new Customer[100];
        internal static Parcel[] parcelArr = new Parcel[1000];

        internal class Config
        {
            internal static int indexOlderForDroneArr = 0;
            internal static int indexOlderForBaseStationArr = 0;
            internal static int indexOlderForCustomerArr = 0;
            internal static int indexOlderForParcelArr = 0;

            public static int CountIdPackage = 0;
        }

        public static void Initialize()
        {
            //
            baseStationArr[0] = new BaseStation()
            {
                Id = random.Next(100000000, 999999999),
                StationName = "BnyBrak",
                ChargeSlots = random.Next(5, 10),
                Longitude = 32.086456,
                Latitude = 34.844476
            };
            baseStationArr[0] = new BaseStation()
            {
                Id = random.Next(100000000, 999999999),
                StationName = "Holon",
                ChargeSlots = random.Next(5, 10),
                Longitude = 32.021679,
                Latitude = 34.789990
                //Latitude = (float)((float)(random.NextDouble() * (33.3 - 31)) + 31)
        };

            //light, medium, heavy
            //free, inMaintenance, busy
            droneArr[0] = new Drone()
            {
                Id = random.Next(100000000, 999999999),
                Model = "a",
                MaxWeight = (WeightCategories)0, //light,
                Battery = 55.8,
                Status = (DroneStatuses)1 //inMaintenance
            };
            droneArr[1] = new Drone()
            {
                Id = random.Next(100000000, 999999999),
                Model = "c",
                MaxWeight = (WeightCategories)2, //heavy
                Battery = 78,
                Status = (DroneStatuses)2 //busy
            };
            droneArr[2] = new Drone()
            {
                Id = random.Next(100000000, 999999999),
                Model = "c",
                MaxWeight = (WeightCategories)2, //heavy
                Battery = 100,
                Status = (DroneStatuses)0 //free
            };
            droneArr[3] = new Drone()
            {
                Id = random.Next(100000000, 999999999),
                Model = "b",
                MaxWeight = (WeightCategories)1, //medium
                Battery = 100,
                Status = (DroneStatuses)0 //free
            };
            droneArr[4] = new Drone()
            {
                Id = random.Next(100000000, 999999999),
                Model = "c",
                MaxWeight = (WeightCategories)2, //heavy
                Battery = 40.8,
                Status = (DroneStatuses)2 //busy
            };

            //
            string[] CustomersNameArr = new string[10]{"James","Robert","John","Michael","William",
                   "David","Richard","Thomas","Mark","Donald"};
            for (int i = 0; i < 10; i++)
            {
                customerArr[i] = new Customer()
                {
                    Id = random.Next(100000000, 999999999),
                    Name = CustomersNameArr[i],
                    PhoneNumber = "0" + random.Next(50, 58) + "-" + random.Next(0000000, 9999999),
                    Longitude = (float)((float)(random.NextDouble() * (33.3 - 31)) + 31),
                    Latitude = (float)((float)(random.NextDouble() * (33.3 - 31)) + 31)
                };
            }

            //




        }
    }
}
