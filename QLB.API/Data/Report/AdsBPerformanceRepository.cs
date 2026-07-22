using System;
using QLB.API.Models;
using QLB.BusinessLogic;
using QLB.Info;

namespace QLB.API.Data
{
    public class AdsBPerformanceRepository
    {
        public ReponseReportEntity GetData(AdsBPerformanceRequest request)
        {
            return Execute(() => new AdsBPerformanceDAL().GetData(request));
        }

        public ReponseReportEntity GetOperators(AdsBPerformanceRequest request)
        {
            return Execute(() => new AdsBPerformanceDAL().GetOperators(request));
        }

        private static ReponseReportEntity Execute(Func<object> action)
        {
            try
            {
                object value = action();
                return new ReponseReportEntity
                {
                    Code = "00",
                    Message = "Lấy dữ liệu thành công",
                    ListValue = value
                };
            }
            catch (ArgumentException ex)
            {
                return new ReponseReportEntity { Code = "-1", Message = ex.Message };
            }
            catch (Exception)
            {
                return new ReponseReportEntity { Code = "-99", Message = "Có lỗi trong quá trình lấy dữ liệu" };
            }
        }
    }
}
