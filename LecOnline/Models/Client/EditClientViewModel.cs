// -----------------------------------------------------------------------
// <copyright file="EditClientViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Client
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Model for the client editing.
    /// </summary>
    public class EditClientViewModel
    {
        /// <summary>
        /// Gets or sets id of the client.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets company name.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldCompanyName", ResourceType = typeof(Resources))]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets contact person within company.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldContactPerson", ResourceType = typeof(Resources))]
        public string ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets contact email for the client.
        /// </summary>
        [Display(Name = "FieldEmail", ResourceType = typeof(Resources))]
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets contact phone for the client.
        /// </summary>
        [Display(Name = "FieldPhone", ResourceType = typeof(Resources))]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Gets or sets notes for the client.
        /// </summary>
        [Display(Name = "FieldNotes", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }
}
