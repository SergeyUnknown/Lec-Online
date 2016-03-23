// -----------------------------------------------------------------------
// <copyright file="RequestContactInformationViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Application contact information view model.
    /// </summary>
    public class RequestContactInformationViewModel
    {
        /// <summary>
        /// Gets or sets contact person for any question related to request.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldContactPerson", ResourceType = typeof(Resources))]
        public string ContactPerson { get; set; }

        /// <summary>
        /// Gets or sets phone of the contact person.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldPhone", ResourceType = typeof(Resources))]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Gets or sets fax of the contact person.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldFax", ResourceType = typeof(Resources))]
        public string ContactFax { get; set; }

        /// <summary>
        /// Gets or sets email of the contact person.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldEmail", ResourceType = typeof(Resources))]
        public string ContactEmail { get; set; }
    }
}
