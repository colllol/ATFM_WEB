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
    public partial class DaylyFlight : PageBaseCallBack
    {
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";

        protected int _i = 1;
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
                case "viewPopupInfoExtensionInsert":
                    kq = viewPopupInfoExtensionInsert(ThamSo);
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
                case "btnExport_Onclick":
                    kq = btnSearchExpot_Onclick(_arg[0]);
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

        
        private string CompareHourString(object etd, object atd)
        {
            string _return = "";
            try
            {
                if (Convert.ToInt32(etd.ToString()) < 2300)
                {
                    if (Convert.ToInt32(etd.ToString()) - Convert.ToInt32(atd.ToString()) > 5)
                        _return = "cssTdBaySom";
                }
            }
            catch
            {
                _return = "";
            }               
            return _return;          

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string RenderTopBar()
        {
            string kq = "";
            DataTable dt=new DayFlightTotalInfoDAL().GetInfoTopBar3();
            //dt = new DayFlightTotalInfoDAL().GetInfoTopBar3();
            if (dt == null) return kq;
            kq += $"$('#lbltopbarNotificon_TotalMessage').text('{dt.Rows[0]["TotalMessage"]}');";
            kq += $"$('#lbltopbarNotificon_TotalFlight').text('{dt.Rows[0]["TotalFlight"]}');";
            kq += $"$('#lbltopbarNotComplate_TotalFlight').text('{dt.Rows[0]["TotalFlightNotAtd"]}');";
            kq = $"<script>{kq}</script>";
            //kq += $"{GetAlarmEvent()}";
            //kq += GetMessageAlert();
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            return kq;
        }
        protected string RenderTopBarInfo()
        {
            string kq = "";
            DataTable dt = new DayFlightTotalInfoDAL().GetAll();
            if (dt.Rows.Count > 0)
            {
                kq += $"<ul class=\\'list-inline\\'>";
                kq += $"<li class=\\'list-inline-item\\'>Total KHB: <span id=\\'totalRecord\\' class=\\'badge\\'>{dt.Rows[0]["TOTALKHB"]}</span></li>";
                kq += $"<li class=\\'list-inline-item\\'>FPL: <span class=\\'badge\\'>{dt.Rows[0]["FPL"]}</span></li>";
                kq += $"<li class=\\'list-inline-item\\'>Recive message: <span class=\\'badge\\'>{dt.Rows[0]["ToTalReciveMessage"]}</span></li>";
                kq += $"<li class=\\'list-inline-item\\'>DEP <span class=\\'badge\\'>{dt.Rows[0]["DEP"]}</span></li>";
                kq += $"<li class=\\'list-inline-item\\'>CHG <span class=\\'badge\\'>{dt.Rows[0]["CHG"]}</span></li>";
                kq += $"<li class=\\'list-inline-item\\'>CNL <span class=\\'badge\\'>{dt.Rows[0]["CNL"]}</span></li>";
                kq += $"<li class=\\'list-inline-item\\'>DLA <span class=\\'badge\\'>{dt.Rows[0]["DLA"]}</span></li>";
                kq += $"<li class=\\'list-inline-item\\'>ARR <span class=\\'badge\\'>{dt.Rows[0]["ARR"]}</span></li>";
               // kq += $"<li class=\\'list-inline-item\\'>Total KHB <span class=\\'badge\\'>{dt.Rows[0]["TOTALRECORD"]}</span></li>";
                kq += "</ul>";
                kq = $"$('#infoTotal').html('{kq}');";
            }/*
            dt = new DayFlightTotalInfoDAL().GetInfoTopBar3();
            if (dt == null) return kq;
            kq += $"$('#lbltopbarNotificon_TotalMessage').text('{dt.Rows[0]["TotalMessage"]}');";
            kq += $"$('#lbltopbarNotificon_TotalFlight').text('{dt.Rows[0]["TotalFlight"]}');";
            kq += $"$('#lbltopbarNotComplate_TotalFlight').text('{dt.Rows[0]["TotalFlightNotAtd"]}');";
            kq = $"<script>{kq}</script>";
            kq += $"{GetAlarmEvent()}";
            kq += GetMessageAlert();
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            //kq += mes;
            */
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
        protected string RenderGrdSourceExport(DataTable dt)
        {
            /* hungtn edit 201801041025
             * chuyển heade table render ra ngoài aspx
             */
            string kq = "";
            kq += ContentRowGrdSourceExport(dt);
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
                    obj.FLIGHTDATE = DateTime.Today.AddDays(0);
                    //obj.FLIGHTDATE = DateTime.Today;
                    if (obj.RowStart.ToString()!="0")
                         _i = Int32.Parse(obj.RowStart.ToString());                   
                    
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-1":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-1);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-2":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-2);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-3":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-3);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-4":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-4);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-5":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-5);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-6":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-6);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-7":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-7);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-8":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-8);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-9":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-9);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-10":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-10);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "1":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(1);
                    if (obj.RowStart.ToString() != "0")
                        _i = Int32.Parse(obj.RowStart.ToString());
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);                    
                    //dt = new DayFlightsDAL().GetTableNgayNCong1(obj);
                    break;                
            }
            return RenderGrdSource(dt);
        }
        private string btnSearchExpot_Onclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            DataTable dt = new DataTable();
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<clsDaylyFlightSearch>(thamso, dateTimeConverter);
            switch (obj.OptionDate)
            {
                case "0":
                    obj.FLIGHTDATE = DateTime.Today;
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-1":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-1);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-2":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-2);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-3":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-3);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-4":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-4);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-5":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-5);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-6":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-6);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-7":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-7);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-8":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-8);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-9":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-9);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "-10":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(-10);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);
                    break;
                case "1":
                    obj.FLIGHTDATE = DateTime.Today.AddDays(1);
                    obj.RowStart = 1;
                    obj.RowFinish = 6000;
                    dt = new DayFlightsDAL().GetTableWithValueSearch(obj);                    
                    break;
            }
            return RenderGrdSourceExport(dt);
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
        private string ContentRowGrdSource_1(DataTable dt)
        {
            string c0 = "", c1 = "", c2 = "", c3 = "", c4 = "", c5 = "", c6 = "", c7 = "", c8 = "", c9 = "", c10 = "", c11 = "", c12 = "", c13 = "", c14 = "", c15 = "", c16 = "", c17 = "", c18 = "", kq = "";
            string c4B = "", c5B = "";
            string mes = "";
            if (dt == null) return "";
            foreach (DataRow r in dt.Rows)
            {
                kq += $"<tr ondblclick=\"viewPopupInfoExtension(this)\""
                    + $" data-id=\"{r["ID"]}\" data-flight-id=\"{r["FLIGHT_ID"]}\" data-timeM=\"{DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{r["RNUM"]}' "
                    + $"class=\"{trColorClass(r["LETTER_TYPE"], r["ETD"], r["ATD"], r["FLIGHTNBR"], r["PERMNBR"], r["HASPERM"])}\" data-CallSign=\"{r["FLIGHTNBR"]}\">";

                c0 = $"<td style=\"width:10px;\" class=\"tdIconStatus\">"
                    + "<div id='divStatusIcon' class='action-buttons'>"
                    + (!string.IsNullOrEmpty(r["CHANGEVALUE"].ToString()) ? $"<a data-toggle=\"tooltip\" title=\"Please check info!(double click)\"><i class=\"glyphicon glyphicon-check\"></i></a>" : "")
                    + (r["CHECKCHANGE"].ToString() == "1" ? $"<a data-toggle=\"tooltip\" title=\"Checked info\"><i class=\"glyphicon glyphicon-eye-close\"></i></a>" : "")
                    + $"<a data-toggle=\"tooltip\" title=\"View Permission\" onclick=\"GetPermInfoById('{r["FLIGHT_ID"]}')\"><i class=\"glyphicon glyphicon-th\"></i></a>"
                    + "</div></td>";

                //c1 = $"<td>{cellHasSpan(r["LETTER_TYPE"].ToString())}</td>";
                c1 = $"<td><input type='text' data-control='_updateAll' maxlength='6' class='sInput' id='txtOPER_ID" + r["ID"].ToString() + "' data-oldValue='" + cellHasSpan(r["LETTER_TYPE"].ToString()) + "' value='" + cellHasSpan(r["LETTER_TYPE"].ToString()) + "' onblur='checkIsUpdate(this)'/></td>";
                
                c2 = $"<td onmouseout=\"hideddrivetip();\" onmouseover=\"ddrivetip('{c2_Tip(r)}', 'aquamarine', 1000);\">"
                    + $"<span>{r["PERMNBR"]}</span></td>";

                c3 = $"<td class=\"cssTdCallSign\"><b>{cellHasSpan(r["FLIGHTNBR"].ToString())}</b></td>";

                c4 = $"<td class=\"{tdColorClassChange("FROM_AIRP", r["CHANGEVALUE"].ToString())} {textColorClassChange("FROM_AIRP", r["CHANGEVALUE"].ToString())}\">"
                    + $"{cellHasSpan(r["FROM_AIRP"])}</td>";

                c4B = $"<td class=\"{tdColorClassChange("ETD", r["CHANGEVALUE"].ToString())}\">{cellHasSpan(r["ETD"])}</td>";

                c5 = $"<td class=\"{tdColorClassChange("TO_AIRP", r["CHANGEVALUE"].ToString())}\">"
                    + $"{cellHasSpan(r["TO_AIRP"])}</td>";

                c5B = $"<td class=\"{tdColorClassChange("ETA", r["CHANGEVALUE"].ToString())}\">{cellHasSpan(r["ETA"])}</td>";                
                c6 = $"<td>{r["EOBT"]}</td>";

                c7 = $"<td class=\"{tdColorClassChange("ATD", r["CHANGEVALUE"].ToString())} {(!CompareHour(r["ETD"], r["ATD"]) ? ColorHelper.GetColorCode(clsColor.cssTextBaySom) : "")}\">{cellHasSpan(r["ATD"].ToString())}</td>";

                c8 = $"<td class=\"{tdColorClassChange("ATA", r["CHANGEVALUE"].ToString())} \">{cellHasSpan(r["ATA"].ToString())}</td>";

                c9 = $"<td class=\"{tdColorClassChange("CRAFT_ID", r["CHANGEVALUE"].ToString())}\">"
                    + $"{cellHasSpan(r["CRAFT_T"].ToString())}</td>";

                c10 = $"<td class=\"{tdColorClassChange("CRAFT_TYPE", r["CHANGEVALUE"].ToString())}\">"
                    + $"{cellHasSpan(r["CRAFT_TYPE"].ToString())}</td>";

                c11 = $"<td>{cellHasSpan(r["REGISTRATION"].ToString())}</td>";

                c12 = $"<td class=\"{tdColorClassChange("PURPOSE", r["CHANGEVALUE"].ToString())}\">{cellHasSpan(r["PURPOSE"].ToString())}</td>";

                c13 = $"<td>{cellHasSpan(r["MTOW"].ToString())}</td>";

                c14 = $"<td class=\"{tdColorClassChange("FLIGHTDATE", r["CHANGEVALUE"].ToString())}\">{cellHasSpan(r["FLIGHTDATE"])}</td>";

                c15 = $"<td class=\"{tdColorClassChange("FLIGHT_TYPE", r["CHANGEVALUE"].ToString())}\">{cellHasSpan(r["FLIGHT_TYPE"].ToString())}</td>";

                c16 = $"<td  class=\"{tdColorClassChange("VIAGOC", r["CHANGEVALUE"].ToString())}\">{cellHasSpan(r["VIAGOC"].ToString())}</td>";

                c17 = $"<td  class=\"{tdColorClassChange("ROUTE", r["CHANGEVALUE"].ToString())}\">{cellHasSpan(r["ROUTE"])}</td>";

                c18 = $"<td class=\"{tdColorClassChange("VIA", r["CHANGEVALUE"].ToString())}\">{r["VIA"]}</td>";

                /* c6, c10 chua bind */
                kq += c0 + c1 + c2 + c3 + c4 + c5 + c4B + c5B + c6 + c7 + c8 + c16 + c17 + c9 + c10 + c11 + c12 + c14 + c18;
                kq += "</tr>";
            }           
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            return kq;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        
        private string ContentRowGrdSourceExport(DataTable dt)
        {

            string c0 = "", c1 = "", c2 = "", c3 = "", c4 = "", c5 = "", c6 = "", c7 = "", c8 = "", c9 = "", c10 = "", c11 = "", c12 = "", c13 = "", c14 = "", c15 = "", c16 = "", c17 = "", c18 = "", kq = "";
            string c4B = "", c5B = "";
            string mes = "";
            if (dt == null) return "";
            string _record = "0";
            foreach (DataRow r in dt.Rows)
            {
                _record = r["SumRecord"].ToString();
                kq += $"<tr\">";                
                
                c1 = $"<td>{cellHasSpan(r["LETTER_TYPE"].ToString())}</td>";

              

                c2 = $"<td>"
                    + $"<span>{r["PERMNBR"].ToString().ToUpper()}</span></td>";



                c3 = $"<td class=\"cssTdCallSign\"><b>{cellHasSpan(r["FLIGHTNBR"].ToString())}</b></td>";

                c4 = $"<td class=\"{tdColorClassChange("FROM_AIRP", r["CHANGEVALUE"].ToString())} {textColorClassChange("FROM_AIRP", r["CHANGEVALUE"].ToString())}\">"
                    + $"{cellHasSpan(r["FROM_AIRP"])}</td>";

                
                c4B = (!string.IsNullOrEmpty(r["ETD"].ToString()) ? $"<td>{cellHasSpan("'" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETD"])}</td>" : "");

                c5 = $"<td class=\"{tdColorClassChange("TO_AIRP", r["CHANGEVALUE"].ToString())}\">"
                    + $"{cellHasSpan(r["TO_AIRP"])}</td>";

                if (!string.IsNullOrEmpty(r["ETA"].ToString()))
                {
                    if (r["ETA"].ToString().IndexOf('+') > 0)
                        c5B = $"<td>{cellHasSpan("'" + DateTime.Parse(r["FLIGHTDATE"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", ""))}</td>";
                    else
                        c5B = $"<td>{cellHasSpan("'"+ DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETA"])}</td>";
                }
                else
                {
                    c5B = $"<td></td>";
                }

                if (!string.IsNullOrEmpty(r["EOBTDATE"].ToString()))
                {
                    c6 = $"<td class=\"{tdColorClassChange("EOBT", r["CHANGEVALUE"].ToString())} {(!CompareHour(r["ETD"], r["EOBT"]) ? ColorHelper.GetColorCode(clsColor.cssTextBaySom) : "")}\">{cellHasSpan("'" + r["EOBTDATE"].ToString())}</td>";
                }
                else
                    c6 = $"<td></td>";            


                
                if (!string.IsNullOrEmpty(r["ATD"].ToString()))
                {
                    c7 = $"<td>{cellHasSpan("'" + r["ATD"])}</td>";
                }
                else
                    c7 = $"<td></td>";

                                
                if (!string.IsNullOrEmpty(r["ATA"].ToString()))
                {
                    c8 = $"<td>{cellHasSpan("'" + r["ATA"])}</td>";
                }
                else
                    c8 = $"<td></td>";



                c9 = $"<td>{cellHasSpan(r["CRAFT_T"].ToString())}</td>";

                c10 = $"<td class=\"{tdColorClassChange("CRAFT_TYPE", r["CHANGEVALUE"].ToString())}\">"
                    + $"{cellHasSpan(r["CRAFT_TYPE"].ToString())}</td>";

                c11 = $"<td>{cellHasSpan(r["REGISTRATION"].ToString())}</td>";

                c12 = $"<td>{cellHasSpan(r["PURPOSE"].ToString())}</td>";

                c13 = $"<td>{cellHasSpan(r["MTOW"].ToString())}</td>";

                string _remark = r["REMARK"].ToString();
                string _result = "";
                if (_remark.Length > 20)
                {
                    _result = _remark.Substring(0, 20) + "<br/><br/>" + _remark.Remove(0, 20);

                }
                else
                    _result = r["REMARK"].ToString();


                c14 = $"<td>{cellHasSpan(_result)}</td>";

                c15 = $"<td>{cellHasSpan(r["FLIGHT_TYPE"].ToString())}</td>";

                string _viagoc = r["VIAGOC"].ToString();
                string _rsviagoc = "";
                if (_viagoc.Length > 20)
                {
                    _rsviagoc = _viagoc.Substring(0, 20) + "<br/><br/>" + _viagoc.Remove(0, 20);

                }
                else
                    _rsviagoc = r["VIAGOC"].ToString();

                c16 = $"<td  class=\"{tdColorClassChange("VIAGOC", r["CHANGEVALUE"].ToString())}\">{cellHasSpan(_rsviagoc.ToString())}</td>";


                string _route = r["ROUTE_TT"].ToString();
                string _rsroute = "";
                if (_route.Length > 20)
                {
                    _rsroute = _route.Substring(0, 20) + "<br/><br/>" + _route.Remove(0, 20);

                }
                else
                    _rsroute = r["ROUTE_TT"].ToString();


                c17 = $"<td>{cellHasSpan(_rsroute)}</td>";

                c18 = $"<td class=\"{tdColorClassChange("VIA", r["CHANGEVALUE"].ToString())}\">{r["VIA"]}</td>";

                /* c6, c10 chua bind */
                kq += c1 + c2 + c11 + c3 + c4 + c5 + c4B + c6 + c5B + c7 + c8 + c16 + c17 + c9 + c10 + c12 + c14 + c18;
                kq += "</tr>";
            }
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            return kq;
        }
        private string ContentRowGrdSource(
            DataTable dt)
        {
            int i = _i;

            string c0 = "", c01 = "", c1 = "", c2 = "", c3 = "", c4 = "", c5 = "", c6 = "", c7 = "", c8 = "", c9 = "", c10 = "", c11 = "", c12 = "", c13 = "", c14 = "", c15 = "", c16 = "", c17 = "", c18 = "", kq = "";
            string c4B = "", c5B = "";
            string mes = "";
            if (dt == null) return "";
            string _record = "0";
            string _color = "";
            foreach (DataRow r in dt.Rows)
            {
                _color = trColorClass(r["LETTER_TYPE"], r["ETD"], r["ATD"], r["FLIGHTNBR"], r["PERMNBR"], r["HASPERM"]) + " sInputDb";

                _record = r["SumRecord"].ToString();
                
                kq += $"<tr data-isUpdate='false' ondblclick=\"viewPopupInfoExtension(this)\""
                    + $" data-id=\"{r["ID"]}\" data-flight-id=\"{r["FLIGHT_ID"]}\" id=\"{r["FLIGHT_ID"]}\" data-timeM=\"{DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{i.ToString()}' "
                    + $"class=\"{trColorClass(r["LETTER_TYPE"], r["ETD"], r["ATD"], r["FLIGHTNBR"], r["PERMNBR"], r["HASPERM"])}\" data-CallSign=\"{r["FLIGHTNBR"]}\">";
                                         


                //c0 = $"<td style=\"width:48px;\" class=\"tdIconStatus\">"
                //      + "<div id='divStatusIcon' class='action-buttons wid_50px'>"
                //       + $"<a data-toggle=\"tooltip\" title=\"Edit info!\" onclick=\"viewPopupInfoExtensionInsert(this);\" data-id=\"{r["FLIGHT_ID"]}\"><i class=\"glyphicon glyphicon-check\"></i></a>"
                //       + $"<a data-toggle=\"tooltip\" title=\"Delete !\" onclick=\"btnDeleteBy_Onclick('{r["FLIGHT_ID"]}');\"><i class=\"ace-icon fa fa-trash-o bigger-130\"></i></a>"
                //       + $"<a data-toggle=\"tooltip\" title=\"Move date!\" onclick=\"moveDate('{r["FLIGHT_ID"]}');\"><i class=\"glyphicon glyphicon-arrow-right\"></i></a>"

                //   + "</div></td>";

                c0 = $"<td style=\"width:48px;\" class=\"tdIconStatus\">"
                      + "<div id='divStatusIcon' class='action-buttons wid_50px'>"
                       + $"<a data-toggle=\"tooltip\" title=\"Edit info!\" onclick=\"viewPopupInfoExtensionInsert(this);\" data-id=\"{r["FLIGHT_ID"]}\"><i class=\"glyphicon glyphicon-check\"></i></a>"
                       + $"<a data-toggle=\"tooltip\" title=\"Delete !\" onclick=\"btnDeleteBy_Onclick('{r["ID"]}');\"><i class=\"ace-icon fa fa-trash-o bigger-130\"></i></a>"
                       + $"<a data-toggle=\"tooltip\" title=\"Move date!\" onclick=\"moveDate('{r["FLIGHT_ID"]}');\"><i class=\"glyphicon glyphicon-arrow-right\"></i></a>"

                   + "</div></td>";




                c01 = $"<td><label id='txtSTT" + i + "' for='stt'>" + i.ToString() + "</label></td>";

                c1 = $"<td><input type = 'text' data-control ='_updateAll' class=\"" + _color + "\"  id='txtLETTER_TYPE" + i + "'  maxlength='3' style='width:40px!important;' data-oldValue='" + r["LETTER_TYPE"].ToString() + "' value='" + r["LETTER_TYPE"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";

                c2 = $"<td style=\"width:250px;\">"
                                 + $"<input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  id='txtPERMNBR" + i + "' data-oldValue='" + r["PERMNBR"].ToString() + "' maxlength='10' value='" + r["PERMNBR"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";



                if (r["MOVEFINISH"].ToString() == "1")
                {

                    c3 = $"<td class=\"cssTdCallSignAr\"><input type = 'text' data-control = '_updateAll' class='cssTdCallSignAr sInputDb'  id='txtFLIGHTNBR" + i + "' data-oldValue='" + r["FLIGHTNBR"].ToString() + "' style='width:80px!important;' value='" + r["FLIGHTNBR"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                }
                else
                {
                    c3 = $"<td class=\"cssTdCallSign\"><input type = 'text' data-control = '_updateAll' class='cssTdCallSign sInputDb'  id='txtFLIGHTNBR" + i + "' data-oldValue='" + r["FLIGHTNBR"].ToString() + "' style='width:80px!important;'  value='" + r["FLIGHTNBR"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                }

                c4 = $"<td class=\"{tdColorClassChange("FROM_AIRP", r["CHANGEVALUE"].ToString())} {textColorClassChange("FROM_AIRP", r["CHANGEVALUE"].ToString())}\">"
                    + $"<input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"   id='txtFROM_AIRP" + i + "' style='width: 55px!important;' data-oldValue='" + r["FROM_AIRP"].ToString() + "' value='" + r["FROM_AIRP"].ToString() + "' onfocusin='binAutocomplete(this,\"AERO\")' onblur='checkIsUpdate(this)'/></td>";

                if (string.Equals(
                    r["CODE"].ToString(),
                    "CS",
                    StringComparison.OrdinalIgnoreCase))
                {
                        if (!string.IsNullOrEmpty(r["ETD"].ToString()))
                        {
                            if (r["ETD"].ToString().ToUpper() != "OPEN")
                            {                               
                                c4B = (!string.IsNullOrEmpty(r["ETD"].ToString()) ? $"<td><input type = 'text' data-control = '_updateAll' style='width: 55px!important;'  class=\"" + _color + "\"  data-minlenght='4' maxlength='6' id='txtETD" + i + "' value='" + DateTime.Parse(r["DATE_OLD"].ToString()).ToString("dd") + r["ETD"] + "' data-oldValue='" + DateTime.Parse(r["DATE_OLD"].ToString()).ToString("dd") + r["ETD"] + "' onblur='checkIsUpdate(this)'/></td>" : "");


                            }
                            else
                            {
                                c4B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETD" + i + "' data-oldValue='" + r["ETD"].ToString() + "' value='" + r["ETD"] + "' onblur='checkIsUpdate(this)'/></td>";
                            }
                        }
                        else
                        {
                            c4B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETD" + i + "' value='' onblur='checkIsUpdate(this)'/></td>";
                        }

                        if (!string.IsNullOrEmpty(r["ETA"].ToString()))
                        {
                            if (r["ETA"].ToString().ToUpper() != "OPEN")
                            {
                                if (r["ETA"].ToString().IndexOf('+') > 0)
                                   
                                    c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + DateTime.Parse(r["DATE_OLD"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", "") + "' value='" + DateTime.Parse(r["DATE_OLD"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", "") + "' onblur='checkIsUpdate(this)'/></td>";
                                else
                                    c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + DateTime.Parse(r["DATE_OLD"].ToString()).ToString("dd") + r["ETA"].ToString() + "' value='" + DateTime.Parse(r["DATE_OLD"].ToString()).ToString("dd") + r["ETA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                            }
                            else
                            {
                                c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + r["ETA"].ToString() + "'  value='" + r["ETA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                            }
                        }
                        else
                        {
                            c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' value='' onblur='checkIsUpdate(this)'/></td>";
                        }
                }
                else
                {

                    if (!string.IsNullOrEmpty(r["ETD"].ToString()))
                    {
                        if (r["ETD"].ToString().ToUpper() != "OPEN")
                        {
                            //c4B = (!string.IsNullOrEmpty(r["ETD"].ToString()) ? $"<td>{cellHasSpan(DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETD"])}</td>" : "");
                            c4B = (!string.IsNullOrEmpty(r["ETD"].ToString()) ? $"<td><input type = 'text' data-control = '_updateAll' style='width: 55px!important;'  class=\"" + _color + "\"  data-minlenght='4' maxlength='6' id='txtETD" + i + "' value='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETD"] + "' data-oldValue='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETD"] + "' onblur='checkIsUpdate(this)'/></td>" : "");


                        }
                        else
                        {
                            c4B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETD" + i + "' data-oldValue='" + r["ETD"].ToString() + "' value='" + r["ETD"] + "' onblur='checkIsUpdate(this)'/></td>";
                        }
                    }
                    else
                    {
                        c4B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETD" + i + "' value='' onblur='checkIsUpdate(this)'/></td>";
                    }

                    if (!string.IsNullOrEmpty(r["ETA"].ToString()))
                    {
                        if (r["ETA"].ToString().ToUpper() != "OPEN")
                        {
                            if (r["ETA"].ToString().IndexOf('+') > 0)
                                //c5B = $"<td>{cellHasSpan(DateTime.Parse(r["FLIGHTDATE"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", ""))}</td>";
                                c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", "") + "' value='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", "") + "' onblur='checkIsUpdate(this)'/></td>";
                            else
                                c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETA"].ToString() + "' value='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                        }
                        else
                        {
                            c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + r["ETA"].ToString() + "'  value='" + r["ETA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                        }
                    }
                    else
                    {
                        c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' value='' onblur='checkIsUpdate(this)'/></td>";
                    }

                }


                c5 = $"<td class=\"{tdColorClassChange("TO_AIRP", r["CHANGEVALUE"].ToString())}\">"
                    + $"<input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  style='width: 55px!important;' id='txtTO_AIRP" + i + "' onfocusin='binAutocomplete(this,\"AERO\")' data-oldValue='" + r["TO_AIRP"].ToString() + "' value='" + r["TO_AIRP"] + "' onblur='checkIsUpdate(this)'/></td>";



                //c6 = $"<td class=\"{tdColorClassChange("EOBT", r["CHANGEVALUE"].ToString())} {(!CompareHour(r["ETD"], r["EOBT"]) ? ColorHelper.GetColorCode(clsColor.cssTextBaySom) : "")}\"><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtEOBT" + i + "' data-oldValue='" + r["EOBTDATE"].ToString() + "' value='" + r["EOBTDATE"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";

                c6 = $"<td class=\"{CompareHourString(r["ETD"], r["EOBT"]).ToString()}\"><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtEOBT" + i + "' data-oldValue='" + r["EOBTDATE"].ToString() + "' value='" + r["EOBTDATE"].ToString() + "' onblur='checkIsUpdate(this)'/></td>"; 


                if (!string.IsNullOrEmpty(r["ATD"].ToString()))
                {
                    c7 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtATD" + i + "' data-oldValue='" + r["ATD"].ToString() + "' value='" + r["ATD"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                }
                else
                    c7 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtATD" + i + "' data-oldValue='" + r["ATD"].ToString() + "' value='' onblur='checkIsUpdate(this)'/></td>";


                if (!string.IsNullOrEmpty(r["ATA"].ToString()))
                {
                    c8 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtATA" + i + "' data-oldValue='" + r["ATA"].ToString() + "' value='" + r["ATA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                }
                else
                    c8 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtATA" + i + "' data-oldValue='" + r["ATA"].ToString() + "' value='' onblur='checkIsUpdate(this)'/></td>";



                c9 = $"<td>{cellHasSpan(r["CRAFT_T"].ToString())}</td>";

                c10 = $"<td class=\"{tdColorClassChange("CRAFT_TYPE", r["CHANGEVALUE"].ToString())}\">"
                    + $"<input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  style='width: 55px!important;' id='txtCRAFT_TYPE" + i + "' onfocusin='binAutocomplete(this,\"CRAFT\")' data-oldValue='" + r["CRAFT_TYPE"].ToString() + "' value='" + r["CRAFT_TYPE"] + "' onblur='checkIsUpdate(this)'/></td>";

                c11 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  style='width: 65px!important;' id='txtREGISTRATION" + i + "' value='" + r["REGISTRATION"] + "' data-oldValue='" + r["REGISTRATION"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";

                c12 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  style='width: 45px!important;' id='txtPURPOSE" + i + "' onfocusin='binAutocomplete(this,\"PURPOSE\")' data-oldValue='" + r["PURPOSE"].ToString() + "'  value='" + r["PURPOSE"] + "' onblur='checkIsUpdate(this)'/></td>";

                c13 = $"<td>{cellHasSpan(r["MTOW"].ToString())}</td>";


                c14 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" id='txtREMARK" + i + "' data-oldValue='" + r["REMARK"].ToString() + "' value='" + r["REMARK"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";

                c15 = $"<td>{cellHasSpan(r["FLIGHT_TYPE"].ToString())}</td>";

                string _viagoc = r["VIAGOC"].ToString();
                string _rsviagoc = "";

                _rsviagoc = SplitRouteNoCheck(_viagoc);
                c16 = $"<td  class=\"{tdColorClassChange("VIAGOC", r["CHANGEVALUE"].ToString())}\"><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" id='txtVIAGOC" + i + "' data-oldValue='" + r["VIAGOC"].ToString() + "' value='" + _viagoc.ToString().Trim() + "' onblur='checkIsUpdate(this)'/></td>";


                string _route = r["ROUTE_TT"].ToString();
                string _rsroute = "";

                _rsroute = SplitRoute(_viagoc, _route);

                c17 = $"<td>{cellHasSpan(_rsroute.ToString())}</td>";

                c18 = $"<td class=\"{tdColorClassChange("VIA", r["CHANGEVALUE"].ToString())}\">{r["VIA"]}</td>";

                /* c6, c10 chua bind */
                kq += c0 + c01 + c1 + c2 + c11 + c3 + c4 + c5 + c4B + c6 + c5B + c7 + c8 + c16 + c17 + c9 + c10 + c12 + c14 + c18;
                kq += "</tr>";
                i++;
                _i = i;
            }
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            return kq + "&&&&" + _record;


        }
        protected string SplitRouteNoCheck(string _route)
        {
            string _return = "";
            string[] _tmp;
            int j = 0;
            _tmp = _route.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i <= _tmp.Length - 1; i++)
            {
                if (j <= 2)
                {
                    if (i == _tmp.Length - 1)
                    {
                        _return += _tmp[i].ToString();
                    }
                    else
                        _return += _tmp[i].ToString() + "/";
                    j += 1;
                }
                else
                {
                    _return = _return + "<br><br>";
                    if (i == _tmp.Length - 1)
                    {
                        _return += _tmp[i].ToString();
                    }
                    else
                        _return += _tmp[i].ToString() + "/";

                    j = 0;
                }
            }
            return _return;
        }

        protected string SplitRoute(string _route, string _routeFPL)
        {
            string _return = "";
            string[] _tmp;
            int j = 0;
            string[] _tmproute;
            _tmproute = _route.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            _tmp = _routeFPL.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i <= _tmp.Length - 1; i++)
            {
                if (j <= 2)
                {
                    if (i == _tmp.Length - 1)
                    {
                        if (Array.IndexOf(_tmproute,_tmp[i].ToString()) >= 0)
                            _return += _tmp[i].ToString();
                        else
                            _return += "<b style='color:red'>" + _tmp[i].ToString() + "</b>";
                    }
                    else
                             if (Array.IndexOf(_tmproute, _tmp[i].ToString()) >= 0)
                                  _return += _tmp[i].ToString() + "/";
                            else
                                _return += "<b style=\"color:red\">" + _tmp[i].ToString() + "/</b>";
                    j += 1;
                }
                else
                {
                    _return = _return + "<br><br>";
                    if (i == _tmp.Length - 1)
                    {
                        if (Array.IndexOf(_tmproute, _tmp[i].ToString()) >= 0)
                            _return += _tmp[i].ToString();
                        else
                            _return += "<b style='color:red'>" + _tmp[i].ToString() + "</b>";
                    }
                    else
                            if (Array.IndexOf(_tmproute, _tmp[i].ToString()) >= 0)
                        _return += _tmp[i].ToString() + "/";
                    else
                        _return += "<b style=\"color:red\">" + _tmp[i].ToString() + "/</b>";

                    j = 0;
                }
            }
            return _return;
        }
        private string ContentRowGrdSource_First(DataTable dt)
        {

            int i = _i;

            string c0 = "", c01 = "", c1 = "", c2 = "", c3 = "", c4 = "", c5 = "", c6 = "", c7 = "", c8 = "", c9 = "", c10 = "", c11 = "", c12 = "", c13 = "", c14 = "", c15 = "", c16 = "", c17 = "", c18 = "", kq = "";
            string c4B = "", c5B = "";
            string mes = "";
            if (dt == null) return "";
            string _record = "0";
            string _color = "";
            foreach (DataRow r in dt.Rows)
            {
                _color = trColorClass(r["LETTER_TYPE"], r["ETD"], r["ATD"], r["FLIGHTNBR"], r["PERMNBR"], r["HASPERM"]) + " sInputDb";

                _record = r["SumRecord"].ToString();
                kq += $"<tr data-isUpdate='false' ondblclick=\"viewPopupInfoExtension(this)\""
                    + $" data-id=\"{r["ID"]}\" data-flight-id=\"{r["FLIGHT_ID"]}\" id=\"{r["FLIGHT_ID"]}\" data-timeM=\"{DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd-MM-yyyy hh:mm:ss")}\" data-RowNumber='{i.ToString()}' "
                    + $"class=\"{trColorClass(r["LETTER_TYPE"], r["ETD"], r["ATD"], r["FLIGHTNBR"], r["PERMNBR"], r["HASPERM"])}\" data-CallSign=\"{r["FLIGHTNBR"]}\">";


                //c0 = $"<td style=\"width:48px;\" class=\"tdIconStatus\">"
                //      + "<div id='divStatusIcon' class='action-buttons wid_50px'>"
                //       + $"<a data-toggle=\"tooltip\" title=\"Save info!\" onclick=\"viewPopupInfoExtensionInsert(this);\" data-id=\"{r["FLIGHT_ID"]}\"><i class=\"glyphicon glyphicon-save\"></i></a>"
                //       + $"<a data-toggle=\"tooltip\" title=\"Delete !\" onclick=\"btnDeleteBy_Onclick('{r["FLIGHT_ID"]}');\"><i class=\"ace-icon fa fa-trash-o bigger-130\"></i></a>"
                //       + $"<a data-toggle=\"tooltip\" title=\"Move date!\" onclick=\"moveDate('{r["FLIGHT_ID"]}');\"><i class=\"glyphicon glyphicon-arrow-right\"></i></a>"

                //   + "</div></td>";


                c0 = $"<td style=\"width:48px;\" class=\"tdIconStatus\">"
                      + "<div id='divStatusIcon' class='action-buttons wid_50px'>"
                       + $"<a data-toggle=\"tooltip\" title=\"Edit info!\" onclick=\"viewPopupInfoExtensionInsert(this);\" data-id=\"{r["FLIGHT_ID"]}\"><i class=\"glyphicon glyphicon-check\"></i></a>"
                       + $"<a data-toggle=\"tooltip\" title=\"Delete !\" onclick=\"btnDeleteBy_Onclick('{r["FLIGHT_ID"]}');\"><i class=\"ace-icon fa fa-trash-o bigger-130\"></i></a>"
                       + $"<a data-toggle=\"tooltip\" title=\"Move date!\" onclick=\"moveDate('{r["FLIGHT_ID"]}');\"><i class=\"glyphicon glyphicon-arrow-right\"></i></a>"

                   + "</div></td>";



                c01 = $"<td><label id='txtSTT" + i + "' for='stt'>"+ i.ToString() + "</label></td>";

                c1 = $"<td><input type = 'text' data-control ='_updateAll' class=\"" + _color + "\"  id='txtLETTER_TYPE" + i + "'  maxlength='3' style='width:40px!important;' data-oldValue='" + r["LETTER_TYPE"].ToString() + "' value='" + r["LETTER_TYPE"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";

                c2 = $"<td style=\"width:250px;\">"
                                 + $"<input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  id='txtPERMNBR" + i + "' data-oldValue='" + r["PERMNBR"].ToString() + "' maxlength='10' value='" + r["PERMNBR"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";



                if (r["MOVEFINISH"].ToString() == "1")
                {
                    
                    c3 = $"<td class=\"cssTdCallSignAr\"><input type = 'text' data-control = '_updateAll' class='cssTdCallSignAr sInputDb'  id='txtFLIGHTNBR" + i + "' data-oldValue='" + r["FLIGHTNBR"].ToString() + "' style='width:80px!important;' value='" + r["FLIGHTNBR"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                }
                else
                {                    
                    c3 = $"<td class=\"cssTdCallSign\"><input type = 'text' data-control = '_updateAll' class='cssTdCallSign sInputDb'  id='txtFLIGHTNBR" + i + "' data-oldValue='" + r["FLIGHTNBR"].ToString() + "' style='width:80px!important;'  value='" + r["FLIGHTNBR"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                }

                c4 = $"<td class=\"{tdColorClassChange("FROM_AIRP", r["CHANGEVALUE"].ToString())} {textColorClassChange("FROM_AIRP", r["CHANGEVALUE"].ToString())}\">"
                    + $"<input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"   id='txtFROM_AIRP" + i + "' style='width: 55px!important;' data-oldValue='" + r["FROM_AIRP"].ToString() + "' value='" + r["FROM_AIRP"].ToString() + "' onfocusin='binAutocomplete(this,\"AERO\")' onblur='checkIsUpdate(this)'/></td>";



                if (string.Equals(
                    r["CODE"].ToString(),
                    "CS",
                    StringComparison.OrdinalIgnoreCase))
                {
                        if (!string.IsNullOrEmpty(r["ETD"].ToString()))
                        {
                            if (r["ETD"].ToString().ToUpper() != "OPEN")
                            {
                                c4B = (!string.IsNullOrEmpty(r["ETD"].ToString()) ? $"<td><input type = 'text' data-control = '_updateAll' style='width: 55px!important;'  class=\"" + _color + "\"  data-minlenght='4' maxlength='6' id='txtETD" + i + "' value='" + DateTime.Parse(r["DATE_OLD"].ToString()).ToString("dd") + r["ETD"] + "' data-oldValue='" + DateTime.Parse(r["DATE_OLD"].ToString()).ToString("dd") + r["ETD"] + "' onblur='checkIsUpdate(this)'/></td>" : "");


                            }
                            else
                            {
                                c4B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETD" + i + "' data-oldValue='" + r["ETD"].ToString() + "' value='" + r["ETD"] + "' onblur='checkIsUpdate(this)'/></td>";
                            }
                        }
                        else
                        {
                            c4B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETD" + i + "' value='' onblur='checkIsUpdate(this)'/></td>";
                        }

                        if (!string.IsNullOrEmpty(r["ETA"].ToString()))
                        {
                            if (r["ETA"].ToString().ToUpper() != "OPEN")
                            {
                                if (r["ETA"].ToString().IndexOf('+') > 0)

                                    c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + DateTime.Parse(r["DATE_OLD"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", "") + "' value='" + DateTime.Parse(r["DATE_OLD"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", "") + "' onblur='checkIsUpdate(this)'/></td>";
                                else
                                    c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + DateTime.Parse(r["DATE_OLD"].ToString()).ToString("dd") + r["ETA"].ToString() + "' value='" + DateTime.Parse(r["DATE_OLD"].ToString()).ToString("dd") + r["ETA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                            }
                            else
                            {
                                c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + r["ETA"].ToString() + "'  value='" + r["ETA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                            }
                        }
                        else
                        {
                            c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' value='' onblur='checkIsUpdate(this)'/></td>";
                        }
                }
                else
                {

                    if (!string.IsNullOrEmpty(r["ETD"].ToString()))
                    {
                        if (r["ETD"].ToString().ToUpper() != "OPEN")
                        {
                            //c4B = (!string.IsNullOrEmpty(r["ETD"].ToString()) ? $"<td>{cellHasSpan(DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETD"])}</td>" : "");
                            c4B = (!string.IsNullOrEmpty(r["ETD"].ToString()) ? $"<td><input type = 'text' data-control = '_updateAll' style='width: 55px!important;'  class=\"" + _color + "\"  data-minlenght='4' maxlength='6' id='txtETD" + i + "' value='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETD"] + "' data-oldValue='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETD"] + "' onblur='checkIsUpdate(this)'/></td>" : "");


                        }
                        else
                        {
                            c4B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETD" + i + "' data-oldValue='" + r["ETD"].ToString() + "' value='" + r["ETD"] + "' onblur='checkIsUpdate(this)'/></td>";
                        }
                    }
                    else
                    {
                        c4B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETD" + i + "' value='' onblur='checkIsUpdate(this)'/></td>";
                    }

                    if (!string.IsNullOrEmpty(r["ETA"].ToString()))
                    {
                        if (r["ETA"].ToString().ToUpper() != "OPEN")
                        {
                            if (r["ETA"].ToString().IndexOf('+') > 0)
                                //c5B = $"<td>{cellHasSpan(DateTime.Parse(r["FLIGHTDATE"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", ""))}</td>";
                                c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", "") + "' value='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).AddDays(1).ToString("dd") + r["ETA"].ToString().Replace("+", "") + "' onblur='checkIsUpdate(this)'/></td>";
                            else
                                c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETA"].ToString() + "' value='" + DateTime.Parse(r["FLIGHTDATE"].ToString()).ToString("dd") + r["ETA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                        }
                        else
                        {
                            c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' data-oldValue='" + r["ETA"].ToString() + "'  value='" + r["ETA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                        }
                    }
                    else
                    {
                        c5B = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtETA" + i + "' value='' onblur='checkIsUpdate(this)'/></td>";
                    }

                }




                c5 = $"<td class=\"{tdColorClassChange("TO_AIRP", r["CHANGEVALUE"].ToString())}\">"
                    + $"<input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  style='width: 55px!important;' id='txtTO_AIRP" + i + "' onfocusin='binAutocomplete(this,\"AERO\")' data-oldValue='" + r["TO_AIRP"].ToString() + "' value='" + r["TO_AIRP"] + "' onblur='checkIsUpdate(this)'/></td>";

                

                c6 = $"<td class=\"{tdColorClassChange("EOBT", r["CHANGEVALUE"].ToString())} {(!CompareHour(r["ETD"], r["EOBT"]) ? ColorHelper.GetColorCode(clsColor.cssTextBaySom) : "")}\"><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtEOBT" + i + "' data-oldValue='" + r["EOBTDATE"].ToString() + "' value='" + r["EOBTDATE"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";

                if (!string.IsNullOrEmpty(r["ATD"].ToString()))
                {
                    c7 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtATD" + i + "' data-oldValue='" + r["ATD"].ToString() + "' value='" + r["ATD"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                }
                else
                    c7 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtATD" + i + "' data-oldValue='" + r["ATD"].ToString() + "' value='' onblur='checkIsUpdate(this)'/></td>";


                if (!string.IsNullOrEmpty(r["ATA"].ToString()))
                {
                    c8 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtATA" + i + "' data-oldValue='" + r["ATA"].ToString() + "' value='" + r["ATA"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";
                }
                else
                    c8 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" style='width: 55px!important;' data-minlenght='4' maxlength='6' id='txtATA" + i + "' data-oldValue='" + r["ATA"].ToString() + "' value='' onblur='checkIsUpdate(this)'/></td>";



                c9 = $"<td>{cellHasSpan(r["CRAFT_T"].ToString())}</td>";

                c10 = $"<td class=\"{tdColorClassChange("CRAFT_TYPE", r["CHANGEVALUE"].ToString())}\">"
                    + $"<input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  style='width: 55px!important;' id='txtCRAFT_TYPE" + i + "' onfocusin='binAutocomplete(this,\"CRAFT\")' data-oldValue='" + r["CRAFT_TYPE"].ToString() + "' value='" + r["CRAFT_TYPE"] + "' onblur='checkIsUpdate(this)'/></td>";

                c11 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  style='width: 65px!important;' id='txtREGISTRATION" + i + "' value='" + r["REGISTRATION"] + "' data-oldValue='" + r["REGISTRATION"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";

                c12 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\"  style='width: 45px!important;' id='txtPURPOSE" + i + "' onfocusin='binAutocomplete(this,\"PURPOSE\")' data-oldValue='" + r["PURPOSE"].ToString() + "'  value='" + r["PURPOSE"] + "' onblur='checkIsUpdate(this)'/></td>";

                c13 = $"<td>{cellHasSpan(r["MTOW"].ToString())}</td>";

                
                c14 = $"<td><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" id='txtREMARK" + i + "' data-oldValue='" + r["REMARK"].ToString() + "' value='" + r["REMARK"].ToString() + "' onblur='checkIsUpdate(this)'/></td>";

                c15 = $"<td>{cellHasSpan(r["FLIGHT_TYPE"].ToString())}</td>";

                string _viagoc = r["VIAGOC"].ToString();
                string _rsviagoc = "";

                _rsviagoc = SplitRouteNoCheck(_viagoc);
                 c16 = $"<td  class=\"{tdColorClassChange("VIAGOC", r["CHANGEVALUE"].ToString())}\"><input type = 'text' data-control = '_updateAll' class=\"" + _color + "\" id='txtVIAGOC" + i + "' data-oldValue='" + r["VIAGOC"].ToString() + "' value='" + _viagoc.ToString().Trim() + "' onblur='checkIsUpdate(this)'/></td>";
                

                string _route = r["ROUTE_TT"].ToString();
                string _rsroute = "";

                _rsroute = SplitRoute(_viagoc, _route);

                c17 = $"<td>{cellHasSpan(_rsroute.ToString())}</td>";

                c18 = $"<td class=\"{tdColorClassChange("VIA", r["CHANGEVALUE"].ToString())}\">{r["VIA"]}</td>";

                /* c6, c10 chua bind */
                kq += c0 + c01 + c1 + c2 + c11 + c3 + c4 + c5 + c4B + c6 + c5B + c7 + c8 + c16 + c17 + c9 + c10 + c12 + c14 + c18;
                kq += "</tr>";
                i++;
                _i = i;
            }
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "</br>");
            return kq;

            
        }
        protected string grdSourceLoadFirt()
        {          
            return RenderGrdSource_First(new DayFlightsDAL().GetTableWithValueSearch(new clsDaylyFlightSearch() { KHUNGGIO1 = "0000", KHUNGGIO2 = "2359", RowStart = 1, RowFinish = 50,PERMTYPE = "LD", FLIGHTDATE= DateTime.Today}));
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thamso"></param>
        /// <returns></returns>
        private string viewPopupInfoExtension(string[] thamso)
        {
            string kq = "";
            string sourceRowId = thamso[0];
            string flightId = thamso.Length > 3
                ? thamso[3]
                : sourceRowId;
            DataTable dt = new DayFlightTotalInfoDAL().GetInfoChangeById(sourceRowId);
            if (dt == null || dt.Rows.Count == 0) return "";
            kq += FlightInfoExtension_IsChange(dt.Rows[0]["CHANGEVALUE"].ToString());
            kq += FlightInfoExtension_IsTimeValid(dt.Rows[0]["HASTIMEVALID"].ToString() == "1" ? "YES" : "NO");
            kq += FlightInfoExtension_ChangeInfo(dt.Rows[0]["CHANGEVALUE"].ToString());
            kq += FlightInfoExtension_HasPerm(dt.Rows[0]["HASPERM"].ToString() == "1" ? "YES" : "NO");
            kq += FlightInfoExtension_Permission(sourceRowId);
            kq += FlightInfoExtension_ActionHistory(flightId);
            kq += FlightInfoExtension_DienVanExt(sourceRowId);
            // view button access checked info change
            kq += FlightInfoExtension_ShowButtonAccess(sourceRowId);
            kq += $"<script>$('#btnUpdateStatusLetter').attr('onclick', 'btnUpdateStatusLetter_OnClick(\\'{sourceRowId}\\')');</script>";
            kq = System.Text.RegularExpressions.Regex.Replace(kq, @"\r\n?|\n", "<br>");
            return kq;
        }

        private string FlightInfoExtension_Permission(string flightId)
        {
            DataTable permission = null;
            DataTable linkFiles = null;
            string errorMessage = "";

            try
            {
                var dal = new DayFlightsDAL();
                permission = dal.GetPermissionByFlightId(flightId);

                try
                {
                    linkFiles = dal.GetPermissionLinkFiles(flightId, "SC");
                }
                catch
                {
                    // File đính kèm không được làm hỏng phần thông tin Permission.
                    linkFiles = null;
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Không tải được thông tin Permission: " + ex.Message;
            }

            string permissionJson = Newtonsoft.Json.JsonConvert.SerializeObject(
                permission ?? new DataTable());
            string linkFilesJson = Newtonsoft.Json.JsonConvert.SerializeObject(
                linkFiles ?? new DataTable());
            string errorJson = Newtonsoft.Json.JsonConvert.SerializeObject(errorMessage);

            // Ngăn dữ liệu có chuỗi </script> kết thúc thẻ script được trả về callback.
            permissionJson = permissionJson.Replace("</", "<\\/");
            linkFilesJson = linkFilesJson.Replace("</", "<\\/");
            errorJson = errorJson.Replace("</", "<\\/");

            return "<script>renderFlightPermission(" +
                permissionJson + "," +
                linkFilesJson + "," +
                errorJson + ");</script>";
        }

        private string FlightInfoExtension_ActionHistory(string flightId)
        {
            string rowsHtml;

            try
            {
                DataTable history = new DayFlightsDAL()
                    .GetActionHistoryByFlightId(flightId);

                if (history == null || history.Rows.Count == 0)
                {
                    rowsHtml =
                        "<tr><td colspan='5' class='text-muted'>" +
                        "Chưa có lịch sử thao tác cho chuyến bay này." +
                        "</td></tr>";
                }
                else
                {
                    var html = new System.Text.StringBuilder();
                    foreach (DataRow row in history.Rows)
                    {
                        string actionType = row["ACTION_TYPE"].ToString();
                        string actionName = string.Equals(
                            actionType,
                            "INSERT",
                            StringComparison.OrdinalIgnoreCase)
                                ? "Thêm mới"
                                : "Cập nhật";

                        html.Append("<tr>")
                            .Append("<td>")
                            .Append(HttpUtility.HtmlEncode(actionName))
                            .Append("</td><td>")
                            .Append(HttpUtility.HtmlEncode(row["CALLSIGN"].ToString()))
                            .Append("</td><td style='white-space:nowrap;'>")
                            .Append(HttpUtility.HtmlEncode(row["FLIGHTDATE"].ToString()))
                            .Append("</td><td>")
                            .Append(HttpUtility.HtmlEncode(row["ACTION_USER"].ToString()))
                            .Append("</td><td style='white-space:nowrap;'>")
                            .Append(HttpUtility.HtmlEncode(row["ACTION_DATE"].ToString()))
                            .Append("</td></tr>");
                    }
                    rowsHtml = html.ToString();
                }
            }
            catch (Exception ex)
            {
                rowsHtml =
                    "<tr><td colspan='5' class='text-danger'>" +
                    HttpUtility.HtmlEncode(
                        "Không tải được lịch sử thao tác: " + ex.Message) +
                    "</td></tr>";
            }

            string rowsJson = Newtonsoft.Json.JsonConvert.SerializeObject(rowsHtml)
                .Replace("</", "<\\/");
            return "<script>renderFlightActionHistory(" + rowsJson + ");</script>";
        }

        private string viewPopupInfoExtensionInsert(string[] thamso)
        {
            string kq = "";
           
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
                            if (p[0].ToUpper().Contains("Bay sớm") )
                            {
                                ct += $"Thay đổi đường bay.</br>";
                            }
                            if (p[0].ToUpper().Contains("ROUTE") )
                            {
                                ct += $"Thay đổi đường bay.</br>";
                            }
                            if (p[0].ToUpper().Contains("FROM_AIRP") || p[0].ToUpper().Contains("TO_AIRP") )
                            {
                                ct += $"Thay đổi sân bay cất hạ cánh.</br>";
                            }                            
                            if (p[0].ToUpper().Contains("CRAFT_ID") )
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
