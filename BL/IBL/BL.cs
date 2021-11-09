
//using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;

using IBL.BO;

namespace IBL
{
    public class BL: IBL
    {
        public IDal.IDal AccessIdal;

        public List<DroneToList> Drones;

        public static double Free;
        public static double LightWeightCarrier;
        public static double MediumWeightBearing;
        public static double CarriesHeavyWeight;
        public static double DroneLoadingRate;

        public BL()
        {
            //IDal.IDal idal = new DalObject.DalObject();
            AccessIdal = new DalObject.DalObject();

            Drones = new List<DroneToList>();

            double[] arr = AccessIdal.RequestPowerConsumptionByDrone();
            //למלא את השדות לפי המערך

            List<IDAL.DO.Drone> drones = AccessIdal.GetDroneList().ToList();

            DroneToList temp = new DroneToList();

            for (int i = 0; i < drones.Count; i++)
            {
                temp.Id = drones[i].Id;
                temp.Model = drones[i].Model;
                temp.MaxWeight = (WeightCategories)drones[i].MaxWeight;

                Drones.Add(temp);

            }

            List<IDAL.DO.Parcel> Parcels = AccessIdal.GetParcelList(i => i.DroneId != 0 && i.Delivered == DateTime.MinValue).ToList();

            foreach (var item in Parcels)
            {
                
                Drones.Find(x => x.Id == item.DroneId).Statuses = DroneStatuses.busy;

                if(item.PickedUp == DateTime.MinValue)
                {
                    foreach (var item in collection)
                    {

                    }
                }

            }


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
        public static double GetDistance(Location senderLocation, Location BaseStationLocation)
        {
            //For the calculation we calculate the earth into a circle (ellipse) Divide its 360 degrees by half
            //180 for each longitude / latitude and then make a pie on each half to calculate the radius for
            //the formula below
            var num1 = senderLocation.longitude * (Math.PI / 180.0);
            var d1 = senderLocation.latitude * (Math.PI / 180.0);
            var num2 = BaseStationLocation.longitude * (Math.PI / 180.0) - num1;
            var d2 = BaseStationLocation.latitude * (Math.PI / 180.0);

            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0); //https://iw.waldorf-am-see.org/588999-calculating-distance-between-two-latitude-QPAAIP
                                                                                                                                   //We calculate the distance according to a formula that
                                                                                                                                   // also takes into account the curvature of the earth
            return (double)(6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))));
        }
        #endregion Function of calculating distance between points (Bonus)
    }
}



/*
                int index = Parcels.FindIndex(x => x.DroneId == item.Id);
                //if (item.Id == Parcels.Exists())
                if(index != -1)
                {
                    item.Statuses = DroneStatuses.busy; 
                    if(Parcels[index].PickedUp == DateTime.MinValue)
                    {
                       // item.CurrentLocation = 



                    }
                }
                */
