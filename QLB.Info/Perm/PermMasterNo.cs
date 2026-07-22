using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class PermMasterNo
    {
        public Int64 ID { get; set; }
        public Int64 PERM_ID { get; set; }
        public string PERMNBR_ID { get; set; }
        //public Fpauthor AUTHOR { get; set; }
        public string AUTHOR_ID { get; set; }
        public string PERMTYPE { get; set; }
        public string FLIGHTTYPE { get; set; }
        public string PERMNBR { get; set; }
        public string VERSION { get; set; }
        public DateTime PERMDATE { get; set; }
        public string OPER_ID { get; set; }
        public string REFERENCE { get; set; }
        public Int64 VALIDHOURS { get; set; }
        public string STATUS { get; set; }
        public string LASTUSER { get; set; }
        public DateTime LASTMODIFY { get; set; }
        public string AUTHOR_NAME { get; set; }
        public string OPER_NAME { get; set; }
        public string BILLINGADDRESS { get; set; }
        public string PERMCONTENT { get; set; }
    }
    public class PermMasterNo_HIS: PermMasterNo
    {
        public string ACTION { get; set; }
        public string CONTENT { get; set; }
        public int NOVERSION { get { return NOVESION; } }
        public int NOVESION { get; set; }
    }
}
