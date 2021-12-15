using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;
using DalApi;

namespace DalObject
{
    /// <summary>
    /// matods that use from the main 
    /// </summary>
    partial class DalObject : IDal 
    {
        /// <summary>
        /// Default constructor.
        /// </summary>  
        #region Singelton

        static DalObject() { }// static ctor to ensure instance init is done just before first usage

        private DalObject() //private  
        {
            DataSource.Initialize();
        }

        internal static DalObject Instance { get; } = new DalObject();// The public Instance property to use

        #endregion Singelton

        public double[] RequestPowerConsumptionByDrone()
        {
            double[] temp = { DataSource.Config.Free, DataSource.Config.LightWeightCarrier,
            DataSource.Config.MediumWeightBearing, DataSource.Config.CarriesHeavyWeight,
                DataSource.Config.DroneLoadingRate};
            return temp;
        }

    }
}


