using prjInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
namespace prjApplication.Cant
{
    public partial class CantRealize : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "CantRealize";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();

                    Session.SetValueForControlSearch(this, _AliasSession);
                }
                catch { Session.SetValueSearch(_AliasSession, " 1=1"); }                
                LoadDataGrid(new string[] { txtStartDate.Value, txtFinishDate.Value});
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CustomPaging1.PageIndex = 0;
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
           
            List<prjInfo.CantRealize> t = new prjBusinessLogic.CantRealizeDAL().GetPage(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch, thamso[0].ToString(), thamso[1].ToString());
           
            CustomPaging1.TotalsRecord = t.GetTotalRecord<prjInfo.CantRealize>();
            grdSource.DataSource = t;
            grdSource.DataBind();            
            return this.RenderToHTML(grdSource);
        }
        private string whereDateHelper(string value)
        {
            return $"TO_DATE('{value}', 'DD-MM-YYYY')";
        }
        private string GetContent(string thamso)
        {
            return new CantRealizeDAL().GetOne(thamso).Rows[0]["CONTENT"].ToString();
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            
        }
    }
}