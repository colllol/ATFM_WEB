using System;
namespace prjInfo
{
    [Serializable]
	public class T_GroupMenu
	{
		#region Member variables and contructor
    
		protected int _GroupMenu_ID;
        protected int _Group_ID;
        protected int _Menu_ID;
		protected DateTime _DateCreated;
		protected DateTime _DateModify;

        protected Boolean _R_Edit;
        protected Boolean _R_Del;
        protected Boolean _R_Add;
        protected Boolean _R_Pub;
		public T_GroupMenu()
		{
		}
		#endregion
		#region Public Properties
        public Boolean R_Edit
        {
            get { return _R_Edit; }
            set { _R_Edit = value; }
        }
        public Boolean R_Del
        {
            get { return _R_Del; }
            set { _R_Del = value; }
        }
        public Boolean R_Add
        {
            get { return _R_Add; }
            set { _R_Add = value; }
        }
        public Boolean R_Pub
        {
            get { return _R_Pub; }
            set { _R_Pub = value; }
        }
        public int GroupMenu_ID
		{
				get{return _GroupMenu_ID;}
				set{_GroupMenu_ID=value;}
		}
       
        public int Group_ID
		{
				get{return _Group_ID;}
				set{_Group_ID=value;}
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
		#endregion
		}
}
