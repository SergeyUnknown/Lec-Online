﻿// -----------------------------------------------------------------------
// <copyright file="ChangesListFilter.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Security
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using LecOnline.Core;
    using LecOnline.Properties;

    /// <summary>
    /// Filter for the changes log items
    /// </summary>
    public class ChangesListFilter
    {
        /// <summary>
        /// Gets or sets beginning date from which search for requests.
        /// </summary>
        [Display(Name = "FilterFromDate", ResourceType = typeof(Resources))]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Gets or sets ending date from which search for requests.
        /// </summary>
        [Display(Name = "FilterTillDate", ResourceType = typeof(Resources))]
        [DataType(DataType.Date)]
        public DateTime? TillDate { get; set; }

        /// <summary>
        /// Apply parameters specified by this filter to the sequence of data.
        /// </summary>
        /// <param name="source">Source sequence to which filter should be applied.</param>
        /// <returns>Filtered sequence.</returns>
        public IQueryable<ChangesLog> Apply(IQueryable<ChangesLog> source)
        {
            if (this.FromDate.HasValue)
            {
                var fromDate = this.FromDate.GetValueOrDefault(DateTime.MinValue);
                source = source.Where(_ => SqlFunctions.DateDiff("minute", _.Changed, fromDate) <= 0);
            }

            if (this.TillDate.HasValue)
            {
                var tillDate = this.TillDate.GetValueOrDefault(DateTime.MinValue);
                source = source.Where(_ => SqlFunctions.DateDiff("minute", _.Changed, tillDate) >= 0);
            }

            return source;
        }
    }
}
