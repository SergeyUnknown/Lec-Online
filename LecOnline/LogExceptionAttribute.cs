﻿// -----------------------------------------------------------------------
// <copyright file="LogExceptionAttribute.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline
{
    using System.Web.Mvc;
    using LecOnline.Core;

    /// <summary>
    /// Represents an attribute that is used to log an exception that is thrown
    /// by an action method.
    /// </summary>
    public class LogExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                var routeData = filterContext.RequestContext.RouteData;
                string controller = routeData.Values["controller"].ToString();
                string action = routeData.Values["action"].ToString();
                var userName = filterContext.HttpContext.User.Identity.Name;
                ExceptionHelper.PublishException(userName, filterContext.Exception);
            }
        }
    }
}
