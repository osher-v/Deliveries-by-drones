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
        /// abstract class of parcel.
        /// </summary>
        public abstract class ParcelParent
        {
            public int Id { get; set; }

            public WeightCategories Weight { get; set; }

            public Priorities Prior { get; set; }

            public override string ToString()
            {
                return string.Format("ID is:{0,-8}\t Weight Categorie:{1,-8}\t Prioritie:{2,-8}", Id, Weight, Prior);
            }
        }
    }
}
