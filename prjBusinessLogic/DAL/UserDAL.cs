using System;
using System.Collections.Generic;
using System.Text;
using prjInfo;
using HPCShareDLL;
using System.Data;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace prjBusinessLogic
{
    public class UserDAL
    {
        HttpClient client = new HttpClient();
        public List<T_Users> GetAllUser()
        {
            return new clsResuftAPI<T_Users>().GetListObj("api/Users/GetAllUsers");
        }
        public List<T_Users> GetPageUsers(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<T_Users>().GetListObj("api/Users/GetPageUsers/", pageSize, pageIndex, where);
        }

        public List<T_Users> GetPageUsers_New(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<T_Users>().GetListObj("api/Users/GetPageUsers_New", pageSize, pageIndex, where);
        }
        public List<T_Users> GetAll(string urlJson)
        {
            List<T_Users> users = new List<T_Users>();
            try
            {
                client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"] + urlJson).Result;
                var rs = response.Content.ReadAsAsync<dynamic>().Result;
                if (rs.Code == "00")
                {
                    dynamic oValue = rs.ListValue;
                    foreach (var item in oValue)
                    {
                        users.Add(new T_Users
                        {
                            UserID = int.Parse((item.UserId ?? "0").ToString()),
                            UserName = (item.UserName ?? "0").ToString(),
                            UserFullName = (item.UserFullName ?? "0").ToString(),
                            UserActive = isCheckBool((item.UserAvtive ?? "0").ToString()),
                        });
                    }
                }
                else
                    users = null;
            }
            catch (Exception ex)
            {
                users = null;
                throw ex;
            }
            return users;
        }
        private bool isCheckBool(string _value)
        {
            if (_value == "1")
                return true;
            else
                return false;
        }
        public DataSet BindGridT_Users(int PageIndex, int PageSize, string WhereCondition)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet("[CMS_ListT_UsersDynamic]", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Login(string userName, string passWord)
        {
            try
            {
                return HPCDataProvider.Instance().Login(userName, passWord);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public T_Users GetUserByUserName(string userName)
        {
            try
            {
                //return HPCDataProvider.Instance().GetUserByUserName(userName);
                return new clsResuftAPI<T_Users>().GetOneObjSys("api/Users/GetUserByUserName/", userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*public T_Users GetUserByUserName(string Username)
        {
            return new clsResuftAPI<T_Users>().GetOneObjSys("api/Users/GetUserByUserName/", Username);
        }
        */

        public T_Users GetUserByUserPassSQL(string userName, string passWord)
        {
            try
            {
                return HPCDataProvider.Instance().GetUserByUserPass(userName, passWord);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T_Users GetUserByUserPass(string Username, string Password)
        {
            return new clsResuftAPI<T_Users>().GetOneObjSys("api/Users/GetUserByUserPass/", Username, Password);
        }
        public void UpdateT_UsersInfo(T_Users _users)
        {
            try
            {
                HPCDataProvider.Instance().UpdateObject(_users, "[CMS_InsertT_UsersInfo]");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int InsertT_Users(T_Users obj)
        {
            return HPCDataProvider.Instance().InsertObjectReturn(obj, "[CMS_InsertT_Users]");
        }
        public string InsertT_UsersOrc(T_Users obj)
        {
            return new clsResuftAPI<T_Users>().InsertReturnId(obj, "api/Users/CreateUsers");
        }
        /* public T_Users GetOneFromT_UsersByID(int ID)
         {
             try
             {
                 return (T_Users)HPCDataProvider.Instance().GetObjectByID(ID.ToString(), "T_Users", "UserID");
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }*/
        public T_Users GetOneFromT_UsersByID(int UserID)
        {
            // api/Users/GetUSERNAME? Username = { Username }&UserID={UserID
            return new clsResuftAPI<T_Users>().GetOneObjSysbyUserID("api/Users/GetByIdUsers/", UserID.ToString());
        }

        public void DeleteFromT_UsersByID(int ID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_DeleteOneFromT_Users]", new string[] { "@UserID" }, new object[] { ID });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "T_USERS_DELETE".ToUpper(), new { P_USER_ID = ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckExistUser(string username)
        {
            try
            {
                int i = int.Parse(new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_CheckExistUser".ToUpper(), new { P_USERNAME = username }).ToString());
                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        public bool CheckDelete_users(int userid)
        {
            try
            {

                int i = int.Parse(new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_CheckDelete_Users".ToUpper(), new { P_USER_ID = userid }).ToString());
                if (i > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            //try
            //{
            //    //DataSet ds = HPCDataProvider.Instance().GetStoreDataSet("[CMS_CheckDelete_Users]", new string[] { "@UserID" }, new object[] { userid });

            //    if (ds.Tables[0].Rows.Count > 0)
            //        return true;
            //    else
            //        return false;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public void InsertT_UserMenu(int UserID, int MenuID, int _R_Edit, int _R_Del, int _R_Add, int _R_Pub, int GroupID)
        {
            try
            {
                //HPCDataProvider.Instance().ExecStore("[CMS_InsertT_UserMenuDynamic]", new string[] { "@User_ID", "@Menu_ID", "@R_Edit", "@R_Del", "@R_Add", "@R_Pub", "@group_ID" }, new object[] { UserID, MenuID, _R_Edit, _R_Del, _R_Add, _R_Pub, GroupID });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_InsertT_UserMenuDynamic".ToUpper(), new { P_USER_ID = UserID, P_MENU_ID = MenuID, P_R_EDIT = _R_Edit, P_R_DEL = _R_Del, P_R_ADD = _R_Add, P_R_PUB = _R_Pub, P_GROUP_ID = GroupID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CMS_ResetPass(int UserID, string p_Pass)
        {
            try
            {
                //HPCDataProvider.Instance().ExecStore("[CMS_InsertT_UserMenuDynamic]", new string[] { "@User_ID", "@Menu_ID", "@R_Edit", "@R_Del", "@R_Add", "@R_Pub", "@group_ID" }, new object[] { UserID, MenuID, _R_Edit, _R_Del, _R_Add, _R_Pub, GroupID });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_ResetPass".ToUpper(), new { P_USER_ID = UserID, P_PASS = p_Pass });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       


        public void InsertT_UserCategory(int UserID, int Categorys_ID, int GroupID)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_InsertT_UserCategoryDynamic]", new string[] { "@User_ID", "@Categorys_ID", "@group_ID" }, new object[] { UserID, Categorys_ID, GroupID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertT_UserReport(int UserID, int ReportID, int GroupID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_InsertT_UserRePortDynamic]", new string[] { "@User_ID", "@Languages_ID", "@Group_ID" }, new object[] { UserID, LangID, GroupID });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_InsertT_UserRePortDynamic".ToUpper(), new { P_REPORTID = ReportID, P_USERID = UserID, P_GROUP_ID = GroupID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertT_UserLanguages(int UserID, int LangID, int GroupID)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_InsertT_UserLanguagesDynamic]", new string[] { "@User_ID", "@Languages_ID", "@Group_ID" }, new object[] { UserID, LangID, GroupID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertT_UserPapers(int UserID, int LangID, int GroupID)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_InsertT_UserPapersDynamic]", new string[] { "@User_ID", "@Paper_ID", "@Group_ID" }, new object[] { UserID, LangID, GroupID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetMenuNameMaster(int ID)
        {
            return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_GetMenuNameMaster".ToUpper(), new { P_MENU_ID = ID });
        }
        public DataTable BindNavigationMaster(string where)
        {
            return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_BindNavigation".ToUpper(), new { P_WHERE = where });
        }
        public DataTable GetRole4UserMenu(string UserName, int Menu_ID)
        {
            return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_GetRole4UserMenu".ToUpper(), new { P_USERNAME = UserName, P_MENU_ID = Menu_ID });
        }


        public DataTable GetT_GroupMenuDynamic(int ID)
        {
            //UltilFunc _untilDAL = new UltilFunc();
            // return _untilDAL.GetStoreDataSet("[CMS_SelectT_GroupMenuDynamic]", new string[] { "@WhereCondition" }, new object[] { " Group_ID = " + ID }).Tables[0];
            return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_SelectT_GroupMenuDynamic".ToUpper(), new { P_GROUP_ID = ID });
        }
        public DataTable GetT_GroupCategoryDynamic(int ID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            return _untilDAL.GetStoreDataSet("[CMS_SelectT_GroupCategoryDynamic]", new string[] { "@WhereCondition" }, new object[] { " Group_ID = " + ID }).Tables[0];
        }
        public DataTable GetT_GroupLanguagesDynamic(int ID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            return _untilDAL.GetStoreDataSet("[CMS_SelectT_GroupLanguagesDynamic]", new string[] { "@WhereCondition" }, new object[] { " Group_ID = " + ID }).Tables[0];
        }
        public DataTable GetT_GroupPapersDynamic(int ID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            return _untilDAL.GetStoreDataSet("[CMS_SelectT_GroupPapersDynamic]", new string[] { "@WhereCondition" }, new object[] { " Group_ID = " + ID }).Tables[0];
        }
        public void DeleteFromT_UserMenuDynamic(int User_ID, string strSql)
        {
            try
            {
                //HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserMenuDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_DeleteT_UserMenuDynamic".ToUpper(), new { P_USER_ID = User_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteT_UserMenuByMenu(int User_ID, int Menu_ID)
        {
            try
            {
                //HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserMenuDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_DeleteT_UserMenuByMenu".ToUpper(), new { P_USER_ID = User_ID, P_MENU_ID = Menu_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteT_UserMenuByAll(int User_ID)
        {
            try
            {
                //HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserMenuDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_DeleteT_UserMenuAll".ToUpper(), new { P_USER_ID = User_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteT_UserMenuByGroup(int User_ID, int group_ID)
        {
            try
            {
                //HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserMenuDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_DeleteT_UserMenuByGroup".ToUpper(), new { P_USER_ID = User_ID, P_GROUP_ID = group_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //CMS_DeleteT_UserMenuDynamic

        public void DeleteFromT_UserCategoryDynamic(int User_ID, string strSql)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserCategoryDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteFromT_UserLanguagesDynamic(int User_ID, string strSql)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserLanguagesDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });
                //new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_DeleteT_UserLanguagesDynamic".ToUpper(), new { P_USER_ID = UserID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //CMS_DeleteT_UserReportDynamic
        public void DeleteFromT_UserReportDynamic(int User_ID, int GroupID)
        {
            try
            {
                //HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserLanguagesDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_DeleteT_UserReportDynamic".ToUpper(), new { P_USER_ID = User_ID, P_GROUP_ID = GroupID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteFromT_UserPaperDynamic(int User_ID, string strSql)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserPapersDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateRow_T_Users(T_Users _t_users)
        {
            try
            {
                HPCDataProvider.Instance().UpdateObject(_t_users);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateFromT_UsersDynamic(string WhereCondition)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update_UserActiveDynamic(int UserID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_UpdateT_UsersDynamic".ToUpper(), new { P_USER_ID = UserID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllFrom_T_GroupNotInUser(int UserID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_GetAll_T_GroupNotInUser".ToUpper(), new { P_USER_ID = UserID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllFrom_T_GroupInUser(int UserID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_GetAllFrom_T_GroupInUser".ToUpper(), new { P_USER_ID = UserID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetMenu4User(int UserID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_GetMenu4User".ToUpper(), new { P_USER_ID = UserID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllMenu4User(int UserID)
        {
            return new clsResuftAPI().GetAllMenu4User("api/Users/GetAllMenu4User", UserID);
        }

        public DataTable BindNavigationByUserID(int UserID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_BindNavigationByUserID".ToUpper(), new { P_USER_ID = UserID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // Group
        public ArrayList GetAllFrom_T_GroupNotInUser(string pane, int id)
        {
            UltilFunc _untilDAL = new UltilFunc();
            ArrayList _arr = new ArrayList();
            DataTable _dt;
            try
            {
                //_dt = _untilDAL.GetStoreDataSet("CMS_GetAllFrom_T_GroupNotInUser", new string[] { "@User_ID" }, new object[] { id }).Tables[0];
                _dt = GetAllFrom_T_GroupNotInUser(id);

                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        T_Groups g = new T_Groups();
                        if (_dt.Rows[i]["Group_ID"] != DBNull.Value)
                            g.Group_ID = Convert.ToInt32(_dt.Rows[i]["Group_ID"]);
                        if (_dt.Rows[i]["Group_Name"] != DBNull.Value)
                            g.Group_Name = Convert.ToString(_dt.Rows[i]["Group_Name"]);
                        _arr.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _arr;
        }
        public ArrayList GetAllFrom_T_GroupInUser(string pane, int id)
        {
            UltilFunc _untilDAL = new UltilFunc();
            ArrayList _arr = new ArrayList();
            DataTable _dt;
            try
            {
                //_dt = _untilDAL.GetStoreDataSet("CMS_GetAllFrom_T_GroupInUser", new string[] { "@User_ID" }, new object[] { id }).Tables[0];
                _dt = GetAllFrom_T_GroupInUser(id);
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        T_Groups g = new T_Groups();
                        if (_dt.Rows[i]["Group_ID"] != DBNull.Value) g.Group_ID = Convert.ToInt32(_dt.Rows[i]["Group_ID"]);
                        if (_dt.Rows[i]["Group_Name"] != DBNull.Value) g.Group_Name = Convert.ToString(_dt.Rows[i]["Group_Name"]);
                        _arr.Add(g);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _arr;
        }

        private DataTable API_BindGridMenuByUse(int ParrentId, int UserID)
        {
            try
            {
                //return new clsResuftAPI<DataTable>().GetOneObjSys4Paramater("api/Users/BindGridMenuByUser/",new string[] { "Parrent_ID", "UserId" },new object[] { ParrentId, UserID } );
                return new clsResuftAPI().GetTableUserParamater("api/Users/BindGridMenuByUser/", ParrentId, UserID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BindGridMenuByUser(int UserID)
        {
            UserDAL _objDAL = new UserDAL();
            UltilFunc _untilDAL = new UltilFunc();
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Menu_ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Menu_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Module_Order", typeof(string)));
            dt.Columns.Add(new DataColumn("Module_URL", typeof(string)));
            dt.Columns.Add(new DataColumn("Menu_Desc", typeof(string)));
            dt.Columns.Add(new DataColumn("Role_Menu", typeof(bool)));
            dt.Columns.Add(new DataColumn("Role_Group", typeof(bool)));

            dt.Columns.Add(new DataColumn("R_Add", typeof(bool)));
            dt.Columns.Add(new DataColumn("R_Edit", typeof(bool)));
            dt.Columns.Add(new DataColumn("R_Del", typeof(bool)));
            dt.Columns.Add(new DataColumn("R_Pub", typeof(bool)));

            //DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridMenuByUser]", new string[] { "@Parrent_ID", "@User_ID" }, new object[] { 0, UserID }).Tables[0];
            DataTable _dt = _objDAL.API_BindGridMenuByUse(0, UserID);
            if (_dt.Rows.Count > 0)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {

                    dr = dt.NewRow();
                    dr[0] = _dt.Rows[i]["ID"].ToString();
                    dr[1] = _dt.Rows[i]["MenuName"].ToString();
                    dr[2] = _dt.Rows[i]["MenuOrder"].ToString();
                    dr[3] = _dt.Rows[i]["MenuURL"].ToString();
                    dr[4] = _dt.Rows[i]["MenuDesc"].ToString();
                    dr[5] = Convert.ToBoolean(_dt.Rows[i]["role"]);
                    if (_dt.Rows[i]["ingroup"] != DBNull.Value)
                        dr[6] = !Convert.ToBoolean(_dt.Rows[i]["ingroup"]);

                    if (_dt.Rows[i]["R_Add"] != DBNull.Value)
                        dr[7] = Convert.ToInt32(_dt.Rows[i]["R_Add"]);
                    else dr[7] = false;
                    if (_dt.Rows[i]["R_Edit"] != DBNull.Value)
                        dr[8] = Convert.ToBoolean(_dt.Rows[i]["R_Edit"]);
                    else dr[8] = false;
                    if (_dt.Rows[i]["R_Del"] != DBNull.Value)
                        dr[9] = Convert.ToBoolean(_dt.Rows[i]["R_Del"]);
                    else dr[9] = false;
                    if (_dt.Rows[i]["R_Pub"] != DBNull.Value)
                        dr[10] = Convert.ToBoolean(_dt.Rows[i]["R_Pub"]);
                    else dr[10] = false;
                    dt.Rows.Add(dr);
                    //}
                    //Kiem tra xem chuc nang co chuyen muc con hay khong

                    if (prjBusinessLogic.UltilFunc.GetLatestID("T_Menus", "ID", "WHERE ParrentID=" + _dt.Rows[i].ItemArray[0].ToString()) > 0)
                    {
                        // DataTable _dtChild = _untilDAL.GetStoreDataSet("[CMS_BindGridMenuByUser]", new string[] { "@Parrent_ID", "@User_ID" }, new object[] { _dt.Rows[i].ItemArray[0], UserID }).Tables[0];
                        DataTable _dtChild = _objDAL.API_BindGridMenuByUse(Convert.ToInt32(_dt.Rows[i].ItemArray[0]), UserID);
                        if (_dtChild.Rows.Count > 0)
                        {
                            for (int j = 0; j < _dtChild.Rows.Count; j++)
                            {
                                dr = dt.NewRow();
                                dr[0] = _dtChild.Rows[j]["ID"].ToString();
                                dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild.Rows[j]["MenuName"].ToString();
                                dr[2] = _dtChild.Rows[j]["MenuOrder"].ToString();
                                dr[3] = _dtChild.Rows[j]["MenuURL"].ToString();
                                dr[4] = _dtChild.Rows[j]["MenuDesc"].ToString();
                                dr[5] = Convert.ToBoolean(_dtChild.Rows[j]["role"]);
                                if (_dtChild.Rows[j]["ingroup"] != DBNull.Value)
                                    dr[6] = !Convert.ToBoolean(_dtChild.Rows[j]["ingroup"]);

                                if (_dtChild.Rows[j]["R_Add"] != DBNull.Value)
                                    dr[7] = Convert.ToBoolean(_dtChild.Rows[j]["R_Add"]);
                                else dr[7] = false;
                                if (_dtChild.Rows[j]["R_Edit"] != DBNull.Value)
                                    dr[8] = Convert.ToBoolean(_dtChild.Rows[j]["R_Edit"]);
                                else dr[8] = false;
                                if (_dtChild.Rows[j]["R_Del"] != DBNull.Value)
                                    dr[9] = Convert.ToBoolean(_dtChild.Rows[j]["R_Del"]);
                                else dr[9] = false;
                                if (_dtChild.Rows[j]["R_Pub"] != DBNull.Value)
                                    dr[10] = Convert.ToBoolean(_dtChild.Rows[j]["R_Pub"]);
                                else dr[10] = false;
                                dt.Rows.Add(dr);

                                // Kiem tra xem chuc nang hien tai co chuyen muc cap 3 hay khong
                                if (prjBusinessLogic.UltilFunc.GetLatestID("T_Menus", "ID", "WHERE ParrentID=" + _dtChild.Rows[j].ItemArray[0].ToString()) > 0)
                                {
                                    //DataTable _dtChild1 = _untilDAL.GetStoreDataSet("[CMS_BindGridMenuByUser]", new string[] { "@Parrent_ID", "@User_ID" }, new object[] { _dtChild.Rows[j].ItemArray[0], UserID }).Tables[0];
                                    DataTable _dtChild1 = _objDAL.API_BindGridMenuByUse(Convert.ToInt32(_dtChild.Rows[i].ItemArray[0]), UserID);
                                    for (int j1 = 0; j1 < _dtChild1.Rows.Count; j1++)
                                    {
                                        dr = dt.NewRow();
                                        dr[0] = _dtChild1.Rows[j1]["ID"].ToString();
                                        dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild1.Rows[j1]["MenuName"].ToString();
                                        dr[2] = _dtChild1.Rows[j1]["MenuOrder"].ToString();
                                        dr[3] = _dtChild1.Rows[j1]["MenuURL"].ToString();
                                        dr[4] = _dtChild1.Rows[j1]["MenuDesc"].ToString();
                                        dr[5] = Convert.ToBoolean(_dtChild1.Rows[j1]["role"]);
                                        if (_dtChild1.Rows[j1]["ingroup"] != DBNull.Value)
                                            dr[6] = !Convert.ToBoolean(_dtChild1.Rows[j1]["ingroup"]);

                                        if (_dtChild1.Rows[j1]["R_Add"] != DBNull.Value)
                                            dr[7] = Convert.ToBoolean(_dtChild1.Rows[j1]["R_Add"]);
                                        else dr[7] = false;
                                        if (_dtChild1.Rows[j1]["R_Edit"] != DBNull.Value)
                                            dr[8] = Convert.ToBoolean(_dtChild1.Rows[j1]["R_Edit"]);
                                        else dr[8] = false;
                                        if (_dtChild1.Rows[j1]["R_Del"] != DBNull.Value)
                                            dr[9] = Convert.ToBoolean(_dtChild1.Rows[j1]["R_Del"]);
                                        else dr[9] = false;
                                        if (_dtChild1.Rows[j1]["R_Pub"] != DBNull.Value)
                                            dr[10] = Convert.ToBoolean(_dtChild1.Rows[j1]["R_Pub"]);
                                        else dr[10] = false;
                                        dt.Rows.Add(dr);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }
        #region ext
        public DataTable BindGridT_UserCategoryByUser(int UserID, int LangID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("Categorys_ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Category_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Category_Order", typeof(string)));
            dt.Columns.Add(new DataColumn("Role_Cate", typeof(bool)));
            dt.Columns.Add(new DataColumn("Role_Group", typeof(bool)));
            DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByUser]", new string[] { "@Parrent_ID", "@User_ID", "@LangID" }, new object[] { 0, UserID, LangID }).Tables[0];

            if (_dt.Rows.Count > 0)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {

                    dr = dt.NewRow();
                    dr[0] = _dt.Rows[i]["Categorys_ID"].ToString();
                    dr[1] = "<b>" + _dt.Rows[i]["Category_Name"].ToString();
                    dr[2] = _dt.Rows[i]["Category_Order"].ToString();
                    dr[3] = Convert.ToBoolean(_dt.Rows[i]["role"]);
                    dr[4] = !Convert.ToBoolean(_dt.Rows[i]["ingroup"]);
                    dt.Rows.Add(dr);
                    //}
                    //Kiem tra xem chuc nang co chuyen muc con hay khong
                    if (prjBusinessLogic.UltilFunc.GetLatestID("T_Categorys", "Categorys_ID", "WHERE Category_ParrentID=" + _dt.Rows[i]["Categorys_ID"].ToString()) > 0)
                    {
                        DataTable _dtChild = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByUser]", new string[] { "@Parrent_ID", "@User_ID", "@LangID" }, new object[] { _dt.Rows[i]["Categorys_ID"].ToString(), UserID, LangID }).Tables[0];
                        if (_dtChild.Rows.Count > 0)
                        {
                            for (int j = 0; j < _dtChild.Rows.Count; j++)
                            {
                                dr = dt.NewRow();
                                dr[0] = _dtChild.Rows[j]["Categorys_ID"].ToString();
                                dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild.Rows[j]["Category_Name"].ToString();
                                dr[2] = _dtChild.Rows[j]["Category_Order"].ToString();
                                dr[3] = Convert.ToBoolean(_dtChild.Rows[j]["role"]);
                                dr[4] = !Convert.ToBoolean(_dtChild.Rows[j]["ingroup"]);
                                dt.Rows.Add(dr);

                                // Kiem tra xem chuc nang hien tai co chuyen muc cap 3 hay khong
                                if (prjBusinessLogic.UltilFunc.GetLatestID("T_Categorys", "Categorys_ID", "WHERE Category_ParrentID=" + _dtChild.Rows[j]["Categorys_ID"].ToString()) > 0)
                                {
                                    DataTable _dtChild1 = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByUser]", new string[] { "@Parrent_ID", "@User_ID", "@LangID" }, new object[] { _dtChild.Rows[j]["Categorys_ID"].ToString(), UserID, LangID }).Tables[0];

                                    for (int j1 = 0; j1 < _dtChild1.Rows.Count; j1++)
                                    {
                                        dr = dt.NewRow();
                                        dr[0] = _dtChild1.Rows[j1]["Categorys_ID"].ToString();
                                        dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild1.Rows[j1]["Category_Name"].ToString();
                                        dr[2] = _dtChild1.Rows[j1]["Category_Order"].ToString();
                                        dr[3] = Convert.ToBoolean(_dtChild1.Rows[j1]["role"]);
                                        dr[4] = !Convert.ToBoolean(_dtChild1.Rows[j1]["ingroup"]);
                                        dt.Rows.Add(dr);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }
        //Add by nvthai
        public DataTable BindGridT_UserCategoryByLoaibao(int UserID, int LoaiID, int LangID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("Categorys_ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Category_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Category_Order", typeof(string)));
            dt.Columns.Add(new DataColumn("Role_Cate", typeof(bool)));
            dt.Columns.Add(new DataColumn("Role_Group", typeof(bool)));
            DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByLoaibao]", new string[] { "@Parrent_ID", "@User_ID", "@LoaiID", "@LangID" }, new object[] { 0, UserID, LoaiID, LangID }).Tables[0];

            if (_dt.Rows.Count > 0)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {

                    dr = dt.NewRow();
                    dr[0] = _dt.Rows[i]["Categorys_ID"].ToString();
                    dr[1] = "<b>" + _dt.Rows[i]["Category_Name"].ToString();
                    dr[2] = _dt.Rows[i]["Category_Order"].ToString();
                    dr[3] = Convert.ToBoolean(_dt.Rows[i]["role"]);
                    dr[4] = !Convert.ToBoolean(_dt.Rows[i]["ingroup"]);
                    dt.Rows.Add(dr);
                    //}
                    //Kiem tra xem chuc nang co chuyen muc con hay khong
                    if (prjBusinessLogic.UltilFunc.GetLatestID("T_Categorys", "Categorys_ID", "WHERE Category_ParrentID=" + _dt.Rows[i]["Categorys_ID"].ToString()) > 0)
                    {
                        DataTable _dtChild = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByLoaibao]", new string[] { "@Parrent_ID", "@User_ID", "@LoaiID", "@LangID" }, new object[] { _dt.Rows[i]["Categorys_ID"].ToString(), UserID, LoaiID, LangID }).Tables[0];
                        if (_dtChild.Rows.Count > 0)
                        {
                            for (int j = 0; j < _dtChild.Rows.Count; j++)
                            {
                                dr = dt.NewRow();
                                dr[0] = _dtChild.Rows[j]["Categorys_ID"].ToString();
                                dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild.Rows[j]["Category_Name"].ToString();
                                dr[2] = _dtChild.Rows[j]["Category_Order"].ToString();
                                dr[3] = Convert.ToBoolean(_dtChild.Rows[j]["role"]);
                                dr[4] = !Convert.ToBoolean(_dtChild.Rows[j]["ingroup"]);
                                dt.Rows.Add(dr);

                                // Kiem tra xem chuc nang hien tai co chuyen muc cap 3 hay khong
                                if (prjBusinessLogic.UltilFunc.GetLatestID("T_Categorys", "Categorys_ID", "WHERE Category_ParrentID=" + _dtChild.Rows[j]["Categorys_ID"].ToString()) > 0)
                                {
                                    DataTable _dtChild1 = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByLoaibao]", new string[] { "@Parrent_ID", "@User_ID", "@LoaiID", "@LangID" }, new object[] { _dtChild.Rows[j]["Categorys_ID"].ToString(), UserID, LoaiID, LangID }).Tables[0];

                                    for (int j1 = 0; j1 < _dtChild1.Rows.Count; j1++)
                                    {
                                        dr = dt.NewRow();
                                        dr[0] = _dtChild1.Rows[j1]["Categorys_ID"].ToString();
                                        dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild1.Rows[j1]["Category_Name"].ToString();
                                        dr[2] = _dtChild1.Rows[j1]["Category_Order"].ToString();
                                        dr[3] = Convert.ToBoolean(_dtChild1.Rows[j1]["role"]);
                                        dr[4] = !Convert.ToBoolean(_dtChild1.Rows[j1]["ingroup"]);
                                        dt.Rows.Add(dr);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }

        public DataTable BindGridT_UserCategoryByUserSubject(int UserID, int LangID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("Categorys_ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Category_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Category_Order", typeof(string)));
            dt.Columns.Add(new DataColumn("Role_Cate", typeof(bool)));
            dt.Columns.Add(new DataColumn("Role_Group", typeof(bool)));
            DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByUserSubject]", new string[] { "@Parrent_ID", "@User_ID", "@LangID" }, new object[] { 0, UserID, LangID }).Tables[0];

            if (_dt.Rows.Count > 0)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {

                    dr = dt.NewRow();
                    dr[0] = _dt.Rows[i]["Categorys_ID"].ToString();
                    dr[1] = "<b>" + _dt.Rows[i]["Category_Name"].ToString();
                    dr[2] = _dt.Rows[i]["Category_Order"].ToString();
                    dr[3] = Convert.ToBoolean(_dt.Rows[i]["role"]);
                    dr[4] = !Convert.ToBoolean(_dt.Rows[i]["ingroup"]);
                    dt.Rows.Add(dr);
                    //}
                    //Kiem tra xem chuc nang co chuyen muc con hay khong
                    if (prjBusinessLogic.UltilFunc.GetLatestID("T_Categorys", "Categorys_ID", "WHERE Category_ParrentID=" + _dt.Rows[i]["Categorys_ID"].ToString()) > 0)
                    {
                        DataTable _dtChild = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByUserSubject]", new string[] { "@Parrent_ID", "@User_ID", "@LangID" }, new object[] { _dt.Rows[i]["Categorys_ID"].ToString(), UserID, LangID }).Tables[0];
                        if (_dtChild.Rows.Count > 0)
                        {
                            for (int j = 0; j < _dtChild.Rows.Count; j++)
                            {
                                dr = dt.NewRow();
                                dr[0] = _dtChild.Rows[j]["Categorys_ID"].ToString();
                                dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild.Rows[j]["Category_Name"].ToString();
                                dr[2] = _dtChild.Rows[j]["Category_Order"].ToString();
                                dr[3] = Convert.ToBoolean(_dtChild.Rows[j]["role"]);
                                dr[4] = !Convert.ToBoolean(_dtChild.Rows[j]["ingroup"]);
                                dt.Rows.Add(dr);

                                // Kiem tra xem chuc nang hien tai co chuyen muc cap 3 hay khong
                                if (prjBusinessLogic.UltilFunc.GetLatestID("T_Categorys", "Categorys_ID", "WHERE Category_ParrentID=" + _dtChild.Rows[j]["Categorys_ID"].ToString()) > 0)
                                {
                                    DataTable _dtChild1 = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserCategoryByUserSubject]", new string[] { "@Parrent_ID", "@User_ID", "@LangID" }, new object[] { _dtChild.Rows[j]["Categorys_ID"].ToString(), UserID, LangID }).Tables[0];

                                    for (int j1 = 0; j1 < _dtChild1.Rows.Count; j1++)
                                    {
                                        dr = dt.NewRow();
                                        dr[0] = _dtChild1.Rows[j1]["Categorys_ID"].ToString();
                                        dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild1.Rows[j1]["Category_Name"].ToString();
                                        dr[2] = _dtChild1.Rows[j1]["Category_Order"].ToString();
                                        dr[3] = Convert.ToBoolean(_dtChild1.Rows[j1]["role"]);
                                        dr[4] = !Convert.ToBoolean(_dtChild1.Rows[j1]["ingroup"]);
                                        dt.Rows.Add(dr);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return dt;
        }
        #endregion
        #region Phan Quyen Report
        public DataTable BindGridT_UserReportByUser(int UserID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Languages_ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Languages_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Role_Lang", typeof(bool)));
            dt.Columns.Add(new DataColumn("Role_Group", typeof(bool)));
            DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserLanguagesByUser]", new string[] { "@User_ID" }, new object[] { UserID }).Tables[0];
            if (_dt.Rows.Count > 0)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {

                    dr = dt.NewRow();
                    dr[0] = _dt.Rows[i]["Languages_ID"].ToString();
                    dr[1] = "<b>" + _dt.Rows[i]["Languages_Name"].ToString();
                    dr[2] = _dt.Rows[i]["Description"].ToString();
                    dr[3] = _dt.Rows[i]["Code"].ToString();
                    dr[4] = Convert.ToBoolean(_dt.Rows[i]["role"]);
                    dr[5] = !Convert.ToBoolean(_dt.Rows[i]["ingroup"]);
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion 
        #region Phan Quyen Languages
        public DataTable BindGridT__UserLanguagesByUser(int UserID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("NAME_REPORT", typeof(string)));
            //dt.Columns.Add(new DataColumn("Description", typeof(string)));
            // dt.Columns.Add(new DataColumn("Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Role", typeof(bool)));
            dt.Columns.Add(new DataColumn("Role_Group", typeof(bool)));
            //DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserLanguagesByUser]", new string[] { "@User_ID" }, new object[] { UserID }).Tables[0];
            DataTable _dt = GetT_UserReportByUser(UserID);
            if (_dt.Rows.Count > 0)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {

                    dr = dt.NewRow();
                    dr[0] = _dt.Rows[i]["ID"].ToString();
                    dr[1] = "<b>" + _dt.Rows[i]["NAME_REPORT"].ToString();
                    // dr[2] = _dt.Rows[i]["Description"].ToString();
                    // dr[3] = _dt.Rows[i]["Code"].ToString();
                    dr[2] = Convert.ToBoolean(_dt.Rows[i]["role"]);
                    dr[3] = !Convert.ToBoolean(_dt.Rows[i]["ingroup"]);
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion 
        #region Phan Quyen PAPER
        public DataTable BindGridT_UserPaperByUser(int UserID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add(new DataColumn("Paper_ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Paper_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Note", typeof(string)));
            dt.Columns.Add(new DataColumn("Role_Paper", typeof(bool)));
            dt.Columns.Add(new DataColumn("Role_Group", typeof(bool)));
            DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridT_UserPapersByUser]", new string[] { "@User_ID" }, new object[] { UserID }).Tables[0];
            if (_dt.Rows.Count > 0)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {

                    dr = dt.NewRow();
                    dr[0] = _dt.Rows[i]["Paper_ID"].ToString();
                    dr[1] = "<b>" + _dt.Rows[i]["Paper_Name"].ToString();
                    dr[2] = _dt.Rows[i]["Description"].ToString();
                    dr[3] = _dt.Rows[i]["Note"].ToString();
                    dr[4] = Convert.ToBoolean(_dt.Rows[i]["role"]);
                    dr[5] = !Convert.ToBoolean(_dt.Rows[i]["ingroup"]);
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        #endregion 

        #region ext - user-role
        public T_RolePermission GetRole4UserMenu(int User_ID, int Menu_ID)
        {
            //return HPCDataProvider.Instance().GetRole4UserMenu(User_ID, Menu_ID);
            return new clsResuftAPI<T_RolePermission>().GetOneObjSysbyUserMenu("api/Users/GetRole4UserMenu/", User_ID.ToString(), Menu_ID.ToString());
        }
        /*public T_Users GetUSERNAME(string UserName, int UserID)
        {
            return (T_Users)HPCDataProvider.Instance().GetObjectByCondition("T_Users", " UserName = N'" + UserName + "'  AND UserID <> " + UserID + "");
        }*/

        public T_Users GetUSERNAME(string UserName, int UserID)
        {
            // api/Users/GetUSERNAME? Username = { Username }&UserID={UserID
            return new clsResuftAPI<T_Users>().GetOneObjSysbyUser("api/Users/GetUSERNAME/", UserName, UserID.ToString());
        }




        public T_Users GetUserByUserName_ID(int userID)
        {
            try
            {
                //return HPCDataProvider.Instance().GetUserByUserName_ID(userID);
                return new clsResuftAPI<T_Users>().GetOneObjSysbyUserID("api/Users/GetUSERNAME/", userID.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        public DataTable GetAllUser_By_CatID(int CatID)
        {
            UltilFunc _untilDAL = new UltilFunc();
            ArrayList _arr = new ArrayList();
            DataTable _dt = new DataTable(); ;
            try
            {
                _dt = HPCDataProvider.Instance().GetStoreDataSet("[CMS_GetAll_User]", new string[] { "@CATID" }, new object[] { CatID }).Tables[0];
            }
            catch {; }
            return _dt;
        }

        public DataTable GetT_UserReportByUser(int p_USER_ID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_BindGridT_UserReportByUser".ToUpper(), new { P_USER_ID = p_USER_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getAllUserLogin(string id)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "sp_getAllUser".ToUpper(), new { P_LOGIN = id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getATMExp(string id)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "sp_getAllATM".ToUpper(), new { P_LOGIN = id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getATM(string pdate, string ptype)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "sp_getATM".ToUpper(), new { P_DATE = pdate, P_TYPE=ptype });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertATMExp(string name,string note,string type,string dateupdate)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "sp_UpdateAllATM".ToUpper(), new { P_NAME = name,P_NOTE=note,P_TYPE=type,P_DATEUPDATE=dateupdate });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleATMExp(string name, string type, string dateupdate)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "sp_DeleteAllATM".ToUpper(), new { P_NAME = name, P_TYPE = type, P_DATEUPDATE = dateupdate });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getLogExp(string id)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "sp_getAllExport".ToUpper(), new { P_LOGIN = id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getSendInbox(string id)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "sp_getSendInbox".ToUpper(), new { P_LOGIN = id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ViewMess(string _date,string _content)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "sp_ViewMess".ToUpper(), new { P_DATE = _date, P_CONTENT= _content });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ViewListCanCel(string _date, string _CALLSIGN)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("A_TEST_SEARCH", "sp_ViewFlightCancel".ToUpper(), new { P_DATE = _date, P_CALLSIGN = _CALLSIGN });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ViewMessAirport(string _date, string _content)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "sp_ViewMessAirport".ToUpper(), new { P_DATE = _date, P_CONTENT = _content });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteExportLog(string _date)
        {
            try
            {
                //HPCDataProvider.Instance().ExecStore("[CMS_DeleteT_UserMenuDynamic]", new string[] { "@WhereCondition" }, new object[] { strSql });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "sp_deleteAllExport".ToUpper(), new { P_LOGIN = _date });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetLevel(string _airport)
        {
            string _level="";
            try
            {
                              
                _level = new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "sp_LevelAirport".ToUpper(), new { P_AIRPORT = _airport }).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _level;
        }
    }
}
