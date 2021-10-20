using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
using System.ComponentModel;

namespace DalObject
{
    public static class DataSource
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
            baseStationArr[Config.indexOlderForBaseStationArr++] = new BaseStation()
            {
                Id = random.Next(100000000, 999999999),
                StationName = "BnyBrak",
                ChargeSlots = random.Next(5, 10),
                Longitude = 32.086456,
                Latitude = 34.844476
            };

            baseStationArr[Config.indexOlderForBaseStationArr++] = new BaseStation()
            {
                Id = random.Next(100000000, 999999999),
                StationName = "Holon",
                ChargeSlots = random.Next(5, 10),
                Longitude = 32.021679,
                Latitude = 34.789990
            };

            string[] modelNameArr = new string[5] { "I", "IX", "IIX", "VI", "IL" };
            for (int i = 0; i < 5; i++)
            {
                droneArr[Config.indexOlderForDroneArr++] = new Drone()
                {
                    Id = random.Next(100000000, 999999999),
                    Model = modelNameArr[i],
                    MaxWeight = (WeightCategories)random.Next(0, 2),
                    Battery = random.Next(50, 100),
                    Status = (DroneStatuses)random.Next(0, 2)
                };
            }

            string[] CustomersNameArr = new string[10]{"James","Robert","John","Michael","William",
                   "David","Richard","Thomas","Mark","Donald"};
            for (int i = 0; i < 10; i++)
            {
                customerArr[Config.indexOlderForCustomerArr++] = new Customer()
                {
                    Id = random.Next(100000000, 999999999),
                    Name = CustomersNameArr[i],
                    PhoneNumber = "0" + random.Next(50, 58) + "-" + random.Next(0000000, 9999999),
                    Longitude = (float)((float)(random.NextDouble() * (33.3 - 31)) + 31),
                    Latitude = (float)((float)(random.NextDouble() * (35.5 - 34.3)) + 34.3)
                };
            }

            //
            for (int i = 0; i < 10; i++)
            {
                parcelArr[Config.indexOlderForParcelArr++] = new Parcel()
                {
                    Id = Config.CountIdPackage++,
                    SenderId = customerArr[random.Next(0, 4)].Id,
                    TargetId = customerArr[random.Next(5, 9)].Id,
                    Weight = (WeightCategories)random.Next(0, 2),
                    Priority = (Priorities)random.Next(0, 2),
                    DroneId = 0,
                    Requested = new DateTime(2021, 7, random.Next(1, 7))
                };
                //parcelArr[i].Assigned = parcelArr[i].Requested.AddHours(1);
                //parcelArr[i].PickedUp = parcelArr[i].Assigned.AddHours(1);
                //parcelArr[i].Delivered = parcelArr[i].PickedUp.AddHours(1);
            }
        }
    }
}
