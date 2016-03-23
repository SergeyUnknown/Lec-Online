// -----------------------------------------------------------------------
// <copyright file="ExternalLoginListViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Account
{
    /// <summary>
    /// View model for the list of external login providers.
    /// </summary>
    public class ExternalLoginListViewModel
    {
        /// <summary>
        /// Gets or sets url to which return after authorization.
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
