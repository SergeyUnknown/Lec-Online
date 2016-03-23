// -----------------------------------------------------------------------
// <copyright file="AttendanceStatus.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Enumeration for the attendance status
    /// </summary>
    public enum AttendanceStatus
    {
        /// <summary>
        /// Attendance is pending confirmation.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Member would be present on the meeting.
        /// </summary>
        InvitationAccepted = 10,

        /// <summary>
        /// Member would not be present on the meeting.
        /// </summary>
        InvitationDeclined = 20,
    }
}
