using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Parcel :ParcelParent
        {
            public Drone MyDrone { get; set; }
            public DateTime Requested { get; set; }
            public DateTime Assigned { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

             public override string ToString()
            {
                return base.ToString() + string.Format("{0}\t Request started in: { 6,-12}\t Assigned: { 7,-12}\t" +
                    "pick up time at: {8,-12}\t  arivel time at: {9,-12}\t",MyDrone,Requested,Assigned,PickedUp,Delivered);
                   
            }
        }
    }
}
