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
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.BaseStationsList.Exists(x => x.Id == newCustomer.Id))
            {
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            }
            DataSource.CustomersList.Add(newCustomer);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer newCustomer)
        {
            if (!DataSource.CustomersList.Exists(x => x.Id == newCustomer.Id))
            {
                throw new NonExistentObjectException();
            }
            DataSource.CustomersList[DataSource.CustomersList.FindIndex(x => x.Id == newCustomer.Id)] = newCustomer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int ID)
        {
            if (!DataSource.CustomersList.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return DataSource.CustomersList.Find(x => x.Id == ID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomerList()
        {
            return DataSource.CustomersList.Select(item => item);
        }
    }
}
