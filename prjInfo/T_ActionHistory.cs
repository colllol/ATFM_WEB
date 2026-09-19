using System;
namespace prjInfo
{
    [Serializable]
	public class T_ActionHistory
	{
		#region Member variables and contructor
		protected Double _ID;
		protected Double _UserID;
		protected String _FullName=string.Empty;
		protected String _HostIP=string.Empty;
		protected DateTime _DateModify;
		protected String _ActionsCode=string.Empty;
		protected Double _News_ID;
		protected String _Notes=string.Empty;
        protected int _MenuID;//added by haolm
		public T_ActionHistory()
		{
		}
		#endregion
		#region Public Properties
		public Double ID
        {
				get{return _ID; }
				set{ _ID = value;}
		}
		public Double UserID
		{
				get{return _UserID;}
				set{_UserID=value;}
		}
		public String FullName
		{
				get{return _FullName;}
				set{_FullName=value;}
		}
		public String HostIP
		{
				get{return _HostIP;}
				set{_HostIP=value;}
		}
		public DateTime DateModify
		{
				get{return _DateModify;}
				set{_DateModify=value;}
		}
		public String ActionsCode
		{
				get{return _ActionsCode;}
				set{_ActionsCode=value;}
		}
		public Double News_ID
		{
				get{return _News_ID;}
				set{_News_ID=value;}
		}
		public String Notes
		{
				get{return _Notes;}
				set{_Notes=value;}
		}
       
        public int Menu_ID // added by haolm
        {
            get { return _MenuID; }
            set { _MenuID = value; }
        }
         
		#endregion
		}
}
