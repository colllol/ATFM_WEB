using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class PlanMessageNew
    {
        public DateTime FLIGHTDATE { get; set; }
        public Int64 PART_NO { get; set; }
        public string CONTENT { get; set; }
        public string MESS_TYPE { get; set; }
        public Int64 Record_Sum { get; set; }
    }
    public class clsSearchPlanMessageNew : PlanMessageNew
    {
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public int PAGE_INDEX { get; set; }
        public int PAGE_SIZE { get; set; }
    }
}
