using IMSLibrary;
using IMSLibrary.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using TestProject.InvoiceServiceRef;

////using Excel = Microsoft.Office.Interop.Excel;

namespace TestProject
{
    //<endpoint address="http://localhost:6519/Services/DataService.svc"
    //                binding="basicHttpBinding" bindingConfiguration="BasicHttpBinding_IDataService"
    //                contract="DataServiceRef.IDataService" name="BasicHttpBinding_IDataService" />

    // //class Program
    // //{
    // //    static void Main(string[] args)
    // //    {
    // //        string val1 = "151";
    // //        int val2 = 151;

    // //        string sourPath = @"C:\Users\rohan\Downloads\Invoicing feed.xlsx";
    // //        string destPath = @"http://192.168.137.172/CaheadService/Images/Invoicing feed.xlsx";

    // //        FileStream fs = new FileStream(sourPath, FileMode.Open, FileAccess.Read);
    // //        BinaryReader br = new BinaryReader(fs);
    // //        byte[] photo = br.ReadBytes((int)fs.Length);
    // //        br.Close();
    // //        fs.Close();
    // //        //return photo;

    // //        System.IO.File.WriteAllBytes(@"C:\Users\rohan\Downloads\TEMPInvoicingfeed.xlsx", photo);

    // //        //File.Copy(sourPath, destPath);

    // //        WebClient a = new WebClient();
    // //        a.Credentials = new NetworkCredential("rohan", "Rohan", "cahead.com");
    // //        //WebRequest serverRequest = WebRequest.Create(@"http://192.168.137.1/CaheadService/Images/");
    // //        a.UploadFile(destPath, sourPath);

    // //        StreamReader reader = new StreamReader(sourPath);
    // //        byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(reader.ReadToEnd());
    // //        System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

    // //        //using (FileStream file = new FileStream(destPath, FileMode.Create, System.IO.FileAccess.Write))
    // //        //{
    // //            //byte[] bytes = new byte[stream.Length];
    // //            //stream.Read(bytes, 0, (int)stream.Length);
    // //            //file.Write(bytes, 0, bytes.Length);
    // //            //stream.Close();
    // //        //}
    // //    }
    // //}

    // //public class MyClass
    // //{
    // //    static MyClass()
    // //    {
    // //        Console.WriteLine("In MyStatic Class");
    // //    }

    // //    public MyClass()
    // //    {
    // //        Console.WriteLine("In MyClass");
    // //    }

    // //    public MyClass(int i)
    // //    {
    // //        Console.WriteLine("In MyClass (int)");
    // //    }

    // //    public static void Main(String[] arg)
    // //    {
    // //        new MyClass(23);
    // //    }
    // //}

    internal class Program
    {
        public static void Run(Object state)
        {
            Console.WriteLine("Line 4");
        }

