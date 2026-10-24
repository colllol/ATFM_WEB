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

namespace prjApplication.Static.AddressAftn
{
    public partial class EditAddressAftn : PageCoreAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                    ddlAftnSource();
                    LoadObjectById();
                btnSave.Enabled = _Role.R_Add;
            }
        }


        #region control - event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            prjInfo.AddressAftn obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoObject();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new AddressAftnDAL().UpdateObject(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit AddressAftn]", 0, "[UpdateAddressAftn]" + lblResuft.InnerText, 0.0);

            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new AddressAftnDAL().InsertObject(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit AddressAftn]", 0, "[InsertAddressAftn]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/AddressAftn/ListAddressAftn.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion
        //#region bind data to control
        //private void ddlGroupAftnSource()
        //{
        //    List<Aftn> lis = new AftnDAL().GetAllObject();
        //    this.FillDropdownList<Aftn>(ddlGroupAftn, lis, "CTR_ENAME", "CTR_CODE");
        //}
        //#endregion
        private void ddlAftnSource()
        {
            List<Aftn> lis = new AftnDAL().GetAllObject();
            this.FillDropdownList<Aftn>(ddlGroupAddressAftn, lis, "GROUP_NAME", "ID");
        }
        private prjInfo.AddressAftn GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            return new prjInfo.AddressAftn()
            {
                G_A_M_ID = Convert.ToInt32(ddlGroupAddressAftn.SelectedValue),
                DESCRIPTION = txtDESCRIPTION.Text,
                ADDRESS = txtADDRESS.Text,
                
                ID = id
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
            prjInfo.AddressAftn obj = new AddressAftnDAL().GetOneGad(_id.ToString());
            this.SetDisplayDropdownList(ddlGroupAddressAftn, obj.G_A_M_ID.ToString());
            txtDESCRIPTION.Text = obj.DESCRIPTION;
            txtADDRESS.Text = obj.ADDRESS;
            
        }


        private bool CheckValidate()
        {
            return true;
        }
    }
}