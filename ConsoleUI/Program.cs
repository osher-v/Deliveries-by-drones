using System;

namespace ConsoleUI
{
    //enum Options {Insert=1,Update,Display,DisplayList,EXIT}
    class Program
    {
        static void Main(string[] args)
        {
           Console.WriteLine(@"
choose from the following options (type the selected number): 

1.Insert options.
2.Update options.
3.Display options(singal).
4.Display options (for the whole list).
5.EXIT.
Your choice:");
            int choice = 0;
            int.TryParse( Console.ReadLine(),out choice);
            switch (choice)
            {
                case 1:
                    Console.WriteLine(@"
Insert options:

1. Add a base station to the list of stations.
2. Add a drone to the list of existing drones.
3. Admission of a new customer to the customer list.
4. Admission of package for shipment.

Your choice:");
                       int.TryParse(Console.ReadLine(), out choice);

                    break;
                case 2:
                    Console.WriteLine(@"
Insert options:

1. Add a base station to the list of stations.
2. Add a drone to the list of existing drones.
3. Admission of a new customer to the customer list.
4. Admission of package for shipment.

Your choice:");

                    break;
                case 3:
                    Console.WriteLine(@"
Insert options:

1. Add a base station to the list of stations.
2. Add a drone to the list of existing drones.
3. Admission of a new customer to the customer list.
4. Admission of package for shipment.

Your choice:");
                    break;
                case 4:
                    Console.WriteLine(@"
Insert options:

1. Add a base station to the list of stations.
2. Add a drone to the list of existing drones.
3. Admission of a new customer to the customer list.
4. Admission of package for shipment.

Your choice:");
                    break;
                case 5:
                    Console.WriteLine(@"
Insert options:

1. Add a base station to the list of stations.
2. Add a drone to the list of existing drones.
3. Admission of a new customer to the customer list.
4. Admission of package for shipment.

Your choice:");
                    break;
                default:
                    break;
            }



















        }
    }
}
