using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class CreditNotePatch
    {
        public CreditNotePatch()
        {

        }

        private string GetConnectionString()
        {
            //return System.Configuration.ConfigurationSettings.AppSettings["ImsConnection"].ToString();
            return "Data Source=JIGAR1;Initial Catalog=Test;User ID=sa;Password=sevenrays;";
        }

        public void UpdateCrNote()
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            List<string> lstDate = new List<string>();
            SqlCommand cmd = null;
            StreamWriter sw = new StreamWriter(@"D:\ROHAN\IMS26July2015\SevenRays\IMS\IMS\TestProject\UpdateCrNote.txt");
            cmd = new SqlCommand("Select * from CreditNote order by CreationDate", sqlCon);

            try
            {
                sqlCon.Open();
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        lstDate.Add(Convert.ToDateTime(dataReader["CreationDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }
                }
                dataReader.Close();

                int SrNo = 0;
                foreach (var CrDate in lstDate)
                {
                    Console.WriteLine();
                    Console.WriteLine(CrDate);
                    ++SrNo;
                    string CrNoteNo = "SR-C-" + SrNo.ToString().PadLeft(10, '0');
                    cmd = new SqlCommand("Update CreditNote Set CrNoteNo='" + CrNoteNo + "' Where CreationDate='" + CrDate + "'", sqlCon);
                    int cnt = cmd.ExecuteNonQuery();
                    if (cnt != 1)
                    {
                        Console.WriteLine("Error");
                        sw.WriteLine("Error  -  " + CrDate + "  -  " + CrNoteNo);
                        continue;
                    }
                    Console.WriteLine("Success");
                    sw.WriteLine("Success  -  " + CrDate + "  -  " + CrNoteNo);
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

        public void DeleteDuplicateCrNote()
        {
            SqlConnection sqlCon = new SqlConnection(GetConnectionString());
            List<InvoiceMaster> lstInvoiceMaster = new List<InvoiceMaster>();
            SqlCommand cmd = null;
            StreamWriter sw = new StreamWriter(@"D:\ROHAN\IMS26July2015\SevenRays\IMS\IMS\TestProject\DeleteDuplicateCrNote.txt");
            cmd = new SqlCommand("Select ProductID, OrderID, OrderDate From CreditNote Group By ProductID, OrderID, OrderDate Having count(*) > 1", sqlCon);

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
                                ProductID = Convert.ToString(dr["ProductID"]),
                                OrderID = Convert.ToString(dr["OrderID"]),
                                OrderDate = Convert.ToDateTime(dr["OrderDate"]).ToString("yyyy-MM-dd HH:mm:ss.fff")
                            });
                        }
                    }
                }

                foreach (InvoiceMaster item in lstInvoiceMaster)
                {
                    cmd = new SqlCommand("Delete From CreditNote " +
                                         "Where ProductID = '" + item.ProductID + "' And OrderID ='" + item.OrderID + "' And OrderDate = '" + item.OrderDate + "' " +
                                         "And CreationDate != (Select Max(CreationDate) From CreditNote Where ProductID = '" + item.ProductID + "' And OrderID ='" + item.OrderID + "' And OrderDate = '" + item.OrderDate + "')", sqlCon);
                    int cnt = cmd.ExecuteNonQuery();
                    if (cnt != 1)
                    {
                        Console.WriteLine("CrNote Error");
                        sw.WriteLine("Error  -  " + item.ProductID + "  -  " + item.OrderID + "    -   " + item.OrderDate);
                        cmd.Dispose();
                        continue;
                    }
                    Console.WriteLine("CrNote Success");
                    sw.WriteLine("Success  -  " + item.ProductID + "  -  " + item.OrderID + "    -   " + item.OrderDate);
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
}
