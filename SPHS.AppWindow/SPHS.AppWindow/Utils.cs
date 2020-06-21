using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPHS.AppWindow
{
    public class Utils
    {
        public static bool compareStringLike(string str1, string str2)
        {
            return str1.ToUpper() == str2.ToUpper();
        }

        // return -1: data input error
        // return seconds
        public static double subDateTime(string dt1, string dt2)
        {
            try
            {
                TimeSpan result = DateTime.Parse(dt1) - DateTime.Parse(dt2);
                return result.TotalSeconds;
            }
            catch
            {
                return -1;
            }
        }
    }
}
