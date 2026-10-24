using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class FlyPurpose
    {
        private int _ID;
        private string _PURPOSE_CODE;
        private string _PURPOSE_NAME;

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

        public string PURPOSE_CODE
        {
            get
            {
                return _PURPOSE_CODE;
            }

            set
            {
                _PURPOSE_CODE = value;
            }
        }

        public string PURPOSE_NAME
        {
            get
            {
                return _PURPOSE_NAME;
            }

            set
            {
                _PURPOSE_NAME = value;
            }
        }
    }
}
