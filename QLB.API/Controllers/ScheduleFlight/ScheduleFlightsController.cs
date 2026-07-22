using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using System;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Web.Http.Cors;
using QLB.BusinessLogic;
using QLB.API.Common;
namespace QLB.API.Controllers.ScheduleFlight
{
    public class ScheduleFlightsController : ApiController
    {
        [AcceptVerbs("Post")]
        public ReponseEntity Insert(ScheduleDayFlight obj)
        {
            //var ax =  new ScheduleDayFlightDAL().Insert(obj);
            return new ReponseEntityHelper().Insert<ScheduleDayFlight, ScheduleDayFlightDAL>(obj, "Insert");
        }
        [AcceptVerbs("Put")]
        public ReponseEntity Update(ScheduleDayFlight obj)
        {
            var ax = new ScheduleDayFlightDAL().Update(obj);
            return new ReponseEntityHelper().Update<ScheduleDayFlight, ScheduleDayFlightDAL>(obj, "Update");
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity Delete(Int64 Id)
        {
            return new ReponseEntityHelper().DeleteReturnParamOut<ScheduleDayFlight, ScheduleDayFlightDAL>(Id, "Delete");
        }
        [AcceptVerbs("Put")]
        public ReponseEntity GetBySearch(clsDayFlightValueSearch objSearch)
        {
            return new ReponseEntityHelper().GetAll<ScheduleDayFlight>(new ScheduleDayFlightDAL().GetBySearch(objSearch));
        }


        [AcceptVerbs("Get")]
        public ReponseEntity GetById(Int64 id)
        {
            return new ReponseEntityHelper().GetByID<ScheduleDayFlight>(new ScheduleDayFlightDAL().GetById(id));
        }

        [AcceptVerbs("Put", "Post")]
        public ReponseEntity GetBySearchAll(clsDayFlightValueSearch objSearch)
        {
            ReponseEntity objRes = new ReponseEntity();
            try
            {

                objRes= new ReponseEntityHelper().GetAll<ScheduleDayFlight>(new ScheduleDayFlightDAL().GetBySearchAll(objSearch));
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, objSearch.StartDate + ex.ToString());
            }
            return objRes;
            //return new ReponseEntityHelper().GetAll<ScheduleDayFlight>(new ScheduleDayFlightDAL().GetBySearchAll(objSearch));
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseEntity GetBySearchAll_New(clsDayFlightValueSearch objSearch)
        {
            ReponseEntity objRes = new ReponseEntity();
            try
            {

                objRes = new ReponseEntityHelper().GetAll<ScheduleDayFlight>(new ScheduleDayFlightDAL().GetBySearchAll_New(objSearch));
            }

            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, objSearch.StartDate + ex.ToString());
            }
            return objRes;
           
        }
    }
}
