using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// class of drone to List.
    /// </summary>
    public class DroneToList : DroneParent
    {
        public int NumberOfLinkedParcel { get; set; }

        public override string ToString()
        {
            return base.ToString() + string.Format("the number of Linked Parcel is: {0,-5}\n ~~~~~~~~~~~~~", NumberOfLinkedParcel); ;
        }
    }
}

