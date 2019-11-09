namespace IMS.Helper
{
    public class PageMode
    {
        public const int Browse = 1;
        public const int Add = 2;
        public const int Modify = 3;
        public const int Delete = 4;

        protected PageMode()
        {
        }
    }

    public class Const
    {
        public static readonly string HostedPath = System.Configuration.ConfigurationManager.AppSettings["HostedPath"];
        public const string AppPath = "";

        #region ==Lookup==

        public const int Screen = 1;
        public const int Size = 2;
        public const int PosterType = 3;
        public const int Publisher = 4;

        ////public const int Category = 5;
        ////public const int SubCategory = 6;
        public const int PaperFinish = 7;

        public const int Shape = 8;
        public const int Orientation = 9;
        public const int WarrantySummary = 10;
        public const int SheetType = 11;
        public const int UserType = 12;
        public const int Gender = 13;
        public const int InvoiceStatus = 14;
        public const int SocialName = 15;
        public const int PaperType = 16;
        public const int YesNo = 17;
        public const int ProductGroup = 18;

        #endregion ==Lookup==

        #region ==Screens==

        public const int Home = 1;
        public const int Logout = 2;
        public const int About = 3;
        public const int Help = 4;
        public const int Exit = 5;
        public const int Calc = 6;
        public const int Lock = 7;
        public const int IE = 8;
        public const int Skype = 9;
        public const int Outlook = 10;
        public const int Music = 99;

        public const int UserMaster = 11;
        public const int Vendor = 12;
        public const int Menu = 13;
        public const int Product = 14;
        public const int Sales = 15;
        public const int Lookup = 16;
        public const int SubLookup = 17;
        public const int Purchase = 18;
        public const int Damage = 19;
        public const int Invoice = 21;
        public const int UploadSheet = 22;
        public const int Company = 23;
        public const int Category = 24;
        public const int SubCategory = 25;

        public const int InvoicePatch = 81;
        public const int BulkDeleteInvoicePatch = 82;
        public const int InvoiceDetailsPatch = 83;
        public const int InvoiceProductPatch = 84;
        public const int InvoiceSettlementPatch = 85;

        public const int InvoiceReport = 91;
        public const int StatuswiseInvoiceReport = 92;
        public const int FinalInvoiceReport = 93;
        public const int StockReport = 94;
        public const int TaxReport = 95;
        public const int SalesReport = 96;

        #endregion ==Screens==

        public const int Admin = 1;
        public const int User = 2;

        public const string Namespace = "IMS.Views";

        public const string DatabaseName = "Test";
        public const string DatabaseServer = "Data Source=localhost;Initial Catalog=Test;User ID=sa;Password=password@123;";

        #region ==Sheet Type==

        public const int InvoicingSheetType = 1;
        public const int SettlementSheetType = 2;
        public const int CatalogueSheetType = 3;
        public const int PurchaseSheetType = 4;

        public const int InvoiceDetailsSheetType = 5;
        public const int InvoiceProductSheetType = 6;
        public const int InvoiceSettlementSheetType = 7;

        protected Const()
        {
        }

        #endregion ==Sheet Type==
    }

    public class Search
    {
        public int ID { get; set; }
        public string Desc { get; set; }
        public string Val { get; set; }
    }
}