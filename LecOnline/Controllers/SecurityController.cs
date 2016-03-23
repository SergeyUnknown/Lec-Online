// -----------------------------------------------------------------------
// <copyright file="SecurityController.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Controllers
{
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using LecOnline.Core;
    using LecOnline.Models.Security;
    using Microsoft.AspNet.Identity.Owin;

    /// <summary>
    /// Controller for managing security related stuff.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    public class SecurityController : Controller
    {
        /// <summary>
        /// Initializes static members of the <see cref="SecurityController"/> class.
        /// </summary>
        static SecurityController()
        {
            Mapper.CreateMap<ChangesLog, ChangesLogViewModel>();
            Mapper.CreateMap<ErrorLog, ErrorLogViewModel>();
        }

        /// <summary>
        /// Display list of log entries.
        /// </summary>
        /// <param name="filter">Filter which should be applied to the log entries.</param>
        /// <returns>Return action result.</returns>
        public ActionResult Errors([Bind(Prefix = "Filter")]ErrorsListFilter filter)
        {
            var context = this.HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var model = new ErrorsListViewModel(dbContext.ErrorLogs, filter);
            return this.View(model);
        }

        /// <summary>
        /// Display list of audit log entries.
        /// </summary>
        /// <param name="filter">Filter which should be applied to the log entries.</param>
        /// <returns>Return action result.</returns>
        public ActionResult AuditLog([Bind(Prefix = "Filter")]ChangesListFilter filter)
        {
            var context = this.HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var model = new ChangesListViewModel(dbContext.ChangesLogs, filter);
            return this.View(model);
        }

        /// <summary>
        /// Display audit log entries.
        /// </summary>
        /// <param name="id">Id of the entry to look.</param>
        /// <returns>Task which asynchronously return action result.</returns>
        public async Task<ActionResult> ChangeDetail(int id)
        {
            var context = this.HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var logEntry = await dbContext.ChangesLogs.FindAsync(id);
            var model = new ChangesLogViewModel();
            Mapper.Map(logEntry, model);
            return this.View(model);
        }

        /// <summary>
        /// Display error log entries.
        /// </summary>
        /// <param name="id">Id of the entry to look.</param>
        /// <returns>Task which asynchronously return action result.</returns>
        public async Task<ActionResult> ErrorDetail(int id)
        {
            var context = this.HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var logEntry = await dbContext.ErrorLogs.FindAsync(id);
            var model = new ErrorLogViewModel();
            Mapper.Map(logEntry, model);
            return this.View(model);
        }
    }
}