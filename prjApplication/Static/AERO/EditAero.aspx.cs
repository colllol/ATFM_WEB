using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;


namespace prjApplication.Static.AERO
{
    public partial class EditAirport : PageCoreAdmin
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
                        //ddlQuocGiaSource();
                        LoadCountryById();
                        btnSave.Enabled = _Role.R_Add;
                    }
                }
                //else
                //{
                //    if (!IsPostBack)
                //        ddlQuocGiaSource();
                //}
            }
        }


        #region control - event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            Aero obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoObject();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new AeroDAL().UpdateAero(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Aero]", 0, "[UpdateAero]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new AeroDAL().InsertAero(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Aero]", 0, "[InsertAero]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Aero/ListAero.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region bind data to control
        //private void ddlQuocGiaSource()
        //{            
        //    List<Ctry> lis = new CtryDAL().GetAllCountry();
        //    this.FillDropdownList<Ctry>(ddlQuocGia, lis, "CTR_ENAME", "CTR_CODE");            
        //}
        #endregion

        #region funciton

        private Aero GetInfoObject()
        {            
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            int aeID = CommonLib.IsNumeric(Request["AE_ID"]) ? Convert.ToInt32(Request["AE_ID"].ToString()) : 0;

            string aeINTER = "N";
            string adISOVERSEA = "N";
            if (ckINTER.Checked)
                aeINTER = "Y";
            if (ckISOVERSEA.Checked)
                adISOVERSEA = "Y";

            return new Aero() {
                //AE_ID = Convert.ToInt32(txtAE_ID.Text),
                AE_ID = aeID,
                //CTR_CODE = ddlQuocGia.SelectedValue,
                //FR_ID = Convert.ToInt32(txtFR_ID.Text),
                AE_CODE = txtAE_CODE.Text,
                AE_NAME = txtAE_NAME.Text,
                AE_ZONE = txtAE_ZONE.Text,               
                AE_INTER = aeINTER,
                AE_IATA = txtAE_IATA.Text,               
                //AE_LAT = txtAE_LAT.Text,
                //AE_LONG = txtAE_LONG.Text,
                AD_ISOVERSEA = adISOVERSEA,               
                SUMMER_TIME = txtSUMMER_TIME.Text,
                WINTER_TIME = txtWINTER_TIME.Text,
               // TM = txtTM.Text,
                TN = txtGioihan.Text,
                MIEN=ddlVung.SelectedValue.ToString(),
                ID = id
            };
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
            Aero obj = new AeroDAL().GetAeroById(_id.ToString());
            //txtAE_ID.Text = obj.AE_ID.ToString();
            //this.SetDisplayDropdownList(ddlQuocGia, obj.CTR_CODE);
           // txtFR_ID.Text = obj.FR_ID.ToString();
            txtAE_CODE.Text = obj.AE_CODE;
            txtAE_NAME.Text = obj.AE_NAME;
            txtAE_ZONE.Text = obj.AE_ZONE;
            //txtAE_INTER.Text = obj.AE_INTER;
            txtAE_IATA.Text = obj.AE_IATA;
            //txtAE_LAT.Text = obj.AE_LAT;
            //txtAE_LONG.Text = obj.AE_LONG;
            //txtAD_ISOVERSEA.Text = obj.AD_ISOVERSEA;                                 
            txtSUMMER_TIME.Text = obj.SUMMER_TIME;
            txtWINTER_TIME.Text = obj.WINTER_TIME;
            //txtTM.Text = obj.TM;
            txtGioihan.Text = obj.TN;
            this.SetDisplayDropdownList(ddlVung, obj.MIEN);

            if (!String.IsNullOrEmpty(obj.AE_INTER.ToString()))
            {
                if (obj.AE_INTER.ToString() == "Y")
                    ckINTER.Checked = true;
            }

            if (!String.IsNullOrEmpty(obj.AD_ISOVERSEA.ToString()))
            {
                if (obj.AD_ISOVERSEA.ToString() == "Y")
                    ckISOVERSEA.Checked = true;
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
        #endregion

    }
}