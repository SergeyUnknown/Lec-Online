// -----------------------------------------------------------------------
// <copyright file="ForgotPasswordViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Model for the forget password procedure.
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// Gets or sets email on which send password reset request.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceName = "ValidationMessageEmailAddress", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether that this block should be initially visible.
        /// </summary>
        public bool InitiallyVisible { get; set; }
    }
}
