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
        /// abstract class of baseStation.
        /// </summary>
        public abstract class BaseStationParnt
        {
            public int Id { get; set; }
            public string Name { get; set; }       
            public int FreeChargeSlots { get; set; }

            public override string ToString()
            {
                return string.Format("Id is:{0,-8}\t name is:{1,-8}\t number of free charge slots:{2,-4}\t", Id, Name, FreeChargeSlots);
            }

        }
    }
}
