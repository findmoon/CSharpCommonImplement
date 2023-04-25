using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ASPNETWebMVCBasic
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Users",
                url: "Users/{id}",
                defaults: new { controller = "Users", action = "GetAll", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            #region 实现html扩展名伪静态 https://www.cnblogs.com/djd66/p/15238368.html
            //<system.webServer>
            //      <validation validateIntegratedModeConfiguration="false"/>
            //      <modules runAllManagedModulesForAllRequests="true" />
            //</system.webServer>
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}.html",
            //    defaults: new { controller = "Home", action = "Info", id = UrlParameter.Optional }
            //); 
            #endregion
        }
    }
}
