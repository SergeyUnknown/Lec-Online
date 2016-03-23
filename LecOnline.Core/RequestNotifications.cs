// -----------------------------------------------------------------------
// <copyright file="RequestNotifications.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using LecOnline.Core.Properties;

    /// <summary>
    /// Provides notification about request changes.
    /// </summary>
    public class RequestNotifications : IDisposable
    {
        /// <summary>
        /// Request manager which produce notifications.
        /// </summary>
        private RequestManager manager;

        /// <summary>
        /// User manager which will provide information.
        /// </summary>
        private ApplicationUserManager userManager;

        /// <summary>
        /// Mail address from which all messages would be sent.
        /// </summary>
        private string noReplyAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestNotifications"/> class.
        /// </summary>
        /// <param name="manager">Requests manager which produce notifications.</param>
        /// <param name="userManager">User manager to use.</param>
        public RequestNotifications(RequestManager manager, ApplicationUserManager userManager)
        {
            this.manager = manager;
            this.userManager = userManager;
            var configuration = LecOnlineConfigurationSection.Instance ?? new LecOnlineConfigurationSection();
            this.noReplyAddress = configuration.NoReplyAddress;
        }

        /// <summary>
        /// Subscribe to events.
        /// </summary>
        public void Subscribe()
        {
            this.manager.RequestAccepted += this.OnRequestAccepted;
            this.manager.RequestRejected += this.OnRequestRejected;
            this.manager.RequestSubmitted += this.OnRequestSubmitted;
            this.manager.MeetingSetup += this.OnMeetingSetup;
            this.manager.RequestResolutionMade += this.OnRequestResolutionMade;
        }

        /// <summary>
        /// Unsubscribe from events.
        /// </summary>
        public void Unsubscribe()
        {
            this.manager.RequestAccepted -= this.OnRequestAccepted;
            this.manager.RequestRejected -= this.OnRequestRejected;
            this.manager.RequestSubmitted -= this.OnRequestSubmitted;
            this.manager.MeetingSetup -= this.OnMeetingSetup;
            this.manager.RequestResolutionMade -= this.OnRequestResolutionMade;
        }

        /// <summary>
        /// Frees unused resources.
        /// </summary>
        public void Dispose()
        {
            this.Unsubscribe();
        }

        /// <summary>
        /// Creates mail client.
        /// </summary>
        /// <returns>A <see cref="SmtpClient"/> which is used for send messages.</returns>
        private static SmtpClient CreateMailClient()
        {
            var client = new SmtpClient();
            if (client.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                client.PickupDirectoryLocation = System.IO.Path.GetFullPath(client.PickupDirectoryLocation);
                System.IO.Directory.CreateDirectory(client.PickupDirectoryLocation);
            }

            return client;
        }

        /// <summary>
        /// Generates notification about request acceptance.
        /// </summary>
        /// <param name="request">Request which was accepted.</param>
        private void OnRequestAccepted(Request request)
        {
            var client = CreateMailClient();
            var message = new MailMessage();
            var users = this.userManager.GetClientMembers(request.ClientId)
                .Where(_ => _.Roles.FirstOrDefault(r => r.RoleId == RoleNames.ManagerId) != null);
            message.To.Add(new MailAddress(this.noReplyAddress, Resources.MailNoReplyUser));
            foreach (var user in users)
            {
                message.Bcc.Add(new MailAddress(user.Email, user.LastName + " " + user.FirstName));
            }

            message.Subject = Resources.MailRequestAcceptedSubject;
            message.Body = string.Format(Resources.MailRequestAcceptedBody, request.Title);
            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                ExceptionHelper.PublishException("system", new InvalidOperationException(Resources.MailErrorFailedToSendEmail, ex));
            }
        }

        /// <summary>
        /// Generates notification about request rejection.
        /// </summary>
        /// <param name="request">Request which was rejected.</param>
        private void OnRequestRejected(Request request)
        {
            var client = CreateMailClient(); 
            var message = new MailMessage();
            var users = this.userManager.GetClientMembers(request.ClientId)
                .Where(_ => _.Roles.FirstOrDefault(r => r.RoleId == RoleNames.ManagerId) != null);
            message.To.Add(new MailAddress(this.noReplyAddress, Resources.MailNoReplyUser));
            foreach (var user in users)
            {
                message.Bcc.Add(new MailAddress(user.Email, user.LastName + " " + user.FirstName));
            }

            message.Subject = Resources.MailRequestRejectedSubject;
            message.Body = string.Format(Resources.MailRequestRejectedBody, request.Title, request.RejectionComments);
            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                ExceptionHelper.PublishException("system", new InvalidOperationException(Resources.MailErrorFailedToSendEmail, ex));
            }
        }

        /// <summary>
        /// Generates notification about submitting request to committee.
        /// </summary>
        /// <param name="request">Request which was submitted.</param>
        private void OnRequestSubmitted(Request request)
        {
            var client = CreateMailClient();
            var message = new MailMessage();
            var users = this.userManager.GetCommitteeMembers(request.CommitteeId.Value);
            var dbContext = new LecOnlineDbEntities();
            var committee = dbContext.Committees.Find(request.CommitteeId);
            message.To.Add(new MailAddress(this.noReplyAddress, Resources.MailNoReplyUser));
            foreach (var user in users)
            {
                if (user.Id == committee.Secretary || user.Id == committee.Chairman)
                {
                    message.Bcc.Add(new MailAddress(user.Email, user.LastName + " " + user.FirstName));
                }
            }

            message.Subject = Resources.MailRequestSubmittedSubject;
            message.Body = string.Format(Resources.MailRequestSubmittedBody, request.Title);
            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                ExceptionHelper.PublishException("system", new InvalidOperationException(Resources.MailErrorFailedToSendEmail, ex));
            }
        }

        /// <summary>
        /// Generates notification about setting up meeting.
        /// </summary>
        /// <param name="meeting">Meeting which was setup.</param>
        private void OnMeetingSetup(Meeting meeting)
        {
            this.OnMeetingSetupAsync(meeting).Wait();
        }

        /// <summary>
        /// Generates notification about setting up meeting.
        /// </summary>
        /// <param name="meeting">Meeting which was setup.</param>
        /// <returns>Task which asynchronously send message.</returns>
        private async Task OnMeetingSetupAsync(Meeting meeting)
        {
            var client = CreateMailClient();
            var message = new MailMessage();
            var request = meeting.Request;
            message.To.Add(new MailAddress(this.noReplyAddress, Resources.MailNoReplyUser));
            foreach (var attendee in meeting.MeetingAttendees)
            {
                var user = await this.userManager.FindByIdAsync(attendee.UserId);
                message.Bcc.Add(new MailAddress(user.Email, user.LastName + " " + user.FirstName));
            }

            message.Subject = Resources.MailMeetingSetupSubject;
            string requestUrl = "http://lec-online.ru/Request/Details/" + request.Id;
            message.Body = string.Format(Resources.MailMeetingSetupBody, request.Title, meeting.MeetingDate, requestUrl);
            try
            {
                await client.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                ExceptionHelper.PublishException("system", new InvalidOperationException(Resources.MailErrorFailedToSendEmail, ex));
            }
        }

        /// <summary>
        /// Generates notification about resolution made for request by the committee.
        /// </summary>
        /// <param name="request">Request which was submitted.</param>
        /// <param name="accepted">A value indicating that request was accepted or not.</param>
        private void OnRequestResolutionMade(Request request, bool accepted)
        {
            this.OnRequestResolutionMadeAsync(request, accepted).Wait();
        }

        /// <summary>
        /// Generates notification about resolution made for request by the committee.
        /// </summary>
        /// <param name="request">Request which was submitted.</param>
        /// <param name="accepted">A value indicating that request was accepted or not.</param>
        /// <returns>Task which asynchronously send message.</returns>
        private async Task OnRequestResolutionMadeAsync(Request request, bool accepted)
        {
            var client = CreateMailClient();
            var message = new MailMessage();
            message.To.Add(new MailAddress(this.noReplyAddress, Resources.MailNoReplyUser));
            
            var user = await this.userManager.FindByEmailAsync(request.CreatedBy);
            message.Bcc.Add(new MailAddress(user.Email, user.LastName + " " + user.FirstName));

            message.Subject = Resources.MailRequestResolutionMadeSubject;
            var resolution = accepted ? Resources.StudyAccepted : Resources.StudyRejected;
            var detailsLink = string.Empty;
            message.Body = string.Format(Resources.MailRequestResolutionMadeBody, request.Title, resolution, detailsLink);
            try
            {
                await client.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                ExceptionHelper.PublishException("system", new InvalidOperationException(Resources.MailErrorFailedToSendEmail, ex));
            }
        }
    }
}
