// -----------------------------------------------------------------------
// <copyright file="VoteStatus.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Statuses for votes.
    /// </summary>
    public enum VoteStatus
    {
        /// <summary>
        /// No vote placed.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Accept proposed study
        /// </summary>
        AcceptStudy = 1,

        /// <summary>
        /// Reject the proposed study.
        /// </summary>
        RejectStudy = 2,

        /// <summary>
        /// Abstain from voting.
        /// </summary>
        Abstain = 3,
    }
}
