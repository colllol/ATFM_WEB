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
    public partial class EditPermNoImp : System.Web.UI.Page
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

        }
        private void PopulateItem(int _id)
        {
            PermNoIMP obj = new PermNoImpDAL().GetOnePermNoIMP(_id);
            txtPERMDATE.Value = obj.PERMDATE;
            txtCALLSIGN.Value = obj.CALLSIGN;            
            txtFROM_AIRP.Value = obj.FROM_AIRP;
            txtTO_AIRP.Value = obj.TO_AIRP;
            txtETD.Value = obj.ETD;
            txtETA.Value = obj.ETA;
            txtVIA.Value = obj.VIA;
            txtREMARK.Value = obj.REMARK;
            ddlPERMTYPE.SelectedIndex = UltilFunc.GetIndexControl(ddlPERMTYPE, obj.PERMTYPE.ToString());
            ddlCRAFT.SelectedItem.Text = obj.CRAFT;
        }

        private PermNoIMP GetInfoPermNoIMP()
        {

            PermNoIMP _obj = new PermNoIMP();
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;

            _obj.ID = id;
            _obj.CALLSIGN = txtCALLSIGN.Value;
            if (this.txtPERMDATE.Value.ToString() != "")
                _obj.PERMDATE = this.txtPERMDATE.Value.ToString();
            
            _obj.TO_AIRP = txtTO_AIRP.Value.ToString();
            _obj.VIA = txtVIA.Value.ToString();
            _obj.PERMTYPE = ddlPERMTYPE.SelectedValue.ToString();
            _obj.CRAFT = ddlCRAFT.SelectedItem.Text.ToString();
            _obj.REMARK = txtREMARK.Value.ToString();           
            _obj.ETA = txtETA.Value.ToString();
            _obj.ETD = txtETD.Value.ToString();
            _obj.FROM_AIRP = txtFROM_AIRP.Value.ToString();
            return _obj;


        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            PermNoIMP obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoPermNoIMP();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new PermNoImpDAL().UpdatePermNoIMPAsync(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Perm No Imp]", 0, "[Update Perm No IMP]" + lblResuft.InnerText, 0.0);
            }


        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PermIMP/ListPermNoIMP.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
    }
}