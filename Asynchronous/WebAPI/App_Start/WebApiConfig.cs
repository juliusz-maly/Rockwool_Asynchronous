using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("http://localhost", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //ThreadPool.GetMaxThreads(out int maxThreads, out int maxIoThreads);
            //System.Diagnostics.Debug.WriteLine($"MaxThreads: {maxThreads}, max io threads: {maxIoThreads}");
            //ThreadPool.SetMinThreads(2, 2);
            //ThreadPool.SetMaxThreads(2, 2);
            //ThreadPool.GetMaxThreads(out maxThreads, out maxIoThreads);
            //System.Diagnostics.Debug.WriteLine($"MaxThreads: {maxThreads}, max io threads: {maxIoThreads}");
        }
    }
}
