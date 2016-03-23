﻿// -----------------------------------------------------------------------
// <copyright file="EditUserViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.User
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Properties;

    /// <summary>
    /// Model for the user editing.
    /// </summary>
    public class EditUserViewModel
    {
        /// <summary>
        /// Gets or sets id of the user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets first name of the user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldFirstName", ResourceType = typeof(Resources))]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of the user.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldLastName", ResourceType = typeof(Resources))]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets patronymic name of the user.
        /// </summary>
        [Display(Name = "FieldPatronymicName", ResourceType = typeof(Resources))]
        public string PatronymicName { get; set; }

        /// <summary>
        /// Gets or sets name where user is located.
        /// </summary>
        [Display(Name = "FieldCity", ResourceType = typeof(Resources))]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets address of the user.
        /// </summary>
        [Display(Name = "FieldAddress", ResourceType = typeof(Resources))]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets company for the user.
        /// </summary>
        [Display(Name = "FieldCompany", ResourceType = typeof(Resources))]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets contact phone for the user.
        /// </summary>
        [Display(Name = "FieldPhone", ResourceType = typeof(Resources))]
        public string ContactPhone { get; set; }

        /// <summary>
        /// Gets or sets degree of the user.
        /// </summary>
        [Display(Name = "FieldDegree", ResourceType = typeof(Resources))]
        public string Degree { get; set; }

        /// <summary>
        /// Gets or sets roles for the user.
        /// </summary>
        [Display(Name = "FieldRoles", ResourceType = typeof(Resources))]
        public string[] Roles { get; set; }

        /// <summary>
        /// Gets or sets id of the client to which this user belongs.
        /// </summary>
        [Display(Name = "FieldClient", ResourceType = typeof(Resources))]
        [UIHint("Client")]
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets id of the committee to which this user belongs.
        /// </summary>
        [Display(Name = "FieldCommittee", ResourceType = typeof(Resources))]
        [UIHint("Committee")]
        public int? CommitteeId { get; set; }
    }
}
