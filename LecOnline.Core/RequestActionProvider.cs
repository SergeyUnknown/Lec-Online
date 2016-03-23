// -----------------------------------------------------------------------
// <copyright file="RequestActionProvider.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using LecOnline.Core.Properties;

    /// <summary>
    /// Action provider for the request.
    /// </summary>
    public class RequestActionProvider : IActionProvider
    {
        /// <summary>
        /// Checks whether entity type is supported by this provider.
        /// </summary>
        /// <param name="entityType">Type of the entity to check.</param>
        /// <returns>True if action type is supported.</returns>
        public bool IsTypeSuported(Type entityType)
        {
            return typeof(Request).IsAssignableFrom(entityType);
        }

        /// <summary>
        /// Gets actions which action provider could add for given entity.
        /// </summary>
        /// <param name="principal">Principal which attempts to retrieve list of actions.</param>
        /// <param name="item">Entity for which actions could be provided.</param>
        /// <returns>Sequence of <see cref="ActionDescription"/> objects which represents actions.</returns>
        public IEnumerable<ActionDescription> GetActions(ClaimsPrincipal principal, object item)
        {
            var targetRequest = (Request)item;
            var status = (RequestStatus)targetRequest.Status;
            var couldCreateRequests = RequestManager.CouldCreateRequest(principal);
            if (couldCreateRequests)
            {
                yield return new ActionDescription
                {
                    Id = "Common.Create",
                    CssClass = "info",
                    SmallCssClass = "blue",
                    Icon = "fa-edit",
                    Text = Resources.ActionEdit,
                    SortOrder = 10,
                    Action = "Create",
                    NotItemOperation = true,
                };
            }

            if (RequestManager.CouldEditRequest(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Common.Edit",
                    CssClass = "info",
                    SmallCssClass = "blue",
                    Icon = "fa-edit",
                    Text = Resources.ActionEdit,
                    SortOrder = 10,
                    Action = "Edit",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            var couldManagerOtherClients = principal.IsInRole(RoleNames.Administrator);
            if (principal.IsInRole(RoleNames.Administrator)
                || principal.IsInRole(RoleNames.Manager)
                || principal.IsInRole(RoleNames.MedicalCenter))
            {
                if (!couldManagerOtherClients)
                {
                    int? clientId = targetRequest.ClientId;
                    int? editorClientId = principal.GetClient();
                    if (clientId != editorClientId)
                    {
                        yield break;
                    }
                }

                if (RequestManager.CouldDeleteRequest(targetRequest, principal))
                {
                    yield return new ActionDescription
                    {
                        Id = "Common.Delete",
                        CssClass = "danger",
                        SmallCssClass = "red",
                        Icon = "fa-remove",
                        Text = Resources.ActionDelete,
                        SortOrder = 100,
                        Action = "Delete",
                        RouteParameters = new { id = targetRequest.Id },
                    };
                }
            }

            if (RequestManager.CouldSubmitRequest(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Request.Submit",
                    CssClass = "success",
                    SmallCssClass = "success",
                    Icon = "fa-share",
                    Text = Resources.ActionSubmit,
                    SortOrder = 20,
                    Action = "Submit",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            if (RequestManager.CouldRevokeRequest(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Request.Revoke",
                    CssClass = "warning",
                    SmallCssClass = "warning",
                    Icon = "fa-reply",
                    Text = Resources.ActionRevoke,
                    SortOrder = 20,
                    Action = "Revoke",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            if (RequestManager.CouldAcceptOrRejectRequest(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Request.Accept",
                    CssClass = "info",
                    SmallCssClass = "blue",
                    Icon = "fa-check-square-o",
                    Text = Resources.ActionAccept,
                    SortOrder = 20,
                    Action = "Accept",
                    RouteParameters = new { id = targetRequest.Id },
                };
                yield return new ActionDescription
                {
                    Id = "Request.Reject",
                    CssClass = "danger",
                    SmallCssClass = "red",
                    Icon = "fa-remove",
                    Text = Resources.ActionReject,
                    SortOrder = 20,
                    Action = "Reject",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            if (RequestManager.CouldSetMeetingRequest(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Request.SetMeeting",
                    CssClass = "success",
                    SmallCssClass = "success",
                    Icon = "fa-clock-o",
                    Text = Resources.ActionSetMeeting,
                    SortOrder = 20,
                    Action = "SetMeeting",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            var couldCreateSecondaryRequests = RequestManager.CouldCreateSecondaryRequest(targetRequest, principal);
            if (couldCreateSecondaryRequests)
            {
                yield return new ActionDescription
                {
                    Id = "Request.CreateSecondary",
                    CssClass = "info",
                    SmallCssClass = "blue",
                    Icon = "fa-plus",
                    Text = Resources.ActionCreateSecondary,
                    SortOrder = 10,
                    Action = "CreateSecondary",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            var couldCreateNotficationRequests = RequestManager.CouldCreateNotificationRequest(targetRequest, principal);
            if (couldCreateNotficationRequests)
            {
                yield return new ActionDescription
                {
                    Id = "Request.CreateNotification",
                    CssClass = "info",
                    SmallCssClass = "blue",
                    Icon = "fa-plus",
                    Text = Resources.ActionCreateNotification,
                    SortOrder = 10,
                    Action = "CreateNotification",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            var couldViewRequestsHistory = RequestManager.CouldViewRequestsHistory(targetRequest, principal);
            if (couldViewRequestsHistory)
            {
                yield return new ActionDescription
                {
                    Id = "Request.ViewHistory",
                    CssClass = "info",
                    SmallCssClass = "blue",
                    Icon = "fa-plus",
                    Text = Resources.ActionViewRequestHistory,
                    SortOrder = 10,
                    Action = "ViewHistory",
                    RouteParameters = new { id = targetRequest.Id },
                };
            } 
            
            if (RequestManager.CouldStartMeetingRequest(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Request.StartMeeting",
                    CssClass = "success",
                    SmallCssClass = "success",
                    Icon = "fa-gavel",
                    Text = Resources.ActionStartMeeting,
                    SortOrder = 20,
                    Action = "StartMeeting",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            if (RequestManager.CouldStopMeetingRequest(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Request.StopMeeting",
                    CssClass = "danger",
                    SmallCssClass = "red",
                    Icon = "fa-minus-circle",
                    Text = Resources.ActionStopMeeting,
                    SortOrder = 20,
                    Action = "StopMeeting",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            if (RequestManager.CouldFinishMeetingRequest(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Request.FinishMeeting",
                    CssClass = "success",
                    SmallCssClass = "success",
                    Icon = "fa-archive",
                    Text = Resources.ActionFinishMeeting,
                    SortOrder = 20,
                    Action = "FinishMeeting",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            if (RequestManager.CouldEnterMeetingResolution(targetRequest, principal))
            {
                yield return new ActionDescription
                {
                    Id = "Request.EnterMeetingResolution",
                    CssClass = "success",
                    SmallCssClass = "success",
                    Icon = "fa-paperclip",
                    Text = Resources.ActionEnterMeetingResolution,
                    SortOrder = 20,
                    Action = "EnterMeetingResolution",
                    RouteParameters = new { id = targetRequest.Id },
                };
            }

            if (targetRequest.Status == (int)RequestStatus.MeetingSet
                || targetRequest.Status == (int)RequestStatus.MeetingFinished
                || targetRequest.Status == (int)RequestStatus.Processing)
            {
                var meeting = targetRequest.Meetings.FirstOrDefault();
                if (meeting != null && RequestManager.CouldViewMeetingRoom(targetRequest, meeting, principal))
                {
                    yield return new ActionDescription
                    {
                        Id = "Request.MeetingRoom",
                        CssClass = "info",
                        SmallCssClass = "blue",
                        Icon = "fa-bell-o",
                        Text = Resources.ActionMeetingRoom,
                        SortOrder = 10,
                        Action = "MeetingRoom",
                        RouteParameters = new { id = targetRequest.Id },
                    };
                }

                if (meeting != null && RequestManager.CouldViewMeetingAttendances(targetRequest, principal))
                {
                    yield return new ActionDescription
                    {
                        Id = "Request.ViewAttendees",
                        CssClass = "info",
                        SmallCssClass = "blue",
                        Icon = "fa-users",
                        Text = Resources.ActionViewAttendees,
                        SortOrder = 11,
                        Action = "ViewAttendees",
                        RouteParameters = new { id = targetRequest.Id },
                    };
                }
            }

            if (targetRequest.Status == (int)RequestStatus.MeetingSet)
            {
                var meeting = targetRequest.Meetings.FirstOrDefault();
                if (meeting != null && RequestManager.CouldAcceptOrRejectMeeting(meeting, principal))
                {
                    yield return new ActionDescription
                    {
                        Id = "Request.AcceptMeeting",
                        CssClass = "success",
                        SmallCssClass = "success",
                        Icon = "fa-check",
                        Text = Resources.ActionAcceptMeeting,
                        SortOrder = 20,
                        Action = "AcceptMeeting",
                        RouteParameters = new { id = targetRequest.Id },
                    };
                    yield return new ActionDescription
                    {
                        Id = "Request.DeclineMeeting",
                        CssClass = "danger",
                        SmallCssClass = "red",
                        Icon = "fa-remove",
                        Text = Resources.ActionDeclineMeeting,
                        SortOrder = 20,
                        Action = "DeclineMeeting",
                        RouteParameters = new { id = targetRequest.Id },
                    };
                }
            }

            if (targetRequest.Status == (int)RequestStatus.Processing)
            {
                var meeting = targetRequest.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Started);
                if (meeting != null && RequestManager.CouldStartVoting(targetRequest, meeting, principal))
                {
                    yield return new ActionDescription
                    {
                        Id = "Request.StartVoting",
                        CssClass = "success",
                        SmallCssClass = "success",
                        Icon = "fa-check",
                        Text = Resources.ActionStartVoting,
                        SortOrder = 20,
                        Action = "StartVoting",
                        RouteParameters = new { id = targetRequest.Id },
                    };
                }
            }
        }
    }
}
