using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class PermDetailSc
    {
        public Int64 ID { get; set; }
        public Int64 FLIGHT_PK { get; set; }
        public Int64 PERM_ID { get; set; }
        public string PURPOSE_ID { get;set;}
        public Int64 CRAFT_ID { get; set; }
        //public CraftType CRAFT { get; set; }
        public int MTOW { get; set; }
        public string FLIGHTNBR { get; set; }
        public string REGISTRATION { get; set; }
        public string DAY1 { get; set; }
        public string DAY2 { get; set; }
        public string DAY3 { get; set; }
        public string DAY4 { get; set; }
        public string DAY5 { get; set; }
        public string DAY6 { get; set; }
        public string DAY7 { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public string VIA { get; set; }
        public string STATUS { get; set; }        
        public DateTime? LASTMODIFY { get; set; }
        public string LASTUSER { get; set; }
        private DateTime? _BEGINDATE;
        public DateTime? BEGINDATE { get { if (_BEGINDATE == DateTime.MinValue) return null; return _BEGINDATE; } set { _BEGINDATE = value; } }
        private DateTime? _ENDDATE;
        public DateTime? ENDDATE { get { if (_ENDDATE == DateTime.MinValue) return null; return _ENDDATE; } set { _ENDDATE = value; } }
        public string REMARK { get; set; }
        public string PERMNBR_ID { get; set; }
        public string PURPOSE_NAME { get; set; }
        public string CRAFT_NAME { get; set; }
        public string FROM_NAME { get; set; }
        public string TO_NAME { get; set; }
        public Int64 Record_Sum { get; set; }
        public Int64 RNUM { get; set; }

        public string _TAITRONG;
        public string TAITRONG
        {
            get
            {
                if (_TAITRONG == "0") return ""; return _TAITRONG ;
            }
            set
            {
                _TAITRONG = value;
            }
        }

    }
    public class PermDetailSc_HIS: PermDetailSc
    {
        public string ACTION { get; set; }
        public string CONTENT { get; set; }
        public int NOVESION { get; set; }
        public int NOVERSION { get { return NOVESION; } }
    }
    public class PermDetailSc_Search:PermDetailSc
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public Int64 rStart { get; set; }
        public Int64 rFinish { get; set; }
    }
}
