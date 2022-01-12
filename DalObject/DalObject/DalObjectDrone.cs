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
        public void AddDrone(Drone newDrone)
        {
            if (DataSource.DronesList.Exists(x => x.Id == newDrone.Id))
            {
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            }
            DataSource.DronesList.Add(newDrone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone newDrone)
        {
            if (!DataSource.DronesList.Exists(x => x.Id == newDrone.Id))
            {
                throw new NonExistentObjectException();
            }
            DataSource.DronesList[DataSource.DronesList.FindIndex(x => x.Id == newDrone.Id)] = newDrone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int ID)
        {
            if (!DataSource.DronesList.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return DataSource.DronesList.Find(x => x.Id == ID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDroneList()
        {
            return DataSource.DronesList.Select(item => item);
        }
    }
}
