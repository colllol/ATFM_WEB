using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjInfo
{
    public class Aftn
    {
        private int _ID;
        private string _GROUP_NAME;
        private string _DECRIPTION;
        private int _G_A_M_ID;

        public int G_A_M_ID
        {
            get
            {
                return _G_A_M_ID;
            }

            set
            {
                _G_A_M_ID = value;
            }
        }

        public string DECRIPTION
        {
            get
            {
                return _DECRIPTION;
            }

            set
            {
                _DECRIPTION = value;
            }
        }

        public string GROUP_NAME
        {
            get
            {
                return _GROUP_NAME;
            }

            set
            {
                _GROUP_NAME = value;
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
