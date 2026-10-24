using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class ReportingPoint
    {
        private int _ID;
        private string _POINT_NAME;
        private string _COMPULSORY_ON_REQUEST;
        private string _DESCRIPTION;
        private string _IN_FIR_VN;
        private string _FIR_HN;
        private string _FIR_HCM;
        private int _POINT_ID;
        public string MARK { get; set; }
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        public string POINT_NAME
        {
            get
            {
                return _POINT_NAME;
            }
            set
            {
                _POINT_NAME = value;
            }
        }
        public string COMPULSORY_ON_REQUEST
        {
            get
            {
                return _COMPULSORY_ON_REQUEST;
            }
            set
            {
                _COMPULSORY_ON_REQUEST = value;
            }
        }
        public string DESCRIPTION
        {
            get
            {
                return _DESCRIPTION;
            }
            set
            {
                _DESCRIPTION = value;
            }
        }
        public string IN_FIR_VN
        {
            get
            {
                return _IN_FIR_VN;
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
                return _FIR_HN;
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
                return _FIR_HCM;
            }
            set
            {
                _FIR_HCM = value;
            }
        }
        public int POINT_ID
        {
            get
            {
                return _POINT_ID;
            }
            set
            {
                _POINT_ID = value;
            }
        }
    }
}
