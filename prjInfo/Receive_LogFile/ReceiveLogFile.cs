using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class ReceiveLogFile
    {
        public DateTime? LETTERNBR_PK { get; set; }
        public string NBR { get; set; }
        public string CONTENT { get; set; }
        public string TYPE { get; set; }
        public string FROM_PL { get; set; }
        public string C_R__C_S { get; set; }
        public string ORIGIN { get; set; }
        public Int64 ID { get; set; }
        public Int64 Record_Sum { get; set; }
    }
    public class clsSearchReceiveLogFile : ReceiveLogFile
    {
        public string MessageType { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public int PAGE_INDEX { get; set; }
        public int PAGE_SIZE { get; set; }
    }
}
