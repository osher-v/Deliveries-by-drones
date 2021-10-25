using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;

namespace DalObject
{
    /// <summary>
    /// Contains boot data and list structure
    /// </summary>
    public class DataSource 
    {
        /// <summary> A static Random that sets the random to select a millisecond to repel collisions </summary>
        internal static Random random = new Random(DateTime.Now.Millisecond);

        #region List of stracts
        /// <summary> list of drones </summary>
        internal static List<Drone> droneList = new List<Drone>();
        /// <summary> list of base stations </summary>
        internal static List<BaseStation> baseStationsList = new List<BaseStation>();
        /// <summary>list of customers</summary>
        internal static List<Customer> customersList = new List<Customer>();
        /// <summary> list of parcels </summary>
        internal static List<Parcel> parcelsList = new List<Parcel>();
        /// <summary> class that responsible for counters </summary>
        internal static List<DroneCharge> droneChargeList = new List<DroneCharge>();
        #endregion List of stracts

        internal class Config 
        {
        /// <summary> Continuous number For the package </summary>
            public static int CountIdPackage = 1;
        }

        /// <summary> responsible for initializing all entities </summary>
        public static void Initialize()
        {
            #region init BaseStation
            // we chose to Initialize manual the Base Station
            // becuse its just 2 and we want the landmarks of our homes.
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
            #endregion init BaseStation

            #region init Drone
            //initialization of 5 Drones with different and random values.
            // We wanted a way to initialize types efficiently in a loop and chose to create an array of names from which to be randomly selected.
            string[] modelNameArr = new string[5] { "I", "IX", "IIX", "VI", "IL" }; 
            for (int i = 0; i < 5; i++)
            {
                droneList.Add( new Drone {
                    Id = random.Next(100000000, 999999999),
                    Model = modelNameArr[i],
                    MaxWeight = (WeightCategories)random.Next(0, 3),//0=light,1=medium,2=heavy
                    Battery = random.Next(50, 100),
                    Status = (DroneStatuses)random.Next(0, 3)//0=free, 1=inMaintenance, 2=busy
                });
            }
            #endregion init Drone

            #region init Customer
            //initialization of 10 Customers with different and random values.
            string[] CustomersNameArr = new string[10]{"James","Robert","John","Michael","William",
                   "David","Richard","Thomas","Mark","Donald"};
            for (int i = 0; i < 10; i++)
            {
                customersList.Add(new Customer{
                    Id = random.Next(100000000, 999999999),
                    Name = CustomersNameArr[i],
                    PhoneNumber = "0" + random.Next(50, 58) + "-" + random.Next(0000000, 9999999),
                    Longitude = (float)((float)(random.NextDouble() * (33.3 - 31)) + 31),// get israel range 
                    Latitude = (float)((float)(random.NextDouble() * (35.5 - 34.3)) + 34.3)//get israel range 
                });
            }
            #endregion init Customer

            #region init Parcel
            //initialization of 10 Parcels with different and random values
            for (int i = 0; i < 10; i++)
            {
                parcelsList.Add( new Parcel(){
                    Id = Config.CountIdPackage++,
                    SenderId = customersList[random.Next(0, 5)].Id,// we choose from the list that we alrade have 
                    TargetId = customersList[random.Next(5, 10)].Id,// we choose from the list that we alrade have  but not the same as before 
                    Weight = (WeightCategories)random.Next(0, 3),//0=light,1=medium,2=heavy
                    Priority = (Priorities)random.Next(0, 3),//0=regular, 1=fast, 2=urgent
                    DroneId = 0,
                    Requested = new DateTime(2021, 10, random.Next(1, 7))
                    //Requested = DateTime.Now.AddDays(random.Next(-5, 1))
                });
            }
            #endregion init Parcel
        }
    }
}
