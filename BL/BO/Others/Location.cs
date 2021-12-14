using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DalFacade;

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
            //public string LocationStringh { get; set; }
            public override string ToString()
            {
                string convertLongitude = fanctions.ConvertDecimalDegreesToSexagesimal(longitude, (DO.LongitudeAndLatitude)1);
                string convertLatitude = fanctions.ConvertDecimalDegreesToSexagesimal(latitude, (DO.LongitudeAndLatitude)2);
                //return string.Format(" longitude is: {0,-8} \t  latitude is {1,-8} ", convertLongitude, convertLatitude);
                return string.Format("{0,-8} \t {1,-8} ", convertLongitude, convertLatitude);
            }

        }
    }
}
