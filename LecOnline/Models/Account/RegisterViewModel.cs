﻿// -----------------------------------------------------------------------
// <copyright file="RegisterViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Model for the user registration process.
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Gets or sets email of the registered user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [EmailAddress(ErrorMessageResourceName = "ValidationMessageEmailAddress", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldEmail", ResourceType = typeof(Resources))]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets password for the new registered user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [StringLength(100, ErrorMessageResourceName = "ValidationMessageStringLength", ErrorMessageResourceType = typeof(Resources), MinimumLength = 6, ErrorMessage = null)]
        [DataType(DataType.Password)]
        [Display(Name = "FieldPassword", ResourceType = typeof(Resources))]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets password confirmation.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "FieldConfirmPassword", ResourceType = typeof(Resources))]
        [Compare("Password", ErrorMessageResourceName = "ValidationMessageCompare", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        [Display(Name = "FieldFirstName", ResourceType = typeof(Resources))]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        [Display(Name = "FieldLastName", ResourceType = typeof(Resources))]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets patronymic name.
        /// </summary>
        [Display(Name = "FieldPatronymicName", ResourceType = typeof(Resources))]
        public string PatronymicName { get; set; }

        /// <summary>
        /// Gets or sets company of the registered user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldCompany", ResourceType = typeof(Resources))]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets contact phone of the registered user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldPhone", ResourceType = typeof(Resources))]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user agreement accepted.
        /// </summary>
        public bool UserAgreementAccepted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether that this block should be initially visible.
        /// </summary>
        public bool InitiallyVisible { get; set; }
    }
}
