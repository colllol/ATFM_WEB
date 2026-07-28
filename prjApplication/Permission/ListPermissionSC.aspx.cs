using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using TuesPechkin;
using System.Web.UI.HtmlControls;

namespace prjApplication.Permission
{
    public partial class ListPermissionSC : PageBaseCallBack
    {
        public string _Tite = "Edit Permission";
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "ListPermissionSC";

        #region newList
        public string _ListAero
        {
            get
            {
                string kq = "";
                foreach (var item in new AeroDAL().GetListAll())
                {
                    kq += item.AE_CODE + ",";
                }
                return kq.Substring(0, kq.Length - 1);
            }
        }
        public string _ListPurpose
        {
            get
            {
                string kq = "";
                foreach (var item in new FlyPurposeDAL().GetAllObject())
                {
                    kq += item.PURPOSE_CODE + ",";
                }
                return kq.Substring(0, kq.Length - 1);
            }
        }
        public string _ListCraft
        {
            get
            {
                string kq = "[";
                foreach (var item in new CraftTypeDAL().GetAllCraftType())
                {
                    kq += "{\"craftid\":\"" + item.ID + "\", \"name\":\"" + item.MA + "\"},";
                }
                kq = kq.Substring(0, kq.Length - 1);
                return kq + "]";
            }
        }
        public string _ListOper
        {
            get
            {
                string kq = "";
                foreach (var item in new OperDAL().GetAllObject())
                {
                    kq += item.OPER_ICAO + ",";
                }
                return kq.Substring(0, kq.Length - 1);
            }
        }
        #endregion
        public string _ObjRender
        {
            get
            {
                PermMasterSc obj = new PermMasterSc();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }        
        #region page event
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                LoadData();
                //ddl_Load();
                btnAddNew.Enabled = _Role.R_Add;
                

            }
        }
        #endregion

        #region SEARCH
        public DataView BindGridData(DataTable _dt)
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr;

