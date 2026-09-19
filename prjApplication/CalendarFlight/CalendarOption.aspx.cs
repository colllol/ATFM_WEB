using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjApplication.CalendarFlight
{
    public partial class CalendarOption : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var ax = HttpUtility.UrlEncode("01/01/2000");
            var ax1 = HttpUtility.UrlEncode("01/01/2020");
        }

        protected void btnSelectDay_Click(object sender, EventArgs e)
        {
            var ax = txtday.Value;
            Response.Redirect($"~/CalendarFlight/CalendarDayFlight.aspx?Menu_ID={Request["Menu_ID"].ToString()}&time={HttpUtility.UrlEncode(txtday.Value)}");
        }

        protected void btnSelectTime_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/CalendarFlight/CalendarDayFlight.aspx?Menu_ID={Request["Menu_ID"].ToString()}&time1={HttpUtility.UrlEncode(txtDayStart.Value)}&time2={HttpUtility.UrlEncode(txtDayFinish.Value)}");
        }
    }
}