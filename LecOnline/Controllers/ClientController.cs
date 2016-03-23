// -----------------------------------------------------------------------
// <copyright file="ClientController.cs" company="MDP-Soft">
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
    using LecOnline.Models.Client;
    using Microsoft.AspNet.Identity.Owin;

    /// <summary>
    /// Controller for managing clients.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    public class ClientController : Controller
    {
        /// <summary>
        /// Initializes static members of the <see cref="ClientController"/> class.
        /// </summary>
        static ClientController()
        {
            Mapper.CreateMap<Client, EditClientViewModel>();
            Mapper.CreateMap<EditClientViewModel, Client>();
        }

        /// <summary>
        /// Display list of clients.
        /// </summary>
        /// <param name="filter">Filter which should be applied to the clients.</param>
        /// <returns>Return action result.</returns>
        public ActionResult Index([Bind(Prefix = "Filter")]ClientsListFilter filter)
        {
            var context = this.HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var model = new ClientsListViewModel(dbContext.Clients, filter);
            return this.View(model);
        }

        /// <summary>
        /// Show create client page.
        /// </summary>
        /// <returns>Result of the action.</returns>
        public ActionResult Create()
        {
            var model = new EditClientViewModel();
            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the client.
        /// </summary>
        /// <param name="model">Data to save about client.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditClientViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var client = new Client();
            Mapper.Map(model, client);
            dbContext.Clients.Add(client);
            await dbContext.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show edit client page by id.
        /// </summary>
        /// <param name="id">Id of the client to get</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Edit(int id)
        {
            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var client = await dbContext.Clients.FindAsync(id);
            if (client == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new EditClientViewModel();
            Mapper.Map(client, model);
            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the client.
        /// </summary>
        /// <param name="model">Data to save about client.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditClientViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var client = await dbContext.Clients.FindAsync(model.Id);
            if (client == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            Mapper.Map(model, client);
            await dbContext.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show delete prompt for user by it's id.
        /// </summary>
        /// <param name="id">Id of the user to get</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Delete(int id)
        {
            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var client = await dbContext.Clients.FindAsync(id);
            if (client == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new EditClientViewModel();
            Mapper.Map(client, model);
            return this.View(model);
        }

        /// <summary>
        /// Show delete prompt for client by it's id.
        /// </summary>
        /// <param name="model">New data about the client to delete.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(EditClientViewModel model)
        {
            var context = HttpContext.GetOwinContext();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var client = await dbContext.Clients.FindAsync(model.Id);
            if (client == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            dbContext.Clients.Remove(client);
            await dbContext.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }
    }
}