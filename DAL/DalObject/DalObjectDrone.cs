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
        public void AddDrone(Drone newDrone)
        {
            if ((DataSource.BaseStationsList.FindIndex(x => x.Id == newDrone.Id)) != -1)
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            DataSource.DronesList.Add(newDrone);
        }

        public void UpdateDrone(Drone newDrone)
        {
            if (!(DataSource.DronesList.Exists(x => x.Id == newDrone.Id)))
                throw new NonExistentObjectException();
            DataSource.DronesList[DataSource.DronesList.FindIndex(x => x.Id == newDrone.Id)] = newDrone;
        }

        /// <summary>
        /// The function returns the selected Drone.
        /// </summary>
        /// <param name="ID">Id of a selected Drone</param>
        /// <returns>return empty ubjact if its not there</returns>
        public Drone GetDrone(int ID)
        {
            if (!(DataSource.DronesList.Exists(x => x.Id == ID)))
                throw new NonExistentObjectException();
            return DataSource.DronesList.Find(x => x.Id == ID);
        }

        /// <summary>
        /// The function returns an array of all Drone.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Drone> GetDroneList()
        {
            return DataSource.DronesList.Take(DataSource.DronesList.Count);
        }
    }
}
