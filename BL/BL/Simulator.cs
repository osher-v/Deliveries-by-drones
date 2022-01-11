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

        enum StatusSim {Start, onGo, onCharge }
        private const int Delay = 500;
        private const double Speed = 1.0;
        private const double Step   = Speed/ TimeStep;
        private const double TimeStep = Delay / 1000.0;

        private const double kmh = 3600;

        public Simulator(BL _bl,int droneID, Action ReportProgressInSimultor, Func<bool> IsTimeRun)
        {
            DalApi.IDal AccessIdal = DalApi.DalFactory.GetDL();
            AccessIbl = _bl;
            Drone MyDrone = AccessIbl.GetDrone(droneID);
            var dal = AccessIbl;
            //area for seting puse time
            Drone drone = AccessIbl.GetDrone(droneID);
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
                            AccessIbl.AssignPackageToDdrone(MyDrone.Id);
                            ReportProgressInSimultor();
                        }
                        catch
                        {
                            if (MyDrone.BatteryStatus < 100)
                            {
                                b = MyDrone.BatteryStatus;

                                IEnumerable<BaseStation> baseStationBL = (from item in AccessIdal.GetBaseStationList()
                                                                   select new BaseStation()
                                                                   {
                                                                       Id = item.Id,
                                                                       Name = item.StationName,
                                                                       FreeChargeSlots = item.FreeChargeSlots,
                                                                       BaseStationLocation = new Location() { longitude = item.Longitude, latitude = item.Latitude },
                                                                       DroneInChargsList = new List<DroneInCharg>()
                                                                   });

                                dis = AccessIbl.minDistanceBetweenBaseStationsAndLocation(baseStationBL, MyDrone.CurrentLocation).Item2;

                                while(dis > 0)
                                {
                                    drone.BatteryStatus -= AccessIbl.Free;
                                    ReportProgressInSimultor();
                                    dis -= 1;
                                    Thread.Sleep(1000);
                                }

                                MyDrone.BatteryStatus = b;//הפונקציה שליחה לטעינה בודקת בודקת את המרחק ההתחלתי ולפי זה מחשבת את הסוללה ולכן צריך להחזיר למצב ההתחלתי
                                AccessIbl.SendingDroneforCharging(MyDrone.Id);
                                ReportProgressInSimultor();
                            }                         
                        }
                        break;
                    case DroneStatuses.inMaintenance: //

                        TimeSpan interval = DateTime.Now - AccessIdal.GetBaseCharge(droneID).StartChargeTime;
                        double horsnInCahrge = interval.Hours + (((double)interval.Minutes) / 60) + (((double)interval.Seconds) / 3600);
                        double batrryCharge = horsnInCahrge * 10000 + drone.BatteryStatus; //DroneLoadingRate == 10000

                        while (batrryCharge < 100)
                        {
                            //AccessIbl.GetDroneList().First(x => x.Id == droneID).BatteryStatus += 3; // כל שנייה הוא מתקדם ב3%
                            droneToList.BatteryStatus += 3; // כל שנייה הוא מתקדם ב3%
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
                        if (AccessIbl.GetParcel(MyDrone.Delivery.Id).PickedUp == null)
                        {
                            b = MyDrone.BatteryStatus;
                            dis = MyDrone.Delivery.TransportDistance;
                            while (dis > 1)
                            {
                                drone.BatteryStatus -= AccessIbl.Free;
                                ReportProgressInSimultor();
                                dis -= 1;
                                Thread.Sleep(1000);
                            }

                            MyDrone.BatteryStatus = b;
                            AccessIbl.PickedUpPackageByTheDrone(MyDrone.Id);
                            ReportProgressInSimultor();
                        }
                        else // PickedUp != null
                        {
                            b = MyDrone.BatteryStatus;
                            dis = MyDrone.Delivery.TransportDistance;

                            while (dis > 1)
                            {
                                switch (MyDrone.Delivery.Weight)
                                {
                                    case WeightCategories.light:
                                        drone.BatteryStatus -= AccessIbl.LightWeightCarrier;
                                        break;
                                    case WeightCategories.medium:
                                        drone.BatteryStatus -= AccessIbl.MediumWeightBearing;
                                        break;
                                    case WeightCategories.heavy:
                                        drone.BatteryStatus -= AccessIbl.CarriesHeavyWeight;
                                        break;
                                    default:
                                        break;
                                }
                                
                                ReportProgressInSimultor();
                                dis -= 1;
                                Thread.Sleep(1000);
                            }

                            MyDrone.BatteryStatus = b;
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

            switch (MyDrone.Statuses)
            {
                case DroneStatuses.free:

                    break;
                case DroneStatuses.inMaintenance:

                    break;
                case DroneStatuses.busy:

                    break;
                default:
                    break;
            }
        }
    }
}
