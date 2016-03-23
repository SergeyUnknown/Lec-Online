// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

[assembly: Microsoft.Owin.OwinStartupAttribute(typeof(LecOnline.Startup))]

namespace LecOnline
{
    using Owin;

    /// <summary>
    /// OWIN startup application.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configuration of the application.
        /// </summary>
        /// <param name="app">Application builder to use for configuration.</param>
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            app.MapSignalR(new Microsoft.AspNet.SignalR.HubConfiguration()
            {
                EnableDetailedErrors = true
            });
            System.Web.Mvc.ModelMetadataProviders.Current = new LecOnline.Mvc.LecModelMetadataProvider();
        }
    }
}
