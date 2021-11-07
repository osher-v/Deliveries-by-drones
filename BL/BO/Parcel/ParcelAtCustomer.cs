using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        //חבילה אצל לקוח
        class DeliveryToACustomer
        {
            public int Id { get; set; }

            public WeightCategories Weight { get; set; }

            public Priorities Prior { get; set; }

            public DeliveryStatus Status { get; set; }

            public int MyProperty { get; set; }//??????????????????????????????

            public override string ToString()
            {
                return string.Format("Id is: {0,-5} \t  weight is: {1,-5} \t prior is: {2,-5}\t" +
                    "status is: {3,-5} \t the MyProperty is: {4,-6}", Id, Weight, Prior, Status, MyProperty);
            }
        }
    } 
}
