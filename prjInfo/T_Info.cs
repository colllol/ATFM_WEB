using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prjInfo
{
    public class T_Info
    {
        #region Member variables and contructor
        protected Int32 _ID;
        protected String _Tittle = string.Empty;
        protected String _Contents = string.Empty;
        protected Int32 _Lang_ID;
        protected Int32 _Status;
        protected Boolean _Display;
        public T_Info()
        {
        }
        #endregion
        #region Public Properties
        public Int32 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public String Tittle
        {
            get { return _Tittle; }
            set { _Tittle = value; }
        }
        public String Contents
        {
            get { return _Contents; }
            set { _Contents = value; }
        }
        public Int32 Lang_ID
        {
            get { return _Lang_ID; }
            set { _Lang_ID = value; }
        }
        public Int32 Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public Boolean Display
        {
            get { return _Display; }
            set { _Display = value; }
        }
        #endregion
    }
}
