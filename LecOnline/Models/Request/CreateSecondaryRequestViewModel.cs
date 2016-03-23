// -----------------------------------------------------------------------
// <copyright file="CreateSecondaryRequestViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Create secondary request view model.
    /// </summary>
    public class CreateSecondaryRequestViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSecondaryRequestViewModel"/> class.
        /// </summary>
        public CreateSecondaryRequestViewModel()
        {
            this.BaseRequestInformation = new RequestBaseInformationViewModel();
            this.BaseInformation = new RequestBaseInformationViewModel();
            this.ContactInformation = new RequestContactInformationViewModel();
            this.Documentation = new RequestDocumentationViewModel();
        }

        /// <summary>
        /// Gets base information about primary request.
        /// </summary>
        public RequestBaseInformationViewModel BaseRequestInformation { get; private set; }

        /// <summary>
        /// Gets base information about application.
        /// </summary>
        public RequestBaseInformationViewModel BaseInformation { get; private set; }

        /// <summary>
        /// Gets contact person information.
        /// </summary>
        public RequestContactInformationViewModel ContactInformation { get; private set; }

        /// <summary>
        /// Gets documentation information.
        /// </summary>
        public RequestDocumentationViewModel Documentation { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether user would be redirected to the edit page
        /// after application creation or not.
        /// </summary>
        [Display(Name = "FieldContinuteToEditPage", ResourceType = typeof(Resources))]
        public bool ContinuteToEditPage { get; set; }
    }
}
