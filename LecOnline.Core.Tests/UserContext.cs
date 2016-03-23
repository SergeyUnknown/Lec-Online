// -----------------------------------------------------------------------
// <copyright file="UserContext.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System.Security.Claims;

    /// <summary>
    /// Context for steps which require information about authenticated user
    /// </summary>
    public class UserContext
    {
        /// <summary>
        /// Gets or sets currently authenticated user.
        /// </summary>
        public ClaimsPrincipal User { get; set; }
    }
}
