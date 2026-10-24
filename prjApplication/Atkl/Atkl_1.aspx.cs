using prjInfo;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using System.Data;
using System.Text.RegularExpressions;

namespace prjApplication.Atkl
{
    public partial class Atkl_1 : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "Atkl";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var regex = new Regex(@"^[a-zA-Z0-9]+$");

                //txtStartDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                //txtFinishDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                if (!regex.IsMatch(txtStartDate.Value)) return;
                if (!regex.IsMatch(txtFinishDate.Value)) return;
                //txtStartDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                //txtFinishDate.Value = DateTime.Now.ToString("dd/MM/yyyy");
                LoadDataGrid();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PhanTrang1.PageIndex = 0;
            LoadDataGrid();
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
                    kq = LoadDataGrid();
                    break;
            }
            return kq;
        }
        private clsSearchAtkl GetSearch()
        {
            clsSearchAtkl obj = new clsSearchAtkl();
            if (!string.IsNullOrEmpty(txtStartDate.Value))
                obj.FROM_DATE = UltilFunc.ToDate(txtStartDate.Value, "dd/MM/yyyy");
            if (!string.IsNullOrEmpty(txtFinishDate.Value))
                obj.TO_DATE = UltilFunc.ToDate(txtFinishDate.Value, "dd/MM/yyyy");
            obj.PAGE_INDEX = PhanTrang1.PageIndex;
            obj.PAGE_SIZE = PhanTrang1.PageSize;

            return obj;
        }
        private string LoadDataGrid()
        {
            System.Data.DataTable t = new prjBusinessLogic.AtklDAL().GetPage(GetSearch());
            if (t != null)
            {
                PhanTrang1.TotalRecord = Convert.ToInt32(t.Rows[0]["Record_Sum"].ToString());
                grdSource.DataSource = t;
                grdSource.DataBind();
                return this.RenderToHTML(grdSource);
            }
            else
                return "";

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        protected void btnimport_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Value != "")
            {
                object ax;
                ax = new clsResuftAPI().GetPostValueApiExtension("ATKL_PKG", "import_atk", new { P_DATE = txtStartDate.Value });
            }
            else
                this.AlertMessage("Chọn START DATE");
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = new AtklDAL().GetExpFPL(txtStartDate.Value, txtFinishDate.Value);
            GridView gr = new GridView();
            gr.DataSource = dt;
            gr.DataBind();
            for (int i = 0; i < gr.Rows.Count; i++)
            {
                GridViewRow row = gr.Rows[i];
                row.Attributes.Add("class", "textmode");
            }
            this.CreateExcel(this.RenderToHTML(gr), "ATS.xls");
        }
        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadDataGrid();
        }
    }
}