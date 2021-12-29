using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;
using DalApi;
namespace DalObject
{
     partial class DalObject : IDal
    {
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.DronesList.Exists(x => x.Id == newDrone.Id))
            {
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            }
            DataSource.DronesList.Add(newDrone);
        }

        public void UpdateDrone(Drone newDrone)
        {
            if (!DataSource.DronesList.Exists(x => x.Id == newDrone.Id))
            {
                throw new NonExistentObjectException();
            }
            DataSource.DronesList[DataSource.DronesList.FindIndex(x => x.Id == newDrone.Id)] = newDrone;
        }
      
        public Drone GetDrone(int ID)
        {
            if (!DataSource.DronesList.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return DataSource.DronesList.Find(x => x.Id == ID);
        }
    
        public IEnumerable<Drone> GetDroneList()
        {
            //return DataSource.DronesList.Take(DataSource.DronesList.Count);
            return DataSource.DronesList.Select(item => item);
        }

        //public void RemoveDrone(int DroneId)
        //{
        //    int index = DataSource.DronesList.FindIndex(x => x.Id == DroneId);
        //    if (index == -1)
        //    {
        //        throw new NonExistentObjectException();
        //    }
        //    DataSource.DronesList.RemoveAt(index); //else

        //    ////this Remove fanction return true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the List<T>.
        //    //bool successOperation = DataSource.DronesList.Remove(Drone);
        //    //if (!successOperation)
        //    //{
        //    //    throw new NonExistentObjectException();
        //    //}
        //}
    }
}
