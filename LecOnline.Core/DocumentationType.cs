// -----------------------------------------------------------------------
// <copyright file="DocumentationType.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Represents the type of documentation.
    /// </summary>
    public enum DocumentationType : byte
    {
        /// <summary>
        /// Additional files.
        /// </summary>
        AdditionalFiles = 0,

        /// <summary>
        /// Allowance from Ministry of Health
        /// </summary>
        AllowanceFromMinistryOfHealth = 1,

        /// <summary>
        /// Excerpt from ethics protocol
        /// </summary>
        ExcerptFromEthicsProtocol = 2,

        /// <summary>
        /// Study protocol.
        /// </summary>
        StudyProtocol = 3,

        /// <summary>
        /// Brochure of the study.
        /// </summary>
        ResearchBrochure = 4,

        /// <summary>
        /// Informed agreement form.
        /// </summary>
        InformedAgreementForm = 5,
    }
}
