using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;

namespace prjApplication.Static.ROUTE_LIST
{
    public partial class EditRoute : PageCoreAdmin
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != string.Empty)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    if (!IsPostBack)
                    {
                        LoadRouteListById();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
            }
        }

        private void LoadRouteListById()
        {
            if (Request["ID"] != null && Request["ID"].ToString() != "" && Request["ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["ID"]) == true)
                {
                    int _id = Convert.ToInt32(Request["ID"].ToString());
                    PopulateItem(_id);
                }
            }
        }

        private void PopulateItem(int _id)
        {           
            
            RouteList obj = new RouteListDAL().GetOneRouteList(_id);
            
            txtROUTE_NAME.Text = obj.ROUTE_NAME;
            txtDESCRIPTION.Text = obj.DESCRIPTION;

            if (!String.IsNullOrEmpty(obj.IS_OVERSEA.ToString()))
            {
                //if (obj.IS_OVERSEA.ToString() == "1")
                    //ckIS_OVERSEA.Checked = true;
            }
        }
        private bool CheckValidate()
        {
            //if (this.txtCountryCode.Text.Length <= 0)
            //{
            //    this.AlertMessage("Please enter a value Country code!");                
            //    return false;
            //}
            //if (this.txtCountryName.Text.Length <= 0)
            //{
            //    this.AlertMessage("Please enter a value Country name!");                
            //    return false;            
            //}
            return true;
        }

        private RouteList GetInfoRouteList()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            int routeId = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ROUTE_ID"].ToString()) : 0;
            string isOversea = "0";
            //if (ckIS_OVERSEA.Checked)
            //    isOversea = "1";
            return new RouteList()
            {
                ROUTE_ID = routeId,              
                ROUTE_NAME = txtROUTE_NAME.Text,
                DESCRIPTION = txtDESCRIPTION.Text,
                IS_OVERSEA = isOversea,
                ID = id
            };
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            RouteList obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoRouteList();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new RouteListDAL().UpdateRouteList(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit RouteList]", 0, "[UpdateRouteList]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoRouteList();
                var kq = new RouteListDAL().InsertRouteList(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit RouteList]", 0, "[InsertRouteList]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/ROUTE_LIST/ListRoute.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}