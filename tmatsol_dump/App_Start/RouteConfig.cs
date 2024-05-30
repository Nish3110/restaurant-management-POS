using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace tmatsol_dump
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "MenuDefault",
              url: "Menu/{controller}/{action}/{id}",
              defaults: new { controller = "RTC", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "RestDefault",
                url: "Rest/{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "BODefault",
               url: "BO/{controller}/{action}/{id}",
               defaults: new { controller = "BO_Login", action = "Login", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Download", id = UrlParameter.Optional }
            );



            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}/{u_id}",
            //    defaults: new { controller = "ABC", action = "Index", id = UrlParameter.Optional , u_id = UrlParameter.Optional }
            //);

            // routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "REPORT", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
