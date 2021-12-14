using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.others
{
    public class BlFactory
    {
        public static IBL.IBL GetBL() => IBL.BL.Instance;
    }
}
