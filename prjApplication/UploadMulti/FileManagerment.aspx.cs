using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace prjApplication.UploadMulti
{
    public partial class FileManagerment : System.Web.UI.Page
    {
        private string strRootPathVirtual;
        public string strNumberArg = "";
        public string strKeyLogo = "1";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //Add News BOCT 26.11.2009
            //string jscript = "function UploadComplete(){" + ClientScript.GetPostBackEventReference(LinkButton1, "") + "};";
           // Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FileCompleteUpload", jscript, true);
            Session["temp"] = "hi";
            //flashUpload.QueryParameters = "User=1B229D2DD798CADBDAFB24241FDD2C1418F4E79EA4F73613699E6429B6025A6EF4FE5C30006EB2E4D381F15C1AAB038D5A93E52F0B982730601B3D6E326B5227&s=" + Request["vType"] + "";
            //END
            strNumberArg = Request.QueryString["vType"].ToString();
            
            if (!this.IsPostBack)
            {
                // Load Ngay thang nam
                cbo_Nam.Items.Clear();
                cbo_Nam.Items.Add(DateTime.Now.Year.ToString());
                for (int i = 1; i < 5; i++)
                {
                    cbo_Nam.Items.Add(Convert.ToString(DateTime.Now.Year - i));
                }
                cbo_Nam.SelectedIndex = 0;

                // Load Ngay thang nam
                cbo_Thang.Items.Clear();
                //cbo_Thang.Items.Add(DateTime.Now.Year.ToString());
                for (int i = 1; i <= 12; i++)
                {
                    cbo_Thang.Items.Add(i.ToString());
                }
                cbo_Thang.SelectedValue = DateTime.Now.Month.ToString();

                // Load Ngay thang nam
                combo_Ngay.Items.Clear();
                //cbo_Thang.Items.Add(DateTime.Now.Year.ToString());
                for (int i = 1; i <= 31; i++)
                {
                    combo_Ngay.Items.Add(i.ToString());
                }
                combo_Ngay.SelectedValue = DateTime.Now.Day.ToString();

                strRootPathVirtual = "/Upload/";

                strRootPathVirtual = strRootPathVirtual + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
                CreateFolderByUserName(strRootPathVirtual);
                lblFolder2Upload.Text = strRootPathVirtual;
                // get all File in current directory
                DisplayAllFolder(strRootPathVirtual);
                DisplayAllFiles(strRootPathVirtual);
            }
        }
        #region "File process"
        public void DisplayAllFiles(string strVirtualDirectory)
        {
            DataTable myDataTable;
            myDataTable = ReadAllFile2DataTable(strVirtualDirectory);
            DataView sortedView = new DataView(myDataTable);
            sortedView.Sort = "Created, FileName asc";

            dlImages.RepeatColumns = 4;
            dlImages.DataSource = sortedView;
            dlImages.DataBind();

            sortedView = null;
            myDataTable = null;
        }
        private DataTable ReadAllFile2DataTable(string strVirtualPath)
        {
            //string strPhysicalPath = Server.MapPath(strVirtualPath);
            string strExtenion = "";
            string strPhysicalPath = Server.MapPath(strVirtualPath);
            FileInfo[] fa;
            DirectoryInfo di = new DirectoryInfo(strPhysicalPath);
            fa = di.GetFiles();

            DataTable dt = GetFileInfoTable();
            dt.BeginLoadData();
            foreach (FileInfo f in fa)
            {
                strExtenion = f.Extension.ToLower();
                if (Convert.ToInt32(strNumberArg) == 1)
                {
                    if (strExtenion == ".gif" || strExtenion == ".png" || strExtenion == ".docx" || strExtenion == ".jpg" || strExtenion == ".bmp" || strExtenion == ".jpeg" || strExtenion == ".doc" || strExtenion == ".xls" || strExtenion == ".xlsx")
                        AddRowToFileInfoTable(f, dt);
                }
                else
                {
                    AddRowToFileInfoTable(f, dt);
                }
            }
            dt.EndLoadData();
            dt.AcceptChanges();
            return dt;
        }

        private DataTable GetFileInfoTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("FileName", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("FilePath", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("FileExtension", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Size", Type.GetType("System.Int64")));
            dt.Columns.Add(new DataColumn("Modified", Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("Created", Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("FileView", Type.GetType("System.String")));
            return dt;
        }
        private void AddRowToFileInfoTable(FileSystemInfo fi, DataTable dt)
        {
            string PicturePreview = Server.MapPath("~/images/PicturePreview/");
            string strExtenion = fi.Extension.ToLower();
            DataRow dr = dt.NewRow();
            dr["FileName"] = fi.Name;
            if (strExtenion == ".gif" || strExtenion == ".png" || strExtenion == ".jpg" || strExtenion == ".bmp")
                dr["FilePath"] = Path.GetFullPath(fi.FullName);
            else
                dr["FilePath"] = Path.GetFullPath(PicturePreview + "\\" + strExtenion.Replace(".", "").Trim() + ".gif");

            dr["FileExtension"] = Path.GetExtension(fi.Name);
            dr["Size"] = new FileInfo(fi.FullName).Length;
            dr["Modified"] = fi.LastWriteTime;
            dr["Created"] = fi.CreationTime;
            dr["FileView"] = Path.GetFullPath(fi.FullName);
            dt.Rows.Add(dr);
        }
        #endregion
        #region "Folder process"
        private void CreateFolderByUserName(string FolderName)
        {
            string strRootPath = "";
            strRootPath = Server.MapPath(FolderName);
            if (Directory.Exists(strRootPath) == false) Directory.CreateDirectory(strRootPath);

        }
        private bool checkFolderExist(string FolderName)
        {
            string strRootPath = "";
            strRootPath = Server.MapPath(FolderName);
            if (Directory.Exists(strRootPath))
                return true;
            else
                return false;

        }
        private void DisplayAllFolder(string strVirtualDirectory)
        {
            DataTable myDataTable;
            myDataTable = ReadAllFolder2DataTable(strVirtualDirectory);
            DataView sortedView = new DataView(myDataTable);
            sortedView.Sort = "Created, FileName asc";

            dlFolder.RepeatColumns = 4;
            dlFolder.DataSource = sortedView;
            dlFolder.DataBind();

            sortedView = null;
            myDataTable = null;
        }
        private DataTable ReadAllFolder2DataTable(string strVirtualPath)
        {
            //string strPhysicalPath = Server.MapPath(strVirtualPath);
            string strPhysicalPath = Server.MapPath(strVirtualPath);
            DirectoryInfo[] da;
            DirectoryInfo di = new DirectoryInfo(strPhysicalPath);
            da = di.GetDirectories();
            DataTable dt = GetFolderInfoTable();

            dt.BeginLoadData();
            foreach (DirectoryInfo d in da)
            {
                AddRowToFolderInfoTable(d, dt);
            }
            dt.EndLoadData();
            dt.AcceptChanges();
            return dt;
        }

        private DataTable GetFolderInfoTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("FileName", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("FilePath", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("FileExtension", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Size", Type.GetType("System.Int64")));
            dt.Columns.Add(new DataColumn("Modified", Type.GetType("System.DateTime")));
            dt.Columns.Add(new DataColumn("Created", Type.GetType("System.DateTime")));
            return dt;
        }
        private void AddRowToFolderInfoTable(FileSystemInfo fi, DataTable dt)
        {
            DataRow dr = dt.NewRow();
            dr["FileName"] = fi.Name;
            dr["FilePath"] = Path.GetFullPath(fi.FullName);
            dr["FileExtension"] = Path.GetExtension(fi.Name);
            dr["Size"] = 0;
            dr["Modified"] = fi.LastWriteTime;
            dr["Created"] = fi.CreationTime;
            dt.Rows.Add(dr);
        }
        #endregion

        #region "Show Alert Message"
        public void Show(string message)
        {
            // Cleans the message to allow single quotation marks 
            string cleanMessage = message.Replace("'", "\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page 
            Page page = HttpContext.Current.Handler as Page;
            // Checks if the handler is a Page and that the script isn't allready on the Page 
            if (page != null && !page.IsClientScriptBlockRegistered("alert"))
            {
                page.RegisterClientScriptBlock("alert", script);
            }

        }

        #endregion
        #region "Upload process"
        private void SaveFile(string strDestDirectory)
        {

            string strClientFullPath = "";
            string strFileName = null;
            string strInputFilePath = null;
            if (strDestDirectory == "" || strDestDirectory == null)
            {
                Show("Mời bạn chọn thư mục để Upload.");
                return;
            }
            else
            {
                strInputFilePath = Server.MapPath(strDestDirectory);
                if (Directory.Exists(strInputFilePath) == false) Directory.CreateDirectory(strInputFilePath);
            }
            if (txtFile.PostedFile.FileName != null && txtFile.PostedFile.FileName != "")
            {
                strClientFullPath = txtFile.PostedFile.FileName;
                char[] ch = { '\\' };
                string[] arr = strClientFullPath.Split(ch);
                strFileName = arr[arr.Length - 1];
                strFileName = ProcessExistFileName(strFileName, strDestDirectory);
                txtFile.PostedFile.SaveAs(strInputFilePath + "\\" + strFileName);
                Show("Cập nhật thành công!");
            }
            else
            {
                Show("Mời bạn chọn một File để Upload.");
            }
        }
        private string ProcessExistFileName(string strFileName, string strDestDir)
        {
            string strFileTempName = "";
            string strInputFilePath = "";
            strInputFilePath = Server.MapPath(strDestDir) + "\\" + strFileName;

            if (System.IO.File.Exists(strInputFilePath))
            {
                string strFile = strFileName;
                string strFileExtension = strFile.Substring(strFile.LastIndexOf('.'));
                string strFileName1 = strFile.Substring(0, strFile.LastIndexOf('.'));
                strFileTempName = strFileName1 + "-" + DateTime.Now.Ticks.ToString() + strFileExtension;
            }
            else
                strFileTempName = strFileName;

            return strFileTempName;

        }
        #endregion
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlFolder.EditCommand += new DataListCommandEventHandler(dlFolder_EditCommand);
            this.btnSearchFolder.Click += new EventHandler(btnSearchFolder_Click);
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            this.Load += new System.EventHandler(this.Page_Load);

            this.btnDeleteFile.Click += new EventHandler(btnDeleteFile_Click);

        }
        #endregion
        /// <summary>
        /// BOCT ADD NEWS
        /// </summary>
        /// <returns></returns>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            strRootPathVirtual = "/Upload/";

            strRootPathVirtual = strRootPathVirtual + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
            DisplayAllFolder(strRootPathVirtual);
            DisplayAllFiles(strRootPathVirtual);
        }
        protected string GetFlashVars()
        {
            return "?" + Server.UrlEncode(Request.QueryString.ToString());
        }
        private void btnSearchFolder_Click(object sender, System.EventArgs e)
        {

            string strPhysLocal="";
            try
            {
                strPhysLocal = "/Upload/";

                strPhysLocal = strPhysLocal + cbo_Nam.SelectedValue.ToString() + "/" + cbo_Thang.SelectedValue.ToString() + "/" + combo_Ngay.SelectedValue.ToString() + "/";
                //Show(strPhysLocal);
                if (checkFolderExist(strPhysLocal))
                {
                    DisplayAllFolder(strPhysLocal);
                    DisplayAllFiles(strPhysLocal);
                    lblFolder2Upload.Text = strPhysLocal;
                }
                else
                {
                    Show("Thư mục này chưa tồn tại!");
                }

            }
            catch (Exception ex)
            {
                Show(ex.Message.ToString());
            }

        }
        private void btnUpload_Click(object sender, System.EventArgs e)
        {
            string strPhysFullPath = "";
            //strPhysFullPath = "/Upload/" + user.UserName.ToString() + "/";
            strPhysFullPath = "/Upload/";//+ user.UserName.ToString() + "/";
            strPhysFullPath = strPhysFullPath + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/";
            SaveFile(strPhysFullPath);
            DisplayAllFiles(strPhysFullPath);
            DisplayAllFolder(strPhysFullPath);
            lblFolder2Upload.Text = strPhysFullPath;
            cbo_Nam.SelectedValue = DateTime.Now.Year.ToString();
            cbo_Thang.SelectedValue = DateTime.Now.Month.ToString();
            combo_Ngay.SelectedValue = DateTime.Now.Day.ToString();
        }

        protected void btnDeleteFile_Click(object sender, EventArgs e)
        {
            string strPhysLocal = "";
            try
            {
                strPhysLocal = "/Upload/";

                if (dlImages.Items.Count > 0)
                {
                    int countfile = 0;
                    foreach (DataListItem item in dlImages.Items)
                    {
                        CheckBox chkDelete = (CheckBox)item.FindControl("chkDelete");
                        if (chkDelete.Checked)
                        {
                            Label lblFilePath = (Label)item.FindControl("lblFilePath");
                            File.Delete(lblFilePath.Text);
                            countfile++;
                        }
                    }
                    if (countfile > 0)
                        Show("Đã xóa " + countfile + " ảnh");
                    else
                        Show("Không có ảnh nào được chọn");
                }
                else
                {
                    Show("Thư mục rỗng");
                }

                strPhysLocal = strPhysLocal + cbo_Nam.SelectedValue.ToString() + "/" + cbo_Thang.SelectedValue.ToString() + "/" + combo_Ngay.SelectedValue.ToString() + "/";
                //Show(strPhysLocal);
                if (checkFolderExist(strPhysLocal))
                {
                    DisplayAllFolder(strPhysLocal);
                    DisplayAllFiles(strPhysLocal);
                    lblFolder2Upload.Text = strPhysLocal;
                }
                else
                {
                    Show("Thư mục này chưa tồn tại!");
                }

            }
            catch (Exception ex)
            {
                Show(ex.Message.ToString());
            }

        }

        public string UrlPathImage_Display(object PhysPathFull)
        {
            string strLocalRootPath = Server.MapPath("/Upload/");
            //string PicturePreview = Server.MapPath("~");
            string strTemp = "";
            //strTemp = WebPath(PhysPathFull.ToString());
            strTemp =  "/upload/PicturePreview/" + PhysPathFull.ToString().Replace(".","") + ".gif";

            return strTemp;
        }
        /// <summary>
        /// Remove Upload
        /// </summary>
        /// <param name="PhysPathFull"></param>
        /// <returns></returns>
        public string UrlPathImage_RemoveUpload(object PhysPathFull)
        {
            string PicturePreview = Server.MapPath("~");
            string strLocalRootPath = Server.MapPath("/Upload/");
            string strTemp = "";
            strTemp = WebPath(PhysPathFull.ToString());
            //if (strTemp.ToLower().IndexOf("upload") > 0)
            strTemp = strTemp.Substring(strLocalRootPath.Length);
            //else
            //strTemp = strTemp.Substring(PicturePreview.Length);
            strTemp = "/" + strTemp;
            return strTemp;
        }
        private string WebPath(string path1)
        {
            string strTemp = path1.Replace("\\", "/");
            return strTemp;
        }
        private string UrlPathImage(string PhysPathFull)
        {
            string strLocalRootPath = Server.MapPath("/Upload/");
            string strTemp = "";
            strTemp = WebPath(PhysPathFull);
            strTemp = strTemp.Substring(strLocalRootPath.Length);
            return strTemp;
        }

        private void dlFolder_EditCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            if (e.CommandName.ToString().ToLower() == "edit")
            {
                lblFolder2Upload.Text = UrlPathImage_Display(this.dlFolder.DataKeys[e.Item.ItemIndex].ToString());
                DisplayAllFolder(lblFolder2Upload.Text);
                DisplayAllFiles(lblFolder2Upload.Text);
            }
        }
    }
}
