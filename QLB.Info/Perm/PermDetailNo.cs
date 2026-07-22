using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class PermDetailNo
    {
        public Int64 ID { get; set; }
        public Int64 FLIGHT_PK { get; set; }
        public Int64 PERM_ID { get; set; }
        public Int64 CRAFT_ID { get; set; }
        public int MTOW { get; set; }
        public string DAYSFLIGHT { get; set; }
        public string FLIGHTNBR { get; set; }
        public string REGISTRATION { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string VIA { get; set; }
        public string STATUS { get; set; }
        public DateTime LASTMODIFY { get; set; }
        public string LASTUSER { get; set; }
        public string PURPOSE_ID { get; set; }
        public DateTime MAX_DATE { get; set; }
        public string REMARK { get; set; }
        public string PURPOSE_NAME { get; set; }
        public string CRAFT_NAME { get; set; }  
        public string FROM_NAME { get; set; }
        public string TO_NAME { get; set; }
        public string PERMNBR_ID { get; set; }
        public string REMARK_SEND { get; set; }
        public Int64 Record_Sum { get; set; }

        public string _TAITRONG;

        public string TAITRONG
        {
            get
            {
                if (_TAITRONG == "0") return ""; return _TAITRONG;
            }
            set
            {
                _TAITRONG = value;
            }
        }
    }
    public class PermDetailNo_HIS: PermDetailNo
    {
        public string ACTION { get; set; }
        public string CONTENT { get; set; }
        public int NOVESION { get; set; }
        public int NOVERSION { get { return NOVESION; } }
    }
    public class PermDetailNo_Search:PermDetailNo
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public Int64 rStart { get; set; }
        public Int64 rFinish { get; set; }
    }
}
