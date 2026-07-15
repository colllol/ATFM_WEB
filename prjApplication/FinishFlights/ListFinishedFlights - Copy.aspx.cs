using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using System.IO;
using TuesPechkin;
using System.Data;

namespace prjApplication.FinishFlights
{
    public partial class ListFinishedFlights : PageCoreAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                               
            }

        }

        protected void btnExportFin_Click(object sender, EventArgs e)
        {
            Make_Finished();
        }
        public bool Make_Finished()
        {
            var ax = new clsResuftAPI().GetPostValueApiExtension("MAKE_FINISHED", "make_finished_flights_news", new { p_string ="" }).ToString();
            return ax == "1" ? false : true;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string html = GetContent();
            this.CreateExcel(html, "THSLB_BCSL_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        
        public string GetContent()
        {
            string _html = null;
            StringWriter sw = new StringWriter();
            HtmlTextWriter h = new HtmlTextWriter(sw);
            exportid.RenderControl(h);
            _html = sw.GetStringBuilder().ToString();
            return _html;
        }
       
    }


}