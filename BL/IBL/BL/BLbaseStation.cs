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
            IDAL.DO.BaseStation newStation = new IDAL.DO.BaseStation()
            {
                Id = newbaseStation.Id,
                StationName = newbaseStation.Name,
                FreeChargeSlots = newbaseStation.FreeChargeSlots,
                Longitude = newbaseStation.BaseStationLocation.longitude,
                Latitude = newbaseStation.BaseStationLocation.latitude
            };
            try
            {
                AccessIdal.AddStation(newStation);
            }
            catch (IDAL.DO.AddAnExistingObjectException ex)
            {
                throw new Exception("", ex);
            }
        }

        public void UpdateBaseStaison(int baseStationId, string baseName, int chargeslots)
        {
            try
            {
                IDAL.DO.BaseStation newbase = AccessIdal.GetBaseStation(baseStationId);
                if (baseName != "")
                {
                    newbase.StationName = baseName;
                }

                //if (!(chargeslots==0))
                //{
                //    IDAL.DO.BaseStation newbase = AccessIdal.GetBaseStation(baseStationId);
                //    newbase.StationName = baseName;
                //    AccessIdal.UpdateBaseStation(newbase);
                //}

                AccessIdal.UpdateBaseStation(newbase);
            }
            catch { }

        }

    }
}
