using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.others
{
    public static class BlFactory
    {
        public static BlApi.IBL GetBL() => BlApi.BL.Instance;
    }
}
