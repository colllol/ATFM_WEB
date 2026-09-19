using System;
using System.Collections.Generic;
using System.Text;

namespace prjInfo
{
    public class T_RolePermission
    {
        #region Member variables and contructor
        private bool _R_Edit;
        private bool _R_Del;
        private bool _R_Add;
        private bool _R_Pub;
        #endregion

        #region Public Properties
        /// <summary>
        /// Quyền sửa
        /// </summary>
        public bool R_Edit
        {
            get { return _R_Edit; }
            set { _R_Edit = value; }
        }
        /// <summary>
        /// Quyền xóa
        /// </summary>
        public bool R_Del
        {
            get { return _R_Del; }
            set { _R_Del = value; }
        }

        /// <summary>
        /// Quyền thêm
        /// </summary>
        public bool R_Add
        {
            get { return _R_Add; }
            set { _R_Add = value; }
        }
        /// <summary>
        /// Quyền duyệt bài
        /// </summary>
        public bool R_Pub
        {
            get { return _R_Pub; }
            set { _R_Pub = value; }
        }
        #endregion
    }
}
