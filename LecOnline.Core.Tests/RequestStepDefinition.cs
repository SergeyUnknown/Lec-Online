// -----------------------------------------------------------------------
// <copyright file="RequestStepDefinition.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LecOnline.Core.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Request related steps.
    /// </summary>
    [Binding]
    public class RequestStepDefinition
    {
        /// <summary>
        /// User context for the step.
        /// </summary>
        private UserContext context;

        /// <summary>
        /// Request context for the step.
        /// </summary>
        private RequestContext requestContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestStepDefinition"/> class.
        /// </summary>
        /// <param name="context">User context in which step should be executed.</param>
        /// <param name="requestContext">Request context for the steps.</param>
        public RequestStepDefinition(UserContext context, RequestContext requestContext)
        {
            this.context = context;
            this.requestContext = requestContext;
        }

        /// <summary>
        /// Creates request in the context.
        /// </summary>
        /// <param name="clientId">Id of the client to which request should belongs.</param>
        [Given(@"Request from client (.*)")]
        public void GivenRequestFromClient(int clientId)
        {
            var request = this.requestContext.CurrentRequest ?? new Request();
            request.ClientId = clientId;
            this.requestContext.CurrentRequest = request;
        }

        /// <summary>
        /// Creates request in the context.
        /// </summary>
        /// <param name="committeeId">Id of the client to which request should belongs.</param>
        [Given(@"Request for committee (.*)")]
        public void GivenRequestForCommittee(int committeeId)
        {
            var request = this.requestContext.CurrentRequest ?? new Request();
            request.CommitteeId = committeeId;
            this.requestContext.CurrentRequest = request;
        }

        /// <summary>
        /// Specifies current status of the request.
        /// </summary>
        /// <param name="requestStatus">Status for the request.</param>
        [Given(@"Request status is '(.*)'")]
        public void GivenRequestStatusIs(string requestStatus)
        {
            var status = (RequestStatus)Enum.Parse(typeof(RequestStatus), requestStatus);
            this.requestContext.CurrentRequest.Status = (int)status;
        }

        /// <summary>
        /// Specifies start date of the meeting.
        /// </summary>
        /// <param name="meetingsStartDate">Date when meeting is started.</param>
        [Given(@"has meeting started (.*)")]
        public void HasMeetingStarted(DateTime meetingsStartDate)
        {
            var request = this.requestContext.CurrentRequest;
            var meeting = new Meeting()
            {
                Request = request,
                RequestId = request.Id,
                MeetingDate = meetingsStartDate
            };
            request.Meetings.Add(meeting);
            request.Status = (int)RequestStatus.MeetingSet;
        }

        /// <summary>
        /// Specifies current status of the request.
        /// </summary>
        /// <param name="requestStatus">Status for the request.</param>
        [Then(@"Request status now is '(.*)'")]
        public void ThenRequestStatusNowIs(string requestStatus)
        {
            var status = (RequestStatus)Enum.Parse(typeof(RequestStatus), requestStatus);
            Assert.AreEqual((int)status, this.requestContext.CurrentRequest.Status);
        }
    }
}
