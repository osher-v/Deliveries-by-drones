using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        /// <summary>
        /// The class specifies location by longitude and latitude.
        /// </summary>
        public class Location
        {
            public double longitude { get; set; }
            public double latitude { get; set; }

            public override string ToString()
            {
                string convertLongitude = DalObject.DalObject.ConvertDecimalDegreesToSexagesimal(longitude, (IDAL.DO.LongitudeAndLatitude)1);
                string convertLatitude = DalObject.DalObject.ConvertDecimalDegreesToSexagesimal(latitude, (IDAL.DO.LongitudeAndLatitude)2);
                return string.Format(" longitude is: {0,-8} \t  latitude is {1,-8} ", convertLongitude, convertLatitude);
            }
        }
    }
}
