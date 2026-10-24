using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    [Serializable]
    public class Ctry
    {
        public int ID { get; set; }
        public string CTR_CODE { get; set; }
        public string CTR_ENAME { get; set; }
    }
}
