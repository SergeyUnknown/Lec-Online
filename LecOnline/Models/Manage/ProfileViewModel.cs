// -----------------------------------------------------------------------
// <copyright file="ProfileViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Manage
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Profile view model.
    /// </summary>
    public class ProfileViewModel
    {
        /// <summary>
        /// Gets or sets first name of the user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldFirstName", ResourceType = typeof(Resources))]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of the user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldLastName", ResourceType = typeof(Resources))]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets patronymic name of the user.
        /// </summary>
        [Display(Name = "FieldPatronymicName", ResourceType = typeof(Resources))]
        public string PatronymicName { get; set; }

        /// <summary>
        /// Gets or sets name where user is located.
        /// </summary>
        [Display(Name = "FieldCity", ResourceType = typeof(Resources))]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets address of the user.
        /// </summary>
        [Display(Name = "FieldAddress", ResourceType = typeof(Resources))]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets contact phone for the user.
        /// </summary>
        [Display(Name = "FieldPhone", ResourceType = typeof(Resources))]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Gets or sets degree of the user.
        /// </summary>
        [Display(Name = "FieldDegree", ResourceType = typeof(Resources))]
        public string Degree { get; set; }
    }
}
