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
                name: "BlogbyCategorybyPage",
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
                url: "Project/Create",
                defaults: new { controller = "Project", action = "Create" }
            );

            routes.MapRoute(
                name: "ProjectbyCategorybyPage",
                url: "Project/{category}/Page{page}",
                defaults: new { controller = "Project", action = "Index" }
            );

            routes.MapRoute(
                name: "ProjectsbyPage",
                url: "Project/Page{page}",
                defaults: new { controller = "Project", action = "Index" }
            );

            routes.MapRoute(
                name: "ProjectsbyCategory",
                url: "Project/{category}",
                defaults: new { controller = "Project", action = "Index" }
            );

            routes.MapRoute(
                name: "ProjectIndex",
                url: "Project",
                defaults: new { controller = "Project", action = "Index" }
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
