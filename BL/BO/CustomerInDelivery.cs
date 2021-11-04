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
        /// 
        /// </summary>
        class CustomerInDelivery
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public override string ToString()
            {
                return string.Format( "Id of the customer is {0, -3} \t  Name of the customer is {1, -3}", Id ,Name);
            }
        }
    }
}
