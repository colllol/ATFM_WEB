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
    public partial class DaylyFlightChange : PageBaseCallBack
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
                    return ColorHelper.GetColorCode(clsColor.cssTextColChangeCustom);
                case "2":
                    return ColorHelper.GetColorCode(clsColor.cssTrChange);
                case "3":
                    return ColorHelper.GetColorCode(clsColor.cssTrChuaThucHien);
                default:
                    return ColorHelper.GetColorCode(clsColor.cssTrChuaThucHienCustom);
            }
        }
        private static string tdColor(object val)
        {
            switch (val.ToString())
            {
                case "1":
                    return ColorHelper.GetColorCode(clsColor.cssTdCallSign);
                case "2":
                    return ColorHelper.GetColorCode(clsColor.cssTdCallSignCustom);
                case "3":
                    return ColorHelper.GetColorCode(clsColor.cssTdColChange);
                default:
                    return ColorHelper.GetColorCode(clsColor.cssTdColChangeCustom);
            }
        }
        private static string itemColor(object val)
        {
            switch (val.ToString())
            {
                case "1":
                    return ColorHelper.GetColorCode(clsColor.cssTrDaCoKeHoachBay);
                case "2":
                    return ColorHelper.GetColorCode(clsColor.cssTrDaCoKeHoachBayCustom);
                case "3":
                    return ColorHelper.GetColorCode(clsColor.cssTrDienVanCatCanhCustom);
                default:
                    return ColorHelper.GetColorCode(clsColor.cssTrDienVanHaCanh);
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
            DataTable dt = new DayFlightsDAL().GetTableWithValueSearch(obj);

            return RenderGrdSource(dt);
        }
        private string LoadGrdSourceScroll(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsDaylyFlightSearch>(thamso, dateTimeConverter);
            DataTable dt = new DayFlightsDAL().GetTableFlightHasChange(obj);
            return ContentRowGrdSource(dt);
        }
        private string ContentRowGrdSource(DataTable dt)
        {
            string c0 = "", c1 = "", c2 = "", c3 = "", c4 = "", c5 = "", c6 = "", c7 = "", c8 = "", c9 = "", c10 = "", c11 = "", c12 = "", c13 = "", c14 = "", c15 = "", c16 = "", c17 = "", c18 = "", kq = "";
            string c4B = "", c5B = "";
            if (dt == null) return "";
            foreach (DataRow r in dt.Rows)
            {
                kq += $"<tr ondblclick=\"viewPopupInfoExtension(this)\""
                    + $" data-id=\"{r["FLIGHT_ID"]}\" data-timeM=\"{DateTime.Parse(r["LETTERNBR_PK"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{r["RNUM"]}' "
                    + $"class=\"{trColor(r["HASPERM"].ToString() == "1" ? r["PLAN_STATUS"] : "CssTrDo")}\" data-CallSign=\"{r["FLIGHTNBR"]}\">";

                c0 = $"<td class=\"tdIconStatus\">"
                    + "<div id='divStatusIcon' class='action-buttons'>"
                    + (!string.IsNullOrEmpty(r["CONTENTCHANGE"].ToString()) ? $"<a data-toggle=\"tooltip\" title=\"Please check info!(double click)\"><i class=\"glyphicon glyphicon-check\"></i></a>" : "")
                    + (r["CHECKCHANGE"].ToString() == "1" ? $"<a data-toggle=\"tooltip\" title=\"Checked info\"><i class=\"glyphicon glyphicon-eye-close\"></i></a>" : "")
                    + "</div></td>";

                c1 = $"<td>{cellHasSpan(r["STATUS"].ToString())}</td>";

                c2 = $"<td onmouseout=\"hideddrivetip();\" onmouseover=\"ddrivetip('{c2_Tip(r)}', 'aquamarine', 1000);\">"
                    + $"<span>{r["PERMNBR"]}</span></td>";

                c3 = $"<td>{cellHasSpan(r["FLIGHTNBR"].ToString())}</td>";

                c4 = $"<td class=\"{tdColor(CellColorIfChange("FROM_AIRP", r["CONTENTCHANGE"].ToString()))}\">"
                    + $"{cellHasSpan(r["FROM_AIRP"])}</td>";

                c4B = $"<td class=\"{tdColor(CellColorIfChange("ETD", r["CONTENTCHANGE"].ToString()))}\">{cellHasSpan(r["ETD"])}</td>";

                c5 = $"<td class=\"{tdColor(CellColorIfChange("TO_AIRP", r["CONTENTCHANGE"].ToString()))}\">"
                    + $"{cellHasSpan(r["TO_AIRP"])}</td>";

                c5B = $"<td class=\"{tdColor(CellColorIfChange("ETA", r["CONTENTCHANGE"].ToString()))}\">{cellHasSpan(r["ETA"])}</td>";

                c6 = $"<td></td>";

                c7 = $"<td></td>";

                c8 = $"<td class=\"{tdColor(CellColorIfChange("ATA", r["CONTENTCHANGE"].ToString()))}\">{cellHasSpan(r["ATA"].ToString())}</td>";

                c9 = $"<td class=\"{tdColor(CellColorIfChange("CRAFT_ID", r["CONTENTCHANGE"].ToString()))}\">"
                    + $"{cellHasSpan(r["CRAFT_T"].ToString())}</td>";

                c10 = $"<td></td>";

                c11 = $"<td>{cellHasSpan(r["REGISTRATION"].ToString())}</td>";

                c12 = $"<td class=\"{tdColor(CellColorIfChange("PURPOSE", r["CONTENTCHANGE"].ToString(), "2"))}\">{cellHasSpan(r["PURPOSE"].ToString())}</td>";

                c13 = $"<td>{cellHasSpan(r["MTOW"].ToString())}</td>";

                c14 = $"<td class=\"{tdColor(CellColorIfChange("FLIGHTDATE", r["CONTENTCHANGE"].ToString(), "1"))}\">{cellHasSpan(r["FLIGHTDATE"])}</td>";

                c15 = $"<td class=\"{tdColor(CellColorIfChange("FLIGHT_TYPE", r["CONTENTCHANGE"].ToString(), "2"))}\">{cellHasSpan(r["FLIGHT_TYPE"].ToString())}</td>";

                c16 = $"<td class=\"{tdColor(CellColorIfChange("ROUTE", r["CONTENTCHANGE"].ToString(), "3"))}\">{cellHasSpan(r["ROUTE"].ToString())}</td>";

                c17 = $"<td class=\"{tdColor(CellColorIfChange("ROUTE_TT", r["CONTENTCHANGE"].ToString(), "2"))}\">{cellHasSpan(r["ROUTE_TT"])}</td>";

                c18 = $"<td class=\"{tdColor(CellColorIfChange("VIA", r["CONTENTCHANGE"].ToString(), "2"))}\">{cellHasSpan(r["VIA"].ToString())}</td>";


                kq += c0 + c1 + c2 + c3 + c4 + c5 + c4B + c5B + c6 + c7 + c8 + c16 + c17 + c9 + c10 + c11 + c12 + c14 + c18;
                kq += "</tr>";
            }
            return kq;
        }
        protected string grdSourceLoadFirt()
        {
            return RenderGrdSource(new DayFlightsDAL().GetTableFlightHasChange(new clsDaylyFlightSearch() { RowStart=1, RowFinish=20}));
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