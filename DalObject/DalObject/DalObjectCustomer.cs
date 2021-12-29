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
            if (DataSource.BaseStationsList.Exists(x => x.Id == newCustomer.Id))
            {
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            }
            DataSource.CustomersList.Add(newCustomer);
        }
       
        public void UpdateCustomer(Customer newCustomer)
        {
            if (!DataSource.CustomersList.Exists(x => x.Id == newCustomer.Id))
            {
                throw new NonExistentObjectException();
            }
            DataSource.CustomersList[DataSource.CustomersList.FindIndex(x => x.Id == newCustomer.Id)] = newCustomer;
        }

        public Customer GetCustomer(int ID)
        {
            if (!DataSource.CustomersList.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return DataSource.CustomersList.Find(x => x.Id == ID);
        }
        
        public IEnumerable<Customer> GetCustomerList()
        {
            //return DataSource.CustomersList.Take(DataSource.CustomersList.Count).ToList();
            return DataSource.CustomersList.Select(item => item);
        }

        //public void RemoveCustomer(int CustomerId)
        //{
        //    int index = DataSource.CustomersList.FindIndex(x => x.Id == CustomerId);
        //    if (index == -1)
        //    {
        //        throw new NonExistentObjectException();
        //    }
        //    DataSource.CustomersList.RemoveAt(index); //else

        //    ////this Remove fanction return true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the List<T>.
        //    //bool successOperation = DataSource.CustomersList.Remove(Customer);
        //    //if (!successOperation)
        //    //{
        //    //    throw new NonExistentObjectException();
        //    //}
        //}
    }
}
