using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using Newtonsoft.Json;
using System.IO;

namespace QLB.API.Controllers
{
    public class PermDetailScController : ApiController
    {
        PermDetailScRepository PermDetailScRepository = null;

        public PermDetailScController()
        {
            if (PermDetailScRepository == null)
            {
                PermDetailScRepository = new PermDetailScRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermDetailSc(int page_size, int page_index, string where)
        {
            return PermDetailScRepository.GetPagePermDetailSc(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPagePermDetailScExport(string where)
        {
            return PermDetailScRepository.GetPagePermDetailScExport(where);
        }

        [AcceptVerbs("Get")]


        public ReponseEntity GetAllPermDetailSc()
        {
            return PermDetailScRepository.GetAllPermDetailSc();
        }


        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdPermDetailSc(int ID)
        {

            return PermDetailScRepository.GetByIdPermDetailSc(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreatePermDetailSc(PermDetailSc oPERMDETAILSC)
        {
            return PermDetailScRepository.CreatePermDetailSc(oPERMDETAILSC);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdatePermDetailSc(PermDetailSc oPERMDETAILSC)
        {
            //var axxxx = JsonConvert.DeserializeObject<PermDetailSc_Search>(Request.Content.ReadAsStringAsync().Result);
            //var json = Request.Content.ReadAsStreamAsync().Result;
            //Stream stream = new MemoryStream();
            //json.Seek(0, SeekOrigin.Begin);
            //StreamReader rd = new StreamReader(json);
            //var ax = rd.ReadToEnd();
            return PermDetailScRepository.UpdatePermDetailSc(oPERMDETAILSC);
        }
        [AcceptVerbs("Put")]
        public ReponseEntity UpdatePermDetailSc_GenBack(PermDetailSc oPERMDETAILSC)
        {
            return PermDetailScRepository.UpdatePermDetailSc_GenBack(oPERMDETAILSC);
        }


        [AcceptVerbs("Delete")]
        public ReponseEntity DeletePermDetailSc(int ID)
        {
            return PermDetailScRepository.DeletePermDetailSc(ID);
        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetRecordDeleted(string where)
        {
            return new PermDetailScRepository().GetDeleted(where);
        }
        [AcceptVerbs("Put", "Post")]
        public ReponseEntity GetBySearch(PermDetailSc_Search obj)
        {
            //var axxxx = JsonConvert.DeserializeObject<PermDetailSc_Search>(Request.Content.ReadAsStringAsync().Result);
            //var json = Request.Content.ReadAsStreamAsync().Result;
            //Stream stream = new MemoryStream();
            //json.Seek(0, SeekOrigin.Begin);
            //StreamReader rd = new StreamReader(json);
            //var ax = rd.ReadToEnd();
            
            return PermDetailScRepository.GetBySearch(obj);
        }
    }
}
