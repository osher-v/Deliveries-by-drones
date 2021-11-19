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
        /// class of baseStations to List.
        /// </summary>
        public class BaseStationsToList : BaseStationParnt
        {
            public int BusyChargeSlots { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("number of busy charge slots:{0,-5}\n ~~~~~~~", BusyChargeSlots);
            }
        }
    }
}
