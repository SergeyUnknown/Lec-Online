// -----------------------------------------------------------------------
// <copyright file="ManageLoginsViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Manage
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    /// <summary>
    /// View model for managing logins from different providers.
    /// </summary>
    public class ManageLoginsViewModel
    {
        /// <summary>
        /// Gets or sets list of available logins.
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        /// <summary>
        /// Gets or sets list of available providers.
        /// </summary>
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
}
