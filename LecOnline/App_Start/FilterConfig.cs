// -----------------------------------------------------------------------
// <copyright file="FilterConfig.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Class for configuring filters.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register global filters.
        /// </summary>
        /// <param name="filters">Filters configuration.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogExceptionAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
