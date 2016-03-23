// -----------------------------------------------------------------------
// <copyright file="RequestDocumentationFileViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Core;
    using LecOnline.Properties;

    /// <summary>
    /// View model for the file.
    /// </summary>
    public class RequestDocumentationFileViewModel
    {
        /// <summary>
        /// Gets or sets id of the file.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of the request to which file belongs.
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// Gets or sets type of the documentation.
        /// </summary>
        public DocumentationType FileType { get; set; }

        /// <summary>
        /// Gets or sets name of the file.
        /// </summary>
        [Display(Name = "FieldFileName", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets date when file was created.
        /// </summary>
        [Display(Name = "FieldCreatedDate", ResourceType = typeof(Resources))]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets name of the user who creates the file.
        /// </summary>
        [Display(Name = "FieldCreatedBy", ResourceType = typeof(Resources))]
        public string CreatedBy { get; set; }
    }
}
