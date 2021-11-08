using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public abstract class CustomerParent
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string PhoneNumber { get; set; }

            public override string ToString()
            {
                return string.Format("the Id is {0,-5} \t the name is {1,-5} \t" +
                    "the PhoneNumber is {2,-5}", Id, Name, PhoneNumber);
            }

        }
    }  
}
