using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class FlyplanType
    {
        public DateTime LETTERNBR_PK { get; set; }

        public DateTime VALID_DATE { get; set; }
        public string NBR { get; set; }
        public string CONTENT3 { get; set; }
        public string CONTENT4 { get; set; }
        public Int64 ID { get; set; }
        public Int64 Record_Sum { get; set; }
    }
    public class clsSearchFlyplanType : FlyplanType
    {
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public int PAGE_INDEX { get; set; }
        public int PAGE_SIZE { get; set; }
    }
}
