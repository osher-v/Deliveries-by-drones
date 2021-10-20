using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IDAL.DO;
using System.ComponentModel;
namespace ConsoleUI
{
    enum Options {Insert=1,Update, DisplaySingle, DisplayList,EXIT}
    enum InsertrOption { baseStation = 1, Drone, AddCustomer, Shipment}
    enum DisplaySingleOption { BaseStationView = 1, Dronedisplay, CustomerView, PackageView }
     enum DisplayListOptions{ ListOfBaseStationView = 1, ListOfDronedisplay, ListOfCustomerView,
        ListOfPackageView, ListOfFreePackageView, BaseStationWhisFreeChargingView }
    class Program
    {
          static public void InsertOptions()
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
                    Console.WriteLine(@"Next Please enter the name of the station:");
                    name=Console.ReadLine();
                    Console.WriteLine(@"Next Please enter the number of charging slots at the station:");
                    while (!int.TryParse(Console.ReadLine(), out chargsSlots)) ;
                    Console.WriteLine(@"Next Please enter the longitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out longitude)) ;
                    Console.WriteLine(@"Next Please enter the latitude of the station:");
                    while (!double.TryParse(Console.ReadLine(), out latitude)) ;
                    SetStation(ID, name, chargsSlots, longitude, latitude);
                    break;
                case InsertrOption.Drone:
                    int droneID ;
                    string model,;
                    double a ;
                    Console.WriteLine(@"
You have selected to add a new Drone.
Please enter an ID number for the new drone:");
                    while (!int.TryParse(Console.ReadLine(), out droneID)) ;
                    Console.WriteLine(@"Next Please enter the model of the Drone:");
                    model = Console.ReadLine();
                    Console.WriteLine(@"Next enter the weight category of the new Drone:");
                    while (!int.TryParse(Console.ReadLine(), out chargsSlots)) ;
                    Console.WriteLine(@"Next enter the charge level of the battery:");
                    while (!double.TryParse(Console.ReadLine(), out longitude)) ;
                    Console.WriteLine(@"Next Enter the status of the Drone:");
                    while (!double.TryParse(Console.ReadLine(), out latitude)) ;
                    AddDrone(ID, name, chargsSlots, longitude, latitude);  
                    break;
                case InsertrOption.AddCustomer:
                    break;
                case InsertrOption.Shipment:
                    break;
                default:
                    break;
            }
        }
        static public void UpdateOptions()
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
            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
        }
        static public void DisplaySingleOptions()
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
                    DalObject.DalObject.GetBaseStation(idForViewObject).ToString();
                    break;
                case DisplaySingleOption.Dronedisplay:
                    Console.WriteLine("Insert ID number of requsted drone:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);
                    GetDrone(idForViewObject).toString;
                    break;
                case DisplaySingleOption.CustomerView:
                    Console.WriteLine("Insert ID number of requsted Customer:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);
                    GetCustomer(idForViewObject).toString;
                    break;
                case DisplaySingleOption.PackageView:
                    Console.WriteLine("Insert ID number of reqused parcel:");
                    int.TryParse(Console.ReadLine(), out idForViewObject);
                    GetParcel(idForViewObject).toString;
                    break;
                default:
                    break;
            }
        }
        static public void DisplayListOptions()
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
            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                default:
                    break;
            }
        }
        static void Main(string[] args)
        {
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
                
               while(!int.TryParse(Console.ReadLine(), out choice));
                options = (Options)choice;                      
                switch (options)
                {
                    case Options.Insert:
                       InsertOptions();
                        break;

                    case Options.Update:
                        UpdateOptions();
                        break;

                    case Options.DisplaySingle:
                        DisplaySingleOptions();
                        break;

                    case Options.DisplayList:
                        DisplayListOptions();
                        break;
                    case Options.EXIT:
                        Console.WriteLine("Have a good day");
                        break;         
                    default:
                        break;
                }
            } while (!(choice==5));
        }
    }
}
