// -----------------------------------------------------------------------
// <copyright file="MeetingRoomViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.Collections.Generic;
    using LecOnline.Core;

    /// <summary>
    /// View model for the meeting room.
    /// </summary>
    public class MeetingRoomViewModel
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
        /// Gets or sets id of the meeting.
        /// </summary>
        public int MeetingId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether voting is started.
        /// </summary>
        public bool VotingStarted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether chat is enabled.
        /// </summary>
        public bool IsChatEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether vote was placed.
        /// </summary>
        public bool VotePlaced { get; set; }

        /// <summary>
        /// Gets or sets meeting attendees.
        /// </summary>
        public IEnumerable<MeetingAttendee> Attendees { get; set; }
    }
}
