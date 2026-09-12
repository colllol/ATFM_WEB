using System;
using System.Collections.Generic;
using System.Text;

namespace prjInfo
{
    public class T_ImageFiles
    {
        #region Member variables and contructor
        protected Double _ID;
        protected Int16 _ImageType;
        protected String _ImageFileName = string.Empty;
        protected String _ImgeFilePath = string.Empty;
        protected Double _ImageFileSize;
        protected String _ImageFileExtension = string.Empty;
        protected DateTime _DateCreated;
        protected Double _UserCreated;
        protected Int16 _Status;
        protected Int16 _FileType;
        protected Double _Categorys_ID;
        protected Int32 _AuthorID;
        public T_ImageFiles()
        {
        }
        #endregion
        #region Public Properties
        public Double ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public Int16 ImageType
        {
            get { return _ImageType; }
            set { _ImageType = value; }
        }
        public String ImageFileName
        {
            get { return _ImageFileName; }
            set { _ImageFileName = value; }
        }
        public String ImgeFilePath
        {
            get { return _ImgeFilePath; }
            set { _ImgeFilePath = value; }
        }
        public Double ImageFileSize
        {
            get { return _ImageFileSize; }
            set { _ImageFileSize = value; }
        }
        public String ImageFileExtension
        {
            get { return _ImageFileExtension; }
            set { _ImageFileExtension = value; }
        }
        public DateTime DateCreated
        {
            get { return _DateCreated; }
            set { _DateCreated = value; }
        }
        public Double UserCreated
        {
            get { return _UserCreated; }
            set { _UserCreated = value; }
        }
        public Int16 Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public Int16 FileType
        {
            get { return _FileType; }
            set { _FileType = value; }
        }
        public Double Categorys_ID
        {
            get { return _Categorys_ID; }
            set { _Categorys_ID = value; }
        }
        public Int32 AuthorID
        {
            get { return _AuthorID; }
            set { _AuthorID = value; }
        }
        #endregion
    }
}
