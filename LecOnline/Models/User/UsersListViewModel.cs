// -----------------------------------------------------------------------
// <copyright file="UsersListViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.User
{
    using System.Linq;
    using LecOnline.Core;

    /// <summary>
    /// View model for the user lists.
    /// </summary>
    public class UsersListViewModel
    {
        /// <summary>
        /// A value indicating whether need to display clients to which user belongs.
        /// </summary>
        private bool displayClients;

        /// <summary>
        /// A value indicating whether need to display committee to which user belongs.
        /// </summary>
        private bool displayCommittee;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersListViewModel"/> class.
        /// </summary>
        /// <param name="items">Items to which should be applied filter.</param>
        /// <param name="filter">Filter which should be applied to the items.</param>
        public UsersListViewModel(IQueryable<ApplicationUser> items, UsersListFilter filter)
        {
            if (filter == null)
            {
                this.Users = items;
                this.Filter = new UsersListFilter();
            }
            else
            {
                this.Users = filter.Apply(items);
                this.Filter = filter;
            }
        }

        /// <summary>
        /// Gets or sets users.
        /// </summary>
        public IQueryable<ApplicationUser> Users { get; set; }

        /// <summary>
        /// Gets filter which applied to the items.
        /// </summary>
        public UsersListFilter Filter { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to display clients to which user belongs.
        /// </summary>
        public bool DisplayClients
        {
            get
            {
                return this.displayClients;
            }

            set
            {
                this.displayClients = value;
                this.Filter.FilterClients = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether need to display committee to which user belongs.
        /// </summary>
        public bool DisplayCommittee
        {
            get
            {
                return this.displayCommittee;
            }

            set
            {
                this.displayCommittee = value;
                this.Filter.FilterCommittee = value;
            }
        }
    }
}
