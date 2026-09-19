using System;
using System.Text;
using System.Data;
namespace prjInfo
{
	public class T_Menus
	{
		#region Member variables and contructor
		protected Int32 _ID;
		protected String _MenuName=string.Empty;
		protected String _MenuDesc=string.Empty;
		protected Int32 _MenuOrder;
		protected Int32 _ParrentID;
		protected DateTime _DateCreated;
		protected DateTime _DateModify;
		protected String _MenuURL=string.Empty;
		protected String _MenuIcon=string.Empty;
		protected Int32 _UserCreate;
        protected Int32 _UserModify;
		protected Int32 _isDisplay;
        protected Int32 _activeSync;
        protected Int32 _activeSyncImages;
		public T_Menus()
		{
		}
		#endregion
		#region Public Properties
		public Int32 ID
		{
				get{return _ID;}
				set{_ID=value;}
		}
		public String MenuName
		{
				get{return _MenuName;}
				set{_MenuName=value;}
		}
		public String MenuDesc
		{
				get{return _MenuDesc;}
				set{_MenuDesc=value;}
		}
		public Int32 MenuOrder
		{
				get{return _MenuOrder;}
				set{_MenuOrder=value;}
		}
		public Int32 ParrentID
		{
				get{return _ParrentID;}
				set{_ParrentID=value;}
		}
		public DateTime DateCreated
		{
				get{return _DateCreated;}
				set{_DateCreated=value;}
		}
		public DateTime DateModify
		{
				get{return _DateModify;}
				set{_DateModify=value;}
		}
		public String MenuURL
		{
				get{return _MenuURL;}
				set{_MenuURL=value;}
		}
		public String MenuIcon
		{
				get{return _MenuIcon;}
				set{_MenuIcon=value;}
		}
		public Int32 UserCreate
		{
				get{return _UserCreate;}
				set{_UserCreate=value;}
		}
        public Int32 UserModify
        {
            get { return _UserModify; }
            set { _UserModify = value; }
        }
		public Int32 isDisplay
		{
				get{return _isDisplay;}
				set{_isDisplay=value;}
		}
        /// <summary>
        /// Đồng bộ Text
        /// </summary>
        public Int32 ActiveSync
        {
            get { return _activeSync; }
            set { _activeSync = value; }
        }
        /// <summary>
        /// Đồng bộ ảnh
        /// </summary>
        public Int32 ActiveSyncImages
        {
            get { return _activeSyncImages; }
            set { _activeSyncImages = value; }
        }
		#endregion
	}
}
