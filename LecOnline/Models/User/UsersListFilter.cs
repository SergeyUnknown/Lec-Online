// -----------------------------------------------------------------------
// <copyright file="UsersListFilter.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using LecOnline.Core;
    using LecOnline.Properties;

    /// <summary>
    /// Filter parameters for the users list.
    /// </summary>
    public class UsersListFilter
    {
        /// <summary>
        /// Gets or sets text which could appear in the name.
        /// </summary>
        [Display(Name = "FilterUserName", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets id of the client to which user should belongs.
        /// </summary>
        [Display(Name = "FilterUserClient", ResourceType = typeof(Resources))]
        [UIHint("Client")]
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets id of the committee to which user should belongs.
        /// </summary>
        [Display(Name = "FilterUserCommittee", ResourceType = typeof(Resources))]
        [UIHint("Committee")]
        public int? CommitteeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to display clients to which user belongs.
        /// </summary>
        public bool FilterClients { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to display committee to which user belongs.
        /// </summary>
        public bool FilterCommittee { get; set; }

        /// <summary>
        /// Apply parameters specified by this filter to the sequence of data.
        /// </summary>
        /// <param name="source">Source sequence to which filter should be applied.</param>
        /// <returns>Filtered sequence.</returns>
        public IQueryable<ApplicationUser> Apply(IQueryable<ApplicationUser> source)
        {
            if (!string.IsNullOrWhiteSpace(this.Name))
            {
                source = source.Where(_ => _.FirstName.Contains(this.Name)
                    || _.LastName.Contains(this.Name)
                    || _.PatronymicName.Contains(this.Name));
            }

            if (this.ClientId.HasValue)
            {
                source = source.Where(_ => _.ClientId == this.ClientId.Value);
            }

            if (this.CommitteeId.HasValue)
            {
                source = source.Where(_ => _.CommitteeId == this.CommitteeId.Value);
            }

            return source;
        }
    }
}
