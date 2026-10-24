using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class FpAuthor
    {
        private int _ID;
        private string _AUTHOR_CODE;
        private string _AUTHOR_NAME;

        public string AUTHOR_NAME
        {
            get
            {
                return _AUTHOR_NAME;
            }

            set
            {
                _AUTHOR_NAME = value;
            }
        }

        public string AUTHOR_CODE
        {
            get
            {
                return _AUTHOR_CODE;
            }

            set
            {
                _AUTHOR_CODE = value;
            }
        }

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
    }
}
