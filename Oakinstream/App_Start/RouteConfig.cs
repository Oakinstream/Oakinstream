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

            #region Blog
            routes.MapRoute(
                name: "BlogCreate",
                url: "Blog/Create",
                defaults: new { controller = "Blog", action = "Create" }
            );

            routes.MapRoute(
                name: "BlogsbyCategorybyPage",
                url: "Blog/{category}/Page{page}",
                defaults: new { controller = "Blog", action = "Index" }
            );

            routes.MapRoute(
                name: "BlogsbyPage",
                url: "Blog/Page{page}",
                defaults: new { controller = "Blog", action = "Index" }
            );

            routes.MapRoute(
                name: "BlogsbyCategory",
                url: "Blog/{category}",
                defaults: new { controller = "Blog", action = "Index" }
            );

            routes.MapRoute(
                name: "BlogsIndex",
                url: "Blog",
                defaults: new { controller = "Blog", action = "Index" }
            );
            #endregion

            #region Project
            routes.MapRoute(
                name: "ProjectCreate",
                url: "Projects/Create",
                defaults: new { controller = "Projects", action = "Create" }
            );

            routes.MapRoute(
                name: "ProjectsbyCategorybyPage",
                url: "Projects/{category}/Page{page}",
                defaults: new { controller = "Projects", action = "Index" }
            );

            routes.MapRoute(
                name: "ProjectsbyPage",
                url: "Projects/Page{page}",
                defaults: new { controller = "Projects", action = "Index" }
            );

            routes.MapRoute(
                name: "ProjectsbyCategory",
                url: "Projects/{category}",
                defaults: new { controller = "Projects", action = "Index" }
            );

            routes.MapRoute(
                name: "ProjectIndex",
                url: "Projects",
                defaults: new { controller = "Projects", action = "Index" }
            );
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
