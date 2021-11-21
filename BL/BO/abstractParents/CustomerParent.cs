using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        /// <summary>
        /// abstract class of customer.
        /// </summary>
        public abstract class CustomerParent
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string PhoneNumber { get; set; }

            public override string ToString()
            {
                return string.Format("the Id is: {0,-5}\tthe name is: {1,-5} \n" +
                    "the PhoneNumber is: {2,-12}\n", Id, Name, PhoneNumber);
            }

        }
    }  
}
