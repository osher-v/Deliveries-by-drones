
using System;
using IDAL.DO;

namespace IDAL
{
    namespace DO
    {   
        public struct DroneCharge
        {
            public int StationId { get; set; }
            public int DroneId { get; set; }
            public override string ToString()
            {
                return string.Format("Station id is: {0}\t DroneId: {1} ", StationId, DroneId);
            }
        }
    }
}
