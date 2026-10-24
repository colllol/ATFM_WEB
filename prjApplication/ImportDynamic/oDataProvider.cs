using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.DataAccess.Client;
using System.Data;
using System.Reflection;
using System.Configuration;

namespace prjApplication.ImportData
{
    public class oDataProvider
    {
        private static readonly string _cnnOracle = ConfigurationManager.AppSettings["OracleConnectionString"];
        public int ExecuteNonQueryProduce(string produceName, params OracleParameter[] papa)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            int kq = 1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(papa.ToArray());
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex) { cmd.Connection.Close(); kq = -1; }
            return kq;
        }
        public int ExecuteNonQuery(string packageName, string produceName, object obj)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            int kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                List<OracleParameter> lisParam = GetOracleParameter(packageName, produceName, obj);
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                cmd.ExecuteNonQuery();
                foreach (var item in lisParam)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        try
                        {
                            kq = int.Parse(item.Value.ToString());
                        }
                        catch 
                        {
                        }
                    }
                }
                cmd.Connection.Close();
            }
            catch (Exception ex) { cmd.Connection.Close(); }
            return kq;
        }
        public int ExecuteNonQuery(string produceName, params OracleParameter[] papa)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            int kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(papa.ToArray());
                cmd.ExecuteNonQuery();
                foreach (var item in papa)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        try
                        {
                            kq = int.Parse(item.Value.ToString());
                        }
                        catch { }
                    }
                }
                cmd.Connection.Close();
            }
            catch (Exception ex) { cmd.Connection.Close(); }
            return kq;
        }
        public int ExecuteNonQuery(string packageName, string produceName, params OracleParameter[] papa)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            int kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{packageName}.{produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(papa.ToArray());
                cmd.ExecuteNonQuery();
                foreach (var item in papa)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        try
                        {
                            kq = int.Parse( item.Value.ToString());
                        }
                        catch { }
                    }
                }
                cmd.Connection.Close();
            }
            catch (Exception ex) { cmd.Connection.Close(); }
            return kq;
        }
        public DataSet ExecuteDatase(string packageName, string produceName, object obj)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                List<OracleParameter> lisParam = GetOracleParameter(packageName, produceName, obj);
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                cmd.ExecuteReader();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
                cmd.Connection.Close();
                return ds;
            }
            catch { cmd.Connection.Close(); return ds; }
        }
        public DataSet ExecuteDatase(string cmdText)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ cmdText}";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteReader();
                List<OracleParameter> lis = GetOracleParameterOUT(cmdText);
                if (lis.Count > 0)
                    cmd.Parameters.AddRange(lis.ToArray());
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
                cmd.Connection.Close();
                return ds;
            }
            catch { cmd.Connection.Close(); return ds; }
        }
        public DataSet ExecuteDatase(string packageName, string produceName, params OracleParameter[] papa)
        {

            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(papa.ToArray());
                //cmd.ExecuteReader();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
                cmd.Connection.Close();
                return ds;
            }
            catch { cmd.Connection.Close(); return ds; }
        }
        public object ExecuteScalar(string packageName, string produceName, object obj)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            object kq = new object();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                List<OracleParameter> lisParam = GetOracleParameter(packageName, produceName, obj);
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                foreach (var item in lisParam)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        kq = item.Value;
                        break;
                    }
                }
                return kq;
            }
            catch (Exception ex) { cmd.Connection.Close(); return new object(); }
        }
        public Int64 ExecuteReturnID(string packageName, string produceName, object obj)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            Int64 kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                List<OracleParameter> lisParam = GetOracleParameter(packageName, produceName, obj);
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                foreach (var item in lisParam)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        kq = Int64.Parse(item.Value.ToString());
                        break;
                    }
                }
                return kq;
            }
            catch (Exception ex) { cmd.Connection.Close(); return -1; }
        }

        public int ExecuteNonQueryProduce(string cmdText, CommandType cmdtype)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            int kq = 1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ cmdText}";
                cmd.CommandType = cmdtype;                
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex) { cmd.Connection.Close(); kq = -1; }
            return kq;
        }
        public DataTable ExecuteDataseProcduce(string produceName, params OracleParameter[] papa)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(papa.ToArray());
                //cmd.ExecuteReader();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
                cmd.Connection.Close();
                return ds.Tables[0];
            }
            catch { cmd.Connection.Close(); return new DataTable(); }
        }
        private List<OracleParameter> SetOracleParamDefault(List<OracleParameter> paramOfDB, List<OracleParameter>parmOfSet)
        {
            foreach (var item in paramOfDB)
            {
                foreach (var i2 in parmOfSet)
                {
                    if (item.ParameterName == i2.ParameterName)
                        item.Value = i2.Value;
                }
            }
            return paramOfDB;
        }
        private List<OracleParameter> GetOracleParameter(string packageName, string produceName, object obj)
        {
            var lis = new List<OracleParameter>();
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataTable dt = new DataTable();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"select argument_name, data_type,in_out  from ALL_ARGUMENTS where package_name='{packageName}' and object_name = '{produceName}' order by position asc";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteReader();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                cmd.Connection.Close();
            }
            catch { cmd.Connection.Close(); return new List<OracleParameter>(); }
            if (dt.Rows.Count > 0)
            {
                OracleParameter item;
                foreach (DataRow r in dt.Rows)
                {
                    item = new OracleParameter();
                    item.ParameterName = r["ARGUMENT_NAME"].ToString();
                    if (r["IN_OUT"].ToString() == "OUT")
                    {
                        item.Direction = ParameterDirection.Output;
                        item.OracleDbType = OracleDbType.Int64;
                    }
                    lis.Add(item);
                }
                var properties = GetProperties(obj);
                foreach (var p in properties)
                {
                    string name = p.Name;
                    var value = p.GetValue(obj, null);
                    foreach (var item2 in lis)
                    {
                        if (item2.ParameterName.Substring(2) == name)
                        {
                            item2.Value = value;
                        }
                    }
                }
            }
            return lis;
        }
        private List<OracleParameter> GetOracleParameterOUT(string storeProduce)
        {
            var lis = new List<OracleParameter>();
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataTable dt = new DataTable();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"select argument_name, data_type,in_out  from ALL_ARGUMENTS where package_name is null and object_name = '{storeProduce.ToUpper()}' order by position asc";
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteReader();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                cmd.Connection.Close();
            }
            catch { cmd.Connection.Close(); return new List<OracleParameter>(); }
            if (dt.Rows.Count > 0)
            {
                OracleParameter item;
                foreach (DataRow r in dt.Rows)
                {
                    item = new OracleParameter();
                    item.ParameterName = r["ARGUMENT_NAME"].ToString();
                    if (r["IN_OUT"].ToString() == "OUT")
                    {
                        item.Direction = ParameterDirection.Output;
                        item.OracleDbType = OracleDbType.Object;
                    }
                    lis.Add(item);
                }
            }
            return lis;
        }
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
    }
}
