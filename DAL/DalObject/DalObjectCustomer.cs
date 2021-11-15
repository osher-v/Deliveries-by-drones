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
        /// The function adds a customer to the list of customers.
        /// </summary>
        /// <param name="newCustomer"></param>
        public void AddCustomer(Customer newCustomer)
        {
            if ((DataSource.BaseStationsList.FindIndex(x => x.Id == newCustomer.Id)) != -1)
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            DataSource.CustomersList.Add(newCustomer);
        }
        /// <summary>
        /// The function returns the selected Customer.
        /// </summary>
        /// <param name="ID">Id of a selected Customer</param>
        /// <returns>return empty ubjact if its not there</returns>
        
        
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
        /// <summary>
        /// The function returns an array of all Customer.
        /// </summary>
        /// <returns>returns a new List that hold all the data from the reqsted List</returns>
        public IEnumerable<Customer> GetCustomerList()
        {
            return DataSource.CustomersList.Take(DataSource.CustomersList.Count).ToList();
        }
    }
}
