﻿// -----------------------------------------------------------------------
// <copyright file="SendCodeViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Account
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// View model for the sending security code for two-factor authorization.
    /// </summary>
    public class SendCodeViewModel
    {
        /// <summary>
        /// Gets or sets selected login provider.
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets list of available login providers.
        /// </summary>
        public ICollection<SelectListItem> Providers { get; set; }
        
        /// <summary>
        /// Gets or sets a url to which return after authentication.
        /// </summary>
        public string ReturnUrl { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether need to persist authenticated user.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
