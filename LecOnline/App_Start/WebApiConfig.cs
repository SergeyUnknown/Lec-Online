// -----------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline
{
    using System.Web.Http;

    /// <summary>
    /// Configure WebAPI within application.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Setup WebAPI specific configuration and services.
        /// </summary>
        /// <param name="config">Configuration object which should be configured.</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}
