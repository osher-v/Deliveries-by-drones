
using System;
using IDAL.DO;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public DroneStatuses Status { get; set; }
            public override string ToString()
            {
                return string.Format("id is: {0}\t model of the drone is: {1}\t drone type is: {2}\t" +
                    "Battery level is: {3}\t  drone status is: {4}\t ", Id, Model, MaxWeight, Battery, Status);
            }
        }
    }
}






