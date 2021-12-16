using System;

namespace DO
{
    /// <summary>
    /// Data structure for charging stations indicates the 
    /// station number and the drone number in charging.
    /// </summary>
    public struct DroneCharge
    {
        public int StationId { get; set; }
        public int DroneId { get; set; }
        public DateTime StartChargeTime { get; set; }
        public override string ToString()
        {
            return string.Format("Station id is: {0,-9}\t DroneId: {1,-9} \t Start charge time: ", StationId, DroneId, StartChargeTime);
        }
    }
}

