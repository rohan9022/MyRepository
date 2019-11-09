using IMSLibrary;
using IMSLibrary.Helper;
using IMSService.Entities;
using IMSService.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace IMSService.Services
{
    [ServiceContract]
    public interface IDataService
    {
        #region ==Menu==

        #region Description

        /*
         * GetMenuList
         * Parameter Status Type
         * Description :- Will Retrive MenuItems from Database which will be used in the Project and Will ,When item will be selected Particular page will open,
         * Status Type:-
         * 1 :-Activate(Will Display Activated Items
         * 2 :- De-activate :- Will display Deactivate Items
         * 3:- All :- All Items
         *  Stored Procedure:- sp_mm_GetMenuList
         */

        #endregion Description

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<MenuMaster> GetMenuList(int StatusType);

        #region Description

        /*
        * InsertUpdateMenuMaster
        * Description :- Will Perform Insert Update Operations
        */

        #endregion Description

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateMenuMaster(MenuMaster objMenu);

        #region Description

        /*
        * DeleteMenuMaster
        * Description :- Will Perform Delete Operation
        */

        #endregion Description

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteMenuMaster(int MenuID);

        #endregion ==Menu==

        #region ==Lookup==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool CheckLookupName(string Name);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool CheckSubLookupName(string Name, int LookupID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateDeleteLookup(string Name, int LookupID, int UserID, int Mode);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateDeleteSubLookup(string Name, int LookupID, int SubLookupID, int UserID, int Mode);

        #region Description

        /*
        * GetLookupList
        * Description : Retrives LookUp Master Data
        * Retrive Look Name ,and Lookup ID
        * Returns List
        * Stored Procedure :- sp_lkm_GetLookupList
        */

        #endregion Description

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<Lookup> GetLookupList();

        #region Description

        /*
         * GetSubLookupList
         * Description : Retrives SubLookup List Data ,based on LookupType
         * Returns List
         * Stored Procedure :- sp_slm_GetSubLookupList
         */

        #endregion Description

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<Lookup> GetSubLookupList(int LookupType);

        #endregion ==Lookup==

        #region ==UploadSheet==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        Tuple<string, string, bool> UploadSheet(byte[] sheetData, string FileName, int UserID, int SheetType);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        byte[] GetTemplate(int SheetType);

        #endregion ==UploadSheet==

        #region ==User==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool CheckUserNameAvailability(string UserName);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        UserList IsValidUser(string Username, string Password);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        ObservableCollection<UserList> GetUserList();

        [OperationContract]
        [FaultContract(typeof(DataService))]
        UserDetails GetUserDetails(int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateUserDetails(UserDetails objUser, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteUser(int UserID);

        #endregion ==User==

        #region ==Product==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<ProductList> GetProductList();

        [OperationContract]
        [FaultContract(typeof(DataService))]
        Product GetProductDetails(string ProductID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateProductMaster(Product objProduct, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteProduct(string ProductID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        ObservableCollection<Product> CheckStock(int Quantity, string ProductID, int SearchMode);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<Purchase> GetPurchaseDetails(string ProductID, DateTime PurchaseDate);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateDeletePurchase(int Mode, Purchase objPurchase, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<Sales> GetSalesDetails(string ProductID, DateTime SalesDate);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateDeleteSales(int Mode, Sales objSales, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<Damage> GetDamagedGoodsDetails(string ProductID, DateTime DamageDate);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateDeleteDamage(int Mode, Damage objDamage, int UserID);

        #endregion ==Product==

        #region ==Invoice==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateInvoice(InvoiceMaster objInvoice, int UserID, int Mode);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        InvoiceMaster GetInvoiceDetails(int InvoiceNo, string OrderID, int SearchMode);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteInvoice(int InvoiceNo);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        ObservableCollection<InvoiceReport> GenInvoiceReport(int fromInvoiceNo, int toInvoiceNo, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<InvoiceReport> FinalInvoiceReport(int fromInvoiceNo, int toInvoiceNo, DateTime fromDate, DateTime toDate);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        ObservableCollection<InvoiceStatus> StatuswiseInvoiceReport(DateTime fromDate, DateTime toDate, int CategoryID, int SubCategoryID, int GroupID, int VendorID, int Status);

        #endregion ==Invoice==

        #region ==Vendor==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        ObservableCollection<VendorMaster> GetVendorList();

        [OperationContract]
        [FaultContract(typeof(DataService))]
        VendorMaster GetVendorDetails(int VendorID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateVendor(VendorMaster objVendor, int UserID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteVendor(int VendorID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool CheckVendorNameAvailability(string VendorName);

        #endregion ==Vendor==

        #region ==Company==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        CompanyMaster GetCompanyDetails();

        #endregion ==Company==

        #region ==Category==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool CheckCategoryName(string Name, int CategoryID, int PageMode);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool CheckSubCategoryName(string Name, int CategoryID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateCategory(string Name, int CategoryID, decimal Cgst, decimal Sgst, decimal Igst);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool InsertUpdateSubCategory(string Name, int CategoryID, int SubCategoryID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<Category> GetCategoryList();

        [OperationContract]
        [FaultContract(typeof(DataService))]
        List<SubCategory> GetSubCategoryList(int CategoryID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteCategory(int CategoryID);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool DeleteSubCategory(int CategoryID, int SubCategoryID);

        #endregion ==Category==

        #region ==Patches==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        bool BulkDeleteInvoice(int frmInvoiceNo, int toInvoiceNo);

        #endregion ==Patches==

        #region ==Reports==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        ObservableCollection<TaxReport> MonthVendorTaxReport(int VendorID, DateTime FromDate, DateTime ToDate);

        [OperationContract]
        [FaultContract(typeof(DataService))]
        ObservableCollection<SalesReport> Rpt_Sales(DateTime FrmSalesDt, DateTime ToSalesDt, string PrdCd, int VendorID, int GroupID, int CategoryID, int SubCategoryID);

        #endregion ==Reports==

        #region ==CreditNote==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        ObservableCollection<CreditNoteReport> GenCreditNoteReport(string crNoteNo, string orderID, DateTime fromDate, DateTime toDate);

        #endregion ==CreditNote==

        [OperationContract]
        [FaultContract(typeof(DataService))]
        decimal FindGstRate(string ProductID, string TaxType);
    }

    public class DataService : IDataService
    {
        private string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[Const.ImsConnectionString].ConnectionString;
        }

        private SqlConnection sqlCon()
        {
            SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[Const.ImsConnectionString].ConnectionString);
            sqlCon.Open();
            return sqlCon;
        }

        #region ==Menu==

        public List<MenuMaster> GetMenuList(int StatusType)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intStatusType", StatusType)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_mm_GetMenuList", sqlParam);
                List<MenuMaster> lstMenu = new List<MenuMaster>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstMenu.Add(new MenuMaster
                    {
                        ID = (int)dr["ID"],
                        Name = dr["Name"].ToString(),
                        ParentID = (int)dr["ParentID"],
                        ScreenID = (int)dr["ScreenID"],
                        IsActive = (bool)dr["IsActive"]
                    });
                }
                return lstMenu;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateMenuMaster(MenuMaster objMenu)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intID", objMenu.ID),
                    [1] = new SqlParameter("@varcharName", objMenu.Name),
                    [2] = new SqlParameter("@intParentID", objMenu.ParentID),
                    [3] = new SqlParameter("@intScreenID", objMenu.ScreenID),
                    [4] = new SqlParameter("@boolIsActive", objMenu.IsActive)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_mm_InsertUpdateMenu", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteMenuMaster(int MenuID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intID", MenuID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_mm_DeleteMenu", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Menu==

        #region ==Lookup==

        public bool CheckLookupName(string Name)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@varcharLookupName", Converter.ToString(Name))
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_lkm_CheckLookupNameAvailability", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool CheckSubLookupName(string Name, int LookupID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@varcharSubLookupName", Converter.ToString(Name)),
                    [1] = new SqlParameter("@intLookupID", LookupID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_slm_CheckSubLookupNameAvailability", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateDeleteLookup(string Name, int LookupID, int UserID, int Mode)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@varcharLookupName", Name),
                    [1] = new SqlParameter("@intLookupID", LookupID),
                    [2] = new SqlParameter("@intCreatedModifiedBy", UserID),
                    [3] = new SqlParameter("@intMode", Mode)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_lkm_InsertUpdateDeleteLookup", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateDeleteSubLookup(string Name, int LookupID, int SubLookupID, int UserID, int Mode)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@varcharSubLookupName", Name),
                    [1] = new SqlParameter("@intLookupID", LookupID),
                    [2] = new SqlParameter("@intCreatedModifiedBy", UserID),
                    [3] = new SqlParameter("@intMode", Mode),
                    [4] = new SqlParameter("@intSubLookupID", SubLookupID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_slm_InsertUpdateDeleteSubLookup", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<Lookup> GetLookupList()
        {
            try
            {
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_lkm_GetLookupList", new Dictionary<int, SqlParameter>());
                List<Lookup> lstLookup = new List<Lookup>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstLookup.Add(new Lookup
                    {
                        ID = (int)dr["LookupID"],
                        Name = (string)dr["LookupName"]
                    });
                }
                return lstLookup;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<Lookup> GetSubLookupList(int LookupType)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intLookupID", LookupType)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_slm_GetSubLookupList", sqlParam);
                List<Lookup> lstSubLookup = new List<Lookup>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstSubLookup.Add(new Lookup
                    {
                        ID = (int)dr["SubLookupID"],
                        Name = (string)dr["SubLookupName"]
                    });
                }
                return lstSubLookup;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Lookup==

        #region ==UploadSheet==

        public Tuple<string, string, bool> UploadSheet(byte[] sheetData, string FileName, int UserID, int SheetType)
        {
            try
            {
                string ErrorCsvFileName = string.Empty, ErrorFolderPath = string.Empty, MainFolderPath = string.Empty, MainFileName = string.Empty;
                string dt = DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                if (SheetType == Const.Invoicing) { ErrorFolderPath = "Upload\\Sheets\\Invoicing\\" + dt + "\\" + UserID; }
                if (SheetType == Const.Settlement) { ErrorFolderPath = "Upload\\Sheets\\Settlement\\" + dt + "\\" + UserID; }
                if (SheetType == Const.Catalogue) { ErrorFolderPath = "Upload\\Sheets\\Catalogue\\" + dt + "\\" + UserID; }
                if (SheetType == Const.Purchase) { ErrorFolderPath = "Upload\\Sheets\\Purchase\\" + dt + "\\" + UserID; }

                string destFilePath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + ErrorFolderPath;
                MainFolderPath = destFilePath;
                if (!Directory.Exists(MainFolderPath))
                {
                    Directory.CreateDirectory(MainFolderPath);
                }
                MainFileName = ErrorCsvFileName = Guid.NewGuid().ToString() + FileName;
                string FilePath = destFilePath + "\\" + MainFileName;
                System.IO.File.WriteAllBytes(FilePath, sheetData);

                string con = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + ";Extended Properties ='Excel 8.0;HDR=Yes';";
                using (OleDbConnection OleDbCon = new OleDbConnection(con))
                {
                    OleDbCon.Open();
                    OleDbCommand oleDbCommand = new OleDbCommand();
                    if (SheetType == Const.Invoicing)
                    {
                        ////oleDbCommand = new OleDbCommand("Select [Date], [Invoice No],[Party's Name], [Order ID], [Order Date], [Address], [Email ID], [Mobile No], [Product], [Rate], [Quantity], [Total], [Net Sales], [VAT], [Packaging], [Invoice Amt], [Sales Group] From [Invoicing feed$] Where [Invoice No] > 0", OleDbCon);
                        oleDbCommand = new OleDbCommand("Select * From [Invoicing$]", OleDbCon);
                    }
                    else if (SheetType == Const.Settlement)
                    {
                        ////oleDbCommand = new OleDbCommand("Select [SettlementDate],[Order ID],[Invoice No],[Invoice Date],[Product Code],[Quantity],[Invoice Amt],[Settlement Amount],[Status] from [Amount Receivable$]", OleDbCon);
                        oleDbCommand = new OleDbCommand("Select * From [Settlement$]", OleDbCon);
                    }
                    else if (SheetType == Const.Catalogue)
                    {
                        ////oleDbCommand = new OleDbCommand("Select [Product Code], [Title], [Size], [MRP], [PosterType], [PosterDimensions],Dimensions in Inches, Poster Weight, Package Contents, Packaging Information, Shipping Duration, Variant, Color, Keywords," +
                        ////    "SocialName,Publisher,PaperType,Description,Category,SubCategory,ArtistName,PaintingStyle,BlackAndWhitePoster,ColorPoster,PaperFinish,Shape,Orientation,Framed,FrameMaterial,OtherFrameDetails,Width,Height,PaperDepth," +
                        ////    "Weight,OtherDimensions,OtherFeatures,SupplierImage,ImageLink,Note,WarrantSummary From MasterSheet");
                        oleDbCommand = new OleDbCommand("Select * From [Catalogue$] Where ModeType <> ''", OleDbCon);
                    }
                    else if (SheetType == Const.Purchase)
                    {
                        ////oleDbCommand = new OleDbCommand("Select [Product Code], [Title], [Size], [MRP], [PosterType], [PosterDimensions],Dimensions in Inches, Poster Weight, Package Contents, Packaging Information, Shipping Duration, Variant, Color, Keywords," +
                        ////    "SocialName,Publisher,PaperType,Description,Category,SubCategory,ArtistName,PaintingStyle,BlackAndWhitePoster,ColorPoster,PaperFinish,Shape,Orientation,Framed,FrameMaterial,OtherFrameDetails,Width,Height,PaperDepth," +
                        ////    "Weight,OtherDimensions,OtherFeatures,SupplierImage,ImageLink,Note,WarrantSummary From MasterSheet");
                        oleDbCommand = new OleDbCommand("Select * From [Purchase$]", OleDbCon);
                    }
                    OleDbDataReader dr = oleDbCommand.ExecuteReader();
                    ErrorCsvFileName = ErrorCsvFileName.Replace(".xlsx", ".csv");
                    ErrorFolderPath += "\\Error\\";
                    if (!Directory.Exists(MainFolderPath + "\\Error\\"))
                    {
                        Directory.CreateDirectory(MainFolderPath + "\\Error\\");
                    }
                    StreamWriter sw = new StreamWriter(MainFolderPath + "\\Error\\" + ErrorCsvFileName);
                    if (SheetType == Const.Invoicing) sw.WriteLine("InvoiceNo,Invoice Date,Party's Name, Order Date,Order ID,SKU,Rate,Quantity,Shipping & Handling,Sales Group,Tax Group,Address,Email ID,Mobile No,ErrorMessage");
                    if (SheetType == Const.Settlement) sw.WriteLine("Invoice No,Order ID,Invoice Date,SKU,Settlement Date,Settlement Amount,Status,ErrorMessage");
                    if (SheetType == Const.Catalogue) sw.WriteLine("SKU,Title,Size,MRP,Group,Category,SubCategory,ErrorMessage");
                    if (SheetType == Const.Purchase) sw.WriteLine("SKU,Quantity,MRP,Purchase Price,Selling Price,ErrorMessage");

                    while (dr.Read())
                    {
                        int invoiceNo = 0;
                        string orderID = string.Empty;
                        DateTime invoiceDate = new DateTime();
                        string productID = "";
                        string ErrorMsg = string.Empty;
                        int Result = 0;
                        try
                        {
                            Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>();
                            if (SheetType == Const.Invoicing)
                            {
                                int quantity = 0;
                                decimal rate, unitPrice, netSales, total;
                                decimal CGST = 0.00M, SGST = 0.00M, IGST = 0.00M;

                                decimal shipping_UnitPrice = 0.00M, shipping_NetSales = 0.00M, shipping_Total;
                                decimal shipping_CGST = 0.00M, shipping_SGST = 0.00M, shipping_IGST = 0.00M;

                                DateTime orderDate;
                                string TaxGroup, PartyName, Address, EmailID, MobNo, Vendor;

                                orderID = Converter.ToString(dr["Order ID"]);
                                if (string.IsNullOrEmpty(orderID)) continue;
                                orderDate = Converter.ToDateTime(dr["Order Date"]);
                                if (orderDate <= new DateTime(1900, 01, 01)) orderDate = DateTime.Now.Date;
                                invoiceNo = 0;
                                invoiceDate = Converter.ToDateTime(dr["Invoice Date"]);
                                if (invoiceDate <= new DateTime(1900, 01, 01)) invoiceDate = DateTime.Now.Date;

                                productID = Converter.ToString(dr["SKU"]).ToUpper();
                                quantity = Converter.ToInt(dr["Quantity"]);
                                rate = Converter.ToDecimal(dr["Rate"]);
                                TaxGroup = Converter.ToString(dr["Tax Group"]);
                                PartyName = Converter.ToString(dr["Party's Name"]);
                                Address = Converter.ToString(dr["Address"]);
                                EmailID = Converter.ToString(dr["Email ID"]);
                                MobNo = Converter.ToString(dr["Mobile No"]);
                                shipping_Total = Math.Round((Converter.ToDecimal(dr["Shipping & Handling"])), 2);
                                Vendor = Converter.ToString(dr["Sales Group"]);

                                #region ==SGST, CGST & IGST Calculation==

                                string TaxType = string.Empty;
                                decimal SGST_Perc = 0.00M, CGST_Perc = 0.00M, IGST_Perc = 0.00M;
                                string[] splitTax = null;
                                if (!string.IsNullOrEmpty(TaxGroup))
                                {
                                    splitTax = TaxGroup.Split('-');
                                    if (splitTax.Count() > 1)
                                    {
                                        TaxType = splitTax[0];
                                    }
                                    else
                                    {
                                        TaxType = TaxGroup;
                                    }
                                }

                                if (TaxType == "CSGST")
                                {
                                    if (splitTax.Count() > 1)
                                    {
                                        CGST_Perc = Converter.ToDecimal(splitTax[1]);
                                    }
                                    if (CGST_Perc <= 0.00M)
                                    {
                                        CGST_Perc = FindGstRate(productID, "CGST");
                                    }

                                    if (splitTax.Count() > 2)
                                    {
                                        SGST_Perc = Converter.ToDecimal(splitTax[2]);
                                    }
                                    if (SGST_Perc <= 0.00M)
                                    {
                                        SGST_Perc = FindGstRate(productID, "SGST");
                                    }

                                    unitPrice = Math.Round(((rate / (CGST_Perc + 100)) * 100), 2);
                                    CGST = (rate - unitPrice) * quantity;

                                    unitPrice = Math.Round(((rate / (SGST_Perc + 100)) * 100), 2);
                                    SGST = (rate - unitPrice) * quantity;

                                    unitPrice = (rate * quantity) - CGST - SGST;

                                    if (shipping_Total > 0.00M)
                                    {
                                        shipping_UnitPrice = Math.Round(((shipping_Total / (CGST_Perc + 100)) * 100), 2);
                                        shipping_CGST = shipping_Total - shipping_UnitPrice;

                                        shipping_UnitPrice = Math.Round(((shipping_Total / (SGST_Perc + 100)) * 100), 2);
                                        shipping_SGST = shipping_Total - shipping_UnitPrice;

                                        shipping_UnitPrice = (shipping_Total - shipping_CGST - shipping_SGST) / quantity;
                                        shipping_NetSales = shipping_UnitPrice * quantity;
                                    }
                                }
                                else
                                {
                                    if (splitTax.Count() > 1)
                                    {
                                        IGST_Perc = Converter.ToDecimal(splitTax[1]);
                                    }
                                    if (IGST_Perc <= 0.00M)
                                    {
                                        IGST_Perc = FindGstRate(productID, "IGST");
                                    }

                                    unitPrice = Math.Round(((rate / (IGST_Perc + 100)) * 100), 2);
                                    IGST = (rate - unitPrice) * quantity;

                                    if (shipping_Total > 0.00M)
                                    {
                                        shipping_UnitPrice = Math.Round(((shipping_Total / (IGST_Perc + 100)) * 100), 2);
                                        shipping_IGST = shipping_Total - shipping_UnitPrice;

                                        shipping_UnitPrice = (shipping_Total - shipping_IGST) / quantity;
                                        shipping_NetSales = shipping_UnitPrice * quantity;
                                    }
                                }

                                #endregion ==SGST, CGST & IGST Calculation==

                                netSales = unitPrice * quantity;
                                total = (rate * quantity) + Math.Round((Converter.ToDecimal(dr["Shipping & Handling"])), 2);

                                sqlParam[0] = new SqlParameter("@datetimeInvoiceDate", invoiceDate);
                                sqlParam[1] = new SqlParameter("@intInvoiceNo", invoiceNo);
                                sqlParam[2] = new SqlParameter("@nvarcharPartysName", PartyName);
                                sqlParam[3] = new SqlParameter("@nvarcharOrderID", orderID);
                                sqlParam[4] = new SqlParameter("@datetimeOrderDate", orderDate);

                                sqlParam[5] = new SqlParameter("@nvarcharAddress", Address);
                                sqlParam[6] = new SqlParameter("@nvarcharEmailID", EmailID);
                                sqlParam[7] = new SqlParameter("@nvarcharMobileNo", MobNo);
                                sqlParam[8] = new SqlParameter("@nvarcharProductID", productID);

                                sqlParam[9] = new SqlParameter("@decimalRate", rate);
                                sqlParam[10] = new SqlParameter("@intQuantity", quantity);
                                sqlParam[11] = new SqlParameter("@decimalTotal", total);
                                sqlParam[12] = new SqlParameter("@decimalNetSale", netSales);
                                sqlParam[13] = new SqlParameter("@decimalCGST", CGST);
                                sqlParam[14] = new SqlParameter("@decimalSGST", SGST);
                                sqlParam[15] = new SqlParameter("@decimalIGST", IGST);
                                sqlParam[16] = new SqlParameter("@decimalUnitPrice", unitPrice);

                                sqlParam[17] = new SqlParameter("@decimalCGSTPerc", CGST_Perc);
                                sqlParam[18] = new SqlParameter("@decimalSGSTPerc", SGST_Perc);
                                sqlParam[19] = new SqlParameter("@decimalIGSTPerc", IGST_Perc);

                                sqlParam[20] = new SqlParameter("@decimalShipping_Total", shipping_Total);
                                sqlParam[21] = new SqlParameter("@decimalShipping_CGST", shipping_CGST);
                                sqlParam[22] = new SqlParameter("@decimalShipping_SGST", shipping_SGST);
                                sqlParam[23] = new SqlParameter("@decimalShipping_IGST", shipping_IGST);
                                sqlParam[24] = new SqlParameter("@decimalShipping_UnitPrice", shipping_UnitPrice);
                                sqlParam[25] = new SqlParameter("@decimalShipping_NetSale", shipping_NetSales);

                                sqlParam[26] = new SqlParameter("@nvarcharVendor", Vendor);
                                sqlParam[27] = new SqlParameter("@intStatus", 1);
                                sqlParam[28] = new SqlParameter("@intCreatedModifiedBy", UserID);

                                Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_InsertUpdateInvoice", sqlParam);

                                if (Result >= Const.Success) ErrorMsg = string.Empty;
                                if (Result == Const.ProductNotExists) ErrorMsg = "Product not found";
                                if (Result == Const.Duplicate) ErrorMsg = "Duplicate Record";
                                if (Result == Const.Error) ErrorMsg = "Error";
                                if (Result == Const.VendorIDNotFound) ErrorMsg = "Vendor ID not found";
                                if (Result == Const.NetworkError) ErrorMsg = "Network Error";
                                Address = Address.Replace(',', ' ');
                                if (invoiceDate.Date == DateTime.Now.Date && orderDate.Date == DateTime.Now.Date) sw.WriteLine(Result + ",," + PartyName + ",," + orderID + "," + productID + "," + rate + "," + quantity + "," + shipping_Total + "," + Vendor + "," + TaxGroup + "," + Address + "," + EmailID + "," + MobNo + "," + ErrorMsg);
                                if (invoiceDate.Date == DateTime.Now.Date && orderDate.Date != DateTime.Now.Date) sw.WriteLine(Result + ",," + PartyName + "," + orderDate.ToString("dd-MM-yyyy") + "," + orderID + "," + productID + "," + rate + "," + quantity + "," + shipping_Total + "," + Vendor + "," + TaxGroup + "," + Address + "," + EmailID + "," + MobNo + "," + ErrorMsg);
                                if (invoiceDate.Date != DateTime.Now.Date && orderDate.Date == DateTime.Now.Date) sw.WriteLine(Result + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + PartyName + ",," + orderID + "," + productID + "," + rate + "," + quantity + "," + shipping_Total + "," + Vendor + "," + TaxGroup + "," + Address + "," + EmailID + "," + MobNo + "," + ErrorMsg);
                                if (invoiceDate.Date != DateTime.Now.Date && orderDate.Date != DateTime.Now.Date) sw.WriteLine(Result + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + PartyName + "," + orderDate.ToString("dd-MM-yyyy") + "," + orderID + "," + productID + "," + rate + "," + quantity + "," + shipping_Total + "," + Vendor + "," + TaxGroup + "," + Address + "," + EmailID + "," + MobNo + "," + ErrorMsg);
                                continue;
                            }
                            if (SheetType == Const.Settlement)
                            {
                                DateTime SettlementDate;
                                decimal SettlementAmt;
                                string Status = string.Empty;

                                invoiceNo = Converter.ToInt(dr["Invoice No"]);
                                ////if (invoiceNo == 0) continue;
                                orderID = Converter.ToString(dr["Order ID"]);
                                invoiceDate = Converter.ToDateTime(dr["Invoice Date"]);
                                productID = Converter.ToString(dr["SKU"]).ToUpper();

                                SettlementDate = Converter.ToDateTime(dr["Settlement Date"]);
                                SettlementAmt = Converter.ToDecimal(dr["Settlement Amount"]);
                                Status = Converter.ToStatusString(dr["Status"]);

                                int TempStatusID = GetStatusID(Status);
                                if (TempStatusID > 0)
                                {
                                    int SrNo = GetSrNoForSettlement(orderID, productID, SettlementDate, SettlementAmt, TempStatusID);

                                    if (SrNo > 0)
                                    {
                                        sqlParam[0] = new SqlParameter("@datetimeSettlementDate", SettlementDate);
                                        sqlParam[1] = new SqlParameter("@nvarcharOrderID", orderID);
                                        sqlParam[2] = new SqlParameter("@intInvoiceNo", invoiceNo);
                                        sqlParam[3] = new SqlParameter("@datetimeInvoiceDate", invoiceDate);
                                        sqlParam[4] = new SqlParameter("@nvarcharProductID", productID);
                                        sqlParam[5] = new SqlParameter("@decimalSettlementAmount", SettlementAmt);
                                        sqlParam[6] = new SqlParameter("@nvarcharStatus", Status);
                                        sqlParam[7] = new SqlParameter("@intCreatedModifiedBy", UserID);
                                        sqlParam[8] = new SqlParameter("@datetimeCrNoteDate", DateTime.Now.Date);
                                        sqlParam[9] = new SqlParameter("@intSrNo", SrNo);

                                        if (dr["Overwrite"].ToString().ToUpper() == "YES")
                                        {
                                            Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateAmountReceivable_New_Multi_Settlement_Overwrite", sqlParam);
                                        }
                                        else
                                        {
                                            Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateAmountReceivable_New_Multi_Settlement", sqlParam);
                                        }
                                    }
                                    else
                                    {
                                        Result = SrNo;
                                    }
                                }
                                else
                                {
                                    Result = TempStatusID;
                                }

                                if (Result == Const.Success) continue;
                                if (Result == Const.InvoiceReceived) ErrorMsg = "Already Received";
                                if (Result == Const.InvoiceReturned) ErrorMsg = "Already Returned";
                                if (Result == Const.InvoiceCancelled) ErrorMsg = "Already Cancelled";
                                if (Result == Const.InvoiceStatusNotFound) ErrorMsg = "Invoice Status Not Found";
                                if (Result == Const.InvalidPendingInvoiceStatus) ErrorMsg = "Invalid Status Pending";
                                if (Result == Const.InvoiceDetailsNotFound) ErrorMsg = "Invoice Details Not Found";
                                if (Result == Const.CreditNoteDetailsNotFound) ErrorMsg = "Credit Note Details Not Found";
                                if (Result == Const.SalesDetailsNotFound) ErrorMsg = "Sales Details Not Found";
                                if (Result == Const.Error) ErrorMsg = "Error";
                                if (Result == Const.NetworkError) ErrorMsg = "Network Error";
                                if (Result == Const.InvoiceProductNotFound) ErrorMsg = "Invoice Product Not Found";
                                if (Result == Const.Duplicate) ErrorMsg = "Duplicate Invoice";
                                if (Result == Const.DuplicateSettlement) ErrorMsg = "Duplicate Settlement. Please check";
                                sw.WriteLine(invoiceNo + "," + orderID + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + productID + "," + SettlementDate.ToString("dd-MM-yyyy") + "," + SettlementAmt + "," + Status + "," + ErrorMsg);
                                continue;
                            }
                            if (SheetType == Const.Catalogue)
                            {
                                int ModeType = 0;
                                string ModeTypeStr = Converter.ToString(dr["ModeType"]).ToUpper();
                                if (ModeTypeStr == "NEW") ModeType = 1;
                                if (ModeTypeStr == "UPDATE") ModeType = 2;
                                if (ModeTypeStr == "DELETE") ModeType = 3;
                                if (ModeTypeStr == "PARTIALUPDATE") ModeType = 4;

                                productID = Converter.ToString(dr["SKU"]).ToUpper();
                                if (string.IsNullOrEmpty(productID)) continue;

                                string Title, Size, Group, Category, SubCategory;
                                decimal MRP;

                                Title = Converter.ToString(dr["Title"]);
                                Size = Converter.ToString(dr["Size"]);
                                Group = Converter.ToString(dr["Group"]);
                                MRP = Converter.ToDecimal(dr["MRP"]);
                                Category = Converter.ToString(dr["Category"]);
                                SubCategory = Converter.ToString(dr["SubCategory"]);

                                if (string.IsNullOrEmpty(Title)) Title = "EMPTY";
                                if (string.IsNullOrEmpty(Size)) Size = "EMPTY";
                                if (string.IsNullOrEmpty(Group)) Group = "EMPTY";
                                if (string.IsNullOrEmpty(Category)) Category = "EMPTY";
                                if (string.IsNullOrEmpty(SubCategory)) SubCategory = "EMPTY";

                                if (ModeType > 0)
                                {
                                    sqlParam[0] = new SqlParameter("@nvarcharProductID", productID);
                                    sqlParam[1] = new SqlParameter("@nvarcharTitle", Title);
                                    sqlParam[2] = new SqlParameter("@nvarcharSize", Size);
                                    sqlParam[3] = new SqlParameter("@decimalMRP", MRP);
                                    sqlParam[4] = new SqlParameter("@nvarcharGroup", Group);
                                    sqlParam[5] = new SqlParameter("@nvarcharCategory", Category);
                                    sqlParam[6] = new SqlParameter("@nvarcharSubCategory", SubCategory);
                                    sqlParam[7] = new SqlParameter("@intCreatedModifiedBy", UserID);
                                    sqlParam[8] = new SqlParameter("@intModeType", ModeType);
                                    Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_pm_UploadProduct", sqlParam);

                                    if (Result == Const.Success) continue;
                                    if (Result == Const.Duplicate) ErrorMsg = "Already Exists";
                                    if (Result == Const.NameNotAvailable) ErrorMsg = "Product Name Already Exists";
                                    if (Result == Const.GroupNotFound) ErrorMsg = "Group Not Found";
                                    if (Result == Const.CategoryNotFound) ErrorMsg = "Category Not Found";
                                    if (Result == Const.SubCategoryNotFound) ErrorMsg = "Sub-Category Not Found";
                                    if (Result == Const.Error) ErrorMsg = "Error";
                                    if (Result == Const.NetworkError) ErrorMsg = "Network Error";
                                }
                                else
                                {
                                    ErrorMsg = "Check ModeType content";
                                }
                                sw.WriteLine(productID + "," + Title + "," + Size + "," + MRP + "," + Group + "," + Category + "," + SubCategory + "," + ErrorMsg);
                                continue;
                            }
                            if (SheetType == Const.Purchase)
                            {
                                int Quantity = 0;
                                decimal MRP, PP, SP;

                                productID = Converter.ToString(dr["SKU"]).ToUpper();
                                Quantity = Converter.ToInt(dr["Quantity"]);
                                MRP = Converter.ToDecimal(dr["MRP"]);
                                PP = Converter.ToDecimal(dr["PP"]);
                                SP = Converter.ToDecimal(dr["SP"]);

                                sqlParam[0] = new SqlParameter("@nvarcharProductID", productID);
                                sqlParam[1] = new SqlParameter("@datetimePurchaseDate", DateTime.Now.Date);
                                sqlParam[2] = new SqlParameter("@intQuantity", Quantity);
                                sqlParam[3] = new SqlParameter("@decimalMRP", MRP);
                                sqlParam[4] = new SqlParameter("@decimalPP", PP);
                                sqlParam[5] = new SqlParameter("@decimalSP", SP);
                                sqlParam[6] = new SqlParameter("@intCreatedModifiedBy", UserID);
                                sqlParam[7] = new SqlParameter("@intMode", 2);
                                sqlParam[8] = new SqlParameter("@intType", 1);

                                Result = SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_pum_InsertUpdateDeletePurchase", sqlParam);
                                if (Result == Const.Success) continue;
                                if (Result == Const.ProductNotExists) ErrorMsg = "Product not found";
                                if (Result == Const.Error) ErrorMsg = "Error";
                                if (Result == Const.NetworkError) ErrorMsg = "Network Error";
                                sw.WriteLine(productID + "," + Quantity + "," + MRP + "," + PP + "," + SP + "," + ErrorMsg);
                                continue;
                            }
                            dr.Close();
                            oleDbCommand.Dispose();
                            OleDbCon.Close();
                            if (sw != null)
                            {
                                sw.Flush();
                                sw.Close();
                            }
                            return new Tuple<string, string, bool>(ErrorFolderPath, ErrorCsvFileName, false);
                        }
                        catch (Exception ex)
                        {
                            sw.WriteLine("DO NOT UPLOAD THIS DOCUMENT");
                            sw.WriteLine(invoiceNo + "," + orderID + "," + invoiceDate.ToString("dd-MM-yyyy") + "," + productID + "," + ex.Message);
                        }
                    }
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    }
                }
                return new Tuple<string, string, bool>(ErrorFolderPath, ErrorCsvFileName, true);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public byte[] GetTemplate(int SheetType)
        {
            try
            {
                string imgName = string.Empty;
                if (SheetType == 1) imgName = "Invoicing.png";
                else if (SheetType == 2) imgName = "Settlement.png";
                else if (SheetType == 3) imgName = "Catalogue.png";
                return IMSLibrary.Class.Helper.ConvertToByte(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "Images\\" + imgName);
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==UploadSheet==

        #region ==User==

        public bool CheckUserNameAvailability(string UserName)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@varcharUserName", Converter.ToString(UserName))
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_um_CheckUserNameAvailability", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public UserList IsValidUser(string Username, string Password)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@varcharUserName", Converter.ToString(Username)),
                    [1] = new SqlParameter("@varcharUserPassword", EncryptDecrypt.Encrypt(Converter.ToString(Password), Converter.ToString(Username)))
                };

                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_um_IsValidUser", sqlParam);
                DataRow dr = dt.Rows[0];
                if (dr != null)
                {
                    return new UserList
                    {
                        UserID = Convert.ToInt32(dr["UserID"]),
                        UserName = dr["UserName"].ToString(),
                        UserType = Convert.ToInt32(dr["UserType"])
                    };
                }
                throw new FaultException("Please Enter a valid Username or Password");
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public ObservableCollection<UserList> GetUserList()
        {
            try
            {
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_um_GetUserList", new Dictionary<int, SqlParameter>());
                ObservableCollection<UserList> lstUser = new ObservableCollection<UserList>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstUser.Add(new UserList
                    {
                        UserID = (int)dr["UserID"],
                        UserName = dr["UserName"].ToString(),
                        UserType = Convert.ToInt32(dr["UserType"])
                    });
                }
                return lstUser;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public UserDetails GetUserDetails(int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>();
                sqlParam[0] = new SqlParameter("@intUserID", UserID);
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_um_GetUserDetails", sqlParam);
                UserDetails objUserDet = new UserDetails();
                DataRow dr = dt.Rows[0];
                if (dr != null)
                {
                    objUserDet.FirstName = Converter.ToString(dr["FirstName"]);
                    objUserDet.MiddleName = Converter.ToString(dr["MiddleName"]);
                    objUserDet.LastName = Converter.ToString(dr["LastName"]);
                    objUserDet.DateOfBirth = Converter.ToDateTime(dr["DateOfBirth"]);
                    objUserDet.UserType = Converter.ToInt(dr["UserType"]);
                    objUserDet.ContactNo = Converter.ToString(dr["ContactNo"]);
                    objUserDet.Gender = Converter.ToInt(dr["Gender"]);
                    objUserDet.Email = Converter.ToString(dr["Email"]);
                    objUserDet.UserID = UserID;
                    objUserDet.UserName = Converter.ToString(dr["UserName"]);
                    objUserDet.UserPassword = EncryptDecrypt.Decrypt(Converter.ToString(dr["UserPassword"]), objUserDet.UserName);
                }
                return objUserDet;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteUser(int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intUsertID", UserID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_DeleteUser", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateUserDetails(UserDetails objUser, int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>();
                sqlParam[0] = new SqlParameter("@varcharUserName", objUser.UserName);
                sqlParam[1] = new SqlParameter("@varacharUserPassword", EncryptDecrypt.Encrypt(objUser.UserPassword, objUser.UserName));
                sqlParam[2] = new SqlParameter("@charUserType", objUser.UserType);
                sqlParam[3] = new SqlParameter("@varcharFirstName", objUser.FirstName);
                sqlParam[4] = new SqlParameter("@varcharMiddleName", objUser.MiddleName);
                sqlParam[5] = new SqlParameter("@varcharLastName", objUser.LastName);
                sqlParam[6] = new SqlParameter("@datetimeDateOfBirth", objUser.DateOfBirth);
                sqlParam[7] = new SqlParameter("@varcharContactNo", objUser.ContactNo);
                sqlParam[8] = new SqlParameter("@varcharEmail", objUser.Email);
                sqlParam[9] = new SqlParameter("@intGender", objUser.Gender);
                sqlParam[10] = new SqlParameter("@intUserID", objUser.UserID);
                sqlParam[11] = new SqlParameter("@intCreatedModifiedBy", UserID);

                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_um_InsertUpdateUserDetails", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==User==

        #region ==Product==

        public List<ProductList> GetProductList()
        {
            try
            {
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_pm_GetProductList", new Dictionary<int, SqlParameter>());
                List<ProductList> lstProduct = new List<ProductList>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstProduct.Add(new ProductList
                    {
                        ID = (string)dr["ProductID"],
                        Name = (string)dr["Name"]
                    });
                }
                return lstProduct;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public Product GetProductDetails(string ProductID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", ProductID)
                };
                DataSet ds = SqlUtility.ExecuteQueryWithDS(GetConnectionString(), "sp_pm_GetProductDetails", sqlParam);
                Product objProduct = new Product();
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr != null)
                {
                    objProduct.ProductID = Converter.ToString(dr["ProductID"]);
                    objProduct.Name = Converter.ToString(dr["Name"]);
                    objProduct.TotalQuota = Converter.ToInt(dr["TotalAvailableQuota"]);
                    objProduct.TotalSale = Converter.ToInt(dr["TotalSale"]);
                    objProduct.TotalDamage = Converter.ToInt(dr["TotalDamage"]);

                    DataRow dr1 = ds.Tables[1].Rows[0];
                    if (dr1 != null)
                    {
                        objProduct.GroupID = Converter.ToInt(dr1["GroupID"]);
                        objProduct.Size = Converter.ToString(dr1["Size"]);
                        objProduct.Price = Converter.ToDecimal(dr1["MRPPrice"]);
                        objProduct.PosterType = Converter.ToString(dr1["PosterType"]);
                        objProduct.PosterDimensions = Converter.ToString(dr1["PosterDimensions"]);
                        objProduct.DimensionsInInches = Converter.ToString(dr1["DimensionsInInches"]);
                        objProduct.PosterWeight = Converter.ToString(dr1["PosterWeight_grams"]);
                        objProduct.PackageContents = Converter.ToString(dr1["PackageContents"]);
                        objProduct.PackageInformation = Converter.ToString(dr1["PackagingInformation"]);
                        objProduct.ShippingDuration = Converter.ToString(dr1["ShippingDuration"]);
                        objProduct.Variant = Converter.ToString(dr1["Variant"]);
                        objProduct.Color = Converter.ToString(dr1["Color"]);
                        objProduct.Keywords = Converter.ToString(dr1["Keywords"]);
                        objProduct.SocialName = Converter.ToString(dr1["SocialName"]);
                        objProduct.Publisher = Converter.ToString(dr1["Publisher"]);
                        objProduct.PaperType = Converter.ToString(dr1["PaperType"]);
                        objProduct.Description = Converter.ToString(dr1["Description"]);
                        objProduct.Category = Converter.ToInt(dr1["Category"]);
                        objProduct.SubCategory = Converter.ToInt(dr1["SubCategory"]);
                        objProduct.ArtistName = Converter.ToString(dr1["ArtistName"]);
                        objProduct.PaintingStyle = Converter.ToString(dr1["PaintingStyle"]);
                        objProduct.BlackWhitePoster = Converter.ToString(dr1["BlackAndWhitePoster"]);
                        objProduct.ColorPoster = Converter.ToString(dr1["ColorPoster"]);
                        objProduct.PaperFinish = Converter.ToString(dr1["PaperFinish"]);
                        objProduct.Shape = Converter.ToString(dr1["Shape"]);
                        objProduct.Orientation = Converter.ToString(dr1["Orientation"]);
                        objProduct.Framed = Converter.ToString(dr1["Framed"]);
                        objProduct.FrameMaterial = Converter.ToString(dr1["FrameMaterial"]);
                        objProduct.OtherFrameDetails = Converter.ToString(dr1["OtherFrameDetails"]);
                        objProduct.Width = Converter.ToDecimal(dr1["Width"]);
                        objProduct.Height = Converter.ToDecimal(dr1["Height"]);
                        objProduct.PaperDepth = Converter.ToString(dr1["PaperDepth_gsm"]);
                        objProduct.Weight = Converter.ToString(dr1["Weight_grams"]);
                        objProduct.OtherDimensions = Converter.ToString(dr1["OtherDimensions"]);
                        objProduct.OtherFeatures = Converter.ToString(dr1["OtherFeatures"]);
                        objProduct.SupplierImage = Converter.ToString(dr1["SupplierImage"]);
                        objProduct.ImageLink = Converter.ToString(dr1["ImageLink"]);
                        objProduct.Note = Converter.ToString(dr1["Note"]);
                        objProduct.WarrantySummary = Converter.ToString(dr1["WarrantySummary"]);
                    }
                }
                return objProduct;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateProductMaster(Product objProduct, int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", Converter.ToString(objProduct.ProductID)),
                    [1] = new SqlParameter("@nvarcharProductName", Converter.ToString(objProduct.Name)),
                    [2] = new SqlParameter("@intGroupID", objProduct.GroupID),
                    [3] = new SqlParameter("@nvarcharSize", Converter.ToString(objProduct.Size)),
                    [4] = new SqlParameter("@decimalMRP", objProduct.Price),
                    [5] = new SqlParameter("@nvarcharPosterType", Converter.ToString(objProduct.PosterType)),
                    [6] = new SqlParameter("@nvarcharPosterDimensions", Converter.ToString(objProduct.PosterDimensions)),
                    [7] = new SqlParameter("@nvarcharDimensionsInInches", Converter.ToString(objProduct.DimensionsInInches)),
                    [8] = new SqlParameter("@nvarcharPosterWeight", Converter.ToString(objProduct.PosterWeight)),
                    [9] = new SqlParameter("@nvarcharPackageContents", Converter.ToString(objProduct.PackageContents)),
                    [10] = new SqlParameter("@nvarcharPackagingInformation", Converter.ToString(objProduct.PackageInformation)),
                    [11] = new SqlParameter("@nvarcharShippingDuration", Converter.ToString(objProduct.ShippingDuration)),
                    [12] = new SqlParameter("@nvarcharVariant", Converter.ToString(objProduct.Variant)),
                    [13] = new SqlParameter("@nvarcharColor", Converter.ToString(objProduct.Color)),
                    [14] = new SqlParameter("@nvarcharKeywords", Converter.ToString(objProduct.Keywords)),
                    [15] = new SqlParameter("@nvarcharSocialName", Converter.ToString(objProduct.SocialName)),
                    [16] = new SqlParameter("@nvarcharPublisher", Converter.ToString(objProduct.Publisher)),
                    [17] = new SqlParameter("@nvarcharPaperType", Converter.ToString(objProduct.PaperType)),
                    [18] = new SqlParameter("@nvarcharDescription", Converter.ToString(objProduct.Description)),
                    [19] = new SqlParameter("@intCategory", objProduct.Category),
                    [20] = new SqlParameter("@intSubCategory", objProduct.SubCategory),
                    [21] = new SqlParameter("@nvarcharArtistName", Converter.ToString(objProduct.ArtistName)),
                    [22] = new SqlParameter("@nvarcharPaintingStyle", Converter.ToString(objProduct.PaintingStyle)),
                    [23] = new SqlParameter("@nvarcharBlackAndWhitePoster", Converter.ToString(objProduct.BlackWhitePoster)),
                    [24] = new SqlParameter("@nvarcharColorPoster", Converter.ToString(objProduct.ColorPoster)),
                    [25] = new SqlParameter("@nvarcharPaperFinish", Converter.ToString(objProduct.PaperFinish)),
                    [26] = new SqlParameter("@nvarcharShape", Converter.ToString(objProduct.Shape)),
                    [27] = new SqlParameter("@nvarcharOrientation", Converter.ToString(objProduct.Orientation)),
                    [28] = new SqlParameter("@nvarcharFramed", Converter.ToString(objProduct.Framed)),
                    [29] = new SqlParameter("@nvarcharFrameMaterial", Converter.ToString(objProduct.FrameMaterial)),
                    [30] = new SqlParameter("@nvarcharOtherFrameDetails", Converter.ToString(objProduct.OtherFrameDetails)),
                    [31] = new SqlParameter("@intWidth", objProduct.Width),
                    [32] = new SqlParameter("@intHeight", objProduct.Height),
                    [33] = new SqlParameter("@nvarcharPaperDepth", Converter.ToString(objProduct.PaperDepth)),
                    [34] = new SqlParameter("@nvarcharWeight", Converter.ToString(objProduct.Weight)),
                    [35] = new SqlParameter("@nvarcharOtherDimensions", Converter.ToString(objProduct.OtherDimensions)),
                    [36] = new SqlParameter("@nvarcharOtherFeatures", Converter.ToString(objProduct.OtherFeatures)),
                    [37] = new SqlParameter("@nvarcharSupplierImage", Converter.ToString(objProduct.SupplierImage)),
                    [38] = new SqlParameter("@nvarcharImageLink", Converter.ToString(objProduct.ImageLink)),
                    [39] = new SqlParameter("@nvarcharNote", Converter.ToString(objProduct.Note)),
                    [40] = new SqlParameter("@nvarcharWarrantySummary", Converter.ToString(objProduct.WarrantySummary)),
                    [41] = new SqlParameter("@intCreatedModifiedBy", UserID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_pm_InsertUpdateProduct", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteProduct(string ProductID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", ProductID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_pm_DeleteProduct", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public ObservableCollection<Product> CheckStock(int Quantity, string ProductID, int SearchMode)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intQuantity", Quantity),
                    [1] = new SqlParameter("@nvarcharProductID", Converter.ToString(ProductID)),
                    [2] = new SqlParameter("@intSearchMode", SearchMode)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_pm_CheckStock", sqlParam);
                ObservableCollection<Product> lstProduct = new ObservableCollection<Product>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstProduct.Add(new Product
                    {
                        ProductID = Converter.ToString(dr["ProductID"]),
                        Name = Converter.ToString(dr["Name"]),
                        TotalQuota = Converter.ToInt(dr["TotalAvailableQuota"]),
                        TotalSale = Converter.ToInt(dr["TotalSale"]),
                        TotalDamage = Converter.ToInt(dr["TotalDamage"])
                    });
                }
                return lstProduct;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<Purchase> GetPurchaseDetails(string ProductID, DateTime PurchaseDate)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", Converter.ToString(ProductID)),
                    [1] = new SqlParameter("@datetimePurchaseDate", PurchaseDate.Date)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_pum_GetDetails", sqlParam);
                List<Purchase> lstPurchase = new List<Purchase>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstPurchase.Add(new Purchase
                    {
                        ProductID = Converter.ToString(dr["ProductID"]),
                        PurchaseDate = Converter.ToDateTime(dr["PurchaseDate"]),
                        MRP = Converter.ToDecimal(dr["Price"]),
                        PP = Converter.ToDecimal(dr["PP"]),
                        SP = Converter.ToDecimal(dr["SP"]),
                        Quantity = Converter.ToInt(dr["Quantity"])
                    });
                }
                return lstPurchase;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateDeletePurchase(int Mode, Purchase objPurchase, int UserID)
        {
            try
            {
                if (Mode == 2) objPurchase.PurchaseDate = DateTime.Now.Date;
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", Converter.ToString(objPurchase.ProductID)),
                    [1] = new SqlParameter("@datetimePurchaseDate", objPurchase.PurchaseDate.Date),
                    [2] = new SqlParameter("@decimalMRP", objPurchase.MRP),
                    [3] = new SqlParameter("@intQuantity", objPurchase.Quantity),
                    [4] = new SqlParameter("@intMode", Mode),
                    [5] = new SqlParameter("@intCreatedModifiedBy", UserID),
                    [6] = new SqlParameter("@intType", 1),//Type - Inventory / Product [15-09-2014]
                    [7] = new SqlParameter("@decimalPP", objPurchase.PP),
                    [8] = new SqlParameter("@decimalSP", objPurchase.SP)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_pum_InsertUpdateDeletePurchase", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<Sales> GetSalesDetails(string ProductID, DateTime SalesDate)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", Converter.ToString(ProductID)),
                    [1] = new SqlParameter("@datetimeSalesDate", SalesDate.Date)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_sm_GetDetails", sqlParam);
                List<Sales> lstSales = new List<Sales>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstSales.Add(new Sales
                    {
                        ProductID = Converter.ToString(dr["ProductID"]),
                        SalesDate = Converter.ToDateTime(dr["SalesDate"]),
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        Price = Converter.ToDecimal(dr["Price"]),
                        Quantity = Converter.ToInt(dr["Quantity"]),
                        VendorID = Converter.ToInt(dr["VendorID"]),
                        OrderID = Converter.ToString(dr["OrderID"]),
                        PartyName = Converter.ToString(dr["PartyName"])
                    });
                }
                return lstSales;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateDeleteSales(int Mode, Sales objSales, int UserID)
        {
            try
            {
                if (Mode == 2) objSales.SalesDate = DateTime.Now.Date;
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", objSales.ProductID),
                    [1] = new SqlParameter("@datetimeSalesDate", objSales.SalesDate.Date),
                    [2] = new SqlParameter("@decimalPrice", objSales.Price),
                    [3] = new SqlParameter("@intQuantity", objSales.Quantity),
                    [4] = new SqlParameter("@intMode", Mode),
                    [5] = new SqlParameter("@intCreatedModifiedBy", UserID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_sm_InsertUpdateDeleteSales", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<Damage> GetDamagedGoodsDetails(string ProductID, DateTime DamageDate)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", Converter.ToString(ProductID)),
                    [1] = new SqlParameter("@datetimeDamageDate", DamageDate.Date)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_dgm_GetDetails", sqlParam);
                List<Damage> lstDamage = new List<Damage>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstDamage.Add(new Damage
                    {
                        ProductID = Converter.ToString(dr["ProductID"]),
                        DamageDate = Converter.ToDateTime(dr["Date"]),
                        Price = Converter.ToDecimal(dr["Price"]),
                        Quantity = Converter.ToInt(dr["Quantity"]),
                        Comment = Converter.ToString(dr["Comment"])
                    });
                }
                return lstDamage;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateDeleteDamage(int Mode, Damage objDamage, int UserID)
        {
            try
            {
                if (Mode == 2) objDamage.DamageDate = DateTime.Now.Date;
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", objDamage.ProductID),
                    [1] = new SqlParameter("@datetimeDamageDate", objDamage.DamageDate.Date),
                    [2] = new SqlParameter("@decimalPrice", objDamage.Price),
                    [3] = new SqlParameter("@intQuantity", objDamage.Quantity),
                    [4] = new SqlParameter("@nvarcharComment", objDamage.Comment),
                    [5] = new SqlParameter("@intMode", Mode),
                    [6] = new SqlParameter("@intCreatedModifiedBy", UserID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_dgm_InsertUpdateDeleteDamage", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Product==

        #region ==Invoice==

        public bool InsertUpdateInvoice(InvoiceMaster objInvoice, int UserID, int Mode)
        {
            SqlConnection sqlConnection = null;
            SqlTransaction sqlTrans = null;
            try
            {
                sqlConnection = sqlCon();
                sqlTrans = sqlConnection.BeginTransaction();
                Dictionary<int, SqlParameter> sqlParam = null;
                if (Mode == 3)
                {
                    sqlParam = new Dictionary<int, SqlParameter>
                    {
                        [0] = new SqlParameter("@intInvoiceNo", objInvoice.InvoiceNo)
                    };
                    if (SqlUtility.ExecuteCommandSpReturnVal(sqlConnection, sqlTrans, "sp_im_DeleteInvoice", sqlParam) < 0)
                    {
                        sqlTrans.Rollback();
                        throw new FaultException("Invoice Master Not Modified");
                    }
                }

                foreach (InvoiceProduct item in objInvoice.lstInvoiceProduct)
                {
                    sqlParam = new Dictionary<int, SqlParameter>
                    {
                        [0] = new SqlParameter("@datetimeInvoiceDate", item.InvoiceDate),
                        [1] = new SqlParameter("@intInvoiceNo", objInvoice.InvoiceNo),
                        [2] = new SqlParameter("@nvarcharPartysName", Converter.ToString(objInvoice.PartyName)),
                        [3] = new SqlParameter("@nvarcharOrderID", objInvoice.OrderID),
                        [4] = new SqlParameter("@datetimeOrderDate", item.OrderDate),
                        [5] = new SqlParameter("@nvarcharAddress", Converter.ToString(objInvoice.Address)),
                        [6] = new SqlParameter("@nvarcharEmailID", Converter.ToString(objInvoice.EmailID)),
                        [7] = new SqlParameter("@nvarcharMobileNo", Converter.ToString(objInvoice.ContactNo)),
                        [8] = new SqlParameter("@nvarcharProductID", item.ProductID),

                        [9] = new SqlParameter("@decimalRate", item.Rate),
                        [10] = new SqlParameter("@intQuantity", item.Quantity),
                        [11] = new SqlParameter("@decimalTotal", item.Total),
                        [12] = new SqlParameter("@decimalNetSale", item.NetSale),
                        [13] = new SqlParameter("@decimalCGST", item.CGST),
                        [14] = new SqlParameter("@decimalSGST", item.SGST),
                        [15] = new SqlParameter("@decimalIGST", item.IGST),
                        [16] = new SqlParameter("@decimalUnitPrice", item.UnitPrice),

                        [16] = new SqlParameter("@decimalCGSTPerc", item.CGSTPerc),
                        [17] = new SqlParameter("@decimalSGSTPerc", item.SGSTPerc),
                        [18] = new SqlParameter("@decimalIGSTPerc", item.IGSTPerc),

                        [19] = new SqlParameter("@decimalShipping_Total", item.Shipping_Total),
                        [20] = new SqlParameter("@decimalShipping_CGST", item.Shipping_CGST),
                        [21] = new SqlParameter("@decimalShipping_SGST", item.Shipping_SGST),
                        [22] = new SqlParameter("@decimalShipping_IGST", item.Shipping_IGST),
                        [23] = new SqlParameter("@decimalShipping_UnitPrice", item.Shipping_UnitPrice),
                        [24] = new SqlParameter("@decimalShipping_NetSale", item.Shipping_NetSale),

                        [25] = new SqlParameter("@nvarcharVendor", objInvoice.Vendor),
                        [26] = new SqlParameter("@intStatus", item.Status),
                        [27] = new SqlParameter("@intCreatedModifiedBy", UserID)
                    };
                    if (SqlUtility.ExecuteCommandSpReturnVal(sqlConnection, sqlTrans, "sp_im_InsertUpdateInvoice_Latest_July", sqlParam) > 0) continue;
                    throw new FaultException("Invoice Master Not Modified");
                }
                sqlTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (sqlTrans != null)
                {
                    sqlTrans.Rollback();
                }
                throw new FaultException(ex.Message);
            }
            finally
            {
                if (sqlTrans != null)
                {
                    sqlTrans.Dispose();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Dispose();
                }
            }
        }

        public InvoiceMaster GetInvoiceDetails(int InvoiceNo, string OrderID, int SearchMode)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intInvoiceNo", InvoiceNo),
                    [1] = new SqlParameter("@nvarcharOrderID", OrderID),
                    [2] = new SqlParameter("@intSearchMode", SearchMode)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_im_GetInvoiceDetails", sqlParam);
                InvoiceMaster objInvoice = new InvoiceMaster
                {
                    lstInvoiceProduct = new ObservableCollection<InvoiceProduct>()
                };
                bool firstTime = true;
                foreach (DataRow dr in dt.Rows)
                {
                    if (firstTime)
                    {
                        objInvoice.InvoiceNo = Converter.ToInt(dr["InvoiceNo"]);
                        objInvoice.InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]);
                        objInvoice.OrderID = Converter.ToString(dr["OrderID"]);
                        objInvoice.OrderDate = Converter.ToDateTime(dr["OrderDate"]);
                        objInvoice.Address = Converter.ToString(dr["Address"]);
                        objInvoice.ContactNo = Converter.ToString(dr["MobileNo"]);
                        objInvoice.EmailID = Converter.ToString(dr["EmailID"]);
                        objInvoice.PartyName = Converter.ToString(dr["PartysName"]);
                        objInvoice.Vendor = Converter.ToInt(dr["VendorID"]);
                        firstTime = false;
                    }
                    objInvoice.lstInvoiceProduct.Add(new InvoiceProduct
                    {
                        InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]),
                        OrderDate = Converter.ToDateTime(dr["OrderDate"]),
                        ProductID = Converter.ToString(dr["ProductID"]),

                        Quantity = Converter.ToInt(dr["Quantity"]),
                        Rate = Converter.ToDecimal(dr["Rate"]),
                        UnitPrice = Converter.ToDecimal(dr["UnitPrice"]),
                        NetSale = Converter.ToDecimal(dr["NetSale"]),
                        Total = Converter.ToDecimal(dr["Total"]),

                        CGST = Converter.ToDecimal(dr["CGST"]),
                        SGST = Converter.ToDecimal(dr["SGST"]),
                        IGST = Converter.ToDecimal(dr["IGST"]),

                        Shipping_UnitPrice = Converter.ToDecimal(dr["Shipping_UnitPrice"]),
                        Shipping_NetSale = Converter.ToDecimal(dr["Shipping_NetSale"]),
                        Shipping_CGST = Converter.ToDecimal(dr["Shipping_CGST"]),
                        Shipping_SGST = Converter.ToDecimal(dr["Shipping_SGST"]),
                        Shipping_IGST = Converter.ToDecimal(dr["Shipping_IGST"]),
                        Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"]),

                        CGSTPerc = Converter.ToDecimal(dr["CGSTPerc"]),
                        SGSTPerc = Converter.ToDecimal(dr["SGSTPerc"]),
                        IGSTPerc = Converter.ToDecimal(dr["IGSTPerc"]),

                        Status = Converter.ToInt(dr["Status"])
                    });
                }
                return objInvoice;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteInvoice(int InvoiceNo)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intInvoiceNo", InvoiceNo)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_DeleteInvoice", sqlParam) > 0) return true;
                throw new FaultException("Invoice Master Not Deleted");
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public ObservableCollection<InvoiceReport> GenInvoiceReport(int fromInvoiceNo, int toInvoiceNo, DateTime fromDate, DateTime toDate)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intFromInvoiceNo", fromInvoiceNo),
                    [1] = new SqlParameter("@intToInvoiceNo", toInvoiceNo),
                    [2] = new SqlParameter("@datetimeFromInvoiceDate", fromDate.Date),
                    [3] = new SqlParameter("@datetimeToInvoiceDate", toDate.Date)
                };
                ObservableCollection<InvoiceReport> lstInvRept = new ObservableCollection<InvoiceReport>();
                DataSet ds = SqlUtility.ExecuteQueryWithDS(GetConnectionString(), "sp_im_GenInvoiceReport", sqlParam);
                DataTable dt = ds.Tables[1];
                ObservableCollection<InvDetails> lstInvDet = new ObservableCollection<InvDetails>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstInvDet.Add(new InvDetails
                    {
                        SrNo = lstInvDet.Count(p => p.InvoiceNo == Converter.ToInt(dr["InvoiceNo"])) + 1,
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        Description = Converter.ToString(dr["Description"]),
                        QTY = Converter.ToInt(dr["Quantity"]),
                        UnitPrice = Converter.ToDecimal(dr["UnitPrice"]),
                        NetSale = Converter.ToDecimal(dr["NetSale"]),
                        CGST = Converter.ToDecimal(dr["CGST"]),
                        SGST = Converter.ToDecimal(dr["SGST"]),
                        IGST = Converter.ToDecimal(dr["IGST"]),
                        Rate = Converter.ToDecimal(dr["Rate"]),
                        Shipping_UnitPrice = Converter.ToDecimal(dr["Shipping_UnitPrice"]),
                        Shipping_NetSale = Converter.ToDecimal(dr["Shipping_NetSale"]),
                        Shipping_CGST = Converter.ToDecimal(dr["Shipping_CGST"]),
                        Shipping_SGST = Converter.ToDecimal(dr["Shipping_SGST"]),
                        Shipping_IGST = Converter.ToDecimal(dr["Shipping_IGST"]),
                        Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"]),
                        CGSTPerc = Converter.ToDecimal(dr["CGSTPerc"]),
                        SGSTPerc = Converter.ToDecimal(dr["SGSTPerc"]),
                        IGSTPerc = Converter.ToDecimal(dr["IGSTPerc"]),
                        Total = Converter.ToInt(dr["Quantity"]) * Converter.ToDecimal(dr["Rate"])
                    });
                }
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    ObservableCollection<InvDetails> lstTempInvDet = new ObservableCollection<InvDetails>(lstInvDet.Where(p => p.InvoiceNo == Converter.ToInt(dr["InvoiceNo"])).ToList());
                    decimal InvoiceAmt;
                    InvoiceAmt = lstTempInvDet.Sum(p => p.Total + p.Shipping_Total);
                    lstInvRept.Add(new InvoiceReport
                    {
                        BillTo = Converter.ToString(dr["PartyName"]) + "\r\n" + Converter.ToString(dr["Address"]),
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]),
                        OrderNo = Converter.ToString(dr["OrderNo"]),
                        OrderDate = Converter.ToDateTime(dr["OrderDate"]),
                        GSTNo = Converter.ToString(dr["GSTNo"]),
                        PanNo = Converter.ToString(dr["PanNo"]),
                        lstDetails = lstTempInvDet,
                        InvoiceAmount = InvoiceAmt,
                        AmtInWords = IMSLibrary.Class.Helper.DecimalToWords(InvoiceAmt)
                    });
                }
                return lstInvRept;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        private List<InvoiceReport> GetFinalInvoiceReport(int invoiceNo)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>();
                sqlParam[0] = new SqlParameter("@intInvoiceNo", invoiceNo);
                List<InvoiceReport> lstInvRept = new List<InvoiceReport>();
                DataSet ds = SqlUtility.ExecuteQueryWithDS(GetConnectionString(), "sp_im_FinalInvoiceReport", sqlParam);
                DataTable dt = ds.Tables[1];
                List<InvDetails> lstInvDet = new List<InvDetails>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstInvDet.Add(new InvDetails
                    {
                        SrNo = lstInvDet.Count(p => p.InvoiceNo == Converter.ToInt(dr["InvoiceNo"])) + 1,
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        Description = Converter.ToString(dr["Description"]),
                        QTY = Converter.ToInt(dr["Quantity"]),
                        UnitPrice = Converter.ToDecimal(dr["UnitPrice"]),
                        NetSale = Converter.ToDecimal(dr["NetSale"]),
                        CGST = Converter.ToDecimal(dr["CGST"]),
                        SGST = Converter.ToDecimal(dr["SGST"]),
                        IGST = Converter.ToDecimal(dr["IGST"]),
                        Rate = Converter.ToDecimal(dr["Rate"]),
                        Shipping_UnitPrice = Converter.ToDecimal(dr["Shipping_UnitPrice"]),
                        Shipping_NetSale = Converter.ToDecimal(dr["Shipping_NetSale"]),
                        Shipping_CGST = Converter.ToDecimal(dr["Shipping_CGST"]),
                        Shipping_SGST = Converter.ToDecimal(dr["Shipping_SGST"]),
                        Shipping_IGST = Converter.ToDecimal(dr["Shipping_IGST"]),
                        Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"]),
                        CGSTPerc = Converter.ToDecimal(dr["CGSTPerc"]),
                        SGSTPerc = Converter.ToDecimal(dr["SGSTPerc"]),
                        IGSTPerc = Converter.ToDecimal(dr["IGSTPerc"]),
                        SettlementAmount = Converter.ToDecimal(dr["SettlementAmount"]),
                        DifferenceAmount = Converter.ToDecimal(dr["DifferenceAmount"]),
                        Status = Converter.ToInt(dr["Status"]),
                        Total = Converter.ToInt(dr["Quantity"]) * Converter.ToDecimal(dr["Rate"])
                    });
                }
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    ObservableCollection<InvDetails> lstTempInvDet = new ObservableCollection<InvDetails>(lstInvDet.Where(p => p.InvoiceNo == Converter.ToInt(dr["InvoiceNo"])).ToList());
                    string InvStatus, CommissionTab;
                    decimal InvoiceAmt;
                    InvoiceAmt = lstTempInvDet.Sum(p => p.Total + p.Shipping_Total);
                    if (lstTempInvDet.Count(p => p.Status == 1) == lstTempInvDet.Count()) InvStatus = "PENDING";
                    else if (lstTempInvDet.Count(p => p.Status == 2) == lstTempInvDet.Count()) InvStatus = "RETURNED";
                    else InvStatus = "PAID";
                    if (Convert.ToBoolean(dr["CommissionTab"])) CommissionTab = "Commission Amount";
                    else CommissionTab = "Difference Amount";
                    if (InvStatus == "RETURNED") CommissionTab = "Credit Amount";
                    lstInvRept.Add(new InvoiceReport
                    {
                        BillTo = Converter.ToString(dr["PartyName"]) + "\r\n" + Converter.ToString(dr["Address"]),
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]),
                        OrderNo = Converter.ToString(dr["OrderNo"]),
                        OrderDate = Converter.ToDateTime(dr["OrderDate"]),
                        GSTNo = Converter.ToString(dr["GSTNo"]),
                        PanNo = Converter.ToString(dr["PanNo"]),
                        lstDetails = lstTempInvDet,
                        InvoiceAmount = InvoiceAmt,
                        AmtInWords = IMSLibrary.Class.Helper.DecimalToWords(InvoiceAmt),
                        SettlementAmount = lstTempInvDet.Sum(p => p.SettlementAmount),
                        DifferenceAmount = lstTempInvDet.Sum(p => p.DifferenceAmount),
                        InvoiceStatus = InvStatus,
                        CommissionTab = CommissionTab
                    });
                }
                return lstInvRept;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<InvoiceReport> FinalInvoiceReport(int fromInvoiceNo, int toInvoiceNo, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<InvoiceReport> lstInvRept = new List<InvoiceReport>();
                if (fromInvoiceNo == 0 && toInvoiceNo == 0 && fromDate != new DateTime(1900, 01, 01))
                {
                    Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                    {
                        [0] = new SqlParameter("@datetimeFromInvoiceDate", fromDate.Date),
                        [1] = new SqlParameter("@datetimeToInvoiceDate", toDate.Date)
                    };
                    DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_im_ListInvoiceNo", sqlParam);
                    foreach (DataRow dr in dt.Rows)
                    {
                        var lstReportData = GetFinalInvoiceReport(Converter.ToInt(dr["InvoiceNo"].ToString()));
                        if (lstReportData.Count > 0)
                        {
                            lstInvRept.AddRange(lstReportData);
                        }
                    }
                }
                else
                {
                    for (int i = fromInvoiceNo; i <= toInvoiceNo; i++)
                    {
                        var lstReportData = GetFinalInvoiceReport(i);
                        if (lstReportData.Count > 0)
                        {
                            lstInvRept.AddRange(lstReportData);
                        }
                    }
                }
                return lstInvRept;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public ObservableCollection<InvoiceStatus> StatuswiseInvoiceReport(DateTime fromDate, DateTime toDate, int CategoryID, int SubCategoryID, int GroupID, int VendorID, int Status)
        {
            try
            {
                string query = StatuswiseReportQuery(fromDate, toDate);
                if (CategoryID > 0)
                {
                    query += "And PDM.Category = " + CategoryID;
                    if (SubCategoryID > 0)
                    {
                        query += "And PDM.SubCategory = " + SubCategoryID;
                    }
                }
                if (GroupID > 0)
                {
                    query += "And PDM.GroupID = " + GroupID;
                }
                if (VendorID > 0)
                {
                    query += "And IDM.VendorID = " + VendorID;
                }
                if (Status > 0)
                {
                    query += "And IPM.Status = " + Status;
                }
                SqlConnection sqlCon = new SqlConnection(GetConnectionString());
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ObservableCollection<InvoiceStatus> lstInvoiceStatus = new ObservableCollection<InvoiceStatus>();
                string InvoiceStatus = string.Empty, GSTStatus = string.Empty;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        InvoiceStatus = Converter.ToString(dr["Status"]);
                        GSTStatus = "Nil";
                        if (InvoiceStatus == "Received" || InvoiceStatus == "Delivered") GSTStatus = "Paid";
                        if (InvoiceStatus == "Pending" || InvoiceStatus == "Check") GSTStatus = "Pending";

                        lstInvoiceStatus.Add(new InvoiceStatus
                        {
                            InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                            InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]),
                            OrderNo = Converter.ToString(dr["OrderNo"]),
                            OrderDate = Converter.ToDateTime(dr["OrderDate"]),
                            PartyName = Converter.ToString(dr["PartyName"]),
                            ProductCode = Converter.ToString(dr["ProductCode"]),
                            Quantity = Converter.ToInt(dr["Quantity"]),
                            SellingPrice = Converter.ToDecimal(dr["SellingPrice"]),

                            Shipping_CGST = Converter.ToDecimal(dr["Shipping_CGST"]),
                            Shipping_SGST = Converter.ToDecimal(dr["Shipping_SGST"]),
                            Shipping_IGST = Converter.ToDecimal(dr["Shipping_IGST"]),
                            Shipping_NetSale = Converter.ToDecimal(dr["Shipping_NetSale"]),
                            Shipping_UnitPrice = Converter.ToDecimal(dr["Shipping_UnitPrice"]),
                            Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"]),

                            CGST = Converter.ToDecimal(dr["CGST"]),
                            SGST = Converter.ToDecimal(dr["SGST"]),
                            IGST = Converter.ToDecimal(dr["IGST"]),
                            CGSTPerc = Converter.ToDecimal(dr["CGSTPerc"]),
                            SGSTPerc = Converter.ToDecimal(dr["SGSTPerc"]),
                            IGSTPerc = Converter.ToDecimal(dr["IGSTPerc"]),

                            Total = Converter.ToDecimal(dr["Total"]),
                            SettlementAmount = Converter.ToDecimal(dr["SettlementAmount"]),
                            SettlementDate = Converter.ToDateTime(dr["SettlementDate"]),
                            Category = Converter.ToString(dr["Category"]),
                            SubCategory = Converter.ToString(dr["SubCategory"]),
                            Group = Converter.ToString(dr["Group"]),
                            Vendor = Converter.ToString(dr["Vendor"]),
                            Status = InvoiceStatus,
                            GSTStatus = GSTStatus
                        });
                    }
                }
                dr.Close();
                cmd.Dispose();
                sqlCon.Close();
                return lstInvoiceStatus;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        private string StatuswiseReportQuery(DateTime fromDate, DateTime toDate)
        {
            return "Select IPM.InvoiceNo As InvoiceNo, " +
                                   "IPM.InvoiceDate As InvoiceDate, " +
                                   "IPM.OrderID As OrderNo, " +
                                   "IPM.OrderDate As OrderDate, " +
                                   "IDM.PartysName As PartyName, " +
                                   "IPM.ProductID As ProductCode, " +
                                   "IPM.Quantity As Quantity, " +
                                   "IPM.Rate As SellingPrice, " +
                                   "IPM.Total As Total, " +
                                   "IPM.CGST As CGST, " +
                                   "IPM.SGST As SGST, " +
                                   "IPM.IGST As IGST, " +

                                   "IPM.CGSTPerc As CGSTPerc, " +
                                   "IPM.SGSTPerc As SGSTPerc, " +
                                   "IPM.IGSTPerc As IGSTPerc, " +

                                   "IPM.Shipping_CGST As Shipping_CGST, " +
                                   "IPM.Shipping_SGST As Shipping_SGST, " +
                                   "IPM.Shipping_IGST As Shipping_IGST, " +

                                   "IPM.Shipping_UnitPrice As Shipping_UnitPrice, " +
                                   "IPM.Shipping_NetSale As Shipping_NetSale, " +
                                   "IPM.Shipping_Total As Shipping_Total, " +

                                   "IPM.SettlementAmount As SettlementAmount, " +
                                   "(Select MAX(ISM.SettlementDate) From InvoiceSettlementMaster ISM Where ISM.InvoiceDate = IPM.InvoiceDate And ISM.InvoiceNo = IPM.InvoiceNo And ISM.OrderDate = IPM.OrderDate And ISM.OrderID = IPM.OrderID And ISM.ProductID = IPM.ProductID) As SettlementDate, " +
                                   "(Select Distinct(CM.Name) From CategoryMaster CM Where CM.CategoryID = PDM.Category) As Category, " +
                                   "(Select Distinct(SCM.Name) From SubCategoryMaster SCM Where SCM.CategoryID = PDM.Category And SCM.SubCategoryID = PDM.SubCategory) As SubCategory, " +
                                   "(Select Distinct(SLM.SubLookupName) From SubLookupMaster SLM Where SLM.LookupID = 18 And SLM.SubLookupID = PDM.GroupID) As [Group], " +
                                   "(Select Distinct(VM.VendorName) From VendorMaster VM Where VM.VendorID = IDM.VendorID) As Vendor," +
                                   "(Select Distinct(SLM.SubLookupName) From SubLookupMaster SLM Where SLM.LookupID = 14 And SLM.SubLookupID = IPM.Status) As Status " +
                                   "From InvoiceProductMaster IPM " +
                                   "Join ProductDescriptionMaster PDM On IPM.ProductID = PDM.ProductID " +
                                   "Join InvoiceDetailsMaster IDM On IPM.InvoiceDate = IDM.InvoiceDate And IPM.InvoiceNo = IDM.InvoiceNo And IPM.OrderDate = IDM.OrderDate And IPM.OrderID = IDM.OrderID " +
                                   "Where IPM.InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "' And IPM.InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "' ";
        }

        #endregion ==Invoice==

        #region ==Vendor==

        public ObservableCollection<VendorMaster> GetVendorList()
        {
            try
            {
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_vm_GetVendorList", new Dictionary<int, SqlParameter>());
                ObservableCollection<VendorMaster> lstVendor = new ObservableCollection<VendorMaster>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstVendor.Add(new VendorMaster
                    {
                        ID = Converter.ToInt(dr["VendorID"]),
                        Name = Converter.ToString(dr["VendorName"])
                    });
                }
                return lstVendor;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public VendorMaster GetVendorDetails(int VendorID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intVendorID", VendorID)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_vm_GetVendorDetails", sqlParam);
                VendorMaster objVendor = new VendorMaster();
                DataRow dr = dt.Rows[0];
                if (dr != null)
                {
                    objVendor.ID = Convert.ToInt32(dr["VendorID"]);
                    objVendor.Name = Converter.ToString(dr["VendorName"]);
                    objVendor.Address = Converter.ToString(dr["Address"]);
                    objVendor.GSTNo = Converter.ToString(dr["GSTNo"]);
                    objVendor.PanNo = Converter.ToString(dr["PanNo"]);
                    objVendor.CommissionTab = Convert.ToBoolean(dr["CommissionTab"]);
                }
                return objVendor;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateVendor(VendorMaster objVendor, int UserID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intVendorID", objVendor.ID),
                    [1] = new SqlParameter("@nvarcharVendorName", Converter.ToString(objVendor.Name)),
                    [2] = new SqlParameter("@nvarcharAddress", Converter.ToString(objVendor.Address)),
                    [3] = new SqlParameter("@nvarcharGSTNo", Converter.ToString(objVendor.GSTNo)),
                    [4] = new SqlParameter("@nvarcharPanNo", Converter.ToString(objVendor.PanNo)),
                    [5] = new SqlParameter("@bitCommissionTab", objVendor.CommissionTab),
                    [6] = new SqlParameter("@intCreatedModifiedBy", UserID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_vm_InsertUpdateVendor", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteVendor(int VendorID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intVendorID", VendorID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_vm_DeleteVendor", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool CheckVendorNameAvailability(string VendorName)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharVendorName", Converter.ToString(VendorName))
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_vm_CheckVendorNameAvailability", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Vendor==

        #region ==Company==

        public CompanyMaster GetCompanyDetails()
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intID", 1)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_comp_GetCompanyDetails", sqlParam);
                CompanyMaster objCompanyDet = new CompanyMaster();
                DataRow dr = dt.Rows[0];
                if (dr != null)
                {
                    objCompanyDet.Name = Converter.ToString(dr["Name"]);
                    objCompanyDet.Address1 = Converter.ToString(dr["Address1"]);
                    objCompanyDet.Address2 = Converter.ToString(dr["Address2"]);
                    objCompanyDet.Address3 = Converter.ToString(dr["Address3"]);
                    objCompanyDet.ContactNo1 = Converter.ToString(dr["ContactNo1"]);
                    objCompanyDet.ContactNo2 = Converter.ToString(dr["ContactNo2"]);
                    objCompanyDet.EmailID = Converter.ToString(dr["EmailID"]);
                    objCompanyDet.GSTNo = Converter.ToString(dr["GSTNo"]);
                    try
                    {
                        objCompanyDet.Logo = (byte[])dr["Logo"];
                    }
                    catch
                    {
                        objCompanyDet.Logo = null;
                    }
                }
                return objCompanyDet;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Company==

        #region ==Category==

        public bool CheckCategoryName(string Name, int CategoryID, int PageMode)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharName", Converter.ToString(Name)),
                    [1] = new SqlParameter("@intCategoryID", CategoryID),
                    [2] = new SqlParameter("@intPageMode", PageMode)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_cm_CheckCategoryNameAvailability", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool CheckSubCategoryName(string Name, int CategoryID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharName", Converter.ToString(Name)),
                    [1] = new SqlParameter("@intCategoryID", CategoryID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_slm_CheckSubCategoryNameAvailability", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateCategory(string Name, int CategoryID, decimal Cgst, decimal Sgst, decimal Igst)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharName", Name),
                    [1] = new SqlParameter("@intCategoryID", CategoryID),
                    [2] = new SqlParameter("@decimalCgst", Cgst),
                    [3] = new SqlParameter("@decimalSgst", Sgst),
                    [4] = new SqlParameter("@decimalIgst", Igst)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_cm_InsertUpdateCategory", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool InsertUpdateSubCategory(string Name, int CategoryID, int SubCategoryID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharName", Name),
                    [1] = new SqlParameter("@intCategoryID", CategoryID),
                    [2] = new SqlParameter("@intSubCategoryID", SubCategoryID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_scm_InsertUpdateSubCategory", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<Category> GetCategoryList()
        {
            try
            {
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_cm_GetCategoryList", new Dictionary<int, SqlParameter>());
                List<Category> lstCategory = new List<Category>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstCategory.Add(new Category
                    {
                        CategoryID = (int)dr["CategoryID"],
                        Name = (string)dr["Name"],
                        CGST = (decimal)dr["CGST"],
                        SGST = (decimal)dr["SGST"],
                        IGST = (decimal)dr["IGST"]
                    });
                }
                return lstCategory;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<SubCategory> GetSubCategoryList(int CategoryID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intCategoryID", CategoryID)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_scm_GetSubCategoryList", sqlParam);
                List<SubCategory> lstSubCategory = new List<SubCategory>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstSubCategory.Add(new SubCategory
                    {
                        SubCategoryID = (int)dr["SubCategoryID"],
                        CategoryID = (int)dr["CategoryID"],
                        Name = (string)dr["Name"]
                    });
                }
                return lstSubCategory;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteCategory(int CategoryID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intCategoryID", CategoryID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_cm_DeleteCategory", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteSubCategory(int CategoryID, int SubCategoryID)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intCategoryID", CategoryID),
                    [1] = new SqlParameter("@intSubCategoryID", SubCategoryID)
                };
                if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_scm_DeleteSubCategory", sqlParam) > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Category==

        #region ==Patches==

        public bool BulkDeleteInvoice(int frmInvoiceNo, int toInvoiceNo)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intfrmInvoiceNo", frmInvoiceNo),
                    [1] = new SqlParameter("@inttoInvoiceNo", toInvoiceNo)
                };
                SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_BulkDeleteInvoice", sqlParam);
                return true;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Patches==

        #region ==Reports==

        public ObservableCollection<TaxReport> MonthVendorTaxReport(int VendorID, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intVendorID", VendorID),
                    [1] = new SqlParameter("@datetimeFromDate", Converter.ToDateTime(FromDate)),
                    [2] = new SqlParameter("@datetimeToDate", Converter.ToDateTime(ToDate))
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_tx_MonthVendorTaxReport", sqlParam);
                ObservableCollection<TaxReport> lstTaxReport = new ObservableCollection<TaxReport>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstTaxReport.Add(new TaxReport
                    {
                        Month = Converter.ToMonth(dr["Month"]),
                        Vendor = Converter.ToString(dr["Vendor"]),
                        CGSTRate = Converter.ToDecimal(dr["CGSTRate"]),
                        SGSTRate = Converter.ToDecimal(dr["SGSTRate"]),
                        IGSTRate = Converter.ToDecimal(dr["IGSTRate"]),

                        CGST = Converter.ToDecimal(dr["CGST"]),
                        SGST = Converter.ToDecimal(dr["SGST"]),
                        IGST = Converter.ToDecimal(dr["IGST"]),

                        Shipping_CGST = Converter.ToDecimal(dr["Shipping_CGST"]),
                        Shipping_SGST = Converter.ToDecimal(dr["Shipping_SGST"]),
                        Shipping_IGST = Converter.ToDecimal(dr["Shipping_IGST"]),

                        Total = Converter.ToDecimal(dr["Total"]),
                        Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"])
                    });
                }
                return lstTaxReport;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public ObservableCollection<SalesReport> Rpt_Sales(DateTime FrmSalesDt, DateTime ToSalesDt, string PrdCd, int VendorID, int GroupID, int CategoryID, int SubCategoryID)
        {
            try
            {
                string ProductCode = Converter.ToString(PrdCd);
                if (string.IsNullOrEmpty(ProductCode)) ProductCode = "EMPTY";
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@intVendorID", VendorID),
                    [1] = new SqlParameter("@datetimeFromSalesDate", Converter.ToDateTime(FrmSalesDt)),
                    [2] = new SqlParameter("@datetimeToSalesDate", Converter.ToDateTime(ToSalesDt)),
                    [3] = new SqlParameter("@nvarcharPrdCd", ProductCode),
                    [4] = new SqlParameter("@intGroupID", GroupID),
                    [5] = new SqlParameter("@intCategoryID", CategoryID),
                    [6] = new SqlParameter("@intSubCategoryID", SubCategoryID)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_sm_SalesReport", sqlParam);
                ObservableCollection<SalesReport> lstSalesReport = new ObservableCollection<SalesReport>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstSalesReport.Add(new SalesReport
                    {
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]),
                        OrderID = Converter.ToString(dr["OrderID"]),
                        OrderDate = Converter.ToDateTime(dr["OrderDate"]),
                        PartyName = Converter.ToString(dr["PartysName"]),
                        PrdCd = Converter.ToString(dr["PrdCd"]),
                        PrdName = Converter.ToString(dr["PrdName"]),
                        Vendor = Converter.ToString(dr["Vendor"]),
                        SellingPrice = Converter.ToDecimal(dr["SellingPrice"]),

                        UnitPrice = Converter.ToDecimal(dr["UnitPrice"]),
                        Quantity = Converter.ToInt(dr["Quantity"]),
                        NetSales = Converter.ToDecimal(dr["NetSales"]),
                        TaxAmount = Converter.ToDecimal(dr["TaxAmount"]),
                        SubTotal = Converter.ToDecimal(dr["SubTotal"]),

                        Shipping_UnitPrice = Converter.ToDecimal(dr["Shipping_UnitPrice"]),
                        Shipping_NetSale = Converter.ToDecimal(dr["Shipping_NetSale"]),
                        ShippingTaxAmount = Converter.ToDecimal(dr["ShippingTaxAmount"]),
                        Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"]),

                        CGST = Converter.ToDecimal(dr["CGST"]),
                        SGST = Converter.ToDecimal(dr["SGST"]),
                        IGST = Converter.ToDecimal(dr["IGST"]),

                        CGSTPerc = Converter.ToDecimal(dr["CGSTPerc"]),
                        SGSTPerc = Converter.ToDecimal(dr["SGSTPerc"]),
                        IGSTPerc = Converter.ToDecimal(dr["IGSTPerc"]),

                        Shipping_CGST = Converter.ToDecimal(dr["Shipping_CGST"]),
                        Shipping_SGST = Converter.ToDecimal(dr["Shipping_SGST"]),
                        Shipping_IGST = Converter.ToDecimal(dr["Shipping_IGST"]),

                        InvoiceAmount = Converter.ToDecimal(dr["InvoiceAmount"]),
                    });
                }
                return lstSalesReport;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==Reports==

        #region ==CreditNote==

        public ObservableCollection<CreditNoteReport> GenCreditNoteReport(string crNoteNo, string orderID, DateTime fromDate, DateTime toDate)
        {
            try
            {
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharCrNoteNo", Converter.ToString(crNoteNo)),
                    [1] = new SqlParameter("@nvarcharOrderID", Converter.ToString(orderID)),
                    [2] = new SqlParameter("@datetimeFromCrNoteDate", fromDate.Date),
                    [3] = new SqlParameter("@datetimeToCrNoteDate", toDate.Date)
                };
                ObservableCollection<CreditNoteReport> lstCreditNoteReport = new ObservableCollection<CreditNoteReport>();
                DataSet ds = SqlUtility.ExecuteQueryWithDS(GetConnectionString(), "sp_Cr_CreditNoteReport", sqlParam);
                DataTable dt = ds.Tables[1];
                ObservableCollection<InvDetails> lstInvDet = new ObservableCollection<InvDetails>();
                foreach (DataRow dr in dt.Rows)
                {
                    lstInvDet.Add(new InvDetails
                    {
                        SrNo = lstInvDet.Count(p => p.InvoiceNo == Converter.ToInt(dr["InvoiceNo"])) + 1,
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        Description = Converter.ToString(dr["Description"]),
                        QTY = Converter.ToInt(dr["Quantity"]),

                        UnitPrice = Converter.ToDecimal(dr["UnitPrice"]),
                        NetSale = Converter.ToDecimal(dr["NetSale"]),
                        CGST = Converter.ToDecimal(dr["CGST"]),
                        SGST = Converter.ToDecimal(dr["SGST"]),
                        IGST = Converter.ToDecimal(dr["IGST"]),

                        Shipping_UnitPrice = Converter.ToDecimal(dr["Shipping_UnitPrice"]),
                        Shipping_NetSale = Converter.ToDecimal(dr["Shipping_NetSale"]),
                        Shipping_CGST = Converter.ToDecimal(dr["Shipping_CGST"]),
                        Shipping_SGST = Converter.ToDecimal(dr["Shipping_SGST"]),
                        Shipping_IGST = Converter.ToDecimal(dr["Shipping_IGST"]),
                        Shipping_Total = Converter.ToDecimal(dr["Shipping_Total"]),

                        CGSTPerc = Converter.ToDecimal(dr["CGSTPerc"]),
                        SGSTPerc = Converter.ToDecimal(dr["SGSTPerc"]),
                        IGSTPerc = Converter.ToDecimal(dr["IGSTPerc"]),

                        Total = Converter.ToInt(dr["NetSale"]) + Converter.ToDecimal(dr["CGST"]) + Converter.ToDecimal(dr["SGST"]) + Converter.ToDecimal(dr["IGST"])
                    });
                }
                dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    ObservableCollection<InvDetails> lstTempInvDet = new ObservableCollection<InvDetails>(lstInvDet.Where(p => p.InvoiceNo == Converter.ToInt(dr["InvoiceNo"])).ToList());
                    decimal InvoiceAmt;
                    InvoiceAmt = lstTempInvDet.Sum(p => p.Total + p.Shipping_Total);
                    lstCreditNoteReport.Add(new CreditNoteReport
                    {
                        CrNoteNo = Converter.ToString(dr["CrNoteNo"]),
                        CrNoteDate = Converter.ToDateTime(dr["CrNoteDate"]),
                        BillTo = Converter.ToString(dr["PartyName"]) + "\r\n" + Converter.ToString(dr["Address"]),
                        InvoiceNo = Converter.ToInt(dr["InvoiceNo"]),
                        OrderID = Converter.ToString(dr["OrderID"]),
                        OrderDate = Converter.ToDateTime(dr["OrderDate"]),
                        lstDetails = lstTempInvDet,
                        InvoiceAmount = InvoiceAmt,
                        AmtInWords = IMSLibrary.Class.Helper.DecimalToWords(InvoiceAmt)
                    });
                }
                return lstCreditNoteReport;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        #endregion ==CreditNote==

        public decimal FindGstRate(string ProductID, string TaxType)
        {
            try
            {
                decimal Rate = 0M;
                int gstType = 0;
                if (TaxType == "CGST")
                {
                    gstType = 1;
                }
                else if (TaxType == "SGST")
                {
                    gstType = 2;
                }
                else if (TaxType == "IGST")
                {
                    gstType = 3;
                }
                decimal result = 0M;
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>
                {
                    [0] = new SqlParameter("@nvarcharProductID", ProductID),
                    [1] = new SqlParameter("@intGstType", gstType)
                };
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_FindGstRate", sqlParam);
                DataRow dr = dt.Rows[0];
                if (dr != null)
                {
                    result = Converter.ToDecimal(dr["Rate"]);
                }
                if (result > 0)
                {
                    return result;
                }
                return Rate;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        private int GetStatusID(string Status)
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlCommand cmd = new SqlCommand("Select SubLookupID From SubLookupMaster Where LookupID = 14 And SubLookupName = '" + Status + "'", sqlCon);
            sqlCon.Open();
            int StatusID = Converter.ToInt(cmd.ExecuteScalar());
            sqlCon.Close();
            cmd.Dispose();
            if (StatusID == 0)
            {
                StatusID = Const.InvoiceStatusNotFound;
            }
            return StatusID;
        }

        private int GetSrNoForSettlement(string OrderID, string ProductID, DateTime SettlementDate, decimal SettlementAmount, int Status)
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            string query = "Select Count(*) From InvoiceSettlementMaster Where OrderID = '" + OrderID + "' And ProductID = '" + ProductID + "' And SettlementDate = '" + SettlementDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' And SettlementAmount = " + SettlementAmount + " And Status = " + Status;
            SqlCommand cmd = new SqlCommand(query, sqlCon);
            sqlCon.Open();
            int SrNo = Converter.ToInt(cmd.ExecuteScalar());
            if (SrNo > 0)
            {
                SrNo = Const.DuplicateSettlement;
            }
            else
            {
                query = "Select Max(SrNo) From InvoiceSettlementMaster Where OrderID = '" + OrderID + "' And ProductID = '" + ProductID + "' And SettlementDate = '" + SettlementDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
                cmd = new SqlCommand(query, sqlCon);
                SrNo = Converter.ToInt(cmd.ExecuteScalar()) + 1;
            }
            sqlCon.Close();
            cmd.Dispose();
            return SrNo;
        }
    }
}