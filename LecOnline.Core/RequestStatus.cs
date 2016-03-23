// -----------------------------------------------------------------------
// <copyright file="RequestStatus.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Status of the request.
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Request is created.
        /// </summary>
        Created = 0,

        /// <summary>
        /// Requests is submitted.
        /// </summary>
        Submitted = 10,

        /// <summary>
        /// Submission rejected by committee due to insufficient information, or other reasons.
        /// </summary>
        InvalidSubmission = 20,

        /// <summary>
        /// Submission accepted for processing by committee.
        /// </summary>
        Accepted = 30,
        
        /// <summary>
        /// Meeting date set for the request processing.
        /// </summary>
        MeetingSet = 40,

        /// <summary>
        /// Meeting started and request is evaluating now.
        /// </summary>
        Processing = 50,

        /// <summary>
        /// Meeting is finished, but decision is not yet prepared.
        /// </summary>
        MeetingFinished = 60,

        /// <summary>
        /// Need more information about request.
        /// </summary>
        NeedMoreInformation = 70,

        /// <summary>
        /// Request is resolved. 
        /// For such requests meeting has resolution which define would study accepted or rejected.
        /// </summary>
        Resolved = 80,

        /// <summary>
        /// Request is rejected.
        /// </summary>
        Rejected = 90,
    }
}
