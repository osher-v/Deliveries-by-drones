using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

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
            };

        }
    }
}
