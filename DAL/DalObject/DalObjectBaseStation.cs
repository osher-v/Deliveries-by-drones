using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
using IDal;

namespace DalObject
{
    public partial class DalObject : IDal.IDal
    {
        public void AddStation(BaseStation newbaseStation)
        {
            if ((DataSource.BaseStationsList.FindIndex(x => x.Id == newbaseStation.Id)) != -1)
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            DataSource.BaseStationsList.Add(newbaseStation);
        }
        public BaseStation GetBaseStation(int ID)
        {
            if ((DataSource.BaseStationsList.FindIndex(x => x.Id == ID)) == -1)
                throw new NonExistentObjectException();
            return DataSource.BaseStationsList.Find(x => x.Id == ID);
        }
        /// <summary>
        /// The function returns an array of all base stations.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<BaseStation> GetBaseStationList()
        {
            return DataSource.BaseStationsList.Take(DataSource.BaseStationsList.Count).ToList();
        }


    }
}
