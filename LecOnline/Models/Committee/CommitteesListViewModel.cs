// -----------------------------------------------------------------------
// <copyright file="CommitteesListViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Committee
{
    using System.Linq;
    using LecOnline.Core;

    /// <summary>
    /// View model for the committees lists.
    /// </summary>
    public class CommitteesListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommitteesListViewModel"/> class.
        /// </summary>
        /// <param name="items">Items to which should be applied filter.</param>
        /// <param name="filter">Filter which should be applied to the items.</param>
        public CommitteesListViewModel(IQueryable<Committee> items, CommitteesListFilter filter)
        {
            if (filter == null)
            {
                this.Items = items;
                this.Filter = new CommitteesListFilter();
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
        public IQueryable<Committee> Items { get; set; }

        /// <summary>
        /// Gets filter which applied to the items.
        /// </summary>
        public CommitteesListFilter Filter { get; private set; }
    }
}
