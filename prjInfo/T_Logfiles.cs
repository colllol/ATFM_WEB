using System;
namespace prjInfo
{
   [Serializable]
	public class T_Logfiles
	{
		#region Member variables and contructor
		protected Double _ID;
		protected Double _UserID;
		protected String _UserName=string.Empty;
		protected DateTime _DateLogin;
		protected String _TimeLogin=string.Empty;
		protected String _LoginStatus=string.Empty;
		protected String _SessionID=string.Empty;
		protected DateTime _DateLogout;
		protected String _TimeLogout=string.Empty;
		protected String _Remote_Host=string.Empty;
		public T_Logfiles()
		{
		}
		#endregion
		#region Public Properties
		public Double ID
		{
				get{return _ID;}
				set{_ID=value;}
		}
		public Double UserID
		{
				get{return _UserID;}
				set{_UserID=value;}
		}
		public String UserName
		{
				get{return _UserName;}
				set{_UserName=value;}
		}
		public DateTime DateLogin
		{
				get{return _DateLogin;}
				set{_DateLogin=value;}
		}
		public String TimeLogin
		{
				get{return _TimeLogin;}
				set{_TimeLogin=value;}
		}
		public String LoginStatus
		{
				get{return _LoginStatus;}
				set{_LoginStatus=value;}
		}
		public String SessionID
		{
				get{return _SessionID;}
				set{_SessionID=value;}
		}
		public DateTime DateLogout
		{
				get{return _DateLogout;}
				set{_DateLogout=value;}
		}
		public String TimeLogout
		{
				get{return _TimeLogout;}
				set{_TimeLogout=value;}
		}
		public String Remote_Host
		{
				get{return _Remote_Host;}
				set{_Remote_Host=value;}
		}
		#endregion
		}
}
