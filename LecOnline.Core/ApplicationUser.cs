// -----------------------------------------------------------------------
// <copyright file="ApplicationUser.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
        
    /// <summary>
    /// Represents application user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets first name of the user.
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Gets or sets last name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets patronymic name of the user.
        /// </summary>
        public string PatronymicName { get; set; }

        /// <summary>
        /// Gets or sets name where user is located.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets address of the user.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets contact phone for the user.
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Gets or sets company name for the user.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets degree of the user.
        /// </summary>
        public string Degree { get; set; }

        /// <summary>
        /// Gets or sets date when user was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets date when user was modified.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets id of the client to which this user belongs.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets id of the committee to which this user belongs.
        /// </summary>
        public int? CommitteeId { get; set; }

        /// <summary>
        /// Gets full name of the person.
        /// </summary>
        [NotMapped]
        public string FullName
        {
            get
            {
                return this.LastName + " " + this.FirstName;
            }
        }

        /// <summary>
        /// Gets full name of the person.
        /// </summary>
        [NotMapped]
        public string ShortFullName
        {
            get
            {
                return this.Initials + " " + this.LastName;
            }
        }

        /// <summary>
        /// Gets initials of the person.
        /// </summary>
        [NotMapped]
        public string Initials
        {
            get
            {
                var firstName = string.IsNullOrEmpty(this.FirstName) ? string.Empty : this.FirstName[0] + ".";
                var patronymicName = string.IsNullOrEmpty(this.PatronymicName) ? string.Empty : this.PatronymicName[0] + ".";
                return firstName + patronymicName;
            }
        }
        
        /// <summary>
        /// Generates identity from this user entity.
        /// </summary>
        /// <param name="manager">User manager which used for create identity</param>
        /// <returns>Task which return identity based on current user entity</returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            userIdentity.AddClaim(new Claim(ClaimTypes.Sid, this.Id.ToString()));
            userIdentity.AddClaim(new Claim(ClaimTypes.Surname, this.LastName));
            userIdentity.AddClaim(new Claim(ClaimTypes.GivenName, this.FirstName));
            if (this.ClientId.HasValue)
            {
                userIdentity.AddClaim(new Claim(WellKnownClaims.ClientClaim, this.ClientId.ToString()));
            }

            if (this.CommitteeId.HasValue)
            {
                userIdentity.AddClaim(new Claim(WellKnownClaims.CommitteeClaim, this.CommitteeId.ToString()));

                var dbContext = new LecOnlineDbEntities();
                var committee = await dbContext.Committees.FindAsync(this.CommitteeId);
                if (this.Id == committee.Chairman)
                {
                    userIdentity.AddClaim(new Claim(WellKnownClaims.CommitteeChairmanClaim, this.CommitteeId.ToString()));
                }

                if (this.Id == committee.Secretary)
                {
                    userIdentity.AddClaim(new Claim(WellKnownClaims.CommitteeSecretaryClaim, this.CommitteeId.ToString()));
                }
            }

            return userIdentity;
        }
    }
}
