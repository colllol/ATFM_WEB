using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace prjBusinessLogic
{
    public class FTPClass
    {
        public class FtpState
        {
            private ManualResetEvent wait;
            private FtpWebRequest request;
            private string fileName;
            private int _ftpPort;
            private Exception operationException = null;
            string status;

            byte[] _FileSize;

            public FtpState()
            {
                wait = new ManualResetEvent(false);
            }

            public ManualResetEvent OperationComplete
            {
                get { return wait; }
            }

            public FtpWebRequest Request
            {
                get { return request; }
                set { request = value; }
            }

            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }
            public int FptPort
            {
                get { return _ftpPort; }
                set { _ftpPort = value; }
            }
            public Exception OperationException
            {
                get { return operationException; }
                set { operationException = value; }
            }
            public string StatusDescription
            {
                get { return status; }
                set { status = value; }
            }

            public byte[] Filesize
            {
                get { return _FileSize; }
                set { _FileSize = value; }
            }
        }
        public void CreateFTPDirectory(string directory, string ftpUser, string ftpPassword)
        {

            try
            {
                //create the directory
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri(directory));
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                }
                else
                {
                    response.Close();
                }
            }

        }

        public bool FtpDirectoryExists(string directoryPath, string ftpUser, string ftpPassword)
        {
            
            /* Create an FTP Request */
            FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(directoryPath);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            try
            {
                using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            /* Resource Cleanup */
            finally
            {
                ftpRequest = null;
            }
        }
       
        public void AsynchronousUpload(string destinationPath, string sourcePath, string userName, string password, byte[] Filesize)
        {

            ManualResetEvent waitObject;

            Uri target = new Uri(destinationPath);
            string fileName = sourcePath;
            FtpState state = new FtpState();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.KeepAlive = false;
            request.Proxy = null;

            if (!(string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password)))
            {
                request.Credentials = new NetworkCredential(userName, password);
            }

            state.Request = request;
            state.FileName = fileName;
            state.Filesize = Filesize;
            waitObject = state.OperationComplete;

            request.BeginGetRequestStream(
                new AsyncCallback(EndGetStreamCallback),
                state
            );

            waitObject.WaitOne();

            //if (state.OperationException != null)
            //{
            //    throw state.OperationException;
            //}

        }

        public void AsynchronousUpload1(string destinationPath, string sourcePath, string userName, string password)
        {
            ManualResetEvent waitObject;
            Uri target = new Uri(destinationPath);
            string fileName = sourcePath;
            FtpState state = new FtpState();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.KeepAlive = false;
            request.Proxy = null;

            if (!(string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password)))
            {
                request.Credentials = new NetworkCredential(userName, password);
            }

            state.Request = request;
            state.FileName = fileName;
            waitObject = state.OperationComplete;

            request.BeginGetRequestStream(
                new AsyncCallback(EndGetStreamCallback),
                state
            );

            waitObject.WaitOne();

            //if (state.OperationException != null)
            //{
            //    throw state.OperationException;
            //}

        }

        private static void EndGetStreamCallback(IAsyncResult ar)
        {
            FtpState state = (FtpState)ar.AsyncState;

            Stream requestStream = null;

            try
            {
                requestStream = state.Request.EndGetRequestStream(ar);
                const int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int count = 0;
                int readBytes = 0;
                Stream stream = new MemoryStream(state.Filesize);
                do
                {
                    readBytes = stream.Read(buffer, 0, bufferLength);
                    requestStream.Write(buffer, 0, readBytes);
                    count += readBytes;
                }
                while (readBytes != 0);
                stream.Close();
                requestStream.Close();
                state.Request.BeginGetResponse(
                    new AsyncCallback(EndGetResponseCallback),
                    state
                );
                             
                
            }
            catch (Exception e)
            {
                state.OperationException = e;
                state.OperationComplete.Set();
                return;
            }
            finally
            {
                requestStream.Close();
                requestStream.Dispose();
            }            
        }

        private static void EndGetResponseCallback(IAsyncResult ar)
        {
            FtpState state = (FtpState)ar.AsyncState;
            FtpWebResponse response = null;
            try
            {
                response = (FtpWebResponse)state.Request.EndGetResponse(ar);
                response.Close();
                state.StatusDescription = response.StatusDescription;
                state.OperationComplete.Set();
            }
            catch (Exception e)
            {
                state.OperationException = e;
                state.OperationComplete.Set();
            }
        }
        public void Download(string _ftpSourcePath, string _ftpUserID, string _ftpPassword, string _fileDesPath, string _fileName)
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(_fileDesPath + "\\" + _fileName, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_ftpSourcePath + "/" + _fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(_ftpUserID, _ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool ftpFileExist(string fileName, string userName, string password)
        {
            WebClient wc = new WebClient();
            try
            {
                wc.Credentials = new NetworkCredential(userName, password);
                byte[] fData = wc.DownloadData(fileName);
                if (fData.Length > -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
        #region Multidownload

        private string[] GetFileList(string ftpSourcePath, string ftpUserID, string ftpPassword)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpSourcePath + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                downloadFiles = null;
                return downloadFiles;
            }
        }
        public void MultiDownload(string _file, string _ftpSourcePath, string _ftpUserID, string _ftpPassword, string _localDestnDir)
        {
            string[] _strFiles = GetFileList(_ftpSourcePath, _ftpUserID, _ftpPassword);
            foreach (string file in _strFiles)
            {
                try
                {
                    string uri = "ftp://" + _ftpSourcePath + "/" + file;
                    Uri serverUri = new Uri(uri);
                    if (serverUri.Scheme != Uri.UriSchemeFtp)
                    {
                        return;
                    }
                    FtpWebRequest reqFTP;
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + _ftpSourcePath + "/" + file));
                    reqFTP.Credentials = new NetworkCredential(_ftpUserID, _ftpPassword);
                    reqFTP.KeepAlive = false;
                    reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                    reqFTP.UseBinary = true;
                    reqFTP.Proxy = null;
                    reqFTP.UsePassive = false;
                    FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    FileStream writeStream = new FileStream(_localDestnDir + @"\" + file, FileMode.Create);
                    int Length = 2048;
                    Byte[] buffer = new Byte[Length];
                    int bytesRead = responseStream.Read(buffer, 0, Length);
                    while (bytesRead > 0)
                    {
                        writeStream.Write(buffer, 0, bytesRead);
                        bytesRead = responseStream.Read(buffer, 0, Length);
                    }
                    writeStream.Close();
                    response.Close();
                }
                catch (WebException wEx)
                {
                    throw wEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
        #region DELETE
        public void DeleteFile(string host,string user,string pass,string deleteFile)
        {
            try
            {
                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return;
        }
        public bool CheckFileExit(string host, string user, string pass, string deleteFile)
        {
            bool _exit = true;
            try
            {
                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + deleteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;
                _exit = false;
            }
            catch (Exception ex) { _exit = false; }
            return _exit;
        }
        #endregion
    }
}
