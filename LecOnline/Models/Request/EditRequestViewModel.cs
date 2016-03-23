// -----------------------------------------------------------------------
// <copyright file="EditRequestViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Edit request view model.
    /// </summary>
    public class EditRequestViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditRequestViewModel"/> class.
        /// </summary>
        public EditRequestViewModel()
        {
            this.BaseInformation = new RequestBaseInformationViewModel();
            this.ContactInformation = new RequestContactInformationViewModel();
            this.Documentation = new RequestDocumentationViewModel();
        }

        /// <summary>
        /// Gets or sets id of the request.
        /// </summary>
        public int Id { get; set; }

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
    }
}
