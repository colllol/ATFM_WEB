using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class PermMasterNo
    {
        private DateTime? _PERMDATE;
        private DateTime? _LASTMODIFY;
        private string _BILLINGADDRESS;
        private string _PERMCONTENT;
        public Int64 ID { get; set; }
        public Int64 PERM_ID { get; set; }
        public string PERMNBR_ID { get; set; }
        public string AUTHOR_ID { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHTTYPE { get; set; }        
        public string PERMNBR { get; set; }
        public string VERSION { get; set; }
        public DateTime? PERMDATE { get { if (_PERMDATE == DateTime.MinValue) return null; return _PERMDATE; } set { _PERMDATE = value; } }
        public string OPER_ID { get; set; }
        public string REFERENCE { get; set; }
        public int VALIDHOURS { get; set; }
        public string STATUS { get; set; }
        public string LASTUSER { get; set; }
        public DateTime? LASTMODIFY { get { if (_LASTMODIFY == DateTime.MinValue) return null; return _LASTMODIFY; } set { _LASTMODIFY = value; } }
        public string AUTHOR_NAME { get; set; }
        public string OPER_NAME { get; set; }
        public string BILLINGADDRESS { get { if (string.IsNullOrEmpty(_BILLINGADDRESS)) return "";return _BILLINGADDRESS; } set { _BILLINGADDRESS = value; } }
        public string PERMCONTENT { get { if (string.IsNullOrEmpty(_PERMCONTENT)) return ""; return _PERMCONTENT; } set { _PERMCONTENT = value; } }
    }
}
