using System;
using System.Collections.Generic;
using System.Text;

namespace prjInfo
{
    [Serializable]
    public class T_Users
    {
        #region Member variables and contructor
        protected Int32 _UserID;
        protected String _UserName = string.Empty;
        protected String _UserPass = string.Empty;
        protected String _UserFullName = string.Empty;
        protected String _UserEmail = string.Empty;
        protected String _UserMobile = string.Empty;
        protected String _UserAddress = string.Empty;
        protected DateTime _UserBirthday;
        protected Int32 _UserActive;
        protected DateTime _DateCreated;
        protected DateTime _DateModify;
        protected Int32 _UserCreate;
        protected Int32 _Group_ID;
        protected Int32 _IsReporter;
        protected String _RedirectPages = string.Empty;
        public T_Users()
        {
        }
        #endregion
        #region Public Properties
        public Int32 UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        public String RedirectPages
        {
            get { return _RedirectPages; }
            set { _RedirectPages = value; }
        }
        public String UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public String UserPass
        {
            get { return _UserPass; }
            set { _UserPass = value; }
        }
        public String UserFullName
        {
            get { return _UserFullName; }
            set { _UserFullName = value; }
        }
        public String UserEmail
        {
            get { return _UserEmail; }
            set { _UserEmail = value; }
        }
        public String UserMobile
        {
            get { return _UserMobile; }
            set { _UserMobile = value; }
        }
        public String UserAddress
        {
            get { return _UserAddress; }
            set { _UserAddress = value; }
        }
        public DateTime UserBirthday
        {
            get { return _UserBirthday; }
            set { _UserBirthday = value; }
        }
        public Int32 UserActive
        {
            get { return _UserActive; }
            set { _UserActive = value; }
        }
        public Int32 IsReporter
        {
            get { return _IsReporter; }
            set { _IsReporter = value; }
        }
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }
        public DateTime DateModify
        {
            get { return _DateModify; }
            set { _DateModify = value; }
        }
        public Int32 UserCreate
        {
            get { return _UserCreate; }
            set { _UserCreate = value; }
        }
        public Int32 Group_ID
        {
            get { return _Group_ID; }
            set { _Group_ID = value; }
        }
        #endregion       
    }
}
