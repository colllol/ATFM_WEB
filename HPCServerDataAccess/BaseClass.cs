using System;
using HPCShareDLL;
using System.Data.SqlClient;
using System.Data;
using prjInfo;
using System.Text;
using System.Reflection;
using System.IO;
namespace HPCServerDataAccess
{
    public class BaseClass : HPCDataProvider
    {
        #region Private Property
        string databaseOwner = "dbo";
        string connectionString;
        #endregion

        #region Constructor
        public BaseClass(string databaseOwner, string connectionString)
        {
            this.connectionString = connectionString;
            this.databaseOwner = databaseOwner;
        }
        #endregion	

        public override void UpdateObject(object obj, string spName)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            string sql = spName;
            foreach (PropertyInfo propertyinfo in obj.GetType().GetProperties())
            {
                if (propertyinfo.PropertyType.ToString() == "System.DateTime")
                {
                    if ((DateTime)propertyinfo.GetValue(obj, null) == DateTime.MinValue || (DateTime)propertyinfo.GetValue(obj, null) == DateTime.MaxValue)
                        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, DBNull.Value));
                    else
                        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
                }
                else
                {
                    _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
                }
            }
            try
            {
                _sqlservice.ExecuteSP(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }

        }
        public override object GetObjectByID(string sp_name, string Id, string TypeName, string FieldIDName)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            string sql = sp_name;
            SqlDataReader reader;
            _sqlservice.AddParameter(new SqlParameter("@" + FieldIDName, Id));
            try
            {
                reader = _sqlservice.ExecuteSPReader(sql);
                //return CBO.FillObject(reader, Type.GetType("HPC_QLNS.Objects." + TypeName + ",DataAccess"));
                return CBO.FillObject(reader, Type.GetType("prjInfo." + TypeName + ",prjInfo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }

        public override int InsertObjectReturn(object obj, string spName)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            string sql = spName;
            foreach (PropertyInfo propertyinfo in obj.GetType().GetProperties())
            {
                if (propertyinfo.PropertyType.ToString() == "System.DateTime")
                {
                    if ((DateTime)propertyinfo.GetValue(obj, null) == DateTime.MinValue || (DateTime)propertyinfo.GetValue(obj, null) == DateTime.MaxValue)
                        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, DBNull.Value));
                    else
                        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
                }
                else
                    _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
            }
            SqlParameter paraOutput = new SqlParameter("@ReturnValue", SqlDbType.Int);
            paraOutput.Value = 0;
            paraOutput.Direction = ParameterDirection.Output;
            _sqlservice.AddParameter(paraOutput);
            try
            {
                _sqlservice.ExecuteSP(sql);
                return (int)paraOutput.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override void InsertObject(object obj, string spName)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            string sql = spName;
            foreach (PropertyInfo propertyinfo in obj.GetType().GetProperties())
            {
                if (propertyinfo.PropertyType.ToString() == "System.DateTime")
                {
                    if ((DateTime)propertyinfo.GetValue(obj, null) == DateTime.MinValue || (DateTime)propertyinfo.GetValue(obj, null) == DateTime.MaxValue)
                        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, DBNull.Value));
                    else
                        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
                }
                else
                    _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
            }
            try
            {
                _sqlservice.ExecuteSP(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }

        public override DataSet GetDataSet(string TableName, string ColumnList, string Where)
        {
            string _sql = "[CMS_GetColumnValues]";
            SqlService _sqlservice = new SqlService(connectionString);
            _sqlservice.AddParameter("@TableName", SqlDbType.NVarChar, TableName);
            _sqlservice.AddParameter("@ColumnList", SqlDbType.NVarChar, ColumnList);
            _sqlservice.AddParameter("@Where", SqlDbType.NVarChar, Where);
            try
            {
                return _sqlservice.ExecuteSPDataSet(_sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override string GetColumnValues(string TableName, string ColumnName, string Where)
        {
            string _sql = "[CMS_GetColumnValues]";
            SqlService _sqlservice = new SqlService(connectionString);
            SqlDataReader _reader;
            StringBuilder _result = new StringBuilder();
            string tmp = string.Empty;
            _sqlservice.AddParameter("@TableName", SqlDbType.NVarChar, TableName);
            _sqlservice.AddParameter("@ColumnList", SqlDbType.NVarChar, ColumnName);
            _sqlservice.AddParameter("@Where", SqlDbType.NVarChar, Where);
            try
            {
                _reader = _sqlservice.ExecuteSPReader(_sql);
                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        if (_reader[ColumnName] != DBNull.Value)
                            _result.Append(_reader[ColumnName].ToString() + ";");
                        else
                            _result.Append(" ;");
                    }
                }
                tmp = _result.ToString();
                return tmp.EndsWith(";") ? tmp.Substring(0, tmp.Length - 1) : tmp;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override DataSet GetStoreDataSet(string StoreName, string[] param1, object[] value)
        {
            string _sql = StoreName;
            SqlService _sqlservice = new SqlService(connectionString);
            for (int i = 0; i < param1.Length; i++)
            {
                _sqlservice.AddParameter(new SqlParameter(param1[i], value[i]));
            }
            try
            {
                return _sqlservice.ExecuteSPDataSet(_sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override double ExecStoreReturn(string StoreName, string[] param1, object[] value)
        {
            string _sql = StoreName;
            SqlService _sqlservice = new SqlService(connectionString);
            for (int i = 0; i < param1.Length; i++)
            {
                _sqlservice.AddParameter(new SqlParameter(param1[i], value[i]));
            }
            SqlParameter paraOutput = new SqlParameter("@ReturnValue", SqlDbType.Float);
            paraOutput.Value = 0;
            paraOutput.Direction = ParameterDirection.Output;
            _sqlservice.AddParameter(paraOutput);
            try
            {
                _sqlservice.ExecuteSP(_sql);
                return (double)paraOutput.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override void ExecStore(string StoreName, string[] param1, object[] value)
        {
            string _sql = StoreName;
            SqlService _sqlservice = new SqlService(connectionString);
            for (int i = 0; i < param1.Length; i++)
            {
                _sqlservice.AddParameter(new SqlParameter(param1[i], value[i]));
            }
            try
            {
                _sqlservice.ExecuteSP(_sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override void ExecStore(string StoreName)
        {
            string _sql = StoreName;
            SqlService _sqlservice = new SqlService(connectionString);            
            _sqlservice.ExecuteSP(_sql);
            _sqlservice.CloseConnect();         
            _sqlservice.Disconnect();
        }

        public override void ExecSql(string sql)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            try
            {
                _sqlservice.ExecuteSql(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override int ExecSqlReturnVal(string sql)
        {
            int returnVal = 0;
            SqlService _sqlservice = new SqlService(connectionString);
            try
            {
                returnVal = _sqlservice.ExecuteSqlReturn(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.Disconnect();
            }
            return returnVal;
        }
        public override DataSet ExecStoreDataSet(string StoreName)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            try
            {
                return _sqlservice.ExecuteSPDataSet(StoreName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override DataSet ExecSqlDataSet(string sql)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            try
            {
                return _sqlservice.ExecuteSqlDataSet(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override void InsertObject(object obj)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            string sql = "Sp_Insert" + obj.GetType().Name;
            foreach (PropertyInfo propertyinfo in obj.GetType().GetProperties())
            {
                if (propertyinfo.PropertyType.ToString() == "System.DateTime")
                {
                    if ((DateTime)propertyinfo.GetValue(obj, null) == DateTime.MinValue || (DateTime)propertyinfo.GetValue(obj, null) == DateTime.MaxValue)
                        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, DBNull.Value));
                    else
                        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
                }
                else
                    _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
            }
            try
            {
                _sqlservice.ExecuteSP(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override void UpdateObject(object obj)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            string sql = "CMS_UpdateRow" + obj.GetType().Name;
            foreach (PropertyInfo propertyinfo in obj.GetType().GetProperties())
            {
                //if (propertyinfo.PropertyType.ToString() == "System.DateTime")
                //{
                //    if ((DateTime)propertyinfo.GetValue(obj, null) == DateTime.MinValue || (DateTime)propertyinfo.GetValue(obj, null) == DateTime.MaxValue)
                //        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, DBNull.Value));
                //    else
                //        _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
                //}
                //else
                //{
                _sqlservice.AddParameter(new SqlParameter("@" + propertyinfo.Name, propertyinfo.GetValue(obj, null)));
                // }
            }
            try
            {
                _sqlservice.ExecuteSP(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override object GetObjectByID(string Id, string TypeName, string FieldIDName)
        {
            SqlService _sqlservice = new SqlService(connectionString);
            string sql = "CMS_SelectOneFrom" + TypeName;
            SqlDataReader reader;
            _sqlservice.AddParameter(new SqlParameter("@" + FieldIDName, Id));
            try
            {
                reader = _sqlservice.ExecuteSPReader(sql);
                return CBO.FillObject(reader, Type.GetType("prjInfo." + TypeName + ",prjInfo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        
        public override object GetObjectByCondition(string TypeName, string whereCondition)
        {
            object obj;
            SqlService _sqlservice = new SqlService(connectionString);
            //string sql = "select * from " + TypeName + " where " + whereCondition;
            string sql = "[CMS_GetObjectByCondition]";// Edit By BOCT// 26092009
            try
            {
                _sqlservice.AddParameter("@TypeName", SqlDbType.NVarChar, TypeName);
                _sqlservice.AddParameter("@whereCondition", SqlDbType.NVarChar, whereCondition);
                obj = CBO.FillObject(_sqlservice.ExecuteSPReader(sql), Type.GetType("prjInfo." + TypeName + ",prjInfo"));
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                _sqlservice.Disconnect();
            }
            return obj;
        }
        /// <summary>
        /// QUYNX - 18-06-2009
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        private T_Users GetFromReader_T_Users(SqlDataReader _reader)
        {
            T_Users t_users = null;
            if (!_reader.IsClosed)
            {
                t_users = new T_Users();
                t_users.UserID = Convert.ToInt32(_reader["UserID"]);
                if (_reader["UserName"] != DBNull.Value) t_users.UserName = Convert.ToString(_reader["Username"]);
                if (_reader["UserPass"] != DBNull.Value) t_users.UserPass = Convert.ToString(_reader["UserPass"]);
                if (_reader["UserFullName"] != DBNull.Value) t_users.UserFullName = Convert.ToString(_reader["UserFullName"]);
                if (_reader["UserEmail"] != DBNull.Value) t_users.UserEmail = Convert.ToString(_reader["UserEmail"]);
                if (_reader["UserMobile"] != DBNull.Value) t_users.UserMobile = Convert.ToString(_reader["UserMobile"]);
                if (_reader["UserAddress"] != DBNull.Value) t_users.UserAddress = Convert.ToString(_reader["UserAddress"]);
                if (_reader["UserBirthday"] != DBNull.Value) t_users.UserBirthday = Convert.ToDateTime(_reader["UserBirthday"]);
                if (_reader["UserActive"] != DBNull.Value) t_users.UserActive = Convert.ToInt32(_reader["UserActive"]);
                if (_reader["DateCreated"] != DBNull.Value) t_users.DateCreated = Convert.ToDateTime(_reader["DateCreated"]);
                if (_reader["DateModify"] != DBNull.Value) t_users.DateModify = Convert.ToDateTime(_reader["DateModify"]);
                if (_reader["UserCreate"] != DBNull.Value) t_users.UserCreate = Convert.ToInt32(_reader["UserCreate"]);
                if (_reader["IsReporter"] != DBNull.Value) t_users.IsReporter = Convert.ToInt32(_reader["IsReporter"]);
                if (_reader["Group_ID"] != DBNull.Value) t_users.Group_ID = Convert.ToInt32(_reader["Group_ID"]);
                if (_reader["RedirectPages"] != DBNull.Value) t_users.RedirectPages = Convert.ToString(_reader["RedirectPages"]);
            }
            return t_users;
        }
        //BOCT
        private T_Languages GetFromReader_T_Languages(SqlDataReader _reader)
        {
            T_Languages objLang = null;
            if (!_reader.IsClosed)
            {
                objLang = new T_Languages();
                objLang.Languages_ID = Convert.ToInt32(_reader["Languages_ID"]);
                if (_reader["Languages_Name"] != DBNull.Value) objLang.Languages_Name = Convert.ToString(_reader["Languages_Name"]);
                if (_reader["Description"] != DBNull.Value) objLang.Description = Convert.ToString(_reader["Description"]);
                if (_reader["Code"] != DBNull.Value) objLang.Code = Convert.ToString(_reader["Code"]);
                if (_reader["Tab"] != DBNull.Value) objLang.Tab = Convert.ToString(_reader["Tab"]);
                if (_reader["CssName"] != DBNull.Value) objLang.CssName = Convert.ToString(_reader["CssName"]);
                if (_reader["BannerBackground"] != DBNull.Value) objLang.BannerBackground = Convert.ToString(_reader["BannerBackground"]);
            }
            return objLang;
        }
        private T_Groups GetFromReader_T_Groups(SqlDataReader _reader)
        {
            T_Groups t_group = null;
            if (!_reader.IsClosed)
            {
                t_group = new T_Groups();
                t_group.Group_ID = Convert.ToInt32(_reader["Group_ID"]);
                if (_reader["Group_Name"] != DBNull.Value) t_group.Group_Name = Convert.ToString(_reader["Group_Name"]);
                if (_reader["Group_Description"] != DBNull.Value) t_group.Group_Description = Convert.ToString(_reader["Group_Description"]);
            }
            return t_group;
        }
        //private T_Categorys GetFromReader_T_Category(SqlDataReader _reader)
        //{
        //    T_Categorys obj_Cate = null;
        //    if (!_reader.IsClosed)
        //    {
        //        obj_Cate = new T_Categorys();
        //        obj_Cate.Categorys_ID = Convert.ToInt32(_reader["Categorys_ID"]);
        //        if (_reader["Category_Name"] != DBNull.Value) obj_Cate.Category_Name = Convert.ToString(_reader["Category_Name"]);
        //        if (_reader["Display"] != DBNull.Value) obj_Cate.Display = Convert.ToBoolean(_reader["Display"]);
        //        if (_reader["RssActive"] != DBNull.Value) obj_Cate.RssActive = Convert.ToBoolean(_reader["RssActive"]);
        //    }
        //    return obj_Cate;
        //}
        public override T_Users GetUserByUserPass(string userName, string passWord)
        {
            string _sql = "[CMS_GetUserByUserPass]";
            SqlService _sqlservice = new SqlService(connectionString);
            SqlDataReader _reader;
            _sqlservice.AddParameter("@userName", SqlDbType.NVarChar, userName);
            _sqlservice.AddParameter("@passWord", SqlDbType.NVarChar, passWord);
            try
            {
                _reader = _sqlservice.ExecuteSPReader(_sql);
                if (_reader.HasRows)
                {
                    _reader.Read();
                    return GetFromReader_T_Users(_reader);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override T_Users GetUserByUserName(string userName)
        {
            string _sql = "[CMS_GetUserByName]";
            SqlService _sqlservice = new SqlService(connectionString);
            SqlDataReader _reader;
            _sqlservice.AddParameter("@userName", SqlDbType.NVarChar, userName);
            try
            {
                _reader = _sqlservice.ExecuteSPReader(_sql);
                if (_reader.HasRows)
                {
                    _reader.Read();
                    return GetFromReader_T_Users(_reader);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override T_Users GetUserByUserName_ID(int userID)
        {
            string _sql = "[CMS_GetUserByUserName_ID]";
            SqlService _sqlservice = new SqlService(connectionString);
            SqlDataReader _reader;
            _sqlservice.AddParameter("@userID", SqlDbType.Int, userID);
            try
            {
                _reader = _sqlservice.ExecuteSPReader(_sql);
                if (_reader.HasRows)
                {
                    _reader.Read();
                    return GetFromReader_T_Users(_reader);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override T_Languages GetLanguagesNameBy_ID(int ID)
        {
            string _sql = "[CMS_GetLanguagesNameBy_ID]";
            SqlService _sqlservice = new SqlService(connectionString);
            SqlDataReader _reader;
            _sqlservice.AddParameter("@ID", SqlDbType.Int, ID);
            try
            {
                _reader = _sqlservice.ExecuteSPReader(_sql);
                if (_reader.HasRows)
                {
                    _reader.Read();
                    return GetFromReader_T_Languages(_reader);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        //public override T_Categorys GetCategoryNameBy_ID(int ID)
        //{
        //    string _sql = "[CMS_GetCategorysNameBy_ID]";
        //    SqlService _sqlservice = new SqlService(connectionString);
        //    SqlDataReader _reader;
        //    _sqlservice.AddParameter("@ID", SqlDbType.Int, ID);
        //    try
        //    {
        //        _reader = _sqlservice.ExecuteSPReader(_sql);
        //        if (_reader.HasRows)
        //        {
        //            _reader.Read();
        //            return GetFromReader_T_Category(_reader);
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        _sqlservice.CloseConnect();
        //        _sqlservice.Disconnect();
        //    }
        //}
        public override T_Groups GetGroupByGroupName_ID(int Group_ID)
        {
            string _sql = "[CMS_GetGroupByGroupName_ID]";
            SqlService _sqlservice = new SqlService(connectionString);
            SqlDataReader _reader;
            _sqlservice.AddParameter("@Group_ID", SqlDbType.Int, Group_ID);
            try
            {
                _reader = _sqlservice.ExecuteSPReader(_sql);
                if (_reader.HasRows)
                {
                    _reader.Read();
                    return GetFromReader_T_Groups(_reader);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override bool Login(string userName, string passWord)
        {
            string _sql = "[CMS_UserLogin]";
            SqlService _sqlservice = new SqlService(connectionString);
            _sqlservice.AddParameter("@userName", SqlDbType.NVarChar, userName);
            _sqlservice.AddParameter("@passWord", SqlDbType.NVarChar, passWord);
            try
            {
                return _sqlservice.ExecuteSPReader(_sql).HasRows ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        //public override T_RolePermission GetRole4User(int User_ID, string MenuName)
        //{
        //    string _sql = "GetRole4User";
        //    SqlService _sqlservice = new SqlService(connectionString);
        //    T_RolePermission _role = new T_RolePermission();
        //    SqlDataReader _reader;
        //    try
        //    {
        //        _sqlservice.AddParameter("@User_ID", SqlDbType.Int, User_ID);

        //        _sqlservice.AddParameter("@MenuName", SqlDbType.NVarChar, string.Format("{0}", Lib.SqlFormatText(MenuName)), 125);
        //        _reader = _sqlservice.ExecuteSPReader(_sql);

        //        if (_reader.HasRows)
        //        {
        //            _reader.Read();
        //            if (_reader["R_Add"] != DBNull.Value) _role.R_Add = Convert.ToBoolean(_reader["R_Add"]);
        //            if (_reader["R_Del"] != DBNull.Value) _role.R_Del = Convert.ToBoolean(_reader["R_Del"]);
        //            if (_reader["R_Edit"] != DBNull.Value) _role.R_Edit = Convert.ToBoolean(_reader["R_Edit"]);
        //            if (_reader["R_Pub"] != DBNull.Value) _role.R_Pub = Convert.ToBoolean(_reader["R_Pub"]);
        //            _reader.Close();
        //        }
        //        else
        //        {
        //            _role.R_Add = false;
        //            _role.R_Del = false;
        //            _role.R_Edit = false;
        //            _role.R_Pub = false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        _sqlservice.Disconnect();
        //    }
        //    return _role;
        //}

        public override T_RolePermission GetRole4UserMenu(int User_ID, int Menu_ID)
        {
            string _sql = "[CMS_GetRoleFourUserMenu]";
            SqlService _sqlservice = new SqlService(connectionString);
            T_RolePermission _role = new T_RolePermission();
            SqlDataReader _reader;
            try
            {
                _sqlservice.AddParameter("@User_ID", SqlDbType.Int, User_ID);

                _sqlservice.AddParameter("@Menu_ID", SqlDbType.Int, Menu_ID);
                _reader = _sqlservice.ExecuteSPReader(_sql);
                if (_reader.HasRows)
                {
                    _reader.Read();
                    if (_reader["R_Add"] != DBNull.Value) _role.R_Add = Convert.ToBoolean(_reader["R_Add"]);
                    if (_reader["R_Del"] != DBNull.Value) _role.R_Del = Convert.ToBoolean(_reader["R_Del"]);
                    if (_reader["R_Edit"] != DBNull.Value) _role.R_Edit = Convert.ToBoolean(_reader["R_Edit"]);
                    if (_reader["R_Pub"] != DBNull.Value) _role.R_Pub = Convert.ToBoolean(_reader["R_Pub"]);
                    _reader.Close();
                }
                else
                {
                    _role.R_Add = false;
                    _role.R_Del = false;
                    _role.R_Edit = false;
                    _role.R_Pub = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.Disconnect();
            }
            return _role;
        }
        public override void Insert_T_NewsEvent_Content_From_T_NewsEvent_Content(int news_id, int _CopyFrom)
        {
            string _sql = "[CMS_Insert_T_NewsEvent_Content_From_T_NewsEvent_Content]";
            SqlService _sqlservice = new SqlService();
            try
            {
                _sqlservice.AddParameter("@ID", SqlDbType.Int, news_id, true);
                _sqlservice.AddParameter("@Copyfrom", SqlDbType.Int, _CopyFrom, true);
      
                _sqlservice.ExecuteSP(_sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override void Insert_T_Event_Category_From_T_Event_Category(int news_id)
        {
            string _sql = "[CMS_Insert_T_Event_Category_From_T_Event_Category]";
            SqlService _sqlservice = new SqlService();
            try
            {
                _sqlservice.AddParameter("@ID", SqlDbType.Int, news_id, true);

                _sqlservice.ExecuteSP(_sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override void Insert_T_CategoryChuyenDe_From_T_CategoryChuyenDe(int news_id,int copyfrom)
        {
            string _sql = "[CMS_Insert_T_CategoryChuyenDe_From_T_CategoryChuyenDe]";
            SqlService _sqlservice = new SqlService();
            try
            {
                _sqlservice.AddParameter("@ID", SqlDbType.Int, news_id, true);
                _sqlservice.AddParameter("@CopyFrom", SqlDbType.Int, copyfrom, true);

                _sqlservice.ExecuteSP(_sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override void Insert_T_NewsChuyenDe_From_T_NewsChuyenDe(int news_id, int copyfrom, int Lang_id ,int status,int nguoisua,DateTime ngaysua)
        {
            string _sql = "[CMS_Insert_T_NewsChuyenDe_From_T_NewsChuyenDe]";
            SqlService _sqlservice = new SqlService();
            try
            {
                _sqlservice.AddParameter("@ID", SqlDbType.Int, news_id, true);
                _sqlservice.AddParameter("@CopyFrom", SqlDbType.Int, copyfrom, true);
                _sqlservice.AddParameter("@Lang_Id", SqlDbType.Int, Lang_id, true);
                _sqlservice.AddParameter("@News_Status", SqlDbType.Int, status, true);
                _sqlservice.AddParameter("@News_EditorID", SqlDbType.Int, nguoisua, true);
                _sqlservice.AddParameter("@News_DateEdit", SqlDbType.DateTime, ngaysua, true);

                _sqlservice.ExecuteSP(_sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override bool Check_Nhanban_T_Categorys_Chuyende(int id)
        {
            string _sql = "[CMS_Check_Nhanban_T_Category_ChuyenDe]";
            SqlService _sqlservice = new SqlService();
            DataSet _ds;
            try
            {
                _sqlservice.AddParameter("@ID", SqlDbType.Int, id, true);
                _ds = _sqlservice.ExecuteSPDataSet(_sql);
                if (_ds.Tables[0].Rows.Count > 0)
                    return false;
                else
                    return true;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override bool LanquageExitsTranlate(int id,int langId)
        {
            string _sql = "[CMS_LanquageExitsTranlate]";
            SqlService _sqlservice = new SqlService();
            DataSet _ds;
            try
            {
                _sqlservice.AddParameter("@New_ID", SqlDbType.Int, id, true);
                _sqlservice.AddParameter("@Languages_ID", SqlDbType.Int, langId, true);
                _ds = _sqlservice.ExecuteSPDataSet(_sql);
                if (_ds.Tables[0].Rows.Count > 0)
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override bool LanquageExitsTranlatein_T_Album_Categories(int id, int langId)
        {
            string _sql = "[CMS_LanquageExitsTranlatein_T_Album_Categories]";
            SqlService _sqlservice = new SqlService();
            DataSet _ds;
            try
            {
                _sqlservice.AddParameter("@Cat_Album_ID", SqlDbType.Int, id, true);
                _sqlservice.AddParameter("@Languages_ID", SqlDbType.Int, langId, true);
                _ds = _sqlservice.ExecuteSPDataSet(_sql);
                if (_ds.Tables[0].Rows.Count > 0)
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override bool ExitsTranlate_T_Album_Photo(int id, int langId)
        {
            string _sql = "[CMS_ExitsTranlate_T_Album_Photo]";
            SqlService _sqlservice = new SqlService();
            DataSet _ds;
            try
            {
                _sqlservice.AddParameter("@Alb_Photo_ID", SqlDbType.Int, id, true);
                _sqlservice.AddParameter("@Languages_ID", SqlDbType.Int, langId, true);
                _ds = _sqlservice.ExecuteSPDataSet(_sql);
                if (_ds.Tables[0].Rows.Count > 0)
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override bool ExitsTranlate_T_Multimedia(int id, int langId)
        {
            string _sql = "[CMS_ExitsTranlate_T_Multimedia]";
            SqlService _sqlservice = new SqlService();
            DataSet _ds;
            try
            {
                _sqlservice.AddParameter("@id", SqlDbType.Int, id, true);
                _sqlservice.AddParameter("@Languages_ID", SqlDbType.Int, langId, true);
                _ds = _sqlservice.ExecuteSPDataSet(_sql);
                if (_ds.Tables[0].Rows.Count > 0)
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
        public override bool ExitsTranlate_T_Photo_Even(int id, int langId)
        {
            string _sql = "[CMS_ExitsTranlate_T_Photo_Event]";
            SqlService _sqlservice = new SqlService();
            DataSet _ds;
            try
            {
                _sqlservice.AddParameter("@Photo_ID", SqlDbType.Int, id, true);
                _sqlservice.AddParameter("@Languages_ID", SqlDbType.Int, langId, true);
                _ds = _sqlservice.ExecuteSPDataSet(_sql);
                if (_ds.Tables[0].Rows.Count > 0)
                    return false;
                else
                    return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _sqlservice.CloseConnect();
                _sqlservice.Disconnect();
            }
        }
    }
}
