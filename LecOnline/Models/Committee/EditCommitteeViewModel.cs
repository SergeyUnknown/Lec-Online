// -----------------------------------------------------------------------
// <copyright file="EditCommitteeViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Committee
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Model for the committee editing.
    /// </summary>
    public class EditCommitteeViewModel
    {
        /// <summary>
        /// Gets or sets id of the committee.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets committee name.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldName", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets city for the committee.
        /// </summary>
        [Display(Name = "FieldCity", ResourceType = typeof(Resources))]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets notes for the committee.
        /// </summary>
        [Display(Name = "FieldNotes", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets id of the chairman for the committee.
        /// </summary>
        [Display(Name = "FieldChairman", ResourceType = typeof(Resources))]
        [UIHint("User")]
        public string Chairman { get; set; }

        /// <summary>
        /// Gets or sets id of the secretary for the committee.
        /// </summary>
        [Display(Name = "FieldSecretary", ResourceType = typeof(Resources))]
        [UIHint("User")]
        public string Secretary { get; set; }

        /// <summary>
        /// Gets or sets id of the vice chairman for the committee.
        /// </summary>
        [Display(Name = "FieldViceChairman", ResourceType = typeof(Resources))]
        [UIHint("User")]
        public string ViceChairman { get; set; }
    }
}
