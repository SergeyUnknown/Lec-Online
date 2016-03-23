// -----------------------------------------------------------------------
// <copyright file="RequestActionType.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Type of the actions which happens with the request.
    /// </summary>
    public enum RequestActionType
    {
        /// <summary>
        /// Request created.
        /// </summary>
        RequestCreated = 0,

        /// <summary>
        /// Documentation file for the request uploaded.
        /// </summary>
        FileUploaded = 1,

        /// <summary>
        /// Documentation file for the request deleted.
        /// </summary>
        FileDeleted = 2,

        /// <summary>
        /// Information for the request was updated.
        /// </summary>
        RequestInformationUpdate = 3,

        /// <summary>
        /// Request submitted to the committee.
        /// </summary>
        RequestSubmitted = 4,

        /// <summary>
        /// Review for the request accepted by the committee.
        /// </summary>
        RequestReviewAccepted = 5,

        /// <summary>
        /// Review for the request accepted by the committee.
        /// </summary>
        RequestReviewRejected = 6,

        /// <summary>
        /// Meeting set for the request.
        /// </summary>
        MeetingSet = 7,

        /// <summary>
        /// Meeting for the request started.
        /// </summary>
        MeetingStarted = 8,

        /// <summary>
        /// Meeting for the request finished.
        /// </summary>
        MeetingFinished = 9,

        /// <summary>
        /// Study accepted by the committee.
        /// </summary>
        StudyAccepted = 10,

        /// <summary>
        /// Study rejected by the committee.
        /// </summary>
        StudyRejected = 11,
    }
}
