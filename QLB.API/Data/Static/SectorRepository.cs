using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLB.API.Models;
using System.Data.Common;
using QLB.API.Common;
using System.Data;
using QLB.Info;
//using Oracle.ManagedDataAccess;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class SectorRepository
    {
        public ReponseEntity GetPageSector(int page_size, int page_index, string where)
        {
           
            return new ReponseEntityHelper().GetPage<Sectors>(new SectorsDAL().GetPageSectors(page_size, page_index, where).Tables[0]);
        }

        public ReponseEntity GetPageSectorExport(string where)
        {
           
            return new ReponseEntityHelper().GetPageExport<Sectors>(new SectorsDAL().GetPageSectorsExport(where).Tables[0]);
        }

        public ReponseEntity GetAllSectors()
        {
            return new ReponseEntityHelper().GetAll<Sectors>(new SectorsDAL().GetAllSectors().Tables[0]);
        }
       
        public ReponseEntity GetByIdSector(int Id)
        {            
            return new ReponseEntityHelper().GetByID<Sectors>(new SectorsDAL().GetByIdSectors(Id).Tables[0]);
        }

        public ReponseEntity CreateSector(Sectors oAERO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;

            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                SectorsDAL objDAL = new SectorsDAL();
                Int64 Id = objDAL.CreateSectors(oAERO);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "SectorRepository.CreateSector:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "SectorRepository.CreateSector: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
            
        }

        public ReponseEntity UpdateSector(Sectors oAERO)
        {
            string sReturn = string.Empty;
            string sOutput = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                SectorsDAL objDAL = new SectorsDAL();
                Int64 Id = objDAL.UpdateSectors(oAERO);
                oResponse.Code = Id.ToString();
                if (Id == -1)
                {
                    oResponse.Code = "-1";
                    oResponse.Message = "Dữ liệu đã tồn tại";
                    oResponse.Value = -1;
                }
                else if (Id == -99)
                {
                    LogAPI.LogToFile(LogFileType.EXCEPTION, "SectorRepository.UpdateSector:");
                    oResponse.Code = "-99";
                    oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
                }
                else
                { oResponse.Message = "Cập nhật dữ liệu thành công"; }

            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "SectorRepository.UpdateSector: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }

        public ReponseEntity DeleteSector(int Id)
        {
            string sReturn = string.Empty;
            string sOutPut = string.Empty;
            ReponseEntity oResponse = new ReponseEntity();

            try
            {
                SectorsDAL objDAL = new SectorsDAL();
                objDAL.DeleteSectors(Id);
                oResponse.Code = Id.ToString();
                oResponse.Message = "Cập nhật dữ liệu thành công";
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "SectorRepository.DeleteSector: " + ex.Message);
                oResponse.Code = "-99";
                oResponse.Message = "Có lỗi trong quá trình lấy dữ liệu";
            }
            return oResponse;
        }
    }
}