using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace csFiddle
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{version}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, version = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Edit",
                url: "Home/EditVersion/{id}/{version}",
                defaults: new { id = UrlParameter.Optional, version = UrlParameter.Optional }
            );
        }
    }
}