// -----------------------------------------------------------------------
// <copyright file="UserController.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using LecOnline.Core;
    using LecOnline.Identity;
    using LecOnline.Models;
    using LecOnline.Models.User;
    using LecOnline.Properties;
    using Microsoft.AspNet.Identity.Owin;

    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [Authorize]
    public class UserController : Controller
    {
        /// <summary>
        /// User manager.
        /// </summary>
        private ApplicationUserManager userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        public UserController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userManager">User manager for this controller.</param>
        public UserController(ApplicationUserManager userManager)
        {
            this.UserManager = userManager;
        }

        /// <summary>
        /// Gets user manager.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        /// <summary>
        /// Displays list of all users.
        /// </summary>
        /// <param name="filter">Filter which should be applied to the users.</param>
        /// <returns>Result of the action.</returns>
        [Authorize(Roles = LecOnline.Core.RoleNames.AdministratorAndManager)]
        public ActionResult Index([Bind(Prefix = "Filter")]UsersListFilter filter)
        {
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var principal = (System.Security.Claims.ClaimsPrincipal)HttpContext.User;
            var users = userManager.GetAccessibleUsers(principal);
            var isAdmin = principal.IsInRole(RoleNames.Administrator);
            var model = new UsersListViewModel(users, filter)
            {
                DisplayClients = isAdmin,
                DisplayCommittee = isAdmin,
            };
            return this.View(model);
        }

        /// <summary>
        /// Displays list of the users which wait for approval of the registration.
        /// </summary>
        /// <param name="filter">Filter which should be applied to the users.</param>
        /// <returns>Result of the action.</returns>
        public ActionResult Pending([Bind(Prefix = "Filter")]UsersListFilter filter)
        {
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var principal = (System.Security.Claims.ClaimsPrincipal)HttpContext.User;
            var notConfirmedUsers = userManager.GetAccessibleUsers(principal)
                .Where(_ => !_.EmailConfirmed);
            var isAdmin = principal.IsInRole(RoleNames.Administrator);
            var model = new UsersListViewModel(notConfirmedUsers, filter)
            {
                DisplayClients = isAdmin,
                DisplayCommittee = isAdmin,
            };
            return this.View(model);
        }

        /// <summary>
        /// Show create user page.
        /// </summary>
        /// <returns>Result of the action.</returns>
        public ActionResult Create()
        {
            var model = new CreateUserViewModel();
            return this.View(model);
        }

        /// <summary>
        /// Saves changes during creation of the user.
        /// </summary>
        /// <param name="model">Data to use when create user.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var user = new ApplicationUser();
            user.Email = model.Email;
            user.UserName = model.Email;
            
            if (this.User.IsInRole(RoleNames.Administrator))
            {
                user.ClientId = model.ClientId;
                user.CommitteeId = model.CommitteeId;
            }
            else
            {
                user.ClientId = claimPrincipal.GetClient();
                user.CommitteeId = null;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PatronymicName = model.PatronymicName;
            user.City = model.City;
            user.Address = model.Address;
            user.ContactPhone = model.ContactPhone;
            user.Degree = model.Degree;
            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                // Add errors.
                foreach (var errorMessage in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, errorMessage);
                }

                return this.View(model);
            }

            model.Roles = this.SanitizeRoles(model.Roles ?? new string[0]);

            var currentRoles = await userManager.GetRolesAsync(user.Id);

            // Add new roles
            var rolesAdded = model.Roles.Except(currentRoles).ToArray();
            result = await userManager.AddToRolesAsync(user.Id, rolesAdded);
            if (!result.Succeeded)
            {
                // Add errors.
                foreach (var errorMessage in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, errorMessage);
                }

                return this.View(model);
            }

            // Remove roles
            var rolesRemoved = currentRoles.Except(model.Roles).ToArray();
            result = await userManager.RemoveFromRolesAsync(user.Id, rolesRemoved);
            if (!result.Succeeded)
            {
                foreach (var errorMessage in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, errorMessage);
                }

                return this.View(model);
            }

            // Automatically confirm account
            string code = await this.UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            await this.UserManager.ConfirmEmailAsync(user.Id, code);

            // Send an email with password reset
            var passwordResetToken = await this.UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = this.Url.Action("ResetPassword", "Account", new { userId = user.Id, code = passwordResetToken }, protocol: Request.Url.Scheme);
            var message = string.Format(MailStrings.MailCreateAccountBody, callbackUrl);
            await this.UserManager.SendEmailAsync(
                user.Id,
                MailStrings.MailCreateAccountSubject,
                message);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show edit user page by id.
        /// </summary>
        /// <param name="id">Id of the user to get</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Edit(string id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new EditUserViewModel();
            model.Id = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.PatronymicName = user.PatronymicName;
            model.City = user.City;
            model.Address = user.Address;
            model.ContactPhone = user.ContactPhone;
            model.Degree = user.Degree;
            var roles = await userManager.GetRolesAsync(user.Id);
            model.Roles = roles.ToArray();
            if (this.User.IsInRole(RoleNames.Administrator))
            {
                model.ClientId = user.ClientId;
                model.CommitteeId = user.CommitteeId;
            }
            else
            {
                model.ClientId = claimPrincipal.GetClient();
                model.CommitteeId = null;
            }

            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the user.
        /// </summary>
        /// <param name="model">Data to save about user.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(model.Id);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PatronymicName = model.PatronymicName;
            user.City = model.City;
            user.Address = model.Address;
            user.ContactPhone = model.ContactPhone;
            user.Degree = model.Degree;

            if (this.User.IsInRole(RoleNames.Administrator))
            {
                user.ClientId = model.ClientId;
                user.CommitteeId = model.CommitteeId;
            }
            else
            {
                user.ClientId = claimPrincipal.GetClient();
                user.CommitteeId = null;
            }

            if (model.ClientId != null && model.CommitteeId != null)
            {
                this.ModelState.AddModelError("ClientId", Resources.ValidationMessageCouldNotSetBothClientAndCommittee);
                this.ModelState.AddModelError("CommitteeId", Resources.ValidationMessageCouldNotSetBothClientAndCommittee);
                return this.View(model);
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // Add errors.
                foreach (var errorMessage in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, errorMessage);
                }

                return this.View(model);
            }

            model.Roles = this.SanitizeRoles(model.Roles);
            if (model.Roles.Length > 1)
            {
                this.ModelState.AddModelError("Roles", Resources.ValidationMessageCouldNotHaveMoreThenOneRole);
                return this.View(model);
            }

            if (model.Roles.Length == 0)
            {
                this.ModelState.AddModelError("Roles", Resources.ValidationMessageShouldHaveAtLeastOneRole);
                return this.View(model);
            }

            var currentRoles = await userManager.GetRolesAsync(user.Id);

            // Add new roles
            var rolesAdded = model.Roles.Except(currentRoles).ToArray();
            result = await userManager.AddToRolesAsync(user.Id, rolesAdded);
            if (!result.Succeeded)
            {
                // Add errors.
                foreach (var errorMessage in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, errorMessage);
                }

                return this.View(model);
            }

            // Remove roles
            var rolesRemoved = currentRoles.Except(model.Roles).ToArray();
            result = await userManager.RemoveFromRolesAsync(user.Id, rolesRemoved);
            if (!result.Succeeded)
            {
                foreach (var errorMessage in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, errorMessage);
                }

                return this.View(model);
            }

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show delete prompt for user by it's id.
        /// </summary>
        /// <param name="id">Id of the user to get</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Delete(string id)
        {
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new EditUserViewModel();
            model.Id = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.PatronymicName = user.PatronymicName;
            model.City = user.City;
            model.Address = user.Address;
            model.ContactPhone = user.ContactPhone;
            var roles = await userManager.GetRolesAsync(user.Id);
            model.Roles = roles.ToArray();
            return this.View(model);
        }

        /// <summary>
        /// Show delete prompt for user by it's id.
        /// </summary>
        /// <param name="model">New data about the user to delete.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(EditUserViewModel model)
        {
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Add errors.
                foreach (var errorMessage in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, errorMessage);
                }

                return this.View(model);
            }

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show delete prompt for user by it's id.
        /// </summary>
        /// <param name="id">Id of the user to get</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Activate(string id)
        {
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            user.EmailConfirmed = true;
            var result = await userManager.UpdateAsync(user);

            return this.ReturnToPreviousPage();
        }

        /// <summary>
        /// Show delete prompt for user by it's id.
        /// </summary>
        /// <param name="id">Id of the user to get</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Deactivate(string id)
        {
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            user.EmailConfirmed = false;
            var result = await userManager.UpdateAsync(user);

            return this.ReturnToPreviousPage();
        }

        /// <summary>
        /// Shows prompt for resetting user password.
        /// </summary>
        /// <param name="id">Id of the user to reset password for.</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> ResetPassword(string id)
        {
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            user.EmailConfirmed = true;
            var passwordResetToken = await userManager.GeneratePasswordResetTokenAsync(id);
            var callbackUrl = this.Url.Action(
                "ResetPassword", 
                "Account", 
                new { userId = user.Id, code = passwordResetToken }, 
                protocol: Request.Url.Scheme);
            var message = string.Format(MailStrings.ResetPasswordBody, callbackUrl);
            await this.UserManager.SendEmailAsync(
                user.Id,
                MailStrings.ResetPasswordSubject,
                message);
            return this.ReturnToPreviousPage();
        }

        /// <summary>
        /// Filter list of users based on availability.
        /// </summary>
        /// <param name="users">Sequence of users.</param>
        /// <param name="principal">Principal which is used for accessing data.</param>
        /// <returns>Filter sequence with applied security rules.</returns>
        private static IQueryable<ApplicationUser> FilterUsers(IQueryable<ApplicationUser> users, System.Security.Claims.ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.Manager))
            {
                var claim = principal.FindFirst(WellKnownClaims.ClientClaim);
                var clientId = claim == null ? null : (int?)Convert.ToInt32(claim.Value);
                users = users.Where(_ => _.ClientId == clientId);
            }

            return users;
        }

        /// <summary>
        /// Redirect to page from which this action was called.
        /// </summary>
        /// <returns>Result of action execution.</returns>
        private ActionResult ReturnToPreviousPage()
        {
            // Return to the same page as before.
            // This should be checked to be called from correct
            // address, since somebody from untrusted website could execute our 
            // method and returns to their website.
            // We should always return to our website, when in doubts.
            return this.Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        /// <summary>
        /// Filter out roles which user could not manage.
        /// </summary>
        /// <param name="roles">Roles which user has.</param>
        /// <returns>Roles which user allowed to manage.</returns>
        private string[] SanitizeRoles(string[] roles)
        {
            string[] allowedManageRoles = ApplicationUserManager.GetManagedRoles(this.User);
            allowedManageRoles = roles.Intersect(allowedManageRoles).ToArray();
            return allowedManageRoles;
        }
    }
}