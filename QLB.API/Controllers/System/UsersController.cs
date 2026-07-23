using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;
using QLB.API.Filters;

namespace QLB.API.Controllers
{
    public class UsersController : ApiController
    {
        UsersRepository UsersRepository = null;
        /// <summary>
        /// /thai
        /// </summary>
        public UsersController()
        {
            if (UsersRepository == null)
            {
                UsersRepository = new UsersRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageUsers(int page_size, int page_index)
        {
            return UsersRepository.GetPageUsers(page_size, page_index);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageUsers_New(int page_size, int page_index,string where)
        {
            return UsersRepository.GetPageUsers_New(page_size, page_index, where);
        }


        [AcceptVerbs("Get")]
        public ReponseEntity GetAllUsers()
        {
            return UsersRepository.GetAllUsers();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdUsers(int UserId)
        {
            return UsersRepository.GetByIdUsers(UserId);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateUsers(Users oUSERS)
        {
            //var axxxx = JsonConvert.DeserializeObject<Users>(Request.Content.ReadAsStringAsync().Result);
            //var json = Request.Content.ReadAsStreamAsync().Result;
            //Stream stream = new MemoryStream();
            //json.Seek(0, SeekOrigin.Begin);
            //StreamReader rd = new StreamReader(json);
            //var ax = rd.ReadToEnd();
            // oUSERS = JsonConvert.DeserializeObject<Users>(ax, new JsonSerializerSettings() { DateFormatString = "yyyy-MM-ddThh:mm:ss" });

            return UsersRepository.CreateUsers(oUSERS);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateUsers(Users oUSERS)
        {
            return UsersRepository.UpdateUsers(oUSERS);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteUsers(int UserId)
        {
            return UsersRepository.DeleteUsers(UserId);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetUserByUserPass(string Username, string Password)
        {
            return UsersRepository.GetUserByUserPass(Username, Password);

        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity BindGridMenuByUser(int Parrent_ID, int UserId)
        {
            return UsersRepository.BindGridMenuByUser(Parrent_ID, UserId);

        }

        [AcceptVerbs("Get")]
        public ReponseReportEntity isParrentMenu(int MenuId)
        {
            return UsersRepository.isParrentMenu(MenuId);

        }


        [AcceptVerbs("Get")]
        public ReponseReportEntity GetMenu4User(int UserID)
        {
            return UsersRepository.GetMenu4User(UserID);

        }

        [AcceptVerbs("Get")]
        [ApiKeyAuthorize]
        public ReponseReportEntity GetAllMenu4User(int UserID)
        {
            if (UserID <= 0)
                throw new HttpResponseException(global::System.Net.HttpStatusCode.BadRequest);

            return UsersRepository.GetAllMenu4User(UserID);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetUserByUserName(string Username)
        {
            return UsersRepository.GetUserByUserName(Username);

        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetUSERNAME(string Username, int UserID)
        {
            return UsersRepository.GetUSERNAME(Username, UserID);

        }
        [AcceptVerbs("Get")]
        public ReponseEntity GetRole4UserMenu(int UserID, int MenuID)
        {
            return UsersRepository.GetRole4UserMenu(UserID, MenuID);

        }
    }
}
