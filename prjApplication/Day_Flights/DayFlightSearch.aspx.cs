using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjInfo;
using prjBusinessLogic;
using System.Data;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;

namespace prjApplication.Day_Flights
{
    public partial class DayFlightSearch : PageBaseCallBack
    {
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        public string _ObjSearch
        {
            get
            {
                clsDaylyFlightSearch obj = new clsDaylyFlightSearch();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

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
                case "RenderTopBarInfo":
                    kq = RenderTopBarInfo();
                    break;
                case "btnSearch_Onclick":
                    kq = btnSearch_Onclick(_arg[0]);
                    break;
                case "LoadGrdSourceScroll":
                    kq = LoadGrdSourceScroll(_arg[0]);
                    break;
                case "viewPopupInfoExtension":
                    kq = viewPopupInfoExtension(ThamSo);
                    break;
                case "btnAccessInfoChange_Onclick":
                    kq = btnAccessInfoChange_Onclick(_arg[0]);
                    break;
            }

            return kq;
        }
        #endregion
        #region properties
        protected DataTable dtInfo = new DataTable();
        #endregion
        #region function
        #region get color code
        private static string trColor(object val)
        {
            switch (val.ToString().ToLower())
            {
                case "1":
                    return ColorHelper.GetColorCode(clsColor.cssTdCallSign);
                case "2":
                    return ColorHelper.GetColorCode(clsColor.cssTdColChange);
                case "3":
                    return ColorHelper.GetColorCode(clsColor.cssTextColChange);
                default:
                    return ColorHelper.GetColorCode(clsColor.cssTrChange);
            }
        }
        private static string tdColor(object val)
        {
            switch (val.ToString())
            {
                case "1":
                    return ColorHelper.GetColorCode(clsColor.cssTrDienVanHaCanh);
                case "2":
                    return ColorHelper.GetColorCode(clsColor.cssTrDienVanCatCanhCustom);
                case "3":
                    return ColorHelper.GetColorCode(clsColor.cssTrDienVanCatCanh);
                default:
                    return ColorHelper.GetColorCode(clsColor.cssTrDaCoKeHoachBayCustom);
            }
        }
        private static string itemColor(object val)
        {
            switch (val.ToString())
            {
                case "1":
                    return ColorHelper.GetColorCode(clsColor.cssTrChange);
                case "2":
                    return ColorHelper.GetColorCode(clsColor.cssTrChuaThucHien);
                case "3":
                    return ColorHelper.GetColorCode(clsColor.cssTrChuaThucHienCustom);
                default:
                    return ColorHelper.GetColorCode(clsColor.cssTrDaCoKeHoachBay);
            }
        }
        #endregion
        protected string RenderTopBarInfo()
        {
            string kq = "";
            DataTable dt = new DayFlightTotalInfoDAL().GetAll();
            kq += $"<ul>";
            foreach (DataRow item in dt.Rows)
            {
                kq += $"<li>{item["VTEXT"]}<span class=\\'badge\\'>{item["VVALUE"]}</span></li>";
            }
            kq += "</ul>";
            kq = $"$('#infoTotal').html('{kq}');";
            dt = new DayFlightTotalInfoDAL().GetInfoTopBar3();
            if (dt == null) return kq;
            kq += $"$('#lbltopbarNotificon_TotalMessage').text('{dt.Rows[0]["TotalMessage"]}');";
            kq += $"$('#lbltopbarNotificon_TotalFlight').text('{dt.Rows[0]["TotalFlight"]}');";
            kq = $"<script>{kq}</script>";
            return kq;
        }
        protected string RenderGrdSource(DataTable dt)
        {
            /* hungtn edit 201801041025
             * chuyển heade table render ra ngoài aspx
             */
            string kq = "";
            kq += ContentRowGrdSource(dt);
            return kq;
        }
        private string cellHasSpan(string value)
        {
            if (value == "01/01/0001 12:00:00 SA")
                return string.Empty;
            if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
                return string.Empty;
            else return $"<span>{value}</span>";
        }
        private string cellHasSpan(object value)
        {
            if (value.ToString() == "01/01/0001 12:00:00 SA")
                return string.Empty;
            if (string.IsNullOrEmpty(value.ToString()) && string.IsNullOrWhiteSpace(value.ToString()))
                return string.Empty;
            if (value is DateTime)
            {
                return $"<span>{DateTime.Parse(value.ToString()).ToShortDateString()}</span>";
            }
            else return $"<span>{value.ToString()}</span>";
        }
        #region function data
        private string btnSearch_Onclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsDaylyFlightSearch>(thamso, dateTimeConverter);
            DataTable dt = new DayFlightsDAL().GetTableFlightBySearch(obj);

