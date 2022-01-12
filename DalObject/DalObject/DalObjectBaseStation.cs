using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using DO;
using DalApi;

namespace DalObject
{
    partial class DalObject : IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(BaseStation newbaseStation)
        {
            if (DataSource.BaseStationsList.Exists(x => x.Id == newbaseStation.Id))
            {
                throw new AddAnExistingObjectException();
            }
            DataSource.BaseStationsList.Add(newbaseStation); //else
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(BaseStation newBaseStation)
        {
            if (!DataSource.BaseStationsList.Exists(x => x.Id == newBaseStation.Id))
            {
                throw new NonExistentObjectException();
            }
            DataSource.BaseStationsList[DataSource.BaseStationsList.FindIndex(x => x.Id == newBaseStation.Id)] = newBaseStation; //else
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateMinusChargeSlots(int baseStationId)
        {
            //BaseStation update.
            int indexaforBaseStationId = DataSource.BaseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots--;
            DataSource.BaseStationsList[indexaforBaseStationId] = temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePluseChargeSlots(int baseStationId)
        {
            //BaseStation update.
            int indexaforBaseStationId = DataSource.BaseStationsList.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = DataSource.BaseStationsList[indexaforBaseStationId];
            temp.FreeChargeSlots++;
            DataSource.BaseStationsList[indexaforBaseStationId] = temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int ID)
        {
            if (!DataSource.BaseStationsList.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return DataSource.BaseStationsList.Find(x => x.Id == ID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetBaseStationList(Predicate<BaseStation> predicate = null)
        {
            return DataSource.BaseStationsList.Where(x => predicate == null ? true : predicate(x));
        }
    }
}
