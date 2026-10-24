using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class PermMasterSc
    {
        private DateTime? _PERMDATE;
        private DateTime? _BEGINDATE;
        private DateTime? _ENDDATE;
        private DateTime? _LASTMODIFY;
        public Int64? ID { get; set; }
        public int PERM_ID { get; set; }
        public string PERMNBR_ID { get; set; }
        public string AUTHOR_ID { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHTTYPE { get; set; }
        public string PERMNBR { get; set; }
        public string VERSION { get; set; }
        public string SEASON { get; set; }
        public DateTime? PERMDATE { get { if (_PERMDATE == DateTime.MinValue) return null; return _PERMDATE; }set { _PERMDATE = value; } }
        public string OPER_ID { get; set; }
        public string REFERENCE { get; set; }
        public int VALIDHOURS { get; set; }
        public DateTime? BEGINDATE { get { if (_BEGINDATE == DateTime.MinValue) return null; return _BEGINDATE; }set { _BEGINDATE = value; } }
        public DateTime? ENDDATE { get { if (_ENDDATE == DateTime.MinValue) return null; return _ENDDATE; }set { _ENDDATE = value; } }
        public DateTime? LASTMODIFY { get { if (_LASTMODIFY == DateTime.MinValue) return null;return _LASTMODIFY; }set { _LASTMODIFY = value; } }
        public string LASTUSER { get; set; }
        public string STATUS { get; set; }
        public string AUTHOR_NAME { get; set; }
        public string OPER_NAME { get; set; }
        public string BILLINGADDRESS { get; set; }
        public string PERMCONTENT { get; set; }
        public string LinkFiles { get; set; }
    }
}
