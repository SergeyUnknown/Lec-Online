// -----------------------------------------------------------------------
// <copyright file="ApplicationUserManager.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// User manager for this application.
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserManager"/> class.
        /// </summary>
        /// <param name="store">Store which uses for the persisting.</param>
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        /// <summary>
        /// Gets the roles which could be managed by given principal.
        /// </summary>
        /// <param name="user">User for which return roles which he could managed.</param>
        /// <returns>List of roles which could be managed by the user.</returns>
        public static string[] GetManagedRoles(System.Security.Principal.IPrincipal user)
        {
            if (user.IsInRole(RoleNames.Administrator))
            {
                return new[] 
                {
                    RoleNames.Administrator,
                    RoleNames.Manager,
                    RoleNames.EthicalCommitteeMember,
                    RoleNames.MedicalCenter,
                };
            }
            else if (user.IsInRole(RoleNames.Manager))
            {
                return new[] 
                {
                    RoleNames.Manager,
                    RoleNames.MedicalCenter,
                };
            }
            else
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Create a user with no password
        /// </summary>
        /// <param name="user">User which should be created.</param>
        /// <returns>Task which returns information about user creation.</returns>
        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            user.Created = DateTime.UtcNow;
            user.Modified = DateTime.UtcNow;
            return await base.CreateAsync(user);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">User which information should be updated.</param>
        /// <returns>Task which perform updating of the user.</returns>
        public override Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            user.Modified = DateTime.UtcNow;
            return base.UpdateAsync(user);
        }

        /// <summary>
        /// Returns accessible for given principal users.
        /// </summary>
        /// <param name="principal">Principal for which get list of accessible users.</param>
        /// <returns>IQueryable of users accessible for given principal.</returns>
        public IQueryable<ApplicationUser> GetAccessibleUsers(ClaimsPrincipal principal)
        {
            return FilterUsers(this.Users, principal);
        }

        /// <summary>
        /// Returns accessible for given principal users.
        /// </summary>
        /// <param name="principal">Principal for which get list of accessible users.</param>
        /// <param name="committeeId">Id of the committee.</param>
        /// <returns>IQueryable of users accessible for given principal.</returns>
        public IQueryable<ApplicationUser> GetCommitteeMembers(ClaimsPrincipal principal, int committeeId)
        {
            return FilterUsers(this.Users, principal).Where(_ => _.CommitteeId == committeeId);
        }

        /// <summary>
        /// Returns committee members.
        /// </summary>
        /// <param name="committeeId">Id of the committee.</param>
        /// <returns>IQueryable of committee members.</returns>
        public IQueryable<ApplicationUser> GetCommitteeMembers(int committeeId)
        {
            return this.Users.Where(_ => _.CommitteeId == committeeId);
        }

        /// <summary>
        /// Returns client users.
        /// </summary>
        /// <param name="clientId">Id of the client.</param>
        /// <returns>IQueryable of committee members.</returns>
        public IQueryable<ApplicationUser> GetClientMembers(int clientId)
        {
            return this.Users.Where(_ => _.ClientId == clientId);
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
                var clientId = principal.GetClient();
                users = users.Where(_ => _.ClientId == clientId);
            }

            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                var committeeId = principal.GetCommittee();
                users = users.Where(_ => _.CommitteeId == committeeId);
            }

            return users;
        }
    }
}
