// -----------------------------------------------------------------------
// <copyright file="ClientsListFilter.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Client
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using LecOnline.Core;
    using LecOnline.Properties;

    /// <summary>
    /// Filter parameters for the clients list.
    /// </summary>
    public class ClientsListFilter
    {
        /// <summary>
        /// Gets or sets text which could appear in the name.
        /// </summary>
        [Display(Name = "FilterUserName", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        /// <summary>
        /// Apply parameters specified by this filter to the sequence of data.
        /// </summary>
        /// <param name="source">Source sequence to which filter should be applied.</param>
        /// <returns>Filtered sequence.</returns>
        public IQueryable<Client> Apply(IQueryable<Client> source)
        {
            if (!string.IsNullOrWhiteSpace(this.Name))
            {
                source = source.Where(_ => _.CompanyName.Contains(this.Name));
            }

            return source;
        }
    }
}
