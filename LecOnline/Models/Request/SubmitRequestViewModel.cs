// -----------------------------------------------------------------------
// <copyright file="SubmitRequestViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// View model for submitting request.
    /// </summary>
    public class SubmitRequestViewModel
    {
        /// <summary>
        /// Gets or sets id of the request.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether manager is aware about missing files and request should be sent.
        /// </summary>
        [Display(Name = "FieldSuppressMissingFiles", ResourceType = typeof(Resources))]
        public bool SuppressMissingFiles { get; set; }

        /// <summary>
        /// Gets or sets title for the request.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldTitle", ResourceType = typeof(Resources))]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets id of the committee to which this request belongs.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldSelectCommitteeForSubmission", ResourceType = typeof(Resources))]
        [UIHint("Committee")]
        public int? CommitteeId { get; set; }

        /// <summary>
        /// Gets or sets comments for the committee to which application is submitted.
        /// </summary>
        [Display(Name = "FieldSubmissionComment", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}
