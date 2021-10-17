
using System;
using IDAL.DO;

namespace IDAL
{
    namespace DO
    {
   
        public struct Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public override string ToString()
            {
                return string.Format("id is: {0}\t Customer's name: {1}\t Customer's phone naumber: {2}\t" +
                    "Longitude location: {3}\t  Latitude location: {4}\t ", Id, Name, PhoneNumber, Longitude, Latitude);
            }
        }


    }

}

