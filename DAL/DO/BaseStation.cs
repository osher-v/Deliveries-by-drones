
using System;
using IDAL.DO;
using DalObject;
namespace IDAL
{
    namespace DO
    {
        public struct BaseStation
        {
            public int Id { get; set; }
            public string StationName { get; set; }
            public int FreeChargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return string.Format("id is: {0}\t name of the station is: {1}\t number of charge slots: {2}\t" +
                    "Longitude: {3}\t  Latitude: {4}\t ", Id, StationName, FreeChargeSlots, Longitude, Latitude);
            }
        }
    }
}