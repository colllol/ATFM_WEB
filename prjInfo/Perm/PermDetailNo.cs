using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class PermDetailNo
    {
        private DateTime? _LASTMODIFY;
        private DateTime? _MAX_DATE;
        public Int64 ID { get; set; }
        public Int64 FLIGHT_PK { get; set; }
        public Int64 PERM_ID { get; set; }
        public Int64 CRAFT_ID { get; set; }
        public Int64 MTOW { get; set; }
        public string DAYSFLIGHT { get; set; }
        public string FLIGHTNBR { get; set; }
        public string REGISTRATION { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string VIA { get; set; }
        public string STATUS { get; set; }
        public DateTime? LASTMODIFY { get { if (_LASTMODIFY == DateTime.MinValue) return null; return _LASTMODIFY; }set { _LASTMODIFY = value; } }
        public string LASTUSER { get; set; }
        public string PURPOSE_ID { get; set; }
        public DateTime? MAX_DATE { get { if (_MAX_DATE == DateTime.MinValue) return null; return _MAX_DATE; } set { _MAX_DATE = value; } }
        public string REMARK { get; set; }
        public string PURPOSE_NAME { get; set; }
        public string CRAFT_NAME { get; set; }
        public string AE_NAME { get; set; }
        public string FROM_NAME { get; set; }
        public string TO_NAME { get; set; }
        public string PERMNBR_ID { get; set; }
        public string REMARK_SEND { get; set; }
        public Int64 Record_Sum { get; set; }
    }
    public class PermDetailNo_Search : PermDetailNo
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}
