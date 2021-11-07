using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        abstract class ParcelParent
        {
            public int Id { get; set; }
            //public int SenderId { get; set; }
            //public int ReciverId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Prior { get; set; }

            public override string ToString()
            {
                return String.Format("ID is:{0,-8}\t Weight Categorie:{2,-8}\t Prioritie:{3,-8}"
                   , Id, Weight, Prior);
                /*
                return String.Format("ID is:{0,-8}\t Sender ID is:{1,-8}\t Reciver ID is:{1,-8}\t Weight Categorie:{2,-8}\t Prioritie:{3,-8}"
                    ,Id,SenderId,ReciverId,Weight,Prior);
                */
            }
        }
    }
}
