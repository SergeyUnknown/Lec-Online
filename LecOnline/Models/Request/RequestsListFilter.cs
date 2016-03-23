// -----------------------------------------------------------------------
// <copyright file="RequestsListFilter.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.SqlServer;
    using System.Linq;
    using LecOnline.Core;
    using LecOnline.Properties;

    /// <summary>
    /// Filter parameters for the requests list.
    /// </summary>
    public class RequestsListFilter
    {
        /// <summary>
        /// Gets or sets text which could appear in the name.
        /// </summary>
        [Display(Name = "FilterRequestName", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets status which should have request.
        /// </summary>
        [Display(Name = "FilterRequestStatus", ResourceType = typeof(Resources))]
        public RequestStatus? Status { get; set; }

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
        public IQueryable<Request> Apply(IQueryable<Request> source)
        {
            if (!string.IsNullOrWhiteSpace(this.Name))
            {
                source = source.Where(_ => _.Title.Contains(this.Name));
            }

            if (this.Status.HasValue)
            {
                source = source.Where(_ => _.Status == (int)this.Status.Value);
            }

            if (this.FromDate.HasValue)
            {
                var fromDate = this.FromDate.GetValueOrDefault(DateTime.MinValue);
                source = source.Where(_ => (RequestStatus)_.Status != RequestStatus.InvalidSubmission
                        && (RequestStatus)_.Status != RequestStatus.Resolved
                        && (RequestStatus)_.Status != RequestStatus.Resolved)
                    .Where(_ => SqlFunctions.DateDiff("minute", _.Created, fromDate) <= 0);
            }

            if (this.TillDate.HasValue)
            {
                var tillDate = this.TillDate.GetValueOrDefault(DateTime.MinValue);
                source = source.Where(_ => (RequestStatus)_.Status != RequestStatus.InvalidSubmission
                        && (RequestStatus)_.Status != RequestStatus.Resolved)
                    .Where(_ => SqlFunctions.DateDiff("minute", _.Created, tillDate) >= 0);
            }

            return source;
        }
    }
}
