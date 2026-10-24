using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class CantRealize
    {
        public Int64 ID { get; set; }
        public DateTime? LETTERNBR_PK { get; set; }
       
        private string _NBR;
        public string NBR { get { if (string.IsNullOrEmpty(_NBR)) return ""; return _NBR; } set { _NBR = value; } }

        private string _CONTENT;
        public string CONTENT { get { if (string.IsNullOrEmpty(_CONTENT)) return "";return _CONTENT; } set { _CONTENT = value; } }
        public string FROM_PL { get; set; }
    }
}
