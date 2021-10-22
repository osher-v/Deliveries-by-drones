using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
using System.ComponentModel;

////////////////int index = baseStationsList.FindIndex(i => i.Id == 5);
//exemmple//////BaseStation u = baseStationsList.Find(i => i.Id == 5);
////////////////baseStationsList.ForEach(i => Console.WriteLine(i));
namespace DalObject
{
    /// <summary>
    ///  
    /// </summary>
    public static class DataSource
    {
        internal static Random random = new Random(DateTime.Now.Millisecond);
        //~~~~//
        internal static List<Drone> droneList = new List<Drone>();
        internal static List<BaseStation> baseStationsList = new List<BaseStation>();
        internal static List<Customer> customersList = new List<Customer>();
        internal static List<Parcel> parcelsList = new List<Parcel>();

        /// <summary>
        /// 
        /// </summary>
        internal class Config
        {
            public static int CountIdPackage = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Initialize()
        {
            //
            baseStationsList.Add(new BaseStation {
                Id = random.Next(100000000, 999999999),
                StationName = "BnyBrak",
                FreeChargeSlots = random.Next(5, 10),
                Longitude = 32.086456,
                Latitude = 34.844476
            });

            baseStationsList.Add (new BaseStation {
                Id = random.Next(100000000, 999999999),
                StationName = "Holon",
                FreeChargeSlots = random.Next(5, 10),
                Longitude = 32.021679,
                Latitude = 34.789990
            });

            string[] modelNameArr = new string[5] { "I", "IX", "IIX", "VI", "IL" };
            for (int i = 0; i < 5; i++)
            {
                droneList.Add( new Drone {
                    Id = random.Next(100000000, 999999999),
                    Model = modelNameArr[i],
                    MaxWeight = (WeightCategories)random.Next(0, 3),
                    Battery = random.Next(50, 100),
                    Status = (DroneStatuses)random.Next(0, 3)
                });
            }

            string[] CustomersNameArr = new string[10]{"James","Robert","John","Michael","William",
                   "David","Richard","Thomas","Mark","Donald"};
            for (int i = 0; i < 10; i++)
            {
                customersList.Add(new Customer{
                    Id = random.Next(100000000, 999999999),
                    Name = CustomersNameArr[i],
                    PhoneNumber = "0" + random.Next(50, 58) + "-" + random.Next(0000000, 9999999),
                    Longitude = (float)((float)(random.NextDouble() * (33.3 - 31)) + 31),
                    Latitude = (float)((float)(random.NextDouble() * (35.5 - 34.3)) + 34.3)
                });
            }

            //
            for (int i = 0; i < 10; i++)
            {
                parcelsList.Add( new Parcel(){
                    Id = Config.CountIdPackage++,
                    SenderId = customersList[random.Next(0, 5)].Id,
                    TargetId = customersList[random.Next(5, 10)].Id,
                    Weight = (WeightCategories)random.Next(0, 3),
                    Priority = (Priorities)random.Next(0, 3),
                    DroneId = 0,
                    Requested = new DateTime(2021, 7, random.Next(1, 7))
                });
                //parcelArr[i].Assigned = parcelArr[i].Requested.AddHours(1);
                //parcelArr[i].PickedUp = parcelArr[i].Assigned.AddHours(1);
                //parcelArr[i].Delivered = parcelArr[i].PickedUp.AddHours(1);
            }
        }
    }
}
