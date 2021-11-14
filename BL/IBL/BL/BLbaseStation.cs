using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    //public partial class BLbaseStation
    public partial class BL
    {
        public void AddStation(BaseStation newbaseStation)
        {
            IDAL.DO. BaseStation newStation =new IDAL.DO.BaseStation() { Id=newbaseStation.Id, StationName=newbaseStation.Name, 
                FreeChargeSlots=newbaseStation.FreeChargeSlots,Longitude=newbaseStation.BaseStationLocation.longitude,
                Latitude = newbaseStation.BaseStationLocation.latitude} ;
            try
            {
                AccessIdal.AddStation(newStation);
            }
            catch {  }
        }

    }
}