        public static void Main(String[] arg)
        {
            UpdateVatForCategory12();
            return;

            TestProject.Master.InvoiceProductPatch ABCD = new Master.InvoiceProductPatch();
            ABCD.UpdateTaxRate();
            return;

            InvoicePatch obj = new InvoicePatch();
            obj.UpdateInvoiceAmount();
            return;

            string TaxGroup1 = "CST*9";
            string TaxType = string.Empty;
            decimal TaxRate = 0.00M;
            if (!string.IsNullOrEmpty(TaxGroup1))
            {
                string[] splitTax = TaxGroup1.Split('-');
                if (splitTax.Count() > 1)
                {
                    TaxType = splitTax[0];
                    TaxRate = Converter.ToDecimal(splitTax[1]);
                }
                else
                {
                    TaxType = TaxGroup1;
                    TaxRate = 0.00M;
                }
            }

            decimal VatCstRate = 0M;

            if (TaxType == "CST")
            {
                if (TaxRate > 0.00M)
                {
                    VatCstRate = TaxRate + 100;
                }
            }
            else
            {
                if (TaxRate > 0.00M)
                {
                    VatCstRate = TaxRate + 100;
                }
            }

            // //InvoiceServiceTest();
            // //Note: DateTime object
            // //.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

            // //Step-1
            // //CreditNotePatch crPatch = new CreditNotePatch();
            // //crPatch.UpdateCrNote();
            // //crPatch.DeleteDuplicateCrNote();

            // //Step-2
            // //InvoicePatch obj = new InvoicePatch();
            // //obj.UpdateInvoice();

            // //Step-3
            // //DuplicateInvoiceProductPatch obj1 = new DuplicateInvoiceProductPatch();
            // //obj1.UpdateInvoiceProductMasterStatus();

            return;

            //IDemo a = new Demo();
            //a.Print();

            //Abc ob = new Abc();
            //ob.A();
            //return;

            //Console.WriteLine("A" + "\r\n" + "A");

            //Console.WriteLine(DecimalToWords(325.46M));

            int UserID = 3;
            string FilePath = @"D:\DONT DELETE\Desktop\Invoietoday\invoice (188).xlsx";//Invoicing feed.xlsx";
            int SheetType = 1;
            string FileName = "Invoicing feed";
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + ";Extended Properties ='Excel 8.0;HDR=Yes';";
            using (OleDbConnection OleDbCon = new OleDbConnection(con))
            {
                OleDbCon.Open();
                OleDbCommand oleDbCommand = new OleDbCommand();
                if (SheetType == 1)
                {
                    oleDbCommand = new OleDbCommand("Select * From [Invoicing$]", OleDbCon);
                    //oleDbCommand = new OleDbCommand("Select [Date], [Invoice No],[Party's Name], [Order ID], [Order Date], [Address], [Email ID], [Mobile No], [Product], [Rate], [Quantity], [Total], [Net Sales], [VAT], [Packaging], [Invoice Amt], [Sales Group] From [Invoicing feed$] Where [Invoice No] > 0", OleDbCon);
                }
                else if (SheetType == 2)
                {
                    oleDbCommand = new OleDbCommand("Select SettlementDate,Order ID,Invoice No,Invoice Date,Product Code,Quantity,Invoice Amt,Settlement Amount,Status from [Amount Receivable$]", OleDbCon);
                }
                else if (SheetType == 3)
                {
                    //oleDbCommand = new OleDbCommand("Select Product Code, Title, Size, MRP, PosterType, PosterDimensions,Dimensions in Inches, Poster Weight, Package Contents, Packaging Information, Shipping Duration, Variant, Color, Keywords," +
                    //    "SocialName,Publisher,PaperType,Description,Category,SubCategory,ArtistName,PaintingStyle,BlackAndWhitePoster,ColorPoster,PaperFinish,Shape,Orientation,Framed,FrameMaterial,OtherFrameDetails,Width,Height,PaperDepth," +
                    //    "Weight,OtherDimensions,OtherFeatures,SupplierImage,ImageLink,Note,WarrantSummary From MasterSheet");
                    oleDbCommand = new OleDbCommand("Select * From [Master Sheet$]", OleDbCon);
                }
                OleDbDataReader dr = oleDbCommand.ExecuteReader();
                StreamWriter sw = new StreamWriter(FileName);
                sw.WriteLine("       InvoiceNo      InvoiceDate        OrderID        ProductID          ErrorMessage");
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
                        if (SheetType == 1)
                        {
                            int quantity = 0;
                            decimal rate, unitPrice, netSales, total = 0.00M;
                            decimal CST = 0.00M, VAT = 0.00M, ShippingHandling = 0.00M;
                            DateTime orderDate = new DateTime();
                            string TaxGroup, PartyName, Address, EmailID, MobNo, Vendor;

                            orderID = Converter.ToString(dr["Order ID"].ToString());
                            if (string.IsNullOrEmpty(orderID)) continue;
                            orderDate = Converter.ToDateTime(dr["Date"]);
                            if (orderDate <= new DateTime(1900, 01, 01)) orderDate = DateTime.Now.Date;
                            invoiceNo = 0;//Converter.ToInt(dr["Invoice No"]);
                            //if (invoiceNo == 0) continue;
                            invoiceDate = DateTime.Now.Date;

                            productID = Converter.ToString(dr["SKU"]).ToUpper();
                            quantity = Converter.ToInt(dr["Quantity"]);
                            rate = Converter.ToDecimal(dr["Rate"]);
                            TaxGroup = Converter.ToString(dr["Tax Group"]);
                            PartyName = Converter.ToString(dr["Party's Name"]);
                            Address = Converter.ToString(dr["Address"]);
                            EmailID = Converter.ToString(dr["Email ID"]);
                            MobNo = Converter.ToString(dr["Mobile No"]);
                            ShippingHandling = Math.Round((Converter.ToDecimal(dr["Shipping & Handling"])), 2);
                            Vendor = Converter.ToString(dr["Sales Group"]);

                            if (TaxGroup == "CST")
                            {
                                unitPrice = Math.Round(((rate / 102) * 100), 2);
                                CST = (rate - unitPrice) * quantity;
                            }
                            else
                            {
                                unitPrice = Math.Round(((rate / 105) * 100), 2);
                                VAT = (rate - unitPrice) * quantity;
                            }

                            netSales = unitPrice * quantity;
                            total = (rate * quantity) + Math.Round((Converter.ToDecimal(dr["Shipping & Handling"])), 2);

                            sqlParam[0] = new SqlParameter("@datetimeInvoiceDate", invoiceDate);
                            sqlParam[1] = new SqlParameter("@intInvoicNo", invoiceNo);
                            sqlParam[2] = new SqlParameter("@varcharPartysName", dr[2]);
                            sqlParam[3] = new SqlParameter("@intOrderID", orderID);
                            sqlParam[4] = new SqlParameter("@datetimeOrderDate", Convert.ToDateTime(dr[4]));
                            sqlParam[5] = new SqlParameter("@varcharAddress", dr[5]);
                            sqlParam[6] = new SqlParameter("@varcharEmailID", dr[6]);
                            sqlParam[7] = new SqlParameter("@varcharMobileNo", dr[7]);
                            sqlParam[8] = new SqlParameter("@varcharProductID", productID);
                            sqlParam[9] = new SqlParameter("@decimalRate", Convert.ToDecimal(dr[9]));
                            sqlParam[10] = new SqlParameter("@intQuantity", Convert.ToInt32(dr[10]));
                            sqlParam[11] = new SqlParameter("@decimalTotal", Convert.ToDecimal(dr[11]));
                            sqlParam[12] = new SqlParameter("@decimalNetSale", Convert.ToDecimal(dr[12]));
                            sqlParam[13] = new SqlParameter("@decimalVat", Convert.ToDecimal(dr[13]));
                            sqlParam[14] = new SqlParameter("@decimalPackagingAndForwarding", Convert.ToDecimal(dr[14]));
                            sqlParam[15] = new SqlParameter("@decimalInvoiceAmount", Convert.ToDecimal(dr[15]));
                            sqlParam[16] = new SqlParameter("@nvarcharVendor", dr[16]);
                            sqlParam[17] = new SqlParameter("@intCreatedModifiedBy", UserID);
                            if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_InsertInvoicingFeed", sqlParam) > 0)
                            {
                                return;
                            }
                        }
                        if (SheetType == 2)
                        {
                            invoiceNo = Convert.ToInt32(dr[2]);
                            orderID = Convert.ToString(dr[1]);
                            invoiceDate = Convert.ToDateTime(dr[3]);
                            productID = dr[4].ToString();

                            sqlParam[0] = new SqlParameter("@datetimeSettlementDate", dr[0]);
                            sqlParam[1] = new SqlParameter("@intOrderID", dr[1]);
                            sqlParam[2] = new SqlParameter("@intInvoiceNo", dr[2]);
                            sqlParam[3] = new SqlParameter("@datetimeInvoiceDate", dr[3]);
                            sqlParam[4] = new SqlParameter("@varcharProductID", dr[4]);
                            sqlParam[5] = new SqlParameter("@decimalSettlementAmount", dr[5]);
                            sqlParam[6] = new SqlParameter("@intStatus", dr[6]);
                            sqlParam[7] = new SqlParameter("@intCreatedModifiedBy", UserID);
                            if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_im_UpdateAmountReceivable", sqlParam) > 0)
                            {
                                return;
                            }
                        }
                        if (SheetType == 3)
                        {
                            productID = dr[0].ToString();

                            sqlParam[0] = new SqlParameter("@varcharProductID", dr[0]);
                            sqlParam[1] = new SqlParameter("@varcharTitle", dr[1]);
                            sqlParam[2] = new SqlParameter("@varcharSize", dr[2]);
                            sqlParam[3] = new SqlParameter("@decimalMRP", dr[3]);
                            sqlParam[4] = new SqlParameter("@varcharPosterType", dr[4]);
                            sqlParam[5] = new SqlParameter("@varcharPosterDimensions", dr[5]);
                            sqlParam[6] = new SqlParameter("@varcharDimensionsInInches", dr[6]);
                            sqlParam[7] = new SqlParameter("@intPosterWeight", dr[7]);
                            sqlParam[8] = new SqlParameter("@varcharPackageContents", dr[8]);
                            sqlParam[9] = new SqlParameter("@varcharPosterDimensions", dr[9]);
                            sqlParam[10] = new SqlParameter("@varcharPackagingInformation", dr[10]);
                            sqlParam[11] = new SqlParameter("@intShippingDuration", dr[11]);
                            sqlParam[12] = new SqlParameter("@varcharVariant", dr[12]);
                            sqlParam[13] = new SqlParameter("@varcharColor", dr[13]);
                            sqlParam[14] = new SqlParameter("@varcharKeywords", dr[14]);
                            sqlParam[15] = new SqlParameter("@varcharSocialName", dr[15]);
                            sqlParam[16] = new SqlParameter("@varcharPublisher", dr[16]);
                            sqlParam[17] = new SqlParameter("@varcharPaperType", dr[17]);
                            sqlParam[18] = new SqlParameter("@varcharDescription", dr[18]);
                            sqlParam[19] = new SqlParameter("@varcharCategory", dr[19]);
                            sqlParam[20] = new SqlParameter("@varcharSubCategory", dr[20]);
                            sqlParam[21] = new SqlParameter("@varcharArtistName", dr[21]);
                            sqlParam[22] = new SqlParameter("@varcharPaintingStyle", dr[22]);
                            sqlParam[23] = new SqlParameter("@varcharBlackAndWhitePoster", dr[23]);
                            sqlParam[24] = new SqlParameter("@varcharColorPoster", dr[24]);
                            sqlParam[25] = new SqlParameter("@varcharPaperFinish", dr[25]);
                            sqlParam[26] = new SqlParameter("@varcharShape", dr[26]);
                            sqlParam[27] = new SqlParameter("@varcharOrientation", dr[27]);
                            sqlParam[28] = new SqlParameter("@varcharFramed", dr[28]);
                            sqlParam[29] = new SqlParameter("@varcharFrameMaterial", dr[29]);
                            sqlParam[30] = new SqlParameter("@varcharOtherFrameDetails", dr[30]);
                            sqlParam[31] = new SqlParameter("@intwidth", dr[31]);
                            sqlParam[32] = new SqlParameter("@intHeight", dr[32]);
                            sqlParam[33] = new SqlParameter("@intPaperDepth", dr[33]);
                            sqlParam[34] = new SqlParameter("@decimalWeight", dr[34]);
                            sqlParam[35] = new SqlParameter("@varcharOtherDimensions", dr[35]);
                            sqlParam[36] = new SqlParameter("@varcharOtherFeatures", dr[36]);
                            sqlParam[37] = new SqlParameter("@varcharSupplierImage", dr[37]);
                            sqlParam[38] = new SqlParameter("@varcharImageLink", dr[38]);
                            sqlParam[39] = new SqlParameter("@varcharNote", dr[39]);
                            sqlParam[40] = new SqlParameter("@varcharWarrantySummary", dr[40]);
                            sqlParam[41] = new SqlParameter("@intCreatedModifiedBy", UserID);
                            if (SqlUtility.ExecuteCommandSpReturnVal(GetConnectionString(), "sp_pm_InsertProduct", sqlParam) > 0)
                            {
                                return;
                            }
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        sw.WriteLine(invoiceNo + "     " + orderID + "        " + invoiceDate.ToString("dd-MM-yyyy") + "        " + productID + "       " + ex.Message);
                    }
                }

                //string val = IMSLibrary.EncryptDecrypt.Encrypt("1234", "Rohan");

                //System.Threading.Thread t = new System.Threading.Thread(Run);
                //Console.WriteLine("Line 1");
                //t.Start();
                //Console.WriteLine("Line 2");
                //t.Join();
                //Console.WriteLine("Line 3");
                //Console.ReadLine();
            }
        }

