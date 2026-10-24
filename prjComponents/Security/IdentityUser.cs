using System;
using System.Security.Principal;

namespace prjComponents
{
    public class IdentityUser : GenericIdentity
    {
        private int _ID;
        private string _Email = string.Empty;

        public IdentityUser(string name)
            : base(name)
        {
            SetUserData(name);
        }

        public IdentityUser(string name, string type)
            : base(name, type)
        {
            SetUserData(name);
        }

        public IdentityUser(int id, string email)
            : base(id.ToString())
        {
            string name = id + ";1;" + email;
            SetUserData(name);
        }

        private void SetUserData(string name)
        {
            string[] uData = name.Split(';');
            if (uData.GetUpperBound(0) == 2)
            {
                this._ID = Convert.ToInt32(uData[0]);
                // GlobalID is index 1;
                this._Email = uData[2];
            }
            else if (uData.GetUpperBound(0) == 0)
            {
                this._ID = 0;
                this._Email = "";
            }
            else
                throw new Exception("Invalid user");
        }

        public int ID
        {
            get { return this._ID; }
        }

        public string Email
        {
            get { return this._Email; }
        }

        public override string ToString()
        {
            return this._ID + ";1;" + this._Email;
        }
    }
}
