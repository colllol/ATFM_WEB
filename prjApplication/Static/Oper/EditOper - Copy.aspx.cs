using prjComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;


namespace prjApplication.Static.Oper
{
    public partial class EditOper : PageCoreAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                    LoadObjectById();
                btnSave.Enabled = _Role.R_Add;
            }
        }


        #region control - event
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckValidate()) return;
            prjInfo.Oper obj;
            if (CommonLib.IsNumeric(Request["ID"]) == true)
            {
                // update
                obj = GetInfoObject();
                int _id = Convert.ToInt32(Request["ID"].ToString());
                obj.ID = _id;
                var kq = new OperDAL().UpdateObject(obj);
                if (kq) lblResuft.InnerText = "Update sussess!";
                else lblResuft.InnerText = "Update error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Oper]", 0, "[UpdateOper]" + lblResuft.InnerText, 0.0);
            }
            else
            {
                // insert
                obj = GetInfoObject();
                var kq = new OperDAL().InsertObject(obj);
                if (kq) lblResuft.InnerText = "Insert sussess!";
                else lblResuft.InnerText = "Insert error!";

                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Edit Oper]", 0, "[InsertOper]" + lblResuft.InnerText, 0.0);
            }

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Static/Oper/ListOper.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString());
        }
        #endregion

        #region bind data to control

        #endregion

        #region funciton

        private prjInfo.Oper GetInfoObject()
        {
            int id = CommonLib.IsNumeric(Request["ID"]) ? Convert.ToInt32(Request["ID"].ToString()) : 0;
            string isDomestic = "0";
            if (ckIS_DOMESTIC.Checked)
                isDomestic = "1";
            return new prjInfo.Oper()
            {
                OPER_NAME = txtOPER_NAME.Text,
                OPER_ADDRESS = txtOPER_ADDRESS.Text,
                IS_DOMESTIC = isDomestic,
                OPER_ICAO = txtOPER_ICAO.Text,
                OPER_IATA = txtOPER_IATA.Text,
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
            prjInfo.Oper obj = new OperDAL().GetOneObject(_id.ToString());

            txtOPER_NAME.Text = obj.OPER_NAME;
            txtOPER_ADDRESS.Text = obj.OPER_ADDRESS;
            txtOPER_ICAO.Text = obj.OPER_ICAO;
            txtOPER_IATA.Text = obj.OPER_IATA;

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