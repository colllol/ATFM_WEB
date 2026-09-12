using System;

using System.Collections.Generic;
using System.Text;
using prjInfo;
using HPCShareDLL;
using System.Data;
namespace prjBusinessLogic
{
    public class MenuDAL
    {

        public void UpdateStatusMenu (int Menu_ID)
        {
            try
            {
               new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_UPDATESTATUSMENU", new { P_MENU_ID = Menu_ID });
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public List<T_Menus> GetAllMenus()
        {
            return new clsResuftAPI<T_Menus>().GetListObj("api/Menus/GetAllMenus");
        }
        public List<T_Menus> GetPageMenus(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<T_Menus>().GetListObj("api/Menus/GetPageMenus", pageSize, pageIndex, where);
        }
        public T_Menus GetOneFromT_MenusByID(int ID)
        {
            // api/Users/GetUSERNAME? Username = { Username }&UserID={UserID
            return new clsResuftAPI<T_Menus>().GetOneObj(ID.ToString(), "api/Menus/GetByIdMenus/");
        }
        //public T_Menus GetOneFromT_MenusByID(Int32 ID)
        //{
        //    try
        //    {
        //        return (T_Menus)HPCDataProvider.Instance().GetObjectByID(ID.ToString(), "T_Menus", "ID");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public void DeleteFromT_MenusByID(Int32 ID)
        {

            try
            {
                // HPCDataProvider.Instance().ExecStore("[CMS_DeleteOneFromT_Menus]", new string[] { "@ID" }, new object[] { ID });
                bool _done;
                //_done = new clsResuftAPI<T_Menus>().DeleteObj(ID.ToString(), "api/Ctry/DeleteCtry/");
                _done = new clsResuftAPI<T_Menus>().DeleteObj(ID.ToString(), "api/Menus/DeleteMenus/"); 
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateFromT_MenusDynamic(string WhereCondition)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore("[CMS_UpdateT_MenusDynamic]", new string[] { "@WhereCondition" }, new object[] { WhereCondition });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet BindGridT_Menus(int PageIndex, int PageSize, string WhereCondition)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet("[CMS_ListT_MenusDynamic]", new string[] { "@PageIndex", "@PageSize", "@where" }, new object[] { PageIndex, PageSize, WhereCondition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataView BindGridMenu(DataTable _dt)
        {
            try
            {
                prjBusinessLogic.UltilFunc _untilDAL = new prjBusinessLogic.UltilFunc();
                DataTable dt = new DataTable();
                DataRow dr;

                dt.Columns.Add(new DataColumn("ID", typeof(int)));
                dt.Columns.Add(new DataColumn("MenuName", typeof(string)));
                dt.Columns.Add(new DataColumn("MenuOrder", typeof(string)));
                dt.Columns.Add(new DataColumn("MenuURL", typeof(string)));
                dt.Columns.Add(new DataColumn("Menu_Desc", typeof(string)));
                dt.Columns.Add(new DataColumn("ActiveSync", typeof(string)));
                dt.Columns.Add(new DataColumn("ActiveSyncImages", typeof(string)));
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr[0] = _dt.Rows[i].ItemArray[0].ToString();
                        dr[1] = "<b>" + _dt.Rows[i].ItemArray[1].ToString();
                        dr[2] = _dt.Rows[i].ItemArray[2].ToString();
                        dr[3] = _dt.Rows[i]["MenuURL"].ToString();
                        dr[4] = _dt.Rows[i]["MenuDesc"].ToString();
                        dr[5] = _dt.Rows[i]["ActiveSync"].ToString();
                        dr[6] = _dt.Rows[i]["ActiveSyncImages"].ToString();
                        dt.Rows.Add(dr);
                        //Kiem tra xem chuc nang co chuyen muc con hay khong
                        if (prjBusinessLogic.UltilFunc.GetLatestID("T_Menus", "ID", "WHERE ParrentID=" + _dt.Rows[i].ItemArray[0].ToString()) > 0)
                        {
                            DataTable _dtChild = _untilDAL.GetDataSet("T_Menus", "*", " ParrentID=" + _dt.Rows[i].ItemArray[0].ToString() + " order by MenuOrder").Tables[0];
                            if (_dtChild.Rows.Count > 0)
                            {
                                for (int j = 0; j < _dtChild.Rows.Count; j++)
                                {
                                    dr = dt.NewRow();
                                    dr[0] = _dtChild.Rows[j].ItemArray[0].ToString();
                                    dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild.Rows[j].ItemArray[1].ToString();
                                    dr[2] = _dtChild.Rows[j].ItemArray[2].ToString();
                                    dr[3] = _dtChild.Rows[j]["MenuURL"].ToString();
                                    dr[4] = _dtChild.Rows[j]["MenuDesc"].ToString();
                                    dr[5] = _dtChild.Rows[j]["ActiveSync"].ToString();
                                    dr[6] = _dtChild.Rows[j]["ActiveSyncImages"].ToString();
                                    dt.Rows.Add(dr);
                                    // Kiem tra xem co chuc nang con cap 3 hay khong
                                    if (prjBusinessLogic.UltilFunc.GetLatestID("T_Menus", "Menu_ID", "WHERE Parrent_ID=" + _dtChild.Rows[j].ItemArray[0].ToString()) > 0)
                                    {
                                        DataTable _dtChild1 = _untilDAL.GetDataSet("T_Menus", "*", " Parrent_ID=" + _dtChild.Rows[j].ItemArray[0].ToString() + " order by Module_Order").Tables[0];
                                        for (int j1 = 0; j1 < _dtChild1.Rows.Count; j1++)
                                        {
                                            dr = dt.NewRow();
                                            dr[0] = _dtChild1.Rows[j1].ItemArray[0].ToString();
                                            dr[1] = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild1.Rows[j1].ItemArray[1].ToString();
                                            dr[2] = _dtChild1.Rows[j1].ItemArray[2].ToString();
                                            dr[3] = _dtChild1.Rows[j1].ItemArray[3].ToString();
                                            dr[4] = _dtChild1.Rows[j1]["Menu_Desc"].ToString();
                                            dr[5] = _dtChild1.Rows[j1]["ActiveSync"].ToString();
                                            dr[6] = _dtChild1.Rows[j1]["ActiveSyncImages"].ToString();
                                            dt.Rows.Add(dr);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                DataView dv = new DataView(dt);
                return dv;
            }
            catch (Exception ex) { throw ex; }
        }

        public int Insert_T_Menus(T_Menus obj)
        {
            return HPCDataProvider.Instance().InsertObjectReturn(obj, "[CMS_InsertT_Menus]");
        }

        public string Insert_T_MenusOrc(T_Menus obj)
        {
            return new clsResuftAPI<T_Menus>().InsertReturnId(obj, "api/Menus/CreateMenus");
        }


    }
}
