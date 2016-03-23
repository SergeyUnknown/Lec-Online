// -----------------------------------------------------------------------
// <copyright file="MeetingStatus.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Available statuses for the meetings.
    /// </summary>
    public enum MeetingStatus
    {
        /// <summary>
        /// Meeting is just setup.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Meeting is cancelled.
        /// </summary>
        Cancelled = 10,

        /// <summary>
        /// Meeting is started.
        /// </summary>
        Started = 20,

        /// <summary>
        /// Voting phase of meeting is started.
        /// </summary>
        Voting = 30,

        /// <summary>
        /// Meeting is completed and voting is finished.
        /// </summary>
        Completed = 40,

        /// <summary>
        /// Meeting is completed and voting is finished.
        /// </summary>
        HasResolution = 50,
    }
}
