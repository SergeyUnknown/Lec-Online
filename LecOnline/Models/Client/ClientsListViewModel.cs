// -----------------------------------------------------------------------
// <copyright file="ClientsListViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Client
{
    using System.Linq;
    using LecOnline.Core;

    /// <summary>
    /// View model for the clients lists.
    /// </summary>
    public class ClientsListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientsListViewModel"/> class.
        /// </summary>
        /// <param name="items">Items to which should be applied filter.</param>
        /// <param name="filter">Filter which should be applied to the items.</param>
        public ClientsListViewModel(IQueryable<Client> items, ClientsListFilter filter)
        {
            if (filter == null)
            {
                this.Items = items;
                this.Filter = new ClientsListFilter();
            }
            else
            {
                this.Items = filter.Apply(items);
                this.Filter = filter;
            }
        }

        /// <summary>
        /// Gets or sets users.
        /// </summary>
        public IQueryable<Client> Items { get; set; }

        /// <summary>
        /// Gets filter which applied to the items.
        /// </summary>
        public ClientsListFilter Filter { get; private set; }
    }
}
