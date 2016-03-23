// -----------------------------------------------------------------------
// <copyright file="ChangesListViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LecOnline.Core;

    /// <summary>
    /// View model for the changes log items list.
    /// </summary>
    public class ChangesListViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangesListViewModel"/> class.
        /// </summary>
        /// <param name="items">Items to which should be applied filter.</param>
        /// <param name="filter">Filter which should be applied to the items.</param>
        public ChangesListViewModel(IQueryable<ChangesLog> items, ChangesListFilter filter)
        {
            if (filter == null)
            {
                this.Items = items;
                this.Filter = new ChangesListFilter();
            }
            else
            {
                this.Items = filter.Apply(items);
                this.Filter = filter;
            }
        }

        /// <summary>
        /// Gets or sets items to display.
        /// </summary>
        public IQueryable<ChangesLog> Items { get; set; }

        /// <summary>
        /// Gets filter which applied to the items.
        /// </summary>
        public ChangesListFilter Filter { get; private set; }
    }
}
