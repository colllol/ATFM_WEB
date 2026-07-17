using System;

namespace prjApplication.Common
{
    public partial class ChartReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Dashboard hiện đọc dữ liệu giả lập từ Data/dashboard-flights.json.
            // Khi có API thật, chỉ cần thay nguồn dataUrl trong ChartReport.aspx.
        }
    }
}
