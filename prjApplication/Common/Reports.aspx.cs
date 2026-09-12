using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using prjComponents;
using prjInfo;
using prjBusinessLogic;
using prjBusinessLogic.DAL;
using HPCServerDataAccess;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;

namespace prjApplication.Common
{
    public partial class Reports : System.Web.UI.Page
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindBarChart();
            }

        }
        public void BindBarChart()
        {           
            
            DataTable dt = new DayFlightTotalInfoDAL().GetAll();
            string category = "";

            decimal[] values = new decimal[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)

            {

                //category = category + "," + dt.Rows[i]["Ten_Viet_Tat"].ToString();

                //values[i] = Convert.ToDecimal(dt.Rows[i]["NUMBER"]);
                

            }

            //BarChart1.CategoriesAxis = category.Remove(0, 1);

            //BarChart1.Series.Add(new AjaxControlToolkit.BarChartSeries { Data = values, BarColor = "#2fd1f9", Name = "LETTER" });
        }

    }
}
