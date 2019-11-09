using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace TestProject.Master
{
    class InvoiceProductPatch
    {
        StreamWriter sw;
        List<InvoiceProductMaster> lstInvoiceProductMaster;
        private string GetConnectionString()
        {
            //return System.Configuration.ConfigurationSettings.AppSettings["ImsConnection"].ToString();
            return "Data Source=localhost;Initial Catalog=Test;User ID=sa;Password=sevenrays;";
            //return "Data Source=localhost;Initial Catalog=Test;User ID=sa;Password=password@123;";
        }

        public InvoiceProductPatch()
        {
            lstInvoiceProductMaster = new List<InvoiceProductMaster>();
        }

        InvoiceProductMaster AddValues(SqlDataReader dr)
        {
            InvoiceProductMaster objInv = new InvoiceProductMaster();
            objInv.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.InvoiceNo = Convert.ToInt32(dr["InvoiceNo"]);
            objInv.OrderID = dr["OrderID"].ToString();
            objInv.OrderDate = Convert.ToDateTime(dr["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.ProductID = dr["ProductID"].ToString();
            objInv.Quantity = Convert.ToInt32(dr["Quantity"]);
            objInv.Rate = Convert.ToDecimal(dr["Rate"]);
            objInv.UnitPrice = Convert.ToDecimal(dr["UnitPrice"]);
            objInv.NetSale = Convert.ToDecimal(dr["NetSale"]);
            objInv.VAT = Convert.ToDecimal(dr["VAT"]);
            objInv.CST = Convert.ToDecimal(dr["CST"]);
            objInv.Packaging = Convert.ToDecimal(dr["PackagingAndForwarding"]);
            objInv.Total = Convert.ToDecimal(dr["Total"]);
            objInv.SettlementAmount = Convert.ToDecimal(dr["SettlementAmount"]);
            objInv.Status = Convert.ToInt32(dr["Status"]);
            objInv.CreationDate = Convert.ToDateTime(dr["CreationDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.CreatedBy = Convert.ToInt32(dr["CreatedBy"]);
            objInv.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
            objInv.ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]);
            objInv.TaxRate = Convert.ToDecimal(dr["TaxRate"]);
            return objInv;
        }

        public void UpdateTaxRate()
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlCommand cmd = null;
            sw = new StreamWriter(@"D:\UpdateTaxRate.txt");
            //cmd = new SqlCommand("Select * from InvoiceProductMaster IPM Where UnitPrice > 0 And (Round((((IPM.Rate/IPM.UnitPrice)*100)-100),2) % 5) = 0", sqlCon);
            cmd = new SqlCommand("Select * from InvoiceProductMaster IPM Where UnitPrice > 0", sqlCon);

            try
            {
                sqlCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstInvoiceProductMaster.Add(AddValues(dr));
                        }
                    }
                }

                foreach (InvoiceProductMaster item in lstInvoiceProductMaster)
                {
                    decimal TaxRate = Convert.ToDecimal(Math.Round((((item.Rate / item.UnitPrice) * 100) - 100), 2));
                    string tr = TaxRate.ToString("0.00").Replace(".",string.Empty);
                    int val = Convert.ToInt32(tr) % 5;
                    if (val == 1) TaxRate = TaxRate - 0.01M;
                    if (val == 2) TaxRate = TaxRate - 0.02M;
                    if (val == 3) TaxRate = TaxRate + 0.02M;
                    if (val == 4) TaxRate = TaxRate + 0.01M;
                    cmd = new SqlCommand("Update InvoiceProductMaster Set TaxRate=" + TaxRate +
                                        " Where InvoiceDate = '" + item.InvoiceDate + "' And InvoiceNo=" + item.InvoiceNo + " And OrderID = '" + item.OrderID + "' And OrderDate = '" + item.OrderDate + "' And ProductID = '" + item.ProductID + "' And Status =" + item.Status + " And CreationDate ='" + item.CreationDate + "'", sqlCon);
                    int cnt = cmd.ExecuteNonQuery();
                    if (cnt != 1)
                    {
                        Console.WriteLine("Invoice Error");
                        sw.WriteLine("Invoice Details Error," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID + "," + item.CreationDate);
                        cmd.Dispose();
                        return;
                    }
                    Console.WriteLine("Invoice Success");
                    sw.WriteLine("Invoice Details Success," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID + "," + item.CreationDate);
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

    public class InvoiceProductMaster
    {
        #region ==Invoice==
        public string InvoiceDate = string.Empty;//new DateTime();
        public int InvoiceNo = 0;
        public string OrderID = string.Empty;
        public string OrderDate = string.Empty;//new DateTime();
        public string ProductID = string.Empty;
        public int Quantity = 0;
        public decimal Rate = 0M;
        public decimal UnitPrice = 0M;
        public decimal NetSale = 0M;
        public decimal VAT = 0M;
        public decimal CST = 0M;
        public decimal Packaging = 0M;
        public decimal Total = 0M;
        public decimal SettlementAmount = 0M;
        public int Status = 0;
        public string CreationDate = string.Empty;//new DateTime();
        public int CreatedBy = 0;
        public string ModifiedDate = string.Empty;//new DateTime();
        public int ModifiedBy = 0;
        public decimal TaxRate = 0M;
        #endregion
    }
}
