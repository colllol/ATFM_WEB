using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
  public  class Menus
    {
        public int ID { get; set; }
        public string MenuName { get; set; }
        public string MenuDesc { get; set; }
        public int MenuOrder { get; set; }
        public int ParrentID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModify { get; set; }
        public string MenuURL { get; set; }
        public string MenuIcon { get; set; }
        public int UserCreate { get; set; }
        public int UserModify { get; set; }
        public int isDisplay { get; set; }
        public int ActiveSync { get; set; }
        public int ActiveSyncImages { get; set; }
    }
}
