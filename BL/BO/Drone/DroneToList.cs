using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class DroneToList : DroneParent
        {
            //מספר חבילה מועברת אם יש?
            public int NumberOfLinkedParcel { get; set; }

            public override string ToString()
            {
                return base.ToString()+ string.Format("the Linked Parcel is: {0,-5}", NumberOfLinkedParcel); ;
            }
        }
    }
}
