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
            if (DataSource.DronesList.Exists(x => x.Id == newDrone.Id))
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            DataSource.DronesList.Add(newDrone);
        }

        public void UpdateDrone(Drone newDrone)
        {
            if (!DataSource.DronesList.Exists(x => x.Id == newDrone.Id))
                throw new NonExistentObjectException();
            DataSource.DronesList[DataSource.DronesList.FindIndex(x => x.Id == newDrone.Id)] = newDrone;
        }
      
        public Drone GetDrone(int ID)
        {
            if (!(DataSource.DronesList.Exists(x => x.Id == ID)))
                throw new NonExistentObjectException();
            return DataSource.DronesList.Find(x => x.Id == ID);
        }
    
        public IEnumerable<Drone> GetDroneList()
        {
            return DataSource.DronesList.Take(DataSource.DronesList.Count);
        }
    }
}
