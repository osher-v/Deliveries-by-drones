using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;

namespace BL
{
    /// <summary>
    /// drone simulator.
    /// </summary>
    class Simulator
    {
        BL AccessIbl;

        private const double kmh = 3600;//כל קילומטר זה שנייה כי בשעה יש 3600 שניות

        public Simulator(BL _bl,int droneID, Action ReportProgressInSimultor, Func<bool> IsTimeRun)
        {
            DalApi.IDal AccessIdal = DalApi.DalFactory.GetDL();

            AccessIbl = _bl;
            
            var dal = AccessIbl;
            
            double dis;
            double b;

            DroneToList droneToList = AccessIbl.GetDroneList().First(x => x.Id == droneID);

            while (!IsTimeRun())
            {
                switch (droneToList.Statuses)
                {
                    case DroneStatuses.free:
                        try
                        {
                            AccessIbl.AssignPackageToDdrone(droneID);
                            ReportProgressInSimultor();
                        }
                        catch
                        {
                            if (droneToList.BatteryStatus < 100)
                            {
                                b = droneToList.BatteryStatus;

                                IEnumerable<BaseStation> baseStationBL = (from item in AccessIdal.GetBaseStationList()
                                                                   select new BaseStation()
                                                                   {
                                                                       Id = item.Id,
                                                                       Name = item.StationName,
                                                                       FreeChargeSlots = item.FreeChargeSlots,
                                                                       BaseStationLocation = new Location() { longitude = item.Longitude, latitude = item.Latitude },
                                                                       DroneInChargsList = new List<DroneInCharg>()
                                                                   });

                                dis = AccessIbl.minDistanceBetweenBaseStationsAndLocation(baseStationBL, droneToList.CurrentLocation).Item2;

                                while(dis > 0)
                                {
                                    droneToList.BatteryStatus -= AccessIbl.Free;
                                    ReportProgressInSimultor();
                                    dis -= 1;
                                    Thread.Sleep(1000);
                                }

                                droneToList.BatteryStatus = b;//הפונקציה שליחה לטעינה בודקת בודקת את המרחק ההתחלתי ולפי זה מחשבת את הסוללה ולכן צריך להחזיר למצב ההתחלתי
                                AccessIbl.SendingDroneforCharging(droneID);
                                ReportProgressInSimultor();
                            }                         
                        }
                        break;
                    case DroneStatuses.inMaintenance: //

                        TimeSpan interval = DateTime.Now - AccessIdal.GetBaseCharge(droneID).StartChargeTime;
                        double horsnInCahrge = interval.Hours + (((double)interval.Minutes) / 60) + (((double)interval.Seconds) / 3600);
                        double batrryCharge = horsnInCahrge * 10000 + droneToList.BatteryStatus; //DroneLoadingRate == 10000

                        while (batrryCharge < 100)
                        {
                            //AccessIbl.GetDroneList().First(x => x.Id == droneID).BatteryStatus += 3; // כל שנייה הוא מתקדם ב3%
                            droneToList.BatteryStatus += 3; // כל שנייה הוא מתקדם ב3%
                            batrryCharge += 3;
                            if (droneToList.BatteryStatus > 100)//בדיקה אם כבר עברנו את ה100%
                            {
                                droneToList.BatteryStatus = 100;
                            }
                            ReportProgressInSimultor();
                            Thread.Sleep(1000);
                        }

                        AccessIbl.ReleaseDroneFromCharging(droneID); //שחרור מטעינה ברגע שהרחפן מגיע ל100
                        ReportProgressInSimultor();

                        break;
                    case DroneStatuses.busy:
                        Drone MyDrone = AccessIbl.GetDrone(droneID);

                        if (AccessIbl.GetParcel(MyDrone.Delivery.Id).PickedUp == null)
                        {
                            b = droneToList.BatteryStatus;
                            Location d = new Location { longitude = droneToList.CurrentLocation.longitude, latitude = droneToList.CurrentLocation.latitude };
                            dis = MyDrone.Delivery.TransportDistance;

                            double Latitude = Math.Abs((AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer.latitude - droneToList.CurrentLocation.latitude) / dis);
                            double longitude = Math.Abs((AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer.longitude - droneToList.CurrentLocation.longitude) / dis);

                            while (dis > 1)
                            {
                                droneToList.BatteryStatus -= AccessIbl.Free;
                                dis -= 1;
                                locationSteps(MyDrone.CurrentLocation, AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer, MyDrone, longitude, Latitude);
                                droneToList.CurrentLocation = MyDrone.CurrentLocation;
                                ReportProgressInSimultor();
                                Thread.Sleep(1000);
                            }
                            droneToList.CurrentLocation = d;
                            droneToList.BatteryStatus = b;
                            AccessIbl.PickedUpPackageByTheDrone(MyDrone.Id);
                            ReportProgressInSimultor();
                        }
                        else // PickedUp != null
                        {
                            b = droneToList.BatteryStatus;
                            Location d = new Location { longitude = droneToList.CurrentLocation.longitude, latitude = droneToList.CurrentLocation.latitude };

                            dis = MyDrone.Delivery.TransportDistance;

                            double Latitude = Math.Abs((AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer.latitude - AccessIbl.GetCustomer(MyDrone.Delivery.Receiver.Id).LocationOfCustomer.latitude) / dis);
                            double longitude = Math.Abs((AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer.longitude - AccessIbl.GetCustomer(MyDrone.Delivery.Receiver.Id).LocationOfCustomer.longitude) / dis);

                            while (dis > 1)
                            {
                                switch (MyDrone.Delivery.Weight)
                                {
                                    case WeightCategories.light:
                                        droneToList.BatteryStatus -= AccessIbl.LightWeightCarrier;
                                        break;
                                    case WeightCategories.medium:
                                        droneToList.BatteryStatus -= AccessIbl.MediumWeightBearing;
                                        break;
                                    case WeightCategories.heavy:
                                        droneToList.BatteryStatus -= AccessIbl.CarriesHeavyWeight;
                                        break;
                                    default:
                                        break;
                                }
                                locationSteps(MyDrone.CurrentLocation, AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer, MyDrone, longitude, Latitude);
                                droneToList.CurrentLocation = MyDrone.CurrentLocation;
                                ReportProgressInSimultor();
                                dis -= 1;
                                Thread.Sleep(1000);
                            }

                            droneToList.BatteryStatus = b;
                            droneToList.CurrentLocation = d;
                            AccessIbl.DeliveryPackageToTheCustomer(MyDrone.Id);
                            ReportProgressInSimultor();
                        }
                        break;
                    default:
                        break;
                }
                //ReportProgressInSimultor();
                Thread.Sleep(1000);
            }     
        }

