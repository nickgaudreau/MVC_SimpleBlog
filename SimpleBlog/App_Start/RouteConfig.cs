using SimpleBlog.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // for debugging purposes
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // dont use the default for more control, betteer for seo, web api and more
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            // use this since 2 controller name Posts
            var namespaces = new[] {typeof(PostsController).Namespace};
            
            // url empty so act as index/default
            routes.MapRoute(
                "Home", 
                "", 
                new { controller = "Posts", action = "Index" },
                namespaces
                );

            routes.MapRoute(
                "Login",
                "login",
                new { controller = "Auth", action = "Login" },
                namespaces
                );


        }
    }
}