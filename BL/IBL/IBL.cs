using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IBL;

namespace IBL
{
    public interface IBL
    {
        void AddStation(BaseStation newbaseStation);
        void AddDrone(DroneToList newdrone, int firstChargingStation);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newParcel);
    }
}
