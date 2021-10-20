using System;

namespace ConsoleUI
{
    enum Options {Insert=1,Update, DisplaySingle, DisplayList,EXIT}
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
