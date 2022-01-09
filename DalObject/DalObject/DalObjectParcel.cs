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
        public int AddParcel(Parcel newParcel)
        {
            newParcel.Id = DataSource.Config.CountIdPackage++;
            DataSource.ParcelsList.Add(newParcel);
            return newParcel.Id; //Returns the id of the current Parcel.
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignPackageToDdrone(int ParcelId, int droneId)
        {
            //Update the package.
            int indexaforParcel = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = DataSource.ParcelsList[indexaforParcel];
            temp.DroneId = droneId;
            temp.Assigned = DateTime.Now;
            DataSource.ParcelsList[indexaforParcel] = temp; 
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickedUpPackageByTheDrone(int ParcelId)
        {
            //Update the package.
            int indexaforParcel = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = DataSource.ParcelsList[indexaforParcel];
            temp.PickedUp = DateTime.Now;
            DataSource.ParcelsList[indexaforParcel] = temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveryPackageToTheCustomer(int ParcelId)
        {
            int indexaforParcel = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = DataSource.ParcelsList[indexaforParcel];
            temp.Delivered = DateTime.Now;
            DataSource.ParcelsList[indexaforParcel] = temp;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int ID)
        {
            if (!DataSource.ParcelsList.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return DataSource.ParcelsList.Find(x => x.Id == ID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> prdicat = null)
        {
            //return DataSource.ParcelsList.FindAll(x => prdicat == null ? true : prdicat(x));
            return DataSource.ParcelsList.Where(x => prdicat == null ? true : prdicat(x));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int ParcelId)
        {
            int index = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);
            if (index == -1)
            {
                throw new NonExistentObjectException();
            }
            DataSource.ParcelsList.RemoveAt(index); //else    
        }
    }
}
