using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;


namespace prjApplication.Static.Country
{
    public partial class EditCountry : PageCoreAdmin
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
                        LoadCountryById();
                        this.DataBind();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
            }
        }


        #region control - event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (!CheckValidate()) return;
            Ctry country;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                country = GetInfoCtry();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                country.ID = _id;
                var kq = new CtryDAL().UpdateCountryAsync(country);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Error update!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Coutry]", 0, "[UpdateCountryAsync]" + lblResuft.InnerText, 0.0);

            }
            else
            {
                country = GetInfoCtry();                
                var kq = new CtryDAL().InsertCountry(country);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Error insert!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Insert Coutry]", 0, "[InsertCountry]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Country/ListCountry.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region funciton        
        private Ctry GetInfoCtry()
        {
            return new Ctry() { CTR_CODE = txtCTR_CODE.Text, CTR_ENAME = txtCTR_ENAME.Text };
        }
        private void LoadCountryById()
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
            Ctry obj = new CtryDAL().GetCountryByIdAsync(_id.ToString());
            txtCTR_CODE.Text = obj.CTR_CODE;
            txtCTR_ENAME.Text = obj.CTR_ENAME;
        }
        
        #endregion

    }
}