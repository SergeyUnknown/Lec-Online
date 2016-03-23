// -----------------------------------------------------------------------
// <copyright file="ChangesLogViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Security
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// View model for the change log entry.
    /// </summary>
    public class ChangesLogViewModel
    {
        /// <summary>
        /// Gets or sets id of the log entry.
        /// </summary>
        [Display(Name = "FieldId", ResourceType = typeof(Resources))]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets type of the object which was changed.
        /// </summary>
        [Display(Name = "FieldObjectType", ResourceType = typeof(Resources))]
        public int ObjectType { get; set; }

        /// <summary>
        /// Gets or sets id of the changed object.
        /// </summary>
        [Display(Name = "FieldObjectId", ResourceType = typeof(Resources))]
        public int ObjectId { get; set; }

        /// <summary>
        /// Gets or sets date when object was changed.
        /// </summary>
        [Display(Name = "FieldChangedDate", ResourceType = typeof(Resources))]
        public DateTime Changed { get; set; }

        /// <summary>
        /// Gets or sets user who change the object.
        /// </summary>
        [Display(Name = "FieldChangedBy", ResourceType = typeof(Resources))]
        public string ChangedBy { get; set; }

        /// <summary>
        /// Gets or sets id of the client to which this change belongs.
        /// </summary>
        [Display(Name = "FieldClient", ResourceType = typeof(Resources))]
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets id of the committee to which this object belongs.
        /// </summary>
        [Display(Name = "FieldCommittee", ResourceType = typeof(Resources))]
        public int? CommetteeId { get; set; }

        /// <summary>
        /// Gets or sets text description of the change.
        /// </summary>
        [Display(Name = "FieldChangeDescription", ResourceType = typeof(Resources))]
        public string ChangeDescription { get; set; }
    }
}
