// -----------------------------------------------------------------------
// <copyright file="Chat.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Hubs
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using LecOnline.Core;
    using Microsoft.AspNet.SignalR;

    /// <summary>
    /// Chat hub.
    /// </summary>
    public class Chat : Hub
    {
        /// <summary>
        /// Notifies that voting started about specific request.
        /// </summary>
        /// <param name="requestId">Id of the request for which voting started.</param>
        public static void NotifyVotingStarted(int requestId)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<Chat>();
            hub.Clients.Group("r" + requestId).votingStarted();
        }

        /// <summary>
        /// Notifies that vote done on specific request.
        /// </summary>
        /// <param name="requestId">Id of the request for which voting started.</param>
        /// <param name="userId">Id of the user which perform vote.</param>
        /// <param name="voteStatus">Status of the vote.</param>
        public static void NotifyVote(int requestId, string userId, int voteStatus)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<Chat>();
            hub.Clients.Group("r" + requestId).vote(userId, voteStatus);
        }

        /// <summary>
        /// Sends message to all connected clients.
        /// </summary>
        /// <param name="requestId">Id of the request to which meeting send.</param>
        /// <param name="message">Message to send.</param>
        /// <returns>Task which asynchronously performs operation.</returns>
        public async Task Send(int requestId, string message)
        {
            var user = (ClaimsPrincipal)this.Context.User;
            var requestManager = this.GetRequestManager();
            var request = await requestManager.FindByIdAsync(user, requestId);
            var meeting = request.Meetings.FirstOrDefault();
            if (!RequestManager.CouldSendChatInMeetingRoom(request, meeting, user))
            {
                this.Clients.Caller.error("could not send message in the room");
                return;
            }

            await requestManager.SendMessageAsync(user, request, message);
            var userId = user.FindFirst(ClaimTypes.Sid).Value;
            var firstName = user.FindFirst(ClaimTypes.GivenName).Value;
            var lastName = user.FindFirst(ClaimTypes.Surname).Value;
            this.Clients.Group("r" + requestId).message(userId, DateTime.UtcNow, message);
        }

        /// <summary>
        /// Join the meeting for the specific request.
        /// </summary>
        /// <param name="requestId">Id of the request for which join meeting.</param>
        /// <returns>Task which asynchronously performs operation.</returns>
        public async Task Join(int requestId)
        {
            var user = (ClaimsPrincipal)this.Context.User;
            var requestManager = this.GetRequestManager();
            var request = await requestManager.FindByIdAsync(user, requestId);
            var meeting = request.Meetings.FirstOrDefault();
            if (!RequestManager.CouldViewMeetingRoom(request, meeting, user))
            {
                this.Clients.Caller.error("could not view meeting room");
                return;
            }

            var messages = requestManager.GetMeetingChatMessages(meeting.Id).OrderBy(_ => _.SentDate);
            foreach (var message in messages)
            {
                this.Clients.Caller.message(message.UserId, message.SentDate, message.Message);
            }

            await this.Groups.Add(this.Context.ConnectionId, "r" + requestId);
        }

        /// <summary>
        /// Called when the connection connects to this hub instance.
        /// </summary>
        /// <returns>Task which represents this operation.</returns>
        public override Task OnConnected()
        {
            var user = (ClaimsPrincipal)this.Context.User;
            var userId = user.FindFirst(ClaimTypes.Sid).Value;
            var firstName = user.FindFirst(ClaimTypes.GivenName).Value;
            var lastName = user.FindFirst(ClaimTypes.Surname).Value;
            this.Clients.Others.NewUserConnected(userId, lastName + " " + firstName);
            return base.OnConnected();
        }

        /// <summary>
        /// Get request manager.
        /// </summary>
        /// <returns>Request manager to use.</returns>
        private RequestManager GetRequestManager()
        {
            var dbContext = new LecOnlineDbEntities();
            var changeManager = new ChangeManager(new ChangeManagerStore(dbContext));
            var store = new RequestStore(dbContext);
            var requestManager = new RequestManager(store, changeManager);
            return requestManager;
        }

        /// <summary>
        /// Get OWIN variable from request.
        /// </summary>
        /// <typeparam name="T">Type of the element.</typeparam>
        /// <returns>Element of the given type from the request context.</returns>
        private T GetOwinValiable<T>()
        {
            return (T)this.Context.Request.Environment["AspNet.Identity.Owin:" + typeof(T).AssemblyQualifiedName];
        }
    }
}
