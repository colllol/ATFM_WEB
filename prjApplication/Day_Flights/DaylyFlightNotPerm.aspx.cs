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
    public partial class DaylyFlightNotPerm : PageBaseCallBack
    {
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// Load page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                case "UpdateColorBy":
                    kq = UpdateColorBy(ThamSo);
                    break;
                case "SetDefaultColor":
                    kq = SetDefaultColor(ThamSo);
                    break;
                case "RuntimeSoundNotification":
                    kq = RuntimeSoundNotification();
                    break;
                case "GetMessageAlert":
                    kq = GetMessageAlert();
                    break;
                case "GetAlarmEvent":
                    kq = GetAlarmEvent();
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
        private static string trColorClass(object letterType, object etd, object atd, object callSign, object permNbr, object hasPerm)
        {
            if (hasPerm.ToString() == "0")
                return ColorHelper.GetColorCode(clsColor.cssTrKhongCoPhep);
            if (string.IsNullOrEmpty(callSign.ToString()) && string.IsNullOrEmpty(permNbr.ToString()))
                return ColorHelper.GetColorCode(clsColor.cssTrKhongCoPhep);
            if (letterType.ToString().ToLower() == "fpl" && string.IsNullOrEmpty(atd.ToString()))
                return ColorHelper.GetColorCode(clsColor.cssTrChuaThucHien);
            switch (letterType.ToString().ToLower())
            {
                case "arr":
                    return ColorHelper.GetColorCode(clsColor.cssTrDienVanHaCanh);
                case "chg":
                    return ColorHelper.GetColorCode(clsColor.cssTrThayDoi);
                case "cnl":
                    return ColorHelper.GetColorCode(clsColor.cssTrHuy);
                case "dla":
                    return ColorHelper.GetColorCode(clsColor.cssTrHoanBay);
                case "fpl":
                    return ColorHelper.GetColorCode(clsColor.cssTrDaCoKeHoachBay);
                case "dep":
                    return ColorHelper.GetColorCode(clsColor.cssTrDienVanCatCanh);
            }
            return "";
        }
        private static string tdColorClassChange(string sColName, string sValue)
        {
            string[] c = sValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in c)
            {
                if (item.Contains(sColName))
                    return ColorHelper.GetColorCode(clsColor.cssTdColChange);
            }
            return "";
        }
        private static string textColorClassChange(string sColName, string sValue)
        {
            string[] c = sValue.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in c)
            {
                if (item.Contains(sColName))
                    return ColorHelper.GetColorCode(clsColor.cssTextColChange);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="etd"></param>
        /// <param name="atd"></param>
        /// <returns></returns>
        private bool CompareHour(object etd, object atd)
        {
            try
            {
                int gioEtd, phutEtd, gioAtd, phutAtd;
                gioAtd = Convert.ToInt32(atd.ToString().Substring(atd.ToString().Length - 4, 2));
                phutAtd = Convert.ToInt32(atd.ToString().Substring(atd.ToString().Length - 2, 2));
                gioEtd = Convert.ToInt32(etd.ToString().Substring(etd.ToString().Length - 4, 2));
                phutEtd = Convert.ToInt32(etd.ToString().Substring(etd.ToString().Length - 2, 2));
                DateTime _etd = new DateTime(2010, 1, 1, gioEtd, phutEtd, 01);
                DateTime _atd = new DateTime(2010, 1, 1, gioAtd, phutAtd, 01);
                if ((_etd - _atd).Minutes > 5)
                    return false;
                return true;
            }
            catch
            {
                return true;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        protected string RenderTopBarInfo()
        {
            string kq = "";
            //DataTable dt = new DayFlightTotalInfoDAL().GetAll();
            //if (dt.Rows.Count > 0)
            //{
            //    kq += $"<ul class=\\'list-inline\\'>";
            //    kq += $"<li class=\\'list-inline-item\\'>Total KHB: <span id=\\'totalRecord\\' class=\\'badge\\'>{dt.Rows[0]["TOTALKHB"]}</span></li>";
            //    kq += $"<li class=\\'list-inline-item\\'>FPL: <span class=\\'badge\\'>{dt.Rows[0]["FPL"]}</span></li>";
            //    kq += $"<li class=\\'list-inline-item\\'>Recive message: <span class=\\'badge\\'>{dt.Rows[0]["ToTalReciveMessage"]}</span></li>";
            //    kq += $"<li class=\\'list-inline-item\\'>DEP <span class=\\'badge\\'>{dt.Rows[0]["DEP"]}</span></li>";
            //    kq += $"<li class=\\'list-inline-item\\'>CHG <span class=\\'badge\\'>{dt.Rows[0]["CHG"]}</span></li>";
            //    kq += $"<li class=\\'list-inline-item\\'>CNL <span class=\\'badge\\'>{dt.Rows[0]["CNL"]}</span></li>";
            //    kq += $"<li class=\\'list-inline-item\\'>DLA <span class=\\'badge\\'>{dt.Rows[0]["DLA"]}</span></li>";
            //    kq += $"<li class=\\'list-inline-item\\'>ARR <span class=\\'badge\\'>{dt.Rows[0]["ARR"]}</span></li>";
            //    // kq += $"<li class=\\'list-inline-item\\'>Total KHB <span class=\\'badge\\'>{dt.Rows[0]["TOTALRECORD"]}</span></li>";
            //    kq += "</ul>";
            //    kq = $"$('#infoTotal').html('{kq}');";
            //}
            //dt = new DayFlightTotalInfoDAL().GetInfoTopBar3();
            //if (dt == null) return kq;
            //kq += $"$('#lbltopbarNotificon_TotalMessage').text('{dt.Rows[0]["TotalMessage"]}');";
            //kq += $"$('#lbltopbarNotificon_TotalFlight').text('{dt.Rows[0]["TotalFlight"]}');";
            //kq = $"<script>{kq}</script>";
            //kq += $"{GetAlarmEvent()}";
            //kq += GetMessageAlert();
            //kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            ////kq += mes;
            return kq;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected string RenderGrdSource(DataTable dt)
        {
            /* hungtn edit 201801041025
             * chuyển heade table render ra ngoài aspx
             */
            string kq = "";
            kq += ContentRowGrdSource(dt);
            return kq;
        }

        protected string RenderGrdSource_First(DataTable dt)
        {
            /* hungtn edit 201801041025
             * chuyển heade table render ra ngoài aspx
             */
            string kq = "";
            kq += ContentRowGrdSource_First(dt);
            return kq;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string cellHasSpan(string value)
        {
            if (value == "01/01/0001 12:00:00 SA")
                return string.Empty;
            if (string.IsNullOrEmpty(value) && string.IsNullOrWhiteSpace(value))
                return string.Empty;
            else return $"<span>{value}</span>";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thamso"></param>
        /// <returns></returns>
        private string btnSearch_Onclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            DataTable dt = new DataTable();
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsDaylyFlightSearch>(thamso, dateTimeConverter);
            switch (obj.OptionDate)
            {
               
                case "0":
                    obj.FLIGHTDATE = DateTime.Today;
                    dt = new DayFlightsDAL().GetTableFlightNotPerm(obj);
                    break;
                case "-1":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-1);
                    dt = new DayFlightsDAL().GetTableFlightNotPerm(obj);
                    break;
                case "-2":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-2);
                    dt = new DayFlightsDAL().GetTableFlightNotPerm(obj);
                    break;
                case "-3":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-3);
                    dt = new DayFlightsDAL().GetTableFlightNotPerm(obj);
                    break;
                case "-4":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-4);
                    dt = new DayFlightsDAL().GetTableFlightNotPerm(obj);
                    break;
                case "1":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(1);
                    dt = new DayFlightsDAL().GetTableFlightNotPerm(obj);                    
                    break;

            }
            return RenderGrdSource(dt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thamso"></param>
        /// <returns></returns>
        private string LoadGrdSourceScroll(string thamso)
        {
            /*
             *    16042018   update
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsDaylyFlightSearch>(thamso, dateTimeConverter);
            DataTable dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
            return ContentRowGrdSource(dt);
            */


            return btnSearch_Onclick(thamso);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
       
        
        private string ContentRowGrdSource(DataTable dt)
        {
            string c0 = "", c1 = "", c2 = "", c3 = "", c4 = "", c5 = "", c6 = "", c7 = "", c8 = "", c9 = "", c10 = "", c11 = "", c12 = "", c13 = "", c14 = "", c15 = "", c16 = "", c17 = "", c18 = "",c19="",cview="", kq = "";
            string c4B = "", c5B = "";
            string mes = "";
            if (dt == null) return "";
            string _record = "0";
            foreach (DataRow r in dt.Rows)
            {
                _record = r["SumRecord"].ToString();
                if (r["STATUSXL"].ToString() == "0")
                {                    
                   kq += $"<tr ondblclick=\"viewPopupInfoExtension(this)\""
                   + $" data-id=\"{r["ID"]}\" data-timeM=\"{DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{r["RNUM"]}' "
                   + $"class=\"selectdel\" data-CallSign=\"{r["FLIGHTNBR"]}\">";

                    
                }
                else
                {
                    kq += $"<tr ondblclick=\"viewPopupInfoExtension(this)\""
                    + $" data-id=\"{r["ID"]}\" data-timeM=\"{DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{r["RNUM"]}' "
                    + $"class=\"selectdxl\" data-CallSign=\"{r["FLIGHTNBR"]}\">";

                }
                
                c19 = $"<td>{cellHasSpan(r["NBR"].ToString())}</td>";
                c0 = $"<td style=\"width:10px;\" class=\"tdIconStatus\">"
                    + "<div id='divStatusIcon' class='action-buttons'>"
                    + (!string.IsNullOrEmpty(r["CHANGEVALUE"].ToString()) ? $"<a data-toggle=\"tooltip\" title=\"Please check info!(double click)\"><i class=\"glyphicon glyphicon-check\"></i></a>" : "")
                    + (r["CHECKCHANGE"].ToString() == "1" ? $"<a data-toggle=\"tooltip\" title=\"Checked info\"><i class=\"glyphicon glyphicon-eye-close\"></i></a>" : "")
                    + $"<a data-toggle=\"tooltip\" title=\"View Permission\" onclick=\"GetPermInfoById('{r["FLIGHT_ID"]}')\"><i class=\"glyphicon glyphicon-th\"></i></a>"
                    + "</div></td>";

                cview = $"<td style='white-space: nowrap;background-color:white;'><div class='action-buttons'><i id='btnView" + r["FLIGHT_ID"] + "' class='ace-icon fa fa-thumbs-up bigger-130' onclick=\"btnUpdateStatusLetter_New_OnClick(" + r["ID"] + ");\"></i></a></td>";

                c1 = $"<td>{cellHasSpan(r["LETTER_TYPE"].ToString())}</td>";

                c2 = $"<td onmouseout=\"hideddrivetip();\" onmouseover=\"ddrivetip('{c2_Tip(r)}', 'aquamarine', 1000);\">"
                    + $"<span>{r["PERMNBR"].ToString().ToUpper()}</span></td>";

                c3 = $"<td class=\"cssTdCallSign\"><b>{cellHasSpan(r["FLIGHTNBR"].ToString())}</b></td>";

                c4 = $"<td>{r["FROM_AIRP"]}</td>";

                c4B = $"<td>{r["ETD"]}</td>";

                c5 = $"<td>{r["TO_AIRP"]}</td>";

                c5B = $"<td>{r["ETA"]}</td>";
                
                c6 = $"<td>{r["EOBT"]}</td>";

               
                c7 = $"<td>{r["ATD"]}</td>";

                c8 = $"<td>{r["ATA"]}</td>";

                c9 = $"<td>{r["CRAFT_T"]}</td>";

                c10 = $"<td>{r["CRAFT_TYPE"]}</td>";

                c11 = $"<td>{r["REGISTRATION"]}</td>";

                c12 = $"<td>{r["PURPOSE"]}</td>";

                c13 = $"<td>{r["MTOW"]}</td>";

                //c14 = $"<td>{r["REMARK"]}</td>";
                c14 = $"<td>{r["NOIDUNGXL"]}</td>";

                c15 = $"<td>{r["FLIGHT_TYPE"]}</td>";

                c16 = $"<td>{r["VIAGOC"]}</td>";             

                c17 = $"<td>{r["ROUTE_TT"]}</td>";

                c18 = $"<td>{r["VIA"]}</td>";

                /* c6, c10 chua bind */
                kq += c0 + cview + c1 + c19 + c2 + c11 + c3 + c4 + c5 + c4B + c6 + c5B + c7 + c8 + c16 + c17 + c9 + c10 + c12 + c14 + c18;
                kq += "</tr>";
            }
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            return kq + "&&&&" + _record;
        }

        private string ContentRowGrdSource_First(DataTable dt)
        {
            string c0 = "", c1 = "", c2 = "", c3 = "", c4 = "", c5 = "", c6 = "", c7 = "", c8 = "", c9 = "", c10 = "", c11 = "", c12 = "", c13 = "", c14 = "", c15 = "", c16 = "", c17 = "", c18 = "", c19 = "", cview="", kq = "";
            string c4B = "", c5B = "";
            string mes = "";
            if (dt == null) return "";
            string _record = "0";
            foreach (DataRow r in dt.Rows)
            {
                _record = r["SumRecord"].ToString();
                if (r["STATUSXL"].ToString() == "0")
                {
                    kq += $"<tr ondblclick=\"viewPopupInfoExtension(this)\""
                      + $" data-id=\"{r["ID"]}\" data-timeM=\"{DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{r["RNUM"]}' "
                      + $"class=\"selectdel\" data-CallSign=\"{r["FLIGHTNBR"]}\">";

                    
                }
                else
                {
                    kq += $"<tr ondblclick=\"viewPopupInfoExtension(this)\""
                    + $" data-id=\"{r["ID"]}\" data-timeM=\"{DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{r["RNUM"]}' "
                    + $"class=\"selectdxl\" data-CallSign=\"{r["FLIGHTNBR"]}\">";

                }
                c19 = $"<td>{cellHasSpan(r["NBR"].ToString())}</td>";
                c0 = $"<td style=\"width:10px;\" class=\"tdIconStatus\">"
                    + "<div id='divStatusIcon' class='action-buttons'>"
                    + (!string.IsNullOrEmpty(r["CHANGEVALUE"].ToString()) ? $"<a data-toggle=\"tooltip\" title=\"Please check info!(double click)\"><i class=\"glyphicon glyphicon-check\"></i></a>" : "")
                    + (r["CHECKCHANGE"].ToString() == "1" ? $"<a data-toggle=\"tooltip\" title=\"Checked info\"><i class=\"glyphicon glyphicon-eye-close\"></i></a>" : "")
                    + $"<a data-toggle=\"tooltip\" title=\"View Permission\" onclick=\"GetPermInfoById('{r["FLIGHT_ID"]}')\"><i class=\"glyphicon glyphicon-th\"></i></a>"
                    + "</div></td>";

                cview = $"<td style='white-space: nowrap;background-color:white;'><div class='action-buttons'><i id='btnView" + r["FLIGHT_ID"] + "' class='ace-icon fa fa-thumbs-up bigger-130' onclick=\"btnUpdateStatusLetter_New_OnClick(" + r["ID"] + ");\"></i></a></td>";

                c1 = $"<td>{cellHasSpan(r["LETTER_TYPE"].ToString())}</td>";

                c2 = $"<td onmouseout=\"hideddrivetip();\" onmouseover=\"ddrivetip('{c2_Tip(r)}', 'aquamarine', 1000);\">"
                    + $"<span>{r["PERMNBR"].ToString().ToUpper()}</span></td>";

                c3 = $"<td class=\"cssTdCallSign\"><b>{cellHasSpan(r["FLIGHTNBR"].ToString())}</b></td>";

                c4 = $"<td>{r["FROM_AIRP"]}</td>";

                c4B = $"<td>{r["ETD"]}</td>";

                c5 = $"<td>{r["TO_AIRP"]}</td>";

                c5B = $"<td>{r["ETA"]}</td>";

                c6 = $"<td>{r["EOBT"]}</td>";


                c7 = $"<td>{r["ATD"]}</td>";

                c8 = $"<td>{r["ATA"]}</td>";

                c9 = $"<td>{r["CRAFT_T"]}</td>";

                c10 = $"<td>{r["CRAFT_TYPE"]}</td>";

                c11 = $"<td>{r["REGISTRATION"]}</td>";

                c12 = $"<td>{r["PURPOSE"]}</td>";

                c13 = $"<td>{r["MTOW"]}</td>";

                //c14 = $"<td>{r["REMARK"]}</td>";
                c14 = $"<td>{r["NOIDUNGXL"]}</td>";

                c15 = $"<td>{r["FLIGHT_TYPE"]}</td>";

                c16 = $"<td>{r["VIAGOC"]}</td>";

                c17 = $"<td>{r["ROUTE_TT"]}</td>";

                c18 = $"<td>{r["VIA"]}</td>";

                /* c6, c10 chua bind */
                kq += c0 + cview + c1 + c19 + c2 + c11 + c3 + c4 + c5 + c4B + c6 + c5B + c7 + c8 + c16 + c17 + c9 + c10 + c12 + c14 + c18;
                kq += "</tr>";
            }            
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            return kq;
        }
        protected string grdSourceLoadFirt()
        {
            
            return RenderGrdSource_First(new DayFlightsDAL().GetTableFlightNotPerm(new clsDaylyFlightSearch() { KHUNGGIO1 = "0000", KHUNGGIO2 = "2359", RowStart = 1, RowFinish = 20, PERMTYPE = "", FLIGHTDATE = DateTime.Today }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thamso"></param>
        /// <returns></returns>
        private string viewPopupInfoExtension(string[] thamso)
        {
            string kq = "";
            DataTable dt = new DayFlightTotalInfoDAL().GetInfoChangeById(thamso[0]);
            if (dt == null) return "";
            kq += FlightInfoExtension_IsChange(dt.Rows[0]["CHANGEVALUE"].ToString());
            kq += FlightInfoExtension_IsTimeValid(dt.Rows[0]["HASTIMEVALID"].ToString() == "1" ? "YES" : "NO");
            kq += FlightInfoExtension_ChangeInfo(dt.Rows[0]["CHANGEVALUE"].ToString());
            kq += FlightInfoExtension_HasPerm(dt.Rows[0]["HASPERM"].ToString() == "1" ? "YES" : "NO");
            kq += FlightInfoExtension_DienVanExt(thamso[0]);
            // view button access checked info change
            kq += FlightInfoExtension_ShowButtonAccess(thamso[0]);
            kq += $"<script>$('#btnUpdateStatusLetter').attr('onclick', 'btnUpdateStatusLetter_OnClick(\\'{thamso[0]}\\')');</script>";
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "<br>");
            return kq;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sId"></param>
        /// <returns></returns>
        private string FlightInfoExtension_ShowButtonAccess(string sId)
        {
            var obj = new DayFlightsDAL().GetOneObject(sId);
            if (string.IsNullOrEmpty(obj.CHANGEVALUE)) return "<script>$('#btnAcceseeInfoChange').hide()</script>";
            return "<script>$('#btnAcceseeInfoChange').show().attr('onclick','GetArgWithPostBack(\\'" + obj.ID + "\\' + phanCachArg + \\'btnAccessInfoChange_Onclick\\', \\'btnAccessInfoChange_Onclick\\')')</script>";
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="callSign"></param>
        /// <returns></returns>
        private string FlightInfoExtension_DienVan(string date, string callSign)
        {
            DataTable dt = new DayFlightTotalInfoDAL().GetAllRealPlanMessage(UltilFunc.ToDate(date, "dd-MM-yyyy"), callSign);
            string kq = "";
            if (dt == null) return kq;
            if (dt.Rows.Count < 0) return kq;
            string messageContent = "";

            //kq += "<tbody>";
            foreach (DataRow r in dt.Rows)
            {
                //DateTime d = DateTime.Parse(r["LETTERNBR_PK"].ToString()).ToUniversalTime();
                DateTime d = DateTime.Parse(r["LETTERNBR_PK"].ToString());
                kq += $"<tr>";
                kq += $"<td style=\"white-space: nowrap;\">{d.ToString("DD")} - {d.ToString(";")}:{d.ToString("MM")}</td>";
                kq += $"<td style=\"white-space: nowrap;\">{d.ToString("dd-MM-yyyy")}</td>";                               //col 2
                kq += $"<td>{r["LETTER_TYPE"]}</td>";                                       //col 3
                kq += $"<td>{r["FLIGHTNBR"]}</td>";                                         //col 4
                kq += $"<td>{r["REGISTRATION"]}</td>";                                      //col 5
                kq += $"<td>{r["FROM_AIRP"]}</td>";                                         //col 6
                kq += $"<td>{r["TO_AIRP"]}</td>";                                           //col 7
                //kq += $"<td>{r["ETD"]}</td>";                                               //col 8
                //kq += $"<td>{r["ETA"]}</td>";                                               //col 9
                kq += $"<td>{r["VIA"]}</td>";                                               //col 10
                kq += $"<td>{r["TEXT"].ToString().Replace("/r/n", "</br>")}</td>";          //col 11
                kq += $"<td>{r["CNL_TYPE"]}</td>";                                          //col 12
                kq += $"</tr>";
                messageContent += $"{r["TEXT"].ToString()}\r\n------------------------------------------------\r\n";
            }
            //kq += "</tbody>";
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            messageContent = System.Text.RegularExpressions.Regex.Replace(messageContent, @"\r\n?|\n", "<br>");
            //messageContent = "";
            kq += "<script>$('#tblDienVan tbody tr').remove(); $('#tblDienVan tbody').append('" + kq + "'); $('#txtContentMessage').val('" + messageContent + "');</script>";
            return kq;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="callSign"></param>
        /// <returns></returns>
        private string FlightInfoExtension_DienVanExt(string id)
        {
            DataTable dt = new DayFlightTotalInfoDAL().GetAllRealPlanMessageExt(id);
            string kq = "";
            if (dt == null) return kq;
            if (dt.Rows.Count < 0) return kq;
            string messageContent = "";
            //kq += "<tbody>";
            foreach (DataRow r in dt.Rows)
            {
                //var d = DateTime.Parse(r["LETTERNBR_PK"].ToString()).ToUniversalTime();
                var d = DateTime.Parse(r["LETTERNBR_PK"].ToString());
                kq += $"<tr>";
                kq += $"<td style=\"white-space: nowrap;\">{d.ToString("dd")} - {d.ToString("HH:ss")}</td>";                            //col 1
                kq += $"<td style=\"white-space: nowrap;\">{d.ToString("dd-MM-yyyy")}</td>";                               //col 2
                kq += $"<td>{r["LETTER_TYPE"]}</td>";                                       //col 3
                kq += $"<td>{r["FLIGHTNBR"]}</td>";                                         //col 4
                kq += $"<td>{r["REGISTRATION"]}</td>";                                      //col 5
                kq += $"<td>{r["FROM_AIRP"]}</td>";                                         //col 6
                kq += $"<td>{r["TO_AIRP"]}</td>";                                           //col 7
                //kq += $"<td>{r["ETD"]}</td>";                                               //col 8
                //kq += $"<td>{r["ETA"]}</td>";                                               //col 9
                kq += $"<td>{r["VIA"]}</td>";                                               //col 10
                kq += $"<td>{r["TEXT"].ToString().Replace("/r/n", "</br>")}</td>";          //col 11
                kq += $"<td>{r["FROM_PL"]}</td>";                                          //col 12
                kq += $"</tr>";
                messageContent += $"{r["TEXT"].ToString()}\r\n------------------------------------------------\r\n";
            }
            //kq += "</tbody>";
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            messageContent = System.Text.RegularExpressions.Regex.Replace(messageContent, @"\r\n?|\n", "<br>");
            //messageContent = "";
            kq += "<script>$('#tblDienVan tbody tr').remove(); $('#tblDienVan tbody').append('" + kq + "'); $('#txtContentMessage').val('" + messageContent + "');</script>";
            return kq;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sContent"></param>
        /// <returns></returns>
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
        private string UpdateColorBy(string[] thamso)
        {
            var ax = new DayFlightSetColorDAL().UpdateColorByIdUser(thamso[0], thamso[2], thamso[1], thamso[2]);
            return ax ? "Update sussess!" : "Update error!";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thamso"></param>
        /// <returns></returns>
        private string SetDefaultColor(string[] thamso)
        {
            return new DayFlightSetColorDAL().SetDefaultColor(thamso[1], thamso[0]) ? "Update sussess!" : "Update error!";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string RuntimeSoundNotification()
        {
            return new DayFlightSetColorDAL().RuntimeSoundNotification() ? "1" : "-1";
        }
        private string GetAlarmEvent()
        {
            DataTable dt = new clsResuftAPI().GetTableApiExtension("FLIGHT_DAYFLIGHT", "GetEventAlert", new { P_DATE = DateTime.Now });
            if (dt == null)
                return "";
            string kq = "Warring event:</br>";
            foreach (DataRow r in dt.Rows)
            {
                kq += $"{r["EVENT_NAME"]}</br>";
            }
            return $"<script>$('#divAlarm').html('{kq}')</script>";
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Lay noi dung thong bao khi co tau bay thay doi theo dieu kien
        /// </summary>
        /// <param name="etd"></param>
        /// <param name="eta"></param>
        /// <param name="contentChange"></param>
        /// <param name="callSign"></param>
        /// <returns></returns>
        private string GetMessageAlert(object etd, object eta, object contentChange, object callSign, object hasPerm)
        {
            string kq = "";
            kq += $"CallSign {callSign.ToString()}: ";
            int le = kq.Length;
            if (!CompareHour(etd, eta))
                kq += $"Bay sớm.</br>";
            if (hasPerm.ToString() == "0")
                kq += $"Không phép!";
            if (!string.IsNullOrEmpty(contentChange.ToString()))
            {
                string[] con = contentChange.ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in con)
                {
                    string[] p = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (p[0].ToUpper().Contains("VIA"))
                    {
                        kq += $"Thay đổi đường bay.</br>";
                    }
                    if (p[0].ToUpper().Contains("FROM_AIRP"))
                    {
                        kq += $"Thay đổi sân bay hạ cánh.</br>";
                    }
                    if (p[0].ToUpper().Contains("TO_AIRP"))
                    {
                        kq += $"Thay đổi sân bay cất cánh.</br>";
                    }
                    if (p[0].ToUpper().Contains("CRAFT_ID"))
                    {
                        kq += $"Thay đổi tàu bay.</br>";
                    }
                }
            }
            if (kq.Length == le)
                return "";
            return kq;
        }
        private string GetMessageAlert()
        {
            string kq = "";
            try
            {
                DataTable dt = new clsResuftAPI().GetTableApiExtension("FLIGHT_DAYFLIGHT", "GetMessageAlert", null);
                if (dt == null) return "";
                int c = 0;
                foreach (DataRow r in dt.Rows)
                {
                    string ct = $"CallSign {r["FLIGHTNBR"]}: ";
                    int le = ct.Length;
                    if (r["HASPERM"].ToString() == "0")
                        ct += $"Không phép!</br>";
                    if (!string.IsNullOrEmpty(r["CHANGEVALUE"].ToString()))
                    {
                        string[] con = r["CHANGEVALUE"].ToString().Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in con)
                        {
                            string[] p = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (p[0].ToUpper().Contains("Bay sớm"))
                            {
                                ct += $"Thay đổi đường bay.</br>";
                            }
                            if (p[0].ToUpper().Contains("ROUTE"))
                            {
                                ct += $"Thay đổi đường bay.</br>";
                            }
                            if (p[0].ToUpper().Contains("FROM_AIRP") || p[0].ToUpper().Contains("TO_AIRP"))
                            {
                                ct += $"Thay đổi sân bay cất hạ cánh.</br>";
                            }
                            if (p[0].ToUpper().Contains("CRAFT_ID"))
                            {
                                ct += $"Thay đổi tàu bay.</br>";
                            }
                        }
                    }
                    if (ct.Length != le)
                        kq += ct;
                    c++;
                    if (c > 200) break;
                }
            }
            catch (Exception ex)
            {
                kq = "Error!";

            }
            //return kq = $"<script>notifyMe('{kq}');</script>";
            return kq = $"<script>$('#divMessegeChange').html('{kq}');</script>";
        }
        #endregion                      

    }
}