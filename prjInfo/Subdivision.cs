using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    [Serializable]
    public class Subdivision
    {
        private int _ID { get; set; }
        private string _SUBDIVISION_NAME { get; set; }
        private string _DESCRIPTION { get; set; }
        private string _FIR_HN { get; set; }
        private string _FIR_HCM { get; set; }

        public int ID
        {
            get
            {
                return _ID; ;
            }

            set
            {
                _ID = value;
            }
        }
        public string SUBDIVISION_NAME
        {
            get
            {
                return _SUBDIVISION_NAME;
            }
            set
            {
                _SUBDIVISION_NAME = value;
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
    }
}
