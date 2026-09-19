using System;
namespace prjInfo
{
    [Serializable]
	public class T_Groups
	{
		#region Member variables and contructor
		protected int _Group_ID;
		protected string _Group_Name=string.Empty;
		protected string _Group_Description=string.Empty;
		protected DateTime _DateCreated;
		protected DateTime _DateModify;
		public T_Groups()
		{
		}
		#endregion
		#region Public Properties
		public int Group_ID
		{
				get{return _Group_ID;}
				set{_Group_ID=value;}
		}
		public string Group_Name
		{
				get{return _Group_Name;}
				set{_Group_Name=value;}
		}
		public string Group_Description
		{
				get{return _Group_Description;}
				set{_Group_Description=value;}
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
