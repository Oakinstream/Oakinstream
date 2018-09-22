using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Oakinstream
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "BlogpostCreate",
                url: "Blogpost/Create",
                defaults: new { controller = "Blog", action = "Create" }
            );

            routes.MapRoute(
                name: "BlogpostbyCategorybyPage",
                url: "Blogpost/{category}/Page{page}",
                defaults: new { controller = "Blog", action = "Index" }
            );

            routes.MapRoute(
                name: "ProductsbyPage",
                url: "Blogpost/Page{page}",
                defaults: new { controller = "Blog", action = "Index" }
            );

            routes.MapRoute(
                name: "BlogpostsbyCategory",
                url: "Blogpost/{category}",
                defaults: new { controller = "Blog", action = "Index" }
            );

            routes.MapRoute(
                name: "BlogpostIndex",
                url: "Blogpost",
                defaults: new { controller = "Blog", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
