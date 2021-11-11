
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

        public double Free;
        public double LightWeightCarrier;
        public double MediumWeightBearing;
        public double CarriesHeavyWeight;
        public double DroneLoadingRate;

        public BL()
        {
            //Creates an object that will serve as an access point to methods in DAL.
            AccessIdal = new DalObject.DalObject();

            //צריכת החשמל של הרחפנים וקצב טעינתם
            double[] arr = AccessIdal.RequestPowerConsumptionByDrone();
            Free = arr[0];
            LightWeightCarrier = arr[1];
            MediumWeightBearing = arr[2];
            CarriesHeavyWeight = arr[3];
            DroneLoadingRate = arr[4];

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

            //Create a Random object to be used to draw the battery status and Location of the drones.
            Random random = new Random(DateTime.Now.Millisecond);

            //The loop will go through the dronesBL list and check if the drone is associated with the package
            //or if it does not makes a delivery and will update its status, location and battery status.
            foreach (var item in DronesBL)
            {
                // לשנות את השם לindex 
                int index = holdDalParcel.FindIndex(x => x.DroneId == item.Id);
                if (index != -1) //If the drone is indeed associated with one of the Parcels in the list.
                {
                    item.Statuses = DroneStatuses.busy; //Update drone status for shipping operation.

                    if (holdDalParcel[index].PickedUp == DateTime.MinValue)//Check if the Parcel has already been PickedUped.
                    {
                        int CustomerId = holdDalParcel[index].SenderId;
                        /*
                        Location LocationOfCustomer = new Location();
                        LocationOfCustomer.longitude = holdDalCustomer.Find(x => x.Id == CustomerId).Longitude;
                        LocationOfCustomer.latitude = holdDalCustomer.Find(x => x.Id == CustomerId).Latitude;
                        */

                        List<double> listOfDistance = new List<double>();//

                        //
                        foreach (var obj in baseStationBL)
                        {
                            listOfDistance.Add(GetDistance(CustomerBL[index].LocationOfCustomer, obj.BaseStationLocation));
                            /*
                            Location LocationOfBaseStation = new Location();
                            LocationOfBaseStation.longitude = obj.Longitude;
                            LocationOfBaseStation.latitude = obj.Latitude;
                            */
                        }

                        //מציאת המיקום של התחנה הקרובה ביותר לשולח והכנסתו למיקום הרחפן
                        item.CurrentLocation = baseStationBL[listOfDistance.FindIndex(x => x == listOfDistance.Min())].BaseStationLocation;
                    }
                    else //If the package was PickedUped.
                    { 
                        item.CurrentLocation = CustomerBL[CustomerBL.FindIndex(x => x.Id == holdDalParcel[index].SenderId)].LocationOfCustomer;    
                    }

                    // random number battery status between minimum charge to make the shipment and full charge.     
                    item.BatteryStatus = random.Next(70, 101);
                }
                else //If the drone is not associated with one of the parcels on the list and is actually available and does not ship.
                {
                    item.Statuses = (DroneStatuses)random.Next(0, 2);//Lottery drone mode between maintenance mode and free mode.

                    if (item.Statuses == DroneStatuses.inMaintenance)
                    {
                        item.CurrentLocation = baseStationBL[random.Next(0, baseStationBL.Count)].BaseStationLocation;
                        item.BatteryStatus = random.Next(0, 21);
                    }
                    else //item.Statuses == DroneStatuses.free
                    {
                        List<IDAL.DO.Parcel> DeliveredAndSameDroneID = AccessIdal.GetParcelList(i => i.Delivered != DateTime.MinValue && i.DroneId == item.Id).ToList();
                        if (!DeliveredAndSameDroneID.Any())//if the List is empty.
                        {
                            item.CurrentLocation = baseStationBL[random.Next(0, baseStationBL.Count)].BaseStationLocation;
                        }
                        else //if the List is not empty.
                        {
                            item.CurrentLocation = CustomerBL.Find(x => x.Id == DeliveredAndSameDroneID[random.Next(0, DeliveredAndSameDroneID.Count)].TargetId).LocationOfCustomer;
                        }

                        //
                        item.BatteryStatus = random.Next(55, 101);
                    }
                }
            }
            
        }

        public void AddDrone(Drone newdrone)
        {

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
