using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using RouteParameter = System.Web.Http.RouteParameter;

namespace WarehouseManagementSystem.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // Configure authentication filter
            config.Filters.Add(new HostAuthenticationFilter(new OAuthBearerAuthenticationOptions().AuthenticationType));

            // Configure Web API routes
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}