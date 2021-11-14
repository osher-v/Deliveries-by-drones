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
            /*
            if ((BaseStation.FindIndex(x => x.Id == newbaseStation.Id)) != -1)
                throw new AddAnExistingObjectException();
            DataSource.BaseStationsList.Add(newbaseStation);
            */

            AccessIdal.AddStation(newbaseStation);
        }
    }
}
