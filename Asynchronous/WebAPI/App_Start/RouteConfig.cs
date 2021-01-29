using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebAPI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute(
                routeName: "DefaultToHTML",
                routeUrl: "",
                physicalFile: "~/_RunRequest.aspx",
                checkPhysicalUrlAccess: false,
                defaults: new RouteValueDictionary(),
                constraints: new RouteValueDictionary { { "placeholder", "" } }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
