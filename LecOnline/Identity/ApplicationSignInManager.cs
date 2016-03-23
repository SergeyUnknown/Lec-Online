// -----------------------------------------------------------------------
// <copyright file="ApplicationSignInManager.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Identity
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using LecOnline.Core;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;

    /// <summary>
    /// Application sign-in manager.
    /// </summary>
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSignInManager"/> class.
        /// </summary>
        /// <param name="userManager">User manager which will be used for sign-in operation.</param>
        /// <param name="authenticationManager">Authentication manager where to perform sign-in</param>
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        /// <summary>
        /// Create application sign-in manager for the given OWIN context.
        /// </summary>
        /// <param name="options">Options for create sign-in manager.</param>
        /// <param name="context">OWIN context where to create sign-in manager.</param>
        /// <returns>Sign-in manager for given OWIN context.</returns>
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        /// <summary>
        /// Creates identity for given application user.
        /// </summary>
        /// <param name="user">User for which create identity.</param>
        /// <returns>Identity for given user.</returns>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
    }
}
