using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WarehouseManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Blog",
               url: "{lang}/b",
               defaults: new { lang = UrlParameter.Optional, controller = "Blog", action = "Index", link = UrlParameter.Optional }
           ); routes.MapRoute(
               name: "Contact",
               url: "{lang}/cn",
               defaults: new { lang = UrlParameter.Optional, controller = "Contact", action = "Index", link = UrlParameter.Optional }
           );


            routes.MapRoute(
                name: "BlogDetail",
                url: "{lang}/bd/{link}",
                defaults: new { lang = UrlParameter.Optional, controller = "Blog", action = "BlogDetail", link = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Service",
                url: "{lang}/c/{link}",
                defaults: new { lang = UrlParameter.Optional, controller = "Service", action = "Index", link = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ServiceDetail",
                url: "{lang}/p/{link}",
                defaults: new { lang = UrlParameter.Optional, controller = "Service", action = "ServiceDetail", link = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "sss",
                url: "{lang}/sss",
                defaults: new { lang = UrlParameter.Optional, controller = "Faq", action = "Index", link = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { lang = "tr", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}