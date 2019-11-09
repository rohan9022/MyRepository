using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class InvoicePatch
    {
        #region ==Invoice==
        string InvoiceDate = string.Empty;//new DateTime();
        int InvoiceNo = 0;
        string OrderID = string.Empty;
        string OrderDate = string.Empty;//new DateTime();
        string ProductID = string.Empty;
        string PartysName = string.Empty;
        string Address = string.Empty;
        string EmailID = string.Empty;
        string MobNo = string.Empty;
        int Quantity = 0;
        decimal Rate = 0M;
        decimal UnitPrice = 0M;
        decimal NetSale = 0M;
        decimal VAT = 0M;
        decimal CST = 0M;
        decimal Packaging = 0M;
        decimal Total = 0M;
        int VendorID = 0;
        string SettlementDate = string.Empty;//new DateTime();
        decimal SettlementAmount = 0M;
        int Status = 0;
        string CreationDate = string.Empty;//new DateTime();
        int CreatedBy = 0;
        string ModifiedDate = string.Empty;//new DateTime();
        int ModifiedBy = 0;
        string ReturnedDate = string.Empty;//new DateTime(); 
        #endregion

        public InvoicePatch()
        {
            lstInvoiceMaster = new List<InvoiceMaster>();
        }
        StreamWriter sw;
        private string GetConnectionString()
        {
            //return System.Configuration.ConfigurationSettings.AppSettings["ImsConnection"].ToString();
            return "data source=localhost;initial catalog=test;user id=sa;password=sevenrays;";
            //return "Data Source=localhost;Initial Catalog=Test;User ID=sa;Password=password@123;";
        }
        List<InvoiceMaster> lstInvoiceMaster;
        public void UpdateInvoice()
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlCommand cmd = null;
            sw = new StreamWriter(@"D:\ROHAN\IMS26July2015\SevenRays\IMS\IMS\TestProject\UpdateInvoice.txt");
            cmd = new SqlCommand("Select * From InvoiceMaster Order By InvoiceNo", sqlCon);

            try
            {
                string PrevInvoiceDate = string.Empty;
                int PrevInvoiceNo = 0;
                string PrevOrderID = string.Empty;
                string PrevOrderDate = string.Empty;

                sqlCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstInvoiceMaster.Add(AddValues(dr));
                        }
                    }
                }

                foreach (InvoiceMaster item in lstInvoiceMaster)
                {
                    if (item.InvoiceDate != PrevInvoiceDate || item.InvoiceNo != PrevInvoiceNo || item.OrderID != PrevOrderID || item.OrderDate != PrevOrderDate)
                    {
                        UpdateInvoiceDetails(item, sqlCon);
                    }

                    UpdateInvoiceProductDetails(item, sqlCon);

                    PrevInvoiceDate = item.InvoiceDate;
                    PrevInvoiceNo = item.InvoiceNo;
                    PrevOrderID = item.OrderID;
                    PrevOrderDate = item.OrderDate;
                }

                //Clear();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                cmd.Dispose();
                cmd = null;
                sqlCon.Close();
            }
        }

        void Clear()
        {
            InvoiceDate = string.Empty;//new DateTime();
            InvoiceNo = 0;
            OrderID = string.Empty;
            OrderDate = string.Empty;//new DateTime();
            ProductID = string.Empty;
            PartysName = string.Empty;
            Address = string.Empty;
            EmailID = string.Empty;
            MobNo = string.Empty;
            Quantity = 0;
            Rate = 0M;
            UnitPrice = 0M;
            NetSale = 0M;
            VAT = 0M;
            CST = 0M;
            Packaging = 0M;
            Total = 0M;
            VendorID = 0;
            SettlementDate = string.Empty;//new DateTime();
            SettlementAmount = 0M;
            Status = 0;
            CreationDate = string.Empty;//new DateTime();
            CreatedBy = 0;
            ModifiedDate = string.Empty;//new DateTime();
            ModifiedBy = 0;
            ReturnedDate = string.Empty;//new DateTime();
        }

        InvoiceMaster AddValues(SqlDataReader dr)
        {
            InvoiceMaster objInv = new InvoiceMaster();
            objInv.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.InvoiceNo = Convert.ToInt32(dr["InvoiceNo"]);
            objInv.OrderID = dr["OrderID"].ToString();
            objInv.OrderDate = Convert.ToDateTime(dr["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.ProductID = dr["ProductID"].ToString();
            objInv.PartysName = dr["PartysName"].ToString().Replace("'", "''");
            objInv.Address = dr["Address"].ToString().Replace("'", "''");
            objInv.EmailID = dr["EmailID"].ToString().Replace("'", "''");
            objInv.MobNo = dr["MobileNo"].ToString();
            objInv.Quantity = Convert.ToInt32(dr["Quantity"]);
            objInv.Rate = Convert.ToDecimal(dr["Rate"]);
            objInv.UnitPrice = Convert.ToDecimal(dr["UnitPrice"]);
            objInv.NetSale = Convert.ToDecimal(dr["NetSale"]);
            objInv.VAT = Convert.ToDecimal(dr["VAT"]);
            objInv.CST = Convert.ToDecimal(dr["CST"]);
            objInv.Packaging = Convert.ToDecimal(dr["PackagingAndForwarding"]);
            objInv.Total = Convert.ToDecimal(dr["Total"]);
            objInv.VendorID = Convert.ToInt32(dr["VendorID"]);
            objInv.SettlementDate = Convert.ToDateTime(dr["SettlementDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.SettlementAmount = Convert.ToDecimal(dr["SettlementAmount"]);
            objInv.Status = Convert.ToInt32(dr["Status"]);
            objInv.CreationDate = Convert.ToDateTime(dr["CreationDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.CreatedBy = Convert.ToInt32(dr["CreatedBy"]);
            objInv.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]);
            objInv.ReturnedDate = Convert.ToDateTime(dr["ReturnedDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            return objInv;
        }

        InvoiceMaster AddValuesForInvoiceDetails(SqlDataReader dr)
        {
            InvoiceMaster objInv = new InvoiceMaster();
            objInv.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.InvoiceNo = Convert.ToInt32(dr["InvoiceNo"]);
            objInv.OrderID = dr["OrderID"].ToString();
            objInv.OrderDate = Convert.ToDateTime(dr["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.PartysName = dr["PartysName"].ToString().Replace("'", "''");
            objInv.Address = dr["Address"].ToString().Replace("'", "''");
            objInv.EmailID = dr["EmailID"].ToString().Replace("'", "''");
            objInv.MobNo = dr["MobileNo"].ToString();
            objInv.VendorID = Convert.ToInt32(dr["VendorID"]);
            objInv.Status = Convert.ToInt32(dr["FinalStatus"]);
            objInv.CreationDate = Convert.ToDateTime(dr["CreationDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.CreatedBy = Convert.ToInt32(dr["CreatedBy"]);
            objInv.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]);
            return objInv;
        }

        decimal InvoiceAmount = 0M;
        int FinalStatus = 0;
        void UpdateInvoiceDetails(InvoiceMaster item, SqlConnection sqlCon)
        {
            SettlementAmount = lstInvoiceMaster.Where(p => p.InvoiceNo == item.InvoiceNo && p.InvoiceDate == item.InvoiceDate && p.OrderID == item.OrderID && p.OrderDate == item.OrderDate).Sum(p => p.SettlementAmount);
            InvoiceAmount = lstInvoiceMaster.Where(p => p.InvoiceNo == item.InvoiceNo && p.InvoiceDate == item.InvoiceDate && p.OrderID == item.OrderID && p.OrderDate == item.OrderDate).Sum(p => p.Total);
            if (lstInvoiceMaster.Where(p => p.InvoiceNo == item.InvoiceNo).ToList().Where(p => p.Status == 1 || p.Status == 8).Count() > 0)
            {
                FinalStatus = 1;
            }
            else
            {
                FinalStatus = 5;
            }

            SqlCommand cmd = new SqlCommand("Insert Into InvoiceDetailsMaster " +
             "( InvoiceDate, InvoiceNo, OrderID, OrderDate, PartysName, [Address], EmailID, MobileNo, VendorID, InvoiceAmount, SettlementAmount, FinalStatus, CreationDate, CreatedBy, ModifiedDate, ModifiedBy) " +
      "Values ('" + item.InvoiceDate + "'," + item.InvoiceNo + ",'" + item.OrderID + "','" + item.OrderDate + "','" + item.PartysName + "','" + item.Address + "','" + item.EmailID + "','" + item.MobNo + "'," + item.VendorID + "," + InvoiceAmount + "," + SettlementAmount + "," + FinalStatus + ",'" + item.CreationDate + "'," + item.CreatedBy + ",'" + item.ModifiedDate + "'," + item.ModifiedBy + ")", sqlCon);
            int cnt = cmd.ExecuteNonQuery();
            if (cnt != 1)
            {
                Console.WriteLine("Invoice Error");
                sw.WriteLine("Invoice Details Error," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID);
                cmd.Dispose();
                return;
            }
            Console.WriteLine("Invoice Success");
            sw.WriteLine("Invoice Details Success," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID);
            cmd.Dispose();
        }

        void UpdateInvoiceProductDetails(InvoiceMaster item, SqlConnection sqlCon)
        {
            SqlCommand cmd = new SqlCommand("Insert Into InvoiceProductMaster " +
             "( InvoiceDate, InvoiceNo, OrderID, OrderDate, ProductID, Quantity, Rate, UnitPrice, NetSale, VAT, CST, PackagingAndForwarding, Total, Status, CreationDate, CreatedBy, ModifiedDate, ModifiedBy) " +
      "Values ('" + item.InvoiceDate + "'," + item.InvoiceNo + ",'" + item.OrderID + "','" + item.OrderDate + "','" + item.ProductID + "'," + item.Quantity + "," + item.Rate + "," + item.UnitPrice + "," + item.NetSale + "," + item.VAT + "," + item.CST + "," + item.Packaging + "," + item.Total + "," + item.Status + ",'" + item.CreationDate + "'," + item.CreatedBy + ",'" + item.ModifiedDate + "'," + item.ModifiedBy + ")", sqlCon);
            int cnt = cmd.ExecuteNonQuery();
            if (cnt != 1)
            {
                Console.WriteLine("Invoice Product Error");
                sw.WriteLine("Invoice Product Error," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID);
                cmd.Dispose();
                return;
            }
            Console.WriteLine("Invoice Product Success");
            sw.WriteLine("Invoice Product Error," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID);
            cmd.Dispose();
            cmd.Dispose();
        }

        public void UpdateInvoiceAmount()
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlCommand cmd = null;
            sw = new StreamWriter(@"D:\UpdateInvoiceAmount.txt");
            cmd = new SqlCommand("Select * From InvoiceDetailsMaster Order By InvoiceNo", sqlCon);

            try
            {
                sqlCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstInvoiceMaster.Add(AddValuesForInvoiceDetails(dr));
                        }
                    }
                }

                foreach (InvoiceMaster item in lstInvoiceMaster)
                {
                    cmd = new SqlCommand("Select Sum(Total) From InvoiceProductMaster Where InvoiceDate='" + item.InvoiceDate + "' And InvoiceNo=" + item.InvoiceNo + " And OrderID='" + item.OrderID + "' And OrderDate='" + item.OrderDate + "'", sqlCon);
                    decimal InvoiceAmount = Convert.ToDecimal(cmd.ExecuteScalar());
                    cmd.Dispose();

                    cmd = new SqlCommand("Update InvoiceDetailsMaster Set InvoiceAmount=" + InvoiceAmount +
                                        " Where InvoiceDate = '" + item.InvoiceDate + "' And InvoiceNo=" + item.InvoiceNo + " And OrderID = '" + item.OrderID + "' And OrderDate = '" + item.OrderDate + "'", sqlCon);
                    int cnt = cmd.ExecuteNonQuery();
                    if (cnt != 1)
                    {
                        Console.WriteLine("Invoice Error");
                        sw.WriteLine("Invoice Details Error," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID);
                        cmd.Dispose();
                        return;
                    }
                    Console.WriteLine("Invoice Success");
                    sw.WriteLine("Invoice Details Success," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID);
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
                cmd.Dispose();
                cmd = null;
                sqlCon.Close();
            }
        }
    }

    public class InvoiceMaster
    {
        #region ==Invoice==
        public string InvoiceDate = string.Empty;//new DateTime();
        public int InvoiceNo = 0;
        public string OrderID = string.Empty;
        public string OrderDate = string.Empty;//new DateTime();
        public string ProductID = string.Empty;
        public string PartysName = string.Empty;
        public string Address = string.Empty;
        public string EmailID = string.Empty;
        public string MobNo = string.Empty;
        public int Quantity = 0;
        public decimal Rate = 0M;
        public decimal UnitPrice = 0M;
        public decimal NetSale = 0M;
        public decimal VAT = 0M;
        public decimal CST = 0M;
        public decimal Packaging = 0M;
        public decimal Total = 0M;
        public int VendorID = 0;
        public string SettlementDate = string.Empty;//new DateTime();
        public decimal SettlementAmount = 0M;
        public int Status = 0;
        public string CreationDate = string.Empty;//new DateTime();
        public int CreatedBy = 0;
        public string ModifiedDate = string.Empty;//new DateTime();
        public int ModifiedBy = 0;
        public string ReturnedDate = string.Empty;//new DateTime(); 
        #endregion
    }
}
