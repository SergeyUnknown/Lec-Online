// -----------------------------------------------------------------------
// <copyright file="RequestManager.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// Manage the requests.
    /// </summary>
    public class RequestManager : IDisposable
    {
        /// <summary>
        /// A value indicating that object is disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Requests persistence store.
        /// </summary>
        private IRequestStore store;

        /// <summary>
        /// Change manager user to record changes.
        /// </summary>
        private ChangeManager changeManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestManager"/> class using specified database context.
        /// </summary>
        /// <param name="store">Requests persistence store to use when operate with requests.</param>
        /// <param name="changeManager">Change manager which will used to record changes.</param>
        public RequestManager(IRequestStore store, ChangeManager changeManager)
        {
            this.store = store;
            this.changeManager = changeManager;
        }

        /// <summary>
        /// Notifies that meeting is setup.
        /// </summary>
        public event Action<Meeting> MeetingSetup;

        /// <summary>
        /// Notifies that request is submitted.
        /// </summary>
        public event Func<Request, bool> RequestSubmitting;

        /// <summary>
        /// Notifies that request is submitted.
        /// </summary>
        public event Action<Request> RequestSubmitted;

        /// <summary>
        /// Notifies that request is accepted.
        /// </summary>
        public event Action<Request> RequestAccepted;

        /// <summary>
        /// Notifies that request is rejected.
        /// </summary>
        public event Action<Request> RequestRejected;

        /// <summary>
        /// Notifies that voting started.
        /// </summary>
        public event Action<Request> VotingStarted;

        /// <summary>
        /// Notifies that vote made.
        /// </summary>
        public event Action<Request, ClaimsPrincipal, VoteStatus> VoteMade;

        /// <summary>
        /// Notifies that resolution for request is made.
        /// </summary>
        public event Action<Request, bool> RequestResolutionMade;

        /// <summary>
        /// Test that given request object is accessible for specific principal.
        /// </summary>
        /// <param name="request">Request which is trying to access principal.</param>
        /// <param name="principal">Principal for which test request accessibility.</param>
        /// <returns>True if request is visible for the user.</returns>
        public static bool CouldViewRequest(Request request, ClaimsPrincipal principal)
        {
            return FilterRequests(new[] { request }.AsQueryable(), principal).FirstOrDefault() != null;
        }

        /// <summary>
        /// Returns a value indicating whether principal could create request.
        /// </summary>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could create requests; false otherwise.</returns>
        public static bool CouldCreateRequest(ClaimsPrincipal principal)
        {
            var couldCreateRequests = principal.IsInRole(RoleNames.MedicalCenter)
                || principal.IsInRole(RoleNames.Manager);
            return couldCreateRequests;
        }

        /// <summary>
        /// Returns a value indicating whether principal could create request.
        /// </summary>
        /// <param name="request">Request based on which secondary request could be created.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could create requests; false otherwise.</returns>
        public static bool CouldCreateSecondaryRequest(Request request, ClaimsPrincipal principal)
        {
            var couldCreateRequests = CouldCreateRequest(principal);
            var couldCreateSecondaryRequest = request != null
                && (RequestStatus)request.Status == RequestStatus.Resolved;
            return couldCreateRequests && couldCreateSecondaryRequest;
        }

        /// <summary>
        /// Returns a value indicating whether principal could create request.
        /// </summary>
        /// <param name="request">Request based on which secondary request could be created.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could create requests; false otherwise.</returns>
        public static bool CouldCreateNotificationRequest(Request request, ClaimsPrincipal principal)
        {
            var couldCreateRequests = CouldCreateRequest(principal);
            var couldCreateSecondaryRequest = request != null
                && (RequestStatus)request.Status == RequestStatus.Resolved;
            return couldCreateRequests && couldCreateSecondaryRequest;
        }

        /// <summary>
        /// Returns a value indicating whether principal could edit given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could edit given request; false otherwise.</returns>
        public static bool CouldEditRequest(Request request, ClaimsPrincipal principal)
        {
            var status = (RequestStatus)request.Status;
            var statusAllowedEditing = status == RequestStatus.Created
                || status == RequestStatus.InvalidSubmission
                || status == RequestStatus.Submitted
                || status == RequestStatus.NeedMoreInformation;
            if (principal.IsInRole(RoleNames.Administrator)
                || principal.IsInRole(RoleNames.Manager)
                || principal.IsInRole(RoleNames.MedicalCenter))
            {
                var couldManagerOtherClients = principal.IsInRole(RoleNames.Administrator);
                int? clientId = request.ClientId;
                int? editorClientId = principal.GetClient();
                if (clientId != editorClientId && !couldManagerOtherClients)
                {
                    return false;
                }

                var couldEditAcceptedRequests = principal.IsInRole(RoleNames.Administrator);
                return statusAllowedEditing || couldEditAcceptedRequests;
            }

            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var isSecretaryOrChairman = principal.GetCommitteeSecretary() != null
                    || principal.GetCommitteeChairman() != null;
                return statusAllowedEditing || isSecretaryOrChairman;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could delete given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could delete given request; false otherwise.</returns>
        public static bool CouldDeleteRequest(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.Administrator)
                || principal.IsInRole(RoleNames.Manager)
                || principal.IsInRole(RoleNames.MedicalCenter))
            {
                var couldManagerOtherClients = principal.IsInRole(RoleNames.Administrator);
                int? clientId = request.ClientId;
                int? editorClientId = principal.GetClient();
                if (clientId != editorClientId && !couldManagerOtherClients)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedDeleting = status == RequestStatus.Created
                    || status == RequestStatus.Submitted;
                var couldDeleteAcceptedRequests = principal.IsInRole(RoleNames.Administrator);
                return (statusAllowedDeleting && !principal.IsInRole(RoleNames.MedicalCenter))
                    || couldDeleteAcceptedRequests;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could submit given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could submit given request; false otherwise.</returns>
        public static bool CouldSubmitRequest(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.Manager))
            {
                int? clientId = request.ClientId;
                int? editorClientId = principal.GetClient();
                if (clientId != editorClientId)
                {
                    return false;
                }

                if (request.CommitteeId != null)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedSubmit = status == RequestStatus.Created;
                return statusAllowedSubmit;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could revoke given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could revoke given request; false otherwise.</returns>
        public static bool CouldRevokeRequest(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.Manager))
            {
                int? clientId = request.ClientId;
                int? editorClientId = principal.GetClient();
                if (clientId != editorClientId)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedRevoke = status == RequestStatus.Submitted;
                return statusAllowedRevoke;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could accept or reject given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could accept or reject given request; false otherwise.</returns>
        public static bool CouldAcceptOrRejectRequest(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var isSecretaryOrChairman = principal.GetCommitteeSecretary() != null
                    || principal.GetCommitteeChairman() != null;
                var statusAllowedAcceptOrReject = status == RequestStatus.Submitted;
                return isSecretaryOrChairman && statusAllowedAcceptOrReject;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could view meeting attendees.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could accept or reject given request; false otherwise.</returns>
        public static bool CouldViewMeetingAttendances(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedAcceptOrReject = status == RequestStatus.MeetingSet
                    || status == RequestStatus.Processing
                    || status == RequestStatus.MeetingFinished;
                return statusAllowedAcceptOrReject;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could view meeting attendees.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="meeting">Meeting for which test permission.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could accept or reject given request; false otherwise.</returns>
        public static bool CouldViewMeetingRoom(Request request, Meeting meeting, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                if (meeting == null || (meeting.Status != (int)MeetingStatus.Started
                    && meeting.Status != (int)MeetingStatus.Voting))
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedAcceptOrReject = status == RequestStatus.Processing
                    || status == RequestStatus.MeetingFinished;
                var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
                var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
                if (currentAttendee == null || currentAttendee.Status != (byte)AttendanceStatus.InvitationAccepted)
                {
                    return false;
                }

                return statusAllowedAcceptOrReject;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could view meeting attendees.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="meeting">Meeting for which test permission.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could accept or reject given request; false otherwise.</returns>
        public static bool CouldSendChatInMeetingRoom(Request request, Meeting meeting, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                if (meeting == null || meeting.Status != (int)MeetingStatus.Started)
                {
                    return false;
                }

                var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
                var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
                if (currentAttendee == null || currentAttendee.Status != (byte)AttendanceStatus.InvitationAccepted)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedAcceptOrReject = status == RequestStatus.Processing
                    || status == RequestStatus.MeetingFinished;
                return statusAllowedAcceptOrReject;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could view meeting attendees.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="meeting">Meeting for which test permission.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could accept or reject given request; false otherwise.</returns>
        public static bool CouldStartVoting(Request request, Meeting meeting, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                if (meeting == null || meeting.Status != (int)MeetingStatus.Started)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var isSecretaryOrChairman = principal.GetCommitteeSecretary() != null
                    || principal.GetCommitteeChairman() != null;
                var statusStartVoting = status == RequestStatus.Processing
                    && meeting.Status == (int)MeetingStatus.Started;
                return isSecretaryOrChairman && statusStartVoting;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could set meeting for given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could set meeting for given request; false otherwise.</returns>
        public static bool CouldSetMeetingRequest(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedSetMeeting = status == RequestStatus.Accepted;
                var isSecretary = principal.GetCommitteeSecretary() != null;
                return isSecretary && statusAllowedSetMeeting;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could start meeting for given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could starts meeting for given request; false otherwise.</returns>
        public static bool CouldStartMeetingRequest(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedStartMeeting = status == RequestStatus.MeetingSet;
                var isSecretary = principal.GetCommitteeSecretary() != null;
                var isChairman = principal.GetCommitteeChairman() != null;
                return (isSecretary || isChairman) && statusAllowedStartMeeting;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could stop meeting for given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could stop meeting for given request; false otherwise.</returns>
        public static bool CouldStopMeetingRequest(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Started);
                if (meeting == null)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedStopMeeting = status == RequestStatus.Processing;
                var isSecretary = principal.GetCommitteeSecretary() != null;
                var isChairman = principal.GetCommitteeChairman() != null;
                return (isSecretary || isChairman) && statusAllowedStopMeeting;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could enter meeting resolution for given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could enter meeting resolution for given request; false otherwise.</returns>
        public static bool CouldEnterMeetingResolution(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Completed);
                if (meeting == null)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedStopMeeting = status == RequestStatus.MeetingFinished;
                //// var isChairman = principal.GetCommitteeChairman() != null; Принимает решение председатель
                var isSecretary = principal.GetCommitteeSecretary() != null;
                return isSecretary && statusAllowedStopMeeting;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could finish meeting for given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could stop meeting for given request; false otherwise.</returns>
        public static bool CouldFinishMeetingRequest(Request request, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Voting);
                if (meeting == null)
                {
                    return false;
                }

                var status = (RequestStatus)request.Status;
                var statusAllowedStopMeeting = status == RequestStatus.Processing
                    && meeting.Status == (int)MeetingStatus.Voting;
                ////var isChairman = principal.GetCommitteeChairman() != null; Принимает решение председатель
                var isSecretary = principal.GetCommitteeSecretary() != null;
                return isSecretary && statusAllowedStopMeeting;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could accept or reject given meeting.
        /// </summary>
        /// <param name="meeting">Meeting for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could accept or reject given meeting; false otherwise.</returns>
        public static bool CouldAcceptOrRejectMeeting(Meeting meeting, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = meeting.Request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var status = (RequestStatus)meeting.Request.Status;
                if (status != RequestStatus.MeetingSet)
                {
                    return false;
                }

                if ((MeetingStatus)meeting.Status != MeetingStatus.Pending)
                {
                    return false;
                }

                var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
                var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
                if (currentAttendee == null)
                {
                    return false;
                }

                if (currentAttendee.Status != (byte)AttendanceStatus.Pending)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could accept or reject given meeting.
        /// </summary>
        /// <param name="meeting">Meeting for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could accept or reject given meeting; false otherwise.</returns>
        public static bool CouldVote(Meeting meeting, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                int? committeeId = meeting.Request.CommitteeId;
                int? editorCommitteeId = principal.GetCommittee();
                if (committeeId != editorCommitteeId)
                {
                    return false;
                }

                var status = (RequestStatus)meeting.Request.Status;
                if (status != RequestStatus.Processing)
                {
                    return false;
                }

                if ((MeetingStatus)meeting.Status != MeetingStatus.Voting)
                {
                    return false;
                }

                var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
                var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
                if (currentAttendee == null || currentAttendee.Status != (byte)AttendanceStatus.InvitationAccepted)
                {
                    return false;
                }

                if (currentAttendee.Vote != null)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether principal could start meeting for given request.
        /// </summary>
        /// <param name="request">Request for which test ability to perform operation.</param>
        /// <param name="principal">Principal for which test permissions.</param>
        /// <returns>True if principal could starts meeting for given request; false otherwise.</returns>
        public static bool CouldViewRequestsHistory(Request request, ClaimsPrincipal principal)
        {
            var couldViewRequest = CouldViewRequest(request, principal);
            return couldViewRequest && request.SubsequentRequests.Count > 0;
        }

        /// <summary>
        /// Create request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be created.</param>
        /// <returns>Task which creates the request.</returns>
        public async Task CreatePrimaryRequestAsync(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldCreateRequest(principal))
            {
                throw new InvalidOperationException("Principal could not create requests");
            }

            request.ClientId = principal.GetClient().Value;
            request.RequestType = (byte)RequestType.PrimaryRequestType;
            request.BaseRequestId = null;
            request.Created = DateTime.UtcNow;
            request.CreatedBy = principal.Identity.Name;
            request.Status = (int)RequestStatus.Created;
            var changes = this.store.GetChanged(request);
            await this.store.CreateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.RequestCreated);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            await this.changeManager.RegisterRequestCreatedAsync(request, changes, principal).ConfigureAwait(false);
        }

        /// <summary>
        /// Create request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be created.</param>
        /// <param name="baseRequest">Base request for which notification is created.</param>
        /// <returns>Task which creates the request.</returns>
        public async Task CreateNotificationRequestAsync(ClaimsPrincipal principal, Request request, Request baseRequest)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldCreateRequest(principal))
            {
                throw new InvalidOperationException("Principal could not create requests");
            }

            request.ClientId = principal.GetClient().Value;
            request.RequestType = (byte)RequestType.NotificationRequest;
            request.BaseRequestId = baseRequest.Id;
            request.Created = DateTime.UtcNow;
            request.CreatedBy = principal.Identity.Name;
            request.Status = (int)RequestStatus.Created;
            var changes = this.store.GetChanged(request);
            await this.store.CreateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.RequestCreated);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            await this.changeManager.RegisterRequestCreatedAsync(request, changes, principal).ConfigureAwait(false);
        }

        /// <summary>
        /// Create request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be created.</param>
        /// <param name="baseRequest">Base request for which notification is created.</param>
        /// <returns>Task which creates the request.</returns>
        public async Task CreateSecondaryRequestAsync(ClaimsPrincipal principal, Request request, Request baseRequest)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldCreateRequest(principal))
            {
                throw new InvalidOperationException("Principal could not create requests");
            }

            request.ClientId = principal.GetClient().Value;
            request.RequestType = (byte)RequestType.SecondaryRequest;
            request.BaseRequestId = baseRequest.Id;
            request.Created = DateTime.UtcNow;
            request.CreatedBy = principal.Identity.Name;
            request.Status = (int)RequestStatus.Created;
            var changes = this.store.GetChanged(request);
            await this.store.CreateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.RequestCreated);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            await this.changeManager.RegisterRequestCreatedAsync(request, changes, principal).ConfigureAwait(false);
        }

        /// <summary>
        /// Edit request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be changed.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task UpdateAsync(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldEditRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not edit given requests");
            }

            var changes = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.RequestInformationUpdate);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            if (changes.Count > 0)
            {
                await this.changeManager.RegisterRequestChangedAsync(request, changes, principal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Edit request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be changed.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task DeleteAsync(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldEditRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not edit given requests");
            }

            var changes = this.store.GetChanged(request);
            await this.store.DeleteAsync(request).ConfigureAwait(false);
            await this.changeManager.RegisterRequestDeletedAsync(request, changes, principal).ConfigureAwait(false);
        }

        /// <summary>
        /// Finds request with given id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="requestId">Id of the request to retrieve.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task<Request> FindByIdAsync(ClaimsPrincipal principal, int requestId)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var request = await this.store.FindById(requestId);

            if (!CouldViewRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not edit given requests");
            }

            return request;
        }

        /// <summary>
        /// Finds request documentation with given id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="id">Id of the request documentation to retrieve.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task<RequestDocumentation> FindDocumentationByIdAsync(ClaimsPrincipal principal, int id)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var requestDocumentation = await this.store.FindDocumentationByIdAsync(id);
            if (!CouldViewRequest(requestDocumentation.Request, principal))
            {
                throw new InvalidOperationException("Principal could not edit given requests");
            }

            return requestDocumentation;
        }

        /// <summary>
        /// Gets stream which will contain archive for all submission
        /// </summary>
        /// <param name="principal">Principal which should get request submission.</param>
        /// <param name="request">Request for which submission documentation would be created.</param>
        /// <returns>Stream with zip archive for documentation.</returns>
        public Stream GetSubmissionArchive(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldViewRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not view given requests");
            }

            var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                foreach (var doc in request.RequestDocumentations)
                {
                    var entry = archive.CreateEntry(doc.Name, CompressionLevel.Optimal);
                    var entryStream = entry.Open();
                    using (var streamWriter = new BinaryWriter(entryStream))
                    {
                        streamWriter.Write(doc.Content);
                    }
                }
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// Verifies that there is sufficient documentation provided for the request.
        /// </summary>
        /// <param name="request">Request for which test that it has all needed documentation.</param>
        /// <returns>True if request has all necessary documentation for the submission.</returns>
        public bool IsDocumentationSufficient(Request request)
        {
            DocumentationType[] requiredTypes;
            if (request.RequestType == (int)RequestType.PrimaryRequestType)
            {
                requiredTypes = new[] 
                {
                    DocumentationType.StudyProtocol,
                    DocumentationType.ResearchBrochure,
                    DocumentationType.InformedAgreementForm,
                    DocumentationType.ExcerptFromEthicsProtocol,
                    DocumentationType.AllowanceFromMinistryOfHealth
                };
                var fileTypes = (from doc in request.RequestDocumentations
                                 join type in requiredTypes on doc.FileType equals (byte)type
                                 select doc.FileType).Distinct();
                return fileTypes.Count() >= requiredTypes.Length;
            }

            return true;
        }

        /// <summary>
        /// Submits request with given id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="requestId">Id of the request to retrieve.</param>
        /// <param name="committeeId">Id of the committee to which submit request.</param>
        /// <param name="comments">Additional comment for the committee.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task SubmitAsync(ClaimsPrincipal principal, int requestId, int committeeId, string comments)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var request = await this.store.FindById(requestId);
            if (!CouldSubmitRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not submit given request");
            }

            request.CommitteeId = committeeId;
            request.SentDate = DateTime.UtcNow;
            request.SubmittedBy = principal.Identity.Name;
            request.Status = (int)RequestStatus.Submitted;
            request.SubmissionComments = comments;
            request.LastComments = comments;
            var preHandler = this.RequestSubmitting;
            if (preHandler != null)
            {
                var shouldCancel = preHandler(request);
                if (shouldCancel)
                {
                    return;
                }
            }

            var changes = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.RequestSubmitted);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            if (changes.Count > 0)
            {
                await this.changeManager.RegisterRequestSubmittedAsync(request, changes, principal).ConfigureAwait(false);
            }

            var handler = this.RequestSubmitted;
            if (handler != null)
            {
                handler(request);
            }
        }

        /// <summary>
        /// Revoke previously submitted request with given id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="requestId">Id of the request to retrieve.</param>
        /// <param name="comments">Additional comment for the committee.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task RevokeAsync(ClaimsPrincipal principal, int requestId, string comments)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var request = await this.store.FindById(requestId);
            if (!CouldRevokeRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not revoke given request");
            }

            request.CommitteeId = null;
            request.SentDate = null;
            request.Status = (int)RequestStatus.Created;
            request.RevokeComments = comments;
            request.LastComments = comments;
            var changes = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            if (changes.Count > 0)
            {
                await this.changeManager.RegisterRequestRevokedAsync(request, changes, principal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Accept submitted application with given id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which want to accept application.</param>
        /// <param name="requestId">Id of the request to accept.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task AcceptAsync(ClaimsPrincipal principal, int requestId)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var request = await this.store.FindById(requestId);
            if (!CouldAcceptOrRejectRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not accept given request");
            }

            request.Accepted = DateTime.UtcNow;
            request.AcceptedBy = principal.Identity.Name;
            request.Status = (int)RequestStatus.Accepted;
            var changes = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.RequestReviewAccepted);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            if (changes.Count > 0)
            {
                await this.changeManager.RegisterRequestAcceptedAsync(request, changes, principal).ConfigureAwait(false);
            }

            var handler = this.RequestAccepted;
            if (handler != null)
            {
                handler(request);
            }
        }

        /// <summary>
        /// Reject submitted application with given id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which want to accept application.</param>
        /// <param name="requestId">Id of the request to accept.</param>
        /// <param name="rejectionReason">Comments about rejection reason.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task RejectAsync(ClaimsPrincipal principal, int requestId, string rejectionReason)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var request = await this.store.FindById(requestId);
            if (!CouldAcceptOrRejectRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not reject given request");
            }

            request.RejectionComments = rejectionReason;
            request.LastComments = rejectionReason;
            request.Status = (int)RequestStatus.InvalidSubmission;
            var changes = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.RequestReviewRejected);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            if (changes.Count > 0)
            {
                await this.changeManager.RegisterRequestRejectedAsync(request, changes, principal).ConfigureAwait(false);
            }

            var handler = this.RequestRejected;
            if (handler != null)
            {
                handler(request);
            }
        }

        /// <summary>
        /// Asynchronously upload documentation to the server.
        /// </summary>
        /// <param name="principal">Principal which want to upload file to application.</param>
        /// <param name="requestId">Id of the request to which upload file.</param>
        /// <param name="fileType">Type of documentation.</param>
        /// <param name="filename">Name of the uploaded file.</param>
        /// <param name="contentType">Type of the uploading content.</param>
        /// <param name="dataStream">Stream with data.</param>
        /// <returns>A task which asynchronously uploads documentation.</returns>
        public async Task UploadDocumentationAsync(ClaimsPrincipal principal, int requestId, DocumentationType fileType, string filename, string contentType, System.IO.Stream dataStream)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            var request = await this.store.FindById(requestId);
            if (!CouldEditRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not reject given request");
            }

            var documentation = new RequestDocumentation();
            documentation.RequestId = requestId;
            documentation.Name = filename;
            var memoryStream = new MemoryStream();
            await dataStream.CopyToAsync(memoryStream);
            documentation.Content = memoryStream.ToArray();
            documentation.Created = DateTime.UtcNow;
            documentation.FileType = (byte)fileType;
            documentation.CreatedBy = principal.Identity.Name;
            documentation.Signed = DateTime.UtcNow;
            documentation.SignedBy = principal.Identity.Name;
            var changes = this.store.GetChanged(documentation);
            await this.store.CreateAsync(documentation).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.FileUploaded, filename);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            await this.changeManager.RegisterRequestDocumentationCreatedAsync(request, documentation, changes, principal).ConfigureAwait(false);
        }

        /// <summary>
        /// Edit request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="documentation">Data for the request documentation object to be deleted.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task DeleteAsync(ClaimsPrincipal principal, RequestDocumentation documentation)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (documentation == null)
            {
                throw new ArgumentNullException("documentation");
            }

            if (!CouldEditRequest(documentation.Request, principal))
            {
                throw new InvalidOperationException("Principal could not edit given requests");
            }

            var changes = this.store.GetChanged(documentation);
            var request = await this.store.FindById(documentation.RequestId);
            await this.store.DeleteAsync(documentation).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.FileDeleted, documentation.Name);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            await this.changeManager.RegisterRequestDocumentationDeletedAsync(request, documentation, changes, principal).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets requests which available to selected principal.
        /// </summary>
        /// <param name="principal">Principal for which return available requests.</param>
        /// <returns>Queryable sequence of requests available for given principal.</returns>
        public IQueryable<Request> GetRequests(ClaimsPrincipal principal)
        {
            return FilterRequests(this.store.Requests, principal);
        }

        /// <summary>
        /// Gets requests which are waiting when meeting is started.
        /// </summary>
        /// <param name="principal">Principal for which return available requests.</param>
        /// <returns>Queryable sequence of requests for which meeting date is set.</returns>
        public IQueryable<Request> GetPendingRequests(ClaimsPrincipal principal)
        {
            return this.GetRequests(principal).Where(_ => _.Status == (int)RequestStatus.MeetingSet);
        }

        /// <summary>
        /// Starts meeting if possible.
        /// </summary>
        /// <param name="request">Request for which meeting should be started.</param>
        /// <returns>Asynchronous task which start meeting of possible.</returns>
        public async Task StartMeetingIfPossible(Request request)
        {
            if (request.Status == (int)RequestStatus.MeetingSet)
            {
                var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Pending);
                if (meeting != null && meeting.MeetingDate < DateTime.UtcNow)
                {
                    request.Status = (int)RequestStatus.Processing;
                    await this.store.UpdateAsync(request).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Asynchronously setup meeting.
        /// </summary>
        /// <param name="principal">Principal which try create meeting.</param>
        /// <param name="request">Request for which meeting attempt to be set.</param>
        /// <param name="meetingDate">Date when meeting should be set.</param>
        /// <param name="members">Sequence of user ids which would be on the meeting.</param>
        /// <returns>Task which creates the meeting.</returns>
        public async Task<Meeting> SetupMeetingAsync(ClaimsPrincipal principal, Request request, DateTime meetingDate, IEnumerable<string> members)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldSetMeetingRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not setup meeting for given requests");
            }

            var meeting = new Meeting();
            meeting.Request = request;
            meeting.MeetingDate = meetingDate;
            meeting.Status = (int)MeetingStatus.Pending;
            foreach (var userId in members)
            {
                var attendee = new MeetingAttendee();
                attendee.Meeting = meeting;
                attendee.UserId = userId;
                attendee.Status = (byte)AttendanceStatus.Pending;
                meeting.MeetingAttendees.Add(attendee);
            }

            await this.store.CreateAsync(meeting).ConfigureAwait(false);
            request.Status = (int)RequestStatus.MeetingSet;
            var requestChanges = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.MeetingSet);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            if (requestChanges.Count > 0)
            {
                await this.changeManager.RegisterRequestChangedAsync(request, requestChanges, principal).ConfigureAwait(false);
            }

            var handler = this.MeetingSetup;
            if (handler != null)
            {
                handler(meeting);
            }

            return meeting;
        }

        /// <summary>
        /// Start meeting for a request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be changed.</param>
        /// <returns>Task which starts meeting for a request.</returns>
        public async Task StartMeetingAsync(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldStartMeetingRequest(request, principal))
            {
                throw new InvalidOperationException(string.Format(
                    "Principal {0} could not start meeting for given request with id: {1}",
                    principal.Identity.Name,
                    request.Id));
            }

            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Pending);
            if (meeting == null)
            {
                throw new InvalidOperationException(string.Format(
                    "Request {1} does not have any meeting setup.", 
                    request.Id));
            }
            
            request.Status = (int)RequestStatus.Processing;
            meeting.Status = (int)MeetingStatus.Started;
            var requestChanges = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.MeetingStarted);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            await this.store.UpdateAsync(meeting).ConfigureAwait(false);
            if (requestChanges.Count > 0)
            {
                await this.changeManager.RegisterRequestChangedAsync(request, requestChanges, principal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Stop meeting for a request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be changed.</param>
        /// <returns>Task which stops meeting for a request.</returns>
        public async Task StopMeetingAsync(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldStopMeetingRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not stop meeting for given requests");
            }

            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Started
                || _.Status == (int)MeetingStatus.Voting);
            if (meeting == null)
            {
                throw new InvalidOperationException("Request does not have any meeting setup.");
            }

            request.Status = (int)RequestStatus.MeetingFinished;
            meeting.Status = (int)MeetingStatus.Cancelled;
            var requestChanges = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            await this.store.UpdateAsync(meeting).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.MeetingFinished);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            if (requestChanges.Count > 0)
            {
                await this.changeManager.RegisterRequestChangedAsync(request, requestChanges, principal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Perform meeting resolution for a request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be changed.</param>
        /// <returns>Task which stops meeting for a request.</returns>
        public async Task FinalizeMeetingAsync(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldEnterMeetingResolution(request, principal))
            {
                throw new InvalidOperationException("Principal could not enter meeting resolution for given requests");
            }

            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Completed);
            if (meeting == null)
            {
                throw new InvalidOperationException("Request does not have any meeting setup.");
            }

            request.Status = (int)RequestStatus.Resolved;
            meeting.Status = (int)MeetingStatus.HasResolution;
            var requestChanges = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            await this.store.UpdateAsync(meeting).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, meeting.Resolution == (byte)MeetingResolution.StudyAccepted ? RequestActionType.StudyAccepted : RequestActionType.StudyRejected);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            if (requestChanges.Count > 0)
            {
                await this.changeManager.RegisterRequestChangedAsync(request, requestChanges, principal).ConfigureAwait(false);
            }

            var handler = this.RequestResolutionMade;
            if (handler != null)
            {
                handler(request, meeting.Resolution == (int)MeetingResolution.StudyAccepted);
            }
        }

        /// <summary>
        /// Finish meeting for a request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be changed.</param>
        /// <returns>Task which stops meeting for a request.</returns>
        public async Task FinishMeetingAsync(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (!CouldFinishMeetingRequest(request, principal))
            {
                throw new InvalidOperationException("Principal could not finish meeting for given requests");
            }

            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Started
                || _.Status == (int)MeetingStatus.Voting);
            if (meeting == null)
            {
                throw new InvalidOperationException("Request does not have any meeting setup.");
            }

            request.Status = (int)RequestStatus.MeetingFinished;
            meeting.Status = (int)MeetingStatus.Completed;
            var requestChanges = this.store.GetChanged(request);
            await this.store.UpdateAsync(request).ConfigureAwait(false);
            await this.store.UpdateAsync(meeting).ConfigureAwait(false);
            var requestAction = CreateRequestAction(principal, request, RequestActionType.MeetingFinished);
            await this.store.CreateAsync(requestAction).ConfigureAwait(false);
            if (requestChanges.Count > 0)
            {
                await this.changeManager.RegisterRequestChangedAsync(request, requestChanges, principal).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Accept proposed meeting with given id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which want to accept application.</param>
        /// <param name="meeting">Meeting to accept.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task AcceptMeetingAsync(ClaimsPrincipal principal, Meeting meeting)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (meeting == null)
            {
                throw new ArgumentNullException("meeting");
            }

            if (!CouldAcceptOrRejectMeeting(meeting, principal))
            {
                throw new InvalidOperationException(string.Format("Principal {0} could not accept given meeting with id: {1}", principal.Identity.Name, meeting.Id));
            }

            var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
            var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
            currentAttendee.Status = (int)AttendanceStatus.InvitationAccepted;
            await this.store.UpdateAsync(currentAttendee).ConfigureAwait(false);
        }

        /// <summary>
        /// Decline proposed meeting with given id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which want to decline meeting.</param>
        /// <param name="meeting">Meeting to decline.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task DeclineMeetingAsync(ClaimsPrincipal principal, Meeting meeting)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (meeting == null)
            {
                throw new ArgumentNullException("meeting");
            }

            if (!CouldAcceptOrRejectMeeting(meeting, principal))
            {
                throw new InvalidOperationException(string.Format("Principal {0} could not decline given meeting with id: {1}", principal.Identity.Name, meeting.Id));
            }

            var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
            var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
            currentAttendee.Status = (int)AttendanceStatus.InvitationDeclined;
            await this.store.UpdateAsync(currentAttendee).ConfigureAwait(false);
        }

        /// <summary>
        /// Stop meeting for a request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be changed.</param>
        /// <returns>Task which stops meeting for a request.</returns>
        public async Task StartVotingAsync(ClaimsPrincipal principal, Request request)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Started);
            if (meeting == null)
            {
                throw new InvalidOperationException("Request does not have any meeting setup.");
            }

            if (!CouldStartVoting(request, meeting, principal))
            {
                throw new InvalidOperationException("Principal could not stop meeting for given requests");
            }

            meeting.Status = (int)MeetingStatus.Voting;
            await this.store.UpdateAsync(meeting).ConfigureAwait(false);
            await this.changeManager.RegisterVotingStartedAsync(meeting, principal).ConfigureAwait(false);

            var handler = this.VotingStarted;
            if (handler != null)
            {
                handler(request);
            }
        }

        /// <summary>
        /// Accept proposed study with given request id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which want to accept application.</param>
        /// <param name="meeting">Meeting where to put accept vote.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task VoteAcceptAsync(ClaimsPrincipal principal, Meeting meeting)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (meeting == null)
            {
                throw new ArgumentNullException("meeting");
            }

            if (!CouldVote(meeting, principal))
            {
                throw new InvalidOperationException("Principal could not vote in meeting");
            }

            var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
            var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
            currentAttendee.Vote = (int)VoteStatus.AcceptStudy;
            await this.store.UpdateAsync(currentAttendee).ConfigureAwait(false);

            var handler = this.VoteMade;
            if (handler != null)
            {
                handler(meeting.Request, principal, VoteStatus.AcceptStudy);
            }
        }

        /// <summary>
        /// Abstain proposed study with given request id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which want to accept application.</param>
        /// <param name="meeting">Meeting where to put accept vote.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task VoteAbstainAsync(ClaimsPrincipal principal, Meeting meeting)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (meeting == null)
            {
                throw new ArgumentNullException("meeting");
            }

            if (!CouldVote(meeting, principal))
            {
                throw new InvalidOperationException("Principal could not vote in meeting");
            }

            var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
            var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
            currentAttendee.Vote = (int)VoteStatus.Abstain;
            await this.store.UpdateAsync(currentAttendee).ConfigureAwait(false);

            var handler = this.VoteMade;
            if (handler != null)
            {
                handler(meeting.Request, principal, VoteStatus.AcceptStudy);
            }
        }

        /// <summary>
        /// Reject proposed study with given request id as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which want to accept application.</param>
        /// <param name="meeting">Meeting where to put accept vote.</param>
        /// <returns>Task which edits the request.</returns>
        public async Task VoteRejectAsync(ClaimsPrincipal principal, Meeting meeting)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (meeting == null)
            {
                throw new ArgumentNullException("meeting");
            }

            if (!CouldVote(meeting, principal))
            {
                throw new InvalidOperationException("Principal could not vote in meeting");
            }

            var currentUserId = principal.FindFirst(ClaimTypes.Sid).Value;
            var currentAttendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == currentUserId);
            currentAttendee.Vote = (int)VoteStatus.RejectStudy;
            await this.store.UpdateAsync(currentAttendee).ConfigureAwait(false);

            var handler = this.VoteMade;
            if (handler != null)
            {
                handler(meeting.Request, principal, VoteStatus.RejectStudy);
            }
        }

        /// <summary>
        /// Stop meeting for a request as the specified principal.
        /// </summary>
        /// <param name="principal">Principal which should create user.</param>
        /// <param name="request">Data for the requests object to be changed.</param>
        /// <param name="message">Message to send.</param>
        /// <returns>Task which stops meeting for a request.</returns>
        public async Task SendMessageAsync(ClaimsPrincipal principal, Request request, string message)
        {
            this.ThrowIfDisposed();
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Started);
            if (meeting == null)
            {
                throw new InvalidOperationException("Request does not have any meeting setup.");
            }

            if (!CouldViewMeetingRoom(request, meeting, principal))
            {
                throw new InvalidOperationException("Principal could not stop meeting for given requests");
            }

            var chatMessage = new MeetingChatMessage();
            chatMessage.Meeting = meeting;
            chatMessage.SentDate = DateTime.UtcNow;
            chatMessage.UserId = principal.FindFirst(ClaimTypes.Sid).Value;
            chatMessage.Message = message;
            await this.store.CreateAsync(chatMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets messages in the meeting room.
        /// </summary>
        /// <param name="meetingId">Id of the meeting.</param>
        /// <returns>Sequence of meeting messages.</returns>
        public IQueryable<MeetingChatMessage> GetMeetingChatMessages(int meetingId)
        {
            return this.store.MeetingChatMessages.Where(_ => meetingId == _.MeetingId);
        }
 
        /// <summary>
        /// Dispose this object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.store.Dispose();
                this.disposed = true;

                this.RequestAccepted = null;
                this.RequestRejected = null;
                this.VotingStarted = null;
                this.MeetingSetup = null;
            }
        }

        /// <summary>
        /// Filter requests based on permissions.
        /// </summary>
        /// <param name="requests">Requests to filter.</param>
        /// <param name="principal">Principal for which filter requests.</param>
        /// <returns>Filtered requests based on the permissions.</returns>
        private static IQueryable<Core.Request> FilterRequests(IQueryable<Request> requests, ClaimsPrincipal principal)
        {
            if (principal.IsInRole(RoleNames.MedicalCenter))
            {
                // Filter to only requests which are preparing,
                // or completely processed.
                var companyId = principal.GetClient();
                requests = requests.Where(_ => _.ClientId == companyId);
            }

            if (principal.IsInRole(RoleNames.Manager))
            {
                // Filter to only requests which are preparing,
                // completely processed or processing by the ethical commettee.
                var companyId = principal.GetClient();
                requests = requests.Where(_ => _.ClientId == companyId);
            }

            if (principal.IsInRole(RoleNames.EthicalCommitteeMember))
            {
                // Filter to only requests which are assigned for the current committee.
                var committeeId = principal.GetCommittee();
                requests = requests.Where(_ => _.CommitteeId == committeeId);

                var isChairman = principal.GetCommitteeChairman() == committeeId;
                var isSecretary = principal.GetCommitteeSecretary() == committeeId;

                // For regular members filter to only requests which are accepted,
                // and for which date of the meeting is set.
                if (!isSecretary && !isChairman)
                {
                    requests = requests.Where(_ => (RequestStatus)_.Status == RequestStatus.Accepted
                        || (RequestStatus)_.Status == RequestStatus.MeetingSet
                        || (RequestStatus)_.Status == RequestStatus.Processing
                        /* Possible that some members could provide input to the protocol,
                          after the fact. */
                        || (RequestStatus)_.Status == RequestStatus.MeetingFinished);
                }
                else
                {
                    // If commettee secretary, then display also requests which are 
                    // posted by the medical centers or managers.
                    requests = requests.Where(_ => (RequestStatus)_.Status != RequestStatus.Created);
                }
            }

            return requests;
        }

        /// <summary>
        /// Create the specified request action for the given request.
        /// </summary>
        /// <param name="principal">Principal which perform action.</param>
        /// <param name="request">Request for which create action.</param>
        /// <param name="type">Type of the request action.</param>
        /// <param name="parameters">Additional parameters for the event description.</param>
        /// <returns>A <see cref="RequestAction"/> for the given request.</returns>
        private static RequestAction CreateRequestAction(ClaimsPrincipal principal, Request request, RequestActionType type, params object[] parameters)
        {
            var description = RequestActions.ResourceManager.GetString(type.ToString());
            var requestAction = new RequestAction();
            requestAction.Request = request;
            requestAction.ActionType = (byte)type;
            requestAction.Description = string.Format(description, parameters);
            requestAction.ActionDate = DateTime.UtcNow;
            requestAction.UserName = principal.Identity.Name;
            return requestAction;
        }

        /// <summary>
        /// Throws exception if object is disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
    }
}
