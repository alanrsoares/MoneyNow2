using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MoneyNow2.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}
