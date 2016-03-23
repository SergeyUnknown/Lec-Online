// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Controllers
{
    using System.Web.Mvc;
    using LecOnline.Core;

    /// <summary>
    /// Home page controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Main page.
        /// </summary>
        /// <returns>Results of the action.</returns>
        public ActionResult Index()
        {
            if (this.User != null && this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Dashboard");
            }

            return this.View();
        }

        /// <summary>
        /// Shows about page.
        /// </summary>
        /// <returns>Results of the action.</returns>
        public ActionResult About()
        {
            return this.View();
        }

        /// <summary>
        /// Shows contact page.
        /// </summary>
        /// <returns>Results of the action.</returns>
        public ActionResult Contact()
        {
            return this.View();
        }

        /// <summary>
        /// Shows dashboard page.
        /// </summary>
        /// <returns>Results of the action.</returns>
        [Authorize]
        public ActionResult Dashboard()
        {
            if (this.User.IsInRole(RoleNames.Administrator))
            {
                return this.RedirectToAction("Index", "User");
            }

            if (this.User.IsInRole(RoleNames.Manager))
            {
                return this.RedirectToAction("Index", "Request");
            }

            if (this.User.IsInRole(RoleNames.MedicalCenter))
            {
                return this.RedirectToAction("Index", "Request");
            }

            if (this.User.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                return this.RedirectToAction("Index", "Request");
            }

            return this.View();
        }
    }
}