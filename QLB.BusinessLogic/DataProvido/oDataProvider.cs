using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using System.Reflection;


namespace QLB.BusinessLogic
{
    public class oDataProvider
    {
        // Oracle connection is supplied by environment-specific configuration.
        private string _cnnOracle = System.Configuration.ConfigurationManager.AppSettings["ConnectDB"].ToString();
        public bool ExecuteDelete(string packageName, string produceName, string id)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            int kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
                cmd.CommandType = CommandType.StoredProcedure;
                List<OracleParameter> lisParam = GetOracleParameterOUT(packageName, produceName);
                lisParam.Insert(0, new OracleParameter("P_ID", id) { Direction = ParameterDirection.Input, OracleDbType = OracleDbType.Int64 });
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                kq = cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex) { cmd.Connection.Close(); }
            return kq > 0 ? true : false;
        }

        //Restore Perm
        public Int64 ExecuteNonQuery(string produceName, params OracleParameter[] papa)
        {

            OracleConnection cnn = new OracleConnection(_cnnOracle);
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand(produceName, cnn);
            Int64 kq = -1;
            try
            {
                //cmd.Connection = cnn;
                cmd.Connection.Open();
                //cmd.CommandText = $"{ produceName}";
                cmd.CommandType = CommandType.StoredProcedure;
                //List<OracleParameter> lisParam = GetOracleParameter(packageName, produceName, obj);
                //if (lisParam.Count > 0)

                cmd.Parameters.AddRange(papa.ToArray());
                kq = cmd.ExecuteNonQuery();
                foreach (var item in papa)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        try { kq = Int64.Parse(item.Value.ToString()); } catch { }
                    }
                }
                cmd.Connection.Close();
            }
            catch { cmd.Connection.Close(); }
            if (kq == 0) kq = -1;
            return kq;
        }
        public Int64 ExecuteNonQuery(string packageName, string produceName, object obj)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            Int64 kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
                cmd.CommandType = CommandType.StoredProcedure;

                /* 
                 * note hungtn change 21/12/2017 1548
                 * List<OracleParameter> lisParam = GetOracleParameter(packageName, produceName, obj);
                */
                List<OracleParameter> lisParam = GetOracleParameter2(packageName, produceName, obj);
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                kq = cmd.ExecuteNonQuery();
                foreach (var item in lisParam)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        try { kq = Int64.Parse(item.Value.ToString()); break; } catch { }
                    }
                }
                cmd.Connection.Close();
            }
            catch { cmd.Connection.Close(); }
            if (kq == 0) kq = -1;
            return kq;
        }        
        public Int64 ExecuteNonQuery(string packageName, string produceName, params OracleParameter[] papa)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            Int64 kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
                cmd.CommandType = CommandType.StoredProcedure;
                //if (papa.Length > 0)
                //cmd.Parameters.AddRange(papa.ToArray());
                cmd.Parameters.AddRange(GetOracleParameter(packageName, produceName, papa).ToArray());
                cmd.ExecuteNonQuery();


                //03082018
                foreach (OracleParameter item in cmd.Parameters)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        try { kq = Int64.Parse(item.Value.ToString()); } catch { }
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
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
                cmd.CommandType = CommandType.StoredProcedure;
                List<OracleParameter> lisParam = GetOracleParameter2(packageName, produceName, obj);
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                cmd.ExecuteReader();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
                cmd.Connection.Close();
                return ds;
            }
            catch (Exception ex) {

                //LogAPI.LogToFile(LogFileType.EXCEPTION, "UsersRepository.GetUserByUserPass:" + ex.ToString());
                //oResponse.Code = "-99";
                //oResponse.Message = "Có lỗi xảy ra trong quá trình lấy dữ liệu";

                cmd.Connection.Close(); return ds;
            }
        }
        public DataSet ExecuteDatase(string packageName, string produceName, params OracleParameter[] param)
        {

            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataSet ds = new DataSet();
            try
            {

                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddRange(papa.ToArray());
                cmd.Parameters.AddRange(GetOracleParameter(packageName, produceName, param).ToArray());
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                //cb = new OracleCommandBuilder(oda);
                oda.Fill(ds);
                cmd.Connection.Close();
                return ds;
            }
            catch (Exception ex) { cmd.Connection.Close(); return ds; }
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
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
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
        public object ExecuteScalar_ForReport(string packageName, string produceName, params OracleParameter[] papa)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            object kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
                cmd.CommandType = CommandType.StoredProcedure;
                //if (papa.Length > 0)
                //cmd.Parameters.AddRange(papa.ToArray());
                cmd.Parameters.AddRange(GetOracleParameter(packageName, produceName, papa).ToArray());
                cmd.ExecuteNonQuery();
                foreach (OracleParameter item in cmd.Parameters)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        try { return item.Value; } catch { }
                    }
                }
                cmd.Connection.Close();
            }
            catch (Exception ex) { cmd.Connection.Close(); }
            return kq;
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
                cmd.CommandText = $"{ packageName.ToUpper()}.{produceName.ToUpper()}";
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
        #region privae
        private List<OracleParameter> GetOracleParameter2(string packageName, string produceName, object obj)
        {
            var lis = new List<OracleParameter>();
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataTable dt = new DataTable();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"select argument_name, data_type,in_out,position  from ALL_ARGUMENTS where package_name='{packageName.ToUpper()}' and object_name = '{produceName.ToUpper()}' and owner = 'ATFM' order by position asc";
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
                    item.Direction = r["IN_OUT"].ToString() == "IN" ? ParameterDirection.Input : ParameterDirection.Output;
                    item.OracleDbType = ConvertOracleDbType(r["DATA_TYPE"].ToString());
                    lis.Add(item);
                }
            }
            if (obj == null) return lis;
            var properties = GetProperties(obj);
            for (int i = 0; i < lis.Count; i++)
            {
                for (int j = 0; j < properties.Length; j++)
                {
                    if (lis[i].ParameterName.ToUpper().Substring(2) == properties[j].Name.ToUpper())
                    {
                        lis[i].Value = properties[j].GetValue(obj, null);
                    }
                }
            }
            return lis;
        }

        //Tuy bien param
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
                cmd.CommandText = $"select argument_name, data_type,in_out,position  from ALL_ARGUMENTS where package_name='{packageName.ToUpper()}' and object_name = '{produceName.ToUpper()}' and owner = 'ATFM' order by position asc";
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
                    if (r["DATA_TYPE"].ToString().Equals("NVARCHAR2"))
                        item.OracleDbType = OracleDbType.NVarchar2;
                    lis.Add(item);
                }
                var properties = GetProperties(obj);
                if (obj != null)
                {
                    foreach (var p in properties)
                    {
                        string name = p.Name;
                        var value = p.GetValue(obj, null);
                        foreach (var item2 in lis)
                        {
                            if (item2.ParameterName.Substring(2).ToLower() == name.ToLower())
                            {
                                item2.Value = value;
                            }
                        }
                    }
                }
            }
            return lis;
        }
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
        private List<OracleParameter> GetOracleParameterOUT(string packageName, string produceName)
        {
            var lis = new List<OracleParameter>();
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataTable dt = new DataTable();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"select argument_name, data_type,in_out,position  from ALL_ARGUMENTS where package_name='{packageName.ToUpper()}' and object_name = '{produceName.ToUpper()}' and owner = 'ATFM' order by position asc";
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
                        lis.Add(item);
                    }
                }
            }
            return lis;
        }
        private List<OracleParameter> GetOracleParameter(string packageName, string produceName, params OracleParameter[] paramsValue)
        {
            var lis = new List<OracleParameter>();
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataTable dt = new DataTable();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"select argument_name, data_type,in_out,position  from ALL_ARGUMENTS where package_name='{packageName.ToUpper()}' and object_name = '{produceName.ToUpper()}' and owner = 'ATFM' order by position asc";
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
                    item.Direction = r["IN_OUT"].ToString() == "IN" ? ParameterDirection.Input : ParameterDirection.Output;
                    item.OracleDbType = ConvertOracleDbType(r["DATA_TYPE"].ToString());
                    lis.Add(item);
                }
            }
            for (int i = 0; i < lis.Count; i++)
            {
                for (int j = 0; j < paramsValue.Length; j++)
                {
                    if (lis[i].ParameterName.ToUpper() == paramsValue[j].ParameterName.ToUpper())
                    {
                        lis[i] = paramsValue[j];
                    }
                }
            }
            return lis;
        }
        private OracleDbType ConvertOracleDbType(string typeDB)
        {
            string kq = string.Empty;
            int keyCount = DBTypeConversionKey.GetLength(0);

            for (int i = 0; i < keyCount; i++)
            {
                if (DBTypeConversionKey[i, 1].ToLower().Equals(typeDB.ToLower()))
                {
                    kq = DBTypeConversionKey[i, 0];
                    break;
                }
            }
            return (OracleDbType)Enum.Parse(typeof(OracleDbType), kq);
        }
        private static String[,] DBTypeConversionKey = new String[,] {
             { "Int64","NUMBER"},
             { "Array","ARRAY"},
             { "BFile","BFILE"},
             { "BinaryDouble","BinaryDouble"},
             { "BinaryFloat","BinaryFloat"},
             { "Blob","Blob"},
             { "Boolean","Boolean"},
             { "Byte","Byte"},
             { "Char","Char"},
             { "Clob","Clob"},
             { "Date","Date"},
             { "Decimal","Decimal"},
             { "Double","Double"},
             { "Int16","Int16"},
             { "Int32","Int32"},
             { "Int64","Int64"},
             { "IntervalDS","IntervalDS"},
             { "IntervalYM","IntervalYM"},
             { "Long","Long"},
             { "LongRaw","LongRaw"},
             { "NChar","NChar"},
             { "NClob","NClob"},
             { "NVarchar2","NVarchar2"},
             { "Object","Object"},
             { "Raw","Raw"},
             { "Ref","Ref"},
             { "RefCursor","REF CURSOR"},
             { "Single","Single"},
             { "TimeStamp","TimeStamp"},
             { "TimeStampLTZ","TimeStampLTZ"},
             { "TimeStampTZ","TimeStampTZ"},
             { "Varchar2","Varchar2"},
             { "XmlType","XmlType"},
             { "Int32","INT"}
        };
        #endregion
        #region api extension : excute by (packageName) packagename,(produceName) procedure,(object) new {parameternameUpper = Value}
        public object ExecuteNonQueryForApiExtension(string packageName, string produceName, object obj)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            object kq = -1;
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
                cmd.CommandType = CommandType.StoredProcedure;

                /* 
                 * note hungtn change 21/12/2017 15:48
                 * List<OracleParameter> lisParam = GetOracleParameter(packageName, produceName, obj);
                */
                List<OracleParameter> lisParam = GetOracleParameterApiExtension(packageName, produceName, obj);
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                kq = cmd.ExecuteNonQuery();
                foreach (var item in lisParam)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        try { kq = item.Value.ToString(); break; } catch { }
                    }
                }
                cmd.Connection.Close();
            }
            catch(Exception ex) {cmd.Connection.Close();}
            //if (kq == 0) kq = -1;
            return kq;
        }
        private List<OracleParameter> GetOracleParameterApiExtension(string packageName, string produceName, object obj)
        {
            var lis = new List<OracleParameter>();
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataTable dt = new DataTable();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"select argument_name, data_type,in_out,position  from ALL_ARGUMENTS where package_name='{packageName.ToUpper()}' and object_name = '{produceName.ToUpper()}' and owner = 'ATFM' order by position asc";
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
                    item.Direction = r["IN_OUT"].ToString() == "IN" ? ParameterDirection.Input : ParameterDirection.Output;
                    item.OracleDbType = ConvertOracleDbType(r["DATA_TYPE"].ToString());
                    if (r["DATA_TYPE"].ToString().Contains("VARCHAR"))
                        item.Size = 4000;
                    lis.Add(item);
                }
            }
            if (obj == null) return lis;
            for (int i = 0; i < lis.Count; i++)
            {
                lis[i].Value = GetValueFromObjectDynamic(lis[i].ParameterName.ToUpper(), obj);
            }
            return lis;
        }
        private object GetValueFromObjectDynamic(string nameProp, object obj)
        {
            try
            {
                dynamic o = obj;
                return o[nameProp];
            }
            catch
            {
                return null;
            }
        }
        public DataSet ExecuteDataseForApiExtension(string packageName, string produceName, object obj)
        {
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand();
            OracleConnection cnn = new OracleConnection(_cnnOracle);
            DataSet ds = new DataSet();
            try
            {
                cmd.Connection = cnn;
                cmd.Connection.Open();
                cmd.CommandText = $"{ packageName}.{produceName}".ToUpper();
                cmd.CommandType = CommandType.StoredProcedure;
                List<OracleParameter> lisParam = GetOracleParameterApiExtension(packageName, produceName, obj);
                if (lisParam.Count > 0)
                    cmd.Parameters.AddRange(lisParam.ToArray());
                cmd.ExecuteReader();
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
                cmd.Connection.Close();
                return ds;
            }
            catch (Exception ex) { cmd.Connection.Close(); return ds; }
        }
        #endregion

    }

}
