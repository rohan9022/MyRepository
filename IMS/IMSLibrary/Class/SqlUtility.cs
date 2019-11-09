using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IMSLibrary
{
    public class SqlUtility
    {
        protected SqlUtility()
        {
        }

        private static SqlConnection GetConnection(string connectionString)
        {
            ////SqlConnection sqlCon = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionString].ConnectionString);
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            return sqlCon;
        }

        public static DataSet ExecuteQueryWithDS(string connectionString, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            return FillData(connectionString, storedProcName, procParameters);
        }

        public static DataSet ExecuteQueryWithDS(SqlConnection sqlCon, SqlTransaction sqlTrn, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            return FillData(sqlCon, sqlTrn, storedProcName, procParameters);
        }

        public static DataTable ExecuteQueryWithDT(SqlConnection sqlCon, SqlTransaction sqlTrn, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            return FillData(sqlCon, sqlTrn, storedProcName, procParameters).Tables[0];
        }

        public static DataTable ExecuteQueryWithDT(string connectionString, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            return FillData(connectionString, storedProcName, procParameters).Tables[0];
        }

        private static DataSet FillData(string connectionString, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            DataSet ds = new DataSet();
            using (SqlConnection cn = GetConnection(connectionString))
            {
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
            }
            return ds;
        }

        private static DataSet FillData(SqlConnection sqlCon, SqlTransaction sqlTrn, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = sqlCon.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;
            cmd.Transaction = sqlTrn;
            // assign parameters passed in to the command
            foreach (var procParameter in procParameters)
            {
                cmd.Parameters.Add(procParameter.Value);
            }
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(ds);
            }
            return ds;
        }

        public static int ExecuteCommand(string connectionString, string storedProcName)
        {
            int rc;
            using (SqlConnection cn = GetConnection(connectionString))
            {
                // create a SQL command to execute the stored procedure
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    rc = cmd.ExecuteNonQuery();
                }
            }
            return rc;
        }

        public static int ExecuteCommand(SqlConnection sqlCon, SqlTransaction sqlTrn, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            // create a SQL command to execute the stored procedure
            SqlCommand cmd = sqlCon.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;
            cmd.Transaction = sqlTrn;
            // assign parameters passed in to the command
            foreach (var procParameter in procParameters)
            {
                cmd.Parameters.Add(procParameter.Value);
            }
            return cmd.ExecuteNonQuery();
        }

        public static int ExecuteCommand(string connectionString, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            int rc;
            using (SqlConnection cn = GetConnection(connectionString))
            {
                // create a SQL command to execute the stored procedure
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    rc = cmd.ExecuteNonQuery();
                }
            }
            return rc;
        }

        public static int ExecuteCommand(SqlConnection sqlCon, SqlTransaction sqlTrn, string storedProcName)
        {
            // create a SQL command to execute the stored procedure
            SqlCommand cmd = sqlCon.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;
            cmd.Transaction = sqlTrn;
            return cmd.ExecuteNonQuery();
        }

        public static int ExecuteCommandSpReturnVal(SqlConnection sqlCon, SqlTransaction sqlTrn, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            // create a SQL command to execute the stored procedure
            SqlCommand cmd = sqlCon.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storedProcName;
            cmd.Transaction = sqlTrn;
            SqlParameter sqlReturnParam = new SqlParameter("@return", System.Data.SqlDbType.Int);
            sqlReturnParam.Direction = System.Data.ParameterDirection.ReturnValue;
            procParameters.Add(procParameters.Count, sqlReturnParam);
            // assign parameters passed in to the command
            foreach (var procParameter in procParameters)
            {
                cmd.Parameters.Add(procParameter.Value);
            }
            cmd.ExecuteNonQuery();
            return System.Convert.ToInt32(procParameters[procParameters.Count - 1].Value);
        }

        public static int ExecuteCommandSpReturnVal(string connectionString, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            int returnValue = 0;
            using (SqlConnection cn = GetConnection(connectionString))
            {
                // create a SQL command to execute the stored procedure
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    SqlParameter sqlReturnParam = new SqlParameter("@return", System.Data.SqlDbType.Int);
                    sqlReturnParam.Direction = System.Data.ParameterDirection.ReturnValue;
                    procParameters.Add(procParameters.Count, sqlReturnParam);
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    cmd.ExecuteNonQuery();
                    returnValue = System.Convert.ToInt32(procParameters[procParameters.Count - 1].Value);
                }
            }
            return returnValue;
        }

        public static decimal ExecuteCommandSpReturnValDec(string connectionString, string storedProcName, Dictionary<int, SqlParameter> procParameters)
        {
            decimal returnValue = 0;
            using (SqlConnection cn = GetConnection(connectionString))
            {
                // create a SQL command to execute the stored procedure
                using (SqlCommand cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = storedProcName;
                    SqlParameter sqlReturnParam = new SqlParameter("@return", System.Data.SqlDbType.Decimal);
                    sqlReturnParam.Direction = System.Data.ParameterDirection.ReturnValue;
                    procParameters.Add(procParameters.Count, sqlReturnParam);
                    // assign parameters passed in to the command
                    foreach (var procParameter in procParameters)
                    {
                        cmd.Parameters.Add(procParameter.Value);
                    }
                    cmd.ExecuteNonQuery();
                    returnValue = System.Convert.ToDecimal(procParameters[procParameters.Count - 1].Value);
                }
            }
            return returnValue;
        }
    }
}