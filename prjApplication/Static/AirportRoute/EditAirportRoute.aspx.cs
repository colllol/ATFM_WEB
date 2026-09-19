using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;

namespace prjApplication.Static.AirportRoute
{
    public partial class EditAirportRoute : PageCoreAdmin
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    if (!IsPostBack)
                    {
                        ddl();
                        LoadAirportRouteById();
                        this.DataBind();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
            }
        }
        private void ddl()
        {
            List<Aero> lis = new AeroDAL().GetListAll();
            this.FillDropdownList<Aero>(ddlFROM_AIRP, lis, "AE_NAME", "AE_CODE");
            this.FillDropdownList<Aero>(ddlTO_AIRP, lis, "AE_NAME", "AE_CODE");
        }

        #region control - event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            prjInfo.AirportRoute obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoAirportRoute();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new AirportRouteDAL().UpdateAirportRoute(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit AirportRoute]", 0, "[UpdateAirportRoute]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoAirportRoute();
                var kq = new AirportRouteDAL().InsertAirportRoute(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Airlines]", 0, "[InsertAirportRoute]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/AirportRoute/ListAirportRoute.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region bind data to control

        #endregion

        #region funciton

        private prjInfo.AirportRoute GetInfoAirportRoute()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            string isOversea = "0";
            string isDomestic = "0";
            if (ckIS_OVERSEA.Checked)
                isOversea = "1";
            if (ckIS_DOMESTIC.Checked)
                isDomestic = "1";
            return new prjInfo.AirportRoute()
            {
                ROUTE = txtROUTE.Text,
                FROM_AIRP = ddlFROM_AIRP.Text,
                TO_AIRP = ddlTO_AIRP.Text,
                IS_OVERSEA = isOversea,
                DOMESTIC = Convert.ToInt32(txtDOMESTIC.Text),
                INTERNATIONAL = Convert.ToInt32(txtINTERNATIONAL.Text),
                IS_DOMESTIC = isDomestic,
                SUMMARY = Convert.ToInt32(txtDOMESTIC.Text) + Convert.ToInt32(txtINTERNATIONAL.Text),
                ID = id
            };
        }
        private void LoadAirportRouteById()
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
            prjInfo.AirportRoute obj = new AirportRouteDAL().GetOneAirportRoute(_id);

            txtROUTE.Text = obj.ROUTE;
            this.SetDisplayDropdownList(ddlFROM_AIRP, obj.FROM_AIRP);
            this.SetDisplayDropdownList(ddlTO_AIRP, obj.TO_AIRP);
            txtDOMESTIC.Text = obj.DOMESTIC.ToString();
            txtINTERNATIONAL.Text = obj.INTERNATIONAL.ToString();

            if (!String.IsNullOrEmpty(obj.IS_OVERSEA.ToString()))
            {
                if (obj.IS_OVERSEA.ToString() == "1")
                    ckIS_OVERSEA.Checked = true;
            }
            if (!String.IsNullOrEmpty(obj.IS_DOMESTIC.ToString()))
            {
                if (obj.IS_DOMESTIC.ToString() == "1")
                    ckIS_DOMESTIC.Checked = true;
            }

        }
        private bool CheckValidate()
        {
            return true;
        }
        #endregion
    }
}