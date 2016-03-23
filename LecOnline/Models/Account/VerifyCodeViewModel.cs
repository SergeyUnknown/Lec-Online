﻿// -----------------------------------------------------------------------
// <copyright file="VerifyCodeViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// View model for verification of security code in 
    /// two-factor authorization.
    /// </summary>
    public class VerifyCodeViewModel
    {
        /// <summary>
        /// Gets or sets selected login provider.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        public string Provider { get; set; }

        /// <summary>
        /// Gets or sets security code entered by the user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldCode", ResourceType = typeof(Resources))]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets a url to which return after authentication.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether authentication 
        /// in current browser should be persisted.
        /// </summary>
        [Display(Name = "FieldRememberThisBrowser", ResourceType = typeof(Resources))]
        public bool RememberBrowser { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to persist authenticated user.
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
