using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;
using DalFacade;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        public void AddStation(BaseStation newbaseStation)
        {
            if ((DataSource.BaseStationsList.FindIndex(x => x.Id == newbaseStation.Id)) != -1)
                throw new AddAnExistingObjectException();
            DataSource.BaseStationsList.Add(newbaseStation);
        }

        public void UpdateBaseStation(BaseStation newBaseStation)
        {
            if (!(DataSource.BaseStationsList.Exists(x => x.Id == newBaseStation.Id)))
                throw new NonExistentObjectException();
            DataSource.BaseStationsList[DataSource.BaseStationsList.FindIndex(x => x.Id == newBaseStation.Id)] = newBaseStation;
        }

        public void UpdateMinusChargeSlots(int baseStationId)
        {
            //BaseStation update.
            int indexaforBaseStationId = DataSource.BaseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots--;
            DataSource.BaseStationsList[indexaforBaseStationId] = temp;
        }

        public void UpdatePluseChargeSlots(int baseStationId)
        {
            //BaseStation update.
            int indexaforBaseStationId = DataSource.BaseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots++;
            DataSource.BaseStationsList[indexaforBaseStationId] = temp;
        }

        public BaseStation GetBaseStation(int ID)
        {
            if (!(DataSource.BaseStationsList.Exists(x => x.Id == ID)))
                throw new NonExistentObjectException();
            return DataSource.BaseStationsList.Find(x => x.Id == ID);
        }
   
        public IEnumerable<BaseStation> GetBaseStationList(Predicate<BaseStation> predicate = null)
        {
            return DataSource.BaseStationsList.FindAll(x => predicate == null ? true : predicate(x));  
        }      
    }
}