                dt.Columns.Add(new DataColumn("PERMNBR_ID", typeof(string)));                
                dt.Columns.Add(new DataColumn("AUTHOR_NAME", typeof(string)));
                dt.Columns.Add(new DataColumn("PERMTYPE", typeof(string)));
                dt.Columns.Add(new DataColumn("PERMNBR", typeof(string)));
                dt.Columns.Add(new DataColumn("VERSION", typeof(string)));
                dt.Columns.Add(new DataColumn("PERMDATE", typeof(string)));
                dt.Columns.Add(new DataColumn("OPER_ID", typeof(string)));
                dt.Columns.Add(new DataColumn("REFERENCE", typeof(string)));
                dt.Columns.Add(new DataColumn("VALIDHOURS", typeof(string)));                             
                dt.Columns.Add(new DataColumn("ID", typeof(string)));
                dt.Columns.Add(new DataColumn("BEGINDATE", typeof(string)));
                dt.Columns.Add(new DataColumn("ENDDATE", typeof(string)));
                dt.Columns.Add(new DataColumn("SEASON", typeof(string)));
                dt.Columns.Add(new DataColumn("PERM_ID", typeof(string)));
                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        if (Session["MySearchPERMSC"] != null)
                        {
                            DataTable dtseach = (DataTable)Session["MySearchPERMSC"];
                            dt.Rows.Add(dtseach.Rows[0]["PERMNBR_ID"].ToString(), dtseach.Rows[0]["AUTHOR_NAME"].ToString(), dtseach.Rows[0]["PERMTYPE"].ToString(), dtseach.Rows[0]["PERMNBR"].ToString(), dtseach.Rows[0]["VERSION"].ToString(), dtseach.Rows[0]["PERMDATE"].ToString(), dtseach.Rows[0]["OPER_ID"].ToString(), dtseach.Rows[0]["REFERENCE"].ToString(), dtseach.Rows[0]["VALIDHOURS"].ToString(), "0", dtseach.Rows[0]["BEGINDATE"].ToString(), dtseach.Rows[0]["ENDDATE"].ToString(), dtseach.Rows[0]["SEASON"].ToString(), dtseach.Rows[0]["PERM_ID"].ToString());
                        }
                        else
                            dt.Rows.Add("", "", "", "", "", "", "", "", "", "0", "", "","","");
                        for (int i = 0; i < _dt.Rows.Count; i++)
                        {
                            dr = dt.NewRow();
                            dr[0] = _dt.Rows[i]["PERMNBR_ID"].ToString();                            
                            dr[1] = _dt.Rows[i]["AUTHOR_NAME"].ToString();
                            dr[2] = _dt.Rows[i]["PERMTYPE"].ToString();
                            dr[3] = _dt.Rows[i]["PERMNBR"].ToString();
                            dr[4] = _dt.Rows[i]["VERSION"].ToString();
                            if (_dt.Rows[i]["PERMDATE"] != Convert.DBNull)
                            {
                                DateTime _dtleter = Convert.ToDateTime(_dt.Rows[i]["PERMDATE"].ToString());
                                if (_dtleter != DateTime.MinValue)
                                    dr[5] = _dtleter.ToString("dd/MM/yyyy");
                                else
                                    dr[5] = "";
                            }
                            else
                                dr[5] = "";
                            
                            dr[6] = _dt.Rows[i]["OPER_ID"].ToString();
                            dr[7] = _dt.Rows[i]["REFERENCE"].ToString();
                            dr[8] = _dt.Rows[i]["VALIDHOURS"].ToString();                            
                            dr[9] = _dt.Rows[i]["ID"].ToString();
                            if (_dt.Rows[i]["BEGINDATE"] != Convert.DBNull)
                            {
                                DateTime _dtbigin = Convert.ToDateTime(_dt.Rows[i]["BEGINDATE"].ToString());
                                if (_dtbigin != DateTime.MinValue)
                                    dr[10] = _dtbigin.ToString("dd/MM/yyyy");
                                else
                                    dr[10] = "";
                            }
                            else
                                dr[10] = "";

                            if (_dt.Rows[i]["ENDDATE"] != Convert.DBNull)
                            {
                                DateTime _dtend = Convert.ToDateTime(_dt.Rows[i]["ENDDATE"].ToString());
                                if (_dtend != DateTime.MinValue)
                                    dr[11] = _dtend.ToString("dd/MM/yyyy");
                                else
                                    dr[11] = "";
                            }
                            else
                                dr[11] = "";                            
                            dr[12] = _dt.Rows[i]["SEASON"].ToString();
                            dr[13] = _dt.Rows[i]["PERM_ID"].ToString();
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    if (Session["MySearchPERMSC"] != null)
                    {
                        DataTable dtseach = (DataTable)Session["MySearchPERMSC"];
                        dt.Rows.Add(dtseach.Rows[0]["PERMNBR_ID"].ToString(), dtseach.Rows[0]["AUTHOR_NAME"].ToString(), dtseach.Rows[0]["PERMTYPE"].ToString(), dtseach.Rows[0]["PERMNBR"].ToString(), dtseach.Rows[0]["VERSION"].ToString(), dtseach.Rows[0]["PERMDATE"].ToString(), dtseach.Rows[0]["OPER_ID"].ToString(), dtseach.Rows[0]["REFERENCE"].ToString(), dtseach.Rows[0]["VALIDHOURS"].ToString(), "0", dtseach.Rows[0]["BEGINDATE"].ToString(), dtseach.Rows[0]["ENDDATE"].ToString(), dtseach.Rows[0]["SEASON"].ToString(), dtseach.Rows[0]["PERM_ID"].ToString());
                    }
                    else
                        dt.Rows.Add("", "", "", "", "", "", "", "", "", "0", "", "", "", "");
                }
                DataView dv = new DataView(dt);
                return dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetWhereConditionInGrid()
        {
            string where = " 1=1 ";
            
                    

                    if (!String.IsNullOrEmpty(txtSearchPERMNBR.Value.Trim()))
                        where += " AND " + string.Format(" UPPER(PERMNBR_ID) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchPERMNBR.Value.Trim().ToUpper()));
                    //if (!String.IsNullOrEmpty(txtSearchAUTHOR.Value.Trim()))
                    //    where += " AND " + string.Format(" UPPER(AUTHOR_NAME) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchAUTHOR.Value.Trim()));
                    if (!String.IsNullOrEmpty(txtSearchTYPE.Value.Trim()))
                        where += " AND " + string.Format(" UPPER(PERMTYPE) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchTYPE.Value.Trim().ToUpper()));
                    if (!String.IsNullOrEmpty(txtSearchUser.Value.Trim()))
                        where += " AND " + string.Format(" UPPER(LASTUSER) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchUser.Value.Trim().ToUpper()));
                    //if (!String.IsNullOrEmpty(txtSearchFTYPE.Value.Trim()))
                       // where += " AND " + string.Format(" UPPER(FLIGHTTYPE) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchFTYPE.Value.Trim()));


            if (!String.IsNullOrEmpty(txtSearchNUMBER.Value.Trim()))
                        where += " AND " + string.Format(" UPPER(PERMNBR) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchNUMBER.Value.Trim()));

                    //if (!String.IsNullOrEmpty(txtSearchVERSION.Value.Trim()))
                    //    where += " AND " + string.Format(" UPPER(VERSION) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchVERSION.Value.Trim()));
                    if (!String.IsNullOrEmpty(txtSearchDATE.Value.Trim()))
                        where += " AND PERMDATE =" + whereDateHelper(txtSearchDATE.Value.Trim()) + "";
                    if (!String.IsNullOrEmpty(txtSearchOPER.Value.Trim()))
                        where += " AND " + string.Format(" UPPER(OPER_ID) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchOPER.Value.Trim()));
                    //if (!String.IsNullOrEmpty(txtSearchREFERENCE.Value.Trim()))
                    //    where += " AND " + string.Format(" UPPER(REFERENCE) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchREFERENCE.Value.Trim()));
                    //if (!String.IsNullOrEmpty(txtSearchVALIDHOURS.Value.Trim()))
                    //    where += " AND VALIDHOURS = " + Convert.ToInt32(txtSearchVALIDHOURS.Value.Trim());
                 
                    //if (!String.IsNullOrEmpty(txtSearchSEASON.Value.Trim()))
                    //    where += " AND " + string.Format(" UPPER(SEASON) like '%{0}%'", UltilFunc.SqlFormatText(txtSearchSEASON.Value.Trim()));
                
            return HttpUtility.UrlEncode(where.ToUpper());
           
        }
        private string whereDateHelper(string value)
        {
            return $"TO_DATE('{value}', 'DD-MM-YYYY')";
        }
      
        protected void linkSearch_Click(object sender, EventArgs e)
        {
            PhanTrang1.PageIndex = 0;
            LoadData();
        }

        

        #endregion


        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Permission/Edit_PermSC.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }

        #region Call back

        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { "::::" }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "btnUpdateOnclick":
                    kq = btnUpdateOnclick(_arg[0]);
                    break;
                case "btnCreateOnclick":
                    kq = btnCreateOnclick(_arg[0]);
                    break;
                case "GetOneFlight":
                    kq = GetOneFlight(_arg[0]);
                    break;
                case "LoadDataGrid":
                    kq = LoadDataGrid(ThamSo);
                    break;
                case "btnDeleteOnclick":
                    kq = btnDeleteOnclick(_arg[0]);
                    break;
                case "mbtnAddNewFlightDetailOnclick":
                    kq = mbtnAddNewFlightDetailOnclick(_arg[0]);
                    break;
                case "mShowDetail":
                    kq = mShowDetail(_arg[0]);
                    break;
                case "mDeleteRowOnclick":
                    kq = mDeleteRowOnclick(_arg[0]).ToLower();
                    break;
                case "mbtnUpdateFlightDetailOnclick":
                    kq = mbtnUpdateFlightDetailOnclick(_arg[0]);
                    break;
                case "RestoreHistory":
                    kq = RestoreHistory(ThamSo);
                    break;
                case "btnSearchExtension_Click":
                    kq = btnSearchExtension_Click(_arg[0]);
                    break;
            }
            return kq;
        }

        #endregion

        #region function        
        void LoadData()
        {
            List<PermMasterSc> t = new PermMasterScDAL().GetPageObject(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            PhanTrang1.TotalRecord = t.GetTotalRecord<PermMasterSc>();
            rptSource.DataSource = t;
            rptSource.DataBind();
        }
        void ddl_Load()
        {
            this.FillDropdownList(ddlAUTHOR_ID, new FpAuthorDAL().GetAllObject(), "AUTHOR_NAME", "AUTHOR_CODE");
            this.FillDropdownList(ddlOPER_ID, new OperDAL().GetAllObject(), "OPER_NAME", "OPER_ICAO");
            this.FillDropdownList(mAUTHOR_ID, new FpAuthorDAL().GetAllObject(), "AUTHOR_NAME", "AUTHOR_CODE");
            this.FillDropdownList(mOPER_ID, new OperDAL().GetAllObject(), "OPER_NAME", "OPER_ICAO");
        }
        private string GetPERMNBR_ID(PermMasterSc obj)
        {
            return "";
            //return new PermDetailScDAL().GetPERMNBR_ID(obj);
        }
        private string LoadDataGrid(string[] ThamSo)
        {
            List<PermMasterSc> t = new PermMasterScDAL().GetPageObject(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            PhanTrang1.TotalRecord = t.GetTotalRecord<PermMasterSc>();
            rptSource.DataSource = t;
            rptSource.DataBind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            rptSource.RenderControl(hw);
            return sb.ToString();
        }
        private string RestoreHistory(string[] thamso)
        {
            bool kq = new PermMasterScDAL().RestoreRecode(thamso[0], thamso[1], thamso[2]);
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        private string btnUpdateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermMasterSc>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            bool kq = new PermMasterScDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnDeleteOnclick(string thamso)
        {
            bool kq = new PermMasterScDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string btnCreateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermMasterSc>(ThamSo, dateTimeConverter);
            //obj.LASTUSER = _user.UserID.ToString();
            //obj.USER_NAME = _user.UserID.ToString();
            obj.ID = 0;
            bool kq = new PermMasterScDAL().InsertObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Insert sussess" : "Insert error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[InsertObject] [{ax}]", 0.0);
            if (!kq)
                return "Insert error";
            return "Insert sussess";
        }
        private string GetOneFlight(string ThamSo)
        {            
            PermMasterSc obj = new PermMasterScDAL().GetOneObject(ThamSo);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private void PutclsSearchDetail()
        {
            List<clsSearchDetail> lisSearch = new List<clsSearchDetail>();
            //lisSearch.Add(new clsSearchDetail("PERMNBR_ID", "txtSearch_UserName", txtSearch_UserName.Text.Trim()));
            Session.SetValueSearch(_AliasSession, lisSearch);
        }
        private string mbtnAddNewFlightDetailOnclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermMasterSc>(thamso, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            obj.ID = 0;
            string id = new PermMasterScDAL().InsertReturnId(obj);
            string ax = id == "-1".ToString() ? "Insert sussess" : "Insert error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[InsertReturnId] [{ax}]", 0.0);
            return id;
        }
        private string mShowDetail(string thamso)
        {
            return GetOneFlight(thamso);
        }
        private string mDeleteRowOnclick(string thamso)
        {
            bool kq = new PermMasterScDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            return kq.ToString();
        }
        private string mbtnUpdateFlightDetailOnclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermMasterSc>(thamso, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            bool kq = new PermMasterScDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            return kq.ToString().ToLower();
        }
        private string btnSearchExtension_Click(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<object>(thamso, dateTimeConverter);
            DataTable dt1 = new clsResuftAPI().GetTableApiExtension("PERM_PKG", "SearchExtentsion", obj);
            if (dt1 == null || dt1.Rows.Count<1) return "0";
            string kq = string.Empty;
            foreach (DataRow r in dt1.Rows)
            {
                kq += $"<tr>";

                /*
                 * 
                 <a href="#" onclick="window.open('<%= Page.ResolveUrl("~/SendMessage/SendMessageFlight.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)"
                        data-toggle="tooltip" title="Send message">
                 */
                kq += $"<td><a href='#' onclick=\"window.open('{ Page.ResolveUrl("~/Permission/") }{ (r["NHAY"].ToString() == "NO" ? "Edit_PermNo.aspx" : "Edit_PermSc.aspx") }?Menu_Id={Request.Params["Menu_ID"]}&ID={ r["PERM_ID"].ToString()}','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)\" data-toggle=\"tooltip\" title=\"Go to Perm\">{r["Permnbr_id"]}</a></td>";
                kq += $"<td>{r["author_id"]}</td>";
                kq += $"<td>{r["NHAY"]}</td>";
                kq += $"<td>{r["permnbr"]}</td>";
                kq += $"<td>{r["Version"]}</td>";
                kq += $"<td>{DateTime.Parse(r["permdate"].ToString()).ToString("dd/MM/yyyy")}</td>";
                kq += $"<td>{r["oper_id"]}</td>";
                kq += $"<td>{r["Reference"]}</td>";
                kq += $"<td>{r["Validhours"]}</td>";
                kq += $"<td>{r["Season"]}</td>";
                kq += $"<td>{r["NHAY"]}</td>";
                kq += "</tr>";
            }
            return kq;
        }
        #endregion


        #region grd event     
        protected void grdSource_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            switch (e.CommandName)
            {
                case "Edit":
                    Response.Redirect("~/Permission/Edit_PermSC.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + id.ToString());
                    break;
            }
        }
        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex != -1 && e.Row.RowType != DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                GridView grd = ((GridView)sender);
                gvRow.CssClass = "detail-row";
                TableCell tCell = new TableCell();
                tCell.CssClass = "text-left";
                tCell.ColumnSpan = ((GridView)sender).Columns.Count;
                string id = grd.DataKeys[e.Row.RowIndex].Value.ToString();

                if (id != "0")
                {
                    tCell.Text = rListHistoryFlightDetails(new PermMasterScDAL().GetHistoryById(id), id);
                    if (tCell.Text == "")
                        return;
                    gvRow.Cells.Add(tCell);
                    Table tbl = e.Row.Parent as Table;
                    tbl.Rows.Add(gvRow);
                }

                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFFFF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
            }
            if (e.Row.RowIndex == 0 && e.Row.RowType != DataControlRowType.Header)
            {
                TextBox txtColum01 = (TextBox)e.Row.FindControl("txtColum01");
                txtColum01.Visible = true;
                TextBox txtColum02 = (TextBox)e.Row.FindControl("txtColum02");
                txtColum02.Visible = true;
                TextBox txtColum03 = (TextBox)e.Row.FindControl("txtColum03");
                txtColum03.Visible = true;
                TextBox txtColum04 = (TextBox)e.Row.FindControl("txtColum04");
                txtColum04.Visible = true;
                TextBox txtColum05 = (TextBox)e.Row.FindControl("txtColum05");
                txtColum05.Visible = true;
                TextBox txtColum06 = (TextBox)e.Row.FindControl("txtColum06");
                txtColum06.Visible = true;
                TextBox txtColum07 = (TextBox)e.Row.FindControl("txtColum07");
                txtColum07.Visible = true;
                TextBox txtColum08 = (TextBox)e.Row.FindControl("txtColum08");
                txtColum08.Visible = true;
                TextBox txtColum09 = (TextBox)e.Row.FindControl("txtColum09");
                txtColum09.Visible = true;

               
                TextBox txtColum12 = (TextBox)e.Row.FindControl("txtColum12");
                txtColum12.Visible = true;

                Button btnSearch = (Button)e.Row.FindControl("linkSearch");
                btnSearch.Visible = true;

                HtmlControl htmlDivControl = (HtmlControl)e.Row.FindControl("divattribute");
                htmlDivControl.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol01 = (HtmlControl)e.Row.FindControl("col01");
                htmlcol01.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol02 = (HtmlControl)e.Row.FindControl("col02");
                htmlcol02.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol03 = (HtmlControl)e.Row.FindControl("col03");
                htmlcol03.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol04 = (HtmlControl)e.Row.FindControl("col04");
                htmlcol04.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol05 = (HtmlControl)e.Row.FindControl("col05");
                htmlcol05.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol06 = (HtmlControl)e.Row.FindControl("col06");
                htmlcol06.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol07 = (HtmlControl)e.Row.FindControl("col07");
                htmlcol07.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol08 = (HtmlControl)e.Row.FindControl("col08");
                htmlcol08.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol09 = (HtmlControl)e.Row.FindControl("col09");
                htmlcol09.Attributes.Add("style", "display:none;");

                //HtmlControl htmlcol10 = (HtmlControl)e.Row.FindControl("col10");
                //htmlcol10.Attributes.Add("style", "display:none;");

                //HtmlControl htmlcol11 = (HtmlControl)e.Row.FindControl("col11");
                //htmlcol11.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol12 = (HtmlControl)e.Row.FindControl("col12");
                htmlcol12.Attributes.Add("style", "display:none;");

                

                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#F1F5FA'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#D6E1EA'");

            }
        }

        #endregion

        #region pdf excel 

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            GridView ax = new GridView();
            ax.ID = "grdPDF";
            //ax.PreRender += GridView_PreRender;
            ax.Columns[0].Visible = false;
            ax.Columns[ax.Columns.Count - 1].Visible = false;
            ax.DataSource = new PermMasterScDAL().GetPagePermMasterScExport(GetWhereConditionInGrid());
            ax.DataBind();

            ax.RenderControl(htw);
            string html = sw.ToString();
            //divContent.InnerHtml += html;
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/assets/css/bootstrap1.min.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView ax = new GridView();
            ax.DataSource = new PermMasterScDAL().GetPagePermMasterScExport(GetWhereConditionInGrid());
            ax.DataBind();
            ax.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, DateTime.Now.ToFileTime() + ".xls");
        }
      
        protected void GridView_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            if (gv.ShowFooter == true && gv.Rows.Count > 0)
            {
                //Force GridView to use <tfoot> instead of <tbody> - 11/03/2013 - MCR.
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }

        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        #endregion

        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            var kq = new PermMasterScDAL().DeleteObject(((LinkButton)sender).Attributes["data-id"].ToString());
            if (kq) { 
                this.AlertMessage("Delete sussess!");
                PhanTrang1_Paging_IndexChange(sender, e);
            }
            else this.AlertMessage("Delete error!");
        }
    }
}
