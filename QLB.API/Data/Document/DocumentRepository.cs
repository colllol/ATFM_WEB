using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info;
using QLB.BusinessLogic;


namespace QLB.API.Data
{
    public class DocumentRepository
    {
        public ReponseEntity GetPageDocument(int page_size, int page_index, string where)
        {            
            return new ReponseEntityHelper().GetPage<Document>(new DocumentDAL().GetPageDocument(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageDocumentExport(string where)
        {
           
            return new ReponseEntityHelper().GetPageExport<Document>(new DocumentDAL().GetPageDocumentExport(where).Tables[0]);
        }

        public ReponseEntity GetAllDocument()
        {
            return new ReponseEntityHelper().GetAll<Document>(new DocumentDAL().GetAllDocument().Tables[0]);
        }

        public ReponseEntity GetByIdDocument(int Id)
        {
             return new ReponseEntityHelper().GetByID<Document>(new DocumentDAL().GetByIdDocument(Id).Tables[0]);
        }

        public ReponseEntity CreateDocument(Document oDocument)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                DocumentDAL objDAL = new DocumentDAL();
                Int64 Id = objDAL.CreateDocument(oDocument);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "DocumentRepository.CreateDocument:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "DocumentRepository.CreateDocument: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
            
        }

        public ReponseEntity UpdateDocument(Document oDocument)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                DocumentDAL objDAL = new DocumentDAL();
                Int64 Id = objDAL.UpdateDocument(oDocument);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "DocumentRepository.UpdateDocument:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "DocumentRepository.UpdateDocument: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteDocument(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                DocumentDAL objDAL = new DocumentDAL();
                objDAL.DeleteDocument(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "DocumentRepository.DeleteDocument: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}