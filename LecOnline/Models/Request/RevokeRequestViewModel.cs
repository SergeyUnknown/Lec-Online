// -----------------------------------------------------------------------
// <copyright file="RevokeRequestViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// View model for the revocation of previously submitted application .
    /// </summary>
    public class RevokeRequestViewModel
    {
        /// <summary>
        /// Gets or sets id of the request.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets title for the request.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldTitle", ResourceType = typeof(Resources))]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets comments for the committee to which application is submitted.
        /// </summary>
        [Display(Name = "FieldRevocationComment", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}
