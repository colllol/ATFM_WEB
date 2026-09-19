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
    public partial class ImportFlightCancel : PageBaseCallBack
    {
        #region private
        class CB
        {
            public string TXTVIA { get; set; }
            public string TXTOTHER { get; set; }
            public string OPER { get; set; }
            public string OPTION { get; set; }
            public string TYPE { get; set; }
            public List<IMP> Via { get; set; }
            public List<IMPFLIGHT> Flight { get; set; }
        }
        class IMP_2
        {
            public string From_To { get; set; }
            public string Via { get; set; }
        }
        class IMP
        {
            public string From_Airp { get; set; }
            public string To_Airp { get; set; }
            public string Via { get; set; }
            public string ReMark { get; set; }
        }

        class IMPFLIGHT
        {
            public string FLIGHTDATE { get; set; }
            public string CALLSIGN { get; set; }
            public string FROM_AIRP { get; set; }
            public string TO_AIRP { get; set; }
            public string ETD { get; set; }
            public string ETD_PERM { get; set; }
            public string REMARK { get; set; }

            public string OPER_ID { get; set; }
            
        }


        public class PermtIMP
        {
            public string Craft { get; set; }
            public string CallSign { get; set; }
            public string From_Airp { get; set; }
            public string To_Airp { get; set; }
        }
        public class ViaOther
        {
            public string CallSign { get; set; }
            public string Craft { get; set; }
            public string Via { get; set; }
        }
        #endregion

        #region static
        private List<Aero> lisAero { get { return new AeroDAL().GetListAll(); } }
        private List<RouteList> lisRoute { get { return new RouteListDAL().GetAllRouteList(); } }
        private List<Oper> lisOper { get { return new OperDAL().GetAllObject(); } }
        private List<CraftType> lisCraft { get { return new CraftTypeDAL().GetAllCraftType(); } }
        private string[] splitList = new string[] { "-" };
        private string[] splitPhancach = new string[] { ":" };
        private string[] splitCheck = new string[] { "-", "/" };

        #endregion
        protected string _phanCach = "::::";
        protected string _phanCachArg = "_____";
        protected void Page_Load(object sender, EventArgs e)
        {

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
                case "btnImport_Click":
                    try
                    {
                        kq = btnImport_Click(_arg[0]);
                    }
                    catch
                    {
                        kq = "Error: Sai dinh dang";
                    }

                    break;
                case "btnImportAll_Click"://WMKK-ZGGG	M771
                    try
                    {
                        kq = btnImportAll_Click(_arg[0]);
                    }
                    catch
                    {
                        kq = "Error: Sai dinh dang";
                    }

                    break;

                case "btnSearch_Click":
                    kq = btnSearch_Click(_arg[0]);
                    break;
                case "btnUpdate_Click":
                    kq = btnUpdate_Click(_arg[0]);
                    break;
            }

            return kq;
        }
        #endregion

        #region function [chuan 1]

        private string DonDichDong(string v)
        {
            string kq = "";
            string[] dongVia = v.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int ax = 0;
            //for (int i = 0; i < dongVia.Length - 1; i++)
            for (int i = 0; i <= dongVia.Length - 1; i++)
            {
                if (dongVia[i].Contains(":"))
                    ax = i;
                if (!dongVia[i].Contains(":") && i != 0)
                {
                    dongVia[ax] += " " + dongVia[i];
                    dongVia[i] = "";
                }
            }
            foreach (var item in dongVia)
            {
                kq += item + "\r\n";
            }
            return kq;


        }
        private string ChuanHoaDongDinhDangFORMTO(string txt)
        {
            /*
             * 
             * FROM:OMDB/OMDW/OMAA/OAKB/VTBS/VOTV/VABB/VECC/VCBI/VOMM/VOCL/VOCI/
VIDP/VGHS/VGEG/OAIX/OAZI/OAKN/OPLA/OPKC/VOTV/OPRN/OPKC/OBBI/OTHH/
OOMS/OMDB/OMDW/UAAA/EDDP/OAKB/FAOR/EKCH
TO:ZSPD/ZBAA/ZUUU/ZBTJ/ZLIC/RPVM/RJTT/RKSI/RJBB/RJAA/ZGGG/VHHH/
RCTP/ZWWW:A202/A206/A1/G474/L628/M771/L625

             * 
             * 
             */
            string kq = "";
            txt = txt.ToUpper();

            /******* chu y  14/12/2018  *******/
            //txt = txt.Replace("FROM:", "").Replace("TO:", "-").Replace(" TO ", "-");
            /*******  /14/12/2018  *******/

            if ((!txt.Contains("FROM")))
                return txt;
            if ((!txt.Contains("FROM") && !txt.Contains("TO")))
                return txt;
            //txt = txt.Replace("FROM:", "").Replace("TO:", "-").Replace(" TO ", "-");

            //txt = txt.Replace("\r\n", " ");
            //txt = txt.Replace("FROM:", "\r\n");
            //txt = txt.Replace("TO:", "-").Replace(" TO ", "-");
            //txt = txt.Replace(" / ", "/").Replace(" /", "/").Replace("/ ", "/");


            //string[] ds = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            //kq = ds[0] + "\r\n";
            //for (int i = 1; i < ds.Length; i++)
            //{
            //    if (ds[i].Contains(":"))
            //        kq += ds[i] + "\r\n";
            //    else
            //        kq += ds[i];
            //}
            //return kq;

            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dongs.Length; i++)
            {
                if (dongs[i].Contains("-") && dongs[i].Contains(":") && !dongs[i].Contains(" TO ") && !dongs[i].Contains("FROM"))
                {
                    kq += $"\r\n{dongs[i].Trim()}";
                }
                else if (dongs[i].Contains("FROM:") || dongs[i].Contains(" TO ") || dongs[i].Contains("FROM :"))
                    kq += $"\r\n{dongs[i]}";
                else kq += dongs[i];
            }
            kq = kq.Replace("FROM:", "\r\n");
            kq = kq.Replace("TO:", "-").Replace(" TO ", "-");
            kq = kq.Replace(" / ", "/").Replace(" /", "/").Replace("/ ", "/");
            return kq;
        }
        private string ChuanHoaDongDinhDangFromViaTo(string txt)
        {
            /*
             * 
             * ZGOW- TEBAK R474 NOB W21 BQ B214 LADON -VYMD
VYMD- LADON B214 BQ W21 NOB R474 TEBAK -ZGOW
             * 
             * 
             */
            string kq = "";
            txt = txt.ToUpper();
            txt = txt.Replace("-", " ");
            string[] ds = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < ds.Length; i++)
            {
                string[] pt = ds[i].Split(new string[] { " ", "-t" }, StringSplitOptions.RemoveEmptyEntries);
                kq += $"\r\n{NhanDangFrom(pt)}-{NhanDangTo(pt)}:{NhanDangVia(ds[i])}";
            }
            return kq;
        }
        private string ChuanHoaDongDinhDangROUTE(string txt)
        {
            string kq = "";
            txt = txt.ToUpper();
            if (!txt.Contains("ROUTE"))
                return txt;
            txt = txt.Replace("ROUTE", "").Replace(".", "").Replace("…", "");
            //txt = txt.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
            string[] ds = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            kq = ds[0] + "\r\n";
            for (int i = 1; i < ds.Length; i++)
            {
                if (!ds[i].Contains(":"))
                    kq += ds[i] + " ";
                else
                    kq += "\r\n" + ds[i] + " ";
            }
            return kq;
        }
        private List<IMP> DonDichVia(List<IMP> obj)
        {
            List<IMP> ax = obj;
            List<int> l = new List<int>();
            foreach (var item in obj)
            {
                if (item.From_Airp == null || item.To_Airp == null && item.Via != null)
                {
                    if (obj.IndexOf(item) != 0)
                    {
                        obj[obj.IndexOf(item) - 1].Via += item.Via;
                        l.Add(obj.IndexOf(item));
                        break;
                    }
                }
            }
            foreach (var item in l)
            {
                obj.RemoveAt(item);
                DonDichVia(obj);
            }
            return obj;
        }
        private List<IMPFLIGHT> GetImpFlightByRow(string txt)
        {
            /*
             * 1	31-Dec-24	VJC503	VVNB	VVDN	0020		Khong co trong KHB HANG
             */

            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<IMPFLIGHT> lis = new List<IMPFLIGHT>();
            foreach (var p in dongs)
            {
                string[] phantu = p.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                IMPFLIGHT obj = new IMPFLIGHT();
                obj.FLIGHTDATE = phantu[1].Trim().Replace(" ", "");
                obj.CALLSIGN = phantu[2].Trim();
                obj.FROM_AIRP = phantu[3].Trim();
                obj.TO_AIRP = phantu[4].Trim();
                obj.ETD = phantu[5].Trim();
                lis.Add(obj);
            }
            return lis.Distinct().ToList();


            //List<IMPFLIGHT> lis = new List<IMPFLIGHT>();
            //string[] phantu = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            //IMPFLIGHT obj = new IMPFLIGHT();
            //obj.FLIGHTDATE = phantu[1].Trim().Replace(" ", "");
            //obj.CALLSIGN = phantu[2].Trim();
            //obj.FROM = phantu[3].Trim();
            //obj.TO = phantu[4].Trim();
            //obj.ETD = phantu[5].Trim();
            //lis.Add(obj);
           // return lis;

        }
        private List<IMP> GetImpByRow(string dong)
        {
            /*
             *   Dang chuan tac tham so truyen vao co dang
             *   - HVN8908: RKSI/VTBS-VTSP: A1/A202/R474/L628/L642/N892/G474/Q1/A1-W1-G474/L628-G474/N892-L628-G474
             *   ==> Callsign  : From/From-To/To:  Via
             *   ==> Via convert => xxx/xxx/xxx 
             */
            string v = " ", rm = "";
            if (dong.Contains("[REMARK]"))
            {
                try
                {
                    rm = dong.Split(new string[] { "[REMARK]" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    dong = dong.Split(new string[] { "[REMARK]" }, StringSplitOptions.RemoveEmptyEntries)[0];
                }
                catch { }
            }
            List<IMP> lis = new List<IMP>();

            #region get via
            var via = dong.Trim(new char[] { '-', ' ' }).Split(new string[] { "/", "-", ",", " ", ":" }, StringSplitOptions.RemoveEmptyEntries).Where(d => lisRoute.FindIndex(e => e.ROUTE_NAME == d) != -1);
            foreach (var _v in via)
            {
                v += _v + "/";
            }
            v = FilterVia(v.Substring(0, v.Length - 1));
            v = v.Substring(0, v.Length - 1);
            #endregion

            string[] q = dong.Trim(new char[] { '-', ' ' }).Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            var ccc = lisAero.Any(a => q.Contains(a.AE_CODE) || q.Contains(a.AE_IATA));
            string[] phantu = dong.Trim(new char[] { '-', ' ' }).Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in phantu)
            {
                string[] c = item.Split(new string[] { "-", "/", " ", "– ", "--" }, StringSplitOptions.RemoveEmptyEntries);
                string m = item;
                #region find index from to
                bool isFromTo = lisAero.Any(x => c.Contains(x.AE_CODE) || c.Contains(x.AE_IATA));
                if (isFromTo)
                {
                    try
                    {
                        string _Froms = ""; string _Tos = "";
                        if (!item.Contains("-")) m = item.Replace("/", "-");
                        _Froms = m.Trim(new char[] { '-', ' ', '–' }).Split(new char[] { '-', '–', ' ' })[0].Replace(" ", "/").Replace(",", "/");
                        if (_Froms.Length == 3)
                            _Froms = lisAero.FirstOrDefault(a => a.AE_CODE == _Froms || a.AE_IATA == _Froms).AE_CODE;

                        _Tos = m.Trim(new char[] { '-', ' ', '–' }).Split(new char[] { '-', '-', '–' })[1].Replace(" ", "/").Replace(",", "/");
                        if (_Tos.Length == 3)
                            _Tos = lisAero.FirstOrDefault(a => a.AE_CODE == _Tos || a.AE_IATA == _Tos).AE_CODE;
                        string[] lisFrom = _Froms.Split(new string[] { "/", " " }, StringSplitOptions.RemoveEmptyEntries);
                        string[] lisTo = _Tos.Split(new string[] { "/", " " }, StringSplitOptions.RemoveEmptyEntries);
                        var _lisFrom = lisFrom.Where(a => lisAero.Any(b => b.AE_CODE == a || b.AE_IATA == a)).ToList();
                        var _lisTo = lisTo.Where(a => lisAero.Any(b => b.AE_CODE == a || b.AE_IATA == a)).ToList();
                        var _NoFromTo = lisFrom.Where(a => !lisAero.Any(b => b.AE_CODE == a || b.AE_IATA == a)).ToList();
                        _NoFromTo.AddRange(lisTo.Where(a => !lisAero.Any(b => b.AE_CODE == a || b.AE_IATA == a)).ToList());
                        foreach (var f in _NoFromTo)
                        {
                            if (f.Trim() != "" && f.Trim() != "V.V")
                                listError += $"\r\n{f}";
                        }
                        foreach (var f in _lisFrom)
                        {
                            foreach (var t in _lisTo)
                            {
                                IMP obj = new IMP();
                                obj.From_Airp = ChuanHoaSanBay(f);
                                obj.To_Airp = ChuanHoaSanBay(t);
                                obj.Via = v;
                                obj.ReMark = rm;
                                lis.Add(obj);
                            }
                        }


                    }
                    catch(Exception ex) { }
                    break;
                }

                #endregion

            }

            return lis;
        }
        private List<IMP> DinhDangFormTo(string text)
        {

            return new List<IMP>();
        }

        private int InsertFlightCancel(IMPFLIGHT obj)
        {


            var dateTimeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject(thamso);
            //bool ax = new QlbOutBoxDAL().Insert((object)obj);
            bool ax = new clsResuftAPI().GetValueApiExtension("A_TEST_SEARCH", "INERT_T_FLIGHTCANCEL", new { P_FLIGHTDATE = obj.FLIGHTDATE, P_CALLSIGN = obj.CALLSIGN, P_FROM_AIRP = obj.FROM_AIRP
                , P_TO_AIRP = obj.TO_AIRP, P_ETD = obj.ETD,P_ETD_PERM = obj.ETD_PERM,P_REMARK = obj.REMARK,P_OPER_ID=obj.OPER_ID
            }).ToString() == "1" ? true : false;
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[QlbInBoxDAL]", 0, $"[InsertOrigin][{ax.ToString()}]", 0);
            return ax ? 1 : 0;

        }

        private int ImportToDb(CB obj)
        {
            //ViaSearch v = new ViaSearch();
            //v.OPER = obj.OPER;
            //v.FlightType = "O/F";
           // v.PermType = "SC";
            int k = 0;
            try
            {
                foreach (var item in obj.Flight)
                {
                   IMPFLIGHT a = new IMPFLIGHT();

                    // a.OPER = obj.OPER;
                    // //a.FlightType = "O/F";
                    //// a.PermType = "SC";
                    // a.FROM_AIRP = item.From_Airp;
                    // a.TO_AIRP = item.To_Airp;
                    // a.VIA = item.Via;
                    // a.ReMark = item.ReMark;
                    var kq = InsertFlightCancel(a);
                    if (kq == 1)
                        k++;
                 }
              }
            catch { }
            //try
            //{
            //    var x = true;

            //    if (obj.TYPE == "DEL")
            //    {
            //        x = new ViaDAL().DeleteByVia(v);
            //    }                   

            //    if (x)
            //    {
            //        foreach (var item in obj.Via)
            //        {
            //            Via a = new Via();
            //            a.OPER = obj.OPER;
            //            a.FlightType = "O/F";
            //            a.PermType = "SC";
            //            a.FROM_AIRP = item.From_Airp;
            //            a.TO_AIRP = item.To_Airp;
            //            a.VIA = item.Via;
            //            a.ReMark = item.ReMark;
            //            var kq = new ViaDAL().CreateVia(a);
            //            if (kq == 1)
            //                k++;
            //        }
            //    }
            //}
            //catch { }
            return k;
        }



        #endregion

        #region event
        //FORMAT: WMKK-ZGGG	M771
        private string btnImportAll_Click(string thamso)
        {
            string _error = "";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CB>(thamso, dateTimeConverter);
            obj.Via = new List<IMP>();
            /*
             * cat' thanh` tung` dong` 
             */
            obj.TXTVIA = obj.TXTVIA.Replace("：", ":").Replace("-", " - ").Replace("–", " - ");


            obj.TXTVIA = GetViaByOper_UAL(obj.TXTVIA);

            #region CanhBaoLoi
            string _kq = "";
                string[] _txtvia = null;

                if (obj.TXTVIA.Contains("TRYERROR"))
                {
                    _txtvia = obj.TXTVIA.Split(new string[] { "TRYERROR" }, StringSplitOptions.RemoveEmptyEntries);
                    if (_txtvia.Length > 1) _error = _txtvia[1];
                    else _error = "";
                }
                else _error = "";

                //obj.TXTVIA = _txtvia[0];
                #endregion

                obj.TXTVIA = ChuanHoaViaDang_1_2_3(obj.TXTVIA);
                //obj.TXTVIA = ChuanHoaKiTuThua(obj.TXTVIA);
                obj.TXTVIA = ChuanHoaDongDinhDangROUTE(obj.TXTVIA);
                obj.TXTVIA = ChuanHoaDongDinhDangFORMTO(obj.TXTVIA);
                obj.TXTVIA = DonDichDong(obj.TXTVIA);
                //obj.TXTVIA = ChuanHoaDongDinhDangFromViaTo(obj.TXTVIA);
                obj.TXTVIA = ChuanHoaViaDang_X_Y_Z(obj.TXTVIA);
                /*Chuan hoa V.V*/
                obj.TXTVIA = ChuanHoaVV(obj.TXTVIA);
                string[] dongVia = null;
                dongVia = obj.TXTVIA.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in dongVia)
                {
                    obj.Via.AddRange(GetImpByRow(item.Trim()));
                }
                obj.Via = DonDichVia(obj.Via);

            


            var ax = ImportToDb(obj);

            //return listError.Trim() != "" ? $"Import sussess: {ax}/{obj.Via.Count} \r {_error} \r Not Via: {listError} " : $"Import sussess: {ax}/{obj.Via.Count}  {_error} ";
            return listError.Trim() != "" ? $"Import sussess : {ax} chặng \r {_error} \r Not Via : {listError} " : $"Import sussess: {ax}/{obj.Via.Count}  {_error} ";
        }
        private string btnImport_Click(string thamso)
        {
            string _error = "";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CB>(thamso, dateTimeConverter);
            //obj.Via = new List<IMP>();
            /*
             * cat' thanh` tung` dong` 
             */
            obj.TXTVIA = obj.TXTVIA.Replace("：", ":").Replace("-", " - ").Replace("–", " - ");
            //VTBS-VHHH	A202
            int k = 0;
            string[] dongs = obj.TXTVIA.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            //List<IMPFLIGHT> lis = new List<IMPFLIGHT>();
            foreach (var p in dongs)
            {
                string[] phantu = p.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                IMPFLIGHT objFlight = new IMPFLIGHT();
                objFlight.FLIGHTDATE = phantu[1].Trim().Replace(" ", "");
                objFlight.CALLSIGN = phantu[2].Trim();
                objFlight.FROM_AIRP = phantu[3].Trim();
                objFlight.TO_AIRP = phantu[4].Trim();
                objFlight.ETD = phantu[5].Trim();
                objFlight.OPER_ID = obj.OPER;
                var kq = InsertFlightCancel(objFlight);
                if (kq == 1)
                    k++;

            }
            return $"Import sussess: {k.ToString()}";
            //return listError.Trim() != "" ? $"Import sussess: {ax}/{obj.Via.Count} \r {_error} \r Not Via: {listError} " : $"Import sussess: {ax}/{obj.Via.Count}  {_error} ";
            //return listError.Trim() != "" ? $"Import sussess : {ax} chặng \r {_error} \r Not Via : {listError} " : $"Import sussess: {ax}/{obj.Via.Count}  {_error} ";
        }
        private string btnSearch_Click(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ViaSearch>(thamso, dateTimeConverter);
            System.Data.DataTable dt = new ViaDAL().GetTableBySearch(obj);
            string kq = "";
            int c = 0;
            if (dt == null) return "";
            foreach (DataRow row in dt.Rows)
            {
                kq += $"<tr data-isUpdate='false' id='{row["ID"]}'>";
                kq += $"<td style='vertical-align: middle; text-align: center;'>{c + 1}</td>";
                kq += $"<td><input id='txtFrom{row["ID"]}' type='text' data-autocomplete='AERO' value='{row["FROM_AIRP"]}' class='sInput' data-oldValue='{row["FROM_AIRP"]}' onblur='checkIsUpdate(this)'/></td>";
                kq += $"<td><input id='txtTo{row["ID"]}' type='text' data-autocomplete='AERO' value='{row["TO_AIRP"]}' class='sInput' data-oldValue='{row["TO_AIRP"]}' onblur='checkIsUpdate(this)'/></td>";
                kq += $"<td><input id='txtVia{row["ID"]}' type='text' value='{row["VIA"]}' class='sInput' data-oldValue='{row["VIA"]}' onblur='checkIsUpdate(this)'/></td>";
                kq += $"<td><div class=\"\"><i onclick=\"btnDeleteBy_Onclick("+ row["ID"] + ")\">Xoa</i></td>";
                kq += "</tr>";
                c++;
            }
            return kq;
            //return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }
        private string btnUpdate_Click(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CB>(thamso, dateTimeConverter);
            var ax = ImportToDb(obj);
            return $"Sussess {ax}/{obj.Via.Count}.";
        }
        #endregion

        #region function option [other]

        private List<IMP> getByOption(string[] via, string[] other, string opt, string oper)
        {
            List<IMP> lis = new List<IMP>();
            switch (opt)
            {
                case "CallSign":
                    break;
                default:
                    break;
            }

            return lis;
        }
        private List<PermtIMP> GetOtherByRow(string txt, string oper)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<PermtIMP> lis = new List<PermtIMP>();
            foreach (var p in dongs)
            {
                string[] phantu = p.Split(new string[] { "\t", " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                PermtIMP obj = new PermtIMP();
                obj.CallSign = NhanDangCallSign(phantu, oper).Trim();
                obj.Craft = NhanDangCraft(phantu).Trim();
                obj.From_Airp = NhanDangFrom(phantu).Trim();
                obj.To_Airp = NhanDangTo(phantu).Trim();
                lis.Add(obj);
            }
            return lis;
        }
        private List<ViaOther> GetViaOther(string txt, string oper)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<ViaOther> lis = new List<ViaOther>();
            foreach (var p in dongs)
            {
                string[] phantu = p.Split(new string[] { "\t", " ", "-", ":" }, StringSplitOptions.RemoveEmptyEntries);
                ViaOther obj = new ViaOther();
                obj.CallSign = NhanDangCallSign(phantu, oper).Trim();
                obj.Craft = NhanDangCraft(phantu).Trim();
                obj.Via = FilterVia(RutGonVia(NhanDangVia(p)).Trim().Replace("-", " ").Replace("--", " "));
                lis.Add(obj);
            }
            return lis.Distinct().ToList();
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

        private List<IMP> GetViaByFlightIMP(List<PermtIMP> objs, List<ViaOther> objVias, string oper)
        {

            List<IMP> Via = (from o in objs
                             from s in objVias
                             where o.CallSign == s.CallSign && o.Craft == s.Craft
                             //where ((o.CallSign!=null && o.CallSign==s.CallSign)||(o.Craft!=null && o.Craft==s.Craft))
                             select new IMP
                             {
                                 From_Airp = o.From_Airp,
                                 To_Airp = o.To_Airp,
                                 Via = s.Via
                             }).Distinct().ToList();




            return Via;
        }




        #endregion

        #region chuanhoa - nhan dang
        private string ChuanHoaSanBay(string v)
        {
            try
            {
                if (v.Length < 4)
                    return lisAero.FirstOrDefault(a => a.AE_IATA == v).AE_CODE;
                else return v;
            }
            catch { }
            return v;
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
        private string NhanDangFrom(string[] phanTuDong)
        {
            string kq = "";
            foreach (var p in phanTuDong)
            {
                try
                {
                    kq = lisAero.FirstOrDefault(a => a.AE_CODE == p || a.AE_IATA == p).AE_CODE;
                    if (kq != "") break;
                }
                catch { }
            }

            return kq;
        }
        private string NhanDangTo(string[] phanTuDong)
        {
            string kq = "";
            for (int i = phanTuDong.Length - 1; i > 0; i--)
            {
                try
                {
                    kq = lisAero.LastOrDefault(a => a.AE_CODE == phanTuDong[i] || a.AE_IATA == phanTuDong[i]).AE_CODE;
                    if (kq != "") break;
                }
                catch { }
            }

            return kq;
        }
        private string NhanDangCraft(string[] phanTuDong)
        {
            string kq = "";

            foreach (var p in phanTuDong)
            {
                try
                {
                    kq = lisCraft.FirstOrDefault(a => a.MA == p).MA;
                    if (kq != "") break;
                }
                catch { }
            }

            return kq;
        }
        private string NhanDangCallSign(string[] dong, string oper)
        {
            string kq = "";
            Oper o = lisOper.FirstOrDefault(a => a.OPER_IATA == oper || a.OPER_ICAO == oper);

            foreach (var p in dong)
            {
                try
                {
                    kq = dong.FirstOrDefault(a => a.Contains(o.OPER_IATA) || a.Contains(o.OPER_ICAO) && a.Length > 4);
                    if (kq != "") break;
                }
                catch { }
            }
            kq = kq == null ? "" : kq;
            return kq.Replace(":", "").Trim(new char[] { '-', ' ' });
        }
        private string NhanDangVia(string txt)
        {
            txt = txt.Replace("/", " / ").Replace("-", " - ").Replace(":", " : ");
            string[] phantu = txt.Split(new string[] { "\t", " " }, StringSplitOptions.None);
            List<string> d = phantu.Where(a => !lisRoute.Any(b => b.ROUTE_NAME == a)).ToList();
            foreach (var item in d)
            {
                if (item != "" && item != "-" && item != "/")
                    txt = txt.Replace(item, "");
            }
            return txt.Replace(":", "").Replace("\t", "").Trim(new char[] { '-', '/', ' ' });
        }
        private string NhanDangRemark(string dong)
        {
            dong = dong.Replace("/", " / ").Replace("-", " - ").Replace(":", " : ");
            string[] phantu = dong.Split(new string[] { "\t", " " }, StringSplitOptions.None);
            var kq = "";
            List<string> d = phantu.Where(a => lisCraft.Any(b => b.MA == a)).ToList();
            foreach (var item in d)
            {
                kq += $"{item}/";
            }
            return kq != "" ? kq.Substring(0, kq.Length - 1) : kq;
        }
        string[] rexReplace = {
            "Primary",
            "Route", "ROUTE:",
            "Airways","Airways:",
            "Entry/Exit",
            "waypoint","waypoint:",
            "Alternate", "Alternate:",
            "Exit", "Exit:",
            "Entry", "Entry:",
            "POINT", "POINT",
            "POINT/EXIT",
        };
        private string ChuanHoaKiTuThua(string txt, int x = 0)
        {
            string kq = "";
            txt = txt.Replace("-", " - ").Replace(",", "   ");
            kq = txt;
            string[] pt = txt.Split(new string[] { "\t", "\r\n", "\n", "\r", " ", "." }, StringSplitOptions.RemoveEmptyEntries);
            bool c = false, d = false, e = false;
            for (int i = x; i < pt.Length; i++)
            {
                try { c = lisAero.Any(a => a.AE_CODE == pt[i].Substring(0, 4) || a.AE_IATA == pt[i].Substring(0, 3)); } catch { }
                try { d = lisOper.Any(a => a.OPER_IATA == pt[i].Substring(0, 2) || a.OPER_ICAO == pt[i].Substring(0, 3)); } catch { }
                try { e = lisRoute.Any(a => a.ROUTE_NAME == pt[i].Trim().ToUpper()); } catch { }
                if (!c && !d && !e)
                {
                    kq = txt.Replace($" {pt[i]} ", " ");
                    x = i + 1;
                    break;
                }
            }
            if (!c && !d && !e)
            {
                kq = ChuanHoaKiTuThua(kq, x);
            }
            return kq;
        }
        #endregion

        #region by Oper
        #region my PIC
        private string PIC_ChuanHoaDinhDang(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            //string[] dongs = txt.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            foreach (var dong in dongs)
            {
                //string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                string _pts = dong.Substring(0, 1);
                try
                {
                    //var ax = Int32.Parse(pts[0]);
                    if (_pts != "\t")
                        kq += $"\r\n{dong}\t";
                    else
                        kq += $"{dong}\t";
                }
                catch
                {
                    kq += $"\t{dong}\t";
                }
            }
            return kq.Replace("\t", "");
        }
        private List<IMP> PIC_GetImpList(string dong)
        {
            dong = dong.Replace("+", "/");
            dong = dong.Replace("- ", "-").Replace(" -", "-").Replace(" - ", "-");
            //string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            string pts = dong.Substring(0, 7); //dong.Where(a => a.Length == 6).ToList();
            string pvia = dong.Substring(7, dong.Length - 7).Trim();
            string[] ft = pts.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            List<IMP> lis = new List<IMP>();
            //List<string> vs = VJC_GetVia(dong);
            try
            {
                IMP obj = new IMP();
                obj.From_Airp = lisAero.FirstOrDefault(a => a.AE_CODE == ft[0] || a.AE_IATA == ft[0]).AE_CODE;
                obj.To_Airp = lisAero.FirstOrDefault(a => a.AE_CODE == ft[1] || a.AE_IATA == ft[1]).AE_CODE;
                obj.Via = pvia;
                lis.Add(obj);
            }
            catch { }
            return lis;

        }
        private List<string> PIC_GetVia(string dong)
        {
            List<string> lis = new List<string>();
            string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            var pt = pts.Where(a => a.Length > 6).ToList();
            return pt;
        }
        #endregion
        #region my VJC
        private string VJC_ChuanHoaDinhDang(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            foreach (var dong in dongs)
            {
                string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    var ax = Int32.Parse(pts[0]);
                    kq += $"\r\n{dong}\t";
                }
                catch
                {
                    kq += $"\t{dong}\t";
                }
            }
            return kq;
        }
        private List<IMP> VJC_GetImpList(string dong)
        {
            string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            //var ft = pts.Where(a => a.Length == 6).ToList();
            string[] ft = pts[1].Split(new string[] { "-", " - " }, StringSplitOptions.RemoveEmptyEntries);
            List<IMP> lis = new List<IMP>();
            //List<string> vs = VJC_GetVia(dong);
            try
            {
                IMP obj = new IMP();
                obj.From_Airp = lisAero.FirstOrDefault(a => a.AE_CODE == ft[0] || a.AE_IATA == ft[0]).AE_CODE;
                obj.To_Airp = lisAero.FirstOrDefault(a => a.AE_CODE == ft[1] || a.AE_IATA == ft[1]).AE_CODE;
                obj.Via = pts[2];                
                lis.Add(obj);
            }
            catch { }
            return lis;

        }
        private List<string> VJC_GetVia(string dong)
        {
            List<string> lis = new List<string>();
            string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            var pt = pts.Where(a => a.Length > 6).ToList();
            return pt;
        }
        #endregion
        #region my HVN
        private List<IMP> HVN_GetImpList(string dong)
        {

            //string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            string[] pts = dong.Split(new string[] { "\t", " \t" }, StringSplitOptions.RemoveEmptyEntries);
            var ft = pts.Where(a => a.Length == 6).ToList();
            List<IMP> lis = new List<IMP>();
            //List<string> vs = HVN_GetVia(dong);
            string rm = NhanDangRemark(dong);
            string notViaFrom = "";
            try
            {
                
                string _via = "";
                string _fromTo = "";
                //Nvthai edit 05062020
                for (int i = 0; i < 1; i++)
                {
                    IMP obj = new IMP();
                    if(pts.Length>3)                    
                        _via = pts[i + 2] +" "+ pts[3];
                    else
                        _via = pts[i + 2];
                    _fromTo = pts[1];

                    obj.Via = _via;
                    obj.From_Airp = lisAero.FirstOrDefault(a => a.AE_CODE == _fromTo.Trim().Substring(0, 3) || a.AE_IATA == _fromTo.Trim().Substring(0, 3)).AE_CODE;

                    obj.To_Airp = lisAero.FirstOrDefault(a => a.AE_CODE == _fromTo.Trim().Substring(3, 3) || a.AE_IATA == _fromTo.Trim().Substring(3, 3)).AE_CODE;
                    obj.ReMark = rm;

                    lis.Add(obj);
                }



                //for (int i = 0; i < 2; i++)
                //{
                //    IMP obj = new IMP();
                
                //    _via = pts[i + 3];
                //    _fromTo = pts[i + 1];
               

                //    obj.Via = _via;
                //    obj.From_Airp = lisAero.FirstOrDefault(a => a.AE_CODE == _fromTo.Trim().Substring(0, 3) || a.AE_IATA == _fromTo.Trim().Substring(0, 3)).AE_CODE;

                //    obj.To_Airp = lisAero.FirstOrDefault(a => a.AE_CODE == _fromTo.Trim().Substring(3, 3) || a.AE_IATA == _fromTo.Trim().Substring(3, 3)).AE_CODE;
                //    obj.ReMark = rm;

                //    lis.Add(obj);
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lis;

        }
        private List<string> HVN_GetVia(string dong)
        {
            List<string> lis = new List<string>();
            string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
            var pt = pts.Where(a => a.Length > 6).ToList();
            return pt;
        }
        private string HVN_ChuanHoaDinhDang(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            foreach (var dong in dongs)
            {
                string[] pts = dong.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    var ax = Int32.Parse(pts[0]);
                    kq += $"\r\n{dong.Trim()}\t";
                }
                catch
                {
                    kq += $"\t{dong.Trim()}\t";
                }
            }
            return kq;
        }

        #endregion

        #region MSG
        private string GetViaByOper_MSG(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            string via = "";
            string _from = "";
            string _to = "";
            bool c = false;
            txt = DonDichDong(txt);
            foreach (var dong in dongs)
            {
                if (dong.ToUpper().Contains("ROUTE"))
                    via = NhanDangVia(dong);
                if (dong.ToUpper().Contains("POINT"))
                {
                    _from = NhanDangFrom(dong.Split(new string[] { " ", "\t", ":", "/" }, StringSplitOptions.RemoveEmptyEntries));
                    _to = NhanDangTo(dong.Split(new string[] { " ", "\t", ":", "/" }, StringSplitOptions.RemoveEmptyEntries));
                    c = true;
                }
                if (c)
                    kq += $"\r\n{_from}-{_to} : {via}";
                c = false;
            }
            return kq;
        }

        private string GetViaByOper_SIA(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            string _fromTo = "";
            bool c = false;
            txt = DonDichDong(txt);
            foreach (var dong in dongs)
            {

                if (dong.Contains("-") || dong.Contains("–"))
                {
                    if (!dong.Contains(":"))
                    {
                        _fromTo = dong + ":";
                    }
                    else
                        _fromTo = dong;
                    kq += $"\r\n{_fromTo}";
                }
                else
                {
                    kq += " " + dong;
                }


            }
            return kq;
        }

        private string GetViaByOper_AHK(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            string via = "";
            string _from = "";
            string _to = "";
            bool c = false;
            txt = DonDichDong(txt);
            foreach (var dong in dongs)
            {
                try
                {
                    via = NhanDangVia(dong);
                    _from = NhanDangFrom(dong.Split(new string[] { " ", "\t", ":", "/", "...", "-" }, StringSplitOptions.RemoveEmptyEntries));
                    _to = NhanDangTo(dong.Split(new string[] { " ", "\t", ":", "/", "...", "-" }, StringSplitOptions.RemoveEmptyEntries));
                    if (dong.Contains("V.V"))
                    {
                        kq += $"\r\n{_from}-{_to} : {via}";
                        kq += $"\r\n{_to}-{_from} : {via}";
                    }
                    else
                        kq += $"\r\n{_from}-{_to} : {via}";

                }
                catch { }


            }
            return kq;
        }

        private string GetViaByOper_CBJ(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            string via = "";
            string _from = "";
            string _to = "";
            bool c = false;
            txt = DonDichDong(txt);
            foreach (var dong in dongs)
            {
                try
                {
                    via = NhanDangVia(dong);
                    _from = NhanDangFrom(dong.Split(new string[] { " ", "\t", ":", "/", "..." }, StringSplitOptions.RemoveEmptyEntries));
                    _to = NhanDangTo(dong.Split(new string[] { " ", "\t", ":", "/", "..." }, StringSplitOptions.RemoveEmptyEntries));
                    kq += $"\r\n{_from}-{_to} : {via}";
                }
                catch { }


            }
            return kq;
        }
        private string GetViaByOper_QTR(string txt)
        {
            txt = DonDichDong(txt);
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            foreach (var dong in dongs)
            {
                if (dong.Contains("NOTE"))
                    kq += " ";
                else if (dong.Contains("FROM"))
                    kq += $"\r\n{dong} ";
                else if (dong.IndexOf("TO") < 3 && dong.IndexOf("TO") != -1)
                    kq += $" {dong} ";
                else if (dong.IndexOf("RTE") < 4 && dong.IndexOf("RTE") != -1)
                    kq += $" {dong} ";
                else kq += $"\r\n{dong} ";
            }

            return kq;
        }




        #endregion
        private string GetViaByOper_PIC(string txt)
        {
            string kq = "";
            txt = PIC_ChuanHoaDinhDang(txt);
            List<IMP> lis = new List<IMP>();
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                lis.AddRange(PIC_GetImpList(dong));
            }
            foreach (var obj in lis)
            {
                kq += $"\r\n {obj.From_Airp}-{obj.To_Airp} : {NhanDangVia(obj.Via)}";
            }
            return kq;
        }
        private string GetViaByOper_VJC(string txt)
        {
            string kq = "";
            txt = VJC_ChuanHoaDinhDang(txt);
            List<IMP> lis = new List<IMP>();
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                lis.AddRange(VJC_GetImpList(dong));
            }
            foreach (var obj in lis)
            {
                kq += $"\r\n {obj.From_Airp}-{obj.To_Airp} : {NhanDangVia(obj.Via)}";
            }
            return kq;
        }
        private string GetViaByOper_MAS(string txt)
        {
            string kq = "";
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                kq += $"\r\n {dong}".Replace("SECTOR", "").Replace("-", " ").Replace(",", " ");
            }
            return kq;
        }

        private string GetViaByOper_APG(string txt)
        {
            txt = txt.ToUpper();
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Trim() == "PRIMARY ROUTE")
                {
                    kq += $"\r\n{ps[i - 1].Replace("/", "-").Trim()}:";
                }
                if (ps[i].Contains("NAME OF ATS ROUTE:")) kq += $"{ps[i].Split(':')[1]}";

            }
            return kq;
        }

        private string GetViaByOper_DKH(string txt)
        {
            string kq = "";

            List<IMP> lis = new List<IMP>();
            string[] dongs = txt.Split(new string[] { "OR", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);


            foreach (var dong in dongs)
            {
                string via = "";
                string _from = "";
                string _to = "";
                string[] pts = dong.Split(new string[] { "\r\n", "\n", "\r", "-" }, StringSplitOptions.RemoveEmptyEntries);
                _from = dong.Substring(0, dong.IndexOf("-"));
                _to = dong.Substring(dong.LastIndexOf("-") + 1, dong.Length - dong.LastIndexOf("-") - 1);
                for (int i = 1; i < pts.Length - 2; i++)
                {
                    if (!pts[i].Contains("/"))
                        via += $"{pts[i]}/";
                }
                if (_to.Contains("V.V"))
                {
                    kq += $"\r\n{_from} - {_to} : {via}";
                    kq += $"\r\n{_to} - {_from} : {via}";
                }
                else
                    kq += $"\r\n{_from} - {_to} : {via}";

            }

            return kq;
        }


        private string GetViaByOper_CEB(string txt)
        {
            string kq = "";

            List<IMP> lis = new List<IMP>();
            string[] dongs = txt.Split(new string[] { "OR", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);


            foreach (var dong in dongs)
            {
                string via = "";
                string _from = "";
                string _to = "";
                string[] pts = dong.Split(new string[] { "\r\n", "\n", "\r", "-", ":" }, StringSplitOptions.RemoveEmptyEntries);
                _from = dong.Substring(0, dong.IndexOf("-"));
                _to = pts[1].Replace("N.V.V", "");
                for (int i = 2; i <= pts.Length - 1; i++)
                {
                    if (!pts[i].Contains("/"))
                        via += $"{pts[i]}/";
                }
                if (dong.Contains("V.V"))
                {
                    kq += $"\r\n{_from} - {_to} : {via}";
                    kq += $"\r\n{_to} - {_from} : {via}";
                }
                else
                    kq += $"\r\n{_from} - {_to} : {via}";

            }

            return kq;
        }

        private string GetViaByOper_CQH(string txt)
        {
            string kq = "";
            txt = txt.Replace("( OR ", "").Replace("(OR ", "");
            List<IMP> lis = new List<IMP>();
            string[] dongs = txt.Split(new string[] { "OR", "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);


            foreach (var dong in dongs)
            {
                try
                {
                    string via = "";
                    string _from = "";
                    string _to = "";
                    if (dong.Contains(":"))
                    {
                        kq += "\r\n" + dong;
                    }
                    else
                    {
                        string[] pts = dong.Split(new string[] { "\r\n", "\n", "\r", "-" }, StringSplitOptions.RemoveEmptyEntries);
                        _from = dong.Substring(0, dong.IndexOf("-"));
                        _to = dong.Substring(dong.LastIndexOf("-") + 1, dong.Length - dong.LastIndexOf("-") - 1);
                        for (int i = 1; i < pts.Length - 2; i++)
                        {
                            if (!pts[i].Contains("/"))
                                via += $"{pts[i]}/";
                        }
                        if (_to.Contains("V.V"))
                        {
                            kq += $"\r\n{_from} - {_to} : {via}";
                            kq += $"\r\n{_to} - {_from} : {via}";
                        }
                        else
                            kq += $"\r\n{_from} - {_to} : {via}";

                    }
                }
                catch { kq += "\r\n" + dong; }
            }
            return kq;
        }

        private string GetViaByOper_CES(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            string via = "";
            string _from = "";
            string _to = "";
            bool c = false;
            txt = DonDichDong(txt);
            foreach (var dong in dongs)
            {
                try
                {
                    via = NhanDangVia(dong);
                    _from = NhanDangFrom(dong.Split(new string[] { " ", "\t", ":", "/", "-" }, StringSplitOptions.RemoveEmptyEntries));
                    _to = NhanDangTo(dong.Split(new string[] { " ", "\t", ":", "/", "-" }, StringSplitOptions.RemoveEmptyEntries));
                    if (!String.IsNullOrEmpty(_from))
                    {
                        if (dong.Contains("V.V"))
                        {
                            kq += $"\r\n{_from}-{_to} : {via}";
                            kq += $"\r\n{_to}-{_from} : {via}";
                        }
                        else
                            kq += $"\r\n{_from}-{_to} : {via}";
                    }

                }
                catch { }


            }
            return kq;
        }

        private string GetViaByOper_MKR(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            string via = "";
            string _from = "";
            string _to = "";
            bool c = false;
            txt = DonDichDong(txt);
            foreach (var dong in dongs)
            {
                try
                {
                    via = NhanDangVia(dong);
                    _from = NhanDangFrom(dong.Split(new string[] { " ", "\t", ":", "/", "-" }, StringSplitOptions.RemoveEmptyEntries));
                    _to = NhanDangTo(dong.Split(new string[] { " ", "\t", ":", "/", "-" }, StringSplitOptions.RemoveEmptyEntries));
                    if (!String.IsNullOrEmpty(_from))
                    {
                        kq += $"\r\n{_from}-{_to} : {via}";
                    }

                }
                catch { }


            }
            return kq;
        }


        private string GetViaByOper_MXD(string txt)
        {
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            string via = "";
            string _from = "";
            string _to = "";
            txt = DonDichDong(txt);
            foreach (var dong in dongs)
            {
                try
                {
                    string[] phantu = dong.Trim(new char[] { '-', ' ' }).Split('/');
                    if ((dong.Contains(".")) && (phantu.Length == 2))
                    {
                        _from = NhanDangFrom(dong.Split(new string[] { " ", "\t", ":", "/", "-" }, StringSplitOptions.RemoveEmptyEntries));
                        _to = NhanDangTo(dong.Split(new string[] { " ", "\t", ":", "/", "-" }, StringSplitOptions.RemoveEmptyEntries));

                        kq += $"\r\n{_from}-{_to} :";
                    }
                    else
                    {
                        kq += " " + $"{NhanDangVia(dong)}";

                    }

                }
                catch { }


            }
            return kq;
        }

        private string GetViaByOper_HVN(string txt)
        {
            /*
             * 1	CDGSGN	R468
		                    R471 W22 Q1
             * 2	DADHPH	W2 Q2 Q4 W20
             * 
             * 
             */
            string _error = "";
            string kq = "";
            txt = HVN_ChuanHoaDinhDang(txt);
            txt = auto_replace(txt);
            List<IMP> lis = new List<IMP>();
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                try
                {
                    lis.AddRange(HVN_GetImpList(dong));
                    //_error += "<br> Sucess: " + dong;
                }
                catch (Exception ex)
                {
                    //throw ex;
                    _error += "\r Error: " + dong;
                }
            }
            foreach (var obj in lis)
            {
                kq += $"\r\n {obj.From_Airp}-{obj.To_Airp} : {NhanDangVia(obj.Via)} [REMARK] {obj.ReMark}";
            }
            // lblError.Text = _error;
            return kq + "TRYERROR" + _error;
        }
        private string GetViaByOper_AXM(string txt)
        {
            /*
             txt = txt.ToUpper();
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\r", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Trim(new char[] { '-', ' ' }).Split('-').Length == 2)
                {
                    kq += $"\r\n{ps[i].Trim()} : ";
                }
                else
                {
                    kq += " " + $"{NhanDangVia(ps[i])}";
                }

            }
            if (!kq.Contains(":"))
                return txt;
            return kq;
             */
            txt = txt.ToUpper();
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Trim() == "PRIMARY ROUTE")
                {
                    kq += $"\r\n{ps[i - 1].Replace("/", "-")}:";
                }
                if (ps[i].Contains("NAME OF ATS ROUTE:")) kq += $"{ps[i].Split(':')[1]}";

            }
            return kq;
        }
        private string GetViaByOper_THA(string txt)
        {
            #region new
            string[] dongs = txt.Replace(":", "").Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";

            for (int i = 0; i < dongs.Length; i++)
            {
                if (dongs[i].IndexOf(".") < 4 && dongs[i].IndexOf(".") > 0)
                    kq += $"\r\n{dongs[i].Trim()}\r\n";
                else kq += $"/{dongs[i].Trim()}";
            }
            dongs = kq.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            kq = "";
            //for (int i = 0; i < dongs.Length; i++)
            //{
            //    if (dongs[i].IndexOf("/") == 0)
            //        dongs[i] = dongs[i].Substring(1, dongs[i].Length - 1);
            //    if (dongs[i].IndexOf(".") < 4 && dongs[i].IndexOf(".") > 0)
            //        kq += $"\r\n{dongs[i].Trim()}";
            //    else kq += $"\r\n{dongs[i].Trim()} : ";
            //}
            //var ax = kq;
            #endregion
            //string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            //string kq = "";
            string _fromTo = "";
            //txt = DonDichDong(txt);
            foreach (var dong in dongs)
            {

                if (dong.Contains("-") || dong.Contains("V.V"))
                {
                    if (!dong.Contains(":"))
                    {
                        _fromTo = dong + ":";
                    }
                    else
                        _fromTo = dong;
                    kq += $"\r\n{_fromTo}";
                }
                else
                {
                    kq += " " + NhanDangVia(dong);
                }


            }
            return kq;
        }

        private string GetViaByOper_UAE(string txt)
        {
            txt = txt.ToUpper();
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Contains(" TO "))
                {
                    kq += $"\r\n{ps[i].Trim()}";
                }
                else
                {
                    if (ps[i].Contains("-") && ps[i].Contains(":") && !ps[i].Contains(" TO ") && !ps[i].Contains("FROM"))
                    {
                        kq += $"\r\n{ps[i].Trim()}";

                    }
                    else if (ps[i].Contains("FROM"))
                    {
                        kq += $"\r\n{ps[i].Trim()}";
                    }
                    else
                    {
                        if (ps[i].Contains("TO"))
                            kq += " " + $"{ps[i]}";
                        else
                            kq += $"{ps[i]}";
                    }
                }

            }
            return kq;
        }


        private string GetViaByOper_UAL(string txt)
        {
            txt = txt.ToUpper();
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\r", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Trim(new char[] { '-', ' ' }).Split('-').Length == 2)
                {
                    kq += $"\r\n{ps[i].Trim()} : ";
                }
                else
                {
                    kq += " " + $"{NhanDangVia(ps[i])}";
                }

            }
            if (!kq.Contains(":"))
                return txt;
            return kq;
        }


        private string GetViaByOper_BKP(string txt)
        {
            txt = txt.ToUpper();
            string kq = "";
            string _from = "";
            string _to = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\r", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i <= ps.Length - 1; i++)
            {
                string[] pst = ps[i].Trim(new char[] { '-', ' ' }).Split('/');

                if (pst.Length == 2)
                {
                    for (int k = 0; k <= pst.Length - 1; k++)
                    {
                        _from = NhanDangFrom(pst[k].Split(new string[] { " ", "\t", ":", "/", "-", "–" }, StringSplitOptions.RemoveEmptyEntries));
                        _to = NhanDangTo(pst[k].Split(new string[] { " ", "\t", ":", "/", "-", "–" }, StringSplitOptions.RemoveEmptyEntries));

                        kq += $"\r\n{_from}-{_to} : {NhanDangVia(ps[i])} ";

                    }
                }


            }
            return kq;
        }

        private string GetViaByOper_VFC(string txt)
        {
            txt = txt.ToUpper();
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i <= ps.Length - 1; i++)
            {

                string[] pst = ps[i].Split(new string[] { "\r\n", "\n", "\r", "\t" }, StringSplitOptions.RemoveEmptyEntries);

                if (pst.Length == 3)
                {
                    kq += $"\r\n{pst[0]} : {pst[2]} ";
                }

            }
            return kq;
        }
        private string GetViaByOper_CCA(string txt)
        {
            txt = DonDichDong(txt.ToUpper());
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in ps)
            {
                string via = p.Split(new char[] { ':' })[1];
                string callSings = p.Split(new char[] { ':' })[0];
                if (callSings.Contains("/"))
                {
                    string[] cs = callSings.Split('/');
                    kq += cs[0] + " : " + via + "\r\n";
                    for (int i = 1; i < cs.Length; i++)
                    {
                        if (cs[i].Contains("("))
                            kq += "CCA" + cs[i].Substring(0, cs[i].IndexOf("(")) + " : " + via + "\r\n";
                        else
                            kq += "CCA" + cs[i] + " : " + via + "\r\n";
                    }
                }
                else kq += p + "\r\n";
            }
            return kq;
        }
        private string GetViaByOper_SVR(string txt)
        {

            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in ps)
            {

                string[] pts = p.Split(new string[] { "\r\n", "\n", "\r", "\t", " " }, StringSplitOptions.RemoveEmptyEntries);

                string via = NhanDangVia(p);
                string callSings = pts[0].ToString();
                kq += callSings + " : " + via + "\r\n";
            }
            return kq;
        }

        private string GetOtherByOper_SVR(string txt)
        {

            string kq = "";
            var lis = new AeroDAL().GetListAll();
            string[] dongs = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var dong in dongs)
            {
                var refDong = dong;
                string[] pts = dong.Split(new string[] { "\r\n", "\n", "\r", "\t", " ", "-", "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pt in pts)
                {
                    if (pt.Length == 8)
                    {
                        var ax = $"{pt.Substring(0, 4)}";
                        var c = lis.Find(a => a.AE_CODE == ax).AE_CODE;
                        refDong = $"{refDong.Replace(c, c + " ")}";
                    }

                }
                kq += $"\r\n{refDong}";

            }
            return kq;
        }


        private string GetViaByOper_ETD(string txt)
        {
            txt = DonDichDong(txt.ToUpper());
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            string[] cls = ps[0].Split(new string[] { "\r\n", "\n", "\n", " " }, StringSplitOptions.RemoveEmptyEntries);

            string via = GetViaByOper_ETD_GhepDong(txt);

            foreach (var p in cls)
            {

                string callSings = p;
                kq += p + " : " + via + "\r\n";

            }
            return kq;
        }

        private string GetViaByOper_ETD_GhepDong(string txt)
        {
            txt = DonDichDong(txt.ToUpper());
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < ps.Length; i++)
            {
                if (i != 0)
                {
                    kq += " " + ps[i] + " ";
                }
            }
            return kq;
        }
        private string GetViaByOper_CAL(string txt)
        {
            txt = DonDichDong(txt.ToUpper());
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < ps.Length; i++)
            {

                kq += "\r\n" + ps[i].Replace("--", "-") + " ";

            }
            return kq;
        }
        private string GetViaByOper_CLX(string txt)
        {
            string kq = "";
            string[] ps = txt.Split(new string[] { "\r\n", "\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ps.Length; i++)
            {
                if (ps[i].Trim().Length == 9 && ps[i].Contains("-"))
                    kq += ps[i].Trim() + ":\r\n";
                else kq += ps[i].Trim() + "\r\n";
            }
            ps = kq.Split(new string[] { "\r\n", "\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            kq = "";
            for (int i = 0; i < ps.Length; i++)
            {
                if (!ps[i].Contains(":") && !ps[i].Contains(";"))
                    kq += " " + ps[i] + " ";
                else if (ps[i].Contains(";"))
                    kq += "\r\n" + ps[i] + " ";
                else kq += "\r\n" + ps[i] + " ";
            }

            ps = kq.Split(new string[] { "\r\n", "\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            kq = "";
            for (int i = 0; i < ps.Length; i++)
            {
                if (!ps[i].Contains(":"))
                    kq += ps[i];
                else kq += ps[i] + "\r\n";
            }
            ps = kq.Split(new string[] { "\r\n", "\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            kq = "";
            foreach (var dong in ps)
            {
                string via = dong.Split(':')[1];
                if (!dong.Contains(";"))
                    kq += dong + "\r\n";
                else
                {
                    string[] ft = dong.Split(':')[0].Split(';');
                    foreach (var d in ft)
                    {
                        kq += $"{d}:{via}\r\n";
                    }
                }
            }
            return kq;
        }
        private string ChuanHoaVV(string txt)
        {
            txt = DonDichDong(txt);
            string[] ds = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            foreach (var dong in ds)
            {
                if (dong.Contains("V.V"))
                {
                    kq += "\r\n" + dong + " ";
                    string[] phantu = dong.Trim(new char[] { '-', ' ' }).Split(':');
                    if (phantu[0].Trim(new char[] { '-', ' ' }).Split('-').Length < 3)
                    {
                        string[] ps = phantu[0].Trim(new char[] { '-', ' ' }).Split('-');
                        for (int i = 0; i < ps.Length - 1; i++)
                        {
                            kq += "\r\n" + ps[i + 1] + " - " + ps[i] + " : " + phantu[1] + "\r\n";
                        }
                    }
                }
                else
                    kq += "\r\n" + dong + " ";

            }
            return kq;
        }

        private string ChuanHoaViaDang_X_Y_Z(string txt)
        {
            try
            {
                txt = DonDichDong(txt);
                string[] ds = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                string kq = "";
                foreach (var dong in ds)
                {
                    string[] phantu = dong.Trim(new char[] { '-', ' ' }).Split(':');
                    if (phantu[0].Trim(new char[] { '-', ' ' }).Split('-').Length < 3) kq += dong + "\r\n";
                    else
                    {
                        string[] ps = phantu[0].Trim(new char[] { '-', ' ' }).Split('-');
                        for (int i = 0; i < ps.Length - 1; i++)
                        {
                            kq += ps[i] + " - " + ps[i + 1] + " : " + phantu[1] + "\r\n";
                        }
                    }
                }
                return kq;
            }
            catch { return txt; }
        }
        private string ChuanHoaViaDang_1_2_3(string txt)
        {
            //txt = DonDichDong(txt);
            string[] ds = txt.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            string kq = "";
            foreach (var dong in ds)
            {
                int ax = 0;
                string c = "";
                string s = dong.Trim(new char[] { '.', ')', '-', ' ' });
                if (s.IndexOf(".") < 4 && s.IndexOf(".") != -1)
                {
                    ax = 1;
                    c = ".";
                }
                if (s.IndexOf(")") < 4 && s.IndexOf(")") != -1)
                {
                    ax = 1;
                    c = ")";
                }
                if (s.IndexOf("/") < 4 && s.IndexOf("/") != -1)
                {
                    c = "/";
                    ax = 1;
                }
                if (ax == 1) kq += s.Substring(s.IndexOf(c) + 1, s.Length - s.IndexOf(c) - 1) + "\r\n";
                else kq += s + "\r\n";

            }
            return kq;
        }
        #endregion

        private int kk = 0;
        private string listError = "";
        private string auto_replace(string txt)
        {
            string[] pts = txt.Split(new string[] { "\r\n", "\n", "\r", " ", "/", "-", "\t" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = kk; i < pts.Length; i++)
            {
                if (pts[i].Length == 5)
                {
                    txt = txt.Replace(pts[i], " ");
                    kk = i;
                    txt = auto_replace(txt);
                }
            }
            return txt;
        }

    }
}