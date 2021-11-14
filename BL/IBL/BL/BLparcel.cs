using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    //public partial class BLparcel
    public partial class BL
    {
        public void AddParcel(Parcel newParcel)
        {
            IDAL.DO.Parcel parcel = new IDAL.DO.Parcel()
            {
                Id = newParcel.Id,
                SenderId=newParcel.Sender.Id,
                TargetId= newParcel.Receiver.Id,
                Weight =(IDAL.DO.WeightCategories)newParcel.Weight,
                Priority=(IDAL.DO.Priorities)newParcel.Prior,
                Assigned=DateTime.Now,
                DroneId=0 // לשאול את דן   
            };

            try
            {
                AccessIdal.AddParcel(parcel);
            }
            catch { }
        }
    }
}