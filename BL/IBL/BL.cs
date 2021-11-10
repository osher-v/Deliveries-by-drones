
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

        public List<DroneToList> DronesBL;

        public static double Free;
        public static double LightWeightCarrier;
        public static double MediumWeightBearing;
        public static double CarriesHeavyWeight;
        public static double DroneLoadingRate;

        public BL()
        {
            //IDal.IDal idal = new DalObject.DalObject();
            AccessIdal = new DalObject.DalObject();

            double[] arr = AccessIdal.RequestPowerConsumptionByDrone();
            //למלא את השדות לפי המערך

            //המרת מערך הרחפנים של שכבת הנתונים למערך רחפנים של השכבה הלוגית
            DronesBL = new List<DroneToList>();
            List<IDAL.DO.Drone> holdDalDrones = AccessIdal.GetDroneList().ToList();
            foreach (var item in holdDalDrones)
            {
                DronesBL.Add(new DroneToList { Id = item.Id, Model = item.Model,
                     MaxWeight = (WeightCategories)item.MaxWeight });
            }

            //יצירת רשימת לקוחות משכבת הנתונים
            List<Customer> CustomerBL = new List<Customer>();
            List<IDAL.DO.Customer> holdDalCustomer = AccessIdal.GetCustomerList().ToList();
            foreach (var item in holdDalCustomer)
            {
                Location LocationOfItem = new Location() { longitude = item.Longitude, latitude = item.Latitude };
                CustomerBL.Add(new Customer { Id = item.Id, Name = item.Name, PhoneNumber
                = item.PhoneNumber, LocationOfCustomer = LocationOfItem});
            }

            //יצירת רשימת תחנות בסיס משכבת הנתונים
            List<BaseStation> baseStationBL = new List<BaseStation>();
            List <IDAL.DO.BaseStation> holdDalBaseStation = AccessIdal.GetBaseStationList().ToList();
            foreach (var item in holdDalBaseStation)
            {
                Location LocationOfItem = new Location() { longitude = item.Longitude, latitude = item.Latitude };
                baseStationBL.Add(new BaseStation { Id = item.Id, Name = item.StationName,
                    FreeChargeSlots = item.FreeChargeSlots, BaseStationLocation = LocationOfItem});
            }
            //יצירת רשימת חבילות עם תנאי משכבת הנתונים
            List<IDAL.DO.Parcel> holdDalParcel = AccessIdal.GetParcelList(i => i.DroneId != 0 && i.Delivered == DateTime.MinValue).ToList();

            /*
            foreach (var item in holdDalParcel)
            {
                DronesBL.Find(x => x.Id == item.DroneId).Statuses = DroneStatuses.busy;
            }
            */
            Random random = new Random(DateTime.Now.Millisecond);

            foreach (var item in DronesBL)
            {
                //if(holdDalParcel.Exists(x => x.DroneId == item.Id))
                int index = holdDalParcel.FindIndex(x => x.DroneId == item.Id);
                if (index != -1)
                {
                    item.Statuses = DroneStatuses.busy;

                    if(holdDalParcel[index].PickedUp == DateTime.MinValue)
                    {
                        int CustomerId = holdDalParcel[index].SenderId;

                        Location LocationOfCustomer = new Location();
                        LocationOfCustomer.longitude = holdDalCustomer.Find(x => x.Id == CustomerId).Longitude;
                        LocationOfCustomer.latitude = holdDalCustomer.Find(x => x.Id == CustomerId).Latitude;

                        List<double> listOfDistance = new List<double>();

                        foreach (var obj in holdDalBaseStation)
                        {
                            
                            Location LocationOfBaseStation = new Location();
                            LocationOfBaseStation.longitude = obj.Longitude;
                            LocationOfBaseStation.latitude = obj.Latitude;

                            listOfDistance.Add(GetDistance(LocationOfCustomer, LocationOfBaseStation));
                        }

                        //מציאת המיקום של התחנה הקרובה ביותר לשולח והכנסתו למיקום הרחפן
                        item.CurrentLocation = baseStationBL[listOfDistance.FindIndex(x => x == listOfDistance.Min())].BaseStationLocation;
                    }
                    else
                    { 
                        item.CurrentLocation = CustomerBL[CustomerBL.FindIndex(x => x.Id == holdDalParcel[index].SenderId)].LocationOfCustomer;    
                    }

                    
                    item.BatteryStatus = random.Next(70, 101);
                }
                else
                {
                    item.Statuses = (DroneStatuses)random.Next(0, 2);
                    if(item.Statuses == DroneStatuses.inMaintenance)
                    {
                        item.CurrentLocation = baseStationBL[random.Next(0, baseStationBL.Count)].BaseStationLocation;
                        item.BatteryStatus = random.Next(0, 21);
                    }
                    else //item.Statuses == DroneStatuses.free
                    {
                        List<IDAL.DO.Parcel> holdDalParcelWhoDelivered = AccessIdal.GetParcelList(i => i.Delivered != DateTime.MinValue).ToList();
                        List<Customer> holdCustomer = new List<Customer>();
                        holdCustomer.Add(holdDalParcelWhoDelivered)

                        item.BatteryStatus = random.Next(55, 101);
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
