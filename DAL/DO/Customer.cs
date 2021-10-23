
using System;
using IDAL.DO;

namespace IDAL
{
    namespace DO
    {
        /// <summary>
        /// Data structure for customers The structure contains the
        /// customer's identity number, his name, his telephone number.
        /// As well as the customer's waypoints.
        /// </summary>
        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return string.Format("id is: {0,9}\t Customer's name: {1,14}\t Customer's phone naumber: {2,14}\t" +
                    "Longitude location: {3,10}\t  Latitude location: {4,10}\t ", Id, Name, PhoneNumber, Longitude, Latitude);
            }
        }


    }

}

