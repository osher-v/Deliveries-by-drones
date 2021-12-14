using System;
using DO;

namespace DalApi
{
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
            //int degrees = (int)decimalValueToConvert;// we lose the numbers affter the dot.
            //int minutes = (int)((decimalValueToConvert - degrees) * 60);//we take the decimal number and we remove the number that we take before 
            //                                                            //and multiplay by 60 (becuse we want minuts)
            //float seconds = (float)((decimalValueToConvert - degrees - (minutes / 60)) * 3600);//and multiplay by 3600 (becuse we want seconds)
            int sec = (int)Math.Round(decimalValueToConvert * 3600);
            int deg = sec / 3600;
            sec = Math.Abs(sec % 3600);
            int min = sec / 60;
            sec %= 60;


            return String.Format("{0}° {1}' {2}'' {3}", Math.Abs(deg), Math.Abs(min), Math.Abs(sec), daricton);// return the complited number
        }

        #endregion Convert decima to sexagesimal function (Bonus)
    }
}
