// -----------------------------------------------------------------------
// <copyright file="CommitteeController.cs" company="MDP-Soft">
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
    using LecOnline.Models.Committee;
    using Microsoft.AspNet.Identity.Owin;

    /// <summary>
    /// Controller for managing clients.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    public class CommitteeController : Controller
    {
        /// <summary>
        /// Initializes static members of the <see cref="CommitteeController"/> class.
        /// </summary>
        static CommitteeController()
        {
            Mapper.CreateMap<Committee, EditCommitteeViewModel>();
            Mapper.CreateMap<EditCommitteeViewModel, Committee>();
        }

        /// <summary>
        /// Display list of committees.
        /// </summary>
        /// <param name="filter">Filter which should be applied to the committees.</param>
        /// <returns>Return action result.</returns>
        public ActionResult Index([Bind(Prefix = "Filter")]CommitteesListFilter filter)
        {
            var context = this.HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var model = new CommitteesListViewModel(dbContext.Committees, filter);
            return this.View(model);
        }

        /// <summary>
        /// Show create committee page.
        /// </summary>
        /// <returns>Result of the action.</returns>
        public ActionResult Create()
        {
            var model = new EditCommitteeViewModel();
            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the committee.
        /// </summary>
        /// <param name="model">Data to save about committee.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditCommitteeViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var committee = new Committee();
            Mapper.Map(model, committee);
            dbContext.Committees.Add(committee);
            await dbContext.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show edit committee page by id.
        /// </summary>
        /// <param name="id">Id of the committee to get</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Edit(int id)
        {
            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var committee = await dbContext.Committees.FindAsync(id);
            if (committee == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new EditCommitteeViewModel();
            Mapper.Map(committee, model);
            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the committee.
        /// </summary>
        /// <param name="model">Data to save about committee.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditCommitteeViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var committee = await dbContext.Committees.FindAsync(model.Id);
            if (committee == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            Mapper.Map(model, committee);
            await dbContext.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show delete prompt for committee by it's id.
        /// </summary>
        /// <param name="id">Id of the committee to delete</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Delete(int id)
        {
            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var committee = await dbContext.Committees.FindAsync(id);
            if (committee == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new EditCommitteeViewModel();
            Mapper.Map(committee, model);
            return this.View(model);
        }

        /// <summary>
        /// Show delete prompt for committee by it's id.
        /// </summary>
        /// <param name="model">New data about the committee to delete.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(EditCommitteeViewModel model)
        {
            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var committee = await dbContext.Committees.FindAsync(model.Id);
            if (committee == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            dbContext.Committees.Remove(committee);
            await dbContext.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}