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
        private const int Delay = 1000;
        private const double Speed = 1.0;
        private const double Step   = Speed/ TimeStep;
        private const double TimeStep = Delay / 1000.0;


        public Simulator(BL _bl,int droneID, Action ReportProgressInSimultor, Func<bool> IsTimeRun)
        {
            DalApi.IDal AccessIdal = DalApi.DalFactory.GetDL();
            AccessIbl = _bl;
            Drone MyDrone = AccessIbl.GetDrone(droneID);
            var dal = AccessIbl;
            //area for seting puse time
            Drone drone = AccessIbl.GetDrone(droneID);

            while (!IsTimeRun())
            {
                switch (MyDrone.Statuses)
                {
                    case DroneStatuses.free:
                        try
                        {
                            AccessIbl.AssignPackageToDdrone(MyDrone.Id);
                        }
                        catch
                        {
                            if (MyDrone.BatteryStatus < 100)
                            {
                                double b = MyDrone.BatteryStatus;

                                AccessIbl.d




                                AccessIbl.SendingDroneforCharging(MyDrone.Id);
                            }
                        }
                        break;
                    case DroneStatuses.inMaintenance:

                        TimeSpan interval = DateTime.Now - AccessIdal.GetBaseCharge(droneID).StartChargeTime;
                        double horsnInCahrge = interval.Hours + (((double)interval.Minutes) / 60) + (((double)interval.Seconds) / 3600);
                        double batrryCharge = horsnInCahrge * 10000 + drone.BatteryStatus; //DroneLoadingRate == 10000

                        while (batrryCharge < 100)
                        {
                            AccessIbl.GetDroneList().First(x => x.Id == droneID).BatteryStatus += 3; // כל שנייה הוא מתקדם ב3%
                            ReportProgressInSimultor();
                            Thread.Sleep(1500);
                        }

                        AccessIbl.ReleaseDroneFromCharging(MyDrone.Id);
                        //if (MyDrone.BatteryStatus<100)
                        //AccessIbl.SendingDroneforCharging(MyDrone.Id);
                        break;
                    case DroneStatuses.busy:
                        if (AccessIbl.GetParcel(MyDrone.Delivery.Id).PickedUp == null)
                        {
                            AccessIbl.PickedUpPackageByTheDrone(MyDrone.Id);
                        }
                        else
                        {
                            AccessIbl.DeliveryPackageToTheCustomer(MyDrone.Id);
                        }
                        break;
                    default:
                        break;
                }
                ReportProgressInSimultor();
                Thread.Sleep(1500);
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
