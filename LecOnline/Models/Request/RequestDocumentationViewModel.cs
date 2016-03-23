// -----------------------------------------------------------------------
// <copyright file="RequestDocumentationViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LecOnline.Core;

    /// <summary>
    /// View model for the documentation associated with request.
    /// </summary>
    public class RequestDocumentationViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestDocumentationViewModel"/> class.
        /// </summary>
        public RequestDocumentationViewModel()
        {
            this.Files = new List<RequestDocumentationFileViewModel>();
        }

        /// <summary>
        /// Gets or sets id of the request.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets type of the request.
        /// </summary>
        public RequestType RequestType { get; set; }

        /// <summary>
        /// Gets allowance from ministry of health.
        /// </summary>
        public RequestDocumentationFileViewModel AllowanceFromMinistryOfHealth
        {
            get
            {
                return this.Files.FirstOrDefault(_ => _.FileType == DocumentationType.AllowanceFromMinistryOfHealth);
            }
        }

        /// <summary>
        /// Gets excerpt from ethics protocol.
        /// </summary>
        public RequestDocumentationFileViewModel ExcerptFromEthicsProtocol
        {
            get
            {
                return this.Files.FirstOrDefault(_ => _.FileType == DocumentationType.ExcerptFromEthicsProtocol);
            }
        }

        /// <summary>
        /// Gets study protocol.
        /// </summary>
        public RequestDocumentationFileViewModel StudyProtocol
        {
            get
            {
                return this.Files.FirstOrDefault(_ => _.FileType == DocumentationType.StudyProtocol);
            }
        }

        /// <summary>
        /// Gets researcher's brochure.
        /// </summary>
        public RequestDocumentationFileViewModel ResearchBrochure
        {
            get
            {
                return this.Files.FirstOrDefault(_ => _.FileType == DocumentationType.ResearchBrochure);
            }
        }

        /// <summary>
        /// Gets informed consent form.
        /// </summary>
        public RequestDocumentationFileViewModel InformedAgreementForm
        {
            get
            {
                return this.Files.FirstOrDefault(_ => _.FileType == DocumentationType.InformedAgreementForm);
            }
        }

        /// <summary>
        /// Gets additional files.
        /// </summary>
        public IEnumerable<RequestDocumentationFileViewModel> AdditionalFiles
        {
            get
            {
                return this.Files.Where(_ => _.FileType == DocumentationType.AdditionalFiles);
            }
        }

        /// <summary>
        /// Gets list of files.
        /// </summary>
        public IList<RequestDocumentationFileViewModel> Files { get; private set; }
    }
}
