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
                SenderId = newParcel.Sender.Id,
                TargetId = newParcel.Receiver.Id,
                Weight = (IDAL.DO.WeightCategories)newParcel.Weight,
                Priority = (IDAL.DO.Priorities)newParcel.Prior,
                Assigned = DateTime.Now,
                DroneId = 0 // לשאול את דן   
            };

            try
            {
                AccessIdal.AddParcel(parcel);
            }
            catch { }
        }
        public Parcel GetParcel(int idForDisplayObject)
        {
            IDAL.DO.Parcel printParcel = AccessIdal.GetParcel(idForDisplayObject);
            DroneToList droneToLIist= DronesBL.Find(x => x.Id == printParcel.DroneId);
            CustomerInDelivery senderInDelivery = new CustomerInDelivery() {Id=printParcel.SenderId,Name = AccessIdal.GetCustomer(printParcel.SenderId).Name };
            CustomerInDelivery reciverInDelivery = new CustomerInDelivery() { Id = printParcel.TargetId, Name = AccessIdal.GetCustomer(printParcel.TargetId).Name };
            Parcel parcel = new Parcel() { Id=printParcel.Id,Weight= (WeightCategories)printParcel.Weight, Prior= (Priorities)printParcel.Priority ,
                Sender=senderInDelivery, Receiver=reciverInDelivery, Requested=printParcel.Requested, Assigned=printParcel.Assigned,
                PickedUp=printParcel.PickedUp, Delivered=printParcel.Delivered};
            if (parcel.Assigned!=DateTime.MinValue)
            {
                DroneInThePackage droneInThePackage = new DroneInThePackage()
                {
                    Id = droneToLIist.Id,
                    BatteryStatus = droneToLIist.BatteryStatus,
                    CurrentLocation = droneToLIist.CurrentLocation
                };
                parcel.MyDrone = droneInThePackage;
            }

            return parcel;
        }

    }
}