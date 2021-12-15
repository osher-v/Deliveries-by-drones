using System;
using DO;

namespace DalApi
{
    /// <summary>
    /// help fanctions.
    /// </summary>
    public class fanctions
    {
        #region Convert decima to sexagesimal function (Bonus)

        /// <summary>
        /// A function that converts from decimal to Sexagesimal
        /// </summary>
        /// <param name="decimalValueToConvert">The number to convert</param>
        /// <param name="side"></param>
        /// <returns>string that hold the convert location</returns>
        public static string ConvertDecimalDegreesToSexagesimal(double decimalValueToConvert, DO.LongitudeAndLatitude side)
        {
            string daricton = null;
            switch (side)
            {
                case LongitudeAndLatitude.Longitude:
                    if (decimalValueToConvert >= 0)
                        daricton = "N";
                    else daricton = "S";
                    break;

                case LongitudeAndLatitude.Latitude:
                    if (decimalValueToConvert >= 0)//chack the number if its too east or weast
                        daricton = "E";
                    else daricton = "W";
                    break;

                default:
                    break;
            }
            
            int sec = (int)Math.Round(decimalValueToConvert * 3600);
            int deg = sec / 3600;
            sec = Math.Abs(sec % 3600);
            int min = sec / 60;
            sec %= 60;

            return string.Format("{0}° {1}' {2}'' {3}", Math.Abs(deg), Math.Abs(min), Math.Abs(sec), daricton);// return the complited number
        }

        #endregion Convert decima to sexagesimal function (Bonus)
    }
}
