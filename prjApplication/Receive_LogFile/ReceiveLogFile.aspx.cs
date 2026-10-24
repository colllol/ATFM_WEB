using prjInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;

namespace prjApplication.Receive_LogFile
{
    public partial class ReceiveLogFile : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "ReceiveLogFile";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                LoadDataGrid(new string[] { txtStartDate.Value, txtFinishDate.Value });
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            phantrang1.PageIndex = 0;
            LoadDataGrid(new string[] { txtStartDate.Value, txtFinishDate.Value });
        }
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "LoadDataGrid":
                    kq = LoadDataGrid(ThamSo);
                    break;
                case "GetContent":
                    kq = GetContent(_arg[0]);
                    break;
            }
            return kq;
        }
        private string LoadDataGrid(string[] thamso)
        {

            System.Data.DataTable t = new prjBusinessLogic.ReceiveLogFileDAL().GetPage(GetSearch());
            if (t != null)
                phantrang1.TotalRecord= Convert.ToInt32(t.Rows[0]["Record_Sum"].ToString());
            grdSource.DataSource = t;
            grdSource.DataBind();
            return this.RenderToHTML(grdSource);
        }
        private clsSearchReceiveLogFile GetSearch()
        {
            clsSearchReceiveLogFile obj = new clsSearchReceiveLogFile();
            if (!string.IsNullOrEmpty(txtStartDate.Value))
                obj.FROM_DATE = UltilFunc.ToDate(txtStartDate.Value, "dd/MM/yyyy");
            if (!string.IsNullOrEmpty(txtFinishDate.Value))
                obj.TO_DATE = UltilFunc.ToDate(txtFinishDate.Value, "dd/MM/yyyy");
            if (!string.IsNullOrEmpty(txtFROM_PL.Value))
                obj.FROM_PL = txtFROM_PL.Value.ToString().Trim();
            if (!string.IsNullOrEmpty(txtORIGIN.Value))
                obj.ORIGIN = txtORIGIN.Value.ToString().Trim();
            if (!string.IsNullOrEmpty(txtNBR.Value))
                obj.NBR = txtNBR.Value.ToString().Trim();
            obj.PAGE_INDEX = phantrang1.PageIndex;
            obj.PAGE_SIZE = phantrang1.PageSize;
            obj.MessageType = ddlTypeMessage.SelectedValue;
            return obj;
        }
        private string whereDateHelper(string value)
        {
            return $"TO_DATE('{value}', 'DD-MM-YYYY')";
        }
        private string GetContent(string thamso)
        {
            return new ReceiveLogFileDAL().GetOne(thamso).Rows[0]["CONTENT"].ToString();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GridView ax = GridView1;
            System.Data.DataTable t = new prjBusinessLogic.ReceiveLogFileDAL().GetPage(GetSearch());
            ax.DataSource = t;
            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "ReceiveLogFile_" + DateTime.Now.ToFileTime() + ".xls");
        }

        protected void phantrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadDataGrid(new string[] { txtStartDate.Value, txtFinishDate.Value });
        }
    }
}