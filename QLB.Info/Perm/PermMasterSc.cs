using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class PermMasterSc
    {
        public Int64 ID { get; set; }
        public int PERM_ID { get; set; }
        public string PERMNBR_ID { get; set; }
        public string AUTHOR_ID { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHTTYPE { get; set; }
        public string PERMNBR { get; set; }
        public string VERSION { get; set; }
        public string SEASON { get; set; }
        public DateTime? PERMDATE { get; set; }
        public string OPER_ID { get; set; }
        public string REFERENCE { get; set; }
        public int VALIDHOURS { get; set; }
        public DateTime? BEGINDATE { get; set; }
        public DateTime? ENDDATE { get; set; }
        public DateTime? LASTMODIFY { get; set; }
        public string LASTUSER { get; set; }
        public string STATUS { get; set; }
        public string AUTHOR_NAME { get; set; }
        public string OPER_NAME { get; set; }
        public string BILLINGADDRESS { get; set; }
        public string PERMCONTENT { get; set; }
        public string LinkFiles { get; set; }

    }
    public class PermMasterSc_HIS: PermMasterSc
    {
        public string ACTION { get; set; }
        public string CONTENT { get; set; }
        public int NOVERSION { get; set; }
    }
}
