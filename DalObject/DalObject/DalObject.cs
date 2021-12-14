using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;
using DalFacade;

namespace DalObject
{
    /// <summary>
    /// matods that use from the main 
    /// </summary>
    public partial class DalObject : IDal 
    {
        /// <summary>
        /// Default constructor.
        /// </summary>  
        #region Singelton
        static DalObject() { }// static ctor to ensure instance init is done just before first usage

        private DalObject() // default => private  
        {
            DataSource.Initialize();
        }

        internal static DalObject Instance { get; } = new DalObject();// The public Instance property to use
        #endregion Singelton

        public double[] RequestPowerConsumptionByDrone()
        {
            double[] temp = { DataSource.Config.Free, DataSource.Config.LightWeightCarrier,
            DataSource.Config.MediumWeightBearing, DataSource.Config.CarriesHeavyWeight,
                DataSource.Config.DroneLoadingRate};
            return temp;
        }

        #region Function of calculating distance between points (Bonus)
        /// <summary>
        /// A function that calculates the distance between points
        /// </summary>
        /// <param name="longitude">The new longitude</param>
        /// <param name="latitude">The new latitude</param>
        /// <param name="ID">the ID for the reqsted objact</param>
        /// <param name="yourChoise">the choise for the objact (to know the list that we need)</param>
        /// <returns>the distence between the points</returns>
        public static double GetDistance(double longitude, double latitude, int ID ,int yourChoise)
        {
            double otherLongitude, otherLatitude;
            int index;

            if (yourChoise == 1)//BaseStation point
            {
                index = DataSource.BaseStationsList.FindIndex(x => x.Id == ID);
                otherLongitude = DataSource.BaseStationsList[index].Longitude;
                otherLatitude = DataSource.BaseStationsList[index].Latitude;
            }
            else if (yourChoise == 2)//customer point
            {
                index = DataSource.CustomersList.FindIndex(x => x.Id == ID);
                otherLongitude = DataSource.CustomersList[index].Longitude;
                otherLatitude = DataSource.CustomersList[index].Latitude;
            }
            else return -1;

            //For the calculation we calculate the earth into a circle (ellipse) Divide its 360 degrees by half
            //180 for each longitude / latitude and then make a pie on each half to calculate the radius for
            //the formula below
            var num1 = longitude * (Math.PI / 180.0); 
            var d1 = latitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0); //https://iw.waldorf-am-see.org/588999-calculating-distance-between-two-latitude-QPAAIP
                                                                                                                                   //We calculate the distance according to a formula that
                                                                                                                                   // also takes into account the curvature of the earth
            return (double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }
        #endregion Function of calculating distance between points (Bonus)
    }
}


