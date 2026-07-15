using System;
using prjInfo;
using System.Data;
using HPCShareDLL;
using System.Collections.Generic;

namespace prjBusinessLogic
{

    public class GroupDAL
    {
        public List<T_Groups> GetAllGroup()
        {
            return new clsResuftAPI<T_Groups>().GetListObj("api/Groups/GetAllGroups");
        }
        public List<T_Groups> GetPageGroups(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<T_Groups>().GetListObj("api/Groups/GetPageGroups", pageSize, pageIndex, where);
        }
        public List<T_Groups> GetPageGroupExport(string where)
        {
            return new clsResuftAPI<T_Groups>().GetListObjExportData("api/Groups/GetPageGroupExport", where);
        }
        public T_Groups GetOneFromT_GroupsByID(double Group_ID)
        {
            try
            {
                //return (T_Groups)HPCDataProvider.Instance().GetObjectByID(Group_ID.ToString(),"T_Groups","Group_ID");
                List<T_Groups> lisObj = new List<T_Groups>();
                T_Groups Obj = new T_Groups();
                lisObj = new clsResuftAPI<T_Groups>().GetListObjExportData("api/Groups/GetPageGroupExport", " Group_ID = " + Group_ID.ToString() + "");

                if (lisObj != null) Obj = lisObj[0];
                else Obj = null;
                return Obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteFromT_GroupsByID(int Group_ID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_DeleteOneFromT_Groups]", new string[] { "@Group_ID" }, new object[] { Group_ID });
                new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "T_GROUPS_DELETE".ToUpper(), new { P_GROUP_ID = Group_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteFromT_GroupsDynamic(string WhereCondition)
        {
            try
            {
                 HPCDataProvider.Instance().ExecStore("Sp_DeleteT_GroupsDynamic",new string[]{"@WhereCondition"}, new object[]{WhereCondition});
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteFromT_GroupCategoryDynamic(string WhereCondition)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_DeleteFromT_GroupCategoryDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet BindGridT_Groups(int PageIndex, int PageSize, string WhereCondition)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet("[CMS_ListT_GroupsDynamic]", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });				
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataTable API_BindGridMenuByGroup(int ParrentId, int GroupID)
        {
            try
            {
                //return new clsResuftAPI<DataTable>().GetOneObjSys4Paramater("api/Users/BindGridMenuByUser/",new string[] { "Parrent_ID", "UserId" },new object[] { ParrentId, UserID } );
                return new clsResuftAPI().GetTableParamater("api/Groups/BindGridMenuByGroup/", ParrentId, GroupID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable BindGridMenuByGroup(int GroupID)
        {
           

            UltilFunc _untilDAL = new UltilFunc();
            DataTable dt = new DataTable();
            DataRow dr;
           

            dt.Columns.Add(new DataColumn("Menu_ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Menu_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Module_Order", typeof(string)));
            dt.Columns.Add(new DataColumn("Module_URL", typeof(string)));
            dt.Columns.Add(new DataColumn("Menu_Desc", typeof(string)));
            dt.Columns.Add(new DataColumn("Role_Menu", typeof(bool)));

            dt.Columns.Add(new DataColumn("R_Add", typeof(bool)));
            dt.Columns.Add(new DataColumn("R_Edit", typeof(bool)));
            dt.Columns.Add(new DataColumn("R_Del", typeof(bool)));
            dt.Columns.Add(new DataColumn("R_Pub", typeof(bool)));
            //DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridMenuByGroup]", new string[] { "@Parrent_ID", "@Group_ID" }, new object[] { 0, GroupID }).Tables[0];

            
            DataTable _dt = API_BindGridMenuByGroup(0, GroupID);


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
                    if (_dt.Rows[i]["R_Add"] != DBNull.Value)
                        dr[6] = Convert.ToBoolean(_dt.Rows[i]["R_Add"]);
                    else dr[6] = false;
                    if (_dt.Rows[i]["R_Edit"] != DBNull.Value)
                        dr[7] = Convert.ToBoolean(_dt.Rows[i]["R_Edit"]);
                    else dr[7] = false;
                    if (_dt.Rows[i]["R_Del"] != DBNull.Value)
                        dr[8] = Convert.ToBoolean(_dt.Rows[i]["R_Del"]);
                    else dr[8] = false;
                    if (_dt.Rows[i]["R_Pub"] != DBNull.Value)
                        dr[9] = Convert.ToBoolean(_dt.Rows[i]["R_Pub"]);
                    dt.Rows.Add(dr);
                    //Kiem tra xem chuc nang co chuyen muc con hay khong
                    if (prjBusinessLogic.UltilFunc.GetLatestID("T_Menus", "ID", "WHERE ParrentID=" + _dt.Rows[i].ItemArray[0].ToString()) > 0)
                    {
                        // DataTable _dtChild = _untilDAL.GetStoreDataSet("[CMS_BindGridMenuByGroup]", new string[] { "@Parrent_ID", "@Group_ID" }, new object[] { _dt.Rows[i].ItemArray[0], GroupID }).Tables[0];
                        DataTable _dtChild = API_BindGridMenuByGroup(Convert.ToInt32(_dt.Rows[i].ItemArray[0]), GroupID);
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

                                if (_dtChild.Rows[j]["R_Add"] != DBNull.Value)
                                    dr[6] = Convert.ToBoolean(_dtChild.Rows[j]["R_Add"]);
                                else dr[6] = false;
                                if (_dtChild.Rows[j]["R_Edit"] != DBNull.Value)
                                    dr[7] = Convert.ToBoolean(_dtChild.Rows[j]["R_Edit"]);
                                else dr[7] = false;
                                if (_dtChild.Rows[j]["R_Del"] != DBNull.Value)
                                    dr[8] = Convert.ToBoolean(_dtChild.Rows[j]["R_Del"]);
                                else dr[8] = false;
                                if (_dtChild.Rows[j]["R_Pub"] != DBNull.Value)
                                    dr[9] = Convert.ToBoolean(_dtChild.Rows[j]["R_Pub"]);
                                else dr[9] = false;
                                dt.Rows.Add(dr);

                                //// Kiem tra xem chuc nang hien tai co chuyen muc cap 3 hay khong
                                if (prjBusinessLogic.UltilFunc.GetLatestID("T_Menus", "ID", "WHERE ParrentID=" + _dtChild.Rows[j].ItemArray[0].ToString()) > 0)
                                {
                                    //DataTable _dtChild1 = _untilDAL.GetStoreDataSet("[CMS_BindGridMenuByGroup]", new string[] { "@Parrent_ID", "@Group_ID" }, new object[] { _dtChild.Rows[j].ItemArray[0], GroupID }).Tables[0];
                                    DataTable _dtChild1 = API_BindGridMenuByGroup(Convert.ToInt32(_dtChild.Rows[i].ItemArray[0]), GroupID);
                                    for (int j1 = 0; j1 < _dtChild1.Rows.Count; j1++)
                                    {
                                        dr = dt.NewRow();
                                        dr[0] = _dtChild1.Rows[j1]["ID"].ToString();
                                        dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild1.Rows[j1]["MenuName"].ToString();
                                        dr[2] = _dtChild1.Rows[j1]["MenuOrder"].ToString();
                                        dr[3] = _dtChild1.Rows[j1]["MenuURL"].ToString();
                                        dr[4] = _dtChild1.Rows[j1]["MenuDesc"].ToString();
                                        dr[5] = Convert.ToBoolean(_dtChild1.Rows[j1]["role"]);

                                        if (_dtChild1.Rows[j1]["R_Add"] != DBNull.Value)
                                            dr[6] = Convert.ToBoolean(_dtChild1.Rows[j1]["R_Add"]);
                                        else dr[6] = false;
                                        if (_dtChild1.Rows[j1]["R_Edit"] != DBNull.Value)
                                            dr[7] = Convert.ToBoolean(_dtChild1.Rows[j1]["R_Edit"]);
                                        else dr[7] = false;
                                        if (_dtChild1.Rows[j1]["R_Del"] != DBNull.Value)
                                            dr[8] = Convert.ToBoolean(_dtChild1.Rows[j1]["R_Del"]);
                                        else dr[8] = false;
                                        if (_dtChild1.Rows[j1]["R_Pub"] != DBNull.Value)
                                            dr[9] = Convert.ToBoolean(_dtChild1.Rows[j1]["R_Pub"]);
                                        else dr[9] = false;
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
        public void XoaChucnangNhomNguoidung(int ID)
        {
            //UltilFunc _lib = new UltilFunc();
            //_lib.ExecStore("[CMS_DeleteFromT_GroupMenuDynamic]", new string[] { "@ID" }, new object[] { ID });
            new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_DeleteGroupMenuDynamic".ToUpper(), new { P_GROUP_ID = ID });
            
        }
        public void XoaChucnangNhomNguoidungLanguages(int ID)
        {
            // UltilFunc _lib = new UltilFunc();
            //_lib.ExecStore("[CMS_DeleteFromT_GroupLanguagesDynamic]", new string[] { "@ID" }, new object[] { ID });
            new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_DeleteFromT_GroupReport".ToUpper(), new { P_GROUP_ID = ID });
        }
        public void XoaChucnangNhomNguoidungPaper(int ID)
        {
            UltilFunc _lib = new UltilFunc();
            _lib.ExecStore("[CMS_DeleteFromT_GroupPapersDynamic]", new string[] { "@ID" }, new object[] { ID });
        }
        public void InsertT_GroupMenu(int Menu_ID, int Group_ID, int _R_Edit, int _R_Del, int _R_Add, int _R_Pub)
        {
            // UltilFunc _lib = new UltilFunc();
            //_lib.ExecStore("[CMS_InsertT_GroupMenu]", new string[] { "@Menu_ID", "@Group_ID", "@R_Edit", "@R_Del", "@R_Add", "@R_Pub" }, new object[] { Menu_ID, Group_ID, _R_Edit, _R_Del, _R_Add, _R_Pub });
            new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_InsertT_GroupMenu".ToUpper(), new { P_GROUP_ID = Group_ID, P_MENU_ID = Menu_ID, P_R_EDIT = _R_Edit, P_R_DEL = _R_Del, P_R_ADD = _R_Add, P_R_PUB = _R_Pub });

        }
        public void InsertT_GroupReport(int ReportID, int Group_ID)
        {
           
            new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_InsertT_GroupReport".ToUpper(), new { P_REPORTID = ReportID, P_GROUP_ID = Group_ID });

        }
        public void InsertT_GroupLanguages(int LangID, int Group_ID)
        {
            UltilFunc _lib = new UltilFunc();
            _lib.ExecStore("[CMS_InsertT_GroupLanguages]", new string[] { "@Languages_ID", "@Group_ID" }, new object[] { LangID, Group_ID });
            
        }
        public void InsertT_GroupPapers(int PID, int Group_ID)
        {
            UltilFunc _lib = new UltilFunc();
            _lib.ExecStore("[CMS_InsertT_GroupPapers]", new string[] { "@Paper_ID", "@Group_ID" }, new object[] { PID, Group_ID });
        }
        public void InsertT_GroupCategory(int Categorys_ID, int Group_ID)
        {
            UltilFunc _lib = new UltilFunc();
            _lib.ExecStore("[CMS_InsertT_GroupCategory]", new string[] { "@Group_ID", "@Categorys_ID" }, new object[] { Group_ID, Categorys_ID });
        }
        public string Insert_T_Group(T_Groups obj)
        {
            //return HPCDataProvider.Instance().InsertObjectReturn(obj, "[CMS_InsertT_Group]");
            return new clsResuftAPI<T_Groups>().InsertReturnId(obj, "api/Groups/CreateGroups");
        }
        public T_Groups GetGroupName(string groupName,int groupId)
        {
            //return (T_Groups)HPCDataProvider.Instance().GetObjectByCondition("T_Groups", " Group_Name = N'" + groupName + "' AND Group_ID <> " + groupId + "");
            List<T_Groups> lisObj = new List<T_Groups>();
            T_Groups Obj = new T_Groups();
            lisObj =  new clsResuftAPI<T_Groups>().GetListObjExportData("api/Groups/GetPageGroupExport", " Group_Name = N'" + groupName + "' AND Group_ID <> " + groupId + "");

            if (lisObj != null) Obj = lisObj[0];
            else Obj = null;
            return Obj;
        }
        public T_Groups GetByGroupName_ID(int groupID)
        {
            try
            {
                return HPCDataProvider.Instance().GetGroupByGroupName_ID(groupID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Sp_AutoInsertCategoryFromGroup(int Categorys_ID, int Group_ID, DateTime datecreate)
        {
            UltilFunc _lib = new UltilFunc();
            _lib.ExecStore("[CMS_AutoInsertCategoryFromGroup]", new string[] { "@Categorys_ID", "@Group_ID", "@DateCreated" }, new object[] { Categorys_ID, Group_ID, datecreate });
        }
        public DataTable GetT_GroupReportByGroupID(int p_Group_ID)
        {
            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_UsersDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });
                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_BindGridReportByGroup".ToUpper(), new { P_GROUP_ID = p_Group_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable BindGridReportByGroup(int Group_ID)
        {
            return GetT_GroupReportByGroupID(Group_ID);
            //UltilFunc _untilDAL = new UltilFunc();
            //DataTable dt = new DataTable();
            //DataRow dr;

            //dt.Columns.Add(new DataColumn("ID", typeof(int)));
            //dt.Columns.Add(new DataColumn("NAME_REPORT", typeof(string)));
            //dt.Columns.Add(new DataColumn("Role", typeof(bool)));
            //// DataTable _dt = _untilDAL.GetStoreDataSet("[CMS_BindGridLanguagesByGroup]", new string[] { "@Group_ID" }, new object[] { Group_ID }).Tables[0];
            //DataTable _dt = GetT_GroupReportByGroupID(Group_ID);
            //if (_dt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < _dt.Rows.Count; i++)
            //    {
            //        dr = dt.NewRow();
            //        dr[0] = _dt.Rows[i]["ID"].ToString();
            //        dr[1] = "<b>" + _dt.Rows[i]["NAME_REPORT"].ToString();
            //        dr[2] = Convert.ToBoolean(_dt.Rows[i]["Role"]);
            //        dt.Rows.Add(dr);
            //    }
            //}
            //return dt;
        }
    }

}
