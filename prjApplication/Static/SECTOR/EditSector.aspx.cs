using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjApplication;

namespace prjApplication.Static.SECTOR
{
    public partial class EditSector : PageCoreAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    LoadObjectById();
                    btnSave.Enabled = _Role.R_Add;
                }
            
            }
        }


        #region control - event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            prjInfo.Sectors obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoObject();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.SECTOR_ID = _id;
                var kq = new SectorDAL().UpdateSectors(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Sector]", 0, "[UpdateSectors]" + lblResuft.InnerText, 0.0);

            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new SectorDAL().InsertSectors(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Sector]", 0, "[InsertSectors]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Sector/ListSectors.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion
               
        private prjInfo.Sectors GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            return new prjInfo.Sectors()
            {
                NOTE = txtDESCRIPTION.Text,
                SECTOR_NAME = txtADDRESS.Text,

                SECTOR_ID = id
            };
        }
        private void LoadObjectById()
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
            prjInfo.Sectors obj = new SectorDAL().GetSectorsById(_id.ToString());            
            txtDESCRIPTION.Text = obj.NOTE;
            txtADDRESS.Text = obj.SECTOR_NAME;

        }


        private bool CheckValidate()
        {
            return true;
        }
    }
}