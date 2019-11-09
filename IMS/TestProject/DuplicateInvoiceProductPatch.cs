using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace TestProject
{
    class DuplicateInvoiceProductPatch
    {
        List<InvoiceMaster> lstInvoiceMaster;
        public DuplicateInvoiceProductPatch()
        {
            lstInvoiceMaster = new List<InvoiceMaster>();
        }

        private string GetConnectionString()
        {
            //return System.Configuration.ConfigurationSettings.AppSettings["ImsConnection"].ToString();
            return "Data Source=JIGAR1;Initial Catalog=Test;User ID=sa;Password=sevenrays;";
        }

        public void UpdateInvoiceProductMasterStatus()
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            SqlCommand cmd = null;
            StreamWriter sw = new StreamWriter(@"D:\ROHAN\IMS26July2015\SevenRays\IMS\IMS\TestProject\DuplicateInvoiceProductPatch.txt");
            cmd = new SqlCommand("Select InvoiceNo, OrderID, ProductID From InvoiceProductMaster Group By InvoiceNo, OrderID, ProductID Having count(*) > 1", sqlCon);
            try
            {
                sqlCon.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lstInvoiceMaster.Add(new InvoiceMaster
                            {
                                InvoiceNo = Convert.ToInt32(dr["InvoiceNo"]),
                                OrderID = Convert.ToString(dr["OrderID"]),
                                ProductID = Convert.ToString(dr["ProductID"])
                            });
                        }
                    }
                }

                foreach (InvoiceMaster item in lstInvoiceMaster)
                {
                    cmd = new SqlCommand("Update InvoiceProductMaster Set Status=9 " +
                                         "Where InvoiceNo = " + item.InvoiceNo + " And OrderID ='" + item.OrderID + "' And ProductID = '" + item.ProductID + "' " +
                                         "And InvoiceDate = (Select Min(InvoiceDate) From InvoiceProductMaster Where InvoiceNo = " + item.InvoiceNo + " And OrderID ='" + item.OrderID + "' And ProductID = '" + item.ProductID + "')", sqlCon);
                    int cnt = cmd.ExecuteNonQuery();
                    if (cnt != 1)
                    {
                        Console.WriteLine("Invoice Product Error");
                        sw.WriteLine("Invoice Product Error," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID);
                        cmd.Dispose();
                        continue;
                    }
                    Console.WriteLine("Invoice Product Success");
                    sw.WriteLine("Invoice Product Error," + item.InvoiceDate + "," + item.InvoiceNo + "," + item.OrderID + "," + item.OrderDate + "," + item.ProductID);
                    cmd.Dispose();
                }
            }
            catch (SqlException ex)
            {
                throw;
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
            }
        }
    }
}
