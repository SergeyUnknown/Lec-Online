// -----------------------------------------------------------------------
// <copyright file="SetMeetingViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using LecOnline.Properties;

    /// <summary>
    /// View model for setting meeting.
    /// </summary>
    public class SetMeetingViewModel
    {
        /// <summary>
        /// Gets or sets id of the request.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title for the request.
        /// </summary>
        [Display(Name = "FieldTitle", ResourceType = typeof(Resources))]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets meeting date.
        /// </summary>
        [Display(Name = "FieldMeetingDate", ResourceType = typeof(Resources))]
        public DateTime MeetingDate { get; set; }

        /// <summary>
        /// Gets or sets list of committee members which would be on the meeting.
        /// </summary>
        [Display(Name = "FieldMembers", ResourceType = typeof(Resources))]
        public IEnumerable<string> Members { get; set; }

        /// <summary>
        /// Gets or sets list of committee members which could be on the meeting.
        /// </summary>
        public IEnumerable<SelectListItem> AvailableMembers { get; set; }
    }
}
