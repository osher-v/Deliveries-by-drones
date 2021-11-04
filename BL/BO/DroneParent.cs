using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class DroneParent
        {
            public int Id { get; set; }

            public string Model { get; set; }

            public WeightCategories MaxWeight { get; set; }

            public int MyProperty { get; set; }
        }
    }  
}
