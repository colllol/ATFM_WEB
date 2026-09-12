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
    public partial class ImportsPermSC_LD : System.Web.UI.Page
    {
        private List<Aero> lisAero { get { return new AeroDAL().GetListAll(); } }
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
            // InserVia4text(txtvia.Value);

            #endregion



            #region code old
            string[] TextObject = ResultText(txtContent.Value);
            if (TextObject == null) { this.AlertMessage("Not value!"); return; };
            if (String.IsNullOrEmpty(txtPERMNBR.Value)) { this.AlertMessage("PERMNBR Not value!"); return; };
            try
            {
                switch (ddlLoaiImport.Value)
                {
                    case "HVN":
                        this.AlertMessage("Sussess: " + HVN(TextObject).ToString());
                        break;
                    case "PIC":
                        this.AlertMessage("Susses: " + PIC(TextObject).ToString());
                        break;
                    case "VJC":
                        this.AlertMessage("Susses: " + VJC(TextObject).ToString());
                        break;
                    case "VFC":
                        this.AlertMessage("Success:" + VFC(TextObject).ToString());
                        break;
                    case "BAV":
                        this.AlertMessage("Success:" + BAV(TextObject).ToString());
                        break;
                    case "ABW":
                        this.AlertMessage("Success:" + ABW(TextObject).ToString());
                        break;
                    case "ALL_OPER":
                        this.AlertMessage("Success:" + ALL_OPER(TextObject).ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                var ax = ex;
                this.AlertMessage("Error insert type!");
            }
            #region xu ly huy chuyen
            if (ddlAction.Value == "HuyChuyen")
            {
                btnTimHuyChuyen_Click(sender, e);
            }
            #endregion
            if (ddlAction.Value == "ThayDoi")
            {
                //btnTimThayDoi_Click(sender, e);
                switch (ddlLoaiImport.Value)
                {
                    case "VJC":
                        //txtRemark.InnerText = "PHEP THAY ĐỔI";
                        this.AlertMessage("Susses: " + VJC_THAYDOI(TextObject).ToString());
                        break;
                }
                
            }
            #region xu ly thay doi

            #endregion

            #endregion
        }

        public string ReturnCraft(string _craft)
        {
            string _kq = "";
            switch (_craft.Trim())
            {
                case "321":
                    _kq = "A321";
                    break;
                case "320":
                    _kq = "A320";
                    break;
                case "319":
                    _kq = "A319";
                    break;
                case "787":
                    _kq = "B787";
                    break;
                default:
                    _kq = _craft;
                    break;

            }
            return _kq;

        }

        public string Via4text(string _textinput, string _from, string _to)
        {
            string route = _from + '-' + _to;
            string route1 = _from + '-' + _to;
            string via = "";

            string[] value = ResultText(_textinput);
            switch (ddlLoaiImport.Value)
            {
                case "VJC":
                    for (int i = 0; i < value.Length; i++)
                    {
                        string[] d = value[i].Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
                        string via1 = "";
                        for (int j = 1; j < d.Length; j++)
                        {
                            via1 += d[j] + "/";
                        }
                        if (d[0].ToString().Replace("  ", string.Empty) == route1) via = via1.Replace("OR", "");
                    }
                    break;
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
            }

            return via;
        }

        private string[] ResultText(string value)
        {
            if (string.IsNullOrEmpty(value.Trim())) return null;
            return value.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
        #region LD
        public int ALL_OPER(string[] value)
        {
            int iCount = 0;
            string _error = ""; string _callSign = ""; string _from = ""; string _to = ""; string _craft = "";
            string _daily = ""; string _etd = ""; string _eta = ""; string _fromDate = "";
            string _toDate = ""; string _route = ""; string _remark = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    if (d.Length == 9)
                    {
                        _fromDate = d[1]; _toDate = d[2]; _daily = clsChuanHoaImport.ChuanHoaDayly(d[3]);
                        //_craft = d[8].Split('/')[0];
                        _craft = txtCraft.Value;

                        _from = d[4]; _etd = d[5]; _eta = d[7].Trim();
                        _to = d[6]; _route = txtRoutes.Value; _remark = clsChuanHoaImport.Remark_Craft(d[8]);
                    }
                    else if (d.Length == 10)
                    {
                        _fromDate = d[1]; _toDate = d[2]; _daily = clsChuanHoaImport.ChuanHoaDayly(d[3]);
                        //_craft = d[8].Split('/')[0]; QUYNX SUA
                        _craft = txtCraft.Value;
                        _from = d[4]; _etd = d[5]; _eta = d[7].Trim(); _to = d[6]; _route = d[9];
                    }
                    else
                    {
                        _fromDate = d[1]; _toDate = d[2]; _daily = clsChuanHoaImport.ChuanHoaDayly(d[3]);
                        _craft = txtCraft.Value;
                        _from = d[4]; _etd = d[5]; _eta = d[7].Trim(); _to = d[6]; _route = txtRoutes.Value;
                    }
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT_ALL_OPER"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(_fromDate),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(_toDate),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly(_daily),
                            P_CRAFT = ReturnCraft(clsChuanHoaImport.CraftType(_craft.ToUpper())),
                            P_FROM_AIRP = _from,
                            P_TO_AIRP = _to,
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_etd),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_eta),
                            P_VIA = _route,
                            P_REMARK = _remark,
                            P_PERMTYPE = "LD",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_REGISTRATION = txtReg.Value,
                            //QUYNX SỬA 12082026
                            P_ACTION = ddlAction.Value
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
        public int HVN(string[] value)
        {
            int iCount = 0;
            string remark = "";
            string _Eta = "";
            string _callSign = "";
            string _error = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("Đường bay"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[2];
                }
                else if (value[i].Contains("Đường"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                else if (value[i].Contains("VN"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        int ax = Convert.ToInt32(d[6].Split(':')[0]);
                        int bx = Convert.ToInt32(d[8].Split(':')[0]);
                        if (ax > bx) _Eta = d[8] + "+";
                        else _Eta = d[8];
                        _callSign = d[0];

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                    , new
                    {
                        P_CALLSIGN = d[0],
                        P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                        P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                        P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                        P_CRAFT = ReturnCraft(d[4]),
                        P_FROM_AIRP = d[5],
                        P_TO_AIRP = d[7],
                        P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                        P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                        P_VIA = "",
                        P_REMARK = clsChuanHoaImport.Remark_Craft(d[4]),
                        P_PERMTYPE = "LD",
                        P_PERMNBR = txtPERMNBR.Value,
                        P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                        P_SEASON = ddlSeason.Value,
                        P_OPER = ddlLoaiImport.Value,
                        P_AUTHOR = ddlAUTHOR.SelectedValue,
                        P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                        P_VERSION = txtVersion.Value,
                        P_PURPOSE = ddlPURPOSE.SelectedValue,
                        P_ACTION = ddlAction.Value
                    }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        break;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }

        public int PIC(string[] value)
        {
            int iCount = 0;
            string remark = "";
            string _Eta = "";
            //string aaa = "";
            string _callSign = "";
            string _error = "";

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
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        int ax = Convert.ToInt32(d[5].Split(':')[0]);
                        int bx = Convert.ToInt32(d[7].Split(':')[0]);
                        if (ax > bx) _Eta = d[7] + "+";
                        else _Eta = d[7];
                        #region Sai Chang
                        //var isFrom = lisAero.Any(a => a.AE_CODE == d[4] || a.AE_IATA == d[4]);
                        //var isTo = lisAero.Any(a => a.AE_CODE == d[6] || a.AE_IATA == d[6]);
                        //if (!isFrom || !isTo)
                        //    aaa += $"{d[4]} - {d[6]}. ";
                        #endregion

                        if (d.Length == 12)
                        {
                            int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[2],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[4]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[5])),
                                P_CRAFT = "A" + clsChuanHoaImport.CraftType(d[10]),
                                P_FROM_AIRP = d[6],
                                P_TO_AIRP = d[8],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(d[10]),
                                P_PERMTYPE = "LD",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value,
                                P_ACTION = ddlAction.Value
                            }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
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
                                P_CRAFT = "A" + clsChuanHoaImport.CraftType(d[8]),
                                P_FROM_AIRP = d[4],
                                P_TO_AIRP = d[6],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = "",
                                P_REMARK = clsChuanHoaImport.Remark_Craft(d[8]),
                                P_PERMTYPE = "LD",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value,
                                P_ACTION = ddlAction.Value
                            }).ToString());
                            if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        }
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        break;
                    }

                }
            }
            //if (aaa != "") this.ExcuteJavascript($"$('#lblAERO').text('Khong co san bay: {aaa}');");
            lblLOG.Text = _error;
            return iCount;
        }
        private int BAV(string[] value)
        {
            int iCount = 0;
            string remark = "";
            string _Eta = "";
            string _callSign = "";
            string _error = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("ĐƯỜNG BAY"))
                {
                    remark = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries)[2];
                }
                else if (value[i].Contains("QH"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        int ax = Convert.ToInt32(d[5].Split(':')[0]);
                        int bx = Convert.ToInt32(d[7].Split(':')[0]);
                        if (ax > bx) _Eta = d[7] + "+";
                        else _Eta = d[7];

                        int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0],
                            P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                            P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                            P_DAILY = clsChuanHoaImport.ChuanHoaDayly((d[3])),
                            P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                            P_FROM_AIRP = d[4],
                            P_TO_AIRP = d[6],
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                            P_VIA = "",
                            P_REMARK = clsChuanHoaImport.Remark_Craft(txtCraft.Value.ToUpper()),
                            P_PERMTYPE = "LD",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value,
                            P_ACTION = ddlAction.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        break;
                    }
                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        private int ABW(string[] value)
        {
            int iCount = 0;
            string remark = "";
            string _Eta = "";
            string _callSign = "";
            string _error = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    if (value[i].Contains("RU"))
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "+1" }, StringSplitOptions.RemoveEmptyEntries);
                        int ax = Convert.ToInt32(d[5].Substring(0, 2));
                        int bx = Convert.ToInt32(d[7].Substring(0, 2));
                        if (ax > bx) _Eta = d[7] + "+";
                        else _Eta = d[7];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                                , new
                                {
                                    P_CALLSIGN = d[0],
                                    P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[1].Replace(".", "-")),
                                    P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[2].Replace(".", "-")),
                                    P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[3]),
                                    P_CRAFT = clsChuanHoaImport.CraftType(txtCraft.Value.ToUpper()),
                                    P_FROM_AIRP = d[4],
                                    P_TO_AIRP = d[6],
                                    P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[5]),
                                    P_ETA = clsChuanHoaImport.ChuanHoaGioBay(d[7]),
                                    P_VIA = "",
                                    P_REMARK = "",
                                    P_PERMTYPE = "LD",
                                    P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                    P_PERMNBR = txtPERMNBR.Value,
                                    P_SEASON = ddlSeason.Value,
                                    P_OPER = ddlLoaiImport.Value,
                                    P_AUTHOR = ddlAUTHOR.SelectedValue,
                                    P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                    P_PURPOSE = ddlPURPOSE.SelectedValue,
                                    P_VERSION = txtVersion.Value.ToUpper(),
                                    P_ACTION = ddlAction.Value
                                }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                }
                catch (Exception)
                {

                    _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    lblLOG.Text = _error;
                    break;
                }

            }
            lblLOG.Text = _error;
            return iCount;
        }
        private int VJC(string[] value)
        {
            int iCount = 0;
            string _Eta = "";
            string remark = "";
            string _callSign = "";
            string _error = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Length < 40)
                {
                    remark = value[i].Substring(value[i].IndexOf('-', 1));
                }
                else
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        int ax = Convert.ToInt32(d[6].Split(':')[0]);
                        int bx = Convert.ToInt32(d[7].Split(':')[0]);
                        if (ax > bx) _Eta = d[7] + "+";
                        else _Eta = d[7];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = "A" + clsChuanHoaImport.CraftType(d[8]),
                                P_FROM_AIRP = d[4],
                                P_TO_AIRP = d[5],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = "",
                                P_REMARK = d[8],
                                P_PERMTYPE = "LD",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_ACTION = ddlAction.Value
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {

                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        break;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        private int VJC_THAYDOI(string[] value)
        {
            int iCount = 0;
            string _Eta = "";
            string remark = "";
            string _callSign = "";
            string _error = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Length < 40)
                {
                    remark = value[i].Substring(value[i].IndexOf('-', 1));
                }
                else
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                        int ax = Convert.ToInt32(d[6].Split(':')[0]);
                        int bx = Convert.ToInt32(d[7].Split(':')[0]);
                        if (ax > bx) _Eta = d[7] + "+";
                        else _Eta = d[7];
                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_FROMDATE = clsChuanHoaImport.ChuanHoaDateTime(d[2]),
                                P_TODATE = clsChuanHoaImport.ChuanHoaDateTime(d[3]),
                                P_DAILY = clsChuanHoaImport.ChuanHoaDayly(d[1]),
                                P_CRAFT = "A" + clsChuanHoaImport.CraftType(d[8]),
                                P_FROM_AIRP = d[4],
                                P_TO_AIRP = d[5],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[6]),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = "",
                                P_REMARK = "PHEP THAY DOI",
                                P_PERMTYPE = "LD",
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_SEASON = ddlSeason.Value,
                                P_OPER = ddlLoaiImport.Value,
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_VERSION = txtVersion.Value.ToUpper(),
                                P_ACTION = "TangChuyen"
                            }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {

                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        break;
                    }

                }
            }
            lblLOG.Text = _error;
            return iCount;
        }
        public int VFC(string[] value)
        {
            int iCount = 0;
            string _callSign = "";
            string _error = "";

            for (int i = 0; i < value.Length; i++)
            {
                if (!value[i].Contains("Đường bay"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMSC_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = d[0].ToUpper(),
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
                            P_PERMTYPE = "LD",
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_SEASON = ddlSeason.Value,
                            P_OPER = ddlLoaiImport.Value,
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtENDDATE.Value),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_VERSION = txtVersion.Value.ToUpper(),
                            P_ACTION = ddlAction.Value
                        }).ToString());
                        if (kq > 0) iCount++; else _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                    }
                    catch (Exception)
                    {
                        _error += $"{"Insert lỗi " + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        break;
                    }
                }
            }
            lblLOG.Text = _error;
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
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
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

        #region phep huy chuyen

        protected void btnTimHuyChuyen_Click(object sender, EventArgs e)
        {
            LoadHuyChuyen_IMP();
            DataTable dt = new clsResuftAPI().GetTableApiExtension("PERM_IMP_PKG", "GetPhepBayCoHuyChuyen", null);
            rptKetQuaChuyenHuy.DataSource = dt;
            rptKetQuaChuyenHuy.DataBind();
            this.ExcuteJavascript("ddlAction_Display();");
        }
        protected void btnLenhHuy_Click(object sender, EventArgs e)
        {
            try
            {
                int b = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "impToPerm_Huy", null).ToString());
            }
            catch (Exception ex)
            {  throw ex; };

            //foreach (RepeaterItem r in rptKetQuaChuyenHuy.Items)
            //{
            //    CheckBox chk = (CheckBox)r.FindControl("chkSelectRow");
            //    if (chk.Checked)
            //    {
            //        try
            //        {
            //            string id = chk.Attributes["data-Id"].ToString();
            //            string Loai = chk.Attributes["data-PermType"].ToString();
            //            string thamchieu = chk.Attributes["data-ThamChieu"].ToString();
            //            string fdate = chk.Attributes["data-fDate"].ToString();
            //            string tdate = chk.Attributes["data-tDate"].ToString();
            //            string daily = chk.Attributes["data-daily"].ToString();
            //            int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "Access_LenhHuyChuyen", new { P_IDDETAIL = id, P_PERMTYPE = Loai, P_THAMCHIEU = thamchieu, P_HUYFROMDATE = fdate, P_HUYTODATE = tdate, P_DAILY = daily }).ToString());
            //        }
            //        catch (Exception ex)
            //        {
            //            throw ex;
            //        }
            //    }
            //}

            //rptKetQuaChuyenHuy.DataSource = null;
           // rptKetQuaChuyenHuy.DataBind();
            this.ExcuteJavascript("Action_OnChange(b);");
        }
        private void LoadHuyChuyen_IMP()
        {
            List<PermScIMP> t = new PermScImpDAL().GetPagePermScIMP(10000, 0, " Action = 'HuyChuyen' ");
            grdHuyChuyen.DataSource = t;
            grdHuyChuyen.DataBind();
        }
        protected void btnXoaHuyChuyen_Click(object sender, EventArgs e)
        {
            new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "DeleteChuyenHuy_IMP", null);
            btnTimHuyChuyen_Click(sender, e);
        }

        #endregion

        #region phep thay doi
        protected void btnLenhThayDoi_Click(object sender, EventArgs e)
        {
            int b = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "impToPerm_ThayDoi", null).ToString());
            btnTimThayDoi_Click(sender, e);
        }

        protected void btnTimThayDoi_Click(object sender, EventArgs e)
        {
            LoadThayDoi_IMP();
            DataTable dt = new clsResuftAPI().GetTableApiExtension("PERM_IMP_PKG", "GetPhepBayCoThayDoi", null);
            rptKetQuaChuyenHuy.DataSource = dt;
            rptKetQuaChuyenHuy.DataBind();
            this.ExcuteJavascript("ddlAction_Display();");
        }

        protected void btnXoaThayDoi_Click(object sender, EventArgs e)
        {
            new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "DeleteChuyenHayThayDoi_IMP", null);
            btnTimThayDoi_Click(sender, e);
        }
        private void LoadThayDoi_IMP()
        {
            List<PermScIMP> t = new PermScImpDAL().GetPagePermScIMP(10000, 0, " Action = 'ThayDoi' ");
            grdHuyChuyen.DataSource = t;
            grdHuyChuyen.DataBind();
        }
        #endregion

    }
}