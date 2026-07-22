using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
    public class ReportingPoint
    {
        private string _COMPULSORY_ON_REQUEST;
        private string _IN_FIR_VN;
        private string _FIR_HN;
        private string _FIR_HCM;
        public int ID { get; set; }
        public string POINT_NAME { get; set; }
        public string COMPULSORY_ON_REQUEST {
            get
            {
                if (_COMPULSORY_ON_REQUEST == "1") return "1"; return "0";
            }
            set
            {
                _COMPULSORY_ON_REQUEST = value;
            }
        }
        public string DESCRIPTION { get; set; }
        public string IN_FIR_VN
        {
            get
            {
                if (_IN_FIR_VN == "1") return "1"; return "0";
            }
            set
            {
                _IN_FIR_VN = value;
            }
        }
        public string FIR_HN
        {
            get
            {
                if (_FIR_HN == "1") return "1"; return "0";
            }
            set
            {
                _FIR_HN = value;
            }
        }
        public string FIR_HCM
        {
            get
            {
                if (_FIR_HCM == "1") return "1"; return "0";
            }
            set
            {
                _FIR_HCM = value;
            }
        }
        public int POINT_ID { get; set; }
        public string MARK { get; set; }
        public Int64 ROUTE_ID { get; set; }

    }
}
