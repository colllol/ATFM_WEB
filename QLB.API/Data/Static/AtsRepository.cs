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
    public class AtsRepository
    {
        public ReponseEntity GetPageAtsRoute(int page_size, int page_index, string where)
        {            
            return new ReponseEntityHelper().GetPage<ATSROUTE>(new AtsDAL().GetPageAtsRoute(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity DeleteAtsRoute(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AtsDAL objDAL = new AtsDAL();
                objDAL.DeleteAtsRoute(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AtsRepository.DeleteAtsRoute: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity GetPageAtsFir(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<AtsFIR>(new AtsDAL().GetPageAtsFir(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity DeleteAtsFir(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AtsDAL objDAL = new AtsDAL();
                objDAL.DeleteAtsFir(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AtsRepository.DeleteAtsFir: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity GetPageAtsWay(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<ATSWAYPOINT>(new AtsDAL().GetPageAtsWay(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity DeleteAtsWay(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AtsDAL objDAL = new AtsDAL();
                objDAL.DeleteAtsWay(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AtsRepository.DeleteAtsWay: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity GetPageAtsAreo(int page_size, int page_index, string where)
        {
            return new ReponseEntityHelper().GetPage<AREODROME>(new AtsDAL().GetPageAtsAreo(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity DeleteAtsAreo(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                AtsDAL objDAL = new AtsDAL();
                objDAL.DeleteAtsAreo(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "AtsRepository.DeleteAtsAreo: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}