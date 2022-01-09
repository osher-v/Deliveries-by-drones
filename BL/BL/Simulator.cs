using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;

namespace BL
{
    /// <summary>
    /// drone simulator.
    /// </summary>
    class Simulator
    {
        BlApi.IBL AccessIbl;

        enum StatusSim {Start, onGo, onCharge }
        private const int Delay = 1000;
        private const double Speed = 1.0;
        private const double Step   = Speed/ TimeStep;
        private const double TimeStep = Delay / 1000.0;


        public Simulator(BlApi.IBL _bl,int droneID, Action action, Func<bool> func)
        {
            AccessIbl = _bl;
            Drone MyDrone = AccessIbl.GetDrone(droneID);
            var dal = AccessIbl;
            //area for seting puse time
            Drone drone = AccessIbl.GetDrone(droneID);

            while (true)//isTimeRun)
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
                                AccessIbl.SendingDroneforCharging(MyDrone.Id);
                        }
                        break;
                    case DroneStatuses.inMaintenance:
                        AccessIbl.ReleaseDroneFromCharging(MyDrone.Id);
                        if (MyDrone.BatteryStatus < 100)
                            AccessIbl.SendingDroneforCharging(MyDrone.Id);
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
                //DroneSimultor.ReportProgress(1);?????????????????????????????
                Thread.Sleep(1500);
            }
        }
    }
}
