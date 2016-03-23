// -----------------------------------------------------------------------
// <copyright file="ConfigureTwoFactorViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Manage
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// View model for the two-factor authorization configuration page.
    /// </summary>
    public class ConfigureTwoFactorViewModel
    {
        /// <summary>
        /// Gets or sets external login provider selected.s
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Gets or sets list of available login providers.
        /// </summary>
        public ICollection<SelectListItem> Providers { get; set; }
    }
}
