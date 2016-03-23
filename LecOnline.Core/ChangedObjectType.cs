// -----------------------------------------------------------------------
// <copyright file="ChangedObjectType.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Type of the object that changed.
    /// </summary>
    public enum ChangedObjectType
    {
        /// <summary>
        /// System settings changed.
        /// </summary>
        SystemSettings = 0,

        /// <summary>
        /// User changed.
        /// </summary>
        User = 1,

        /// <summary>
        /// Client changed.
        /// </summary>
        Client = 2,

        /// <summary>
        /// Committee changed.
        /// </summary>
        Committee = 3,

        /// <summary>
        /// Request changed.
        /// </summary>
        Request = 4,

        /// <summary>
        /// Meeting changed.
        /// </summary>
        Meeting = 5,
    }
}
