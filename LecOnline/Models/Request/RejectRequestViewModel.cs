// -----------------------------------------------------------------------
// <copyright file="RejectRequestViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// View model for the rejection of processing submitted application.
    /// </summary>
    public class RejectRequestViewModel
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
        /// Gets or sets comments why application was rejected.
        /// </summary>
        [Display(Name = "FieldRejectionReason", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        public string RejectionReason { get; set; }
    }
}
