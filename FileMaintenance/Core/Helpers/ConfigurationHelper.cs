using System;

namespace FileMaintenance.Core.Helpers
{
    public class ConfigurationHelper
    {
        /// <summary>
        /// Parses a given string into days, hours and minutes.
        /// </summary>
        /// <param name="keepFor"></param>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <returns>Wheather the method suceeded or failed parsing the string.</returns>
        /// <remarks>Expected keepFor format is {"d.m:s"}.</remarks>
        public static bool TryParseKeepFor(string keepFor, out int days, out int hours, out int minutes)
        {
            days = 0;
            hours = 0;
            minutes = 0;

            string[] elements = keepFor.Split('.', ':');

            bool success = Int32.TryParse(elements[0], out days);
            success = success && Int32.TryParse(elements[1], out hours);
            success = success && Int32.TryParse(elements[2], out minutes);
            
            return success;
        }
    }
}
