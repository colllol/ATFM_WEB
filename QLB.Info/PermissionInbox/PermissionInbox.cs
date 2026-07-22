using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class PermissionInbox
    {
        public DateTime LETTERNBR_PK { get; set; }
        public string NBR { get; set; }
        public string CONTENT { get; set; }        
        public Int64 Record_Sum { get; set; }
    }
    public class clsSearchPermissionInbox : PermissionInbox
    {
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public int PAGE_INDEX { get; set; }
        public int PAGE_SIZE { get; set; }
    }
}
