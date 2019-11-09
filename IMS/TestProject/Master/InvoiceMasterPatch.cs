using IMSLibrary;
using IMSLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace TestProject.Master
{
    internal class InvoiceMasterPatch
    {
        public InvoiceMasterPatch()
        {
        }

        private string GetConnectionString()
        {
            ////return System.Configuration.ConfigurationSettings.AppSettings["ImsConnection"].ToString();
            return "Data Source=localhost;Initial Catalog=Test;User ID=sa;Password=sevenrays;";
        }

        public void UpdateInvoiceMaster()
        {
            string FilePath = @"D:\DONT DELETE\Desktop\Invoietoday\invoice (188).xlsx";
            string con = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + ";Extended Properties ='Excel 8.0;HDR=Yes';";
            using (OleDbConnection OleDbCon = new OleDbConnection(con))
            {
                OleDbCon.Open();
                OleDbCommand oleDbCommand = new OleDbCommand("Select * From [Invoicing$]", OleDbCon);
                OleDbDataReader dr = oleDbCommand.ExecuteReader();

                SqlConnection sqlCon = new SqlConnection(GetConnectionString());

                while (dr.Read())
                {
                    int invoiceNo = 0;
                    string orderID = string.Empty;
                    DateTime invoiceDate = new DateTime();
                    string productID = "";
                    string ErrorMsg = string.Empty;
                    int Result = 0;

                    int quantity = 0;
                    decimal rate = 0M, unitPrice = 0M, netSales = 0M, total = 0.00M;
                    decimal CST = 0.00M, VAT = 0.00M, ShippingHandling = 0.00M;
                    DateTime orderDate = new DateTime();
                    string TaxGroup = string.Empty, PartyName, Address, EmailID, MobNo, Vendor;

                    orderID = Converter.ToString(dr["Order ID"].ToString());
                    if (string.IsNullOrEmpty(orderID)) continue;
                    productID = Converter.ToString(dr["SKU"]).ToUpper();
                    quantity = Converter.ToInt(dr["Quantity"]);
                    rate = Converter.ToDecimal(dr["Rate"]);
                    //TaxGroup = Converter.ToString(dr["Tax Group"]);
                    PartyName = Converter.ToString(dr["Party's Name"]);
                    Address = Converter.ToString(dr["Address"]);
                    EmailID = Converter.ToString(dr["Email ID"]);
                    MobNo = Converter.ToString(dr["Mobile No"]);
                    ShippingHandling = Math.Round((Converter.ToDecimal(dr["Shipping & Handling"])), 2);
                    //Vendor = Converter.ToString(dr["Sales Group"]);

                    netSales = unitPrice * quantity;
                    total = (rate * quantity) + Math.Round((Converter.ToDecimal(dr["Shipping & Handling"])), 2);

                    string query = "Update InvoiceMaster Set";
                    if (!string.IsNullOrEmpty(PartyName)) query += " PartyName = '" + PartyName.Replace("'", "''") + "'";
                    if (!string.IsNullOrEmpty(Address)) query += " Address = '" + Address.Replace("'", "''") + "'";
                    if (!string.IsNullOrEmpty(EmailID)) query += " EmailID = '" + PartyName + "'";
                    if (!string.IsNullOrEmpty(MobNo)) query += " MobilNo = '" + MobNo + "'";
                    if (quantity > 0) query += " Quantity = " + quantity;
                    if (ShippingHandling > 0) query += " PackagingAndForwarding = " + ShippingHandling;

                    int DbQuantity = 0;
                    decimal DbCST = 0M;
                    decimal DbShippingHandling = 0M;

                    SqlCommand cmd = null;
                    if (rate > 0 || quantity > 0)
                    {
                        cmd = new SqlCommand("Select CST, Quantity From InvoiceMaster Where OrderID = '" + orderID + "' ProductID = '" + productID + "'");

                        using (SqlDataReader dr1 = cmd.ExecuteReader())
                        {
                            if (dr1.HasRows)
                            {
                                while (dr1.Read())
                                {
                                    DbCST = Convert.ToDecimal(dr["CST"]);
                                    DbQuantity = Convert.ToInt32(dr["Quantity"]);

                                    if (DbCST > 0)
                                    {
                                        TaxGroup = "CST";
                                    }
                                    break;
                                }
                            }
                        }

                        if (quantity == 0)
                        {
                            quantity = DbQuantity;
                        }

                        decimal VatCstRate = 0M;
                        if (TaxGroup == "CST")
                        {
                            VatCstRate = FindVatCstRate(productID, false) + 100;
                            unitPrice = Math.Round(((rate / VatCstRate) * 100), 2);
                            CST = (rate - unitPrice) * quantity;
                        }
                        else
                        {
                            VatCstRate = FindVatCstRate(productID, true) + 100;
                            unitPrice = Math.Round(((rate / VatCstRate) * 100), 2);
                            VAT = (rate - unitPrice) * quantity;
                        }

                        netSales = unitPrice * quantity;

                        if (ShippingHandling == 0)
                        {
                            ShippingHandling = DbShippingHandling;
                        }

                        total = (rate * quantity) + ShippingHandling;

                        query += " Rate = " + rate + ", UnitPrice = " + unitPrice + ",NetSale=" + netSales + ", VAT=" + VAT + ", CST=" + CST + ", Total=" + total;
                    }

                    query += " Where ProductID = '" + productID + "' And OrderID = '" + orderID + "'";

                    cmd = new SqlCommand(query, sqlCon);
                }
            }
        }

        private decimal FindVatCstRate(string ProductID, bool IsVat)
        {
            try
            {
                decimal Rate = 0M;
                Dictionary<int, SqlParameter> sqlParam = new Dictionary<int, SqlParameter>();
                sqlParam[0] = new SqlParameter("@nvarcharProductID", ProductID);
                if (IsVat)
                {
                    Rate = 5;
                    sqlParam[1] = new SqlParameter("@intIsVat", 1);
                }
                else
                {
                    Rate = 2;
                    sqlParam[1] = new SqlParameter("@intIsVat", 2);
                }
                decimal result = 0M;
                DataTable dt = SqlUtility.ExecuteQueryWithDT(GetConnectionString(), "sp_FindVatCstRate", sqlParam);
                foreach (DataRow dr in dt.Rows)
                {
                    result = Converter.ToDecimal(dr["Rate"]);
                    break;
                }
                if (result > 0)
                {
                    return result;
                }
                return Rate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}