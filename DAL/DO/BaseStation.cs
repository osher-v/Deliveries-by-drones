
using System;
using IDAL.DO;
using DalObject;
namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Data structure for drone stations, the structure contains 
        /// the ID number, the name of the station, 
        /// the number of available charging points
        /// as well as the landmarks of the station.
        /// </summary>
        public struct BaseStation
        {
            public int Id { get; set; }
            public string StationName { get; set; }
            public int FreeChargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return string.Format("id is: {0,9}\t name of the station is: {1,14}\t number of charge slots: {2,2}\t" +
                    "Longitude: {3,12}\t  Latitude: {4,12}\t ", Id, StationName, FreeChargeSlots, Longitude, Latitude);
            }
        }
    }
}