using System;

namespace IMSService.Helper
{
    public static class Const
    {
        public const string ImsConnectionString = "ImsConnection";
        public const string Admin = "A";
        public const string User = "U";
        public const string All = "All";
        public const string ServerPath = "path";
        public static readonly string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public const string Done = "Done";

        #region ==ErrorType==

        public const int InvoiceSettlementNotFound = -19;
        public const int DuplicateSettlement = -18;
        public const int InvoiceProductNotFound = -17;
        public const int SalesDetailsNotFound = -16;
        public const int CreditNoteDetailsNotFound = -15;
        public const int InvalidPendingInvoiceStatus = -14;
        public const int InvoiceStatusNotFound = -13;
        public const int SubCategoryNotFound = -12;
        public const int CategoryNotFound = -11;
        public const int GroupNotFound = -10;
        public const int VendorIDNotFound = -9;
        public const int NameNotAvailable = -8;
        public const int InvoiceDetailsNotFound = -7;
        public const int InvoiceCancelled = -6;
        public const int InvoiceReturned = -5;
        public const int InvoiceReceived = -4;
        public const int ProductNotExists = -3;
        public const int Duplicate = -2;
        public const int Error = -1;
        public const int NetworkError = 0;
        public const int Success = 1;

        #endregion ==ErrorType==

        #region ==SheetType==

        public const int Invoicing = 1;
        public const int Settlement = 2;
        public const int Catalogue = 3;
        public const int Purchase = 4;

        public const int InvoiceDetails = 5;
        public const int InvoiceProduct = 6;
        public const int InvoiceSettlement = 7;

        #endregion ==SheetType==
    }
}