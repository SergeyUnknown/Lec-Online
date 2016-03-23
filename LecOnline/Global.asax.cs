// -----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline
{
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    /// <summary>
    /// Application class.
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Called when application initialized.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var dateTimeBinder = new LecOnline.Mvc.DateTimeModelBinder("d", "G");
            ModelBinders.Binders[typeof(System.DateTime)] = dateTimeBinder;
            ModelBinders.Binders[typeof(System.DateTime?)] = dateTimeBinder;
        }
    }
}
