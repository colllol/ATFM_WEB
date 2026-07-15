using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using Newtonsoft.Json.Converters;
using System.Data;

namespace prjApplication.Tool
{
    public partial class ImportPermNo : System.Web.UI.Page
    {
        #region lis default
        private List<Aero> lisAero { get { return new AeroDAL().GetListAll(); } }
        private List<RouteList> lisRoute { get { return new RouteListDAL().GetAllRouteList(); } }
        private List<Oper> lisOper { get { return new OperDAL().GetAllObject(); } }
        private List<CraftType> lisCraft { get { return new CraftTypeDAL().GetAllCraftType(); } }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // LoadDataFromDB();
            }
        }

        #region control event
        protected void btnAccess_Click(object sender, EventArgs e)
        {
            if (txtOPER.Value == "HVN")
            {
                int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "Hvn_Imp_No_Excute", null).ToString());
                LoadDataFromDB();
            }
            else
            {

            }
        }
        protected void btnLoc_Click(object sender, EventArgs e)
        {
            string oper = "", regis = "", add = "", number = "", permType = "", craft = "", callsign = "", lichbay = "", ngayphep = "", via = "";
            oper = NhanDang_Oper(txtCONTENT.Text.ToUpper());
            regis = NhanDang_Registrator(txtCONTENT.Text.ToUpper());
            add = NhanDang_Address(txtCONTENT.Text.ToUpper());
            number = NhanDang_Number(txtCONTENT.Text.ToUpper());
            permType = NhanDang_PermType(txtCONTENT.Text.ToUpper());
            craft = NhanDang_Craft(txtCONTENT.Text.ToUpper());
            callsign = NhanDang_CallSign(txtCONTENT.Text.ToUpper());

            if (txtOPER.Value == "HVN")
            {
                ngayphep = hvn_NhanDang_PermDate(txtCONTENT.Text.ToUpper());
                lichbay = hvn_NhanDang_LichBay(txtCONTENT.Text.ToUpper());

            }
            else
            {
                ngayphep = NhanDang_PermDate(txtCONTENT.Text.ToUpper());
                lichbay = NhanDang_Schedule(txtCONTENT.Text.ToUpper());
                via = NhanDang_Via(txtCONTENT.Text.ToUpper());
                via = _chuanhoa_via(via);

                if (!string.IsNullOrEmpty(callsign) && string.IsNullOrEmpty(oper))
                {
                    var o = lisOper.FirstOrDefault(a => a.OPER_ICAO == callsign.Substring(0, 3)).OPER_ICAO;
                    var o2 = lisOper.FirstOrDefault(a => a.OPER_IATA == callsign.Substring(0, 2)).OPER_ICAO;
                    if (o2 != null)
                        txtOPER.Value = o2;
                    if (o != null)
                        txtOPER.Value = o;
                }
                else
                    txtOPER.Value = oper;
            }
            txtREGISTRATION.Value = regis;
            txtCraft.Value = craft;
            //txtPERMNBR.Value = number;

            if (permType.Trim() != "")
                txtPERMTYPE.Value = permType;
            txtBILLINGADDRESS.Value = add;
            txtSCHEDULE.Value = lichbay;
            if (ngayphep.Trim() != "")
                txtPERMDATE.Value = clsChuanHoaImport.ChuanHoaDateTime(ngayphep.Trim());
            txtVIA.Value = via;
            LoadDataFromDB();

        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            #region by oper HVN
            if (txtOPER.Value == "HVN")
            {
                int iKq = 0;

                List<Hvn_Perm> lis = hvn_Get_ListPerm(txtSCHEDULE.Value);
                foreach (var obj in lis)
                {
                    iKq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "HVN_NO_Insert",
                        new
                        {
                            P_FLIGHTNBR = obj.FLIGHTNBR,
                            P_FROM_AIRP = obj.FROM_AIRP,
                            P_TO_AIRP = obj.TO_AIRP,
                            P_ETD_NEW = obj.ETD_NEW,
                            P_ETA_NEW = obj.ETA_NEW,
                            P_ETD_OLD = obj.ETD_OLD,
                            P_ETA_OLD = obj.ETA_OLD,
                            P_FLIGHTDATE_NEW = obj.FLIGHTDATE_NEW,
                            P_FLIGHTDATE_OLD = obj.FLIGHTDATE_OLD,
                            P_CRAFT = ReturnCraft(obj.CRAFT),
                            P_PURPOSE = obj.PURPOSE,
                            P_PERMNBR = obj.PERMNBR,
                            P_PERMDATE = obj.PERMDATE,
                            P_AUTHOR = obj.AUTHOR,
                            P_OPER = obj.OPER,
                            P_LOAI = obj.LOAI,
                            P_PERMTYPE = obj.PERMTYPE,
                            P_FLIGHTTYPE = obj.FLIGHTTYPE
                }));
                }
                LoadDataFromDB();
                return;
            }
            #endregion
            var oper = txtOPER.Value;
            var callsign = NhanDang_CallSign(txtCONTENT.Text.ToUpper());
            var craft = NhanDang_Craft(txtCONTENT.Text.ToUpper());
            var ax = _chuanHoa_Year(txtSCHEDULE.Value.ToUpper());
            ax = _chuanHoa_Schedule(ax);

            string[] dongs = ax.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < dongs.Length; i++)
            {
                ax = _chuanHoa_Hour_Z_Cong_InLine(dongs[i]);
                ax = _chuanHoa_DOF_InLine(ax);

                /********************************************************************************************************
                 * 
                 * Thu tu khi get
                 * Get:  YEAR => CALLSIGN => FROM => TO => ETD => ETA => CRAFT
                 * 
                 ********************************************************************************************************/


                if (callsign == "")
                {
                    callsign = __Get_CALLSIGN_InLine(ref dongs[i], txtOPER.Value.ToUpper());
                }
                var dayly = __Get_DOF_Inline(ref ax);
                var f = __Get_FROM_TO_Inline(ref ax);
                var t = __Get_FROM_TO_Inline(ref ax);
                var etd = __Get_ETD_ETA_Inline(ref ax);
                var eta = __Get_ETD_ETA_Inline(ref ax);
                if (craft == "")
                    craft = __Get_Craft_Inline(ref ax);


            }
        }
        public string ReturnCraft(string _craft)
        {
            string _kq = "";
            switch(_craft.Trim())
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
        protected void btnDeleteId_Click(object sender, EventArgs e)
        {
            LinkButton bnt = (LinkButton)sender;
            var id = bnt.Attributes["data-id"].ToString();
            int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "Hvn_Imp_No_DeleteBy", new { P_ID = id }).ToString());
            if (kq == 1)
                this.AlertMessage("Delete sussess!");
            else this.AlertMessage("Delete error!");
            LoadDataFromDB();
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            int kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "Hvn_Imp_No_DeleteAll", null));
            if (kq == 1)
                this.AlertMessage("Delete sussess!");
            else this.AlertMessage("Delete error!");
            LoadDataFromDB();
        }
        #endregion

        #region function
        private void LoadDataFromDB()
        {
            DataTable dt = new clsResuftAPI().GetTableApiExtension("PERM_IMP_PKG", "Hvn_Imp_No_GetAll", null);
            rptSource.DataSource = dt;
            rptSource.DataBind();
        }
        private PermNoIMP ReadToPermIMP_NO(string dong)
        {
            PermNoIMP obj = new PermNoIMP();
            string ax = dong;


            return obj;
        }
        private List<clsVia> ReadToVia(string txt)
        {
            List<clsVia> lis = new List<clsVia>();
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                string[] pts = dong.Split(new string[] { "\t", " ", ".", "/", "-", ":" }, StringSplitOptions.RemoveEmptyEntries);

            }
            return lis;
        }
        private string Ref_To_Dong(string a, string dong)
        {
            int idx = dong.IndexOf(a);
            return dong.Remove(idx, a.Length);
        }
        #endregion

        #region nhan dang
        private string NhanDang_Registrator(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                foreach (var item in chuoi_Registrator)
                {
                    if (dong.Contains(item))
                        return _cat(chuoi_Registrator, dong);
                }
            }
            return kq;
        }
        private string NhanDang_Address(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                foreach (var item in chuoi_BillingAddress)
                {
                    if (dong.Contains(item))
                        return _cat(chuoi_BillingAddress, dong);
                }
                foreach (var item in chuoi_Address)
                {
                    if (dong.Contains(item))
                        return _cat(chuoi_Address, dong);
                }
            }
            return kq;
        }
        private string NhanDang_Oper(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                foreach (var item in chuoi_OPER)
                {
                    if (dong.Contains(item))
                        return _cat(chuoi_OPER, dong);
                }
            }
            return kq;
        }
        private string NhanDang_CallSign(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                foreach (var item in chuoi_CallSign)
                {
                    if (dong.Contains(item))
                        return _cat(chuoi_CallSign, dong);
                }
            }
            return kq;
        }
        private string NhanDang_Number(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                foreach (var item in chuoi_Number)
                {
                    try
                    {
                        if (dong.Contains(item))
                        {
                            if (dong.Contains(":"))
                            {
                                string ax = "";
                                try
                                {
                                    ax = dong.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                                }
                                catch
                                {
                                    ax = dong.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                                }
                                var idx = ax.IndexOf("-");
                                var idxx = ax.IndexOf("/", idx);
                                return ax.Substring(idx + 1, idxx - idx - 1);
                            }
                            else
                            {
                                var idS = dong.IndexOf("NBR");
                                var idx_ = dong.IndexOf("-", idS);
                                var idxX = dong.IndexOf("/", idx_);
                                return dong.Substring(idx_ + 1, idxX - idx_ - 1);
                            }
                        }
                    }
                    catch { }
                }
            }
            return kq;
        }
        private string NhanDang_PermType(string txt)
        {
            try
            {
                foreach (var item in chuoi_PermType)
                {
                    if (txt.IndexOf(item) > 0)
                        return item.Replace("-", "").Trim();
                }
            }
            catch { return ""; }
            return "";
        }
        private string NhanDang_Craft(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                foreach (var item in chuoi_Craft)
                {
                    if (dong.Contains(item))
                        return _cat(chuoi_Craft, dong);
                }
            }
            return kq;
        }
        private string NhanDang_Via(string txt)
        {
            string kq = "";
            foreach (var item in chuoi_Route)
            {
                if (txt.IndexOf(item) > 0)
                {
                    kq = txt.Remove(0, txt.IndexOf(item) + item.Length);
                    break;
                }
            }
            foreach (var item in chuoi_ValidHour)
            {
                if (kq.IndexOf(item) > 0)
                {
                    kq = kq.Substring(0, kq.IndexOf(item));
                    break;
                }
            }
            foreach (var item in chuoi_ValidHour)
            {
                if (kq.IndexOf(item) > 0)
                {
                    kq = kq.Substring(0, kq.IndexOf(item));
                    break;
                }
            }
            foreach (var item in chuoi_Purpose)
            {
                if (kq.IndexOf(item) > 0)
                {
                    kq = kq.Substring(0, kq.IndexOf(item));
                    break;
                }
            }
            return _chuanHoa_1_2_3(kq);
        }
        private string NhanDang_Schedule(string txt)
        {
            string kq = "";
            foreach (var item in chuoi_Schedule)
            {
                if (txt.IndexOf(item) > 0)
                {
                    kq = txt.Remove(0, txt.IndexOf(item) + item.Length);
                    break;
                }
            }
            foreach (var item in chuoi_Route)
            {
                if (kq.IndexOf(item) > 0)
                {
                    kq = kq.Substring(0, kq.IndexOf(item));
                    break;
                }
            }
            foreach (var item in chuoi_Purpose)
            {
                if (kq.IndexOf(item) > 0)
                {
                    kq = kq.Substring(0, kq.IndexOf(item));
                    break;
                }
            }
            foreach (var item in chuoi_ValidHour)
            {
                if (kq.IndexOf(item) > 0)
                {
                    kq = kq.Substring(0, kq.IndexOf(item));
                    break;
                }
            }
            return _chuanHoa_1_2_3(kq);
        }
        private string NhanDang_PermDate(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                foreach (var item in chuoi_Year)
                {
                    var idx = dong.IndexOf(item);
                    if (idx > 0)
                    {
                        string[] pts = dong.Split(new string[] { "\t", " ", ",", ".", ";" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var pt in pts)
                        {
                            try
                            {
                                if (pt.IndexOf(item) > 0)
                                    return clsChuanHoaImport.ChuanHoaDateTime(_chuanHoa_Year(pt).Trim());
                            }
                            catch { }
                        }
                    }
                }
            }
            return kq;
        }
        #endregion
        #region chuan hoa        
        private string _chuanHoa_1_2_3(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                string[] pts = dong.Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
                int ck = 0;
                try
                {
                    ck = Convert.ToInt32(pts[0].Replace(".", "").Replace("/", ""));
                    if (ck > 0)
                        kq += $"\r\n{dong.Remove(0, pts[0].Length)}";
                }
                catch
                {
                    kq += $"\r\n{dong}";
                }
            }
            return kq;
        }
        private string _chuanHoa_Schedule(string txt)
        {
            string kq = "";
            int c = txt.IndexOf("NEW");
            if (c > 0)
            {
                txt = txt.Substring(c, txt.Length - c);
            }
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dongs.Length; i++)
            {
                if (dongs[i].Length > 20)
                {
                    kq += $"\r\n {dongs[i]} ";
                }
            }
            return kq;
        }
        private string _chuanHoa_Year(string txt)
        {
            string kq = "";
            string[] pts = txt.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < pts.Length; i++)
            {
                foreach (var item in chuoi_Year)
                {
                    if (pts[i].Contains(item))
                    {
                        if (pts[i].Length == 7)
                        {
                            pts[i] = pts[i].Substring(0, 5) + DateTime.Now.Year;
                        }
                        if (pts[i].Length == 5)
                        {
                            pts[i] = pts[i] + DateTime.Now.Year;
                        }
                        if (pts[i].Length == 3)
                        {
                            pts[i] = pts[i - 1] + pts[i];
                            pts[i - 1] = "";
                            if (DateTime.Now.Year.ToString().Contains(pts[i + 1]))
                            {
                                pts[i] += pts[i + 1];
                                pts[i + 1] = "";
                            }
                            else pts[i] += DateTime.Now.Year;
                        }
                    }
                }
                kq += $" {pts[i]} ";
            }
            return kq;
        }
        private string _chuanHoa_DOF_InLine(string dong)
        {
            string kq = "";
            string rdd = "";
            string[] dongs = dong.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dongs.Length; i++)
            {
                try
                {
                    try
                    {
                        DateTime ax = DateTime.Parse(dongs[i]);
                        rdd = dongs[i];
                    }
                    catch
                    {
                        DateTime ax = UltilFunc.ToDate(dongs[i], "dd-MM-yyyy");
                        rdd = dongs[i];
                    }

                }
                catch
                {
                    kq += $"\r\n {dongs[i]} {rdd}";
                }

            }
            return kq;
        }
        private string _chuanHoa_Hour_Z_Cong_InLine(string dong)
        {
            string kq = "";
            dong = dong.Replace("+1", " ").Replace("+2", " ").Replace("+", " ");
            string[] pts = dong.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < pts.Length; i++)
            {
                try
                {
                    if (pts[i].IndexOf("Z", 4) > 0)
                    {
                        pts[i] = pts[i].Substring(0, pts[i].Length - 1);
                    }
                }
                catch { }
                kq += $" {pts[i]} ";
            }
            return kq;
        }
        private string __Get_DOF_Inline(ref string dong)
        {
            string[] pts = dong.Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pt in pts)
            {
                foreach (var item in chuoi_Year)
                {
                    if (pt.Contains(item))
                    {
                        dong = dong.Remove(dong.IndexOf(item), item.Length);
                        return pt;
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// Firt Get_FROM  second Get_TO
        /// </summary>
        /// <param name="dong"></param>
        /// <returns></returns>
        private string __Get_FROM_TO_Inline(ref string dong)
        {
            string[] pts = dong.Split(new string[] { "\t", " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pt in pts)
            {
                try
                {
                    var ax = lisAero.FirstOrDefault(a => a.AE_CODE == pt || a.AE_IATA == pt).AE_CODE;
                    if (ax != null)
                    {
                        dong = dong.Remove(dong.IndexOf(pt), pt.Length);
                        return ax;
                    }
                    if (pt.Length == 8)
                    {
                        var bx = lisAero.FirstOrDefault(a => a.AE_CODE == pt.Substring(0, 4)).AE_CODE;
                        if (bx != null)
                        {
                            dong = dong.Remove(dong.IndexOf(pt), pt.Length);
                            return bx;
                        }
                    }
                }
                catch { }
            }
            return "";
        }
        /// <summary>
        /// Firt get Etd second get eta
        /// </summary>
        /// <param name="dong"></param>
        /// <returns></returns>
        private string __Get_ETD_ETA_Inline(ref string dong)
        {
            string[] pts = dong.Split(new string[] { "\t", " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pt in pts)
            {
                if (pt.Length == 4)
                {
                    try
                    {
                        int ax = Convert.ToInt32(pt);
                        if (ax >= 0)
                        {
                            dong = dong.Remove(dong.IndexOf(pt), pt.Length);
                            return pt;
                        }
                    }
                    catch { }
                }
            }
            return "";
        }
        private string __Get_CALLSIGN_InLine(ref string dong, string oper)
        {
            string[] pts = dong.Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pt in pts)
            {
                try
                {
                    string _pt1 = pt.Trim();
                    var cs = lisOper.Any(a => a.OPER_ICAO == _pt1.Substring(0, 3));
                    if (cs)
                    {
                        dong = dong.Remove(dong.IndexOf(_pt1), _pt1.Length);
                        return _pt1;
                    }

                    var s = _pt1.Substring(0, 2);

                    var cs1 = lisOper.Any(a => a.OPER_IATA == _pt1.Substring(0, 2));
                    if (cs1)
                    {
                        dong = dong.Remove(dong.IndexOf(_pt1), _pt1.Length);
                        return _pt1;
                    }
                    
                }
                catch { }
            }
            return "";
        }
        private string __Get_Craft_Inline(ref string dong)
        {
            string[] pts = dong.Split(new string[] { "\t", " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pt in pts)
            {
                try
                {
                    var cs = lisCraft.Any(a => a.MA == pt || a.MA.Contains(pt));
                    if (cs)
                    {
                        dong = dong.Remove(dong.IndexOf(pt), pt.Length);
                        return pt;
                    }
                }
                catch { }
            }
            return "";
        }

        #region chuan hoa ROUTE
        private string _chuanhoa_via(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                string[] pts = dong.Split(new string[] { "\t", " ", ":", ".", "-", ",", "/" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pt in pts)
                {
                    var f_t = lisAero.Any(a => a.AE_IATA == pt || a.AE_CODE == pt) || lisRoute.Any(a => a.ROUTE_NAME == pt);
                    if (!f_t)
                    {
                        txt = txt.Replace(pt, " ");
                    }
                }
            }
            return txt;
        }
        #endregion

        #endregion

        #region by oper HVN
        public class Hvn_Perm
        {
            public string FLIGHTNBR { get; set; }
            public string PERMTYPE { get; set; }
            public string FLIGHTTYPE { get; set; }
            public string FROM_AIRP { get; set; }
            public string TO_AIRP { get; set; }
            public string ETD_NEW { get; set; }
            public string ETA_NEW { get; set; }
            public string ETD_OLD { get; set; }
            public string ETA_OLD { get; set; }
            public string FLIGHTDATE_NEW { get; set; }
            public string FLIGHTDATE_OLD { get; set; }
            public string CRAFT { get; set; }
            public string PURPOSE { get; set; }
            public string PERMNBR { get; set; }
            public string PERMDATE { get; set; }
            public string AUTHOR { get; set; }
            public string OPER { get; set; }
            public string LOAI { get; set; }
        }


        private string hvn_NhanDang_LichBay(string txt)
        {
            var kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int idxS = 0, idxF = 0;
            foreach (var item in hvn_chuoi_duongbay)
            {
                if (txt.IndexOf(item) > 0)
                {
                    idxF = txt.IndexOf(item) + item.Length;
                    break;
                }
            }
            foreach (var item in hvn_chuoi_schedule)
            {
                if (txt.IndexOf(item) > 0)
                {
                    idxS = txt.IndexOf(item);
                    break;
                }
            }
            if (idxS > 0 && idxF > 0)
                kq = txt.Substring(idxS, idxF - idxS);
            dongs = kq.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            kq = "";
            for (int i = 1; i < dongs.Length - 1; i++)
            {
                kq += $"\r\n{dongs[i]}";
            }
            return kq;
        }
        private string hvn_NhanDang_PermDate(string txt)
        {
            try
            {
                string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                int idxS = 0, idxF = 0;
                foreach (var dong in dongs)
                {
                    foreach (var item in hvn_chuoi_ngayPhep)
                    {
                        idxS = dong.IndexOf(item);
                        if (idxS > 0)
                        {
                            return dong.Substring(idxS + item.Length, dong.IndexOf(",") - (idxS + item.Length)).Replace("CUA VNA", "");
                            //return dong.Substring(idxS + item.Length, dong.IndexOf("CUA VNA,") - (idxS + item.Length));
                        }
                    }
                }
                return "";
            }
            catch { return ""; }
        }
        private List<Hvn_Perm> hvn_Get_ListPerm(string txt)
        {
            List<Hvn_Perm> lis = new List<Hvn_Perm>();
            string flightdate = "", purpose = "", thaydoi = "4";   // thaydoi: 4, tang chuyen: 0, 
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dongs.Length; i++)
            {
                string _dong = dongs[i];
                if (string.IsNullOrEmpty(_dong.Trim()))
                    continue;
                _dong = _dong.Replace("+1", " ").Replace("+ 1 ", " ").Replace("+", "");
                if (_dong.Contains("NGAY"))
                    flightdate = clsChuanHoaImport.ChuanHoaDateTime(_dong.Replace("NGAY", "").Trim());
                else
                {
                    if (_dong.Contains("THAY DOI"))
                    {
                        thaydoi = "ThayDoi";
                        purpose = "PAX";
                    }
                    else if (_dong.Contains("TANG CHUYEN"))
                    {
                        thaydoi = "TangChuyen";
                        purpose = "EXTTM";
                    }
                    //else if ((_dong.Contains("C/B")) || (_dong.Contains("CHUYEN SAN")))
                    else if (_dong.Contains("CHUYEN SAN"))
                    {
                        purpose = "FER";
                        thaydoi = "ChuyenSan";
                    }
                    else if (_dong.Contains("HUY CHUYEN"))
                    {
                        purpose = "HUY";
                        thaydoi = "HuyChuyen";
                    }
                    else if (_dong.Contains("CHO HANG"))
                    {
                        purpose = "CAR";
                        thaydoi = "ChoHang";
                    }                    
                    else
                    {
                        Hvn_Perm obj = new Hvn_Perm();
                        obj.FLIGHTDATE_NEW = flightdate;
                        obj.PURPOSE = purpose;
                        obj.LOAI = thaydoi;
                        obj.PERMDATE = txtPERMDATE.Value;
                        obj.PERMNBR = txtPERMNBR.Value.ToUpper();
                        obj.PERMTYPE = txtPERMTYPE.Value.ToUpper();
                        obj.FLIGHTTYPE = ddlFLIGHTTYPE.Value.ToUpper();
                        obj.CRAFT = __Get_Craft_Inline(ref _dong);
                        obj.FLIGHTNBR = __Get_CALLSIGN_InLine(ref _dong, "HVN");
                        obj.FROM_AIRP = __Get_FROM_TO_Inline(ref _dong);
                        obj.TO_AIRP = __Get_FROM_TO_Inline(ref _dong);
                        obj.ETD_NEW = __Get_ETD_ETA_Inline(ref _dong);
                        obj.ETA_NEW = __Get_ETD_ETA_Inline(ref _dong);
                        if (_dong.Contains("ISO"))
                        {
                            obj.ETD_OLD = __Get_ETD_ETA_Inline(ref _dong);
                            obj.ETA_OLD = __Get_ETD_ETA_Inline(ref _dong);
                        }
                        if (_dong.Contains("ON"))
                            obj.FLIGHTDATE_OLD = clsChuanHoaImport.ChuanHoaDateTime(_chuanHoa_Year(__Get_DOF_Inline(ref _dong).Trim()));
                        lis.Add(obj);
                    }
                }
            }
            return lis;
        }

        string[] hvn_chuoi_schedule = { "LICH BAY (GIO UTC):", "LICH BAY (GIO UTC)", "LICH BAY" };
        string[] hvn_chuoi_duongbay = { "DUONG BAY:", "DUONG BAY" };
        string[] hvn_chuoi_ngayPhep = { "VNA NGAY", "NGAY" };
        #endregion

        private string _cat(string[] chuoi, string dong)
        {
            foreach (var item in chuoi)
            {
                int idxC = dong.IndexOf(item);
                if (idxC > 0) { dong = dong.Substring(idxC, dong.Length - idxC); break; }

            }
            if (dong.Contains(":"))
                return dong.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            else
                return "";
        }

        #region const
        string[] chuoi_Registrator = { "REG:", "REGISTRY", "REGISTRATION", "REGN" };
        string[] chuoi_Address = { "ADD:", "ADDRESS:" };
        string[] chuoi_BillingAddress = { "BILLING", "BILL:" };
        string[] chuoi_OPER = { "OPRT:", "OPR:", "OPERATOR" };
        string[] chuoi_ICAO = { "ICAO" };
        string[] chuoi_CallSign = { "CALLSIGN", "CALL SIGN", "CLSN" };
        string[] chuoi_Route = { "ATS ROUTS", "ATS ROUTE", "ATS ROUTES", "ROUTES:", "ROUTE:" };
        string[] chuoi_Craft = { "AIRCRAFT", "AC TYPE:", "A/C TYPE:", "ACFT TYPE", "ACFT:", "ACFT " };
        string[] chuoi_Schedule = { "SCHEDULE", "FLT DETAIL" };
        string[] chuoi_Number = { "NBR:", "PERMIT NBR", " NBR ", "NBR", "PMT", "LD-", "O/F-", "LD -", "O/F -" };
        string[] chuoi_Year = { "JAN", "FEB", "MAR", "ARP", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        string[] chuoi_ValidHour = { "WINDOW:", "WINDOW" };
        string[] chuoi_Purpose = { "PURPOSE OF FLIGHT:", "PURPOSE" };
        string[] chuoi_PermType = { " LD-", " O/F-" };
        #endregion

        public class clsVia
        {
            public string FROM_AIRP { get; set; }
            public string TO_AIRP { get; set; }
            public string ROUTE { get; set; }
        }


    }
}