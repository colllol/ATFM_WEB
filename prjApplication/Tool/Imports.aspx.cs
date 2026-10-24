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
    public enum LoaiImportword
    {
        AZURAIR_LLC_LD = 1
        , QATAR_EXECUTIVE_LD = 2
    }


    public partial class Imports : System.Web.UI.Page
    {
        ActionHistoryDAL _AcDAL = new ActionHistoryDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddl_Load();
            }
        }
        public void ddl_Load()
        {
            this.FillDropdownList(ddlAUTHOR, new FpAuthorDAL().GetAllObject(), "AUTHOR_CODE", "AUTHOR_CODE");
            this.FillDropdownList(ddlPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_CODE", "PURPOSE_CODE");
        }
        protected void linkSave_Click(object sender, EventArgs e)
        {
            lblLOG.Text = "";
            string[] TextObject = ResultText(txtContent.Value);
            if (TextObject == null) { this.AlertMessage("Not value!"); return; };
           try
            {
                switch (ddlOPER.Value)
                {
                    case "ABG":
                        this.AlertMessage("Success:" + ABG(TextObject).ToString());
                        break;
                    case "KAR":
                        this.AlertMessage("Success:" + KAR(TextObject).ToString());
                        break;
                    case "KTK":
                        this.AlertMessage("Success:" + KTK(TextObject).ToString());
                        break;
                    case "NWS":
                        this.AlertMessage("Success:" + NWS(TextObject).ToString());
                        break;
                    case "NOK":
                        this.AlertMessage("Success:" + NOK(TextObject).ToString());
                        break;                    
                    case "RMY":
                        this.AlertMessage("Success:" + ALL_OPER(TextObject).ToString());
                        break;                    
                    default:
                        this.AlertMessage("Success:" + ALL_DEFAULT(TextObject).ToString());
                        break;
                }
            }
            catch(Exception ex)
            {
                _AcDAL.InsertLog("IMPORT_PERMNO", DateTime.Now.ToString(), ex.ToString());
                this.AlertMessage("Error Insert Type");
            }
        }

        public int ALL_OPER(string[] value)
        {
            //All Oper theo dinh dang : callsign fromAirp etd eta toAirp dayfly
            int iCount = 0;
            string _error = ""; string _callSign = ""; string _from = ""; string _to = ""; string _craft = ""; 
            string _dayfly = ""; string _etd = ""; string _route = ""; string _Eta = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    if (d.Length == 6)
                    {
                        _dayfly = clsChuanHoaImport.ChuanHoaDateTime(d[5]); _Eta = d[3]; _route = txtRoutes.Value;
                        _craft = txtCRAFTTYPE.Value; _from = d[1]; _etd = d[2]; _to = d[4]; 
                    }
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMNO_IMP_INSERT"
                        , new
                        {
                            P_CALLSIGN = _callSign,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtPermDate.Value),
                            P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(_craft.ToUpper()),
                            P_FROM_AIRP = _from,
                            P_TO_AIRP = _to,
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_etd),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                            P_VIA = _route.ToUpper(),
                            P_REMARK = "",
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_DAYFLY = clsChuanHoaImport.ChuanHoaDateTime(_dayfly),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_PERMTYPE = ddlPERMTYPE.Value,
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_OPER = ddlOPER.Value,
                            P_REGISTRATION = txtREG.Value.ToUpper()
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

        public int ALL_DEFAULT(string[] value)
        {
            //All Oper theo dinh dang : callsign fromAirp etd eta toAirp dayfly
            int iCount = 0;
            string _error = ""; string _callSign = ""; string _from = ""; string _to = ""; string _craft = "";
            string _dayfly = ""; string _etd = ""; string _route = ""; string _Eta = "";

            for (int i = 0; i < value.Length; i++)
            {
                try
                {
                    string[] d = value[i].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    _callSign = d[0];
                    if (d.Length == 6)
                    {
                        _dayfly = clsChuanHoaImport.ChuanHoaDateTime(d[5]); _Eta = d[3]; _route = txtRoutes.Value;
                        _craft = txtCRAFTTYPE.Value; _from = d[1]; _etd = d[2]; _to = d[4];
                    }
                    int kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "PERMNO_IMP_INSERT_ALL"
                        , new
                        {
                            P_CALLSIGN = _callSign,
                            P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtPermDate.Value),
                            P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(_craft.ToUpper()),
                            P_FROM_AIRP = _from,
                            P_TO_AIRP = _to,
                            P_ETD = clsChuanHoaImport.ChuanHoaGioBay(_etd),
                            P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                            P_VIA = _route.ToUpper(),
                            P_REMARK = "",
                            P_AUTHOR = ddlAUTHOR.SelectedValue,
                            P_DAYFLY = clsChuanHoaImport.ChuanHoaDateTime(_dayfly),
                            P_PURPOSE = ddlPURPOSE.SelectedValue,
                            P_PERMNBR = txtPERMNBR.Value,
                            P_PERMTYPE = ddlPERMTYPE.Value,
                            P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                            P_OPER = ddlOPER.Value,
                            P_REGISTRATION = txtREG.Value.ToUpper()
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
        public int ABG(string[] value)
        {
            int iCount = 0;
            string _Eta = "";
            string ccc = "";
            string _callsign = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("ABG") || value[i].Contains("RL"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "(", ")", "+1", "+", "-300", "-200" }, StringSplitOptions.RemoveEmptyEntries);
                        _callsign = d[1];
                        int ax = Convert.ToInt32(d[4].Trim().Substring(0, 2));
                        int bx = Convert.ToInt32(d[7].Trim().Substring(0, 2));
                        if (ax > bx) _Eta = d[7].Trim() + "+"; else _Eta = d[7].Trim();
                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMNO_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtPermDate.Value),
                                P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8].ToUpper()),
                                P_FROM_AIRP = d[3],
                                P_TO_AIRP = d[6],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = txtRoutes.Value.ToUpper(),
                                P_REMARK = "",
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_DAYFLY = clsChuanHoaImport.ChuanHoaDateTime(d[1]),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_PERMTYPE = ddlPERMTYPE.Value,
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_OPER = ddlOPER.Value,
                                P_REGISTRATION = txtREG.Value.ToUpper()
                            }).ToString());
                        if (kq > 0) iCount++;
                    }
                    catch (Exception ex)
                    {
                        int dong = i + 1;
                        ccc += $"{"Insert lỗi dòng " + dong + "---"+ "CallSign " + _callsign + "<br />"}";
                        lblLOG.Text = ccc;
                        _AcDAL.InsertLog("IMPORT_PERMNO_IMP", DateTime.Now.ToString(), ex.ToString());
                    }
                    
                }
            }
            return iCount;
        }
        public int KAR(string[] value)
        {
            int iCount = 0;
            string _Eta = "";
            string _callSign = "";
            string _error = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("KAR"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "+1", "+" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int ax = Convert.ToInt32(d[2].Trim().Substring(0, 2));
                        int bx = Convert.ToInt32(d[3].Trim().Substring(0, 2));
                        if (ax > bx) _Eta = d[3].Trim() + "+"; else _Eta = d[3].Trim();
                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMNO_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtPermDate.Value),
                                P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(txtCRAFTTYPE.Value.ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[4],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = txtRoutes.Value.ToUpper(),
                                P_REMARK = "",
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_DAYFLY = clsChuanHoaImport.ChuanHoaDateTime(d[6].Trim('/')),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_PERMTYPE = ddlPERMTYPE.Value,
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_OPER = ddlOPER.Value,
                                P_REGISTRATION = txtREG.Value.ToUpper()
                            }).ToString());
                        if (kq > 0) iCount++;
                    }
                    catch (Exception ex)
                    {
                        int dong = i + 1;
                        _error += $"{"Insert lỗi dòng " + dong + "---" + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        _AcDAL.InsertLog("IMPORT_PERMNO_IMP", DateTime.Now.ToString(), ex.ToString());
                    }
                    
                }
            }
            return iCount;
        }
        public int KTK(string[] value)
        {
            int iCount = 0;
            string _Eta = "";
            string _callSign = "";
            string _error = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("KTK"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "+1", "(", ")", "+" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[1];
                        int ax = Convert.ToInt32(d[4].Trim().Substring(0, 2));
                        int bx = Convert.ToInt32(d[7].Trim().Substring(0, 2));
                        if (ax > bx) _Eta = d[7].Trim() + "+"; else _Eta = d[7].Trim();
                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMNO_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[1],
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtPermDate.Value),
                                P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(d[8].ToUpper()),
                                P_FROM_AIRP = d[3],
                                P_TO_AIRP = d[6],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[4]),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = txtRoutes.Value.ToUpper(),
                                P_REMARK = "",
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_DAYFLY = clsChuanHoaImport.ChuanHoaDateTime(d[0]),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_PERMTYPE = ddlPERMTYPE.Value,
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_OPER = ddlOPER.Value,
                                P_REGISTRATION = txtREG.Value.ToUpper()
                            }).ToString());
                        if (kq > 0) iCount++;
                    }
                    catch (Exception ex)
                    {
                        int dong = i + 1;
                        _error += $"{"Insert lỗi dòng " + dong + "---" + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        _AcDAL.InsertLog("IMPORT_PERMNO_IMP", DateTime.Now.ToString(), ex.ToString());
                    }
                    
                }
            }
            return iCount;
        }
        public int NWS(string[] value)
        {
            int iCount = 0;
            string _Eta = "";
            string _callSign = "";
            string _error = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("NWS"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t", "+1", "+" }, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int ax = Convert.ToInt32(d[2].Trim().Substring(0, 2));
                        int bx = Convert.ToInt32(d[3].Trim().Substring(0, 2));
                        if (ax > bx) _Eta = d[3].Trim() + "+"; else _Eta = d[3].Trim();
                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMNO_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtPermDate.Value),
                                P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(txtCRAFTTYPE.Value.ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[4],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2]),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = txtRoutes.Value.ToUpper(),
                                P_REMARK = "",
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_DAYFLY = clsChuanHoaImport.ChuanHoaDateTime(d[6].Trim('/')),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_PERMTYPE = ddlPERMTYPE.Value,
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_OPER = ddlOPER.Value,
                                P_REGISTRATION = txtREG.Value.ToUpper()
                            }).ToString());
                        if (kq > 0) iCount++;
                    }
                    catch (Exception ex)
                    {
                        int dong = i + 1;
                        _error += $"{"Insert lỗi dòng " + dong + "---" + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        _AcDAL.InsertLog("IMPORT_PERMNO_IMP", DateTime.Now.ToString(), ex.ToString());
                    }
                    
                }
            }
            return iCount;
        }
        public int NOK (string[] value)
        {
            int iCount = 0;
            string _Eta = "";
            string _callSign = "";
            string _error = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i].Contains("DD"))
                {
                    try
                    {
                        string[] d = value[i].Split(new string[] { " ", "\t"}, StringSplitOptions.RemoveEmptyEntries);
                        _callSign = d[0];
                        int ax = Convert.ToInt32(d[2].Trim().Substring(0, 2));
                        int bx = Convert.ToInt32(d[4].Trim().Substring(0, 2));
                        if (ax > bx) _Eta = d[4].Trim().Substring(0,4) + "+"; else _Eta = d[4].Trim().Substring(0,4);
                        int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "PERMNO_IMP_INSERT"
                            , new
                            {
                                P_CALLSIGN = d[0],
                                P_PERMDATE = clsChuanHoaImport.ChuanHoaDateTime(txtPermDate.Value),
                                P_CRAFT = clsChuanHoaImport.ChuanHoaCraft(txtCRAFTTYPE.Value.ToUpper()),
                                P_FROM_AIRP = d[1],
                                P_TO_AIRP = d[3],
                                P_ETD = clsChuanHoaImport.ChuanHoaGioBay(d[2].Substring(0,4)),
                                P_ETA = clsChuanHoaImport.ChuanHoaGioBay(_Eta),
                                P_VIA = txtRoutes.Value.ToUpper(),
                                P_REMARK = "",
                                P_AUTHOR = ddlAUTHOR.SelectedValue,
                                P_DAYFLY = clsChuanHoaImport.ChuanHoaDateTime(d[5].Trim('/')),
                                P_PURPOSE = ddlPURPOSE.SelectedValue,
                                P_PERMNBR = txtPERMNBR.Value,
                                P_PERMTYPE = ddlPERMTYPE.Value,
                                P_FLIGHTTYPE = ddlFLIGHTTYPE.Value,
                                P_OPER = ddlOPER.Value,
                                P_REGISTRATION = txtREG.Value.ToUpper()
                            }).ToString());
                        if (kq > 0) iCount++;
                    }
                    catch (Exception ex)
                    {
                        int dong = i + 1;
                        _error += $"{"Insert lỗi dòng " + dong + "---" + "CallSign " + _callSign + "<br />"}";
                        lblLOG.Text = _error;
                        _AcDAL.InsertLog("IMPORT_PERMNO_IMP", DateTime.Now.ToString(), ex.ToString());
                    }

                }
            }
            return iCount;
        }
        private string[] ResultText(string value)
        {
            if (string.IsNullOrEmpty(value.Trim())) return null;
            return value.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
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
    }
}