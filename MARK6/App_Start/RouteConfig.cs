using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MARK6
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Custom route for MediaGallery
            routes.MapRoute(
                name: "MediaGallery",
                url: "{controller}/{action}/{mediaType}",
                defaults: new { controller = "MediaGallery", action = "Index" }
            );

            //// Custom route for MediaGallery with ID
            //routes.MapRoute(
            //    name: "MediaGalleryWithID",
            //    url: "{controller}/{action}/{mediaType}/{id}",
            //    defaults: new { controller = "MediaGallery", action = "Index",mediaType=UrlParameter.Optional, id=UrlParameter.Optional }
            //);

            // Default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}