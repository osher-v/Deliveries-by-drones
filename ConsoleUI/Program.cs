using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
using System.ComponentModel;

namespace ConsoleUI
{
    enum Options { Insert = 1, Update, DisplaySingle, DisplayList, EXIT }
    enum InsertrOption { baseStation = 1, Drone, AddCustomer, ParcelForShipment }
    enum UpdatesOption { AssignDrone=1,PickUp, Deliverd, Incharging, Outcharging }
    enum DisplaySingleOption { BaseStationView = 1, Dronedisplay, CustomerView, PackageView }
    enum DisplayListOption  { ListOfBaseStationView = 1, ListOfDronedisplay, ListOfCustomerView,
        ListOfPackageView, ListOfFreePackageView, ListOfBaseStationsWithFreeChargSlots  }
    class Program
    {
        static public void InsertOptions(DalObject.DalObject dal)
        {
            int choice = 0;
            Console.WriteLine(@"
Insert options:

1. Add a base station to the list of stations.
2. Add a drone to the list of existing drones.
3. Admission of a new customer to the customer list.
4. Admission of package for shipment.

Your choice:");
            int.TryParse(Console.ReadLine(), out choice);
            InsertrOption insertrOption;
            insertrOption = (InsertrOption)choice;
            switch (insertrOption)
            {
                case InsertrOption.baseStation:
                    int ID, chargsSlots;
                    string name;
                    double longitude, latitude;
                    Console.WriteLine(@"
You have selected to add a new station.
Please enter an ID number for the new station:");
                    while (!int.TryParse(Console.ReadLine(), out ID)) ;
                    Console.WriteLine("Next Please enter the name of the station:");
                    name = Console.ReadLine();
                    Console.WriteLine("Next Please enter the number of charging slots at the station:");
                    while (!int.TryParse(Console.ReadLine(), out chargsSlots)) ;
                    Console.WriteLine("Next Please enter the longitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out longitude)) ;
                    Console.WriteLine("Next Please enter the latitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out latitude)) ;
                    dal.SetStation(ID, name, chargsSlots, longitude, latitude);
                    
                    break;

                case InsertrOption.Drone:
                    int droneID, weightCategory, status;
                    string model;
                    double batterylevel;
                    Console.WriteLine(@"
You have selected to add a new Drone.
Please enter an ID number for the new drone:");
                    while (!int.TryParse(Console.ReadLine(), out droneID)) ;
                    Console.WriteLine("Next Please enter the model of the Drone:");
                    model = Console.ReadLine();
                    Console.WriteLine("Next enter the weight category of the new Drone: 0 for light, 1 for medium and 2 for heavy");
                    while (!int.TryParse(Console.ReadLine(), out weightCategory)) ;
                    Console.WriteLine("Next enter the charge level of the battery:");
                    while (!double.TryParse(Console.ReadLine(), out batterylevel)) ;
                    Console.WriteLine("Next enter the status of the new Drone: 0 for free, 1 for inMaintenance and 2 for busy");
                    while (!int.TryParse(Console.ReadLine(), out status)) ;
                    dal.SetDrone(droneID, model, weightCategory, status, batterylevel);
                    break;

                case InsertrOption.AddCustomer:
                    int customerID;
                    string customerName, PhoneNumber;
                    double customerLongitude, customerLatitude;
                    Console.WriteLine(@"
You have selected to add a new Customer.
Please enter an ID number for the new Customer:");
                    while (!int.TryParse(Console.ReadLine(), out customerID)) ;
                    Console.WriteLine("Next Please enter the name of the customer:");
                    customerName = Console.ReadLine();
                    Console.WriteLine("Next enter the phone number of the new customer:");
                    PhoneNumber = Console.ReadLine();
                    Console.WriteLine("Next Please enter the longitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out customerLongitude)) ;
                    Console.WriteLine("Next Please enter the latitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out customerLatitude)) ;
                    dal.SetCustomer(customerID, customerName, PhoneNumber, customerLongitude, customerLatitude);
                    break;

                case InsertrOption.ParcelForShipment:
                    int parcelID, SenderId, TargetId, Weight, priorities;
                    Console.WriteLine(@"
You have selected to add a new Parcel.");
                  // while (!int.TryParse(Console.ReadLine(), out parcelID)) ;
                    Console.WriteLine("Next Please enter the sender ID number:");
                    while (!int.TryParse(Console.ReadLine(), out SenderId)) ;
                    Console.WriteLine("Next Please enter the target ID number:");
                    while (!int.TryParse(Console.ReadLine(), out TargetId)) ;
                    Console.WriteLine("Next enter the weight category of the new Parcel: 0 for free, 1 for inMaintenance and 2 for busy");
                    while (!int.TryParse(Console.ReadLine(), out Weight)) ;
                    Console.WriteLine("Next enter the priorities of the new Parcel: 0 for regular, 1 for fast and 2 for urgent");
                    while (!int.TryParse(Console.ReadLine(), out priorities)) ;
                    int counterParcelSerialNumber = dal.Se4tParcel(/*parcelID,*/ SenderId, TargetId, Weight, priorities);
                    break;

                default:
                    break;
            }
        }
        static public void UpdateOptions(DalObject.DalObject dal)
        {
            int choice = 0;
            Console.WriteLine(@"
Update options:

1. Assigning a package to a drone
2. Collection of a package by drone
3. Delivery package to customer
4. Sending a drone for charging at a base station
5. Release drone from charging at base station
Your choice:");
            int.TryParse(Console.ReadLine(), out choice);
            UpdatesOption updateOptions = (UpdatesOption)choice;
            switch (updateOptions)
            {
                case UpdatesOption.AssignDrone:
                    Console.WriteLine("please enter Parcel ID:");
                    int ParcelId;
                    int.TryParse(Console.ReadLine(),out ParcelId);
                    Console.WriteLine("please enter Drone ID:");
                    int droneId;
                    int.TryParse(Console.ReadLine(), out droneId);
                    break;
                case UpdatesOption.PickUp:

                    break;
                case UpdatesOption.Deliverd:
                    break;
                case UpdatesOption.Incharging:
                    break;
                case UpdatesOption.Outcharging:
                    break;
                default:
                    break;
            }
        }
        static public void DisplaySingleOptions(DalObject.DalObject dal)
        {
            int choice = 0;
            Console.WriteLine(@"
Display options(single):

1. Base station view.
2. Drone display.
3. Customer view.
4. Package view.

Your choice:");
            int.TryParse(Console.ReadLine(), out choice);
            int idForViewObject;
            DisplaySingleOption displaySingleOption;
            displaySingleOption = (DisplaySingleOption)choice;
            switch (displaySingleOption)
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
        static public void DisplayListOptions(DalObject.DalObject dal)
        {
            int choice = 0;
            Console.WriteLine(@"
Display options (for the whole list):

1. Displays a list of base stations.
2. Displays the list of drone.
3. View the customer list.
4. Displays the list of packages.
5. Displays a list of packages that have not yet been assigned to the drone.
6. Display base stations with available charging stations.

Your choice:");
            int.TryParse(Console.ReadLine(), out choice);
            DisplayListOption displayListOptions;
            displayListOptions = (DisplayListOption)choice;

            switch (displayListOptions)
            {
                case DisplayListOption.ListOfBaseStationView:

                    List<BaseStation> displayBaseList = new List<BaseStation>();
                    displayBaseList = dal.GetBaseStationList();
                    for (int i = 0; i < displayBaseList.Count(); i++)
                    {
                        Console.WriteLine(displayBaseList[i].ToString());
                    }
                    break;
                case DisplayListOption.ListOfDronedisplay:
                    List<Drone> displayDroneList = new List<Drone>();
                    displayDroneList = dal.GetDroneList();
                    for (int i = 0; i < displayDroneList.Count(); i++)
                    {
                        Console.WriteLine(displayDroneList[i].ToString());
                    }
                    break;
                case DisplayListOption.ListOfCustomerView:
                    List<Customer> displayCustomerList = new List<Customer>();
                    displayCustomerList = dal.GetCustomerList();
                    for (int i = 0; i < displayCustomerList.Count(); i++)
                    {
                        Console.WriteLine(displayCustomerList[i].ToString());
                    }
                    break;
                case DisplayListOption.ListOfPackageView:
                    List<Parcel> displayPackageList = new List<Parcel>();
                    displayPackageList = dal.GetParcelList();
                    for (int i = 0; i < displayPackageList.Count(); i++)
                    {
                        Console.WriteLine(displayPackageList[i].ToString());
                    }
                    break;
                case DisplayListOption.ListOfFreePackageView:
                    List<Parcel> displayParcelWithoutDrone = new List<Parcel>();
                    displayParcelWithoutDrone = dal.GetParcelWithoutDrone();
                    for (int i = 0; i < displayParcelWithoutDrone.Count(); i++)
                    {
                        Console.WriteLine(displayParcelWithoutDrone[i].ToString());
                    }
                    break;
                case DisplayListOption.ListOfBaseStationsWithFreeChargSlots:
                    List<BaseStation> displayBaseStationWithFreeChargSlots = new List<BaseStation>();
                    displayBaseStationWithFreeChargSlots = dal.GetBaseStationsWithFreeChargSlots();
                    for (int i = 0; i < displayBaseStationWithFreeChargSlots.Count(); i++)
                    {
                        Console.WriteLine(displayBaseStationWithFreeChargSlots[i].ToString());
                    }
                    break;
                default:
                    break;
            }
        }


        static void Main(string[] args)
        {
            DalObject.DalObject dalObject = new DalObject.DalObject();
            
            Options options;
            int choice = 0;
            do
            {
                Console.WriteLine(@"
choose from the following options (type the selected number): 

1. Insert options.
2. Update options.
3. Display options(singel).
4. Display options (for the whole list).
5. EXIT.
Your choice:");
                while (!int.TryParse(Console.ReadLine(), out choice)) ;
                options = (Options)choice;
                switch (options)
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
                    case Options.EXIT:
                        Console.WriteLine("Have a good day");
                        break;
                    default:
                        break;
                }
            } while (!(choice == 5));
        }
    }
}
