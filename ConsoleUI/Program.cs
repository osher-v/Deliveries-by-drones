using System;

namespace ConsoleUI
{
    //enum Options {Insert=1,Update,Display,DisplayList,EXIT}
    class Program
    {
        static void Main(string[] args)
        {
            //DateTime time = new DateTime(2021, 10, 20);
            //DateTime time1 = new DateTime(2021, 10, 20);
            //Console.WriteLine(time.AddDays(5));
            //time = time1.AddDays(5);
            //Console.WriteLine();

            int choice = 0;
            do
            {
               // Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(@"
choose from the following options (type the selected number): 

1. Insert options.
2. Update options.
3. Display options(singel).
4. Display options (for the whole list).
5. EXIT.
Your choice:");
                
               while(!int.TryParse(Console.ReadLine(), out choice));
                switch (choice)
                {
                    case 1:
                       // Console.BackgroundColor = ConsoleColor.Blue;
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
                        break;

                    case 2:
                        //Console.BackgroundColor = ConsoleColor.Yellow;
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
                        break;

                    case 3:
                        Console.WriteLine(@"
Display options(singel):

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
                        break;

                    case 4:
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
                            default:
                                break;
                        }
                        break;
                    case 5:
                        Console.WriteLine("Have a good day");
                        break;         
                    default:
                        break;
                }
            } while (!(choice==5));
        }
    }
}
