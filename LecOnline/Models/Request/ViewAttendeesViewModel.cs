// -----------------------------------------------------------------------
// <copyright file="ViewAttendeesViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.Collections.Generic;
    using LecOnline.Core;

    /// <summary>
    /// View model for the displaying meetings attendees list.
    /// </summary>
    public class ViewAttendeesViewModel
    {
        /// <summary>
        /// Gets or sets id of the request.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title of the request for which display meeting attendees.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets meeting attendees.
        /// </summary>
        public IEnumerable<MeetingAttendee> Attendees { get; set; }
    }
}
