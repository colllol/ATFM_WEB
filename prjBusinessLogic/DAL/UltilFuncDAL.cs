using System;
using prjInfo;
using HPCShareDLL;
using System.Data;
using HPCServerDataAccess;
using System.Web.UI.WebControls;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Resources;
using System.Collections;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using prjBusinessLogic.DAL;
using System.Net;
using HtmlAgilityPack;
using System.Globalization;

namespace prjBusinessLogic
{
    public class UltilFunc
    {
        public DataSet GetDataSet(string TableName, string ColumnList, string Where)
        {
            try
            {
                return HPCDataProvider.Instance().GetDataSet(TableName, ColumnList, Where);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetDataTable(string TableName, string ColumnList, string Where)
        {
            try
            {
                //return HPCDataProvider.Instance().GetDataSet(TableName, ColumnList, Where);

                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_GetColumnValues".ToUpper(), new { P_TABLENAME = TableName, P_COLUMNLIST= ColumnList, P_WHERE= Where });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetLatestTable(string TableName, string IDFieldName, string Condition)
        {
            try
            {
                //return HPCDataProvider.Instance().GetDataSet(TableName, ColumnList, Where);

                return new clsResuftAPI().GetPostTableApiExtension("SYS_PKG", "CMS_GetLatestID".ToUpper(), new { P_TABLENAME = TableName, P_IDFIELDNAME = IDFieldName, P_CONDITION = Condition });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetColumnValues(string TableName, string ColumnName, string Where)
        {
            try
            {

                return HPCDataProvider.Instance().GetColumnValues(TableName, ColumnName, Where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetColumnValuesOne(string TableName, string ColumnName, string Where)
        {
            DataTable _dt = new DataTable();
            try
            {
                _dt = HPCDataProvider.Instance().GetStoreDataSet("Sp_GetColumnValues", new string[] { "@TableName", "@ColumnList", "@Where" }, new object[] { TableName, ColumnName, Where }).Tables[0];
                if (_dt != null && _dt.Rows.Count > 0)
                    return int.Parse(_dt.Rows[0][0].ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStoreDataSet(string StoreName, string[] param1, object[] value)
        {
            try
            {
                return HPCDataProvider.Instance().GetStoreDataSet(StoreName, param1, value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExecStore(string StoreName, string[] param1, object[] value)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore(StoreName, param1, value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExecStore(string StoreName)
        {
            try
            {
                HPCDataProvider.Instance().ExecStore(StoreName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExecSql(string sql)
        {
            try
            {
                HPCDataProvider.Instance().ExecSql(sql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet ExecSqlDataSet(string sql)
        {
            try
            {
                return HPCDataProvider.Instance().ExecSqlDataSet(sql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void LoadCombo(DropDownList cbo, string strTableName, string strTextField, string strValueField, string strBlank)
        {
            cbo.DataSource = HPCDataProvider.Instance().ExecSqlDataSet("SELECT * FROM " + strTableName);
            cbo.DataTextField = strTextField;
            cbo.DataValueField = strValueField;
            cbo.DataBind();
            if (!(strBlank == null))
            {
                ListItem item = new ListItem();
                item.Text = strBlank;
                item.Value = "";
                cbo.Items.Insert(0, item);
            }
        }
        public static void LoadCombo(DropDownList cbo, string strTableName, string strTextField, string strValueField, string valField, string strBlank)
        {
            cbo.DataSource = HPCDataProvider.Instance().ExecSqlDataSet("SELECT * FROM " + strTableName + " WHERE " + valField);
            cbo.DataTextField = strTextField;
            cbo.DataValueField = strValueField;
            cbo.DataBind();
            if (!(strBlank == null))
            {
                ListItem item = new ListItem();
                item.Text = strBlank;
                item.Value = "";
                cbo.Items.Insert(0, item);
            }
        }
        public static int GetLatestID(string TableName, string IDFieldName)
        {
            prjBusinessLogic.UltilFunc _untilFuncDAL = new prjBusinessLogic.UltilFunc();
            try
            {
                DataSet ds = _untilFuncDAL.GetStoreDataSet("CMS_GetLatestID", new string[] { "@TableName", "@IDFieldName" }, new object[] { TableName, IDFieldName });
                return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray.GetValue(0));
            }
            catch
            {
                return 0;
            }
        }
        public static int SPGet_ChuyenMucDefault()
        {
            prjBusinessLogic.UltilFunc _untilFuncDAL = new prjBusinessLogic.UltilFunc();
            try
            {
                DataSet ds = _untilFuncDAL.GetStoreDataSet("[CMS_Get_ChuyenMucDefault]", new string[] { }, new object[] { });
                return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray.GetValue(0));
            }
            catch
            {
                return 0;
            }
        }
        public static int GetLatestID(string TableName, string IDFieldName, string Condition)
        {

            prjBusinessLogic.UltilFunc _untilFuncDAL = new prjBusinessLogic.UltilFunc();
            DataTable _dt = _untilFuncDAL.GetLatestTable(TableName, IDFieldName, Condition);
            try
            {
                if (Int32.Parse(_dt.Rows[0][0].ToString()) > 0)
                    return 1;
                else
                    return 0;
                //DataSet ds = _untilFuncDAL.GetStoreDataSet("CMS_GetLatestID", new string[] { "@TableName", "@IDFieldName", "@Condition" }, new object[] { TableName, IDFieldName, Condition });
                // return Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray.GetValue(0));
                //return Int32.Parse(new clsResuftAPI().GetPostValueApiExtension("SYS_PKG", "CMS_GetLatestID".ToUpper(), new { P_TABLENAME = TableName, P_IDFIELDNAME = IDFieldName, P_CONDITION = Condition }).ToString());
            }
            catch
            {
                return 0;
            }
        }

        public static void BindCombox(System.Web.UI.WebControls.DropDownList oObject, string mField_ID, string mField_Value, string mTable)
        {
            BindCombox(oObject, mField_ID, mField_Value, mTable, "", "", "");
        }

        public static void BindCombox(System.Web.UI.WebControls.DropDownList oObject, string mField_ID, string mField_Value, string mTable, string sWhere)
        {
            BindCombox(oObject, mField_ID, mField_Value, mTable, sWhere, "", "");
        }
        public static void BindCombox(System.Web.UI.WebControls.DropDownList oObject, string mField_ID, string mField_Value, string mTable, string sWhere, string sText)
        {
            BindCombox(oObject, mField_ID, mField_Value, mTable, sWhere, sText, "");
        }
        public static void BindCombox(System.Web.UI.WebControls.DropDownList oObject, string mField_ID, string mField_Value, string mTable, string sWhere, string sText, string Parrent_ID)
        {
            UltilFunc _untilFuncDAL = new UltilFunc();
            DataTable _dt;
            DataTable _dtChild;
            DataTable _dtThree;
            int i = 0;
            oObject.DataSource = null;
            oObject.DataBind();
            if (sText != "")
            {
                if (sText != " ")
                {
                    oObject.Items.Add("<<--" + sText + "-->>");
                    oObject.Items[i].Value = "0"; ++i;
                }
            }
            else
            {
                oObject.Items.Add("<<-- Chọn giá trị -->>");
                oObject.Items[i].Value = "0"; ++i;
            }
            if (Parrent_ID != "")
            {
                string not_childrent = "(0";
                string not_inParent = "(0";
                // _dt = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = 0 ").Tables[0];
                _dt = _untilFuncDAL.GetDataTable(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = 0 ");
                

                try
                {
                    if (_dt.Rows.Count > 0)
                    {
                        for (int n = 0; n < _dt.Rows.Count; n++)
                        {
                            oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                            oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                            i += 1;
                            if (prjBusinessLogic.UltilFunc.GetLatestID(mTable, Parrent_ID, "WHERE " + Parrent_ID + "=" + _dt.Rows[n][mField_ID]) > 0)
                            {
                                //_dtChild = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dt.Rows[n][mField_ID] + " ORDER BY " + mField_ID).Tables[0];
                                _dtChild = _untilFuncDAL.GetDataTable(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dt.Rows[n][mField_ID] + " ORDER BY " + mField_ID);
                                if (_dtChild.Rows.Count > 0)
                                {
                                    for (int m = 0; m < _dtChild.Rows.Count; m++)
                                    {
                                        not_inParent = not_inParent + "," + _dtChild.Rows[m][mField_ID].ToString();

                                        oObject.Items.Add(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") + _dtChild.Rows[m][mField_Value].ToString());
                                        //oObject.Items.Add(HttpContext.Current.Server.UrlDecode("&nbsp;&nbsp;&nbsp;&nbsp;" + _dtChild.Rows[m][mField_Value].ToString()));
                                        oObject.Items[i].Value = _dtChild.Rows[m][mField_ID].ToString();
                                        i += 1;
                                        if (prjBusinessLogic.UltilFunc.GetLatestID(mTable, Parrent_ID, "WHERE " + Parrent_ID + "=" + _dtChild.Rows[m][mField_ID]) > 0)
                                        {
                                            //_dtThree = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dtChild.Rows[m][mField_ID] + " ORDER BY " + mField_ID).Tables[0];
                                            _dtThree = _untilFuncDAL.GetDataTable(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dtChild.Rows[m][mField_ID] + " ORDER BY " + mField_ID);
                                            if (_dtThree.Rows.Count > 0)
                                            {
                                                for (int k = 0; k < _dtThree.Rows.Count; k++)
                                                {
                                                    not_childrent = not_childrent + "," + _dtThree.Rows[k][mField_ID].ToString();

                                                    oObject.Items.Add(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + _dtThree.Rows[k][mField_Value].ToString());
                                                    oObject.Items[i].Value = _dtThree.Rows[k][mField_ID].ToString();
                                                    i += 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                if (mTable.Trim() == "T_Categorys")
                {
                    not_childrent = not_childrent + ")";
                    not_inParent = not_inParent + ")";
                    string not_in1 = "(0";
                    _dt = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " <> 0 "
                       + " and Categorys_ID not in " + not_inParent + " ").Tables[0];

                    try
                    {
                        if (_dt.Rows.Count > 0)
                        {
                            for (int n = 0; n < _dt.Rows.Count; n++)
                            {
                                not_in1 = not_in1 + "," + _dt.Rows[n][mField_ID].ToString();
                                oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                                oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                                i += 1;
                                if (prjBusinessLogic.UltilFunc.GetLatestID(mTable, Parrent_ID, "WHERE " + Parrent_ID + "=" + _dt.Rows[n][mField_ID]) > 0)
                                {
                                    _dtThree = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dt.Rows[n][mField_ID] + " ORDER BY " + mField_ID).Tables[0];
                                    if (_dtThree.Rows.Count > 0)
                                    {
                                        for (int k = 0; k < _dtThree.Rows.Count; k++)
                                        {
                                            not_in1 = not_in1 + "," + _dtThree.Rows[k][mField_ID].ToString();
                                            oObject.Items.Add(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + _dtThree.Rows[k][mField_Value].ToString());
                                            oObject.Items[i].Value = _dtThree.Rows[k][mField_ID].ToString();
                                            i += 1;
                                        }
                                    }
                                }
                            }
                        }
                        not_in1 = not_in1 + ")";
                    }
                    catch { ;}
                    _dt = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value,
                        " " + sWhere + " AND " + Parrent_ID + " <> 0 " + " and Categorys_ID not in " + not_inParent +
                         " and Categorys_ID not in " + not_childrent +
                          " and Categorys_ID not in " + not_in1 + " ").Tables[0];
                    try
                    {
                        if (_dt.Rows.Count > 0)
                        {
                            for (int n = 0; n < _dt.Rows.Count; n++)
                            {
                                oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                                oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                                i += 1;
                            }
                        }
                    }
                    catch { ;}
                }

            }
            else
            {
                //_dt = _untilFuncDAL.GetDataSet(mTable, mField_ID + "," + mField_Value, " " + sWhere).Tables[0];
                _dt = _untilFuncDAL.GetDataTable(mTable, mField_ID + "," + mField_Value, " " + sWhere);
                try
                {
                    if (_dt.Rows.Count > 0)
                    {
                        for (int n = 0; n < _dt.Rows.Count; n++)
                        {
                            oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                            oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                            i += 1;
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
        public static void BindCheckBoxList(System.Web.UI.WebControls.CheckBoxList oObject, string mField_ID, string mField_Value, string mTable, string sWhere, string Parrent_ID, string OrderBy)
        {
            UltilFunc _untilFuncDAL = new UltilFunc();
            DataTable _dt;
            DataTable _dtChild;
            DataTable _dtThree;
            int i = 0;
            oObject.DataSource = null;
            oObject.DataBind();

            if (Parrent_ID != "")
            {

                _dt = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = 0 " + OrderBy).Tables[0];
                try
                {
                    if (_dt.Rows.Count > 0)
                    {
                        for (int n = 0; n < _dt.Rows.Count; n++)
                        {
                            oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                            oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                            i += 1;
                            if (prjBusinessLogic.UltilFunc.GetLatestID(mTable, Parrent_ID, "WHERE " + Parrent_ID + "=" + _dt.Rows[n][mField_ID]) > 0)
                            {
                                _dtChild = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dt.Rows[n][mField_ID] + OrderBy).Tables[0];
                                if (_dtChild.Rows.Count > 0)
                                {
                                    for (int m = 0; m < _dtChild.Rows.Count; m++)
                                    {
                                        oObject.Items.Add(new ListItem(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") + _dtChild.Rows[m][mField_Value].ToString(), _dtChild.Rows[m][mField_ID].ToString()));
                                        //oObject.Items[i].Value = _dtChild.Rows[m][mField_ID].ToString();
                                        i += 1;
                                        if (prjBusinessLogic.UltilFunc.GetLatestID(mTable, Parrent_ID, "WHERE " + Parrent_ID + "=" + _dtChild.Rows[m][mField_ID]) > 0)
                                        {
                                            _dtThree = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dtChild.Rows[m][mField_ID] + OrderBy).Tables[0];
                                            if (_dtThree.Rows.Count > 0)
                                            {
                                                for (int k = 0; k < _dtThree.Rows.Count; k++)
                                                {
                                                    oObject.Items.Add(new ListItem(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + _dtThree.Rows[k][mField_Value].ToString(), _dtThree.Rows[k][mField_ID].ToString()));
                                                    //oObject.Items[i].Value = _dtThree.Rows[k][mField_ID].ToString();
                                                    i += 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                _dt = _untilFuncDAL.GetDataSet(mTable, mField_ID + "," + mField_Value, " " + sWhere + " " + OrderBy).Tables[0];
                try
                {
                    if (_dt.Rows.Count > 0)
                    {
                        for (int n = 0; n < _dt.Rows.Count; n++)
                        {
                            oObject.Items.Add(new ListItem(_dt.Rows[n][mField_Value].ToString(), _dt.Rows[n][mField_ID].ToString()));
                            //oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                            i += 1;
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static void BindCombox(System.Web.UI.WebControls.DropDownList oObject, string mField_ID, string mField_Value, string mTable, string sWhere, string sText, string Parrent_ID, string OrderBy)
        {
            UltilFunc _untilFuncDAL = new UltilFunc();
            DataTable _dt;
            DataTable _dtChild;
            DataTable _dtThree;
            int i = 0;
            oObject.DataSource = null;
            oObject.DataBind();

            if (sText != "")
            {
                if (sText != " ")
                {
                    oObject.Items.Add("<<--" + sText + "-->>");
                    oObject.Items[i].Value = "0"; ++i;
                }
            }
            else
            {
                oObject.Items.Add("<<--Chọn giá trị-->>");
                oObject.Items[i].Value = "0"; ++i;
            }
            if (Parrent_ID != "")
            {
                string not_childrent = "(0";
                string not_inParent = "(0";
                //EDIT Phan khong co chuyen muc cha
                //DataTable _dtTemp = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " " + OrderBy).Tables[0];
                _dt = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = 0 " + OrderBy).Tables[0];
                //if (_dt.Rows.Count == 0 && _dtTemp.Rows.Count > 0) // Edit Phan khong co chuyen muc cha
                //    _dt = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " " + OrderBy).Tables[0];
                try
                {
                    if (_dt.Rows.Count > 0)
                    {
                        for (int n = 0; n < _dt.Rows.Count; n++)
                        {
                            oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                            oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                            i += 1;
                            if (prjBusinessLogic.UltilFunc.GetLatestID(mTable, Parrent_ID, "WHERE " + Parrent_ID + "=" + _dt.Rows[n][mField_ID]) > 0)
                            {
                                _dtChild = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dt.Rows[n][mField_ID] + OrderBy).Tables[0];
                                if (_dtChild.Rows.Count > 0)
                                {
                                    for (int m = 0; m < _dtChild.Rows.Count; m++)
                                    {
                                        not_inParent = not_inParent + "," + _dtChild.Rows[m][mField_ID].ToString();
                                        oObject.Items.Add(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") + _dtChild.Rows[m][mField_Value].ToString());
                                        oObject.Items[i].Value = _dtChild.Rows[m][mField_ID].ToString();
                                        i += 1;
                                        if (prjBusinessLogic.UltilFunc.GetLatestID(mTable, Parrent_ID, "WHERE " + Parrent_ID + "=" + _dtChild.Rows[m][mField_ID]) > 0)
                                        {
                                            _dtThree = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dtChild.Rows[m][mField_ID] + OrderBy).Tables[0];
                                            if (_dtThree.Rows.Count > 0)
                                            {
                                                for (int k = 0; k < _dtThree.Rows.Count; k++)
                                                {
                                                    not_childrent = not_childrent + "," + _dtThree.Rows[k][mField_ID].ToString();
                                                    oObject.Items.Add(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + _dtThree.Rows[k][mField_Value].ToString());
                                                    oObject.Items[i].Value = _dtThree.Rows[k][mField_ID].ToString();
                                                    i += 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }
                //    if (mTable.Trim() == "T_Categorys")
                //    {
                //        not_childrent = not_childrent + ")";
                //        not_inParent = not_inParent + ")";
                //        string not_in1 = "(0";
                //        _dt = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " <> 0 "
                //           + " and Categorys_ID not in " + not_inParent + " " + OrderBy).Tables[0];

                //        try
                //        {
                //            if (_dt.Rows.Count > 0)
                //            {
                //                for (int n = 0; n < _dt.Rows.Count; n++)
                //                {
                //                    not_in1 = not_in1 + "," + _dt.Rows[n][mField_ID].ToString();
                //                    oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                //                    oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                //                    i += 1;
                //                    if (prjBusinessLogic.UltilFunc.GetLatestID(mTable, Parrent_ID, "WHERE " + Parrent_ID + "=" + _dt.Rows[n][mField_ID]) > 0)
                //                    {
                //                        _dtThree = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value, " " + sWhere + " AND " + Parrent_ID + " = " + _dt.Rows[n][mField_ID] + " ORDER BY " + mField_ID).Tables[0];
                //                        if (_dtThree.Rows.Count > 0)
                //                        {
                //                            for (int k = 0; k < _dtThree.Rows.Count; k++)
                //                            {
                //                                not_in1 = not_in1 + "," + _dtThree.Rows[k][mField_ID].ToString();
                //                                oObject.Items.Add(HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + _dtThree.Rows[k][mField_Value].ToString());
                //                                oObject.Items[i].Value = _dtThree.Rows[k][mField_ID].ToString();
                //                                i += 1;
                //                            }
                //                        }
                //                    }
                //                }
                //            }
                //            not_in1 = not_in1 + ")";
                //        }
                //        catch { ;}
                //        _dt = _untilFuncDAL.GetDataSet(mTable, ' ' + mField_ID + "," + mField_Value,
                //            " " + sWhere + " AND " + Parrent_ID + " <> 0 " + " and Categorys_ID not in " + not_inParent +
                //             " and Categorys_ID not in " + not_childrent +
                //              " and Categorys_ID not in " + not_in1 + " " + OrderBy).Tables[0];
                //        try
                //        {
                //            if (_dt.Rows.Count > 0)
                //            {
                //                for (int n = 0; n < _dt.Rows.Count; n++)
                //                {
                //                    oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                //                    oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                //                    i += 1;
                //                }
                //            }
                //        }
                //        catch { ;}
                //    }
                //}
                //else
                //{
                //    _dt = _untilFuncDAL.GetDataSet(mTable, mField_ID + "," + mField_Value, " " + sWhere + " " + OrderBy).Tables[0];
                //    try
                //    {
                //        if (_dt.Rows.Count > 0)
                //        {
                //            for (int n = 0; n < _dt.Rows.Count; n++)
                //            {
                //                oObject.Items.Add(_dt.Rows[n][mField_Value].ToString());
                //                oObject.Items[i].Value = _dt.Rows[n][mField_ID].ToString();
                //                i += 1;
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //        throw ex;
                //    }
            }
        }
        public static string Message(string str)
        {
            return "<script>alert('" + str + "');</script>";
        }
        public static int GetIndexControl(System.Web.UI.WebControls.DropDownList sControl, string iValue)
        {
            int iCount;
            int retVal = 0;
            iCount = sControl.Items.Count;
            for (int i = 0; i <= iCount - 1; i++)
            {
                if (sControl.Items[i].Value == iValue)
                {
                    return i;
                }
            }
            return retVal;
        }
        public static int GetIndexControlOne(System.Web.UI.WebControls.DropDownList sControl, string iValue)
        {
            int iCount;
            int retVal = 0;
            iCount = sControl.Items.Count;
            for (int i = 0; i <= iCount - 1; i++)
            {
                if (sControl.Items[i].Value == iValue)
                {
                    retVal = i;
                    goto exitForStatement0;
                }
            }
        exitForStatement0: ;
            return retVal;
        }
        public static string ApplicationPath()
        {
            return ConfigurationManager.AppSettings["ApplicationPath"];
        }
        public static string PhongVien()
        {
            return ConfigurationManager.AppSettings["PhongVien"];
        }
        public static string BientapVien()
        {
            return ConfigurationManager.AppSettings["BienTap"];
        }
        public static void BindComboboxYears(System.Web.UI.WebControls.DropDownList sControl, int iValue)
        {
            sControl.Items.Add(new ListItem("-- Chọn -- ", "0", true));
            for (int i = 2003; i <= iValue; i++)
            {
                sControl.Items.Add(i.ToString());
            }
        }
        public static void BindComboboxMonth(System.Web.UI.WebControls.DropDownList sControl, int iValue)
        {
            sControl.Items.Add(new ListItem("-- Chọn -- ", "0", true));
            for (int i = 1; i <= iValue; i++)
            {
                sControl.Items.Add(i.ToString());
            }
        }
        public T_Languages GetLanguageNameBy_ID(int ID)
        {
            try
            {
                return HPCDataProvider.Instance().GetLanguagesNameBy_ID(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static T_Languages GetLanguage(int ID)
        {
            try
            {
                return HPCDataProvider.Instance().GetLanguagesNameBy_ID(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static T_Categorys GetCategory(int ID)
        //{
        //    try
        //    {
        //        return HPCDataProvider.Instance().GetCategoryNameBy_ID(ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public static string GetCategoryName(Object ID)
        //{
        //    string str = "";
        //    try
        //    {
        //        if (prjBusinessLogic.UltilFunc.GetCategory(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            //str = prjBusinessLogic.UltilFunc.GetCategory(Convert.ToInt32(ID)).Category_Name.ToString();
        //            str = HPCDataProvider.Instance().GetStoreDataSet("[CMS_GetCategoryNameAll]", new string[] { "@CatID" }, new object[] { ID }).Tables[0].Rows[0][0].ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string GetCategoryNameByCateID(Object ID)
        //{
        //    string str = "";
        //    try
        //    {
        //        if (prjBusinessLogic.UltilFunc.GetCategory(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            str = prjBusinessLogic.UltilFunc.GetCategory(Convert.ToInt32(ID)).Category_Name.ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        public static string GetLanguageName(Object ID)
        {
            string str = "";
            try
            {
                if (prjBusinessLogic.UltilFunc.GetLanguage(Convert.ToInt32(ID)) == null)
                    str = "";
                else
                    str = prjBusinessLogic.UltilFunc.GetLanguage(Convert.ToInt32(ID)).Languages_Name.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        //public static string CheckLayout(Object ID)
        //{
        //    T_News _obj_T_News = new T_News();
        //    prjBusinessLogic.DAL.T_NewsDAL tt = new prjBusinessLogic.DAL.T_NewsDAL();
        //    string str = "";
        //    try
        //    {
        //        _obj_T_News.News_CopyFrom = tt.load_T_news(Convert.ToInt32(ID)).News_CopyFrom;
        //        if (!tt.Get_NewsVersion(Convert.ToInt32(_obj_T_News.News_CopyFrom.ToString()), 7, 82))
        //        {
        //            str = "Không có Layout";
        //        }
        //        else
        //        {
        //            str = "<b>Có Layout</b>";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string CheckLayoutTinVan(Object ID)
        //{
        //    T_News _obj_T_News = new T_News();
        //    prjBusinessLogic.DAL.T_NewsDAL tt = new prjBusinessLogic.DAL.T_NewsDAL();
        //    string str = "";
        //    try
        //    {
        //        _obj_T_News.News_CopyFrom = tt.load_T_news(Convert.ToInt32(ID)).News_CopyFrom;
        //        if (!tt.Get_NewsVersion(Convert.ToInt32(_obj_T_News.News_CopyFrom.ToString()), 1, 6))
        //        {
        //            str = "Không có Layout";
        //        }
        //        else
        //        {
        //            str = "<b>Có Layout</b>";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        public static T_Users GetUserByUserName_ID(int userID)
        {
            try
            {
                return HPCDataProvider.Instance().GetUserByUserName_ID(userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetUserName(Object ID)
        {
            string str = "";
            if (ID == DBNull.Value)
                str = "";
            else
            {
                try
                {
                    if (prjBusinessLogic.UltilFunc.GetUserByUserName_ID(Convert.ToInt32(ID)) == null)
                        str = "";
                    else
                        str = prjBusinessLogic.UltilFunc.GetUserByUserName_ID(Convert.ToInt32(ID)).UserName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return str;
        }
        public static string GetUserFullName(Object ID)
        {
            string str = "";
            if (ID == DBNull.Value)
                str = "";
            else
            {
                try
                {
                    if (prjBusinessLogic.UltilFunc.GetUserByUserName_ID(Convert.ToInt32(ID)) == null)
                        str = "";
                    else
                        str = prjBusinessLogic.UltilFunc.GetUserByUserName_ID(Convert.ToInt32(ID)).UserFullName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return str;
        }
        

        public static string GetSoghe_byID(Object ID)
        {
            string _str = "";
            DataSet _ds;
            try
            {
                if (ID != System.DBNull.Value)
                {
                    _ds = HPCDataProvider.Instance().GetStoreDataSet("[CMS_SelectT_Soghe_VebyMaDatve]", new string[] { "@ID_Datve" }, new object[] { ID });
                    if (_ds.Tables[0].Rows[0][0].ToString() != null)
                        _str = _ds.Tables[0].Rows[0][0].ToString();
                    else
                        _str = "";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _str;
        }
        //public static string GetThongtinxe(int ID)
        //{
        //    XeDAL dal = new XeDAL();
        //    string str = "";
        //    try
        //    {
        //        if (dal.GetT_XeByID(ID) == null)
        //            str = "";
        //        else
        //            str = dal.GetT_XeByID(Convert.ToInt32(ID)).Soghe.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string GetTenDonViVT(Object ID)
        //{
        //    Donvi_VantaiDAL dal = new Donvi_VantaiDAL();
        //    string str = "";
        //    try
        //    {
        //        if (dal.GetT_Donvi_VantaiByID(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            str = dal.GetT_Donvi_VantaiByID(Convert.ToInt32(ID)).Ten_donvi.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string GetTenHinhThucXuLy(Object ID)
        //{
        //    HinhthucxulyDAL dal = new HinhthucxulyDAL();
        //    string str = "";
        //    try
        //    {
        //        if (dal.GetOneFromT_HinhThuc_XuLyByID(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            str = dal.GetOneFromT_HinhThuc_XuLyByID(Convert.ToInt32(ID)).Hinhthucxuly.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string GetTenLoaiViPham(Object ID)
        //{
        //    LoaiviphamDAL dal = new LoaiviphamDAL();
        //    string str = "";
        //    try
        //    {
        //        if (dal.GetOneFromT_Loai_vi_phamByID(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            str = dal.GetOneFromT_Loai_vi_phamByID(Convert.ToInt32(ID)).Ten_loai_vi_pham.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        public static string GetSLNottai(object _Matuyen)
        {
            DataSet _ds = null;
            string _rt = "";
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("CMS_SelectT_BieudoByMatuyen", new string[] { "@Matuyen" }, new object[] { _Matuyen });
                if (_ds != null)
                {
                    _rt = _ds.Tables[0].Rows[0]["KT"].ToString() + " / " + _ds.Tables[0].Rows[0]["QH"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _rt;
        }
        public static string GetNottai_byDVVT(object _MaDK, object _DVVT)
        {
            DataSet _ds = null;
            string _rt = "";
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("CMS_Select_Nottai_byDVVT", new string[] { "@Ma_DK", "@MaDVVT" }, new object[] { _MaDK, _DVVT });
                if (_ds != null)
                {
                    _rt = _ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _rt;
        }
        public static string GetLuuluongDK_byDVVT(object _MaDK, object _DVVT)
        {
            DataSet _ds = null;
            string _rt = "";
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("CMS_Select_LuuLuongDK_byDVVT", new string[] { "@Ma_DK", "@MaDVVT" }, new object[] { _MaDK, _DVVT });
                if (_ds != null)
                {
                    _rt = _ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _rt;
        }
        //public static string GetTenQuanHuyen(Object ID)
        //{
        //    QuanhuyenDAL dal = new QuanhuyenDAL();
        //    string str = "";
        //    try
        //    {
        //        if (dal.GetOneFromT_QuanhuyenByID(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            str = dal.GetOneFromT_QuanhuyenByID(Convert.ToInt32(ID)).Ten_QuanHuyen.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string GetTenDichVu(Object ID)
        //{
        //    DichvuDAL dal = new DichvuDAL();
        //    string str = "";
        //    try
        //    {
        //        if (dal.GetT_DichvuByID(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            str = dal.GetT_DichvuByID(Convert.ToInt32(ID)).Ten_DV.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string HangGPLX(Object ID)
        //{
        //    HangGPLXDAL dal = new HangGPLXDAL();
        //    LaixeDAL lx = new LaixeDAL();
        //    int _id;
        //    string str = "";
        //    try
        //    {
        //        _id = lx.GetT_Laixe_ByID(Convert.ToInt32(ID)).ID_GPLX;
        //        if (_id != null)
        //        {
        //            if (dal.GetOneFromT_Hang_GPLXByID(Convert.ToInt32(_id)) == null)
        //                str = "";
        //            else
        //                str = dal.GetOneFromT_Hang_GPLXByID(Convert.ToInt32(_id)).Hang_GPLX.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string GetTenCuly(Object ID)
        //{
        //    CulyDAL dal = new CulyDAL();
        //    string str = "";
        //    try
        //    {
        //        if (dal.GetT_CulyByID(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            str = dal.GetT_CulyByID(Convert.ToInt32(ID)).Ten_Culy.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string GetTenLoaiXe(Object ID)
        //{
        //    LoaixeDAL dal = new LoaixeDAL();
        //    string str = "";
        //    try
        //    {
        //        if (dal.GetOneFromT_LoaixeByID(Convert.ToInt32(ID)) == null)
        //            str = "";
        //        else
        //            str = dal.GetOneFromT_LoaixeByID(Convert.ToInt32(ID)).Ten_loai_xe.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}
        //public static string Get_Matuyen(Object ID)
        //{
        //    QuyhoachtuyenDAL dal = new QuyhoachtuyenDAL();
        //    string str = "";
        //    try
        //    {
        //        if (ID != System.DBNull.Value)
        //            str = dal.GetOneFromT_QuyhoachtuyenByID(Convert.ToInt32(ID)).Ma_tuyen.ToString();
        //        else
        //            str = "";
             
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return str;
        //}

        public static double GetGiave(int _Ma_tuyen, int _Ma_DVVT, int _Loaixe)
        {
            double _return;
            DataSet _ds;
           
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("[CMS_Select_Giave]", new string[] { "@Ma_tuyen", "@Ma_DVVT", "@Loai_xe" }, new object[] { _Ma_tuyen, _Ma_DVVT, _Loaixe});
                if (_ds.Tables[0].Rows.Count > 0)
                    _return = Convert.ToDouble(_ds.Tables[0].Rows[0]["Dongia"].ToString());
                else
                    _return = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _return;
        }
        public static DataSet List_Nottai(int Ma_tuyen, int Ma_DVVT)
        {

            DataSet _ds = null;
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("CMS_Select_Nottai_Dangkyve", new string[] { "@Ma_tuyen", "@Ma_DV" }, new object[] { Ma_tuyen, Ma_DVVT });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _ds;

        }
        public static string Get_Nottai_byID(object _ID)
        {
            string str = "";
            DataSet _ds = null;
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("CMS_Select_Nottai_FromT_Bieudo", new string[] { "@where" }, new object[] { " ID = " + _ID });
                if (_ds != null)
                {
                    str = _ds.Tables[0].Rows[0]["Time_start"].ToString();
                }
                else
                    str = "";

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return str;

        }
        //public static string getSenderIDFrom_NewsID(int News_ID, int intFieldIndex)
        //{
        //    SqlService _sqlservice = new SqlService();
        //    SqlDataReader _reader;
        //    string _sql = string.Empty;
        //    string UserModify = "";
        //    double authorid = 0;
        //    string Nguoitao = "";
        //    prjBusinessLogic.DAL.T_NewsDAL NewsDal = new prjBusinessLogic.DAL.T_NewsDAL();
        //    _sql = "[CMS_getSenderIDFrom_NewsID]";
        //    try
        //    {
        //        _sqlservice.AddParameter("@News_ID", SqlDbType.Int, News_ID);
        //        _reader = _sqlservice.ExecuteSPReader(_sql);
        //        if (_reader.HasRows)
        //        {
        //            while (_reader.Read())
        //            {
        //                if (intFieldIndex == 0)
        //                    UserModify = GetUserName(_reader["News_EditorID"].ToString());
        //                else
        //                    UserModify = _reader["News_DateEdit"].ToString();
        //            }
        //        }
        //        authorid = NewsDal.load_T_news(News_ID).News_AuthorID;
        //        Nguoitao = GetUserName(authorid);
        //        if (UserModify != "")
        //            return UserModify;
        //        else
        //            return UserModify = Nguoitao;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {

        //        _sqlservice.Disconnect();
        //    }

        //}

        public static bool CheckItemExist(string TableName, string ColumnName, string Where)
        {

            if (UltilFunc.GetColumnValuesOne(TableName, ColumnName, Where) > 0)
                return false;
            else
                return true;
        }



        #region GET WHERE DIEU KIEN CHUYEN MUC // BOCT
        public static string GetCategory4User(int UserID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT DISTINCT(Categorys_ID) FROM T_UserCategory Where [User_ID] = {0} ", UserID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["Categorys_ID"].ToString();
                        else _return += "," + _dt.Rows[i]["Categorys_ID"].ToString();
                    }
                }
                else
                    _return = "0";
            }
            catch (Exception ex)
            {
                _return = "0";
                throw ex;

            }
            return _return;
        }
        public static string GetCategoryNameByAdsID(int _ID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT Category_Name FROM T_Categorys Where [Categorys_ID] = {0} ", _ID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["Category_Name"].ToString();

                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";
                throw ex;

            }
            return _return;
        }
        #endregion

        #region GET WHERE DIEU KIEN NGON NGU // BOCT
        public static string GetLanguagesByUser(int UserID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT DISTINCT(Languages_ID) FROM T_UserLanguages Where [User_ID] = {0} ", UserID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["Languages_ID"].ToString();
                        else _return += "," + _dt.Rows[i]["Languages_ID"].ToString();
                    }
                }
                else
                    _return = "0";
            }
            catch (Exception ex)
            {
                _return = "0";
                throw ex;

            }
            return _return;
        }
        public static string GetListPapersByUser(int UserID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT DISTINCT(Paper_ID) FROM T_UserPapers Where [User_ID] = {0} ", UserID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["Paper_ID"].ToString();
                        else _return += "," + _dt.Rows[i]["Paper_ID"].ToString();
                    }
                }
                else
                    _return = "0";
            }
            catch (Exception ex)
            {
                _return = "0";
                throw ex;

            }
            return _return;
        }
        #endregion

        #region GET WHERE DIEU KIEN ID CHUYEN MUC // ADD BY NVTHAI
        public static string GetPosition_Display(int _ID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT Position_Display FROM T_Categorys Where [Categorys_ID] = {0} ", _ID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["Position_Display"].ToString();
                    }
                }
                else
                    _return = "0";
            }
            catch (Exception ex)
            {
                _return = "0";
                throw ex;

            }
            return _return;
        }
        public static string GetBaivietLienquan_Display(int _ID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT News_Tittle FROM T_News Where [News_ID] = {0} ", _ID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["News_Tittle"].ToString();
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";
                throw ex;

            }
            return _return;
        }

        #endregion

        #region Clear Dieu Kien TIM KIEM
        public static string SqlFormatText(string text)
        {
            if (text == null) return "";
            else return text.Replace("'", "''");
        }
        public static string OrracleFormatText(string text)
        {
            if (text == null) return "";
            else return text.Replace("/", "");
        }
        public static bool IsNumeric(string strcheck)
        {
            Regex regex = new Regex(
                @"^\d+([\.|,] \d+)?$",
                RegexOptions.IgnoreCase
                | RegexOptions.Multiline
                | RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                );
            return regex.IsMatch(strcheck) ? true : false;
        }
        #endregion

        #region ToDateTime
        public static DateTime ToDate(string x, string kieu)
        {
            try
            {
                int sp1 = x.IndexOf('/'), sp2 = x.LastIndexOf('/');
                int day, month, year;
                if (kieu.Equals("MM/dd/yyyy")) // MM/dd/yyyy hoac M/d/yyyy
                {
                    day = int.Parse(x.Substring(sp1 + 1, sp2 - sp1 - 1));
                    month = int.Parse(x.Substring(0, sp1));
                }
                else //'dd/MM/yyyy' hoac d/M/yyyy
                {
                    day = int.Parse(x.Substring(0, sp1));
                    month = int.Parse(x.Substring(sp1 + 1, sp2 - sp1 - 1));
                }
                year = int.Parse(x.Substring(sp2 + 1, 4));
                return new DateTime(year, month, day);
            }
            catch
            {
                throw new Exception("Sai kieu ngay thang");
            }
        }
        public static DateTime ToDateN(string x, string kieu)
        {
            try
            {
                int sp1 = x.IndexOf('-'), sp2 = x.LastIndexOf('-');
                int day, month, year;
                if (kieu.Equals("MM-dd-yyyy")) // MM/dd/yyyy hoac M/d/yyyy
                {
                    day = int.Parse(x.Substring(sp1 + 1, sp2 - sp1 - 1));
                    month = int.Parse(x.Substring(0, sp1));
                }
                else //'dd/MM/yyyy' hoac d/M/yyyy
                {
                    day = int.Parse(x.Substring(0, sp1));
                    month = int.Parse(x.Substring(sp1 + 1, sp2 - sp1 - 1));
                }
                year = int.Parse(x.Substring(sp2 + 1, 4));
                return new DateTime(year, month, day);
            }
            catch
            {
                throw new Exception("Sai kieu ngay thang");
            }
        }
        public static DateTime ToDateddMMyyyhhmm(string x)
        {
            try
            {
                int sp1 = x.IndexOf('/'), sp2 = x.LastIndexOf('/');
                int day, month, year, hh, mm;
                day = int.Parse(x.Substring(0, sp1));
                month = int.Parse(x.Substring(sp1 + 1, sp2 - sp1 - 1));
                year = int.Parse(x.Substring(sp2 + 1, 4));
                hh = int.Parse(x.Substring(sp2 + 6, 2));
                mm = int.Parse(x.Substring(sp2 + 9, 2));
                return new DateTime(year, month, day, hh, mm, 0);
            }
            catch
            {
                throw new Exception("Sai kieu ngay thang");
            }
        }
        public static string CleanFormatTags(string Contents)
        {
            //Contents = Regex.Replace(Contents, "<(select|option|script|style|title)(.*?)>((.|\n)*?)</(select|option|script|style|title)>", " ", RegexOptions.IgnoreCase);
            //Contents = Regex.Replace(Contents, "&(nbsp|quot|copy);", "");
            Contents = Regex.Replace(Contents, "&(nbsp|quot);", "");
            //Contents = Regex.Replace(Contents, "'", "");
            //Contents = Regex.Replace(Contents, "~", "");
            //Contents = Regex.Replace(Contents, "\"", "");//dung sua      
            //Contents = Regex.Replace(Contents, "!", "");
            //Contents = Regex.Replace(Contents, "@", "");
            //Contents = Regex.Replace(Contents, "#", "");
            //Contents = Regex.Replace(Contents, "(;|--|create|drop|select|insert|delete|update|union|sp_|xp_)", "");
            Contents = Regex.Replace(Contents, "<([\\s\\S])+?>", " ", RegexOptions.IgnoreCase).Replace("  ", " ");

            return Contents;
        }

        #endregion

        #region REPLATE IP
        static public ArrayList Path_Replate
        {
            get
            {
                ArrayList _arr = new ArrayList();

                string _str = System.Configuration.ConfigurationSettings.AppSettings["Path_Replate"];
                char[] ch = { ';' };
                string[] arr = _str.Split(ch);
                for (int i = 0; i < arr.Length; i++)
                {
                    _arr.Add(arr[i]);
                }
                return _arr;

            }
        }
        public static string Rep_NewsOne(string _str, ArrayList _arr)
        {
            string _return = "";
            if (_arr.Count > 0)
            {
                for (int i = 0; i < _arr.Count; i++)
                {
                    _return = Rep_News(_str, _arr[i].ToString());
                }
            }
            return _return;
        }

        private static string Rep_News(string _str, string urlService)
        {
            try
            {
                return _str.Replace(urlService, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public static string GetFullName(Object ID)
        {
            string str = "";
            if (ID == DBNull.Value)
                str = "";
            else
            {
                try
                {
                    if (prjBusinessLogic.UltilFunc.GetUserByUserName_ID(Convert.ToInt32(ID)) == null)
                        str = "";
                    else
                        str = prjBusinessLogic.UltilFunc.GetUserByUserName_ID(Convert.ToInt32(ID)).UserFullName;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return str;
        }
        public static bool checkExitsNewsRealates(string _ID, string _listID)
        {
            bool _return = false;
            try
            {
                string[] sArrProdID = null;
                char[] sep = { ',' };
                sArrProdID = _listID.ToString().Trim().Split(sep);
                for (int i = 0; i < sArrProdID.Length; i++)
                {
                    if (_ID.ToString() == sArrProdID[i].ToString())
                    {
                        _return = true;
                        break;
                    }
                    else
                        _return = false;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _return;
        }

        public static int WordCount(string Text)
        {
            string tmpStr;
            string tt = CleanHTMLSummary(Text);

            tmpStr = tt.Replace("\t", " ").Trim();
            tmpStr = tmpStr.Replace("\n", " ");
            tmpStr = tmpStr.Replace("\r", " ");

            while (tmpStr.IndexOf("  ") != -1)
                tmpStr = tmpStr.Replace("  ", " ");
            if (tt != "")
                return tmpStr.Split(' ').Length;
            else
                return 0;
        }
        public static string CleanHTMLSummary(string Contents)
        {
            Contents = Regex.Replace(Contents, "<(select|option|script|style|title)(.*?)>((.|\n)*?)</(select|option|script|style|title)>", " ", RegexOptions.IgnoreCase);

            Contents = Regex.Replace(Contents, "<div>", "");

            Contents = Regex.Replace(Contents, "</div>", "");
            Contents = Regex.Replace(Contents, "(;|--|create|drop|select|insert|delete|update|union|sp_|xp_)", "");
            Contents = Regex.Replace(Contents, "<([\\s\\S])+?>", " ", RegexOptions.IgnoreCase).Replace("  ", " ");
            Contents = Regex.Replace(Contents, "\r\n", "").Trim();

            Contents = Regex.Replace(Contents.Trim(), "Normal 0 false false false MicrosoftInternetExplorer4", "");
            Contents = Regex.Replace(Contents.Trim(), "Normal 0  false false false     MicrosoftInternetExplorer4", "");
            Contents = Regex.Replace(Contents.Trim(), "Normal 0 false false false MicrosoftInternetExplorer4", "");
            Contents = Regex.Replace(Contents.Trim(), "Normal  0    false  false  false         MicrosoftInternetExplorer4", "");
            Contents = Regex.Replace(Contents.Trim(), "Normal  0    false  false  false          MicrosoftInternetExplorer4", "");

            return (Contents);
        }
        public static DateTime CheckNullDate(object Value)
        {
            if (Value == DBNull.Value) return DateTime.MinValue;
            else return Convert.ToDateTime(Value);
        }

        public static void RunJavaScriptCode(string JavaScriptCode)
        {
            try
            {
                Page mPage = (Page)HttpContext.Current.Handler;
                mPage.ClientScript.RegisterStartupScript(typeof(Page), "", JavaScriptCode, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static string GetTieuDetin(int _where)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT News_Tittle FROM  T_News Where News_ID = " + _where);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["News_Tittle"].ToString();
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";
                throw ex;

            }
            return _return;
        }

        public static bool GetLoaiPhanHoi(int _where)
        {
            bool _return = false;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT IsProducts FROM  T_Suggestions Where ID = " + _where);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        _return = Convert.ToBoolean(_dt.Rows[i]["IsProducts"].ToString());
                    }
                }
                else
                    _return = false;
            }
            catch (Exception ex)
            {
                _return = false;
                throw ex;

            }
            return _return;
        }


        public static string GetListIDTin(string Wherecondition)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT DISTINCT News_ID FROM  T_Suggestions " + Wherecondition);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["News_ID"].ToString();
                        else _return += "," + _dt.Rows[i]["News_ID"].ToString();
                    }
                }
                else
                    _return = "0";
            }
            catch (Exception ex)
            {
                _return = "0";
                throw ex;

            }
            return _return;
        }

        public static string checkUrlExternal(string UrlLocal, object Ads_Images)
        {
            string strTemp = "";
            if (Ads_Images.ToString().Length > 0)
            {
                if (!Ads_Images.ToString().Trim().StartsWith("http"))
                {
                    strTemp = UrlLocal.ToString() + Ads_Images.ToString();
                }
                else
                    strTemp = Ads_Images.ToString();
            }
            return strTemp;
        }

        public static void InsertKeyWords(string _keywords, DateTime _date, int _userID)
        {
            SqlService _CommonSQL = new SqlService();
            string strSP = "CMS_InsertT_KeyWords";
            try
            {
                _CommonSQL.AddParameter("@KeyWord", SqlDbType.NVarChar, _keywords);
                _CommonSQL.AddParameter("@DateCreate", SqlDbType.DateTime, _date);
                _CommonSQL.AddParameter("@UserCreate", SqlDbType.Int, _userID);
                _CommonSQL.ExecuteSP(strSP);
            }
            catch (Exception ex)
            { }
            finally
            {
                _CommonSQL.CloseConnect();
                _CommonSQL.Disconnect();
            }

        }
        public static bool checkExitsKeyWord(string _keyword)
        {
            DataSet _ds = null;
            try
            {
                _ds = HPCDataProvider.Instance().GetStoreDataSet("[CMS_SelectOneFromT_KeyWords]", new string[] { "@WhereCondition" }, new object[] { _keyword });
                if (_ds.Tables[0].Rows.Count > 0)
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
        /// <summary>
        /// Hàm kiểm tra đồng bộ Text
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public static bool Check_SyncNewsDatabase(object menuid)
        {
            bool _check = true;
            try
            {
                int id = 0;
                string sql = "select distinct(ActiveSync) from T_Menus where ID  = " + menuid;
                DataSet ds = HPCDataProvider.Instance().ExecSqlDataSet(sql);
                id = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (id == 1) _check = true; else _check = false;
                ds.Clear();
            }
            catch
            {
                //throw ex;
                _check = false;
            }
            return _check;
        }
        /// <summary>
        /// Hàm kiểm tra đồng bộ Ảnh.
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public static bool Check_SyncImageDatabase(object menuid)
        {
            bool _check = true;
            try
            {
                int id = 0;
                string sql = "select distinct(ActiveSyncImages) from T_Menus where ID  = " + menuid;
                DataSet ds = HPCDataProvider.Instance().ExecSqlDataSet(sql);
                id = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (id == 1) _check = true; else _check = false;
                ds.Clear();
            }
            catch
            {
                //throw ex;
                _check = false;
            }
            return _check;
        }
        /// <summary>
        /// Get Total Record with Status ALL for Data
        /// </summary>
        /// <param name="WhereCondition"></param>
        /// <param name="_store"></param>
        /// <returns></returns>
        public static int GetTotalCountStatus(string WhereCondition, string _store)
        {
            int _Id = 0;
            try
            {
                DataSet _ds = HPCDataProvider.Instance().GetStoreDataSet(_store, new string[] { "@where" }, new object[] { WhereCondition });
                _Id = Convert.ToInt32(_ds.Tables[1].Rows[0].ItemArray[0].ToString());
                _ds.Clear();
                _ds = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Id;
        }
        public static int GetTotalCountT_NewsStatus(string WhereCondition, string fulltext, string _store)
        {
            int _Id = 0;
            try
            {
                DataSet _ds = HPCDataProvider.Instance().GetStoreDataSet(_store, new string[] { "@where", "@search" }, new object[] { WhereCondition, fulltext });
                _Id = Convert.ToInt32(_ds.Tables[1].Rows[0].ItemArray[0].ToString());
                _ds.Clear();
                _ds = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Id;
        }
        /// <summary>
        /// Email check invalid for all
        /// </summary>
        /// <param name="_EmailCheck"></param>
        /// <returns></returns>
        public static bool IsEmailError(string _EmailCheck)
        {
            string check = @"^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$";
            Regex regex = new Regex(
                check,
                RegexOptions.IgnoreCase
                | RegexOptions.Multiline
                | RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                );
            return regex.IsMatch(_EmailCheck) ? true : false;
        }
        /// <summary>
        /// Use Lock News
        /// </summary>
        /// <param name="prm_news_Lock"></param>
        /// <param name="prmNewsEditorID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public static string LockedUser(string prm_news_Lock, string prmNewsEditorID, int _userID)
        {
            string _userLock = "";
            int _prmEditorID = Convert.ToInt32(prmNewsEditorID);
            if (prm_news_Lock == "True" && _prmEditorID != _userID)
                _userLock = "<b> &nbsp;&nbsp;[ <font color='red'> User locked: " + UltilFunc.GetUserName(prmNewsEditorID) + "</font> ]</b>";
            return _userLock;
        }
        public static string IsStatusImages(string str)
        {
            string strReturn = "";
            int _count = CountImgTag(str);
            if (_count != 0)
                strReturn = "<b> &nbsp;&nbsp;[ <font color='CC00FF'> có " + _count.ToString() + " ảnh trong bài</font> ]</b>";
            return strReturn;
        }
        /// <summary>
        /// Disable or Enable T_News
        /// </summary>
        /// <param name="_Role"></param>
        /// <param name="prmNews_Lock"></param>
        /// <param name="prmEditorID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public static bool IsEnable(bool _Role, string prmNews_Lock, string prmEditorID, int _userID)
        {
            bool _isEnabled = true;
            int _prmEditorID = Convert.ToInt32(prmEditorID);
            if (prmNews_Lock == "True" && _prmEditorID != _userID)
                _isEnabled = false;
            else
            {
                _isEnabled = _Role;
            }
            return _isEnabled;
        }
        public static string IsGetTrangThai(object str)
        {
            string strReturn = "";
            if (str != null)
            {
                if (str.ToString() == "1")
                    strReturn = "Bình thường";
                if (str.ToString() == "2")
                    strReturn = "Nổi bật chuyên mục";
                if (str.ToString() == "3")
                    strReturn = "Nổi bật trang chủ";
                if (str.ToString() == "4")
                    strReturn = "Nổi bật chuyên mục cha";
                if (str.ToString() == "5")
                    strReturn = "Tin vắn";
            }
            return strReturn;
        }
        public static string IsStatusGet(object _images, object _video)
        {
            string strReturn = "";

            if (_images != null && _video != null)
            {
                if (_images.ToString().ToLower() == "true")
                    strReturn += "&nbsp;&nbsp;<img src=\"" + ConfigurationManager.AppSettings["ApplicationPath"] + "/Images/Icons/i-image.png\" alt=\"Tin Ảnh\" width=\"12px\" height=\"12px\" title=\"Tin Ảnh\" /> &nbsp;";
                if (_video.ToString().ToLower() == "true")
                    strReturn += "&nbsp;&nbsp;<img src=\"" + ConfigurationManager.AppSettings["ApplicationPath"] + "/Images/Icons/i-video.png\" alt=\"Tin Video\" width=\"12px\" height=\"12px\" title=\"Tin Video\" />";
                //if (strReturn.Trim().Length == 0)
                //    strReturn += "&nbsp;&nbsp;<img src=\"" + ConfigurationManager.AppSettings["ApplicationPath"] + "/Images/Icons/Article.gif\" alt=\"Tin\" width=\"12px\" height=\"12px\" title=\"Tin\" />";
            }
            return strReturn;
        }

        public static string ShowImage(object objImages)
        {
            if (objImages != null && objImages.ToString().Length > 2)
            {
                return "<img style=\"cursor:pointer;border:0px;\" alt=\"Xem ảnh\" title=\"Xem ảnh\" src=\"../Images/photo_scenery.png\" onclick=\"ViewImages('" + ConfigurationManager.AppSettings["tinpath"] + objImages + "');\" />";
            }
            else
                return "";
        }
        public static string RemoveHTMLTag(string HTML)
        {
            // Xóa các thẻ html
            System.Text.RegularExpressions.Regex objRegEx = new System.Text.RegularExpressions.Regex("<[^>]*>");
            return objRegEx.Replace(HTML, "");

        }
        public static string RemoveHTMLTagNotImg(string HTML, string tagNotRemove)
        {
            // Xóa các thẻ html tru the img
            //string AcceptableTags = "i|b|u|sup|sub|ol|ul|li|br|h2|h3|h4|h5|span|div|p|a|blockquote";           
            string stringPattern = @"</?(?(?=" + tagNotRemove + @")notag|[a-z,A-Z,0-9]+)(?:\s[a-z,A-Z,0-9,\-]+=?(?:(["",']?).*?\1?))*\s*/?>";
            return Regex.Replace(HTML, stringPattern, "");

        }

        public static string ReplaceCharsRewrite(object input)
        {
            string str = "", StrTemp = RemoveHTMLTag(Convert.ToString(input));
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = StrTemp.Normalize(System.Text.NormalizationForm.FormD);
            str = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            Regex objRegEx = new Regex("<[^>]*>");
            str = str.Replace(" ", "-");
            str = str.Replace(",", "");
            str = str.Replace(".", "");
            str = str.Replace(";", "");
            str = str.Replace(":", "");
            str = str.Replace("?", "");
            str = str.Replace("<", "");
            str = str.Replace(">", "");
            str = str.Replace("`", "");
            str = str.Replace("~", "");
            str = str.Replace("!", "");
            str = str.Replace("@", "");
            str = str.Replace("#", "");
            str = str.Replace("$", "");
            str = str.Replace("%", "");
            str = str.Replace("^", "");
            str = str.Replace("&", "");
            str = str.Replace("*", "");
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            str = str.Replace("+", "");
            str = str.Replace("=", "");
            str = str.Replace("\\", "");
            str = str.Replace("|", "");
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            str = str.Replace("'", "");
            str = str.Replace("\"", "");
            str = str.Replace("”", "");
            str = str.Replace("“", "");
            str = str.Replace("-»", "");
            str = str.Replace("«-", "");
            str = str.Replace("»", "");
            str = str.Replace("»", "");
            str = str.Replace("«", "");
            str = str.Replace("’", "");
            str = str.Replace("--", "-");
            str = str.Replace("---", "-");
            str = str.Replace("----", "-");
            str = str.Replace("-----", "-");
            str = str.Replace(" ", "-");
            //Add by nvthai
            str = str.Replace("Ã°", "");
            str = str.Replace("â€", "");
            str = str.Replace("a€", "");
            str = str.Replace("a°", "");
            return str.ToLower();
        }

        /// <summary>
        /// Insert Keyword into Table Keywords
        /// </summary>
        /// <param name="_listKey"></param>
        /// <param name="UserID"></param>
        public static void InsertKeywords(string _listKey, int UserID)
        {
            string sWhere = "";
            string[] sArrProdID = null;
            char[] sep = { ',' };
            sArrProdID = _listKey.ToString().Trim().Split(sep);
            for (int i = 0; i < sArrProdID.Length; i++)
            {
                sWhere = " KeyWord LIKE " + string.Format("N'%{0}%'", SqlFormatText(sArrProdID[i].ToString().Trim()));
                if (checkExitsKeyWord(sWhere) != false)
                    InsertKeyWords(sArrProdID[i].ToString().Trim(), DateTime.Now, UserID);
            }
        }
        /// <summary>
        /// Insert Image In To Table News_Images
        /// </summary>
        /// <param name="strcontents"></param>
        /// <param name="News_ID"></param>
        public static void Insert_News_Image(string strcontents, double News_ID)
        {
            prjBusinessLogic.ImageFilesDAL imageDAL = new ImageFilesDAL();
            imageDAL.Delete_Image_NewsID(News_ID);
            try
            {
                Regex regex = new Regex(
                    @"(?<=<img[^<]+?id=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(strcontents);
                for (int i = 0; i < matchCollect.Count; i++)
                {
                    string _id = matchCollect[i].Value.Trim();
                    if (_id.Length > 0 && IsNumeric(_id))
                    {
                        imageDAL.Insert_ImagesInNews(News_ID, Convert.ToInt32(_id));
                    }
                }
            }
            catch { }
        }
        public static string Visible_CountImage(bool _R_Edit, string countImage, string prmNews_Lock, string prmEditorID, int _UserID)
        {
            int count = 0;
            string _visible = "None";
            try
            {
                count = int.Parse(countImage);
            }
            catch { ;}
            if (UltilFunc.IsEnable(_R_Edit, prmNews_Lock, prmEditorID, _UserID) && count > 0)
                _visible = "Display";
            return _visible;
        }
        public static string SplitString(string _root)
        {
            string _key = "";
            int _len = _root.Length;
            int checkand = 0, _continue = 0;
            for (int i = 0; i < _len; i++)
            {
                if (_root[i] == ' ' && _continue == 0)
                {
                    checkand = 0;
                    for (int j = i + 1; j < _len; j++)
                    {
                        if (_root[j] == ' ')
                        {
                        }
                        else if (_root[j] == '+')
                        {
                            checkand = 1;
                        }
                        else if (_root[j] != ' ' && _root[j] != '+')
                        {
                            if (checkand == 0)
                                _key = _key + "\" OR \"";
                            else
                                _key = _key + "\" and \"";
                            i = j - 1;
                            checkand = 0;
                            break;
                        }
                    }
                }
                else if (_root[i] == '+')
                {
                    for (int j = i + 1; j < _len; j++)
                    {
                        if (_root[j] == ' ' || _root[j] == '+')
                        {
                        }
                        else
                        {
                            _key = _key + "\" and \"";
                            i = j - 1;
                            break;
                        }
                    }
                }
                else if (_root[i] == '"' && _continue == 0)
                {
                    _continue = 1;
                }

                else if (_root[i] == '"' && _continue == 1)
                {
                    _continue = 0;
                    if (i < _len - 1 && _root[i + 1] != ' ' && _root[i + 1] != '+')
                    {
                        _key = _key + "\" OR \"";
                    }
                }
                else
                {
                    checkand = 0;
                    _key = _key + _root[i];
                }
            }
            _key = _key + "";
            return _key;

        }
        public static string ReplaceAll(string source, string stringToFind, string stringToReplace)
        {
            var temp = source;
            var index = temp.IndexOf(stringToFind);
            while (index != -1)
            {
                temp = temp.Replace(stringToFind, stringToReplace);
                index = temp.IndexOf(stringToFind);
            }
            return temp;
        }

        public static void WriteLogActionHistory(int _UserID, string _FullName, string _HostIP, string _ActionsCode, double _News_ID, string _Notes, int _Menu_ID)
        {
            try
            {
                //ActionHistoryDAL actionDAL = new ActionHistoryDAL();
                //T_ActionHistory action = new T_ActionHistory();
                //action.ID = 0;
                //action.UserID = _UserID;
                //action.FullName = _FullName;
                //action.HostIP = _HostIP;
                //action.DateModify = DateTime.Now;
                //action.ActionsCode = _ActionsCode;
                //action.News_ID = _News_ID;
                //action.Notes = _Notes;
                //action.Menu_ID = _Menu_ID;
                //actionDAL.InserT_Action(action);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Add by Nvthai
        public static string GetCountAllNews(string _status, int _userID, int _order)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                if ((_userID != 0) && (_order == 1))
                    _sql = string.Format("Select COUNT(News_ID) AS News_ID FROM T_News Where News_Status in (" + _status + ") And News_AuthorID=" + _userID + " And CAT_ID in (select DISTINCT(Categorys_ID) from T_UserCategory where User_ID = " + _userID + ") AND Lang_ID IN (SELECT DISTINCT(T_UserLanguages.Languages_ID) FROM T_UserLanguages WHERE T_UserLanguages.[User_ID] = " + _userID + ")");
                else if ((_userID == 0) && (_order == 2))
                    _sql = string.Format("Select COUNT(News_ID) AS News_ID FROM T_News Where News_Status in (" + _status + ") And News_DatePublished is null");
                else if ((_userID != 0) && (_order == 4))
                    _sql = string.Format("Select COUNT(News_ID) AS News_ID FROM T_News Where News_Status in (" + _status + ") And News_DatePublished is NOT null And CAT_ID in (select DISTINCT(T_UserCategory.Categorys_ID) from T_UserCategory where User_ID = " + _userID + ") ");
                else
                    _sql = string.Format("Select COUNT(News_ID) AS News_ID FROM T_News Where News_Status in (" + _status + ") ");

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return += _dt.Rows[i]["News_ID"].ToString();
                        }
                    }
                }
                else
                    _return = "0";
            }
            catch (Exception ex)
            {
                _return = "0";

            }
            return _return;
        }

        //Add by Nvthai
        public static string GetCountAllProducts(string _status, int _userID, int _order)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                if ((_userID != 0) && (_order == 1))
                    _sql = string.Format("Select COUNT(Product_ID) AS News_ID FROM T_Products Where Product_Status in (" + _status + ") And UserCreate=" + _userID + " AND Lang_ID IN (SELECT DISTINCT(T_UserLanguages.Languages_ID) FROM T_UserLanguages WHERE T_UserLanguages.[User_ID] = " + _userID + ")");
                else if ((_userID == 0) && (_order == 2))
                    _sql = string.Format("Select COUNT(Product_ID) AS News_ID FROM T_Products Where Product_Status in (" + _status + ") ");
                else
                    _sql = string.Format("Select COUNT(Product_ID) AS News_ID FROM T_Products Where Product_Status in (" + _status + ") ");

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return += _dt.Rows[i]["News_ID"].ToString();
                        }
                    }
                }
                else
                    _return = "0";
            }
            catch (Exception ex)
            {
                _return = "0";

            }
            return _return;
        }

        public static string GetNguonGiaThitruong()
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select Top 1 TenVung FROM T_GiaThiTruong");

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return += _dt.Rows[i]["TenVung"].ToString();
                        }
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";

            }
            return _return;
        }
        public static string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            return PhysPathFull.ToString().Replace(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToString(), "");
        }
        public static string GetStyleComments(Object _id)
        {
            string _return = null;
            string _comment = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select News_Comment FROM T_News Where News_ID =" + _id);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_comment == null)
                        {
                            _comment += _dt.Rows[i]["News_Comment"].ToString();
                            if (_comment.Length > 0)
                                _return = "linkEditCommend";
                        }
                    }
                }
                else
                    _return = "linkEdit";
            }
            catch (Exception ex)
            {
                _return = "linkEdit";

            }
            return _return;
        }
        public static string GetXPathByID(int _ID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT RssXPath FROM T_RssLinks Where [ID] = {0} ", _ID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["RssXPath"].ToString();
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";
                throw ex;

            }
            return _return;
        }
        public static DataSet GetDataSetRssLink(string TableName, string ColumnList, string Where)
        {
            try
            {
                return HPCDataProvider.Instance().GetDataSet(TableName, ColumnList, Where);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetColumnValuesRssLink(string TableName, string ColumnName, string Where)
        {
            try
            {

                return HPCDataProvider.Instance().GetColumnValues(TableName, ColumnName, Where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetPathImageByProductID(object _ID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT Top 1 ImagePath FROM T_ProductImages Where IsDaidien =1 And  [ProductID] = {0} ", _ID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["ImagePath"].ToString();
                    }
                }
                else
                {
                    string _sql2 = string.Format("SELECT Top 1 ImagePath FROM T_ProductImages Where [ProductID] = {0} ", _ID);
                    DataTable _dt2 = _untilDAL.ExecSqlDataSet(_sql2).Tables[0];
                    if (_dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < _dt2.Rows.Count; i++)
                        {
                            if (_return == null)
                                _return += _dt2.Rows[i]["ImagePath"].ToString();
                        }
                    }
                    else
                        _return = "";
                }

            }
            catch (Exception ex)
            {
                _return = "";
                throw ex;

            }
            return _return;
        }
        public static string GetProvinceNameByID(int _ID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT ProvinName FROM T_Provinces Where  [ID] = {0} ", _ID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["ProvinName"].ToString();
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";
                throw ex;

            }
            return _return;
        }

        public static string GetProvinceID(object _ID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT Provin_ID FROM T_Products Where  [Product_ID] = {0} ", _ID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["Provin_ID"].ToString();
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";
                throw ex;

            }
            return _return;
        }
        public static string GetCompanyID(object _ID)
        {
            string _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = string.Format("SELECT CompanyID FROM T_Products Where  [Product_ID] = {0} ", _ID);
                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                            _return += _dt.Rows[i]["CompanyID"].ToString();
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";
                throw ex;

            }
            return _return;
        }
        public static string GetFullNameCustommerByID(object _id)
        {
            string _return = null;

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select Name FROM T_Customers Where ID =" + _id);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return = _dt.Rows[i]["Name"].ToString();

                        }
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";

            }
            return _return;
        }
        public string GetFullNameByID(int _id)
        {
            string _return = null;

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select UserFullName FROM T_Users Where UserID =" + _id);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return = _dt.Rows[i]["UserFullName"].ToString();

                        }
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";

            }
            return _return;
        }
        public string GetFullNameCustommerByID(int _id)
        {
            string _return = null;

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select Name FROM T_Customers Where ID =" + _id);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return = _dt.Rows[i]["Name"].ToString();

                        }
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";

            }
            return _return;
        }
        public string GetFullNameT_MaterialsByID(int _id)
        {
            string _return = null;

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select Category_Name FROM T_Categorys Where IsVideo=1 AND Categorys_ID =" + _id);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return = _dt.Rows[i]["Category_Name"].ToString();

                        }
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";

            }
            return _return;
        }
        public string GetFullNameLangByID(int _id)
        {
            string _return = null;

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select Languages_Name FROM T_Languages Where Languages_ID =" + _id);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return = _dt.Rows[i]["Languages_Name"].ToString();

                        }
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";

            }
            return _return;
        }
        public static string GetFullLinkByID(object _id)
        {
            string _return = null;

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select Link FROM T_Contacts Where ID =" + _id);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return = _dt.Rows[i]["Link"].ToString();

                        }
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";

            }
            return _return;
        }

        public static string GetFullNameByID(object _id)
        {
            string _return = null;

            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select ProductName FROM T_Products Where Product_ID =" + _id);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == null)
                        {
                            _return = _dt.Rows[i]["ProductName"].ToString();

                        }
                    }
                }
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                _return = "";

            }
            return _return;
        }

        public static ArrayList GetDataKeysFromDataGrid(DataGrid _datagrid, ArrayList arr)
        {
            foreach (DataGridItem m_Item in _datagrid.Items)
            {
                CheckBox chk_Select = (CheckBox)m_Item.FindControl("optSelect");
                if (chk_Select != null && chk_Select.Checked)
                {
                    arr.Add(double.Parse(_datagrid.DataKeys[int.Parse(m_Item.ItemIndex.ToString())].ToString()));
                }
            }
            return arr;
        }
        public static void Log_Action(int UserID, string Fullname, DateTime datemodify, int Machucnang, string ActionsCode)
        {
            ActionHistoryDAL dal = new ActionHistoryDAL();
            T_ActionHistory _t_action = new T_ActionHistory();
            _t_action.UserID = UserID;
            _t_action.FullName = Fullname;
            _t_action.HostIP = IpAddress();
            _t_action.DateModify = datemodify;
            _t_action.ActionsCode = ActionsCode;
            _t_action.Menu_ID = Machucnang;
            dal.InserT_Action(_t_action);
        }
        public static string IpAddress()
        {
            HttpContext hc1 = HttpContext.Current;
            string strIp;
            strIp = hc1.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIp == null)
            {
                strIp = hc1.Request.ServerVariables["REMOTE_ADDR"];
            }
            return strIp;
        }
        #region "Function Copy News From Orther website"
        private static string DownLoadImg2Server(string ImageSource, string ImgaeDest, string _UserID, string DesPathVirtual, string DestPathNoVirtual)
        {
            string imageUrl = ImageSource;
            string saveLocation = ImgaeDest;
            string NewFileName = ImageSource.Substring(ImageSource.LastIndexOf("/") + 1);
            string NewFileNameExte = NewFileName.Substring(NewFileName.LastIndexOf("."));
            saveLocation = saveLocation + NewFileName;
            string _DesPathVirtual = DesPathVirtual + NewFileName;
            string _DestPathNoVirtual = DestPathNoVirtual + NewFileName;
            byte[] imageBytes;
            HttpWebRequest imageRequest = (HttpWebRequest)WebRequest.Create(imageUrl);
            WebResponse imageResponse = imageRequest.GetResponse();

            Stream responseStream = imageResponse.GetResponseStream();

            using (BinaryReader br = new BinaryReader(responseStream))
            {
                imageBytes = br.ReadBytes(500000);
                br.Close();
            }
            responseStream.Close();
            imageResponse.Close();

            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(imageBytes);
            }


            finally
            {
                fs.Close();
                bw.Close();
            }

            //phan insert co so du lieu
            T_ImageFiles _obj = new T_ImageFiles();
            ImageFilesDAL _DAL = new ImageFilesDAL();
            _obj = SetItem(NewFileName, 0, _DestPathNoVirtual, NewFileNameExte, Convert.ToInt16(_UserID), 2, 0);
            _DAL.InsertT_ImageFiles(_obj);


            return _DesPathVirtual;
        }
        private static T_ImageFiles SetItem(string _tenFile, double _size, string _pathfile, string _extenfile, int _userID, Int16 vType, double chuyenmuc)
        {
            T_ImageFiles _obj = new T_ImageFiles();
            _obj.ImageFileName = _tenFile.ToString();
            _obj.ImageFileSize = _size;
            _obj.ImageFileExtension = _extenfile.ToString();
            _obj.ImageType = vType;
            _obj.ImgeFilePath = "/" + _pathfile.ToString();
            _obj.Status = 0;
            _obj.UserCreated = _userID;
            _obj.DateCreated = DateTime.Now;
            _obj.Categorys_ID = chuyenmuc;

            return _obj;
        }
        public static string FindAndReplcaceSrc(string htmlSource, string DestPath, string UserID, string DestPathVirtual, string _DestPathNoVirtul)
        {

            string PathAfterDownload = "";
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlSource);

            foreach (var item in doc.DocumentNode.SelectNodes("//img[@src]"))//select only those img that have a src attribute..ahh not required to do [@src] i guess
            {
                //string matchString = Regex.Match(htmlSource, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                if (item.Attributes["src"].Value.Contains("http://"))
                {
                    PathAfterDownload = DownLoadImg2Server(item.Attributes["src"].Value, DestPath, UserID, DestPathVirtual, _DestPathNoVirtul);
                    item.Attributes["src"].Value = PathAfterDownload;
                }

            }

            return doc.DocumentNode.InnerHtml.ToString();

        }

        public static string ReplapceYoutoubeWidth(string str, string _Width)
        {
            try
            {
                Regex regex = new Regex(
                    @"(?<=<iframe[^<]+?width=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(str);
                for (int i = 0; i < matchCollect.Count; i++)
                {
                    string _url = matchCollect[i].Value.Trim();
                    if (_url.Length > 0)
                    {
                        if (!_url.StartsWith("http"))
                        {
                            string _urlImg = matchCollect[i].Value.Trim();
                            string _urlImgReplate = _Width;
                            if (!str.ToLower().Contains(_urlImgReplate.Trim().ToLower()))
                                str = System.Text.RegularExpressions.Regex.Replace(str, _urlImg, _urlImgReplate, RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            catch { }
            return str;
        }
        public static string ReplapceYoutoubeHight(string str, string _Hight)
        {
            try
            {
                Regex regex = new Regex(
                    @"(?<=<iframe[^<]+?height=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(str);
                for (int i = 0; i < matchCollect.Count; i++)
                {
                    string _url = matchCollect[i].Value.Trim();
                    if (_url.Length > 0)
                    {
                        if (!_url.StartsWith("http"))
                        {
                            string _urlImg = matchCollect[i].Value.Trim();
                            string _urlImgReplate = _Hight;
                            if (!str.ToLower().Contains(_urlImgReplate.Trim().ToLower()))
                                str = System.Text.RegularExpressions.Regex.Replace(str, _urlImg, _urlImgReplate, RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            catch { }
            return str;
        }
        public static bool CheckFrames(string str)
        {
            bool _frame = false;
            try
            {
                Regex regex = new Regex(
                    @"(?<=<iframe[^<]+?height=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(str);
                for (int i = 0; i < matchCollect.Count; i++)
                {
                    string _url = matchCollect[i].Value.Trim();
                    if (_url.Length > 0)
                    {
                        if (!_url.StartsWith("http"))
                        {
                            _frame = true;
                        }
                    }
                }
            }
            catch { }
            return _frame;
        }
        public static int CountImgTag(string str)
        {
            int _return = 0;
            try
            {
                Regex regex = new Regex(
                    @"(?<=<img[^<]+?src=\"")[^\""]+  ",
                    RegexOptions.IgnoreCase
                    | RegexOptions.Multiline
                    | RegexOptions.IgnorePatternWhitespace
                    | RegexOptions.Compiled
                    );
                MatchCollection matchCollect = regex.Matches(str);
                _return = matchCollect.Count;
            }
            catch { }
            return _return;
        }
        #endregion
        #region ADD NEW BY NVTHAI
        public static int GetParentIDSugg(int _ID)
        {
            int _return = 0;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select ParrentID FROM T_Suggestions Where ID=" + _ID);

                DataTable _dt = _untilDAL.ExecSqlDataSet(_sql).Tables[0];
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_return == 0)
                        {
                            _return = Convert.ToInt32(_dt.Rows[i]["ParrentID"].ToString());
                        }
                    }
                }
                else
                    _return = 0;
            }
            catch (Exception ex)
            {
                _return = 0;

            }
            return _return;
        }
        public static string ReturnPath_Images(Object _path)
        {
            string strReturn = "";
            if (_path.ToString() != "")
            {
                string _linkImage = ConfigurationManager.AppSettings["ServerPathDis"] + UrlPathImage_RemoveUpload(_path.ToString());
                strReturn = "<img style=\"cursor:pointer;border:0px;\" alt=\"Xem ảnh\" title=\"Xem ảnh\" src=\"" + ConfigurationManager.AppSettings["tinpath"] + _path.ToString() + "\" onclick=\"ViewImages('" + ConfigurationManager.AppSettings["tinpath"] + _path.ToString() + "');\" />";
            }
            else
                strReturn = "<img style=\"cursor:pointer;border:0px;\" alt=\"Xem ảnh\" title=\"Xem ảnh\" src=\"" + ConfigurationManager.AppSettings["ApplicationPath"] + "/Images/no_images.jpg" + "\" />";

            return strReturn;
        }

        public static void GenCacheHTML()
        {
            try
            {
                int _gencach = Convert.ToInt32(ConfigurationManager.AppSettings["GenCache"].ToString());
                if (_gencach == 1)
                {

                    HPC_GenerateHtml _genDAL = new HPC_GenerateHtml();

                    if (File.Exists(@"" + ConfigurationManager.AppSettings["PhysicalFullPath"].ToString() + ""))
                    {
                        File.Delete(@"" + ConfigurationManager.AppSettings["PhysicalFullPath"].ToString() + "");
                    }
                    _genDAL.GenerateHTML(ConfigurationManager.AppSettings["Url"].ToString(), ConfigurationManager.AppSettings["PhysicalFullPath"].ToString());

                }
            }
            catch
            { };
        }
        public static DataTable GetAllNewsRelation(string _listID)
        {
            DataTable _return = null;
            try
            {
                UltilFunc _untilDAL = new UltilFunc();
                string _sql = "";
                _sql = string.Format("Select News_ID,News_Tittle FROM T_News Where News_ID in (" + _listID + ")");

                _return = _untilDAL.ExecSqlDataSet(_sql).Tables[0];

            }
            catch (Exception ex)
            {
                _return = null;
            }
            return _return;
        }
        public static int ReturnTotalNhuanbut(DataTable _dtReport)
        {
            int _return = 0;
            try
            {
                if (_dtReport.Rows.Count > 0)
                {
                    for (int i = 0; i <= _dtReport.Rows.Count - 1; i++)
                    {
                        if (_dtReport.Rows[i]["Total"].ToString() != "")
                            _return += Convert.ToInt32(_dtReport.Rows[i]["Total"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                _return = 0;
            }
            return _return;
        }
        #endregion

        public static string SelectLoaiBenXe(string _where)
        {
            string _return;
            DataSet _ds;
            string _sql = "select Ten_loai_BX from T_Loai_Benxe where ID =  " + _where;

            try
            {
                _ds = HPCDataProvider.Instance().ExecSqlDataSet(_sql);
                if (_ds.Tables[0].Rows.Count > 0)
                    _return = _ds.Tables[0].Rows[0][0].ToString();
                else
                    _return = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _return;
        }

        #region CHECK DATE SEARCH
        public static bool checkDate(string _dateFrom,string _dateTo)
        {
            bool success = true;
            CultureInfo cultureInfo = new CultureInfo("fr-FR");
            if (!string.IsNullOrEmpty(_dateFrom.Trim()))
            {
                try
                {
                    DateTime.Parse(_dateFrom.Trim(), cultureInfo);
                }
                catch
                {
                    success = false;
                }
            }
            if (!string.IsNullOrEmpty(_dateTo.Trim()))
            {
                try
                {
                    DateTime.Parse(_dateTo.Trim(), cultureInfo);
                }
                catch
                {
                    success = false;
                }
            }

            return success;
        }
        public static string ReturnMonthBefore(DateTime dt)
        {
            string _return = "";
            int _thang = dt.Month - 1;
            if (_thang == 0)
                _return = "12/" + (dt.Year - 1).ToString();
            else
            {
                if (_thang.ToString().Length == 1)
                    _return = "0" + _thang + "/" + dt.Year;
            }
            return _return;

        }

        public static string ReturnMonthBefore(string dt)
        {
            string _return = "";

            int _thang = 0;int _nam = 0;
            char[] sep = { '-' };
            string[] sArrProdID = null;
            if (dt.ToString() != "")
            {
                sArrProdID = dt.Split(sep);
                _thang = Int32.Parse(sArrProdID[0].ToString()) - 1;
                _nam = Int32.Parse(sArrProdID[1].ToString());
                if (_thang == 0)
                    _return = "12-" + (_nam - 1).ToString();
                else
                {
                    if (_thang.ToString().Length == 1)
                        _return = "0" + _thang + "-" + _nam;
                    else
                        _return = _thang + "-" + _nam;
                }
               
            }            
            return _return;

        }
        public static int ReturnDayInMonth(string dt)
        {
            int _return = 0;

            int _thang = 0; int _nam = 0;
            char[] sep = { '-' };
            string[] sArrProdID = null;
            if (dt.ToString() != "")
            {
                sArrProdID = dt.Split(sep);
                _thang = Int32.Parse(sArrProdID[0].ToString());
                _nam = Int32.Parse(sArrProdID[1].ToString());

                _return = DateTime.DaysInMonth(_nam, _thang);               

            }
            return _return;

        }

        public static string ReturnTotalInDay(string total,int day)
        {
            string _return = "0";
                        
            double _average = 0;
            if (total.ToString() != "0")
            {
                double _tt = Convert.ToDouble(total.ToString());
                double _day = Convert.ToDouble(day.ToString());
                _average = Convert.ToDouble(_tt / _day);
                _return = Math.Round(_average, 2).ToString();              

            }
            return _return;

        }

        public static string ReturnPhanTramInDay(string total1, string total2)
        {
            string _return = "0 %";

            double _average = 0;
            if (total1.ToString() != "0")
            {
                double _t1 = Convert.ToDouble(total1.ToString());
                double _t2 = Convert.ToDouble(total2.ToString());
                _average = Convert.ToDouble(_t2/_t1 )* 100;
                _return = Math.Round(_average,0).ToString();

            }
            return _return + " %";

        }

        public static int ReturnUseFix()
        {
            int _return =0;            
            if (ConfigurationManager.AppSettings["ReportFix"].ToString() != "0")
            {
                _return = Int32.Parse(ConfigurationManager.AppSettings["ReportFix"].ToString());
            }
            return _return;
        }

        #endregion


    }
}
