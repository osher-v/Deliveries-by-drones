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
        public void AddCustomer(Customer newCustomer)
        {
            if ((DataSource.BaseStationsList.FindIndex(x => x.Id == newCustomer.Id)) != -1)
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            DataSource.CustomersList.Add(newCustomer);
        }
       
        public void UpdateCustomer(Customer newCustomer)
        {
            if (!(DataSource.CustomersList.Exists(x => x.Id == newCustomer.Id)))
                throw new NonExistentObjectException();
            DataSource.CustomersList[DataSource.CustomersList.FindIndex(x => x.Id == newCustomer.Id)] = newCustomer;
        }

        public Customer GetCustomer(int ID)
        {
            if (!(DataSource.CustomersList.Exists(x => x.Id == ID)))
                throw new NonExistentObjectException();
            return DataSource.CustomersList.Find(x => x.Id == ID);
        }
        
        public IEnumerable<Customer> GetCustomerList()
        {
            return DataSource.CustomersList.Take(DataSource.CustomersList.Count).ToList();
        }
    }
}
