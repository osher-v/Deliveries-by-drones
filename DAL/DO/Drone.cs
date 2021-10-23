
using System;
using IDAL.DO;

namespace IDAL
{
    /// <summary>
    /// drone data structure contains the identity number of the glider,
    /// its model, its weight category, its charge level and the status
    /// in which it is located.
    /// </summary>
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

                return string.Format("id is: {0,-9}\t model of the drone is: {1,-7}\t drone type is: {2,-7}\t" +
                    "Battery level is: {3,-5}\t  drone status is: {4,-7}\t ", Id, Model, MaxWeight, Battery, Status);
            }
        }
    }
}