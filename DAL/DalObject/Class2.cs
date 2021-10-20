using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;

namespace DalObject
{
    public class DalObject
    {
        DalObject()
        {
            DataSource.Initialize();
        }
        public static BaseStation GetBaseStation(int ID)
        {
            for (int i = 0; i <DataSource.Config.indexOlderForBaseStationArr ; i++)
            {
                if ((ID == DataSource.baseStationArr[i].Id))
                    return DataSource.baseStationArr[i];
            }
            return DataSource.baseStationArr[1];
        }

    }
}
