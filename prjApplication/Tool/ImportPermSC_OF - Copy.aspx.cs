using Newtonsoft.Json.Converters;
using prjBusinessLogic;
using prjInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace prjApplication.Tool
{
    public partial class ImportPermSC_OF : System.Web.UI.Page
    {
        #region Static
        private List<RouteList> lisRoute { get { return new RouteListDAL().GetAllRouteList(); } }

        ActionHistoryDAL _AcDAL = new ActionHistoryDAL();
        #endregion
        private string NhanDangVia(string txt)
        {
            txt = txt.Replace("/", " / ").Replace("-", " - ").Replace(":", " : ");
            string[] phantu = txt.Split(new string[] { "\t", " ", "-", ",", "/" }, StringSplitOptions.None);
            List<string> d = phantu.Where(a => !lisRoute.Any(b => b.ROUTE_NAME == a)).ToList();
            foreach (var item in d)
            {
                if (item != "" && item != "-" && item != "/")
                    txt = txt.Replace(item, "");
            }
            return txt.Replace(":", "").Replace("\t", "").Trim(new char[] { '-', '/', ' ' });
        }
        private string GetViaByRow(string dong)
        {
            string kq = "";
            string[] pts = dong.Split(new string[] { " ", "\t", ",", "-", "/", "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pt in pts)
            {
                var ax = lisRoute.Any(a => a.ROUTE_NAME == pt.Trim());
                if (ax)
                    kq += pt.Trim() + "/";
            }
            return kq;
        }
        private string RutGonVia(string v)
        {
            string tu = "";
            for (int i = 0; i < v.Length - 2; i++)
            {
                tu = v[i].ToString() + v[i + 1].ToString();
                if (tu == "  ")
                {
                    v = v.Remove(i, 1);
                    v = RutGonVia(v);
                }
            }
            return v.Replace(" / ", "/").Replace(" - ", "-");
        }
        private string FilterVia(string txt)
        {
            string kq = " ";
            string[] phantu = txt.Split(new string[] { "\t", " ", "/" }, StringSplitOptions.None);
            var b = phantu.Distinct().ToList();
            foreach (var item in b)
            {
                if (item != "")
                    kq += item + "/";
            }
            return kq;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddl_Load();
                //Response.Write(clsChuanHoaImport.ChuanHoaDateTime2("23-MAR-18"));
            }
        }
        public void ddl_Load()
        {
            this.FillDropdownList(ddlAUTHOR, new FpAuthorDAL().GetAllObject(), "AUTHOR_CODE", "AUTHOR_CODE");
            this.FillDropdownList(ddlPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_CODE", "PURPOSE_CODE"); ;
        }
        protected void linkSave_Click(object sender, EventArgs e)
        {
            string[] TextObject = ResultText(txtContent.Value);
            if (TextObject == null) { this.AlertMessage("Not value!"); return; };
            if (String.IsNullOrEmpty(txtPERMNBR.Value)) { this.AlertMessage("PERMNBR Is Not Value!"); return; };
            if (string.IsNullOrEmpty(txtVersion.Value)) { this.AlertMessage("VERSION Is Not Value"); return; };
            switch (ddlLoaiImport.Value)
            {
                case "BSX":
                    this.AlertMessage("Success:" + BSX(TextObject).ToString());
                    break;
                case "LKH":
                    this.AlertMessage("Success:" + LKH(TextObject).ToString());
                    break;
                case "ETD":
                    this.AlertMessage("Success:" + ETD(TextObject).ToString());
                    break;
                case "AHK":
                    this.AlertMessage("Success:" + AHK(TextObject).ToString());
                    break;
                case "UTP":
                    this.AlertMessage("Success:" + UTP(TextObject).ToString());
                    break;
                case "CKK":
                    this.AlertMessage("Success:" + CKK(TextObject).ToString());
                    break;
                case "CCA":
                    this.AlertMessage("Success:" + CCA(TextObject).ToString());
                    break;
                case "RJA":
                    this.AlertMessage("Success:" + RJA(TextObject).ToString());
                    break;
                case "AIQ":
                    this.AlertMessage("Success:" + AIQ(TextObject).ToString());
                    break;
                case "SWM":
                    this.AlertMessage("Success:" + SWM(TextObject).ToString());
                    break;
                case "CSN":
                    this.AlertMessage("Success:" + CSN(TextObject).ToString());
                    break;
                case "CQH":
                    this.AlertMessage("Success:" + CQH(TextObject).ToString());
                    break;
                case "MXD":
                    this.AlertMessage("Success:" + MXD(TextObject).ToString());
                    break;
                case "CXA":
                    this.AlertMessage("Success:" + CXA(TextObject).ToString());
                    break;
                case "THY":
                    this.AlertMessage("Success:" + THY(TextObject).ToString());
                    break;
                case "QTR":
                    this.AlertMessage("Success:" + QTR(TextObject).ToString());
                    break;
                case "HBH":
                    this.AlertMessage("Success:" + HBH(TextObject).ToString());
                    break;
                case "MAS":
                    this.AlertMessage("Success:" + MAS(TextObject).ToString());
                    break;
                case "MAS66":
                    this.AlertMessage("Success:" + MAS66(TextObject).ToString());
                    break;
                case "CHH":
                    this.AlertMessage("Success:" + CHH(TextObject).ToString());
                    break;
                case "TGW":
                    this.AlertMessage("Success:" + TGW(TextObject).ToString());
                    break;
                case "ETH":
                    this.AlertMessage("Success:" + ETH(TextObject).ToString());
                    break;
                case "APG":
                    this.AlertMessage("Success:" + APG(TextObject).ToString());
                    break;
                case "XAX":
                    this.AlertMessage("Success:" + XAX(TextObject).ToString());
                    break;
                case "AXM":
                    this.AlertMessage("Success:" + AXM(TextObject).ToString());
                    break;
                case "DKH":
                    this.AlertMessage("Success:" + DKH(TextObject).ToString());
                    break;
                case "CSH":
                    this.AlertMessage("Success:" + CSH(TextObject).ToString());
                    break;
                case "CEB":
                    this.AlertMessage("Success:" + CEB(TextObject).ToString());
                    break;
                case "CSZ":
                    this.AlertMessage("Success:" + CSZ(TextObject).ToString());
                    break;
                case "JAI":
                    this.AlertMessage("Success:" + JAI(TextObject).ToString());
                    break;
                case "JCC":
                    this.AlertMessage("Success:" + JCC(TextObject).ToString());
                    break;
                case "AIC":
                    this.AlertMessage("Success:" + AIC(TextObject).ToString());
                    break;
                case "HXA":
                    this.AlertMessage("Success:" + HXA(TextObject).ToString());
                    break;
                case "CES":
                    this.AlertMessage("Success:" + CES(TextObject).ToString());
                    break;
                case "MKR":
                    this.AlertMessage("Success:" + MKR(TextObject).ToString());
                    break;
                case "RBA":
                    this.AlertMessage("Success:" + RBA(TextObject).ToString());
                    break;
                case "UAE":
                    this.AlertMessage("Success:" + UAE(TextObject).ToString());
                    break;
                case "CBJ":
                    this.AlertMessage("Success:" + CBJ(TextObject).ToString());
                    break;
                case "SVR":
                    this.AlertMessage("Success:" + SVR(TextObject).ToString());
                    break;
                case "LKE":
                    this.AlertMessage("Success:" + LKE(TextObject).ToString());
                    break;
                case "NCT":
                    this.AlertMessage("Success:" + NCT(TextObject).ToString());
                    break;
                case "GIA":
                    this.AlertMessage("Success:" + GIA(TextObject).ToString());
                    break;
                case "MSR":
                    this.AlertMessage("Success:" + MSR(TextObject).ToString());
                    break;
                case "CAL":
                    this.AlertMessage("Success:" + CAL(TextObject).ToString());
                    break;
                case "QFA":
                    this.AlertMessage("Success:" + QFA(TextObject).ToString());
                    break;
                case "BKP":
                    this.AlertMessage("Success:" + BKP(TextObject).ToString());
                    break;
                case "BAW":
                    this.AlertMessage("Success:" + BAW(TextObject).ToString());
                    break;
                case "EVA":
                    this.AlertMessage("Success:" + EVA(TextObject).ToString());
                    break;
                case "JNA":
                    this.AlertMessage("Success:" + JNA(TextObject).ToString());
                    break;
                case "ESR":
                    this.AlertMessage("Success:" + ESR(TextObject).ToString());
                    break;
                case "SIA":
                    this.AlertMessage("Success:" + SIA(TextObject).ToString());
                    break;
                case "ABL":
                    this.AlertMessage("Success:" + ABL(TextObject).ToString());
                    break;
                case "KAL":
                    this.AlertMessage("Success:" + KAL(TextObject).ToString());
                    break;
                case "TWB":
                    this.AlertMessage("Success:" + TWB(TextObject).ToString());
                    break;
                case "CDG":
                    this.AlertMessage("Success:" + CDG(TextObject).ToString());
                    break;
                case "AAR":
                    this.AlertMessage("Success:" + AAR(TextObject).ToString());
                    break;
                case "CLU":
                    this.AlertMessage("Success:" + CLU(TextObject).ToString());
                    break;
                case "ABW":
                    this.AlertMessage("Success:" + ABW(TextObject).ToString());
                    break;
                case "NOK":
                    this.AlertMessage("Success:" + NOK(TextObject).ToString());
                    break;
                case "NOK_B":
                    this.AlertMessage("Success:" + NOK_B(TextObject).ToString());
                    break;
                case "PAL":
                    this.AlertMessage("Success:" + PAL(TextObject).ToString());
                    break;
                case "OMA":
                    this.AlertMessage("Success:" + OMA(TextObject).ToString());
                    break;
                case "VGO":
                    this.AlertMessage("Success:" + VGO(TextObject).ToString());
                    break;
                case "HKE":
                    this.AlertMessage("Success:" + HKE(TextObject).ToString());
                    break;
                case "SLK":
                    this.AlertMessage("Success:" + SLK(TextObject).ToString());
                    break;
                case "THA":
                    this.AlertMessage("Success:" + THA(TextObject).ToString());
                    break;
                case "SAA":
                    this.AlertMessage("Success:" + SAA(TextObject).ToString());
                    break;
                case "JJA":
                    this.AlertMessage("Success:" + JJA(TextObject).ToString());
                    break;
                case "JAL":
                    this.AlertMessage("Success:" + JAL(TextObject).ToString());
                    break;
                case "SVA":
                    this.AlertMessage("Success:" + SVA(TextObject).ToString());
                    break;
                case "GEC":
                    this.AlertMessage("Success:" + GEC(TextObject).ToString());
                    break;
                case "BOX":
                    this.AlertMessage("Success:" + BOX(TextObject).ToString());
                    break;
                case "THD":
                    this.AlertMessage("Success:" + THD(TextObject).ToString());
                    break;
                case "ANA":
                    this.AlertMessage("Success:" + ANA(TextObject).ToString());
                    break;
                case "KHV":
                    this.AlertMessage("Success:" + KHV(TextObject).ToString());
                    break;
                case "CLX":
                    this.AlertMessage("Success:" + CLX(TextObject).ToString());
                    break;
                case "FDX":
                    this.AlertMessage("Success:" + FDX(TextObject).ToString());
                    break;
                case "CPA":
                    TextObject = ResultText(CPA_ChuanHoa(TextObject));
                    this.AlertMessage("Success:" + CPA(TextObject).ToString());
                    break;
                case "MAU":
                    this.AlertMessage("Success:" + MAU(TextObject).ToString());
                    break;
                case "UAL":
                    this.AlertMessage("Success:" + UAL(TextObject).ToString());
                    break;
                case "HDA":
                    TextObject = ResultText(HDA_ChuanHoa(TextObject));
                    this.AlertMessage("Success:" + HDA(TextObject).ToString());
                    break;
                case "ICV":
                    this.AlertMessage("Success:" + ICV(TextObject).ToString());
                    break;
                case "LAO":
                    this.AlertMessage("Success:" + LAO(TextObject).ToString());
                    break;
                case "CRK":
                    this.AlertMessage("Success:" + CRK(TextObject).ToString());
                    break;
                case "HKC":
                    this.AlertMessage("Success:" + HKC(TextObject).ToString());
                    break;
                case "VOZ":
                    this.AlertMessage("Success:" + VOZ(TextObject).ToString());
                    break;
                case "KME":
                    this.AlertMessage("Success:" + KME(TextObject).ToString());
                    break;
                case "TAX":
                    this.AlertMessage("Success:" + TAX(TextObject).ToString());
                    break;
                case "TLM":
                    this.AlertMessage("Success:" + TLM(TextObject).ToString());
                    break;
                case "UPS":
                    this.AlertMessage("Success:" + UPS(TextObject).ToString());
                    break;
                case "ALL_OPER":
                    this.AlertMessage("Success:" + ALL_OPER(TextObject).ToString());
                    break;
            }
        }
        public string Callsigntext(string _textinput, string _callsign)
        {
            string callSign = _callsign;
            string via = "";
            string[] value = ResultText(_textinput);
            switch (ddlLoaiImport.Value)
            {
                case "CDG":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Trim().ToUpper() == callSign.Trim().ToUpper()) via = d[1];
                    }
                    break;
                case "SLK":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
                        string ax = "";
                        for (int j = 1; j < d.Length; j++)
                        {
                            ax += d[j] + "/";
                        }
                        if (d[0].Trim().ToUpper() == callSign.Trim().ToUpper()) via = ax;
                    }
                    break;
            }
            return via;
        }
        public string Via4text(string _textinput, string _from, string _to)
        {
            string route = _from + '-' + _to;

            string route2 = _from + '/' + _to;
            string route3 = _from + ' ' + '–' + ' ' + _to;
            string route4 = _from + ' ' + '–' + _to;
            string route5 = _from + '–' + ' ' + _to;

            string via = "";

            string[] value = ResultText(_textinput);
            switch (ddlLoaiImport.Value)
            {
                case "UTP":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].ToString().Replace("  ", string.Empty) == route)
                            via = d[1].ToString();
                    }
                    break;
                case "BSX":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].ToString().Replace(" ", string.Empty).Substring(0, 9) == route)
                            via = d[1].ToString();
                        else if (d[0].ToString().Replace(" ", string.Empty).Substring(5, 9) == route)
                            via = d[1].ToString();
                    }
                    break;
                case "AHK":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Replace(" ", "") == route) via = d[1];
                    }
                    break;
                case "AIQ":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].ToString().Replace(" ", string.Empty) == route)
                            via = d[1].ToString();
                    }
                    break;
                case "SWM":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].ToString().Replace(" ", string.Empty) == route)
                            via = d[1].ToString();
                    }
                    break;
                case "CSN":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].ToString().Replace(" ", string.Empty).Substring(d[0].Length - 9, 9) == route)
                            via = d[1].ToString();
                    }
                    break;
                case "CXA":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].ToString().Replace(" ", string.Empty) == route)
                            via = d[1].ToString();
                    }
                    break;
                case "CHH":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[1].ToString().Replace(" ", string.Empty) == route)
                            via = d[2].ToString();
                    }
                    break;
                case "MKR":
                    for (int i = 0; i < value.Length; i++)
                    {

                        string route1 = "";
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        string via1 = d[0] + "-" + d[1];
                        for (int j = 2; j < d.Length; j++)
                        {
                            route1 += d[j] + " ";
                        }
                        if (via1 == route) via = route1;
                    }
                    break;
                case "QFA":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0] == route2) via = d[1];
                    }
                    break;
                case "BAW":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        string _ax = d[0].Replace("-", "");
                        if (_ax.Trim().ToUpper() == route2.Trim().ToUpper()) via = d[1];
                    }
                    break;
                case "JNA":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":", "V.V " }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Substring(1, 9).Trim().ToUpper() == route.Trim().ToUpper()) via = d[1].Replace(" V.V", "");
                    }
                    break;
                case "SIA":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Trim().ToUpper() == route.Trim().ToUpper()) via = d[1];
                        else if (d[0].Trim().ToUpper() == route3.Trim().ToUpper()) via = d[1];
                        else if (d[0].Trim().ToUpper() == route4.Trim().ToUpper()) via = d[1];
                        else if (d[0].Trim().ToUpper() == route5.Trim().ToUpper()) via = d[1];
                    }
                    break;
                case "ABL":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Substring(1, 9).Trim().ToUpper() == route2.Trim().ToUpper()) via = d[1];
                    }
                    break;
                case "CLU":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Trim().ToUpper() == route.Trim().ToUpper()) via = d[1];
                    }
                    break;
                case "ABW":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Trim().ToUpper() == route.Trim().ToUpper()) via = d[1];
                    }
                    break;
                case "PAL":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Trim().ToUpper() == route.Trim().ToUpper()) via = d[1];
                    }
                    break;
                case "OMA":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Trim().ToUpper() == route.Trim().ToUpper()) via = d[1];
                    }
                    break;
                case "JJA":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        string bx = "";
                        for (int j = 0; j < d.Length; j++)
                        {
                            bx += d[j] + " ";
                        }
                        if (d[0].Trim().ToUpper() == route2.Trim().ToUpper()) via = bx;
                    }
                    break;
                case "RBA":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Trim().ToUpper() == route) via = d[1];
                    }
                    break;
            }

            return via;
        }
        private string[] ResultText(string value)
        {
            if (string.IsNullOrEmpty(value.Trim())) return null;
            string[] _tmp = null;
            switch (ddlLoaiImport.Value)
            {
                case "MAS66":
                    string dong = MAS_ChuanHoaDinhDang(value);
                    _tmp = dong.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    break;
                default:
                    _tmp = value.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    break;
            }
            return _tmp;
        }
        private string MAS_ChuanHoaDinhDang(string txt)
        {
            //txt = txt.Replace("  ", " ");
            txt = txt.Replace("  ", " ");
            string[] dongs = txt.Split(new string[] { "\r\n\t", "\r\n", "\n", "\r", "\t", "  " }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            foreach (var dong in dongs)
            {
                if (dong.IndexOf("SECTOR") < 0 && dong != " ")
                {
                    kq += $"\r\n{dong}";
                }
            };
            string[] ds = kq.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string kqs = "";
            foreach (var dong in ds)
            {
                if (dong.Length > 10)
                {
                    string _pts = dong.Substring(0, 3);
                    try
                    {
                        if (_pts != "ETA")
                            kqs += $"\r\n{dong}";
                        else
                            kqs += $"{dong}";
                    }
                    catch
                    {
                        kqs += $"\t{dong}\t";
                    }
                }
            }
            //            string[] dss = kqs.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return kqs;
        }
        #region import extension
        string[,] dIn = new string[,] {
            {"LD_VN", "CALLSIGN_0", "FROMDATE_1", "TODATE_2", "DAILY_3", "CRAFT_4", "FROM_AIRP_5","TO_AIRP_6", "ETD_7","ETA_8","VIA_9", "REMARK_10", "Contains_Đường bay" },
            {"LD_2753NOV2017VN", "CALLSIGN_0", "FROMDATE_1", "TODATE_2", "DAILY_3", "CRAFT_4", "FROM_AIRP_5","TO_AIRP_6", "ETD_7","ETA_8","VIA_9", "REMARK_10", "Lenght_8" },
        };
        private int InsertPermSC(string[] value, string exten)
        {
            int kq = 0;
            string remark = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (exten.Contains("Contains"))
                {
                    if (value[i].Contains(exten.Substring(8)))
                    {
                        remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[2];
                    }
                    else if (exten.Contains("Lenght"))
                    {
                        if (value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).Length < Convert.ToInt32(exten.Substring(6)))
                        {
                            remark = value[i].Trim(new char[] { '\t', ' ' }).Substring(2);
                        }
                    }
                }
            }
            return kq;
        }
        private int InsertPermSC(string callSign, string fromDate, string toDate, string daily, string craft, string fromAirp, string toAirp, string etd, string eta, string via, string remark, string permType)
        {
            int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = callSign,
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(fromDate),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(toDate),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(daily),
                            P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(craft),
                            P_FROM_AIRP = fromAirp,
                            P_TO_AIRP = toAirp,
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(etd),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(eta),
                            P_VIA = via,
                            P_REMARK = remark,
                            P_PERMTYPE = permType,
                            P_PURPOSE = ddlPURPOSE.SelectedValue
                        }).ToString());
            return kq;
        }
        #endregion
        public bool IsDateTime(string _date)
        {
            bool _isTime = false;
            DateTime myDate;
            if (DateTime.TryParse(_date, out myDate))
            {
                _isTime = true;
            }
            return _isTime;
        }
        public int ALL_OPER(string[] value)
        {
            int iCount = 0;
            string _error = ""; string _callSign = ""; string _from = ""; string _to = ""; string _craft = "";
            string _daily = ""; string _etd = ""; string _fromDate = ""; string _toDate = ""; string _route = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    if (d.Length == 9)
                    {
                        _fromDate = d[1]; _toDate = d[2]; _daily = clsChuanHoaImport.ChuanHoaDayly(d[3]);
                        _craft = d[4]; _from = d[5]; _etd = d[6]; _to = d[7]; _route = txtRoutes.Value;
                    }
                    else if(d.Length == 10)
                    {
                        _fromDate = d[1]; _toDate = d[2]; _daily = clsChuanHoaImport.ChuanHoaDayly(d[3]);
                        _craft = d[4]; _from = d[5]; _etd = d[6]; _to = d[7]; _route = d[9];
                    }
                    else
                    {
                        _fromDate = d[1]; _toDate = d[2]; _daily = clsChuanHoaImport.ChuanHoaDayly(d[3]);
                        _craft = txtCraft.Value; _from = d[4]; _etd = d[5]; _to = d[6]; _route = txtRoutes.Value;
                    }
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT_ALL_OPER"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_fromDate),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_toDate),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                            P_CRAFT = clsChuanHoaImport.CraftType(_craft.ToUpper()),
                            P_FROM_AIRP = _from,
                            P_TO_AIRP = _to,
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_etd),
                            P_ETA = "",
                            P_VIA = _route,
                            P_REMARK = txtRemark.Value,
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            //P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int MSR(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            if (SctExportType.Value == "2")
            {
                string[] ax = value[1].Split(new string[] { " ", ",", "-" }, StringSplitOptions.RemoveEmptyEntries);
                string _from = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[1].Replace("TH", "") + ax[2] + ax[3]);
                string _to = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[4].Replace("TH", "") + ax[5] + ax[6]);
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].Contains("MS"))
                    {
                        try
                        {
                            string[] d = value[i].Split(new string[] { " ", "/", "(", ")", "-" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0];
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(d[2]),
                                    P_FROM_AIRP = d[6].Substring(0, 4),
                                    P_TO_AIRP = d[8],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(4, 5)),
                                    P_ETA = "",
                                    P_VIA = txtRoutes.Value,
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(d[2]),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                        catch (Exception)
                        {
                            _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                            lblLOG.Text = _error;
                        }

                    }
                }
            }
            else
            {
                string[] ax = value[0].Split(new string[] { " ", ",", "-" }, StringSplitOptions.RemoveEmptyEntries);
                string _from = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[1].Replace("TH", "") + ax[2] + ax[3]);
                string _to = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[4].Replace("TH", "") + ax[5] + ax[6]);
                for (int i = 0; i < value.Length; i++)
                {
                    if (value[i].Contains("MSR"))
                    {
                        try
                        {
                            string[] d = value[i].Split(new string[] { " ", "/", "(", ")", "-" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0];
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(d[2]),
                                    P_FROM_AIRP = d[6].Substring(0, 4),
                                    P_TO_AIRP = d[8],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(4, 5)),
                                    P_ETA = "",
                                    P_VIA = txtRoutes.Value,
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(d[2]),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                        catch (Exception)
                        {
                            _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                            lblLOG.Text = _error;
                        }

                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int QTR(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (SctExportType.Value == "2")
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4] + ax[5]);
            }
            else
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[2] + ax[4].Substring(ax[4].Length - 4, 4));
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4]);
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("QTR"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "VV" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = clsChuanHoaImport.CraftType(d[6].ToUpper()),
                                P_FROM_AIRP = d[2],
                                P_TO_AIRP = d[5],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = txtRemark.Value,
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int UTP(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] bx = value[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[3]);
            string daily = clsChuanHoaImport.ChuanHoaDayly(bx[2]);

            for (int i = 2; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                    , new
                    {
                        P_CALLSIGN = d[0],
                        P_FROMDATE = _from,
                        P_TODATE = _to,
                        P_DAILY = daily,
                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                        P_FROM_AIRP = d[1],
                        P_TO_AIRP = d[4],
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                        P_ETA = "",
                        P_VIA = "",
                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                        P_PERMTYPE = "O/F",
                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                        P_PERMNBR = txtPERMNBR.Value,
                        P_SEASON = ddlSeason.Value,
                        P_OPER = ddlLoaiImport.Value,
                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                        P_VERSION = txtVersion.Value.ToUpper(),
                        P_REGISTRATION = txtReg.Value
                    }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int DKH(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("DKH"))
                {
                    try
                    {
                        if (SctExportType.Value == "1")
                        {
                            string[] d = value[i].Split(new string[] { " ", "\t", "TO" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0].Split('/')[0];

                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0].Split('/')[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime($"{d[6].Remove(d[6].Length - 2, 2)}-{d[7]}-{d[11]}".Replace(".", "")),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime($"{d[9].Remove(d[9].Length - 2, 2)}-{d[10]}-{d[11]}".Replace(".", "")),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1].Substring(0, 4),
                                    P_TO_AIRP = d[2].Substring(4, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[1].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            int axc = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0].Split('/')[0].Substring(0, d[0].Split('/')[0].Length - d[0].Split('/')[1].Length) + d[0].Split('/')[1],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime($"{d[6].Remove(d[6].Length - 2, 2)}-{d[7]}-{d[11]}".Replace(".", "")),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime($"{d[9].Remove(d[9].Length - 2, 2)}-{d[10]}-{d[11]}".Replace(".", "")),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2].Substring(4, 4),
                                    P_TO_AIRP = d[3].Substring(d[3].Length - 4, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(8, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                            if (axc > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                        else if (SctExportType.Value == "2")
                        {
                            string[] d = value[i].Split(new string[] { " ", "\t", "TO", "FM", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0].Split('/')[0];
                            string _toDate = "";
                            if (d.Length == 11)
                            {
                                _toDate = clsChuanHoaImport.ChuanHoaDateTime($"{d[8].Remove(d[8].Length - 2, 2)}-{d[9]}-{d[10]}".Replace(".", ""));
                            }
                            else if (d.Length == 10)
                            {
                                _toDate = clsChuanHoaImport.ChuanHoaDateTime($"{d[8].Remove(d[8].Length - 2, 2)}-{d[9].Split('.')[0]}-{d[9].Split('.')[1]}".Replace(".", ""));
                            }
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0].Split('/')[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime($"{d[6].Remove(d[6].Length - 2, 2)}-{d[7].Split('.')[0]}-{d[7].Split('.')[1]}".Replace(".", "")),
                                    P_TODATE = _toDate,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1].Substring(0, 4),
                                    P_TO_AIRP = d[2].Substring(4, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[1].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            int axc = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0].Split('/')[0].Substring(0, d[0].Split('/')[0].Length - d[0].Split('/')[1].Length) + d[0].Split('/')[1],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime($"{d[6].Remove(d[6].Length - 2, 2)}-{d[7].Split('.')[0]}-{d[7].Split('.')[1]}".Replace(".", "")),
                                    P_TODATE = _toDate,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2].Substring(4, 4),
                                    P_TO_AIRP = d[4].Substring(d[4].Length - 4, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                            if (axc > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int THY(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("THY"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[10]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[5],
                                P_TO_AIRP = d[8],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(0, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int HBH(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "-", "/" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_DAILY = "1234567",
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[4],
                            P_TO_AIRP = d[7],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }

            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int ETD(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("ETD"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[6]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[7]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[2],
                                P_TO_AIRP = d[3],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[1].Substring(0, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int JAI(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                        new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[7]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[4],
                            P_TO_AIRP = d[6],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }

            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int UAE(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string _from = "";
            string _to = "";
            if (SctExportType.Value == "2")
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime1(ax[1]);
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[3]);
            }
            else
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime1(ax[2]);
                _to = clsChuanHoaImport.ChuanHoaDateTime1(ax[4]);
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("UAE"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = _from,
                                P_TODATE = _to,
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[6].ToUpper()),
                                P_FROM_AIRP = d[2],
                                P_TO_AIRP = d[4],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = txtRemark.Value,
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CEB(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("CEB"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-", "/t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                            new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1] + DateTime.Now.Year),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2] + DateTime.Now.Year),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[4],
                                P_TO_AIRP = d[5],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int VOZ(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("VA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1] + "2018"),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2] + "2019"),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3].Replace(",", "")),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[4],
                            P_TO_AIRP = d[5],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int AIQ(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            string _daily = "";
            string _etd = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("AIQ"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (d.Length == 11)
                        {
                            _from = d[5]; _to = d[8]; _daily = d[10]; _etd = d[6].Substring(0, 4);
                        }
                        else if (d.Length == 10)
                        {
                            _from = d[5].Substring(0, 4); _to = d[7]; _daily = d[9]; _etd = d[5].Substring(4, 4);
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = _from,
                            P_TO_AIRP = _to,
                            P_ETD = _etd,
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CHH(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("EFF FROM"))
                {
                    string[] ax = value[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    _from = clsChuanHoaImport.ChuanHoaDateTime(ax[2] + ax[3]);
                    _to = clsChuanHoaImport.ChuanHoaDateTime(ax[5] + ax[6]);
                }
                else if (value[i].Contains("CHH"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "/", "(", ")", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (d.Length == 8)
                        {
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2],
                                    P_TO_AIRP = d[6],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++;
                        }
                        else
                        {
                            if (d[5].Length == 10)
                            {
                                int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2],
                                    P_TO_AIRP = d[5].Substring(6, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                                if (kq > 0) iCount++;
                            }
                            else
                            {
                                int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2],
                                    P_TO_AIRP = d[5].Substring(4, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                                if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                            }
                        }
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CSH(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (SctExportType.Value == "2")
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2] + ax[3]);
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[5] + ax[6] + ax[7].Replace(".", ""));
            }
            else
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2] + ax[6].Remove(ax[6].Length - 1, 1));
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4] + ax[5] + ax[6].Remove(ax[6].Length - 1, 1));
            }

            for (int i = 1; i < value.Length; i++)
            {
                string _toairp = "";
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t", "(", ")", "+" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0].Split('/')[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                    {
                        P_CALLSIGN = d[0].Split('/')[0],
                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                        P_FROM_AIRP = d[2].Substring(0, 4),
                        P_TO_AIRP = d[3].Substring(4, 4),
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(4, 4)),
                        P_ETA = "",
                        P_VIA = "",
                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                        P_PERMTYPE = "O/F",
                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                        P_PERMNBR = txtPERMNBR.Value,
                        P_SEASON = ddlSeason.Value,
                        P_OPER = ddlLoaiImport.Value,
                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                        P_VERSION = txtVersion.Value.ToUpper(),
                        P_REGISTRATION = txtReg.Value
                    }).ToString());

                    if (d.Length == 6) _toairp = d[5];
                    else _toairp = d[4].Substring(4, 4);
                    int kq2 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                        new
                        {
                            P_CALLSIGN = d[0].Split('/')[0].Substring(0, d[0].Split('/')[0].Length - d[0].Split('/')[1].Length) + d[0].Split('/')[1],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[3].Substring(4, 4),
                            P_TO_AIRP = _toairp,
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(8, 4)),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    if (kq2 > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CES(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (SctExportType.Value == "2")
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2] + ax[3]);
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[5] + ax[6] + ax[7].Replace(".", ""));
            }
            else
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2] + ax[6].Substring(0, 4));
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4] + ax[5] + ax[6].Substring(0, 4));
            }
            for (int i = 1; i < value.Length; i++)
            {
                if (value[i].Contains("CES"))
                {
                    string _fromAirp = "";
                    string _toAirp = "";
                    string _fromAirp1 = "";
                    string _toairp1 = "";
                    string _Etd = "";
                    string _Etd1 = "";
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "(", ")", "+" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0].Split('/')[0];
                        #region DieuKien
                        if (d.Length == 6)
                        {
                            if (d[3].Length == 12)
                            {
                                _fromAirp = d[2].Substring(0, 4);
                                _toAirp = d[3].Substring(4, 4);
                                _fromAirp1 = d[3].Substring(4, 4);
                                _toairp1 = d[5];
                                _Etd = d[2].Substring(4, 4);
                                _Etd1 = d[3].Substring(8, 4);
                            }
                            else
                            {
                                _fromAirp = d[2].Substring(0, 4);
                                _toAirp = d[4].Substring(0, 4);
                                _fromAirp1 = d[4].Substring(0, 4);
                                _toairp1 = d[5].Substring(4, 4);
                                _Etd = d[2].Substring(4, 4);
                                _Etd1 = d[4].Substring(4, 4);
                            }
                        }

                        else
                        {
                            _fromAirp = d[2].Substring(0, 4);
                            _toAirp = d[3].Substring(4, 4);
                            _fromAirp1 = d[3].Substring(4, 4);
                            _toairp1 = d[4].Substring(4, 4);
                            _Etd = d[2].Substring(4, 4);
                            _Etd1 = d[3].Substring(8, 4);
                        }
                        #endregion
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                        {
                            P_CALLSIGN = d[0].Split('/')[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = _fromAirp,
                            P_TO_AIRP = _toAirp,
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        int kq2 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                            new
                            {
                                P_CALLSIGN = d[0].Split('/')[0].Substring(0, d[0].Split('/')[0].Length - d[0].Split('/')[1].Length) + d[0].Split('/')[1],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = _fromAirp1,
                                P_TO_AIRP = _toairp1,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd1),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        if (kq2 > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int AXM(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _fromAirp = "";
            string _toAirp = "";
            string _to = "";
            string _etd = "";
            string _daily = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("AXM"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (SctExportType.Value == "2")
                        {
                            _to = clsChuanHoaImport.ChuanHoaDateTime(d[3]);
                            _fromAirp = d[5];
                            _toAirp = d[8];
                            _etd = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(0, 4));
                            _daily = clsChuanHoaImport.ChuanHoaDayly(d[10]);
                        }
                        else
                        {
                            _to = clsChuanHoaImport.ChuanHoaDateTime(d[3] + d[4]);
                            _fromAirp = d[6];
                            _toAirp = d[9];
                            _etd = clsChuanHoaImport.ChuanHoaGioBay(d[7].Substring(0, 4));
                            _daily = clsChuanHoaImport.ChuanHoaDayly(d[11]);
                        }

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                P_TODATE = _to,
                                P_DAILY = _daily,
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = _fromAirp,
                                P_TO_AIRP = _toAirp,
                                P_ETD = _etd,
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int MXD(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    if (value[i].Contains("DAILY"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "-", "–", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[10]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[5],
                            P_TO_AIRP = d[8],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(0, 4)),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    else if (value[i].Contains("DAYS"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "-", "–", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        string _daily = "";
                        {
                            for (int j = 11; j < d.Length; j++)
                            {
                                _daily += d[j];
                            }
                        }

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[5],
                            P_TO_AIRP = d[8],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(0, 4)),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CXA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string daily = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", ")", "-", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[3];
                    if (d.Length == 9)
                    {
                        if (d[4].Contains("DAILY "))
                        {
                            daily = "1234567";
                        }
                        else if (d[4].Contains("DAY"))
                        {
                            daily = clsChuanHoaImport.ChuanHoaDayly(d[4].Substring(3));
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[3],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(daily),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[5],
                            P_TO_AIRP = d[8],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++;
                    }
                    else
                    {
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[3],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[6],
                            P_TO_AIRP = d[9],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int MKR(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            string _daily = "";
            string _Etd = "";
            string _Eta = "";
            for (int i = 0; i < value.Length; i++)
            {
                string[] d = value[i].Split(new string[] { " ", "　", "\t", "-", ":", "+1" }, StringSplitOptions.RemoveEmptyEntries);
                _callSign = d[0];
                try
                {
                    if (SctExportType.Value == "2")
                    {
                        if (value[i].Contains("FROM"))
                        {
                            _from = clsChuanHoaImport.ChuanHoaDateTime(d[1]);
                            _to = clsChuanHoaImport.ChuanHoaDateTime(d[3]);
                        }
                        else if (value[i].Contains("LQ"))
                        {
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[6]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[2],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(0, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    else
                    {
                        switch (d.Length.ToString())
                        {
                            case "9":
                                _daily = d[6];
                                _Etd = d[7];
                                _Eta = d[8];
                                break;
                            case "10":
                                _daily = d[6] + d[7];
                                _Etd = d[8];
                                _Eta = d[9];
                                break;
                            case "11":
                                _daily = d[6] + d[7] + d[8];
                                _Etd = d[9];
                                _Eta = d[10];
                                break;
                            case "12":
                                _daily = d[6] + d[7] + d[8] + d[9];
                                _Etd = d[10];
                                _Eta = d[11];
                                break;
                            case "13":
                                _daily = d[6] + d[7] + d[8] + d[9] + d[10];
                                _Etd = d[11];
                                _Eta = d[12];
                                break;
                            case "14":
                                _daily = d[6] + d[7] + d[8] + d[9] + d[10] + d[11];
                                _Etd = d[12];
                                _Eta = d[13];
                                break;
                            case "15":
                                _daily = d[6] + d[7] + d[8] + d[9] + d[10] + d[11] + d[12];
                                _Etd = d[13];
                                _Eta = d[14];
                                break;
                        }
                        int kq = 0;
                        //_Via = Via4text(txtRoutes.Value, d[1], d[2]);
                        kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[5]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[1],
                            P_TO_AIRP = d[2],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                            //P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int NOK(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            string _fromAirp = "";
            string _toAirp = "";
            string _daily = "";
            string _etd = "";

            string[] ax = value[0].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
            _to = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    if (value[i].Contains("DD"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (SctExportType.Value == "2")
                        {
                            _fromAirp = d[1];
                            _toAirp = d[3];
                            _daily = clsChuanHoaImport.ChuanHoaDayly(d[6]);
                            _etd = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0, 4));
                        }
                        else
                        {
                            _fromAirp = d[4];
                            _toAirp = d[7];
                            _daily = clsChuanHoaImport.ChuanHoaDayly(d[2]);
                            _etd = clsChuanHoaImport.ChuanHoaGioBay(d[5].Substring(0, 4));
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = _daily,
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = _fromAirp,
                                    P_TO_AIRP = _toAirp,
                                    P_ETD = _etd,
                                    P_ETA = "",
                                    P_VIA = txtRoutes.Value,
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int NOK_B(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("FROM"))
                {
                    string[] ax = value[0].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + DateTime.Now.Year);
                    _to = clsChuanHoaImport.ChuanHoaDateTime(ax[2] + DateTime.Now.Year);
                }
                try
                {
                    if (value[i].Contains("DD"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "+1" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5].Replace("D", "")),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[3],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int BOX(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("BOX"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[4],
                                    P_TO_AIRP = d[6],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                    P_ETA = "",
                                    P_VIA = txtRoutes.Value.ToUpper(),
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CAL(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string _from = ax[1];
            string _to = ax[3];

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("CAL"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "+1" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0] + d[1];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0] + d[1],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[7].ToUpper()),
                                P_FROM_AIRP = d[3],
                                P_TO_AIRP = d[5],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(d[7].ToUpper()),

                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = d[8],
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int EVA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[3]);

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("EVA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = _from,
                                P_TODATE = _to,
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[2],
                                P_TO_AIRP = d[8],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int ESR(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4]);

            for (int i = 1; i < value.Length; i++)
            {
                if (value[i].Contains("ESR"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "/" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0] + d[1];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0] + d[1],
                                P_FROMDATE = _from,
                                P_TODATE = _to,
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[3],
                                P_TO_AIRP = d[9],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int JNA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    if (d.Length == 9)
                    {
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[7]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[8]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[6]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[4],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    else
                    {
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[6]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[7]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5].Replace("D", "")),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[1],
                            P_TO_AIRP = d[4],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int BAW(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[value.Length - 1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4] + ax[5]);

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("BAW"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0] + d[1];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0] + d[1],
                                P_FROMDATE = _from,
                                P_TODATE = _to,
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[3],
                                P_TO_AIRP = d[9],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int AAR(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _error = "";
            string _callSign = "";
            string _daily = "";
            string _fromAirp = "";
            string _toAirp = "";
            string _etd = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("FROM"))
                {
                    string[] ax = value[i].Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ax.Contains("Only"))
                    {
                        _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2]);
                        _to = _from;
                    }
                    else
                    {
                        _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2]);
                        _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4] + ax[5]);
                    }

                }
                else if (value[i].Contains("AAR"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "/" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].Length > 3)
                        {
                            _daily = clsChuanHoaImport.ChuanHoaDayly(d[1]); _etd = clsChuanHoaImport.ChuanHoaGioBay(d[3]);
                            _callSign = d[0]; _fromAirp = d[2]; _etd = d[3]; _toAirp = d[8];
                        }
                        else
                        {
                            _daily = clsChuanHoaImport.ChuanHoaDayly(d[2]); _etd = clsChuanHoaImport.ChuanHoaGioBay(d[4]);
                            _callSign = d[0] + d[1]; _fromAirp = d[3]; _toAirp = d[9];
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = _callSign,
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = _daily,
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = _fromAirp,
                                    P_TO_AIRP = _toAirp,
                                    P_ETD = _etd,
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int BSX(string[] value)
        {

            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string fromdate = "";
            string todate = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("FROM"))
                {
                    fromdate = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    todate = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[3];
                }
                else if (value[i].Contains("BSX"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[1];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[1],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(fromdate),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(todate),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[6])),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[2].Substring(0, 4),
                            P_TO_AIRP = d[2].Substring(5, 4),
                            P_ETD = d[4].Substring(0, 4),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int LKH(string[] value)
        {
            int iCount = 0;
            for (int i = 0; i < value.Length; i++)
            {
                string[] d = value[i].Split(new string[] { " ", "/", "-" }, StringSplitOptions.RemoveEmptyEntries);
                int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                    , new
                    {
                        P_CALLSIGN = d[0],
                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[9]),
                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[10].Substring(4, 7)),
                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[12]),
                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                        P_FROM_AIRP = d[1],
                        P_TO_AIRP = d[5],
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                        P_ETA = "",
                        P_VIA = txtRoutes.Value,
                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                        P_PERMTYPE = "O/F",
                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                        P_PERMNBR = txtPERMNBR.Value,
                        P_SEASON = ddlSeason.Value,
                        P_OPER = ddlLoaiImport.Value,
                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                        P_VERSION = txtVersion.Value.ToUpper(),
                        P_REGISTRATION = txtReg.Value
                    }).ToString());
                if (kq > 0) iCount++;
            }
            return iCount;
        }
        public int CKK(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _eta = "";
            string _from = "";
            string _to = "";
            string _toAirp = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("FROM"))
                {
                    string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    _from = ax[2] + ax[3];
                    _to = ax[5] + ax[6].Replace(".", "");
                }
                else if (SctExportType.Value == "1" && value[i].Contains("CKK"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "+", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];

                        if (d.Length == 6) { _eta = d[4]; _toAirp = d[5]; }
                        else { _eta = d[4].Substring(0, 4); _toAirp = d[4].Substring(4, 4); }

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                P_CRAFT = clsChuanHoaImport.CraftType(d[1].ToUpper()),
                                P_FROM_AIRP = d[3].Substring(0, 4),
                                P_TO_AIRP = _toAirp,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(4, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(d[1]),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
                else if (SctExportType.Value == "2" & value[i].Contains("CKK"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                                P_CRAFT = clsChuanHoaImport.CraftType(d[1].ToUpper()),
                                P_FROM_AIRP = d[2].Substring(0, 4),
                                P_TO_AIRP = d[3].Substring(d[3].Length - 4, 4),
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(4, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(d[1]),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CCA(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("EFF"))
                {
                    string[] ax = value[i].Split(new string[] { ":", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    _from = ax[1];
                    _to = ax[2];
                }
                else if (value[i].Contains("CCA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[2],
                                P_TO_AIRP = d[8],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int SWM(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string daily = "";
            string[] ax = value[0].Split(new string[] { "-", " " }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
            for (int i = 1; i < value.Length; i++)
            {
                if (value[i].Contains("DATE OPS"))
                {
                    string[] cx = value[i].Split(new string[] { ":", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    daily = clsChuanHoaImport.ChuanHoaDayly(cx[1]);
                }
                else
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { "-", " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0].Substring(0, 3) + d[1];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0].Substring(0, 3) + d[1],
                            P_FROMDATE = _from,
                            P_TODATE = _to,
                            P_DAILY = daily,
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[3],
                            P_TO_AIRP = d[6],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CSN(string[] value)
        {
            int iCount = 0;
            string _toAirp = "";
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", ")", "-", "\t", "+1" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[3];
                    if (d.Length == 8) _toAirp = d[7];
                    else _toAirp = d[6].Substring(4, 4);
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                    , new
                    {
                        P_CALLSIGN = d[3],
                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                        P_FROM_AIRP = d[5].Substring(0, 4),
                        P_TO_AIRP = _toAirp,
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5].Substring(4, 4)),
                        P_ETA = "",
                        P_VIA = "",
                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                        P_PERMTYPE = "O/F",
                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                        P_PERMNBR = txtPERMNBR.Value,
                        P_SEASON = ddlSeason.Value,
                        P_OPER = ddlLoaiImport.Value,
                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                        P_VERSION = txtVersion.Value.ToUpper(),
                        P_REGISTRATION = txtReg.Value
                    }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int RJA(string[] value)
        {

            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string fromdate = "";
            string todate = "";
            string daily = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("FROM"))
                {
                    fromdate = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    todate = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[3];
                }
                else
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        daily = d[1].Substring(4);
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(fromdate),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(todate),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(daily),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[2],
                            P_TO_AIRP = d[4],
                            P_ETD = d[3],
                            P_ETA = "",
                            P_VIA = txtRoutes.Value,
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int AHK(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("AHK"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[10].Replace("D", "")),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[5],
                                P_TO_AIRP = d[8],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(0, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CQH(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _daily = "";
            string _fromAirp = "";
            string _toAirp = "";
            string _Etd = "";

            string _fromAirp2 = "";
            string _toAirp2 = "";
            string _Etd2 = "";
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " ", ":", "TH" }, StringSplitOptions.RemoveEmptyEntries);
            if (SctExportType.Value == "2")
            {
                _from = ax[4] + ax[5] + ax[6];
                _to = ax[8] + ax[9] + ax[10];
            }
            else
            {
                _from = ax[3] + ax[4] + ax[5];
                _to = ax[7] + ax[8] + ax[9];
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("CQH"))
                {
                    try
                    {
                        if (value[i].Contains("/"))
                        {
                            string[] bx = value[i].Split(new string[] { "FM", "FROM" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] d1 = bx[0].Split(new string[] { " ", "+" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d1[0].Split('/')[0];
                            if (d1.Length == 4)
                            {
                                _daily = "1234567"; _fromAirp = d1[1].Substring(0, 4); _toAirp = d1[2].Substring(4, 4); _Etd = d1[1].Substring(4, 4);
                                _fromAirp2 = d1[2].Substring(4, 4); _toAirp2 = d1[3].Substring(4, 4); _Etd2 = d1[2].Substring(8, 4);
                            }
                            else if (d1.Length == 5)
                            {
                                _daily = d1[4]; _fromAirp = d1[1].Substring(0, 4); _toAirp = d1[2].Substring(4, 4); _Etd = d1[1].Substring(4, 4);
                                _fromAirp2 = d1[2].Substring(4, 4); _toAirp2 = d1[3].Substring(4, 4); _Etd2 = d1[2].Substring(8, 4);
                            }
                            else if (d1.Length == 6)
                            {
                                if (d1.Contains("DAILY"))
                                {
                                    _daily = d1[5]; _fromAirp = d1[1].Substring(0, 4); _toAirp = d1[2].Substring(4, 4); _Etd = d1[1].Substring(4, 4);
                                    _fromAirp2 = d1[2].Substring(4, 4); _toAirp2 = d1[4]; _Etd2 = d1[3];
                                }
                                else
                                {
                                    _daily = d1[5]; _fromAirp = d1[1].Substring(0, 4); _toAirp = d1[2].Substring(4, 4); _Etd = d1[1].Substring(4, 4);
                                    _fromAirp2 = d1[2].Substring(4, 4); _toAirp2 = d1[3].Substring(4, 4); _Etd2 = d1[2].Substring(8, 4);
                                }
                            }
                            else if (d1.Length == 7)
                            {
                                if (d1.Contains("DAILY"))
                                {
                                    _daily = "1234567"; _fromAirp = d1[1].Substring(0, 4); _toAirp = d1[3].Substring(0, 4); _Etd = d1[1].Substring(4, 4);
                                    _fromAirp2 = d1[3].Substring(0, 4); _toAirp2 = d1[5]; _Etd2 = d1[3].Substring(4, 4);
                                }
                                else
                                {
                                    _daily = d1[6]; _fromAirp = d1[1].Substring(0, 4); _toAirp = d1[2].Substring(4, 4); _Etd = d1[1].Substring(4, 4);
                                    _fromAirp2 = d1[2].Substring(4, 4); _toAirp2 = d1[4]; _Etd2 = d1[2].Substring(8, 4);
                                }
                            }
                            else if (d1.Length == 8)
                            {
                                _daily = d1[7]; _fromAirp = d1[1].Substring(0, 4); _toAirp = d1[3].Substring(0, 4); _Etd = d1[1].Substring(4, 4);
                                _fromAirp2 = d1[3].Substring(0, 4); _toAirp2 = d1[5]; _Etd2 = d1[3].Substring(4, 4);
                            }
                            else
                            {
                                _daily = d1[5]; _fromAirp = d1[1].Substring(4, 4);
                                _toAirp = d1[3].Substring(0, 4); _Etd = d1[2].Substring(0, 4); _Etd2 = d1[3].Substring(4, 4);
                            }
                            int kq1 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                            {
                                P_CALLSIGN = d1[0].Split('/')[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = _fromAirp,
                                P_TO_AIRP = _toAirp,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                            int kq2 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                            {
                                P_CALLSIGN = d1[0].Split('/')[0].Substring(0, d1[0].Split('/')[0].Length - d1[0].Split('/')[1].Length) + d1[0].Split('/')[1],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = _fromAirp2,
                                P_TO_AIRP = _toAirp2,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd2),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                            if (kq1 > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                            if (kq2 > 0) iCount++;
                        }
                        else
                        {
                            string[] cx = value[i].Split(new string[] { "FM", "FROM" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] d = cx[0].Split(new string[] { " ", "/t", "+" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0]; _fromAirp = d[1].Substring(0, 4); _Etd = d[1].Substring(4, 4);
                            if (d.Length == 4)
                            {
                                _daily = d[3];
                                _toAirp = d[2].Substring(4, 4);
                            }
                            else if (d.Length == 5)
                            {
                                if (d[2].Length > 4) _toAirp = d[2].Substring(4, 4);
                                else _toAirp = d[3];
                                _daily = d[4];
                            }
                            else if (d.Length == 6)
                            {
                                _daily = d[5]; _toAirp = d[3];
                            }
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                            {
                                P_CALLSIGN = _callSign,
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = _fromAirp,
                                P_TO_AIRP = _toAirp,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int MAS(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[7],
                            P_TO_AIRP = d[10],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int MAS66(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    string _daily = "";
                    string _fromAirp = "";
                    string _toAirp = "";
                    string _fromDate = "";
                    string _toDate = "";
                    string _Etd = "";
                    if (d[1].Length > 8)
                    {
                        _callSign = d[1].Substring(0, d[1].Length - 7);
                        _fromDate = d[1].Substring(d[1].Length - 7, 7);
                        _toDate = d[3];
                        if (d.Contains("DAILY")) { _daily = "1234567"; _fromAirp = d[6]; _Etd = d[7].Substring(0, 4); _toAirp = d[9]; }
                        else { _daily = d[5]; _fromAirp = d[7]; _Etd = d[8].Substring(0, 4); _toAirp = d[10]; }
                    }
                    else
                    {
                        _callSign = d[1];
                        _fromDate = d[2];
                        _toDate = d[4];
                        if (d.Contains("DAILY")) { _daily = "1234567"; _fromAirp = d[7]; _Etd = d[8].Substring(0, 4); _toAirp = d[10]; }
                        else
                        {
                            if (d[5].Length > 4)
                            {
                                _daily = d[5].Replace("DAYS", "").Replace("DAY", ""); _fromAirp = d[7]; _toAirp = d[10]; _Etd = d[8];
                            }
                            else
                            {
                                _daily = d[6]; _fromAirp = d[8]; _Etd = d[9].Substring(0, 4);
                                if (d[9].Length > 4) _toAirp = d[10];
                                else _toAirp = d[11];
                            }
                        }
                    }
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = _callSign,
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_fromDate),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_toDate),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                P_CRAFT = clsChuanHoaImport.CraftType(d[0]),
                                P_FROM_AIRP = _fromAirp,
                                P_TO_AIRP = _toAirp,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(d[0]),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = "MAS",
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int TGW(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "–", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[6] + d[9]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[7] + d[8] + d[9]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[1],
                            P_TO_AIRP = d[4],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                            P_ETA = "",
                            P_VIA = d[14],
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int ETH(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("ET"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        string _fromDate = ""; string _toDate = "";
                        string _daily = ""; string _craft = ""; string _fromAirp = "";
                        string _toAirp = ""; string _etd = "";
                        switch (d.Length)
                        {
                            case 11:
                                _callSign = d[0] + d[1]; _fromDate = d[2]; _toDate = d[3];
                                _daily = d[4]; _fromAirp = d[5]; _toAirp = d[7]; _craft = d[10]; _etd = d[6];
                                break;
                            case 10:
                                _callSign = d[0]; _fromDate = d[1]; _toDate = d[2];
                                _daily = d[3]; _fromAirp = d[4]; _toAirp = d[6]; _craft = d[9]; _etd = d[5];
                                break;
                            case 9:
                                _callSign = d[1]; _fromDate = d[2].Split('-', '’')[0] + d[2].Split('-', '’')[2];
                                _toDate = d[2].Split('-', '’')[1] + d[2].Split('-', '’')[2];
                                _fromAirp = d[4]; _toAirp = d[6];
                                _daily = d[3]; _craft = d[8]; _etd = d[5];
                                break;
                            case 8:
                                _callSign = d[0]; _fromDate = _fromDate = d[1].Split('-', '’')[0] + d[1].Split('-', '’')[2];
                                _toDate = d[1].Split('-', '’')[1] + d[1].Split('-', '’')[2];
                                _fromAirp = d[3]; _toAirp = d[5];
                                _daily = d[2]; _craft = d[7]; _etd = d[4];
                                break;
                            default:
                                string[] d1 = value[i - 1].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                if (d1.Length == 11)
                                {
                                    _callSign = d1[0] + d1[1]; _fromDate = d1[2]; _toDate = d1[3];
                                    _daily = d[0]; _fromAirp = d[1]; _toAirp = d[3]; _craft = d[6]; _etd = d[2];
                                }
                                else if (d1.Length == 10)
                                {
                                    _callSign = d1[0]; _fromDate = d1[1]; _toDate = d1[2];
                                    _daily = d[0]; _fromAirp = d[1]; _toAirp = d[3]; _craft = d[6]; _etd = d[2];
                                }
                                break;
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                    , new
                                    {
                                        P_CALLSIGN = _callSign,
                                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_fromDate),
                                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_toDate),
                                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                        P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(_craft),
                                        P_FROM_AIRP = _fromAirp,
                                        P_TO_AIRP = _toAirp,
                                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_etd),
                                        P_ETA = "",
                                        P_VIA = "",
                                        P_REMARK = "",
                                        P_PERMTYPE = "O/F",
                                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                        P_PERMNBR = txtPERMNBR.Value,
                                        P_SEASON = ddlSeason.Value,
                                        P_OPER = ddlLoaiImport.Value,
                                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                                        P_VERSION = txtVersion.Value,
                                        P_REGISTRATION = txtReg.Value
                                    }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int APG(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _error = "";
            string _callSign = "";

            string[] ax = value[0].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2] + ax[3]);
            _to = clsChuanHoaImport.ChuanHoaDateTime(ax[5] + ax[6] + ax[7]);
            for (int i = 1; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[10]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[5],
                            P_TO_AIRP = d[8],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(0, 4)),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int XAX(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            for (int i = 0; i < value.Length; i++)
            {

                if (value[i].Contains("FROM"))
                {
                    string[] ax = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _from = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
                    _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4]);
                }
                if (value[i].Contains("XAX"))
                {
                    string daily = "";
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (d.Length > 6)
                        {
                            if (d[5].Contains("DAY"))
                                for (int j = 6; j < d.Length; j++)
                                {

                                    daily += d[j];
                                }
                            else
                                for (int e = 5; e < d.Length; e++)
                                {
                                    daily += d[e];
                                }
                        }
                        else daily = d[5];

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(daily),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[2],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CSZ(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                string _fromAirp = "";
                string _toAirp = "";
                string _fromAirp2 = "";
                string _toAirp2 = "";
                string _Etd = "";
                string _Etd2 = "";
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "-", "/t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    if (d.Length == 7)
                    {
                        _fromAirp = d[5].Substring(0, 4);
                        _toAirp = d[6].Substring(d[6].Length - 4, 4);
                        _Etd = clsChuanHoaImport.ChuanHoaGioBay(d[5].Substring(4, 4));
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                        new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2] + DateTime.Now.Year),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3].Substring(0, 5) + "2019"),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                            P_CRAFT = clsChuanHoaImport.CraftType(d[1].ToUpper()),
                            P_FROM_AIRP = _fromAirp,
                            P_TO_AIRP = _toAirp,
                            P_ETD = _Etd,
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(d[1].ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    else if (d.Length == 8)
                    {
                        _fromAirp = d[5].Substring(0, 4);
                        _toAirp = d[6].Substring(4, 4);
                        _Etd = clsChuanHoaImport.ChuanHoaGioBay(d[5].Substring(4, 4));
                        _fromAirp2 = d[6].Substring(4, 4);
                        _toAirp2 = d[7].Substring(d[7].Length - 4, 4);
                        _Etd2 = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(d[6].Length - 4, 4));
                        int kq1 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                        new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2] + DateTime.Now.Year),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3].Substring(0, 5) + "2019"),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                            P_CRAFT = clsChuanHoaImport.CraftType(d[1].ToUpper()),
                            P_FROM_AIRP = _fromAirp,
                            P_TO_AIRP = _toAirp,
                            P_ETD = _Etd,
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(d[1].ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq1 > 0) iCount++; _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        int kq2 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                        new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2] + DateTime.Now.Year),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3].Substring(0, 5) + "2019"),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                            P_CRAFT = clsChuanHoaImport.CraftType(d[1].ToUpper()),
                            P_FROM_AIRP = _fromAirp2,
                            P_TO_AIRP = _toAirp2,
                            P_ETD = _Etd2,
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(d[1].ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq2 > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int JCC(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            string _daily = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("EVERY DAY"))
                {
                    string[] ex = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    _from = ex[3]; _to = ex[4]; _daily = "123456";
                }

                if (value[i].Contains("JCC"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-", "/QD", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                            new
                            {
                                P_CALLSIGN = d[0] + d[1],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[2].Substring(0, 4),
                                P_TO_AIRP = d[5].Substring(0, 4),
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(4, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int KME(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _via1 = "";
            string _via2 = "";
            string _error = "";
            string _callSign1 = "";
            string _callSign2 = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("DATE"))
                {
                    string[] ax = value[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1].Replace(",", ""));
                    _to = clsChuanHoaImport.ChuanHoaDateTime(ax[3].Replace(",", ""));
                }
                else if (value[i].Contains("KR"))
                {
                    string[] bx = value[i].Trim().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign1 = bx[0];
                    _callSign2 = bx[1];
                }
                else if (value[i].Contains("DAY"))
                {
                    try
                    {
                        string[] cx = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        _via1 = FilterVia(RutGonVia(NhanDangVia(cx[1] + cx[1])).Trim().Replace("-", " ").Replace("--", " "));
                        string[] d = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                        {
                            P_CALLSIGN = _callSign1,
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[2].Substring(0, 4),
                            P_TO_AIRP = d[3].Substring(4, 4),
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0, 4)),
                            P_ETA = "",
                            P_VIA = _via1,
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        i++;
                        string[] d2 = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] cx2 = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        _via2 = FilterVia(RutGonVia(NhanDangVia(cx2[1] + cx2[1])).Trim().Replace("-", " ").Replace("--", " "));
                        int kq2 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                        {
                            P_CALLSIGN = _callSign2,
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d2[1]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d2[2].Substring(0, 4),
                            P_TO_AIRP = d2[3].Substring(4, 4),
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d2[2].Substring(0, 4)),
                            P_ETA = "",
                            P_VIA = _via2,
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign1 + "<br />"}";
                        if (kq2 > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign2 + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign1 + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int AIC(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (!value[i].Contains("FLT NBR"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                            new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2].Substring(2, 7)),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[11]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[4],
                                P_TO_AIRP = d[8],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int TAX(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("TAX"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                            new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[10]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[5],
                                P_TO_AIRP = d[8],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(0, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int HXA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _toAirp1 = "";
            string _toAirp2 = "";
            string _from = "";
            string _to = "";
            string _callSign1 = "";
            string _callSign2 = "";

            string[] ax = value[0].Split(new string[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            _from = ax[1] + ax[2];
            _to = ax[4] + ax[2];

            for (int i = 1; i < value.Length; i++)
            {
                try
                {
                    if (value[i].Contains("HXA"))
                    {
                        string[] d = value[i].Trim().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign1 = d[0];
                        _callSign2 = d[0].Substring(0, d[0].Length - d[1].Length) + d[1];
                    }
                    else if (value[i].Contains("MAIN ROUTE"))
                    {
                        string _via = "";
                        if (i < value.Length - 1)
                        {
                            string[] bx = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] ex = value[i + 1].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (value[i + 1].Contains("BACKUP ROUTE"))
                                _via = FilterVia(RutGonVia(NhanDangVia(bx[1] + ex[1])).Trim().Replace("-", " ").Replace("--", " "));
                            else _via = FilterVia(RutGonVia(NhanDangVia(bx[1])).Trim().Replace("-", " ").Replace("--", " "));
                        }
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        if (SctExportType.Value == "2") _toAirp1 = d[5];
                        else _toAirp1 = d[6];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                            new
                            {
                                P_CALLSIGN = _callSign1,
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[2],
                                P_TO_AIRP = _toAirp1,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                P_ETA = "",
                                P_VIA = _via,
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        i++;
                        if (value[i].Contains("BACKUP ROUTE")) i++;
                        string[] cx = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (i < value.Length - 1)
                        {
                            string[] gx = value[i + 1].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (value[i + 1].Contains("BACKUP ROUTE"))
                                _via = FilterVia(RutGonVia(NhanDangVia(cx[1] + gx[1])).Trim().Replace("-", " ").Replace("--", " "));
                            else
                                _via = FilterVia(RutGonVia(NhanDangVia(cx[1])).Trim().Replace("-", " ").Replace("--", " "));
                        }
                        string[] d1 = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        if (SctExportType.Value == "2") _toAirp2 = d1[5];
                        else _toAirp2 = d[6];
                        int kq2 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT",
                            new
                            {
                                P_CALLSIGN = _callSign2,
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d1[1]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d1[2],
                                P_TO_AIRP = _toAirp2,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d1[3]),
                                P_ETA = "",
                                P_VIA = _via,
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign1 + "<br />"}";
                        if (kq2 > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign2 + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign1 + "<br />"}";
                    lblLOG.Text = _error;
                }

            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int RBA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                string _daily = "";
                string _from = "";
                string _to = "";
                try
                {
                    if (SctExportType.Value == "1" && value[i].Contains("RBA"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "-", "–", "/", "(", ")", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        switch (d.Length.ToString())
                        {
                            case "10":
                                _daily = d[6];
                                _from = d[7];
                                _to = d[9];
                                break;
                            case "11":
                                _daily = d[6] + d[7];
                                _from = d[8];
                                _to = d[10];
                                break;
                            case "12":
                                _daily = d[6] + d[7] + d[8];
                                _from = d[9];
                                _to = d[11];
                                break;
                            case "13":
                                _daily = d[6] + d[7] + d[8] + d[9];
                                _from = d[10];
                                _to = d[12];
                                break;
                            case "14":
                                _daily = d[6] + d[7] + d[8] + d[9] + d[10];
                                _from = d[11];
                                _to = d[13];
                                break;
                            case "15":
                                _daily = d[6] + d[7] + d[8] + d[9] + d[10] + d[11];
                                _from = d[12];
                                _to = d[14];
                                break;
                            case "16":
                                _daily = d[6] + d[7] + d[8] + d[9] + d[10] + d[11] + d[12];
                                _from = d[13];
                                _to = d[15];
                                break;
                        }
                        if (_from.Length < 7) _from += DateTime.Now.Year;
                        if (_to.Length < 7) _to += DateTime.Now.Year;
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from.Replace(".", "")),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to.Replace(".", "")),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[2],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(0, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    else if (SctExportType.Value == "2" && value[i].Contains("RBA"))
                    {
                        string[] d = value[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[5]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[6]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[3].Substring(0, 4),
                                P_TO_AIRP = d[3].Substring(d[3].Length - 4, 4),
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CBJ(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[2] + ax[3]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[5] + ax[6]);

            for (int i = 1; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "/", "(", ")", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = _from,
                            P_TODATE = _to,
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[2],
                            P_TO_AIRP = d[5].Substring(4, 4),
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        #region SVR,LKE Old
        //public int SVR(string[] value)
        //{
        //    int iCount = 0;
        //    for (int i = 0; i < value.Length; i++)
        //    {
        //        if (value[i].Contains("SVR"))
        //        {

        //            string[] d = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
        //            if (d.Length == 7)
        //            {
        //                d = d.Where(w => w != d[0]).ToArray();
        //            }

        //            string[] sArrProdID = null;
        //            char[] sep = { ',' };
        //            sArrProdID = d[3].ToString().Trim().Split(sep);
        //            for (int k = 0; k < sArrProdID.Length; k++)
        //            {

        //                int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
        //                    , new
        //                    {
        //                        P_CALLSIGN = d[0],
        //                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(sArrProdID[k] + d[4] + d[5]),
        //                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(sArrProdID[k] + d[4] + d[5]),
        //                        P_DAILY = clsChuanHoaImport.ChuanHoaDaylyNew(sArrProdID[k].ToString(), d[4], d[5]),
        //                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
        //                        P_FROM_AIRP = d[1].Substring(0, 4),
        //                        P_TO_AIRP = d[2].Substring(0, 4),
        //                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[1].Substring(4)),
        //                        P_ETA = "",//clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(4)),
        //                        P_VIA = txtRoutes.Value.ToUpper(),
        //                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
        //                        P_PERMTYPE = "O/F",
        //                        P_PERMNBR = txtPERMNBR.Value,
        //                        P_SEASON = ddlSeason.Value,
        //                        P_OPER = ddlLoaiImport.Value,
        //                        P_AUTHOR = ddlAUTHOR.SelectedValue,
        //                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
        //                        P_PURPOSE = ddlPURPOSE.SelectedValue,
        //                        P_VERSION = txtVersion.Value.ToUpper(),
        //                        P_REGISTRATION = txtReg.Value
        //                    }).ToString());
        //                if (kq > 0) iCount++;

        //            }

        //        }
        //    }
        //    return iCount;
        //}
        //public int LKE(string[] value)
        //{
        //    int iCount = 0;
        //    string calsign = ""; string fromdate = ""; string todate = ""; string fromair = ""; string toair = ""; string etd = ""; string eta = ""; string[] date1 = null; string[] date2 = null; string[] ax1 = null; string[] ax2 = null;
        //    for (int i = 0; i < value.Length; i++)
        //    {

        //        if (value[i].Contains("LKE"))
        //        {
        //            string[] ax0 = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
        //            calsign = ax0[2].ToString();
        //        }
        //        else if (value[i].Contains("ETD"))
        //        {
        //            ax1 = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
        //            date1 = ax1[4].Split(new string[] { " ", "\t", "-", "–", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
        //            date2 = ax1[8].Split(new string[] { " ", "\t", "-", "–", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
        //            fromair = ax1[1];
        //            etd = ax1[11];
        //        }
        //        else
        //        {
        //            ax2 = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);

        //            toair = ax2[1];
        //            eta = "";
        //        }

        //        if (ax2 != null)
        //        {
        //            int kq = 0;

        //            for (int l = 0; l < date1.Length; l++)
        //            {

        //                kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
        //                    , new
        //                    {
        //                        P_CALLSIGN = calsign,
        //                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(date1[l] + ax1[5] + ax1[6]),
        //                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(date1[l] + ax1[5] + ax1[6]),
        //                        P_DAILY = clsChuanHoaImport.ChuanHoaDaylyNew(date1[l].ToString(), ax1[5], ax1[6]),
        //                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
        //                        P_FROM_AIRP = fromair,
        //                        P_TO_AIRP = toair,
        //                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(etd),
        //                        P_ETA = clsChuanHoaImport.ChuanHoaGioBay(eta),
        //                        P_VIA = txtRoutes.Value.ToUpper(),
        //                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
        //                        P_PERMTYPE = "O/F",
        //                        P_PERMNBR = txtPERMNBR.Value,
        //                        P_SEASON = ddlSeason.Value,
        //                        P_OPER = ddlLoaiImport.Value,
        //                        P_AUTHOR = ddlAUTHOR.SelectedValue,
        //                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
        //                        P_PURPOSE = ddlPURPOSE.SelectedValue,
        //                        P_VERSION = txtVersion.Value.ToUpper(),
        //                        P_REGISTRATION = txtReg.Value
        //                    }).ToString());
        //                if (kq > 0) iCount++;

        //            }

        //            for (int k = 0; k < date2.Length; k++)
        //            {

        //                kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
        //                    , new
        //                    {
        //                        P_CALLSIGN = calsign,
        //                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(date2[k] + ax1[5] + ax1[6]),
        //                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(date2[k] + ax1[5] + ax1[6]),
        //                        P_DAILY = clsChuanHoaImport.ChuanHoaDaylyNew(date2[k].ToString(), ax1[5], ax1[6]),
        //                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
        //                        P_FROM_AIRP = fromair,
        //                        P_TO_AIRP = toair,
        //                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(etd),
        //                        P_ETA = clsChuanHoaImport.ChuanHoaGioBay(eta),
        //                        P_VIA = txtRoutes.Value.ToUpper(),
        //                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
        //                        P_PERMTYPE = "O/F",
        //                        P_PERMNBR = txtPERMNBR.Value,
        //                        P_SEASON = ddlSeason.Value,
        //                        P_OPER = ddlLoaiImport.Value,
        //                        P_AUTHOR = ddlAUTHOR.SelectedValue,
        //                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
        //                        P_PURPOSE = ddlPURPOSE.SelectedValue,
        //                        P_VERSION = txtVersion.Value.ToUpper(),
        //                        P_REGISTRATION = txtReg.Value
        //                    }).ToString());
        //                if (kq > 0) iCount++;

        //            }
        //            ax2 = null;
        //        }
        //    }
        //    return iCount;
        //}
        #endregion
        public int SVR(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("SVR"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (d.Length == 7)
                        {
                            d = d.Where(w => w != d[0]).ToArray();
                        }

                        string[] sArrProdID = null;
                        char[] sep = { ',' };
                        sArrProdID = d[3].ToString().Trim().Split(sep);

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(sArrProdID[0] + d[4] + d[5]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(sArrProdID[sArrProdID.Length - 1] + d[4] + d[5]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDaylySVR(d[3].ToString(), d[4], d[5]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[1].Substring(0, 4),
                                P_TO_AIRP = d[2].Substring(0, 4),
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[1].Substring(4)),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(4)),
                                P_VIA = txtRoutes.Value.ToUpper(),
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int LKE(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string calsign = ""; string fromdate = ""; string todate = ""; string fromair = ""; string toair = ""; string etd = ""; string eta = ""; string[] date1 = null; string[] date2 = null; string[] ax1 = null; string[] ax2 = null;
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    if (value[i].Contains("LKE"))
                    {
                        string[] ax0 = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                        calsign = ax0[2].ToString();
                    }
                    else if (value[i].Contains("ETD"))
                    {
                        ax1 = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                        date1 = ax1[4].Split(new string[] { " ", "\t", "-", "–", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                        date2 = ax1[8].Split(new string[] { " ", "\t", "-", "–", "(", ")", "," }, StringSplitOptions.RemoveEmptyEntries);
                        fromdate = clsChuanHoaImport.ChuanHoaDateTime(date1[0] + ax1[5] + ax1[6]);
                        todate = clsChuanHoaImport.ChuanHoaDateTime(date2[date2.Length - 1] + ax1[9] + ax1[10]);



                        fromair = ax1[1];
                        etd = ax1[11];
                    }
                    else
                    {
                        ax2 = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);

                        toair = ax2[1];
                        eta = ax2[11];
                    }

                    if (ax2 != null)
                    {
                        int kq = 0;

                        kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = calsign,
                                P_FROMDATE = fromdate,
                                P_TODATE = todate,
                                P_DAILY = clsChuanHoaImport.ChuanHoaDaylyLKE(ax1[4].ToString(), ax1[5], ax1[6]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = fromair,
                                P_TO_AIRP = toair,
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(etd),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(eta),
                                P_VIA = txtRoutes.Value.ToUpper(),
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + calsign + "<br />"}";
                        ax2 = null;
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + calsign + "<br />"}";
                    lblLOG.Text = _error;
                }

            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int NCT(string[] value)
        {

            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax1 = null; string[] ax2 = null; string[] ax = null;
            string fromdate = ""; string todate = ""; string dayly = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("EFF"))
                {
                    ax1 = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")", ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ax1.Length == 4)
                    {
                        fromdate = clsChuanHoaImport.ChuanHoaDateTime(ax1[2]);
                        todate = clsChuanHoaImport.ChuanHoaDateTime(ax1[3]);
                    }
                    else if (ax1.Length == 3)
                    {
                        fromdate = clsChuanHoaImport.ChuanHoaDateTime(ax1[1]);
                        todate = clsChuanHoaImport.ChuanHoaDateTime(ax1[2]);
                    }
                }
                else if (value[i].Contains("DOF"))
                {
                    ax = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ax.Length == 3)
                        dayly = ax[2];
                    else
                        dayly = ax[3];
                }
                else if (value[i].Contains("NCT"))
                {
                    try
                    {
                        ax2 = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = ax2[0];
                        if (ax2.Length == 7)
                            dayly = ax2[6];

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = ax2[0],
                                P_FROMDATE = fromdate,
                                P_TODATE = todate,
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(dayly),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = ax2[1].Substring(ax2[1].Length - 4, 4),
                                P_TO_AIRP = ax2[3].Substring(ax2[3].Length - 4, 4),
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(ax2[2].Substring(0, 4)),
                                P_ETA = "",
                                P_VIA = txtRoutes.Value.ToUpper(),
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int GIA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            string _from = ax[1] + DateTime.Now.Year;
            string _to = ax[2] + DateTime.Now.Year;
            for (int i = 1; i < value.Length; i++)
            {
                if (value[i].Contains("GIA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        string _daily = "";
                        if (d.Contains("DLY")) _daily = "1234567";
                        else _daily = d[1].Replace("D", "");
                        string _fromAirp = "";
                        string _toAirp = "";
                        string _Etd = "";
                        if (d[2].Length == 4)
                        {
                            _fromAirp = d[2]; _Etd = d[3].Substring(0, 4); _toAirp = d[4];
                        }
                        else
                        {
                            _fromAirp = d[2].Substring(0, 4); _Etd = d[2].Substring(4, 4); _toAirp = d[3].Substring(0, 4);
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT", new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = _fromAirp,
                            P_TO_AIRP = _toAirp,
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int TWB(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("TWB"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[7]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[8]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[6].Replace("D", "")),
                                P_CRAFT = clsChuanHoaImport.CraftType(d[2].ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[4],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(d[2].ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int QFA(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("FROM"))
                {
                    string[] ax = value[i].Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (SctExportType.Value == "2")
                    {
                        _from = ax[1];
                        _to = ax[3];
                    }
                    else
                    {
                        _from = ax[1] + ax[4];
                        _to = ax[3] + ax[4];
                    }

                }
                else if (value[i].Contains("QFA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (d.Length == 10)
                        {
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2],
                                    P_TO_AIRP = d[8],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = d[8],
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                        else if (d.Length == 11)
                        {
                            _callSign = d[0] + d[1];
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0] + d[1],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[3],
                                    P_TO_AIRP = d[9],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = d[8],
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int BKP(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1] + d[2].Substring(5, 2)),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[4],
                            P_TO_AIRP = d[6],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                            P_ETA = "",
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "O/F",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value
                        }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int SIA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("SIA") || value[i].Contains("SQ"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0] + d[1];
                        if (d.Length == 9)
                        {
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0] + d[1],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[5],
                                    P_TO_AIRP = d[7],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                        else
                        {
                            _callSign = d[0];
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[4],
                                    P_TO_AIRP = d[6],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int ABL(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("ABL"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "/" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[7]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[9]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[3],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int KAL(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("KAL"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[4],
                                    P_TO_AIRP = d[6],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CDG(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[3]);


            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("CDG"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2].Substring(0, 4),
                                    P_TO_AIRP = d[5].Substring(0, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CLU(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t", "-", "/", "+1" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[5] + d[7]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[6] + d[7]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[8]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[3],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0, 4)),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }

            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int ABW(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].ToString() != " ")
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-", "+1" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[6]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[7]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[3],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int PAL(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _fromDate = "";
            string _toDate = "";
            string _daily = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    if (SctExportType.Value == "2")
                    {
                        if (value[i].Contains("FROM"))
                        {
                            string[] ax = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                            _fromDate = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
                            _toDate = clsChuanHoaImport.ChuanHoaDateTime(ax[3]);
                            _daily = clsChuanHoaImport.ChuanHoaDayly(ax[0]);
                        }
                        else if (value[i].Contains("PAL"))
                        {
                            string[] d = value[i].Split(new string[] { " ", "\t", "-", "+1", "(", ")", "'" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0];
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                    , new
                                    {
                                        P_CALLSIGN = d[0],
                                        P_FROMDATE = _fromDate,
                                        P_TODATE = _toDate,
                                        P_DAILY = _daily,
                                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                        P_FROM_AIRP = d[2],
                                        P_TO_AIRP = d[5],
                                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(0, 4)),
                                        P_ETA = "",
                                        P_VIA = "",
                                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                        P_PERMTYPE = "O/F",
                                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                        P_PERMNBR = txtPERMNBR.Value,
                                        P_SEASON = ddlSeason.Value,
                                        P_OPER = ddlLoaiImport.Value,
                                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                                        P_VERSION = txtVersion.Value.ToUpper(),
                                        P_REGISTRATION = txtReg.Value
                                    }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    else if (SctExportType.Value == "1" && value[i].Contains("PAL"))
                    {

                        string[] d = value[i].Split(new string[] { " ", "\t", "-", "+1", "(", ")", "'" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[8] + d[9]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[10] + d[11]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[7]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2],
                                    P_TO_AIRP = d[5],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(0, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }

            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int OMA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string _daily = ax[0];
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[2] + ax[3] + ax[4]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[6] + ax[7] + ax[8]);
            for (int i = 1; i < value.Length; i++)
            {
                if (value[i].ToString() != " ")
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[3],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int VGO(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            string[] ax = value[0].Split(new string[] { ":", "-", " ", "–" }, StringSplitOptions.RemoveEmptyEntries);
            if (SctExportType.Value == "2")
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
            }
            else
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1] + ax[2] + ax[3]);
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[4] + ax[5] + ax[6]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                if (value[i].Contains("VGO"))
                {
                    try
                    {
                        if (value[i].Length > 20)
                        {
                            string _daily = "";
                            string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0];
                            for (int j = 7; j < d.Length; j++)
                            {
                                _daily += d[j];
                            }
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                    , new
                                    {
                                        P_CALLSIGN = d[0],
                                        P_FROMDATE = _from,
                                        P_TODATE = _to,
                                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                        P_FROM_AIRP = d[1],
                                        P_TO_AIRP = d[4],
                                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                        P_ETA = "",
                                        P_VIA = txtRoutes.Value,
                                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                        P_PERMTYPE = "O/F",
                                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                        P_PERMNBR = txtPERMNBR.Value,
                                        P_SEASON = ddlSeason.Value,
                                        P_OPER = ddlLoaiImport.Value,
                                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                                        P_VERSION = txtVersion.Value.ToUpper(),
                                        P_REGISTRATION = txtReg.Value
                                    }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int HKE(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _toAirp = "";
            string _fromDate = "";
            string _toDate = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("HKE"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-", "+1" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (d[4].Length == 4)
                        {
                            _toAirp = d[5];
                            _fromDate = clsChuanHoaImport.ChuanHoaDateTime(d[6]);
                            _toDate = clsChuanHoaImport.ChuanHoaDateTime(d[7]);
                        }
                        else
                        {
                            _toAirp = d[4].Substring(4, 4);
                            _fromDate = clsChuanHoaImport.ChuanHoaDateTime(d[5]);
                            _toDate = clsChuanHoaImport.ChuanHoaDateTime(d[6]);
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _fromDate,
                                    P_TODATE = _toDate,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[1].ToUpper()),
                                    P_FROM_AIRP = d[3].Substring(0, 4),
                                    P_TO_AIRP = _toAirp,
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int SLK(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("SLK"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-", "–" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_REG = d[1],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1] + DateTime.Now.Year),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2] + "2019"),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[4],
                                    P_TO_AIRP = d[6],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int THA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " ", ":" }, StringSplitOptions.RemoveEmptyEntries);
            string _from = ax[1];
            string _to = ax[3];
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("THA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "/" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (d.Length == 6)
                        {
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[2].ToUpper()),
                                    P_FROM_AIRP = d[3].Substring(0, 4),
                                    P_TO_AIRP = d[4].Substring(0, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            int kq1 = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[2].ToUpper()),
                                    P_FROM_AIRP = d[4].Substring(0, 4),
                                    P_TO_AIRP = d[5].Substring(4, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5].Substring(0, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                            if (kq1 > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                        else
                        {
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[2].ToUpper()),
                                    P_FROM_AIRP = d[3].Substring(0, 4),
                                    P_TO_AIRP = d[4].Substring(0, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int SAA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            string _from = ax[3];
            string _to = ax[4] + ax[5] + ax[6];
            for (int i = 1; i < value.Length; i++)
            {
                if (value[i].Contains("SAA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                    P_DAILY = "1234567",
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[3],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                    P_ETA = "",
                                    P_VIA = txtRoutes.Value,
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int JJA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0] + d[1];
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0] + d[1],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2] + DateTime.Now.Year),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3] + DateTime.Now.Year),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[5],
                                P_TO_AIRP = d[7],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int JAL(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "–", "/", "\t", "STD ", "STA " }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[1];
                    if (SctExportType.Value == "2")
                    {
                        _from = clsChuanHoaImport.ChuanHoaDateTime(d[2]);
                        _to = clsChuanHoaImport.ChuanHoaDateTime(d[3]);
                    }
                    else
                    {
                        _from = clsChuanHoaImport.ChuanHoaDateTime(d[2] + "2018");
                        _to = clsChuanHoaImport.ChuanHoaDateTime(d[3] + "2018");
                    }
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[1],
                                P_FROMDATE = _from,
                                P_TODATE = _to,
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[4]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[5],
                                P_TO_AIRP = d[7],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                    if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int SVA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("FROM") || value[i].Contains("Eff"))
                {
                    string[] ax = value[i].Split(new string[] { " ", "–", "-", "." }, StringSplitOptions.RemoveEmptyEntries);
                    if (SctExportType.Value == "2")
                    {
                        _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
                        _to = clsChuanHoaImport.ChuanHoaDateTime(ax[3] + ax[4]);
                    }
                    else
                    {
                        _from = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[1] + ax[2] + ax[3]);
                        _to = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[5] + ax[6] + ax[7]);
                    }
                }
                else if (value[i].Contains("SVA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "–", "/ EV ", "/EV", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0] + d[1];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0] + d[1],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[4],
                                    P_TO_AIRP = d[7],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int GEC(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[value.Length - 1].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("GEC"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "/t" }, StringSplitOptions.RemoveEmptyEntries);
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1].Replace("D", "")),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[2],
                                    P_TO_AIRP = d[4],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int THD(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    if (SctExportType.Value == "1" && value[i].Contains("THD"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[5] + DateTime.Now.Year),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[6] + DateTime.Now.Year),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1].Substring(0, 4),
                                    P_TO_AIRP = d[2].Substring(0, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[1].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    else if (SctExportType.Value == "2")
                    {
                        if (value[i].Contains("FROM"))
                        {
                            string[] ax = value[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            _from = ax[1]; _to = ax[3];
                        }
                        else if (value[i].Contains("THD"))
                        {
                            string[] d = value[i].Split(new string[] { " ", "\t", "-", "/" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0];
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                    , new
                                    {
                                        P_CALLSIGN = d[0],
                                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[5]),
                                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                        P_FROM_AIRP = d[1],
                                        P_TO_AIRP = d[3],
                                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                        P_ETA = "",
                                        P_VIA = "",
                                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                        P_PERMTYPE = "O/F",
                                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                        P_PERMNBR = txtPERMNBR.Value,
                                        P_SEASON = ddlSeason.Value,
                                        P_OPER = ddlLoaiImport.Value,
                                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                                        P_VERSION = txtVersion.Value.ToUpper(),
                                        P_REGISTRATION = txtReg.Value
                                    }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }

            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int ANA(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    if (SctExportType.Value == "1" && value[i].Contains("ANA"))
                    {
                        if (!value[i].Contains("NOTE"))
                        {
                            string[] d = value[i].Split(new string[] { " ", "\t", "-", "+1", "　　　　", "　　　" }, StringSplitOptions.RemoveEmptyEntries);
                            _callSign = d[0];
                            string _from = "";
                            string _to = "";
                            if (d.Length == 13) { _from = d[11]; _to = d[12]; }
                            else { _from = d[10]; _to = d[11]; }
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                    , new
                                    {
                                        P_CALLSIGN = d[0],
                                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from + DateTime.Now.Year),
                                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to + DateTime.Now.Year),
                                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                        P_FROM_AIRP = d[2],
                                        P_TO_AIRP = d[8],
                                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                                        P_ETA = "",
                                        P_VIA = "",
                                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                        P_PERMTYPE = "O/F",
                                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                        P_PERMNBR = txtPERMNBR.Value,
                                        P_SEASON = ddlSeason.Value,
                                        P_OPER = ddlLoaiImport.Value,
                                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                                        P_VERSION = txtVersion.Value.ToUpper(),
                                        P_REGISTRATION = txtReg.Value
                                    }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    else if (SctExportType.Value == "2" && value[i].Contains("NH") && !value[i].Contains("NOTE"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "–", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                    , new
                                    {
                                        P_CALLSIGN = d[0],
                                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1] + DateTime.Now.Year),
                                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2] + "2019"),
                                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                        P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                        P_FROM_AIRP = d[4],
                                        P_TO_AIRP = d[6],
                                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                        P_ETA = "",
                                        P_VIA = "",
                                        P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                        P_PERMTYPE = "O/F",
                                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                        P_PERMNBR = txtPERMNBR.Value,
                                        P_SEASON = ddlSeason.Value,
                                        P_OPER = ddlLoaiImport.Value,
                                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                                        P_VERSION = txtVersion.Value.ToUpper(),
                                        P_REGISTRATION = txtReg.Value
                                    }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int KHV(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("KHV"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1].Substring(0, 7)),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[1].Substring(8, 7)),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[3],
                                    P_TO_AIRP = d[5],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                    P_ETA = "",
                                    P_VIA = d[7],
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int UPS(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("UPS"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-", "", "//", "(", ")", "DAY", ":" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[5] + DateTime.Now.Year),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[6] + "2019"),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[8]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(d[7].ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[3],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(d[7].ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int TLM(string[] value)
        {
            int iCount = 0;
            string _from = "";
            string _to = "";
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("EFF"))
                {
                    string[] ax = value[i].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                    if (ax.Length == 4)
                    {
                        _from = ax[2]; _to = ax[3];
                    }
                    else
                    {
                        _from = ax[1];
                        _to = ax[2];
                    }
                }
                else if (value[i].Contains("TLM"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "-", "/", "==", "==D" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_from),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_to),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[6]),
                                P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[2],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                P_ETA = "",
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                P_PERMTYPE = "O/F",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_REGISTRATION = txtReg.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CLX(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _remark = "";
            string _from = "";
            string _to = "";
            string _toAirp = "";
            string _Etd = "";
            string[] ax = value[0].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (SctExportType.Value == "2")
            {
                _from = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[0].Replace(" ", ""));
                _to = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[1]);
            }
            else
            {
                _from = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[0]);
                _to = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[1]);
            }
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("CLX"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[1];
                        if (d.Length == 8)
                        {
                            if (d[3].Contains("/")) { _toAirp = d[7]; _Etd = d[5]; _remark = "FROM :" + d[4]; }
                            else if (d[6].Contains("/")) { _toAirp = d[6].Substring(0, 4); _Etd = d[4]; _remark = "TO :" + d[7]; }
                        }
                        else if (d.Length == 9)
                        {
                            _remark = d[4] + "/" + d[8]; _Etd = d[5]; _toAirp = d[7].Substring(0, 4);
                        }
                        else { _toAirp = d[6]; _Etd = d[4]; }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[1],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[0].ToUpper()),
                                    P_FROM_AIRP = d[3].Substring(0, 4),
                                    P_TO_AIRP = _toAirp,
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = _remark,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int FDX(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            string[] ax = value[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (SctExportType.Value == "2")
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[3]);
            }
            else
            {
                _from = clsChuanHoaImport.ChuanHoaDateTime(ax[0]);
                _to = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("FDX"))
                {
                    try
                    {
                        string _Etd = "";
                        string _fromAirp = "";
                        string _toAirp = "";
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (d.Length == 4) { _fromAirp = d[2].Substring(0, 4); _Etd = d[2].Substring(4, 4); _toAirp = d[3].Substring(4, 4); }
                        else if (d.Length == 5) { _fromAirp = d[3].Substring(0, 4); _Etd = d[3].Substring(4, 4); _toAirp = d[4].Substring(4, 4); }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = _fromAirp,
                                    P_TO_AIRP = _toAirp,
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CPA(string[] value)
        {
            int iCount = 0; string via = "";
            string _error = "";
            string _callSign = "";
            for (int i = value.Length - 1; i >= 0; i--)
            {
                if (value[i].Contains("ROUTING"))
                    via = GetViaByRow(value[i]);
                if (value[i].Contains("CPA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "+1", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[5]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[6]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[8]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[7].ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[3],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                    P_ETA = "",
                                    P_VIA = via,
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int MAU(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("MK") || value[i].Contains("MAU"))
                {
                    try
                    {
                        string _daily = "";
                        string _fromAirp = "";
                        string _toAirp = "";
                        string _craft = "";
                        string _Etd = "";
                        string _Reg = "";
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        switch (d.Length.ToString())
                        {
                            case "11":
                                _daily = d[3];
                                _craft = d[4];
                                _fromAirp = d[6];
                                _Etd = d[7];
                                _toAirp = d[8];
                                _Reg = d[10];
                                break;
                            case "12":
                                _daily = d[3] + d[4];
                                _craft = d[5];
                                _fromAirp = d[7];
                                _Etd = d[8];
                                _toAirp = d[9];
                                _Reg = d[11];
                                break;
                            case "13":
                                _daily = d[3] + d[4] + d[5];
                                _craft = d[6];
                                _fromAirp = d[8];
                                _Etd = d[9];
                                _toAirp = d[10];
                                _Reg = d[12];
                                break;
                            case "14":
                                _daily = d[3] + d[4] + d[5] + d[6];
                                _craft = d[7];
                                _fromAirp = d[9];
                                _Etd = d[10];
                                _toAirp = d[11];
                                _Reg = d[13];
                                break;
                            case "15":
                                _daily = d[3] + d[4] + d[5] + d[6] + d[7];
                                _craft = d[8];
                                _fromAirp = d[10];
                                _Etd = d[11];
                                _toAirp = d[12];
                                _Reg = d[14];
                                break;
                            case "16":
                                _daily = d[3] + d[4] + d[5] + d[6] + d[7] + d[8];
                                _craft = d[9];
                                _fromAirp = d[11];
                                _Etd = d[12];
                                _toAirp = d[13];
                                _Reg = d[15];
                                break;
                            case "17":
                                _daily = d[3] + d[4] + d[5] + d[6] + d[7] + d[8] + d[9];
                                _craft = d[10];
                                _fromAirp = d[12];
                                _Etd = d[13];
                                _toAirp = d[14];
                                _Reg = d[16];
                                break;
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(_craft.ToUpper()),
                                    P_FROM_AIRP = _fromAirp,
                                    P_TO_AIRP = _toAirp,
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_Etd),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = _Reg
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int UAL(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("UAL"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8].ToUpper()),
                                    P_FROM_AIRP = d[4],
                                    P_TO_AIRP = d[6],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        private string CPA_ChuanHoa(string[] value)
        {
            string kq = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("PART"))
                {
                    value[i] = "";
                }

            }
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("CPA"))
                    kq += $"\r\n{value[i]}";
                else if (value[i].Contains("ROUTING"))
                    kq += $"\r\n{value[i]}";
                else kq += " " + value[i];
            }
            return kq;
        }
        private string HDA_ChuanHoa(string[] value)
        {
            string kq = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("PART"))
                {
                    value[i] = "";
                }

            }
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("HDA"))
                    kq += $"\r\n{value[i]}";
                else if (value[i].Contains("ROUTING"))
                    kq += $"\r\n{value[i]}";
                else kq += " " + value[i];
            }
            return kq;
        }
        public int HDA(string[] value)
        {
            int iCount = 0; string via = "";
            string _error = "";
            string _callSign = "";
            for (int i = value.Length - 1; i >= 0; i--)
            {
                if (value[i].Contains("ROUTING"))
                    via = GetViaByRow(value[i]);
                if (value[i].Contains("HDA"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "+1", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[5]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[6]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[8]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[7].ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[3],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                    P_ETA = "",
                                    P_VIA = via,
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int ICV(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string[] ax = value[0].Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            string _from = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[0] + ax[1] + ax[2]);
            string _to = clsChuanHoaImport.ChuanHoaDateTimeFull(ax[3] + ax[4] + ax[5]);
            for (int i = value.Length; i == 0; i++)
            {
                if (value[i].Contains("ICV"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[1];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[1],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[0].ToUpper()),
                                    P_FROM_AIRP = d[3],
                                    P_TO_AIRP = d[6].Substring(0, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = txtRemark.Value,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int LAO(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("LAO"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[7]),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[8]),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[3].Substring(0, 4),
                                    P_TO_AIRP = d[4].Substring(4, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = txtRoutes.Value,
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int CRK(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _from = "";
            string _to = "";
            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    if (value[i].Contains("-"))
                    {
                        string[] ax = value[0].Split(new string[] { " ", "-", "." }, StringSplitOptions.RemoveEmptyEntries);
                        if (ax.Length == 4)
                        {
                            _from = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
                            _to = clsChuanHoaImport.ChuanHoaDateTime(ax[3]);
                        }
                        else if (ax.Length == 3)
                        {
                            _from = clsChuanHoaImport.ChuanHoaDateTime(ax[1]);
                            _to = clsChuanHoaImport.ChuanHoaDateTime(ax[2]);
                        }

                    }
                    else if (SctExportType.Value == "1" && value[i].Contains("CRK"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "//ADD//" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        string _fromAirp = "";
                        string _toAirp = "";
                        string _Craft = "";
                        string _remark = "";
                        string _Etd = "";
                        string _daily = "";
                        if (d.Length == 5)
                        {
                            _fromAirp = d[3].Substring(0, 4);
                            _toAirp = d[4].Substring(d[4].Length - 4, 4);
                            _Craft = clsChuanHoaImport.CraftType(d[1].ToUpper());
                            _remark = clsChuanHoaImport.Remark_Craft(d[1].ToUpper());
                            _Etd = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(4, 4));
                            _daily = clsChuanHoaImport.ChuanHoaDayly(d[2]);
                        }
                        else
                        {
                            _fromAirp = d[2].Substring(0, 4);
                            _toAirp = d[3].Substring(4, 4);
                            _Craft = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper());
                            _remark = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper());
                            _Etd = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(4, 4));
                            _daily = clsChuanHoaImport.ChuanHoaDayly(d[1]);
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = _daily,
                                    P_CRAFT = _Craft,
                                    P_FROM_AIRP = _fromAirp,
                                    P_TO_AIRP = _toAirp,
                                    P_ETD = _Etd,
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = _remark,
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    else if (SctExportType.Value == "2" && value[i].Contains("CRK"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "//ADD//", "(+1)" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[2]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[3].Substring(0, 4),
                                    P_TO_AIRP = d[4].Substring(4, 4),
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(4, 4)),
                                    P_ETA = "",
                                    P_VIA = "",
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {
                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int HKC(string[] value)
        {
            int iCount = 0;
            string _error = "";
            string _callSign = "";
            string _via = "";
            string _from = "";
            string _to = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (SctExportType.Value == "2")
                {
                    string[] ax = value[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (value[i].Contains("RTE"))
                    {
                        _via = ax[1];
                    }
                }
                else _via = txtRoutes.Value;

                if (value[i].Contains("HKC"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "-", "–", "(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        if (SctExportType.Value == "2")
                        {
                            _from = clsChuanHoaImport.ChuanHoaDateTime(d[7]);
                            _to = clsChuanHoaImport.ChuanHoaDateTime(d[8]);
                        }
                        else
                        {
                            _from = clsChuanHoaImport.ChuanHoaDateTime(d[7] + "2018");
                            _to = clsChuanHoaImport.ChuanHoaDateTime(d[8] + "2018");
                        }
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = _from,
                                    P_TODATE = _to,
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[9].Replace("D", "")),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[1],
                                    P_TO_AIRP = d[4],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[3].Substring(0, 4)),
                                    P_ETA = "",
                                    P_VIA = _via,
                                    P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                                    P_PERMTYPE = "O/F",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_REGISTRATION = txtReg.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
    }
}
