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

namespace prjApplication.Tool
{
    public partial class ImportsPermSC_LDQ : System.Web.UI.Page
    {
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
            #region Insert VIa
            
           // string[] TextVia = ResultText(txt.Value);
            #endregion

            #region code old
            string[] TextObject = ResultText(txtContent.Value);
            if (TextObject == null) { this.AlertMessage("Not value!"); return; };
            if (String.IsNullOrEmpty(txtPERMNBR.Value)) { this.AlertMessage("PERMNBR Not value!"); return; };
            try
            {
                switch (ddlLoaiImport.Value)
                {
                    case "UW":
                        this.AlertMessage("Sussess: " + UW(TextObject).ToString());
                        break;

                    case "HVN":
                        this.AlertMessage("Sussess: " + HVN(TextObject).ToString());
                        break;
                    case "PIC":
                        this.AlertMessage("Susses: " + PIC(TextObject).ToString());
                        break;
                    case "VJC":
                        this.AlertMessage("Susses: " + VJC(TextObject).ToString());
                        break;
                    case "KHV":
                        this.AlertMessage("Susses: " + KHV(TextObject).ToString());
                        break;
                    case "VFC":
                        this.AlertMessage("Success:" + VFC(TextObject).ToString());
                        break;
                    case "CPA":
                        this.AlertMessage("Success: " + CPA(TextObject).ToString());
                        break;
                    case "KA":
                        this.AlertMessage("Success:" + KA(TextObject).ToString());
                        break;
                    case "HKE":
                        this.AlertMessage("Susses:" + HKE(TextObject).ToString());
                        break;
                    case "QTR":
                        this.AlertMessage("Susses:" + QTR(TextObject).ToString());
                        break;
                    case "CZ":
                        this.AlertMessage("Success:" + CZ(TextObject).ToString());
                        break;
                    case "MAS":
                        this.AlertMessage("Success:" + MAS(TextObject).ToString());
                        break;
                    case "AXM":
                        this.AlertMessage("Success:" + AXM(TextObject).ToString());
                        break;
                    case "KRL":
                        this.AlertMessage("Success:" + KRL(TextObject).ToString());
                        break;
                    case "FX":
                        this.AlertMessage("Success:" + FX(TextObject).ToString());
                        break;
                    case "CES":
                        this.AlertMessage("Success:" + CES(TextObject).ToString());
                        break;
                    case "CSN":
                        this.AlertMessage("Success:" + CSN(TextObject).ToString());
                        break;
                    case "HDA":
                        this.AlertMessage("Success:" + HDA(TextObject).ToString());
                        break;
                    case "KAL":
                        this.AlertMessage("Success:" + KAL(TextObject).ToString());
                        break;
                    case "SIA":
                        this.AlertMessage("Success:" + SIA(TextObject).ToString());
                        break;
                    case "SQC":
                        this.AlertMessage("Success:" + SQC(TextObject).ToString());
                        break;
                    case "TWG":
                        this.AlertMessage("Success:" + TWG(TextObject).ToString());
                        break;
                    case "UAE":
                        this.AlertMessage("Success:" + UAE(TextObject).ToString());
                        break;
                    case "THA":
                        this.AlertMessage("Success:" + THA(TextObject).ToString());
                        break;
                    case "CLX":
                        this.AlertMessage("Success:" + CLX(TextObject).ToString());
                        break;
                    case "CPA1":
                        this.AlertMessage("Success:" + CPA1(TextObject).ToString());
                        break;
                    case "QTR1":
                        this.AlertMessage("Success:" + QTR1(TextObject).ToString());
                        break;
                    case "CPASM":
                        this.AlertMessage("Success:" + CPASM(TextObject).ToString());
                        break;

                    case "AXM_WORD":
                        this.AlertMessage("Success:" + AXM_WORD(TextObject).ToString());
                        break;
                    case "CAL":
                        this.AlertMessage("Success:" + CAL(TextObject).ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                var ax = ex;
                this.AlertMessage("Error insert type!");
            }
            #endregion



        }
        /// <summary>
        /// Lấy đường bay theo hãng
        /// </summary>
        /// <param name="_textinput"></param>
        public string Via4text(string _textinput,string _from,string _to)
        {
            //SZX-KUL:EXOTOL642 ESPOB
            //KUL - SZX:DUDIS? M771 34BC
            //VVTS-VVCM	AT7	W16 W18
            string route = _from + '-' + _to;            
            string via = "";
                   
            string[] value = ResultText(_textinput);
            switch (ddlLoaiImport.Value)
            {
                case "UW":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                        if (d[0].ToString().Replace("  ", string.Empty) == route)
                            via = d[1].ToString();
                    }
                    break;
            }
                      
            return via;
        }
        
