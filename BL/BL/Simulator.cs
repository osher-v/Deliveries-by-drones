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
        /// <summary>
        /// Simulator ctor
        /// </summary>
        /// <param name="_bl">AccessIbl</param>
        /// <param name="droneID">drone ID</param>
        /// <param name="ReportProgressInSimultor">Pointer to the function that triggers the change reporting</param>
        /// <param name="IsTimeRun">Pointer to a function that returns whether the process should close</param>
        public Simulator(BL AccessIbl, int droneID, Action ReportProgressInSimultor, Func<bool> IsTimeRun)
        {
            DalApi.IDal AccessIdal = DalApi.DalFactory.GetDL();
            var dal = AccessIbl;

            double distanse;
            double batrry;

            DroneToList droneToList = AccessIbl.GetDroneList().First(x => x.Id == droneID);

            while (!IsTimeRun()) //The process will run as long as the CancellationPending field is false
            {
                //checks the statuses of the drone and decides on the process accordingly.
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
                                batrry = droneToList.BatteryStatus;

                                IEnumerable<BaseStation> baseStationBL = (from item in AccessIdal.GetBaseStationList()
                                                                          select new BaseStation()
                                                                          {
                                                                              Id = item.Id,
                                                                              Name = item.StationName,
                                                                              FreeChargeSlots = item.FreeChargeSlots,
                                                                              BaseStationLocation = new Location() { longitude = item.Longitude, latitude = item.Latitude },
                                                                              DroneInChargsList = new List<DroneInCharg>()
                                                                          });

                                distanse = AccessIbl.minDistanceBetweenBaseStationsAndLocation(baseStationBL, droneToList.CurrentLocation).Item2;

                                while (distanse > 0)
                                {
                                    droneToList.BatteryStatus -= AccessIbl.Free;
                                    ReportProgressInSimultor();
                                    distanse -= 1;
                                    Thread.Sleep(500);
                                }

                                //The SendingDroneforCharging function checks the initial distance and calculates the
                                //battery accordingly and therefore the battery needs to be returned to the initial state.
                                droneToList.BatteryStatus = batrry;

                                AccessIbl.SendingDroneforCharging(droneID);
                                ReportProgressInSimultor();
                            }
                        }
                        break;
                    case DroneStatuses.inMaintenance:

                        while (droneToList.BatteryStatus < 100)
                        {
                            droneToList.BatteryStatus += 3; //every second is advanced by 3%

                            if (droneToList.BatteryStatus > 100)//Check if we have already passed the 100%
                            {
                                droneToList.BatteryStatus = 100;
                            }
                            ReportProgressInSimultor();
                            Thread.Sleep(500);
                        }

                        AccessIbl.ReleaseDroneFromCharging(droneID); //Release from charge as soon as the drone reaches 100%
                        ReportProgressInSimultor();

                        break;
                    case DroneStatuses.busy:
                        Drone MyDrone = AccessIbl.GetDrone(droneID);

                        if (AccessIbl.GetParcel(MyDrone.Delivery.Id).PickedUp == null)
                        {
                            batrry = droneToList.BatteryStatus;
                            Location location = new Location { longitude = droneToList.CurrentLocation.longitude, latitude = droneToList.CurrentLocation.latitude };
                            distanse = MyDrone.Delivery.TransportDistance;

                            //Calculate the progress of each step.
                            double Latitude = Math.Abs((AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer.latitude - droneToList.CurrentLocation.latitude) / distanse);
                            double longitude = Math.Abs((AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer.longitude - droneToList.CurrentLocation.longitude) / distanse);

                            while (distanse > 1)
                            {
                                droneToList.BatteryStatus -= AccessIbl.Free;
                                distanse -= 1;
                                // The function uses the coordinate and distance for each step and calculates the new position of the drone at each step
                                locationSteps(MyDrone.CurrentLocation, AccessIbl.GetCustomer(MyDrone.Delivery.Sender.Id).LocationOfCustomer, MyDrone, longitude, Latitude);
                                droneToList.CurrentLocation = MyDrone.CurrentLocation;
                                ReportProgressInSimultor();
                                Thread.Sleep(500);
                            }
                            
                            droneToList.CurrentLocation = location;
                            droneToList.BatteryStatus = batrry;
                            AccessIbl.PickedUpPackageByTheDrone(MyDrone.Id);
                            ReportProgressInSimultor();
                        }
                        else // PickedUp != null
                        {
                            batrry = droneToList.BatteryStatus;
                            Location location = new Location { longitude = droneToList.CurrentLocation.longitude, latitude = droneToList.CurrentLocation.latitude };
                            distanse = MyDrone.Delivery.TransportDistance;

                            //Calculate the progress of each step.
                            double Latitude = Math.Abs((AccessIbl.GetCustomer(MyDrone.Delivery.Receiver.Id).LocationOfCustomer.latitude - droneToList.CurrentLocation.latitude) / distanse);
                            double longitude = Math.Abs((AccessIbl.GetCustomer(MyDrone.Delivery.Receiver.Id).LocationOfCustomer.longitude - droneToList.CurrentLocation.longitude) / distanse);

                            while (distanse > 1)
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
                                // The function uses the coordinate and distance for each step and calculates the new position of the drone at each step
                                locationSteps(MyDrone.CurrentLocation, AccessIbl.GetCustomer(MyDrone.Delivery.Receiver.Id).LocationOfCustomer, MyDrone, longitude, Latitude);
                                droneToList.CurrentLocation = MyDrone.CurrentLocation;
                                ReportProgressInSimultor();
                                distanse -= 1;
                                Thread.Sleep(500);
                            }

                            droneToList.BatteryStatus = batrry;
                            droneToList.CurrentLocation = location;
                            AccessIbl.DeliveryPackageToTheCustomer(MyDrone.Id);
                            ReportProgressInSimultor();
                        }
                        break;
                    default:
                        break;
                }
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
        private void locationSteps(Location locationOfDrone, Location locationOfNextStep, Drone myDrone, double lon, double lat)
        {
            double droneLatitude = locationOfDrone.latitude;
            double droneLongitude = locationOfDrone.longitude;

            double nextStepLatitude = locationOfNextStep.latitude;
            double nextStepLongitude = locationOfNextStep.longitude;

            //Calculate the latitude of the new location.
            if (droneLatitude < nextStepLatitude)
            {
                myDrone.CurrentLocation.latitude += lat;//If the score of the source is smaller than the target score, an addition must be made at each step.
            }
            else
            {
                myDrone.CurrentLocation.latitude -= lat;// otherwise a subtraction must be made at each step.
            }

            //Calculate the Longitude of the new location.
            if (droneLongitude < nextStepLongitude)//If the score of the source is smaller than the target score, an addition must be made at each step.
            {
                myDrone.CurrentLocation.longitude += lon;
            }
            else// otherwise a subtraction must be made at each step.
            {
                myDrone.CurrentLocation.longitude -= lon;
            }
        }
    }
}
