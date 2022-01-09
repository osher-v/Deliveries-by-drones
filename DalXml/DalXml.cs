using DalApi;
using DalObject;
using DO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.CompilerServices;

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
           //DataSource.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        private DalXml() //private  
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            foreach (var item in droneCharge)
            {
                UpdatePluseChargeSlots(item.StationId);
            }
            droneCharge.Clear();
            XMLTools.SaveListToXMLSerializer(droneCharge, DroneChargeXml);
        }

        internal static DalXml Instance { get; } = new DalXml();// The public Instance property to use

        #endregion Singelton


        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] RequestPowerConsumptionByDrone()
        {
            List<string> config = XMLTools.LoadListFromXMLSerializer<string>(@"DalXmlConfig.xml");
            double[] temp = { double.Parse(config[0]), double.Parse(config[1]), double.Parse(config[2]),
                     double.Parse(config[3]),double.Parse(config[4])};
            return temp;
        }

        #region Stations
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int ID)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            if (!baseStations.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return baseStations.Find(x => x.Id == ID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetBaseStationList(Predicate<BaseStation> predicate = null)
        {
            List<BaseStation> baseStations = XMLTools.LoadListFromXMLSerializer<BaseStation>(BaseStationXml);
            return baseStations.Where(x => predicate == null ? true : predicate(x));
        }
        #endregion Stations

        #region Customers
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer newCustomer)
        {
            XElement element = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in element.Elements()
                                 where cus.Element("Id").Value == newCustomer.Id.ToString()
                                 select cus).FirstOrDefault();
            if (customer != null)
            {
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            }

            XElement CustomerElem = new XElement("Customer",
                                 new XElement("Id", newCustomer.Id),
                                 new XElement("Name", newCustomer.Name),
                                 new XElement("PhoneNumber", newCustomer.PhoneNumber),
                                 new XElement("Longitude", newCustomer.Longitude),
                                 new XElement("Latitude", newCustomer.Latitude));

            element.Add(CustomerElem);

            XMLTools.SaveListToXMLElement(element, CustomerXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer newCustomer)
        {
            XElement element = XMLTools.LoadListFromXMLElement(CustomerXml);

            XElement customer = (from cus in element.Elements()
                                 where cus.Element("Id").Value == newCustomer.Id.ToString()
                                 select cus).FirstOrDefault();
            if (customer == null)
            {
                throw new AddAnExistingObjectException("Error adding an object with an existing ID number");
            }

            customer.Element("Id").Value = newCustomer.Id.ToString();
            customer.Element("Name").Value = newCustomer.Name;
            customer.Element("PhoneNumber").Value = newCustomer.PhoneNumber;
            customer.Element("Longitude").Value = newCustomer.Longitude.ToString();
            customer.Element("Latitude").Value = newCustomer.Latitude.ToString();

            XMLTools.SaveListToXMLElement(element, CustomerXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int ID)
        {
            XElement element = XMLTools.LoadListFromXMLElement(CustomerXml);

            Customer customer = (from cus in element.Elements()
                                 where cus.Element("Id").Value == ID.ToString()
                                 select new Customer()
                                 {
                                     Id = int.Parse(cus.Element("Id").Value),
                                     Name = cus.Element("Name").Value,
                                     PhoneNumber = cus.Element("PhoneNumber").Value,
                                     Longitude = double.Parse(cus.Element("Longitude").Value),
                                     Latitude = double.Parse(cus.Element("Latitude").Value)
                                 }
                        ).FirstOrDefault();

            if(customer.Id != 0)
            {
                return customer;
            }
            else
            {
                throw new NonExistentObjectException();
            }    
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomerList()
        {
            XElement element = XMLTools.LoadListFromXMLElement(CustomerXml);
            IEnumerable<Customer> customer = from cus in element.Elements()
                                         select new Customer()
                                         {
                                             Id = int.Parse(cus.Element("Id").Value),
                                             Name = cus.Element("Name").Value,
                                             PhoneNumber = cus.Element("PhoneNumber").Value,
                                             Longitude = double.Parse(cus.Element("Longitude").Value),
                                             Latitude = double.Parse(cus.Element("Latitude").Value)                  
                                         };
            return customer.Select(item => item);
        }
        #endregion Customers

        #region Drones
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int ID)
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            if (!drones.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return drones.Find(x => x.Id == ID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDroneList()
        {
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(DroneXml);
            return drones.Select(item => item);     
        }
        #endregion Drones

        #region DroneCharge
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromChargingAtBaseStation(int droneId)
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            droneCharge.RemoveAt(droneCharge.FindIndex(x => x.DroneId == droneId));
            XMLTools.SaveListToXMLSerializer(droneCharge, DroneChargeXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetBaseCharge(int droneID)
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);
            if (!droneCharge.Exists(x => x.DroneId == droneID))
            {
                throw new NonExistentObjectException();
            }
            return droneCharge.Find(x => x.DroneId == droneID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetBaseChargeList(Predicate<DroneCharge> predicate = null)
        {
            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargeXml);

            return droneCharge.Where(x => predicate == null ? true : predicate(x));
        }
        #endregion DroneCharge

        #region Parcel
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AddParcel(Parcel newParcel)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            List<string> config = XMLTools.LoadListFromXMLSerializer<string>(@"DalXmlConfig.xml");
            int CountIdPackage = int.Parse(config[5]);
            newParcel.Id = CountIdPackage++;
            config[5] = CountIdPackage.ToString();
            XMLTools.SaveListToXMLSerializer<string>(config,@"DalXmlConfig.xml");

            parcel.Add(newParcel);
            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
            return newParcel.Id; //Returns the id of the current Parcel.
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AssignPackageToDdrone(int ParcelId, int droneId)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            int indexaforParcel = parcel.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = parcel[indexaforParcel];
            temp.DroneId = droneId;
            temp.Assigned = DateTime.Now;
            parcel[indexaforParcel] = temp;

            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickedUpPackageByTheDrone(int ParcelId)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            //Update the package.
            int indexaforParcel = parcel.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = parcel[indexaforParcel];
            temp.PickedUp = DateTime.Now;
            parcel[indexaforParcel] = temp;

            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliveryPackageToTheCustomer(int ParcelId)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            int indexaforParcel = parcel.FindIndex(x => x.Id == ParcelId);

            if (indexaforParcel == -1)
                throw new NonExistentObjectException();

            Parcel temp = parcel[indexaforParcel];
            temp.Delivered = DateTime.Now;
            parcel[indexaforParcel] = temp;
            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int ID)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            if (!parcel.Exists(x => x.Id == ID))
            {
                throw new NonExistentObjectException();
            }
            return parcel.Find(x => x.Id == ID);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcelList(Predicate<Parcel> prdicat = null)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);
            return parcel.Where(x => prdicat == null ? true : prdicat(x));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveParcel(int ParcelId)
        {
            List<Parcel> parcel = XMLTools.LoadListFromXMLSerializer<Parcel>(ParcelXml);

            int index = parcel.FindIndex(x => x.Id == ParcelId);
            if (index == -1)
            {
                throw new NonExistentObjectException();
            }
            parcel.RemoveAt(index); //else

            XMLTools.SaveListToXMLSerializer(parcel, ParcelXml);
        }      
        #endregion Parcel
    }
}
