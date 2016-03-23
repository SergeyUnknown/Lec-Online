// -----------------------------------------------------------------------
// <copyright file="RequestDetailsViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Request details view model.
    /// </summary>
    public class RequestDetailsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestDetailsViewModel"/> class.
        /// </summary>
        public RequestDetailsViewModel()
        {
            this.BaseInformation = new RequestBaseInformationViewModel();
            this.ContactInformation = new RequestContactInformationViewModel();
            this.Documentation = new RequestDocumentationViewModel();
            this.Actions = new RequestActionsViewModel();
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

        /// <summary>
        /// Gets request actions information.
        /// </summary>
        public RequestActionsViewModel Actions { get; private set; }

        /// <summary>
        /// Gets or sets original request from which this model produced.
        /// </summary>
        public Core.Request OriginalRequest { get; set; }
    }
}
