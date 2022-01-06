using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;

namespace BL
{
    class Simulator
    {
        BlApi.IBL bl;

        enum StatusSim {Start, onGo, onCharge }
        private const int Delay = 1000;
        private const double Speed = 1.0;
        private const double Step   = Speed/ TimeStep;
        private const double TimeStep = Delay / 1000.0;


        public Simulator(BlApi.IBL _bl,int droneID, Action action, Func<bool> func)
        {
            bl = _bl;
            var dal = bl;
            //area for seting puse time




        }
    }
}
