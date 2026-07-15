using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using prjBusinessLogic;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
namespace prjApplication.Day_Flights
{
    public partial class DayFlightNotRoute : PageBaseCallBack
    {        
        protected string _phanCachArg = "_____";
        protected string _phanCach = "::::";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.ExcuteJavascript(grdSource_RenderBody(new DayFlyght_NotPermDAL().GetAll()));
            }
        }
        #region call back
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { _phanCachArg }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "btnSearch_Onclick":
                    kq = btnSearch_Onclick(_arg[0]);
                    break;
                case "LoadGrdSourceScroll":
                    kq = LoadGrdSourceScroll(_arg[0]);
                    break;
            }
            return kq;
        }
        #endregion

        #region function
        protected string grdSource_LoadFirst()
        {
            return grdSource_RenderBody(new DayFlightsDAL().GetTableFlightNotRoute(new prjInfo.clsDaylyFlightSearch() {RowStart=1, RowFinish=20 }));
        }
        private string grdSource_RenderBody(DataTable dt)
        {
            string kq = "";
            if (dt == null) return kq;
            string c0, c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12, c13, c14, c15, c16, c17;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                kq += $"<tr data-rowNumber=\"{dt.Rows[i]["RNUM"]}\">";
                c0 = $"<td>{dt.Rows[i]["RNUM"]}</td>";
                c1 = $"<td>{dt.Rows[i]["NBR"]}</td>";
                c2 = $"<td>{dt.Rows[i]["LETTER_TYPE"]}</td>";
                c3 = $"<td>{dt.Rows[i]["FLIGHTNBR"]}</td>";
                c4 = $"<td>{dt.Rows[i]["REGISTRATION"]}</td>";
                c5 = $"<td>{dt.Rows[i]["FROM_AIRP"]}</td>";
                c6 = $"<td>{dt.Rows[i]["TO_AIRP"]}</td>";
                c7 = $"<td>{dt.Rows[i]["ETD"]}</td>";
                c8 = $"<td>{dt.Rows[i]["ETA"]}</td>";
                c9 = $"<td>{dt.Rows[i]["ATD"]}</td>";
                c10 = $"<td>{dt.Rows[i]["ATA"]}</td>";
                c11 = $"<td>{dt.Rows[i]["VIA"]}</td>";
                c12 = $"<td>{dt.Rows[i]["CNL_TYPE"]}</td>";
                c13 = $"<td>{dt.Rows[i]["CRAFT_TYPE"]}</td>";
                c14 = $"<td>{dt.Rows[i]["FLIGHTDATE"]}</td>";
                c15 = $"<td>{dt.Rows[i]["TEXT"]}</td>";
                c16 = $"<td>{dt.Rows[i]["ROUTE"]}</td>";
                c17 = $"<td>{dt.Rows[i]["ROUTE_TT"]}</td>";
                kq += c0 + c1 + c2 + c3 + c4 + c5 + c6 + c7 + c8 + c9 + c10 + c11 + c16 + c17 + c12 + c13 + c14 + c15;
                kq += "</tr>";
            }
            //kq = $"$('#grdSource tbody>tr').remove(); $('#grdSource tbody').append('{kq.Replace("\n", "<br>")}')";
            return kq.Replace("\n", "<br>");
        }
        private string btnSearch_Onclick(string thamso)
        {
            dynamic _obj = JsonConvert.DeserializeObject(thamso);
            return grdSource_RenderBody(new DayFlyght_NotPermDAL().GetBySearch((object)_obj));
        }
        private string LoadGrdSourceScroll(string thamso)
        {
            return btnSearch_Onclick(thamso);
        }
        #endregion
    }
}