        /// <summary>
        /// The function takes the location of the drone and the target location and determines the
        /// location of the drone relative to the progress.
        /// </summary>
        /// <param name="locationOfDrone">location of drone</param>
        /// <param name="locationOfNextStep">location of next step</param>
        /// <param name="myDrone">drone</param>
        //private void locationSteps(Location locationOfDrone , Location locationOfNextStep, Drone myDrone) 
        private void locationSteps(Location locationOfDrone, Location locationOfNextStep, Drone myDrone,double lon, double lat)
        {
            double droneLatitude = locationOfDrone.latitude;
            double droneLongitude = locationOfDrone.longitude;

            double nextStepLatitude = locationOfNextStep.latitude;
            double nextStepLongitude = locationOfNextStep.longitude;

            //Calculate the latitude of the new location.
            if (droneLatitude < nextStepLatitude)// ++++++
            {
                //double step = (nextStepLatitude - droneLatitude) / myDrone.Delivery.TransportDistance;
                //myDrone.CurrentLocation.latitude += (nextStepLatitude - droneLatitude) / myDrone.Delivery.TransportDistance;
                myDrone.CurrentLocation.latitude += lat;
            }
            else
            {
                //double step = (  droneLatitude - nextStepLatitude) / myDrone.Delivery.TransportDistance;
                //myDrone.CurrentLocation.latitude -= (droneLatitude - nextStepLatitude) / myDrone.Delivery.TransportDistance;
                myDrone.CurrentLocation.latitude -= lat;
            }

            //Calculate the Longitude of the new location.
            if (droneLongitude < nextStepLongitude)//+++++++
            {
                // double step = (nextStepLongitude - droneLongitude) / myDrone.Delivery.TransportDistance;
                //myDrone.CurrentLocation.longitude += (nextStepLongitude - droneLongitude) / myDrone.Delivery.TransportDistance;
                myDrone.CurrentLocation.longitude += lon;
            }
            else
            {
                //double step = (droneLongitude - nextStepLongitude) / myDrone.Delivery.TransportDistance;
                //myDrone.CurrentLocation.longitude -= (droneLongitude - nextStepLongitude) / myDrone.Delivery.TransportDistance;
                myDrone.CurrentLocation.longitude -= lon;
            }
        }
    }
}
