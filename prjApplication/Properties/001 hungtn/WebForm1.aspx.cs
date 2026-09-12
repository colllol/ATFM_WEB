using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;
using prjBusinessLogic;

namespace prjApplication._001_hungtn
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string _SanBay
        {
            get
            {
                string kq = "";
                foreach (var item in new AeroDAL().GetListAll())
                {
                    kq += $"<option value='{item.AE_CODE}'>{item.AE_CODE}</option>";
                }
                return kq;
            }
        }
        public string _ListSanBay
        {
            get
            {
                string kq = "";
                foreach (var item in new AeroDAL().GetListAll())
                {
                    kq += item.AE_CODE +",";
                }
                return kq.Substring(0, kq.Length-1);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }
        private void LoadGrid()
        {
            List<Aero> lis = new AeroDAL().GetListAll();
            grdSource.DataSource = lis;
            grdSource.DataBind();
        }
    }
}