using System;
namespace prjInfo
{
     [Serializable]
	public class T_UserMenu
	{
		#region Member variables and contructor
		protected int _UserMenu_ID;
		protected int _User_ID;
		protected int _Menu_ID;		
		protected DateTime _DateCreated;
		protected DateTime _DateModify;
		protected int _Group_ID;

        protected Boolean _RP_R;
        protected Boolean _RP_W;
        protected Boolean _RP_F;
    
		public T_UserMenu()
		{
		}
		#endregion
		#region Public Properties
        public Boolean RP_R
        {
            get { return _RP_R; }
            set { _RP_R = value; }
        }
        public Boolean RP_W
        {
            get { return _RP_W; }
            set { _RP_W = value; }
        }
        public Boolean RP_F
        {
            get { return _RP_F; }
            set { _RP_F = value; }
        }
		public int UserMenu_ID
		{
				get{return _UserMenu_ID;}
				set{_UserMenu_ID=value;}
		}
		public int User_ID
		{
				get{return _User_ID;}
				set{_User_ID=value;}
		}
		public int Menu_ID
		{
				get{return _Menu_ID;}
				set{_Menu_ID=value;}
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
		public int Group_ID
		{
				get{return _Group_ID;}
				set{_Group_ID=value;}
		}
		#endregion
		}
}
