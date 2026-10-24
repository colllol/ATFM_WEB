using System;
namespace prjInfo
{
    [Serializable]
    public class T_Languages
	{
		#region Member variables and contructor
		protected Int32 _Languages_ID;
		protected String _Languages_Name=string.Empty;
		protected String _Description=string.Empty;
		protected String _LangCode = string.Empty;
        protected String _Tab = string.Empty;
        protected String _CssName = string.Empty;
        protected String _BannerBackground = string.Empty;
		#endregion

		#region Public Properties
		public Int32 Languages_ID
		{
				get{return _Languages_ID;}
				set{_Languages_ID=value;}
		}
		public String Languages_Name
		{
				get{return _Languages_Name;}
				set{_Languages_Name=value;}
		}
		public String Description
		{
				get{return _Description;}
				set{_Description=value;}
		}
		public String Code
		{
			get{return _LangCode;}
			set{_LangCode=value;}
		}
        public String Tab
        {
            get { return _Tab; }
            set { _Tab = value; }
        }
        public String CssName
        {
            get { return _CssName; }
            set { _CssName = value; }
        }
        public String BannerBackground
        {
            get { return _BannerBackground; }
            set { _BannerBackground = value; }
        }
		#endregion
	}
}
