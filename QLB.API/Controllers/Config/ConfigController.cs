using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Jwt.Filters;

namespace QLB.API.Controllers
{
    public class ConfigController : ApiController
    {
        ConfigRepository ConfigRepository = null;

        public ConfigController()
        {
            if (ConfigRepository == null)
            {
                ConfigRepository = new ConfigRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageConfig(int page_size, int page_index, string where)
        {
            return ConfigRepository.GetPageConfig(page_size, page_index, where);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetPageConfigExport(string where)
        {
            return ConfigRepository.GetPageConfigExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllConfig()
        {
            return ConfigRepository.GetAllConfig();
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdConfig(int ID)
        {

            return ConfigRepository.GetByIdConfig(ID);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateConfig(Config oConfig)
        {
            return ConfigRepository.CreateConfig(oConfig);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateConfig(Config oConfig)
        {
            return ConfigRepository.UpdateConfig(oConfig);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteConfig(int ID)
        {
            return ConfigRepository.DeleteConfig(ID);
        }

    }
}