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
        /// <summary>
        /// The function adds a Parcel to the list of Parcels.
        /// </summary>
        /// <param name="newParcel"></param>
        /// <returns></returns>
        public int AddParcel(Parcel newParcel)
        {
            newParcel.Id = DataSource.Config.CountIdPackage++;
            DataSource.ParcelsList.Add(newParcel);
            return newParcel.Id; //Returns the id of the current Parcel.
        }
        /// <summary>
        /// The function assigns a package to the drone.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        /// <param name="droneId">Id of drone</param>
        public void AssignPackageToDdrone(int ParcelId, int droneId)
        {
            //האם להוסיף בדיקה גם על תקינות הרחפן??
            //Update the package.
            int indexaforParcel = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = DataSource.ParcelsList[indexaforParcel];
            temp.DroneId = droneId;
            temp.Assigned = DateTime.Now;
            DataSource.ParcelsList[indexaforParcel] = temp;

            /*
            //drone update.
            int indexaforDrone = DataSource.DronesList.FindIndex(x => x.Id == droneId);
            Drone help = DataSource.DronesList[indexaforDrone];
            help.Status = (DroneStatuses)2; //busy
            DataSource.DronesList[indexaforDrone] = help;
            */
        }
        /// <summary>
        /// picked up package by the drone.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
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
        /// <summary>
        /// delivery package to the customer.
        /// </summary>
        /// <param name="ParcelId">Id of Parcel</param>
        public void DeliveryPackageToTheCustomer(int ParcelId)
        {
            int indexaforParcel = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = DataSource.ParcelsList[indexaforParcel];
            temp.Delivered = DateTime.Now;
            DataSource.ParcelsList[indexaforParcel] = temp;
        }
        /// <summary>
        /// The function returns the selected Parcel.
        /// </summary>
        /// <param name="ID">Id of a selected Parcel</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Parcel GetParcel(int ID)
        {
            if (!(DataSource.ParcelsList.Exists(x => x.Id == ID)))
                throw new NonExistentObjectException();
            return DataSource.ParcelsList.Find(x => x.Id == ID);
        }

        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> prdicat = null)
        {
            return DataSource.ParcelsList.FindAll(x => prdicat == null ? true : prdicat(x));
        }

        /*
        /// <summary>
        /// The function returns an array of all Parcel.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Parcel> GetParcelList()
        {
            return DataSource.ParcelsList.Take(DataSource.ParcelsList.Count).ToList();
        }
        /// <summary>
        /// The function returns an array of all packages not associated with the Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Parcel> GetParcelWithoutDrone(Predicate<Parcel> prdicat = null)
        {
            return DataSource.ParcelsList.FindAll(x => prdicat == null ? true : prdicat(x));
        }
        */
    }
}
