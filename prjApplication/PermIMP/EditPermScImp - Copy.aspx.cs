using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjBusinessLogic.DAL;
using prjInfo;

namespace prjApplication.PermIMP
{
    public partial class EditPermScImp : System.Web.UI.Page
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
                        ddl_Load();
                        LoadPermScImpById();
                    }
                }
            }
        }

        private void LoadPermScImpById()
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
        public void ddl_Load()
        {
            this.FillDropdownList(ddlCRAFT, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList(ddlAUTHOR, new FpAuthorDAL().GetAllObject(), "AUTHOR_NAME", "AUTHOR_CODE");

        }
        private void PopulateItem(int _id)
        {
            PermScIMP obj = new PermScImpDAL().GetOnePermScIMP(_id);
            txtCALLSIGN.Value = obj.CALLSIGN;
            if (obj.FROMDATE != null && obj.FROMDATE != "")
                this.txtBEGINDATE.Value = obj.FROMDATE;
            if (obj.TODATE != null && obj.TODATE != "")
                this.txtENDDATE.Value = obj.TODATE;

            txtDAILY.Value = obj.DAILY;
            txtFROM_AIRP.Value = obj.FROM_AIRP;
            txtTO_AIRP.Value = obj.TO_AIRP;
            txtETD.Value = obj.ETD;
            txtETA.Value = obj.ETA;
            txtVIA.Value = obj.VIA;
            txtREMARK.Value = obj.REMARK;
            ddlPERMTYPE.SelectedIndex = UltilFunc.GetIndexControl(ddlPERMTYPE, obj.PERMTYPE);
            ddlCRAFT.SelectedItem.Text = obj.CRAFT;

            ddlAUTHOR.SelectedIndex = UltilFunc.GetIndexControl(ddlAUTHOR, obj.AUTHOR);
            if (obj.PERMDATE != null && obj.PERMDATE != "")
                this.txtPERMDATE.Value = obj.PERMDATE;
            txtOPER.Value = obj.OPER;
            ddlSeason.SelectedIndex = UltilFunc.GetIndexControl(ddlSeason, obj.SEASON);
            txtPERMNBR.Value = obj.PERMNBR;
            txtVIA.Value = obj.VIA;
            txtREFERENCE.Value = obj.REFERENCE;
            txtBILLINGADDRESS.Value = obj.BILLINGADDRESS;
            txtPERMCONTENT.Value = obj.PERMCONTENT;
            txtVERSION.Value = obj.VERSION;
            txtPURPOSE.Value = obj.PURPOSE;
            txtREGISTRATION.Value = obj.REGISTRATION;

        }

        private PermScIMP GetInfoPermScIMP()
        {

            PermScIMP _obj = new PermScIMP();
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
                     
            _obj.ID = id;
            _obj.CALLSIGN = txtCALLSIGN.Value;
            if (this.txtBEGINDATE.Value.ToString() != "")
                _obj.FROMDATE = this.txtBEGINDATE.Value.ToString();
            if (this.txtENDDATE.Value.ToString() != "")
                _obj.TODATE = this.txtENDDATE.Value.ToString();
            _obj.TO_AIRP = txtTO_AIRP.Value;
            _obj.VIA = txtVIA.Value;
            _obj.PERMTYPE = ddlPERMTYPE.SelectedValue.ToString();
            _obj.CRAFT = ddlCRAFT.SelectedItem.Text.ToString();
            _obj.REMARK = txtREMARK.Value;
            _obj.DAILY = txtDAILY.Value;
            _obj.ETA = txtETA.Value;
            _obj.ETD = txtETD.Value;
            _obj.FROM_AIRP = txtFROM_AIRP.Value;


            _obj.AUTHOR = ddlAUTHOR.SelectedValue.ToString();
            if (this.txtPERMDATE.Value.ToString() != "")
                _obj.PERMDATE = this.txtPERMDATE.Value.ToString();

            _obj.OPER = txtOPER.Value;
            _obj.SEASON = ddlSeason.SelectedValue.ToString();
            _obj.PERMNBR = txtPERMNBR.Value;
            _obj.VIA = txtVIA.Value;
            _obj.REFERENCE = txtREFERENCE.Value;
            _obj.BILLINGADDRESS = txtBILLINGADDRESS.Value;
            _obj.PERMCONTENT = txtPERMCONTENT.Value;
            _obj.VERSION = txtVERSION.Value;
            _obj.PURPOSE = txtPURPOSE.Value;
            _obj.REGISTRATION = txtREGISTRATION.Value;           
            
            return _obj;


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            PermScIMP obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoPermScIMP();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new PermScImpDAL().UpdatePermSCIMPAsync(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Perm SC Imp]", 0, "[Update Perm SC IMP]" + lblResuft.InnerText, 0.0);
            }
            

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PermIMP/ListPermScIMP.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}