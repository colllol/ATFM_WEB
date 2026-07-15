using prjBusinessLogic;
using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjApplication
{
    public class PageBaseCallBack : System.Web.UI.Page, ICallbackEventHandler
    {
        public string _EventArgument;
        public virtual string GetCallbackResult()
        {
            throw new NotImplementedException();
        }

        public void RaiseCallbackEvent(string eventArgument)
        {
            _EventArgument = eventArgument;
        }
        protected void RegisScriptCallBack()
        {
            string strScript = ClientScript.GetCallbackEventReference(this, "arg", "DisplayResult", "BQLL", false);
            string strCallScript = @"function GetArgWithPostBack(arg,BQLL){" + strScript + ";}";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "GetArgWithPostBack", strCallScript, true);
        }
        protected override void OnLoad(EventArgs e)
        {
            

            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != String.Empty)
            {
                var regex = new Regex(@"^[a-zA-Z0-9]+$");

                //txtStartDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                //txtFinishDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                if (!regex.IsMatch(Request["Menu_ID"])) return;
                

                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    _Role = _userDAL.GetRole4UserMenu(_user.UserID, Convert.ToInt32(Request["Menu_ID"]));
                }
            }
            RegisScriptCallBack();
            base.OnLoad(e);
        }

        #region instan
        protected prjInfo.T_RolePermission _Role = null;
        private bool _refreshState;
        private bool _isRefresh;
        private int UserID
        {
            get { if (ViewState["UserID"] != null) return Convert.ToInt32(ViewState["UserID"]); else return 0; }

            set { ViewState["UserID"] = value; }
        }
        //protected override void LoadViewState(object savedState)
        //{
        //    try
        //    {
        //        object[] AllStates = (object[])savedState;
        //        base.LoadViewState(AllStates[0]);
        //        _refreshState = bool.Parse(AllStates[1].ToString());
        //        _isRefresh = _refreshState ==
        //        bool.Parse(Session["__ISREFRESH"].ToString());
        //    }
        //    catch
        //    { }
        //}
        //protected override object SaveViewState()
        //{
        //    Session["__ISREFRESH"] = _refreshState;
        //    object[] AllStates = new object[2];
        //    AllStates[0] = base.SaveViewState();
        //    AllStates[1] = !(_refreshState);
        //    return AllStates;
        //}
        protected prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        protected prjInfo.T_Users _user = null;
        #endregion

        #region function
        protected string rListHistoryFlightDetails(System.Data.DataTable dt, string keyId)
        {
            string kq = "";
            try
            {

                #region old
                //string STT = "", lastUser = "", ngayUpdate = "", changeInfo = "";
                //DataTable dt = new PermMasterNoDAL().GetHistoryById(id);
                //if (dt.Rows.Count < 1)
                //    return "";
                //kq += "<div class=\"row\">";
                //STT += "<div class=\"col-lg-1\">";
                //STT += "<div class=\"wid_50px label label-lg label-info arrowed-right\">";
                //STT += "<b>No</b>";
                //STT += "</div>";
                //STT += "<div class=\"wid_50px\">";
                //STT += "<ul class=\"list-unstyled spaced\">";
                //STT += "</ul>";
                //STT += "</div>";
                //STT += "</div>";
                //lastUser += "<div class=\"col-lg-2\">";
                //lastUser += "<div class=\"wid_200px label label-lg label-warning arrowed-in arrowed-right\">";
                //lastUser += "<b>Last user update</b>";
                //lastUser += "</div>";
                //lastUser += "<div>";
                //lastUser += "<ul class=\"list-unstyled spaced\">";
                //lastUser += "</ul>";
                //lastUser += "</div>";
                //lastUser += "</div>";
                //ngayUpdate += "<div class=\"col-lg-2\">";
                //ngayUpdate += "<div class=\"wid_200px label label-lg label-danger arrowed-in arrowed-right\">";
                //ngayUpdate += "<b>Date update</b>";
                //ngayUpdate += "</div>";
                //ngayUpdate += "<div>";
                //ngayUpdate += "<ul class=\"list-unstyled spaced\">";
                //ngayUpdate += "</ul>";
                //ngayUpdate += "</div>";
                //ngayUpdate += "</div>";
                //changeInfo += "<div class=\"col-lg-1\">";
                //changeInfo += "<div class=\"wid_100px label label-lg label-success arrowed-in\">";
                //changeInfo += "<b>Details</b>";
                //changeInfo += "</div>";
                //changeInfo += "<div class=\"wid_100px\">";
                //changeInfo += "<ul class=\"list-unstyled spaced\">";
                //changeInfo += "</ul>";
                //changeInfo += "</div>";
                //changeInfo += "</div>";
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    STT += "<li class=\"text-center\">";
                //    STT += (i + 1).ToString();
                //    STT += "</li>";
                //    lastUser += "<li>";
                //    lastUser += $"<i class=\"ace-icon fa fa-caret-right blue\"></i>{dt.Rows[i]["LAST_USER"].ToString()}";
                //    lastUser += "</li>";
                //    ngayUpdate += "<li>";
                //    ngayUpdate += $"<i class=\"ace-icon fa fa-caret-right blue\"></i>{Convert.ToDateTime(dt.Rows[i]["TIME_UPDATE"].ToString()).ToString("dd/MM/yyyy")}";
                //    ngayUpdate += "</li>";
                //    changeInfo += "<li class=\"text-center\">";
                //    changeInfo += $"<a><i class=\"glyphicon glyphicon-zoom-in\" onclick=\"ShowDetailNoEdit({id});\"></i></a>";
                //    changeInfo += "</li>";
                //}
                //kq += STT + lastUser + ngayUpdate + changeInfo;
                //kq += "</div>";
                #endregion

                #region his style table
                //kq += RenderHistoreById(id);
                #endregion

                #region render table html                
                //DataTable dt = new PermMasterNoDAL().GetHistoryById(id);
                if (dt.Rows.Count < 1)
                    return "";
                kq += "<table class=\"table\">";
                kq += "<thead>";
                kq += "<tr>";
                kq += $"<th>#</th>";                //colum 1
                kq += $"<th>No</th>";               //colum 2
                kq += $"<th>Last user</th>";        //colum 3
                kq += $"<th>Last modify</th>";      //colum 4
                kq += $"<th>Conten change</th>";    //colum 5
                kq += $"<th>Action</th>";           //colum 6
                kq += "</tr>";
                kq += "</thead>";
                kq += "<tbody>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    kq += "<tr>";
                    //colum 1
                    kq += "<td>";
                    kq += "<div class=\"action-buttons\">";
                    kq += "<a data-toggle=\"tooltip\" title=\"Restore\">";
                    kq += "<i class=\"glyphicon glyphicon-refresh bigger-130\"";
                    kq += $" onclick=\"RestoreHistory({keyId}, {dt.Rows[i]["NOVERSION"].ToString()},{_user.UserID.ToString()});\"></i>";
                    kq += "</a>";
                    kq += "</div>";
                    kq += "</td>";
                    //colum 2
                    kq += $"<td>{(i + 1).ToString()}</td>";
                    //colum 3
                    kq += $"<td>{dt.Rows[i]["LASTUSER"].ToString()}</td>";
                    //colum 4
                    kq += $"<td>{Convert.ToDateTime(dt.Rows[i]["LASTMODIFY"].ToString()).ToString("dd/MM/yyyy hh:mm:ss")}</td>";
                    //colum 5
                    kq += $"<td>{dt.Rows[i]["CONTENT"].ToString()}</td>";
                    //colum 6
                    kq += $"<td>{dt.Rows[i]["ACTION"].ToString()}</td>";
                    kq += "</tr>";
                }
                kq += "</tbody>";
                kq += "</table>";
                #endregion

            }
            catch
            {
            }
            return kq;
        }
        protected string Createddl<T>(string id, List<T> source, string valueField, string textField)
        {
            DropDownList ddl = new DropDownList();
            ddl.ID = id;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataSource = source;
            ddl.DataBind();
            return this.RenderToHTML(ddl);
        }
        #endregion
    }
}