        //public void InsertVia(string route, string craft,string via,string oper)
        //{
        //    ViaDAL _objDAL = new ViaDAL();
        //    Via _obj = new Via();
        //    _obj.CRAFT_TYPE = craft;
        //    _obj.SOHIEU = route;
        //    _obj.FROM_AIRP = route.Split('-')[0];
        //    _obj.TO_AIRP = route.Split('-')[1];
        //    _obj.OPER = oper;
        //    _objDAL.InsertVia(_obj);
        //}
        private string[] ResultText(string value)
        {
            if (string.IsNullOrEmpty(value.Trim())) return null;
            return value.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
        #region LD

        public int HVN(string[] value)
        {

            int iCount = 0;
            string remark = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("Đường bay"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[2];
                }
                else
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    
                    string time = "";
                    if (d.Length == 12)
                        time = clsChuanHoaImport.ChuanHoaGioBay(d[8]) + d[9];
                    else
                        time = clsChuanHoaImport.ChuanHoaGioBay(d[8]);

                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                    , new
                    {
                        P_CALLSIGN = d[0],//1
                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),//2
                        P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),//3
                        P_FROM_AIRP = d[5],//4
                        P_TO_AIRP = d[7],//5
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                        P_ETA = time,
                        P_VIA = "",
                        P_REMARK = "",
                        P_PERMTYPE = ddlPERMTYPE.Value,//10
                        P_PERMNBR = txtPERMNBR.Value,//11
                        P_SEASON = ddlSeason.Value,//9
                        P_OPER = ddlLoaiImport.Value,//8
                        P_AUTHOR = ddlAUTHOR.SelectedValue,//7
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value)//6
                         ,
                        P_VERSION = txtVersion.Value,//12
                        P_PURPOSE = ddlPURPOSE.SelectedValue//13
                    }).ToString());
                    if (kq > 0) iCount++;
                }
            }
            return iCount;
        }
        public int HAN(string[] value)
        {
            int iCount = 0;
            string remark = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("Đường bay"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[2];
                }
                else
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    string time = "";
                    if (d.Length == 12)
                        time = clsChuanHoaImport.ChuanHoaGioBay(d[8]) + d[9];
                    else
                        time = clsChuanHoaImport.ChuanHoaGioBay(d[8]);

                    int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                    , new
                    {
                        P_CALLSIGN = d[0],
                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                        P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                        P_FROM_AIRP = d[5],
                        P_TO_AIRP = d[7],
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                        P_ETA = time,
                        P_VIA = "",
                        P_REMARK = "",
                        P_PERMTYPE = ddlPERMTYPE.Value,
                        P_PERMNBR = txtPERMNBR.Value,
                        P_SEASON = ddlSeason.Value,
                        P_OPER = ddlLoaiImport.Value,
                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                        P_PURPOSE = ddlPURPOSE.SelectedValue
                    }).ToString());
                    if (kq > 0) iCount++;
                }
            }
            return iCount;
        }

        public int PIC(string[] value)
        {
            int iCount = 0;
            string remark = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("Route"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[2];
                }
                else if (value[i].Contains("ĐƯỜNG BAY"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[3];
                }
                else if (value[i].Contains("Đường bay"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[3];
                }
                else
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    if (d.Length == 12)
                    {
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[2],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[4]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[5])),
                            P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[10]),
                            P_FROM_AIRP = d[6],
                            P_TO_AIRP = d[8],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[9]),
                            P_VIA = "",
                            P_REMARK = "",
                            P_PERMTYPE = ddlPERMTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue
                             ,
                            P_VERSION = txtVersion.Value,//12
                        }).ToString());
                        if (kq > 0) iCount++;
                    }
                    else
                    {
                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                            P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                            P_FROM_AIRP = d[4],
                            P_TO_AIRP = d[6],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                            P_VIA = "",
                            P_REMARK = "",
                            P_PERMTYPE = ddlPERMTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue
                             ,
                            P_VERSION = txtVersion.Value,//12
                        }).ToString());
                        if (kq > 0) iCount++;
                    }

                }
            }
            return iCount;
        }
        private int VN_LD_2753NOV2017VN(string[] value)
        {
            int iCount = 0;
            string remark = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).Length < 8)
                {
                    remark = value[i].Trim(new char[] { '\t', ' ' }).Substring(2);
                }
                else
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = "",// clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_TODATE = "",// clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                            P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                            P_FROM_AIRP = d[4],
                            P_TO_AIRP = d[5],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                            P_VIA = remark,
                            P_REMARK = d[9],
                            P_PERMTYPE = "LD",
                            P_PURPOSE = ddlPURPOSE.SelectedValue
                        }).ToString());
                    if (kq > 0) iCount++;
                }
            }
            return iCount;
        }
        private int VJC(string[] value)
        {
            int iCount = 0;
            int sRow = 0;
            string remark = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Length < 40)
                {
                    //remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[2];
                    remark = value[i].Substring(value[i].IndexOf('-', 1));
                }
                else
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                            P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                            P_FROM_AIRP = d[4],
                            P_TO_AIRP = d[5],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                            P_VIA = "",
                            P_REMARK = "",
                            P_PERMTYPE = ddlPERMTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value
                        }).ToString());
                    if (kq > 0) iCount++;
                    sRow++;
                }
            }
            return iCount;
        }
        public int VFC(string[] value)
        {
            int iCount = 0;
            string remark = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("Đường bay"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[2];
                }
                else
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    //string time = "";
                    //if (d.Length == 12)
                    //    time = clsChuanHoaImport.ChuanHoaGioBay(d[8]) + d[9];
                    //else
                    //    time = clsChuanHoaImport.ChuanHoaGioBay(d[8]);

                    int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                    , new
                    {
                        P_CALLSIGN = d[0],
                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                        P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                        P_FROM_AIRP = d[5],
                        P_TO_AIRP = d[7],
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                        P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                        P_VIA = "",
                        P_REMARK = "",
                        P_PERMTYPE = ddlPERMTYPE.Value,
                        P_PERMNBR = txtPERMNBR.Value,
                        P_SEASON = ddlSeason.Value,
                        P_OPER = ddlLoaiImport.Value,
                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                        P_VERSION = txtVersion.Value.ToUpper()
                    }).ToString());
                    if (kq > 0) iCount++;
                }
            }
            return iCount;
        }
        #endregion



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
        public int KHV(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int HKE(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = "",
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int CZ(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = d[8],
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int QTR(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int MAS(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int AXM(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }

        public int CPA(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int KA(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int KRL(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = "",
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int FX(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }

        public int CES(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                if (d.Length == 11)
                {
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = d[10],
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                    if (kq > 0) iCount++;
                }
                else
                {
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = d[9],
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                    if (kq > 0) iCount++;
                }
                               
            }
            return iCount;
        }

        public int CSN(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value)
                     ,
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }

        public int HDA(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value)
                    ,
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }


        public int KAL(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = "",
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }

        public int SIA(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = "",
                    P_FROM_AIRP = d[4],
                    P_TO_AIRP = d[6],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int SQC(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }

        public int TWG(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }

        public int UAE(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int THA(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                if (d.Length == 10)
                {
                    int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                    if (kq > 0) iCount++;
                }
                else
                {
                    int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                    , new
                    {
                        P_CALLSIGN = d[0],
                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                        P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                        P_FROM_AIRP = d[5],
                        P_TO_AIRP = d[7],
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                        P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                        P_VIA = "",
                        P_REMARK = "",
                        P_PERMTYPE = ddlPERMTYPE.Value,
                        P_PERMNBR = txtPERMNBR.Value,
                        P_SEASON = ddlSeason.Value,
                        P_OPER = ddlLoaiImport.Value,
                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                        P_VERSION = txtVersion.Value
                    }).ToString());
                    if (kq > 0) iCount++;
                }
            }
            return iCount;
        }
        public int CLX(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int CPA1(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int QTR1(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        //Thai Add mau mua he 27/03/2018
        public int CPASM(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int AXM_WORD(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime2(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime2(d[3]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[10].Replace("DAILY", "1234567"))),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[8],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6].Substring(0,4)),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[9].Substring(0,4)),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }
        public int CAL(string[] value)
        {
            int iCount = 0;

            for (int i = 0; i < value.Length; i++)
            {

                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[4]),
                    P_FROM_AIRP = d[5],
                    P_TO_AIRP = d[7],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[8]),
                    P_VIA = "",
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;

            }
            return iCount;
        }

        public int UW(string[] value)
        {
            int iCount = 0;
            string via = "";
            string[] d0; string[] d1;
            string[] d00; 

            d0 = value[0].Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            string _to = clsChuanHoaImport.ChuanHoaDateTime(d0[1]);
            d00 = value[0].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            string _from  = clsChuanHoaImport.ChuanHoaDateTime(d00[1]);
            d1 = value[1].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 2; i < value.Length; i++)
            {
                string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                via = Via4text(txtRoutes.Value, d[1], d[4]);                                
                int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                , new
                {
                    P_CALLSIGN = d[0],
                    P_FROMDATE = _from,
                    P_TODATE = _to,
                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d1[2]),
                    P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(txtCraft.Text),
                    P_FROM_AIRP = d[1],
                    P_TO_AIRP = d[4],
                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[3]),
                    P_VIA = via,
                    P_REMARK = "",
                    P_PERMTYPE = ddlPERMTYPE.Value,
                    P_PERMNBR = txtPERMNBR.Value,
                    P_SEASON = ddlSeason.Value,
                    P_OPER = ddlLoaiImport.Value,
                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                    P_VERSION = txtVersion.Value
                }).ToString());
                if (kq > 0) iCount++;
            }
            
             
            return iCount;
        }
    }
}