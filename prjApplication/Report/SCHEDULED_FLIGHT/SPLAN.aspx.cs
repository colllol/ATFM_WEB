using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;

namespace prjApplication.Report.SCHEDULED_FLIGHT
{
    public partial class SPLAN : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
        #region export

        public string LoadData(Int64 CalID)
        {
            System.Text.StringBuilder buidler = new System.Text.StringBuilder();
            DataTable _dt;
            _dt = new clsResuftAPI().GetPostTableApiExtension("REPORT_PKG", "SPLAN", new { P_CALENDARID = 1 });
            buidler.Append("<tr>");
            foreach (DataRow dtRow in _dt.Rows)
            {
               
                buidler.Append("<td class=\"tg-031e\">"+ dtRow["PERMNBR_ID"] + "</td>");
                buidler.Append("<td class=\"tg-031e\">" + dtRow["CRAFT_ID"] + "</td>");
                buidler.Append("<td class=\"tg-031e\">" + dtRow["FLIGHTNBR"] + "</td>");
                buidler.Append("<td class=\"tg-031e\">" + dtRow["FROMTO"] + "</td>");
                buidler.Append("<td class=\"tg-031e\">" + dtRow["ETD"] + "</td>");
                buidler.Append("<td class=\"tg-yw4l\">" + dtRow["DAY1"] + "</td>");
                buidler.Append("<td class=\"tg-yw4l\">" + dtRow["DAY2"] + "</td>");
                buidler.Append("<td class=\"tg-yw4l\">" + dtRow["DAY3"] + "</td>");
                buidler.Append("<td class=\"tg-yw4l\">" + dtRow["DAY4"] + "</td>");
                buidler.Append("<td class=\"tg-yw4l\">" + dtRow["DAY5"] + "</td>");
                buidler.Append("<td class=\"tg-yw4l\">" + dtRow["DAY6"] + "</td>");
                buidler.Append("<td class=\"tg-yw4l\">" + dtRow["DAY7"] + "</td>");
                buidler.Append("<td class=\"tg-yw4l\"></td>");
                buidler.Append("<td class=\"tg-yw4l\"></td>");
                buidler.Append("<td class=\"tg-yw4l\"></td>");                               

            }
            buidler.Append("</tr>");
            return buidler.ToString();
        }
        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "REPORT_SCHEDULED_PLAN_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_SCHEDULED_PLAN_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}