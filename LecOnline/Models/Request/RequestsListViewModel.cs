// -----------------------------------------------------------------------
// <copyright file="RequestsListViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.Linq;
    using LecOnline.Core;

    /// <summary>
    /// View model for the requests list.
    /// </summary>
    public class RequestsListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestsListViewModel"/> class.
        /// </summary>
        /// <param name="items">Items to which should be applied filter.</param>
        /// <param name="filter">Filter which should be applied to the items.</param>
        public RequestsListViewModel(IQueryable<Request> items, RequestsListFilter filter)
        {
            if (filter == null)
            {
                this.Items = items;
                this.Filter = new RequestsListFilter();
            }
            else
            {
                this.Items = filter.Apply(items);
                this.Filter = filter;
            }
        }

        /// <summary>
        /// Gets items which should be displayed.
        /// </summary>
        public IQueryable<Request> Items { get; private set; }

        /// <summary>
        /// Gets filter which applied to the items.
        /// </summary>
        public RequestsListFilter Filter { get; private set; }
    }
}
