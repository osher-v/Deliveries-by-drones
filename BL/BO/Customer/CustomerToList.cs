using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// class of customer to List.
    /// </summary>
    public class CustomerToList : CustomerParent
    {
        public int NumberOfPackagesSentAndDelivered { get; set; }

        public int NumberOfPackagesSentAndNotYetDelivered { get; set; }

        public int NumberOfPackagesWhoReceived { get; set; }

        public int NumberPackagesOnTheWayToTheCustomer { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("the Number Of Packages Sent And Delivered is: {0,-5} \n " +
                "the Number Of Packages Sent And Not Yet Delivered is: {1,-5} \n" +
                "the Number Of Packages Who received is: {2,-5} \n" +
                "the Number Packages On The Way To The Customer is: {3,-5}\n" + "~~~~~~~~~~~"
                , NumberOfPackagesSentAndDelivered, NumberOfPackagesSentAndNotYetDelivered,
                NumberOfPackagesWhoReceived, NumberPackagesOnTheWayToTheCustomer);
        }
    }
}

