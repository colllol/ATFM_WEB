using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TuesPechkin;
using System.IO;

namespace prjApplication.Report.SCHEDULED_SUM
{
    public partial class SSUMMARY : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "REPORT_SCHEDULED_SUM_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_SCHEDULED_SUM_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}