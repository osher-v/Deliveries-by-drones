using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;

namespace ConsoleUI
{
    #region enums
    /// <summary>enum for the first dialog </summary>
    enum Options { Insert = 1, Update, DisplaySingle, DisplayList, Distance, EXIT }
    /// <summary> enum for InsertrOption</summary>
    enum InsertrOption { BaseStation = 1, Drone, Customer, Parcel}
    /// <summary> enum for UpdatesOption</summary>
    enum UpdatesOption { AssignDrone = 1, PickUp, Deliverd, Incharging, Outcharging }
    /// <summary>enum for DisplaySingleOption </summary>
    enum DisplaySingleOption { BaseStationView = 1, Dronedisplay, CustomerView, PackageView }
    /// <summary>enum for DisplayListOption </summary>
    enum DisplayListOption
    {
        ListOfBaseStationView = 1, ListOfDronedisplay, ListOfCustomerView,
        ListOfPackageView, ListOfFreePackageView, ListOfBaseStationsWithFreeChargSlots
    }
    /// <summary>enum for chackDistance</summary>
    enum chackDistance { BasePoint = 1, CustomerPoint }
    #endregion enums

    ///<summary> main class </summary> 
    class Program
    {
        #region fanction of main

        #region Handling insert options
        /// <summary>
        /// The function handles various addition options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void InsertOptions(DalApi.IDal dal)
        {
            Console.WriteLine(@"
Insert options:

1. Add a base station to the list of stations.
2. Add a drone to the list of existing drones.
3. Admission of a new customer to the customer list.
4. Admission of package for shipment.

Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);
            
            switch ((InsertrOption)choice)
            {
                case InsertrOption.BaseStation:
                    int newBaseStationID, newchargsSlots;
                    string newname;
                    double newlongitude, newlatitude;

                    Console.WriteLine(@"
You have selected to add a new station.
Please enter an ID number for the new station:");
                    while (!int.TryParse(Console.ReadLine(), out newBaseStationID)) ;
                    Console.WriteLine("Next Please enter the name of the station:");
                    newname = Console.ReadLine();
                    Console.WriteLine("Next Please enter the number of charging slots at the station:");
                    while (!int.TryParse(Console.ReadLine(), out newchargsSlots)) ;
                    Console.WriteLine("Next Please enter the longitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newlongitude)) ;
                    Console.WriteLine("Next Please enter the latitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newlatitude)) ;

                    BaseStation newbaseStation = new BaseStation{
                        Id = newBaseStationID, StationName = newname, FreeChargeSlots = newchargsSlots, Longitude = newlongitude, Latitude = newlatitude
                    };
                    dal.AddStation(newbaseStation);
                    break;

                case InsertrOption.Drone:
                    int newDroneID, newMaxWeight, newStatus;
                    string newmodel;
                    double newBatteryLevel;

                    Console.WriteLine(@"
You have selected to add a new Drone.
Please enter an ID number for the new drone:");
                    while (!int.TryParse(Console.ReadLine(), out newDroneID)) ;
                    Console.WriteLine("Next Please enter the model of the Drone:");
                    newmodel = Console.ReadLine();
                    Console.WriteLine("Next enter the weight category of the new Drone: 0 for light, 1 for medium and 2 for heavy");
                    while (!int.TryParse(Console.ReadLine(), out newMaxWeight)) ;
                    Console.WriteLine("Next enter the charge level of the battery:");
                    while (!double.TryParse(Console.ReadLine(), out newBatteryLevel)) ;
                    Console.WriteLine("Next enter the status of the new Drone: 0 for free, 1 for inMaintenance and 2 for busy");
                    while (!int.TryParse(Console.ReadLine(), out newStatus)) ;
                    
                    Drone newdrone = new Drone {
                        Id = newDroneID, Model = newmodel, MaxWeight = (WeightCategories)newMaxWeight
                    };
                    dal.AddDrone(newdrone);
                    break;

                case InsertrOption.Customer:
                    int newCustomerID;
                    string newCustomerName, newPhoneNumber;
                    double newCustomerLongitude, newCustomerLatitude;

                    Console.WriteLine(@"
You have selected to add a new Customer.
Please enter an ID number for the new Customer:");
                    while (!int.TryParse(Console.ReadLine(), out newCustomerID)) ;
                    Console.WriteLine("Next Please enter the name of the customer:");
                    newCustomerName = Console.ReadLine();
                    Console.WriteLine("Next enter the phone number of the new customer:");
                    newPhoneNumber = Console.ReadLine();
                    Console.WriteLine("Next Please enter the longitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newCustomerLongitude)) ;
                    Console.WriteLine("Next Please enter the latitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out newCustomerLatitude)) ;

                    Customer newCustomer = new Customer {
                        Id = newCustomerID, Name = newCustomerName, PhoneNumber = newPhoneNumber,
                        Longitude = newCustomerLongitude, Latitude = newCustomerLatitude
                    };
                    dal.AddCustomer(newCustomer);
                    break;

                case InsertrOption.Parcel:
                    int newSenderId, newTargetId, newWeight, newPriorities;

                    Console.WriteLine(@"
You have selected to add a new Parcel.
Next Please enter the sender ID number:");
                    while (!int.TryParse(Console.ReadLine(), out newSenderId)) ;
                    Console.WriteLine("Next Please enter the target ID number:");
                    while (!int.TryParse(Console.ReadLine(), out newTargetId)) ;
                    Console.WriteLine("Next enter the weight category of the new Parcel: 0 for free, 1 for inMaintenance and 2 for busy");
                    while (!int.TryParse(Console.ReadLine(), out newWeight)) ;
                    Console.WriteLine("Next enter the priorities of the new Parcel: 0 for regular, 1 for fast and 2 for urgent");
                    while (!int.TryParse(Console.ReadLine(), out newPriorities)) ;

                    Parcel newParcel = new Parcel{
                        SenderId = newSenderId, TargetId = newTargetId, Weight = (WeightCategories)newWeight,
                        Priority = (Priorities)newPriorities
                    };

                    int counterParcelSerialNumber = dal.AddParcel(newParcel);
                    break;

                default:
                    break;
            }
        }
        #endregion Handling insert options

        #region Handling update options
        /// <summary>
        /// The function handles various update options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void UpdateOptions(DalApi.IDal dal)
        {
            Console.WriteLine(@"
Update options:

1. Assigning a package to a drone
2. Collection of a package by drone
3. Delivery package to customer
4. Sending a drone for charging at a base station
5. Release drone from charging at base station
Your choice:");
           int.TryParse(Console.ReadLine(), out int choice);

            int ParcelId, droneId, baseStationId;

            switch ((UpdatesOption)choice)
            {

                case UpdatesOption.AssignDrone:
                    Console.WriteLine("please enter Parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);
                    Console.WriteLine("please enter Drone ID:");
                    int.TryParse(Console.ReadLine(), out droneId);

                    dal.AssignPackageToDdrone(ParcelId, droneId);
                    break;

                case UpdatesOption.PickUp:
                    Console.WriteLine("please enter Parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);

                    dal.PickedUpPackageByTheDrone(ParcelId);
                    break;

                case UpdatesOption.Deliverd:
                    Console.WriteLine("please enter Parcel ID:");
                    int.TryParse(Console.ReadLine(), out ParcelId);

                    dal.DeliveryPackageToTheCustomer(ParcelId);
                    break;

                case UpdatesOption.Incharging:
                    Console.WriteLine("please enter Drone ID:");
                    int.TryParse(Console.ReadLine(), out droneId);
                    Console.WriteLine("please choose baseStationId ID from the List below:");                
                    List<BaseStation> displayBaseStationWithFreeChargSlots = dal.GetBaseStationList(x => x.FreeChargeSlots > 0).ToList();
                    for (int i = 0; i < displayBaseStationWithFreeChargSlots.Count; i++)
                    {
                        Console.WriteLine(displayBaseStationWithFreeChargSlots[i].ToString());
                    }

                    int.TryParse(Console.ReadLine(), out baseStationId);

                    dal.SendingDroneforChargingAtBaseStation(baseStationId, droneId);
                    break;

                case UpdatesOption.Outcharging:
                    Console.WriteLine("please enter Drone ID:");
                    int.TryParse(Console.ReadLine(), out droneId);

                    dal.ReleaseDroneFromChargingAtBaseStation(droneId);
                    break;

                default:
                    break;
            }
        }
        #endregion Handling update options

        #region Handling display options
        /// <summary>
        /// The function handles display options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplaySingleOptions(DalApi.IDal dal)
        {
            Console.WriteLine(@"
Display options(single):

1. Base station view.
2. Drone display.
3. Customer view.
4. Package view.

Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            int idForViewObject;
           
            switch ((DisplaySingleOption)choice)
            {
                case DisplaySingleOption.BaseStationView:
                    Console.WriteLine("Insert ID number of base station:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.GetBaseStation(idForViewObject).ToString());
                    break;

                case DisplaySingleOption.Dronedisplay:
                    Console.WriteLine("Insert ID number of requsted drone:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.GetDrone(idForViewObject).ToString());
                    break;

                case DisplaySingleOption.CustomerView:
                    Console.WriteLine("Insert ID number of requsted Customer:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.GetCustomer(idForViewObject).ToString());
                    break;

                case DisplaySingleOption.PackageView:
                    Console.WriteLine("Insert ID number of reqused parcel:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);

                    Console.WriteLine(dal.GetParcel(idForViewObject).ToString());
                    break;

                default:
                    break;
            }
        }
        #endregion Handling display options

        #region Handling the list display options
        /// <summary>
        /// The function prints the data array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToPrint"></param>
        public static void printTheList<T>(List<T> listToPrint)// where T : struct
        {
            foreach (T item in listToPrint)
            {
                Console.WriteLine(item);
            }
        }

        /// <summary>
        /// The function handles list view options.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DisplayListOptions(DalApi.IDal dal)
        {
            Console.WriteLine(@"
Display options (for the whole list):

1. Displays a list of base stations.
2. Displays the list of drone.
3. View the customer list.
4. Displays the list of packages.
5. Displays a list of packages that have not yet been assigned to the drone.
6. Display base stations with available charging stations.

Your choice:");
            int.TryParse(Console.ReadLine(), out int choice);

            switch ((DisplayListOption)choice)
            {
                case DisplayListOption.ListOfBaseStationView:
                    printTheList(dal.GetBaseStationList().ToList());
                    break;
                    
                case DisplayListOption.ListOfDronedisplay:
                    printTheList(dal.GetDroneList().ToList());
                    break;

                case DisplayListOption.ListOfCustomerView:
                    printTheList(dal.GetCustomerList().ToList());
                    break;

                case DisplayListOption.ListOfPackageView:
                    printTheList(dal.GetParcelList().ToList());
                    break;
           
                case DisplayListOption.ListOfFreePackageView:
                    printTheList(dal.GetParcelList(x => x.DroneId == 0).ToList());
                    break;
    
               case DisplayListOption.ListOfBaseStationsWithFreeChargSlots:
                    printTheList(dal.GetBaseStationList(x => x.FreeChargeSlots > 0).ToList());
                    break;

                default:
                    break;
            }

        }
        #endregion Handling the list display options

        #region Handling calculat the distance of coordinates
        /// <summary>
        /// The function checks the distance between points.
        /// </summary>
        /// <param name="dal">DalObject object that is passed as a parameter to enable the functions in the DalObject class</param>
        static public void DistanceBetweenPoints(DalApi.IDal dal)
        {
            Console.WriteLine(@"
You have chosen the option of calculating distance from a point to a customer or station.
Please enter your point.
longitude:");
            Double longitudeNew, latitudeNew;
            
            while (!double.TryParse(Console.ReadLine(), out  longitudeNew)) ;
            Console.WriteLine("latitude:");
            while (!double.TryParse(Console.ReadLine(), out  latitudeNew)) ;
            Console.WriteLine(@"Would you like to check distance from a station or customer? 
Choose 1 for a station or 2 for a customer");
            int.TryParse(Console.ReadLine(), out int choice);
           
            switch ((chackDistance)choice)
            {
                case chackDistance.BasePoint:
                    Console.WriteLine("please enter base ID:");
                    int.TryParse(Console.ReadLine(), out int baseID);

                    Console.WriteLine(DalObject.DalObject.GetDistance(longitudeNew, latitudeNew, baseID, choice));
                    break;

                case chackDistance.CustomerPoint:
                    Console.WriteLine("please enter Customer ID:");
                    int.TryParse(Console.ReadLine(), out int customerID);

                    Console.WriteLine(DalObject.DalObject.GetDistance(longitudeNew, latitudeNew, customerID, choice));

                    break;

                default:
                    break;
            }
        }
        #endregion Handling calculat the distance of coordinates

        #endregion fanction of main
        
        static void Main(string[] args)
        {
            DalApi.IDal dalObject = DalApi.DalFactory.GetDL();
            
            int choice;
            do
            {
                Console.WriteLine(@"
choose from the following options (type the selected number): 

1. Insert options.
2. Update options.
3. Display options(singel).
4. Display options (for the whole list).
5. Calculate distance between points.
6. EXIT.
Your choice:");
                int.TryParse(Console.ReadLine(), out choice);

                switch ((Options)choice)
                {
                    case Options.Insert:
                        InsertOptions(dalObject);
                        break;

                    case Options.Update:
                        UpdateOptions(dalObject);
                        break;

                    case Options.DisplaySingle:
                        DisplaySingleOptions(dalObject);
                        break;

                    case Options.DisplayList:
                        DisplayListOptions(dalObject);
                        break;
                    case Options.Distance:
                        DistanceBetweenPoints(dalObject);
                        break;
                    case Options.EXIT:
                        Console.WriteLine("Have a good day");
                        break;

                    default:
                        break;
                }
            } while (!(choice == 6));
        }
    }
}
