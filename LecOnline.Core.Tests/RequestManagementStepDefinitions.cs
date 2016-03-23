// -----------------------------------------------------------------------
// <copyright file="RequestManagementStepDefinitions.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using LecOnline.Core.Fakes;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Step definitions which is related to requests management.
    /// </summary>
    [Binding]
    public class RequestManagementStepDefinitions
    {
        /// <summary>
        /// User context for the step.
        /// </summary>
        private SmtpContext smtpContext;

        /// <summary>
        /// Request context for the step.
        /// </summary>
        private RequestContext requestContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestManagementStepDefinitions"/> class.
        /// </summary>
        /// <param name="smtpContext">SMTP context for the steps.</param>
        /// <param name="requestContext">Request context for the steps.</param>
        public RequestManagementStepDefinitions(SmtpContext smtpContext, RequestContext requestContext)
        {
            this.smtpContext = smtpContext;
            this.requestContext = requestContext;
        }

        /// <summary>
        /// Run after each scenario.
        /// </summary>
        [AfterScenario]
        public void AfterScenario()
        {
            if (this.requestContext.Notifications != null)
            {
                this.requestContext.Notifications.Unsubscribe();
                this.requestContext.Notifications = null;
            }
        }

        /// <summary>
        /// Builds the request which is submitted to the committee.
        /// </summary>
        [Given(@"Request submitted to committee")]
        public void GivenRequestSubmittedToCommittee()
        {
            var request = new Request();
            request.CommitteeId = 1;
            request.ClientId = 1;
            request.Status = (int)RequestStatus.Submitted;
            this.requestContext.CurrentRequest = request;
        }

        /// <summary>
        /// Attach notifications to request manager.
        /// </summary>
        [Given(@"manager could send emails")]
        public void GivenManagerCouldSendEmails()
        {
            var fakeDatabase = new StubApplicationDbContext();
            fakeDatabase.SaveChangesAsync01 = () => Task.FromResult(1);
            var usersSet = new InMemoryDbSet<ApplicationUser>();
            usersSet.Attach(new ApplicationUser() { Email = "test1@lec-online.ru", FirstName = "Test1", LastName = "Test", CommitteeId = 1 });
            usersSet.Attach(new ApplicationUser() { Email = "test2@lec-online.ru", FirstName = "Test2", LastName = "Test", CommitteeId = 1 });
            fakeDatabase.UsersGet = () => usersSet;
            fakeDatabase.SetOf1<ApplicationUser>(() => usersSet);
            var userStore = new UserStore<ApplicationUser>(fakeDatabase);
            var applicationManager = new ApplicationUserManager(userStore);
            var notifications = new RequestNotifications(this.requestContext.Manager, applicationManager);
            notifications.Subscribe();
            this.requestContext.Notifications = notifications;

            this.smtpContext.Host.Start();
        }

        /// <summary>
        /// Setup request manager which will return specific request.
        /// </summary>
        [Given(@"manager related to request")]
        public void GivenManagerRelatedToRequest()
        {
            var request = this.requestContext.CurrentRequest;
            var requestStore = new StubIRequestStore()
            {
                FindByIdInt32 = (id) => Task.FromResult(request),
                GetChangedObject = (x) => new Dictionary<string, Tuple<object, object>>(),
                UpdateAsyncRequest = (x) => Task.FromResult(0),
                CreateAsyncRequestAction = (x) => Task.FromResult(0)
            };
            var changeStore = new StubIChangeManagerStore();
            var manager = new RequestManager(requestStore, new ChangeManager(changeStore));
            this.requestContext.Manager = manager;
        }

        /// <summary>
        /// Specifies current status of the request.
        /// </summary>
        [When(@"server start pending meeting")]
        public void WhenServerStartPendingMeeting()
        {
            var request = this.requestContext.CurrentRequest;
            var requestStore = new StubIRequestStore()
            {
                UpdateAsyncRequest = (x) => Task.FromResult(0),
                CreateAsyncRequestAction = (x) => Task.FromResult(0)
            };
            var changeStore = new StubIChangeManagerStore();
            var manager = new RequestManager(requestStore, new ChangeManager(changeStore));
            manager.StartMeetingIfPossible(request).Wait();
        }

        /// <summary>
        /// Accepts the request.
        /// </summary>
        [When(@"request is accepted")]
        public void WhenRequestIsAccepted()
        {
            var request = this.requestContext.CurrentRequest;
            var committeeId = request.CommitteeId.Value;
            var claim = GetCommitteeSecretary(committeeId);
            this.requestContext.Manager.AcceptAsync(claim, request.Id).Wait();
        }

        /// <summary>
        /// Rejects the request.
        /// </summary>
        /// <param name="reason">Rejection reason.</param>
        [When(@"request is rejected with comment '(.*)'")]
        public void WhenRequestIsRejected(string reason)
        {
            var request = this.requestContext.CurrentRequest;
            var committeeId = request.CommitteeId.Value;
            var claim = GetCommitteeSecretary(committeeId);
            this.requestContext.Manager.RejectAsync(claim, request.Id, reason).Wait();
        }

        /// <summary>
        /// Tests that message about acceptance of the request would be sent.
        /// </summary>
        [Then(@"acceptance message should be sent to manager")]
        public void ThenAcceptanceMessageShouldBeSentToManager()
        {
            var messages = this.smtpContext.Host.Messages;
            var message = messages.First();
            Assert.AreEqual(LecOnline.Core.Properties.Resources.MailRequestAcceptedSubject, message.Subject);
        }

        /// <summary>
        /// Tests that message about rejection of the request would be sent.
        /// </summary>
        [Then(@"rejection message should be sent to manager")]
        public void ThenRejectionMessageShouldBeSentToManager()
        {
            var messages = this.smtpContext.Host.Messages;
            var message = messages.First();
            Assert.AreEqual(LecOnline.Core.Properties.Resources.MailRequestRejectedSubject, message.Subject);
        }

        /// <summary>
        /// Gets principal which represents committee secretary.
        /// </summary>
        /// <param name="committeeId">Id of the committee.</param>
        /// <returns>Principal with desired properties.</returns>
        private static ClaimsPrincipal GetCommitteeSecretary(int committeeId)
        {
            var claim = new ClaimsPrincipal();
            var identity = new ClaimsIdentity();
            claim.AddIdentity(identity);
            identity.AddClaim(new Claim(ClaimTypes.Role, RoleNames.EthicalCommitteeMember));
            identity.AddClaim(new Claim(WellKnownClaims.CommitteeClaim, committeeId.ToString()));
            identity.AddClaim(new Claim(WellKnownClaims.CommitteeSecretaryClaim, committeeId.ToString()));
            return claim;
        }

        /// <summary>
        /// Gets principal which represents committee chairman.
        /// </summary>
        /// <param name="committeeId">Id of the committee.</param>
        /// <returns>Principal with desired properties.</returns>
        private static ClaimsPrincipal GetCommitteeChairman(int committeeId)
        {
            var claim = new ClaimsPrincipal();
            var identity = new ClaimsIdentity();
            claim.AddIdentity(identity);
            identity.AddClaim(new Claim(ClaimTypes.Role, RoleNames.EthicalCommitteeMember));
            identity.AddClaim(new Claim(WellKnownClaims.CommitteeClaim, committeeId.ToString()));
            identity.AddClaim(new Claim(WellKnownClaims.CommitteeChairmanClaim, committeeId.ToString()));
            return claim;
        }

        /// <summary>
        /// Gets principal which represents administrator.
        /// </summary>
        /// <returns>Principal with desired properties.</returns>
        private static ClaimsPrincipal GetAdministrator()
        {
            var claim = new ClaimsPrincipal();
            var identity = new ClaimsIdentity();
            claim.AddIdentity(identity);
            identity.AddClaim(new Claim(ClaimTypes.Role, RoleNames.Administrator));
            return claim;
        }
    }
}
