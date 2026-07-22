using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace QLB.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services 
            //config.Filters.Add(new AuthorizeAttribute());
            // Web API routes

            //var cors = new EnableCorsAttribute("http://192.168.62.20:2121", "*", "*");
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            


            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "MethodTwo",
                routeTemplate: "api/{controller}/{action}/{page_size}/{page_index}",
                defaults: new { page_size = RouteParameter.Optional, page_index = RouteParameter.Optional }
            );
            
        }      
    }
}
