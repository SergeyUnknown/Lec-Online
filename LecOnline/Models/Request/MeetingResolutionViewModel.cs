// -----------------------------------------------------------------------
// <copyright file="MeetingResolutionViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Core;
    using LecOnline.Mvc;
    using LecOnline.Properties;

    /// <summary>
    /// View model for the meeting notes screen.
    /// </summary>
    public class MeetingResolutionViewModel
    {
        /// <summary>
        /// Gets or sets id of the request for which meeting notes is displayed.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets meeting resolution protocol.
        /// </summary>
        [Display(Name = "FieldProtocol", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [HelpPopup(HelpText = "HelpProtocol", ResourceType = typeof(Resources))]
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets meeting minutes.
        /// </summary>
        [Display(Name = "FieldMeetingLog", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [HelpPopup(HelpText = "HelpMeetingLog", ResourceType = typeof(Resources))]
        public string MeetingLog { get; set; }

        /// <summary>
        /// Gets or sets resolution for study.
        /// </summary>
        [Display(Name = "FieldResolution", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpResolution", ResourceType = typeof(Resources))]
        [UIHint("MeetingResolution")]
        public MeetingResolution Resolution { get; set; }
    }
}
