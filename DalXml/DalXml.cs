using DalApi;
using DalObject;
using DO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace DalXml
{
    public class DalXml : IDal
    {
        public static string DroneXml = @"DroneXml.xml";
        public static string BaseStationXml = @"BaseStationXml.xml";
        public static string CustomerXml = @"CustomerXml.xml";
        public static string ParcelXml = @"ParcelXml.xml";
        public static string DroneChargeXml = @"DroneChargeXml.xml";

        /// <summary>
        /// Default constructor.
        /// </summary>  
        #region Singelton

        static DalXml()// static ctor to ensure instance init is done just before first usage
        {
            // DataSource.Initialize();
        }

        private DalXml() //private  
        {

        }

        internal static DalXml Instance { get; } = new DalXml();// The public Instance property to use

        #endregion Singelton

        public double[] RequestPowerConsumptionByDrone()
        {
            double[] temp = { DataSource.Config.Free, DataSource.Config.LightWeightCarrier,
            DataSource.Config.MediumWeightBearing, DataSource.Config.CarriesHeavyWeight,
                DataSource.Config.DroneLoadingRate};
            return temp;
        } //צריך לראות מה עושים עפ זה

        #region Stations
        public void AddStation(BaseStation newbaseStation)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            if (baseStations.Exists(x => x.Id == newbaseStation.Id))
            {
                throw new AddAnExistingObjectException();
            }
            baseStations.Add(newbaseStation);
            XMLTools.SaveListToXMLSerializer(baseStations, BaseStationXml);
        }

        public void UpdateBaseStation(BaseStation newBaseStation)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            if (!baseStations.Exists(x => x.Id == newBaseStation.Id))
            {
                throw new NonExistentObjectException();
            }
            baseStations[baseStations.FindIndex(x => x.Id == newBaseStation.Id)] = newBaseStation; //else
            XMLTools.SaveListToXMLSerializer(baseStations, BaseStationXml);
        }

        public void UpdateMinusChargeSlots(int baseStationId)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            //BaseStation update.
            int indexaforBaseStationId = baseStations.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = baseStations[indexaforBaseStationId];
            temp.FreeChargeSlots--;
            baseStations[indexaforBaseStationId] = temp;
            XMLTools.SaveListToXMLSerializer(baseStations, BaseStationXml);
        }

        public void UpdatePluseChargeSlots(int baseStationId)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            //BaseStation update.
            int indexaforBaseStationId = baseStations.FindIndex(x => x.Id == baseStationId);
            BaseStation temp = baseStations[indexaforBaseStationId];
            temp.FreeChargeSlots++;
            baseStations[indexaforBaseStationId] = temp;
            XMLTools.SaveListToXMLSerializer(baseStations, BaseStationXml);
        }

        public BaseStation GetBaseStation(int ID)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            if (!baseStations.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return baseStations.Find(x => x.Id == ID);
        }

        public IEnumerable<BaseStation> GetBaseStationList(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            return baseStations.Where(x => predicate == null ? true : predicate(x));

            //return DataSource.BaseStationsList.FindAll(x => predicate == null ? true : predicate(x));
        }

        #endregion Stations

        #region Customers
        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            if (customers.Exists(x => x.Id == newCustomer.Id))
            {
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            }
            customers.Add(newCustomer);
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }

        public void UpdateCustomer(Customer newCustomer)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            if (!customers.Exists(x => x.Id == newCustomer.Id))
            {
                throw new NonExistentObjectException();
            }
            customers[customers.FindIndex(x => x.Id == newCustomer.Id)] = newCustomer;
            XMLTools.SaveListToXMLSerializer(customers, CustomerXml);
        }

        public Customer GetCustomer(int ID)
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            if (!customers.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return customers.Find(x => x.Id == ID);
        }

        public IEnumerable<Customer> GetCustomerList()
        {
            List<Customer> customers = XMLTools.LoadListFromXMLSerializer<Customer>(CustomerXml);
            return customers.Select(item => item);
            //return DataSource.CustomersList.Take(DataSource.CustomersList.Count).ToList();
        }
        #endregion Customers

        #region Drones
        public void AddDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (drones.Exists(x => x.Id == newDrone.Id))
            {
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            }
            drones.Add(newDrone);
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public void UpdateDrone(Drone newDrone)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (!drones.Exists(x => x.Id == newDrone.Id))
            {
                throw new NonExistentObjectException();
            }
            drones[drones.FindIndex(x => x.Id == newDrone.Id)] = newDrone;
            XMLTools.SaveListToXMLSerializer(drones, DroneXml);
        }

        public Drone GetDrone(int ID)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (!drones.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return drones.Find(x => x.Id == ID);
        }

        public IEnumerable<Drone> GetDroneList()
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return drones.Select(item => item);

            //return DataSource.DronesList.Take(DataSource.DronesList.Count);
        }
        #endregion Drones

        //?????????????????????????????????????
        #region DroneCharge
        public void SendingDroneforChargingAtBaseStation(int baseStationId, int droneId)
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            droneCharge.Add(new DroneCharge()
            {
                StationId = baseStationId,
                DroneId = droneId,
                StartChargeTime = DateTime.Now
            });
            XMLTools.SaveListToXMLSerializer(droneCharge, DroneChargeXml);
        }

        public void ReleaseDroneFromChargingAtBaseStation(int droneId)
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            droneCharge.RemoveAt(droneCharge.FindIndex(x => x.DroneId == droneId));
            XMLTools.SaveListToXMLSerializer(droneCharge, DroneChargeXml);
        }

        public DroneCharge GetBaseCharge(int droneID)//???????????????????????????????
        {
            if (!DataSource.DroneChargeList.Exists(x => x.DroneId == droneID))
            {
                throw new NonExistentObjectException();
            }
            return DataSource.DroneChargeList.Find(x => x.DroneId == droneID);
        }

        public IEnumerable<DroneCharge> GetBaseChargeList(Predicate<DroneCharge> predicate = null)//????????????????????????
        {
            //return DataSource.DroneChargeList.FindAll(x => predicate == null ? true : predicate(x));
            return DataSource.DroneChargeList.Where(x => predicate == null ? true : predicate(x));
        }
        #endregion DroneCharge

        #region Parcel
        public int AddParcel(Parcel newParcel)
        {
            XElement element = XMLTools.LoadListFromXMLElement(ParcelXml);

            newParcel.Id = DataSource.Config.CountIdPackage++; //??????????????????????????????

            XElement ParcelElem = new XElement("Parcel",
                                 new XElement("Id", newParcel.Id),
                                 new XElement("SenderId", newParcel.SenderId),
                                 new XElement("TargetId", newParcel.TargetId),
                                 new XElement("Weight", newParcel.Weight),
                                 new XElement("Priority", newParcel.Priority),
                                 new XElement("DroneId", newParcel.DroneId),
                                 new XElement("Requested", newParcel.Requested),
                                 new XElement("Assigned", newParcel.Assigned),
                                 new XElement("PickedUp", newParcel.PickedUp),
                                 new XElement("Delivered", newParcel.Delivered));
            element.Add(ParcelElem);

            XMLTools.SaveListToXMLElement(element, ParcelXml);

            DataSource.ParcelsList.Add(newParcel);//?????????????????????????????????????

            return newParcel.Id; //Returns the id of the current Parcel.
        }

        public void AssignPackageToDdrone(int ParcelId, int droneId)
        {
            //Update the package.
            int indexaforParcel = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = DataSource.ParcelsList[indexaforParcel];
            temp.DroneId = droneId;
            temp.Assigned = DateTime.Now;
            DataSource.ParcelsList[indexaforParcel] = temp;
        }

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

        public void DeliveryPackageToTheCustomer(int ParcelId)
        {
            int indexaforParcel = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = DataSource.ParcelsList[indexaforParcel];
            temp.Delivered = DateTime.Now;
            DataSource.ParcelsList[indexaforParcel] = temp;
        }

        public Parcel GetParcel(int ID)
        {
            XElement element = XMLTools.LoadListFromXMLElement(ParcelXml);

            Parcel parcel = (from par in element.Elements()
                             where par.Element("Id").Value == ID.ToString()
                             select new Parcel()
                             {
                                 Id = int.Parse(par.Element("Id").Value),
                                 SenderId = int.Parse(par.Element("SenderId").Value),
                                 TargetId = int.Parse(par.Element("TargetId").Value),
                                 Weight = (WeightCategories)Enum.Parse(typeof(WeightCategories), par.Element("Weight").Value),
                                 Priority = (Priorities)Enum.Parse(typeof(Priorities), par.Element("Priority").Value),
                                 DroneId = int.Parse(par.Element("DroneId").Value),
                                 Requested = DateTime.ParseExact(par.Element("Requested").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture),
                                 Assigned = DateTime.ParseExact(par.Element("Assigned").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture),
                                 PickedUp = DateTime.ParseExact(par.Element("PickedUp").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture),
                                 Delivered = DateTime.ParseExact(par.Element("Delivered").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture),
                             }
                        ).FirstOrDefault();

            return parcel ?? throw new NonExistentObjectException();
            //if (!DataSource.ParcelsList.Exists(x => x.Id == ID))
            //{
            //    throw new NonExistentObjectException();
            //}
            //return DataSource.ParcelsList.Find(x => x.Id == ID);
        }

        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> prdicat = null)
        {
            //return DataSource.ParcelsList.FindAll(x => prdicat == null ? true : prdicat(x));
            return DataSource.ParcelsList.Where(x => prdicat == null ? true : prdicat(x));
        }

        public void RemoveParcel(int ParcelId) //????????????????????????????????????????
        {
            XElement element = XMLTools.LoadListFromXMLElement(ParcelXml);
            XElement ParcelElem = (from p in element.Elements()
                                   where p.Element("Id").Value == ParcelId.ToString()
                                   select p).FirstOrDefault();
            if (ParcelElem == null)
            {
                throw new NonExistentObjectException();
            }
            else
            {
                ParcelElem.Remove();
                XMLTools.SaveListToXMLElement(element, ParcelXml);
            }


            //int index = DataSource.ParcelsList.FindIndex(x => x.Id == ParcelId);
            //if (index == -1)
            //{
            //    throw new NonExistentObjectException();
            //}
            //DataSource.ParcelsList.RemoveAt(index); //else    
        }
        #endregion Parcel
    }
}
