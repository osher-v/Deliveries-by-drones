using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDal
{
    public interface IDal
    {
        double[] RequestPowerConsumptionByDrone();

        IEnumerable<Drone> GetDroneList();
        IEnumerable<Parcel> GetParcelList(Predicate<Parcel> prdicat = null);
    }
}