        private static string GetConnectionString()
        {
            //return System.Configuration.ConfigurationManager.ConnectionStrings["ImsConnection"].ConnectionString;
            //return "Data Source=192.168.137.1;Initial Catalog=Test;User ID=admin;Password=cahead@123;";
            return "Data Source=JIGAR-PC;Initial Catalog=Test;User ID=sa;Password=sevenrays;";
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string DecimalToWords(decimal d)
        {
            //Grab a string form of your decimal value ("12.34")
            var formatted = d.ToString();

            if (formatted.Contains("."))
            {
                //If it contains a decimal point, split it into both sides of the decimal
                string[] sides = formatted.Split('.');

                //Process each side and append them with "and", "dot" or "point" etc.
                return NumberToWords(Int32.Parse(sides[0].ToString())) + " and " + NumberToWords(Int32.Parse(sides[1].ToString())) + " Paisa.";
            }
            else
            {
                //Else process as normal
                return NumberToWords(Convert.ToInt32(d));
            }
        }

        public static void InvoiceServiceTest()
        {
            //string FilePath = @"D:\Users\Rohan\Projects\IMS26July2015\SevenRays\IMS\IMS\IMSService\Patches\Generated\InvoiceDetailsPatch.xls";//Invoicing feed.xlsx";
            //int SheetType = 1;
            //string con = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + ";Extended Properties ='Excel 8.0;HDR=Yes';";
            //using (OleDbConnection OleDbCon = new OleDbConnection(con))
            //{
            //    OleDbCon.Open();
            //    OleDbCommand oleDbCommand = new OleDbCommand();
            //    if (SheetType == 1)
            //    {
            //        oleDbCommand = new OleDbCommand("Select InvoiceDate,InvoiceNo,OrderID,OrderDate,PartysName,Address,EmailID,MobileNo,VendorID,FinalStatus From [InvoiceDetailsPatch$]", OleDbCon);
            //        //oleDbCommand = new OleDbCommand("Select [Date], [Invoice No],[Party's Name], [Order ID], [Order Date], [Address], [Email ID], [Mobile No], [Product], [Rate], [Quantity], [Total], [Net Sales], [VAT], [Packaging], [Invoice Amt], [Sales Group] From [Invoicing feed$] Where [Invoice No] > 0", OleDbCon);
            //    }
            //    else if (SheetType == 2)
            //    {
            //        oleDbCommand = new OleDbCommand("Select SettlementDate,Order ID,Invoice No,Invoice Date,Product Code,Quantity,Invoice Amt,Settlement Amount,Status from [Amount Receivable$]", OleDbCon);
            //    }
            //    else if (SheetType == 3)
            //    {
            //        //oleDbCommand = new OleDbCommand("Select Product Code, Title, Size, MRP, PosterType, PosterDimensions,Dimensions in Inches, Poster Weight, Package Contents, Packaging Information, Shipping Duration, Variant, Color, Keywords," +
            //        //    "SocialName,Publisher,PaperType,Description,Category,SubCategory,ArtistName,PaintingStyle,BlackAndWhitePoster,ColorPoster,PaperFinish,Shape,Orientation,Framed,FrameMaterial,OtherFrameDetails,Width,Height,PaperDepth," +
            //        //    "Weight,OtherDimensions,OtherFeatures,SupplierImage,ImageLink,Note,WarrantSummary From MasterSheet");
            //        oleDbCommand = new OleDbCommand("Select * From [Master Sheet$]", OleDbCon);
            //    }
            //    OleDbDataReader dr = oleDbCommand.ExecuteReader();
            //    while (dr.Read())
            //    {
            //    }
            //}

            //InvoiceServiceRef.InvoiceServiceClient pxy = new InvoiceServiceRef.InvoiceServiceClient();
            //pxy.InvoiceDetailsPatch();
        }

        private static List<ProductDetails> InvoiceProductPatch(SqlConnection sqlCon)
        {
            try
            {
                List<ProductDetails> lstInvoiceProduct = new List<ProductDetails>();
                string query = "Select InvoiceDate,InvoiceNo,OrderDate,OrderID,ProductID,Quantity,Rate,PackagingAndForwarding,CST From InvoiceProductMaster ipm " +
                               "where ipm.InvoiceDate between '2016-04-01'And '2016-09-16'" +
                               "And ipm.VAT > 0 and ipm.ProductID in (select ProductID from ProductDescriptionMaster pdm where pdm.Category in (15,4,24,23,8,16,13,22,11))";

                SqlCommand cmd = new SqlCommand(query, sqlCon);
                sqlCon.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ProductDetails invoiceProduct = new ProductDetails();
                        invoiceProduct.InvoiceDate = Converter.ToDateTime(dr["InvoiceDate"]);
                        invoiceProduct.InvoiceNo = Converter.ToInt(dr["InvoiceNo"]);
                        invoiceProduct.OrderDate = Converter.ToDateTime(dr["OrderDate"]);
                        invoiceProduct.OrderID = Converter.ToString(dr["OrderID"]);
                        invoiceProduct.ProductID = Converter.ToString(dr["ProductID"]);
                        invoiceProduct.Quantity = Converter.ToInt(dr["Quantity"]);
                        invoiceProduct.Rate = Converter.ToDecimal(dr["Rate"]);
                        //invoiceProduct.PackagingAndForwarding = Converter.ToDecimal(dr["PackagingAndForwarding"]);
                        //invoiceProduct.CST = Converter.ToDecimal(dr["CST"]);
                        //invoiceProduct.TaxGroup = "VAT";
                        //if (invoiceProduct.CST > 0.00M)
                        //{
                        //    invoiceProduct.TaxGroup = "CST";
                        //}
                        lstInvoiceProduct.Add(invoiceProduct);
                    }
                }
                dr.Close();
                cmd.Dispose();
                sqlCon.Close();
                return lstInvoiceProduct;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static bool UpdateInvoiceProduct(ProductDetails productDetails, SqlConnection sqlCon, SqlTransaction transaction)
        {
            try
            {
                decimal VatCstRate = 0M;

                VatCstRate = 12.5M + 100;
                productDetails.UnitPrice = Math.Round(((productDetails.Rate / VatCstRate) * 100), 2);
                //productDetails.VAT = (productDetails.Rate - productDetails.UnitPrice) * productDetails.Quantity;

                //productDetails.TaxRate = VatCstRate - 100;
                //productDetails.NetSale = productDetails.UnitPrice * productDetails.Quantity;
                //productDetails.Total = (productDetails.Rate * productDetails.Quantity) + Math.Round(productDetails.PackagingAndForwarding, 2);

                string Query = string.Empty;
                //Query = "Update InvoiceProductMaster Set UnitPrice =" + productDetails.UnitPrice + ", Total =" + productDetails.Total + ", NetSale=" + productDetails.NetSale + ",VAT=" + productDetails.VAT + ", TaxRate=" + (VatCstRate - 100) + " ";
                Query += "Where OrderID = '" + productDetails.OrderID + "' And ProductID = '" + productDetails.ProductID + "' And InvoiceDate = '" + productDetails.InvoiceDate.Date + "' And OrderDate = '" + productDetails.OrderDate.Date + "'";
                SqlCommand cmd = new SqlCommand(Query, sqlCon, transaction);
                int Result = cmd.ExecuteNonQuery();
                if (Result <= 0)
                {
                    transaction.Rollback();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void UpdateVatForCategory12()
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            List<ProductDetails> lstPrdDet = new List<ProductDetails>();
            lstPrdDet.Clear();
            lstPrdDet.AddRange(InvoiceProductPatch(sqlCon));

            sqlCon.Open();
            SqlTransaction transaction = sqlCon.BeginTransaction();
            bool result = true;
            foreach (var item in lstPrdDet)
            {
                if (UpdateInvoiceProduct(item, sqlCon, transaction))
                {
                    continue;
                }
                else
                {
                    result = false;
                    break;
                }
            }
            if (result)
            {
                transaction.Commit();
            }
            Console.ReadLine();
        }
    }

    internal class Test : IEnumerable, IEnumerator
    {
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public object Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    internal class Abc
    {
        private string[] str = new string[5] { "One", "Two", "Three", "Four", "Five" };

        public void A()
        {
            IEnumerator eStr = str.GetEnumerator();
            while (eStr.MoveNext())
            {
                Console.WriteLine(eStr.Current);
            }
        }
    }

    internal class Demo : IDemo
    {
        public Demo()
        {
        }

        public void Print()
        {
            Console.WriteLine("IDemo");
        }
    }

    internal interface IDemo
    {
        void Print();
    }
}