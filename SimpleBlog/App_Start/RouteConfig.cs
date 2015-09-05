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


            var namespaces = new[] {typeof(PostsController).Namespace};

            // Posts
            // Mvc does not let us seprate our url with dash.. must use slash so workaround below. I.e: http://blog.dev/post/423-this-is-a-slug
            routes.MapRoute("PostWorkAroundDashURL", "post/{idAndSlug}", new { controller = "Posts", action = "Show" }, namespaces);
            routes.MapRoute("Post", "post/{id}-{slug}", new { controller = "Posts", action = "Show" }, namespaces);

            // Tags
            // Mvc does not let us seprate our url with dash.. must use slash so workaround below. I.e: http://blog.dev/post/423-this-is-a-slug
            routes.MapRoute("TagWorkAroundDashURL", "tag/{idAndSlug}", new { controller = "Posts", action = "Tag" }, namespaces);
            routes.MapRoute("Tag", "tag/{id}-{slug}", new { controller = "Posts", action = "Tag" }, namespaces);

            routes.MapRoute(
                "Login",
                "login",
                new { controller = "Auth", action = "Login" },
                namespaces
                );

            routes.MapRoute(
                "Logout",
                "logout",
                new { controller = "Auth", action = "Logout" },
                namespaces
                );

            // url empty so act as index/default
            routes.MapRoute(
                "Home", 
                "", 
                new { controller = "Posts", action = "Index" },
                namespaces
                );

            routes.MapRoute("Sidebar", "", new { controller = "Layout", action = "Sidebar" }, namespaces);


        }
    }
}