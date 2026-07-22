using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic
{
    public static class clsData
    {
        private static string _cnnOracle = System.Configuration.ConfigurationManager.AppSettings["ConnectDB"];
        public static void ExecuteNonQuery(string storeName, params OracleParameter[] param)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            try
            {                
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = storeName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(param.ToArray());
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch(Exception ex) { cmd.Connection.Close(); }
        }
        public static DataSet ExecuteDataset(string storeName, params OracleParameter[] param)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = storeName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(param.ToArray());
                cmd.ExecuteReader();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
                cmd.Connection.Close();
                return ds;
            }
            catch { cmd.Connection.Close(); return ds; }
        }
        public static object ExecuteScalar(string storeName, params OracleParameter[] param)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = storeName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(param.ToArray());
                cmd.Connection.Close();
                return cmd.ExecuteScalar();
            }
            catch { cmd.Connection.Close(); return new object(); }
        }
        public static Int64 ExecuteReturnID(string storeName, params OracleParameter[] param)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = storeName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(param.ToArray());
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                return (Int64)param[param.Length - 1].Value;
            }
            catch { cmd.Connection.Close(); return new Int64(); }
        }

        

        
    }
    
}
