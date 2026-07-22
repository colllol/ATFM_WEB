using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
   public class Users
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }        

        public string UserMobile { get; set; }

        public string UserAdress { get; set; }

        public DateTime UserBirthday { get; set; }
        public int UserActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModify { get; set; }
        public int UserCreate { get; set; }
        public int Group_Id { get; set; }
        public int IsReporter { get; set; }
        public string RedirectPages {get;set;}

    }
}
