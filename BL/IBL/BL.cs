
using IBL.BO;
//using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBL
{
    public class BL: IBL
    {
        public IDal.IDal idal;

        public List<DroneToList> Drones;

        public static double Free;
        public static double LightWeightCarrier;
        public static double MediumWeightBearing;
        public static double CarriesHeavyWeight;
        public static double DroneLoadingRate;

        public BL()
        {
            IDal.IDal idal = new DalObject.DalObject();

            Drones = new List<DroneToList>();

            double[] arr = idal.RequestPowerConsumptionByDrone();

            List<IDAL.DO.Drone> drones = idal.GetDroneList().ToList();
            DroneToList temp = new DroneToList();
            for (int i = 0; i < drones.Count; i++)
            {
                temp.Id = drones[i].Id;
                temp.Model = drones[i].Model;
                temp.MaxWeight = (WeightCategories)drones[i].MaxWeight;

                Drones.Add(temp);

            }
            List<IDAL.DO.Parcel> Parcels = idal.GetParcelList(i => i.DroneId != 0 && i.Delivered == DateTime.MinValue).ToList();

            foreach (var item in Drones)
            {
                int index = Parcels.FindIndex(x => x.DroneId == item.Id);
                //if (item.Id == Parcels.Exists())
                if(index != -1)
                {
                    item.Statuses = DroneStatuses.busy; 
                    if(Parcels[index].PickedUp == DateTime.MinValue)
                    {
                       // item.CurrentLocation = 



                    }
                }




            }
                


















        }




    }
}