            return RenderGrdSource(dt);
        }
        private string LoadGrdSourceScroll(string thamso)
        {
            return btnSearch_Onclick(thamso);
        }
        private string ContentRowGrdSource(DataTable dt)
        {
            string c0 = "", c1 = "", c2 = "", c3 = "", c4 = "", c5 = "", c6 = "", c7 = "", c8 = "", c9 = "", c10 = "", c11 = "", c12 = "", c13 = "", c14 = "", c15 = "", c16 = "", c17 = "", c18 = "", kq = "";
            string c4B = "", c5B = "";
            if (dt == null) return "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                kq += $"<tr ondblclick=\"viewPopupInfoExtension(this)\""
                    + $" data-id=\"{dt.Rows[i]["FLIGHT_ID"]}\" data-timeM=\"{DateTime.Parse(dt.Rows[i]["LETTERNBR_PK"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{dt.Rows[i]["RNUM"]}' "
                    + $" data-CallSign=\"{dt.Rows[i]["FLIGHTNBR"]}\">";
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
                c11 = $"<td data-cusTip='true'>{dt.Rows[i]["VIA"]}</td>";
                c12 = $"<td>{dt.Rows[i]["CNL_TYPE"]}</td>";
                c13 = $"<td>{dt.Rows[i]["CRAFT_TYPE"]}</td>";
                c14 = $"<td data-cusTip='true'>{dt.Rows[i]["FLIGHTDATE"]}</td>";
                c15 = $"<td data-cusTip='true'>{dt.Rows[i]["TEXT"]}</td>";
                c16 = $"<td data-cusTip='true'>{dt.Rows[i]["ROUTE"]}</td>";
                c17 = $"<td data-cusTip='true'>{dt.Rows[i]["ROUTE_TT"]}</td>";
                kq += c0 + c1 + c2 + c3 + c4 + c5 + c6 + c7 + c8 + c9 + c10 + c11 + c16 + c17 + c12 + c13 + c14 + c15;
                kq += "</tr>";
            }
            return kq;
        }
        protected string grdSourceLoadFirt()
        {
            return RenderGrdSource(new DayFlightsDAL().GetTableFlightBySearch(new clsDaylyFlightSearch() { RowStart=1, RowFinish=20}));
        }
        private string viewPopupInfoExtension(string[] thamso)
        {
            string kq = "";
            DataTable dt = new DayFlightTotalInfoDAL().GetInfoChangeById(thamso[0]);
            kq += FlightInfoExtension_IsChange(dt.Rows[0]["CONTENTCHANGE"].ToString());
            kq += FlightInfoExtension_IsTimeValid(dt.Rows[0]["HASTIMEVALID"].ToString() == "1" ? "YES" : "NO");
            kq += FlightInfoExtension_ChangeInfo(dt.Rows[0]["CONTENTCHANGE"].ToString());
            kq += FlightInfoExtension_HasPerm(dt.Rows[0]["HASPERM"].ToString() == "1" ? "YES" : "NO");
            kq += FlightInfoExtension_DienVan(thamso[1], thamso[2]);
            // view button access checked info change
            kq += FlightInfoExtension_ShowButtonAccess(thamso[0]);

            return kq;
        }
        private string FlightInfoExtension_ShowButtonAccess(string sId)
        {
            var obj = new DayFlightsDAL().GetOneObject(sId);
            if (string.IsNullOrEmpty(obj.CONTENTCHANGE)) return "<script>$('#btnAcceseeInfoChange').hide()</script>";
            return "<script>$('#btnAcceseeInfoChange').show().attr('onclick','GetArgWithPostBack(\\'" + obj.FLIGHT_ID + "\\' + phanCachArg + \\'btnAccessInfoChange_Onclick\\', \\'btnAccessInfoChange_Onclick\\')')</script>";
        }
        private string FlightInfoExtension_IsChange(string sContent)
        {
            return string.IsNullOrEmpty(sContent) ? "<script>$('#txtChange').text('Not change!');</script>" : "<script> $('#txtChange').text('Has change!');</script>";
        }
        private string FlightInfoExtension_IsTimeValid(string sContent)
        {
            return string.IsNullOrEmpty(sContent) ? "<script>$('#txtValidateTime').text('Not valid time!');</script>" : "<script> $('#txtValidateTime').text('Has valid time!');</script>";
        }
        private string FlightInfoExtension_ChangeInfo(string sContent)
        {
            string[] l = sContent.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            kq += "<script>";
            kq += $"$('#txtContentChange').html('";
            foreach (var item in l)
            {
                kq += $"{item}</br>";
            }
            kq += "');";
            kq += "</script>";
            return kq;
        }
        private string FlightInfoExtension_DienVan(string date, string callSign)
        {
            DataTable dt = new DayFlightTotalInfoDAL().GetAllRealPlanMessage(DateTime.Parse(date), callSign);
            string kq = "";
            if (dt == null) return kq;
            if (dt.Rows.Count < 0) return kq;
            kq += "<tbody>";
            foreach (DataRow r in dt.Rows)
            {
                var d = DateTime.Parse(r["LETTERNBR_PK"].ToString());
                kq += $"<tr>";
                kq += $"<td>{d.Day} - {d.Hour}:{d.Minute}</td>";                            //col 1
                kq += $"<td>{d.ToString("dd-MM-yyyy")}</td>";                               //col 2
                kq += $"<td>{r["LETTER_TYPE"]}</td>";                                       //col 3
                kq += $"<td>{r["FLIGHTNBR"]}</td>";                                         //col 4
                kq += $"<td>{r["REGISTRATION"]}</td>";                                      //col 5
                kq += $"<td>{r["FROM_AIRP"]}</td>";                                         //col 6
                kq += $"<td>{r["TO_AIRP"]}</td>";                                           //col 7
                kq += $"<td>{r["ETD"]}</td>";                                               //col 8
                kq += $"<td>{r["ETA"]}</td>";                                               //col 9
                kq += $"<td>{r["VIA"]}</td>";                                               //col 10
                kq += $"<td>{r["TEXT"].ToString().Replace("/r/n", "</br>")}</td>";          //col 11
                kq += $"<td>{r["CNL_TYPE"]}</td>";                                          //col 12
                kq += $"</tr>";
            }
            kq += "</tbody>";
            kq += "<script>$('#tblDienVan tbody').html('" + kq + "');</script>";
            return kq;
        }
        private string FlightInfoExtension_HasPerm(string sContent)
        {
            return string.IsNullOrEmpty(sContent) ? "<script>$('#txtValidateTime').text('No');</script>" : "<script> $('#txtHasPermission').text('Yes!');</script>";
        }
        private string btnAccessInfoChange_Onclick(string thamso)
        {
            if (new DayFlightTotalInfoDAL().AccessInfoChange(thamso))
                return "<script>alert('Update sussess!')</script>";
            return "<script>alert('Update error!')</script>";
        }
        private string CellColorIfChange(string sColName, string sValue)
        {
            string[] c = sValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in c)
            {
                if (item.Contains(sColName))
                    return "1";
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sColName">Column check has change</param>
        /// <param name="sValue">datasource column CONTENTCHANGE</param>
        /// <param name="sReturn">if has change column => return sReturn</param>
        /// <returns></returns>
        private string CellColorIfChange(string sColName, string sValue, string sReturn)
        {
            string[] c = sValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in c)
            {
                if (item.Contains(sColName))
                    return sReturn;
            }
            return "";
        }
        #endregion
        private string c2_Tip(DataRow r)
        {
            string kq = "";
            kq += "<table class=\\'table table-bodered tableTip\\'>";
            kq += $"<tbody>";
            int cot = 4;
            int tong = r.Table.Columns.Count;
            int dong = (r.Table.Columns.Count % cot) == 0 ? r.Table.Columns.Count / cot : (r.Table.Columns.Count / cot) + 1;
            for (int i = 0; i < dong; i++)
            {
                kq += "<tr>";
                for (int j = 0; j < cot; j++)
                {
                    if (i * cot + j > tong - 1)
                        kq += "<td></td>";
                    else
                        kq += $"<th scope=\\'row\\'>{r.Table.Columns[i * cot + j].ColumnName}</td><td>{cellHasSpan(r[r.Table.Columns[i * cot + j].ColumnName])}</td>";
                }
                kq += "</tr>";
            }
            kq += "</tbody>";
            kq += "</table>";
            return kq;
        }
        #endregion
        #region page event

        #endregion
        #region control event

        #endregion
        #region class

        #endregion
    }
}