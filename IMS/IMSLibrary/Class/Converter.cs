using System;

namespace IMSLibrary.Helper
{
    public class Converter
    {
        protected Converter()
        {
        }

        public static int ToInt(object val)
        {
            try
            {
                return Convert.ToInt32(val);
            }
            catch
            {
                return 0;
            }
        }

        public static string ToString(object val)
        {
            try
            {
                if (val == null || val.ToString() == " ") return string.Empty;
                return Convert.ToString(val).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static DateTime ToDateTime(object val)
        {
            try
            {
                DateTime resDt = Convert.ToDateTime(val);
                if (resDt < new DateTime(1900, 01, 01))
                    return new DateTime(1900, 01, 01);
                return resDt;
            }
            catch
            {
                return new DateTime(1900, 01, 01);
            }
        }

        public static decimal ToDecimal(object val)
        {
            try
            {
                return Convert.ToDecimal(val);
            }
            catch
            {
                return 0M;
            }
        }

        public static string ToMonth(object val)
        {
            try
            {
                switch (Converter.ToInt(val))
                {
                    case 1: return "January";
                    case 2: return "February";
                    case 3: return "March";
                    case 4: return "April";
                    case 5: return "May";
                    case 6: return "June";
                    case 7: return "July";
                    case 8: return "August";
                    case 9: return "September";
                    case 10: return "October";
                    case 11: return "November";
                    case 12: return "December";
                    default:
                        return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ToStatusString(object val)
        {
            try
            {
                if (val == null || val.ToString() == " ") return string.Empty;
                string status = Convert.ToString(val).Trim();
                if (status.Length > 1)
                {
                    return status[0].ToString().ToUpper() + status.Substring(1).ToLower();
                }
                return status.ToUpper();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}