using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class M_VIA_ARIPORT
    {
        public Int64 ID { get; set; }
        public string FROM_AIRP { get; set; }
        public string TO_AIRP { get; set; }
        public string VIA { get; set; }
        public string PERMTYPE { get; set; }

    }
    public class M_VIA_ARIPORTSearch : M_VIA_ARIPORT
    {
        public Int64 PageSize { get; set; }
        public Int64 PageIndex { get; set; }
        public Int64 Record_Sum { get; set; }

    }
}
