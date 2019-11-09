using IMSLibrary.UI;
using IMS.Models;
using IMS.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace IMS.Helper
{
    public class Helper
    {
        protected Helper()
        {
        }

        public static void MoveFile(string FolderPath, string FileName, string valType)
        {
            if (!Directory.Exists(FolderPath + "\\" + valType))
            {
                Directory.CreateDirectory(FolderPath + "\\" + valType);
            }
            File.Move(FolderPath + "\\" + FileName, FolderPath + "\\" + valType + "\\" + FileName);
        }

        public static byte[] ConvertToByte(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] photo = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return photo;
        }

        public static BitmapImage ImageFromBuffer(string filePath)
        {
            Byte[] bytes = Helper.ConvertToByte(filePath);
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static bool WriteIni(string OpType, string value)
        {
            try
            {
                IniFile inif = new IniFile(Const.AppPath + "/Config.ini");
                inif.Write("Path", OpType, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ReadIni(string OpType)
        {
            try
            {
                IniFile inif = new IniFile(Const.AppPath + "/Config.ini");
                return inif.Read("Path", OpType);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static byte[] ImageToByte(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static System.Drawing.Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1479:\"switch\" statements should not have too many \"case\" clauses", Justification = "<Pending>")]
        public static void WindowNavigation(int NavigationValue, System.Windows.Controls.Frame MainRegion)
        {
            try
            {
                if (NavigationValue != Const.Home && NavigationValue != Const.Logout && NavigationValue != Const.Help
                 && NavigationValue != Const.About && NavigationValue != Const.InvoiceReport
                 && NavigationValue != Const.Calc && NavigationValue != Const.IE
                 && NavigationValue != Const.Outlook
                 && NavigationValue != Const.Music) Global.DashBoard = false;
                switch (NavigationValue)
                {
                    case Const.Home:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new Dashboard(MainRegion));
                        ////MainRegion.Navigate(new Dashboard7R(MainRegion));
                        break;

                    case Const.Logout:
                        ////    MainRegion.Navigate(new Login());
                        break;

                    case Const.About:
                        new About().ShowDialog();
                        break;

                    case Const.Help:
                        ////MainRegion.Navigate(new Dashboard(MainRegion));
                        break;

                    case Const.Exit:
                        System.Environment.Exit(0);
                        break;

                    case Const.Calc:
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("calc.exe"));
                        break;

                    case Const.Lock:
                        new ScreenLock().ShowDialog();
                        return;

                    case Const.IE:
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("iexplore.exe"));
                        break;
                    ////case Const.Skype:
                    ////    Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Skype\Phone");
                    ////    string skypeLocation = regKey.GetValue("SkypePath").ToString();
                    ////    System.Diagnostics.Process.Start(skypeLocation);
                    ////    break;
                    case Const.Skype:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new IMS.Views.CreditNoteReportView());
                        break;

                    case Const.Outlook:
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("OUTLOOK.exe"));
                        break;

                    case Const.Music:
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("wmplayer.exe"));
                        break;

                    case Const.UserMaster:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new User(Global.GlobalPageMode));
                        break;

                    case Const.Vendor:
                        MainRegion.Navigate(new Vendor(Global.GlobalPageMode));
                        break;

                    case Const.Menu:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new IMS.Views.MenuMaster());
                        break;

                    case Const.Product:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new Product(Global.GlobalPageMode));
                        break;

                    case Const.Sales:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new Sales(Global.GlobalPageMode));
                        break;

                    case Const.Lookup:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new LookupMaster(Global.GlobalPageMode));
                        break;

                    case Const.SubLookup:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new SubLookupMaster(Global.GlobalPageMode));
                        break;

                    case Const.Purchase:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new Purchase(Global.GlobalPageMode));
                        break;

                    case Const.Damage:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new Damage(Global.GlobalPageMode));
                        break;

                    case Const.Invoice:
                        if (!ChkAccess()) break;
                        ////MainRegion.Navigate(new Invoice(Global.GlobalPageMode));
                        MainRegion.Navigate(new InvoiceDetailsMasterSeparate());
                        break;

                    case Const.UploadSheet:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new UploadSheet());
                        break;

                    case Const.InvoiceReport:
                        if (!ChkAccess()) break;
                        GenReports objWin = new GenReports();
                        objWin.ShowDialog();
                        break;

                    case Const.StatuswiseInvoiceReport:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new InvoiceStatus());
                        break;

                    case Const.FinalInvoiceReport:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new FinalInvoice());
                        break;

                    case Const.StockReport:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new Stock());
                        break;

                    case Const.Category:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new CategoryMaster(Global.GlobalPageMode));
                        break;

                    case Const.SubCategory:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new SubCategoryMaster(Global.GlobalPageMode));
                        break;

                    case Const.TaxReport:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new TaxReport());
                        break;

                    case Const.SalesReport:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new SalesReport());
                        break;

                    case Const.InvoicePatch:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new Patch());
                        break;

                    case Const.BulkDeleteInvoicePatch:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new BulkDeleteInvoice());
                        break;

                    case Const.InvoiceDetailsPatch:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new InvoiceDetailsPatch());
                        break;

                    case Const.InvoiceProductPatch:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new InvoiceProductPatch());
                        break;

                    case Const.InvoiceSettlementPatch:
                        if (!ChkAccess()) break;
                        MainRegion.Navigate(new InvoiceSettlementPatch());
                        break;

                    default:
                        break;
                }
                Global.GlobalNavigationValue = NavigationValue;
            }
            catch
            {
                UIHelper.ShowErrorMessage("not found");
            }
        }

        private static bool ChkAccess()
        {
            if (Global.UserType != Const.Admin)
            {
                UIHelper.ShowErrorMessage("Access Denied");
                return false;
            }
            return true;
        }

        public static void ReadWriteFile(string content)
        {
            using (StreamWriter sw = new StreamWriter(System.Windows.Forms.Application.StartupPath + "//Temp.txt"))
            {
                sw.WriteLine(content);
            }

            Process.Start(System.Windows.Forms.Application.StartupPath + "//Temp.txt");
        }
    }
}