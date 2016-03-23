// -----------------------------------------------------------------------
// <copyright file="RequestType.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Type of request.
    /// </summary>
    public enum RequestType : byte
    {
        /// <summary>
        /// Type of request which is sent for the first time.
        /// </summary>
        PrimaryRequestType = 0,

        /// <summary>
        /// Type of request which is sent subsequently after the primary request, 
        /// when no need for the setting up meeting.
        /// This is just informational request.
        /// </summary>
        NotificationRequest = 1,

        /// <summary>
        /// Type of request which is sent subsequently after the primary request, 
        /// when there is the need for the setting up meeting.
        /// </summary>
        SecondaryRequest = 2
    }
}
