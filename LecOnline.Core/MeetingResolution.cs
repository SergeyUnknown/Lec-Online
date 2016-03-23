// -----------------------------------------------------------------------
// <copyright file="MeetingResolution.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Possible values for the meeting resolution.
    /// </summary>
    public enum MeetingResolution
    {
        /// <summary>
        /// Resolution not provided.
        /// </summary>
        NotProvided,

        /// <summary>
        /// Study accepted.
        /// </summary>
        StudyAccepted,

        /// <summary>
        /// Study rejected.
        /// </summary>
        StudyRejected,
    }
}
