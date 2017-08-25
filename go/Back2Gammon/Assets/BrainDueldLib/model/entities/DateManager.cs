
using System.Collections.Generic; using System;

using System.Text;


namespace BrainDuelsLib.model.entities
{
    public class DateManager
    {
        public static string FORMAT = "dd/mm/yyyy";
        public static char delimiter = '/';

        public static DateTime? ParseDate(string input)
        {
            string[] items = input.Split(delimiter);
            if (items.Length == 3)
            {
                int day = 0;
                int month = 0;
                int year = 0;
                bool good = int.TryParse(items[0], out day) && int.TryParse(items[1], out month) && int.TryParse(items[2], out year);
                if (!good)
                {
                    return null;
                }
                DateTime res = new DateTime(year, month, day);
                return res;
            }
            else
            {
                return null;
            }
        }
    }
}
