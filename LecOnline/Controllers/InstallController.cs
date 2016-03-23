// -----------------------------------------------------------------------
// <copyright file="InstallController.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Controller for the installation process.
    /// </summary>
    public class InstallController : Controller
    {
        /// <summary>
        /// Starts the installation process.
        /// </summary>
        /// <returns>Result of action execution.</returns>
        public ActionResult Index()
        {
            return this.View();
        }
    }
}