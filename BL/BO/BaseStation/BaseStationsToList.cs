using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class BaseStationsToList : BaseStationParnt
        {
            public int BusyChargeSlots { get; set; }

            public override string ToString()
            {
                return base.ToString() + string.Format("number of busy charge slots: ", BusyChargeSlots);
            }

        }
    }
}
