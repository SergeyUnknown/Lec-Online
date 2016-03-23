// -----------------------------------------------------------------------
// <copyright file="RequestContext.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    /// <summary>
    /// Context associated with request operations.
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// Gets or sets current request.
        /// </summary>
        public Request CurrentRequest { get; set; }

        /// <summary>
        /// Gets or sets request manager.
        /// </summary>
        public RequestManager Manager { get; set; }

        /// <summary>
        /// Gets or sets request notifications.
        /// </summary>
        public RequestNotifications Notifications { get; set; }
    }
}
