using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjComponents;
using prjBusinessLogic;
using prjInfo;
using System.Data;
namespace prjApplication.Tool
{
    public partial class ImportPermForMonth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDataFromDb();
            }
        }
        #region control event
        protected void btnDeleteId_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Int64 id = Convert.ToInt64(btn.Attributes["data-id"]);
            var kq = Convert.ToInt32(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "perm_imp_month_delete", new { P_ID = id }).ToString());
            if (kq == -1)
                this.AlertMessage("Delete error!");
            else { btnSearch_Click(sender, e); this.AlertMessage("Delete sussess!"); }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadDataFromDb();
        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            string content = getContent(txtCONTENT.Value);
            List<perm_imp_month> lis = new List<perm_imp_month>();
            switch (txtOPER.Value.ToUpper())
            {
                case "HVN":
                    lis = GetListImport_HVN(content.ToUpper(), "HVN");
                    break;
                case "VJC":
                    lis = GetListImport_VJC(content.ToUpper(), "VJC");
                    break;
            }

            this.AlertMessage(importToDb(lis));
            btnSearch_Click(sender, e);
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            var kq = Convert.ToInt32(new clsResuftAPI().GetValueApiExtension("PERM_IMP_PKG", "perm_imp_month_delete_oper", new { P_OPER = txtOPER.Value, P_STARTDATE = txtFromDate.Value, P_FINISHDATE = txtToDate.Value, P_CALLSIGN = txtCALLSIGN.Value }).ToString());
            if (kq == -1)
                this.AlertMessage("Delete error!");
            else this.AlertMessage("Delete sussess!");
            btnSearch_Click(sender, e);
        }
        #endregion
        #region private

        #endregion
        #region class
        private class perm_imp_month
        {
            private Int64 _ID;
            private string _CRAFT_TYPE;
            private string _CRAFT_VERSION;
            private string _CRAFT_LOGICAL_NO;
            private string _REGISTER_CRAFT;
            private string _CALLSIGN;
            private string _SECTOR;
            private string _FROM_AIRP;
            private string _TO_AIRP;
            private string _ETD;
            private string _ETA;
            private DateTime _FLIGHT_DATE;
            private string _OPER;
            private int _STT;
            public DateTime FLIGHT_DATE
            {
                get
                {
                    return _FLIGHT_DATE;
                }

                set
                {
                    _FLIGHT_DATE = value;
                }
            }

            public string ETA
            {
                get
                {
                    return _ETA;
                }

                set
                {
                    _ETA = value;
                }
            }

            public string ETD
            {
                get
                {
                    return _ETD;
                }

                set
                {
                    _ETD = value;
                }
            }

            public string TO_AIRP
            {
                get
                {
                    return _TO_AIRP;
                }

                set
                {
                    _TO_AIRP = value;
                }
            }

            public string FROM_AIRP
            {
                get
                {
                    return _FROM_AIRP;
                }

                set
                {
                    _FROM_AIRP = value;
                }
            }

            public string SECTOR
            {
                get
                {
                    return _SECTOR;
                }

                set
                {
                    _SECTOR = value;
                }
            }

            public string CALLSIGN
            {
                get
                {
                    return _CALLSIGN;
                }

                set
                {
                    _CALLSIGN = value;
                }
            }

            public string REGISTER_CRAFT
            {
                get
                {
                    return _REGISTER_CRAFT;
                }

                set
                {
                    _REGISTER_CRAFT = value;
                }
            }

            public string CRAFT_LOGICAL_NO
            {
                get
                {
                    return _CRAFT_LOGICAL_NO;
                }

                set
                {
                    _CRAFT_LOGICAL_NO = value;
                }
            }

            public string CRAFT_VERSION
            {
                get
                {
                    return _CRAFT_VERSION;
                }

                set
                {
                    _CRAFT_VERSION = value;
                }
            }

            public string CRAFT_TYPE
            {
                get
                {
                    return _CRAFT_TYPE;
                }

                set
                {
                    _CRAFT_TYPE = value;
                }
            }

            public Int64 Id
            {
                get
                {
                    return _ID;
                }

                set
                {
                    _ID = value;
                }
            }

            public string OPER
            {
                get
                {
                    return _OPER;
                }

                set
                {
                    _OPER = value;
                }
            }

            public int STT
            {
                get
                {
                    return _STT;
                }

                set
                {
                    _STT = value;
                }
            }
        }
        private class perm_imp_month_search : perm_imp_month
        {
            private int _pageSize;
            private int _pagaIndex;
            private DateTime _startDate;
            private DateTime _finishDate;

            public DateTime FinishDate
            {
                get
                {
                    return _finishDate;
                }

                set
                {
                    _finishDate = value;
                }
            }

            public DateTime StartDate
            {
                get
                {
                    return _startDate;
                }

                set
                {
                    _startDate = value;
                }
            }

            public int PagaIndex
            {
                get
                {
                    return _pagaIndex;
                }

                set
                {
                    _pagaIndex = value;
                }
            }

            public int PageSize
            {
                get
                {
                    return _pageSize;
                }

                set
                {
                    _pageSize = value;
                }
            }
        }
        #endregion
        #region function
        private string importToDb(List<perm_imp_month> lis)
        {
            Int64 kqInsert = 0;
            Int64 dem = 0;
            string loi = "";
            foreach (var obj in lis)
            {
                kqInsert = Convert.ToInt64(new clsResuftAPI().GetPostValueApiExtension("PERM_IMP_PKG", "perm_imp_month_insert", new
                {
                    P_CRAFT_TYPE = obj.CRAFT_TYPE,
                    P_REGISTER_CRAFT = obj.REGISTER_CRAFT,
                    P_CALLSIGN = obj.CALLSIGN,
                    P_FROM_AIRP = obj.FROM_AIRP,
                    P_TO_AIRP = obj.TO_AIRP,
                    P_ETD = obj.ETD,
                    P_ETA = obj.ETA,
                    P_FLIGHT_DATE = obj.FLIGHT_DATE,
                    P_OPER = obj.OPER
                }).ToString());
                if (kqInsert == -1)
                {
                    loi += $"\r\n{obj.CALLSIGN} STT: {obj.STT}";
                }
                else dem++;
            }
            return $"Insert sussess: {dem}/{lis.Count} {loi}";
        }
        private void LoadDataFromDb()
        {
            DataTable dt = new clsResuftAPI().GetPostTableApiExtension("PERM_IMP_PKG", "perm_imp_month_getSearch", new {
                P_CALLSIGN = txtCALLSIGN.Value.ToUpper(),
                P_CRAFT_TYPE = txtCRAFT_TYPE.Value.ToUpper(),
                P_ETA = txtETA.Value.ToUpper(),
                P_ETD = txtETD.Value.ToUpper(),
                //P_FLIGHT_DATE = txtFLIGHT_DATE.Value=="" ? "" : DateTime.Parse(txtFLIGHT_DATE.Value).ToString("yyyy-MM-dd"),
                P_FROM_AIRP = txtFROM_AIRP.Value.ToUpper(),
                P_TO_AIRP = txtTO_AIRP.Value.ToUpper(),
                P_OPER = txtOPER_Search.Value.ToUpper(),
                P_PAGAINDEX = 0,
                P_PAGESIZE = 100000
            });
            rptSource.DataSource = dt;
            rptSource.DataBind();
        }
        private List<perm_imp_month> GetListImport_VJC(string txt, string oper)
        {
            List<perm_imp_month> lis = new List<perm_imp_month>();
            string[] dongs = txt.ToUpper().Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime _date = DateTime.MinValue;
            string _craft = "";
            foreach (var dong in dongs)
            {
                
                string[] pts = dong.ToUpper().Split(new string[] { "\t", " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
                if (pts.Length == 7)
                    _craft = clsChuanHoaImport.ChuanHoaCraft(pts[0]);
                if (pts.Length < 2) {
                    try
                    {
                        _date = DateTimeHelper.ConvertToDateTime(pts[0]);
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        perm_imp_month obj = new perm_imp_month();
                        obj.CRAFT_TYPE = _craft;
                        obj.REGISTER_CRAFT = _craft;
                        obj.CALLSIGN = pts.Length == 7?pts[1]: pts[0];
                        obj.FROM_AIRP = pts.Length == 7?pts[2]:pts[1];
                        obj.TO_AIRP = pts.Length == 7 ? pts[3] : pts[2];
                        obj.ETD = clsChuanHoaImport.ChuanHoaGioBay(pts.Length == 7 ? pts[4] : pts[3]);
                        obj.ETA = clsChuanHoaImport.ChuanHoaGioBay(pts.Length == 7 ? pts[5] : pts[4]);
                        obj.OPER = oper;
                        obj.FLIGHT_DATE = _date;
                        //obj.STT = Convert.ToInt32(pts[0]);
                        lis.Add(obj);
                    }
                    catch
                    {


                    }

                }
            }
            return lis;
        }
        private List<perm_imp_month> GetListImport_HVN(string txt, string oper)
        {
            List<perm_imp_month> lis = new List<perm_imp_month>();
            string[] dongs = txt.ToUpper().Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime _date = DateTime.MinValue;
            foreach (var dong in dongs)
            {
                string[] pts = dong.ToUpper().Split(new string[] { "\t", " ", "-" }, StringSplitOptions.RemoveEmptyEntries);

                if (pts.Length < 2)
                    _date = DateTimeHelper.ConvertToDateTime(pts[0]);
                else
                {
                    try
                    {
                        perm_imp_month obj = new perm_imp_month();
                        obj.CRAFT_TYPE = clsChuanHoaImport.ChuanHoaCraft(pts[1]);
                        obj.REGISTER_CRAFT = pts[4];
                        string _callsign = pts[5];
                        _callsign = _callsign.Replace("VN", "HVN");
                        _callsign = _callsign.Replace("0V", "VFC");
                        obj.CALLSIGN = _callsign;
                        obj.FROM_AIRP = pts[7];
                        obj.TO_AIRP = pts[8];
                        obj.ETD = clsChuanHoaImport.ChuanHoaGioBay(pts[9]);
                        obj.ETA = clsChuanHoaImport.ChuanHoaGioBay(pts[10]);
                        obj.OPER = oper;
                        obj.FLIGHT_DATE = _date;
                        obj.STT = Convert.ToInt32(pts[0]);
                        lis.Add(obj);
                    }
                    catch
                    {

                        
                    }
                    
                }
            }
            return lis;
        }
        private List<perm_imp_month> GetListImport_PIC(string txt, string oper)
        {
            List<perm_imp_month> lis = new List<perm_imp_month>();
            string[] dongs = txt.ToUpper().Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime _date = DateTime.MinValue;
            foreach (var dong in dongs)
            {
                string[] pts = dong.ToUpper().Split(new string[] { "\t", " ", "-" }, StringSplitOptions.RemoveEmptyEntries);

                if (pts.Length < 2)
                    _date = DateTimeHelper.ConvertToDateTime(pts[0]);
                else
                {
                    try
                    {
                        perm_imp_month obj = new perm_imp_month();
                        obj.CRAFT_TYPE = clsChuanHoaImport.ChuanHoaCraft(pts[1]);
                        obj.REGISTER_CRAFT = pts[4];
                        obj.CALLSIGN = pts[5];
                        obj.FROM_AIRP = pts[7];
                        obj.TO_AIRP = pts[8];
                        obj.ETD = clsChuanHoaImport.ChuanHoaGioBay(pts[9]);
                        obj.ETA = clsChuanHoaImport.ChuanHoaGioBay(pts[10]);
                        obj.OPER = oper;
                        obj.FLIGHT_DATE = _date;
                        obj.STT = Convert.ToInt32(pts[0]);
                        lis.Add(obj);
                    }
                    catch
                    {


                    }

                }
            }
            return lis;
        }
        private string getContent(string txt)
        {
            string kq = "";
            string[] dongs = txt.ToUpper().Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in dongs)
            {
                if (!item.Contains("LICH_BAY") && !item.Contains("THEO GIO") && !item.Contains("AC_SUB") && !item.Contains("AC_TYPE") && !item.Contains("SECTOR") && !item.Contains("----"))
                    kq += $"\r\n{item}";

            }
            return kq;
        }
        #endregion

        
    }
}
