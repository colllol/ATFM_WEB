using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLB.Info
{
   public class GroupMenu
    {
        public int GROUPMENU_ID { get; set; }
        public int GROUP_ID { get; set; }
        public int MENU_ID { get; set; }
        public int R_EDIT { get; set; }
        public int R_DEL { get; set; }
        public int R_ADD { get; set; }
        public int R_PUB { get; set; }
        public DateTime DATECREATED { get; set; }
        public DateTime DATEMODIFY { get; set; }

    }
}
