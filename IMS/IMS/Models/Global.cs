namespace IMS.Models
{
    public class Global
    {
        public static int UserID { get; set; }
        public static string UserName { get; set; }
        public static int UserType { get; set; }

        protected Global()
        {
        }

        public static int GlobalPageMode { get; set; }
        public static int GlobalNavigationValue { get; set; }
        public static bool DashBoard { get; set; } = true;
    }
}