// -----------------------------------------------------------------------
// <copyright file="ActionsStepDefinition.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Actions related steps.
    /// </summary>
    [Binding]
    public class ActionsStepDefinition
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
        /// Actions manager.
        /// </summary>
        private ActionsManager actionsManager;

        /// <summary>
        /// Current actions.
        /// </summary>
        private IEnumerable<ActionDescription> actions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsStepDefinition"/> class.
        /// </summary>
        /// <param name="context">User context in which step should be executed.</param>
        /// <param name="requestContext">Request context for the steps.</param>
        public ActionsStepDefinition(UserContext context, RequestContext requestContext)
        {
            this.context = context;
            this.requestContext = requestContext;
        }

        /// <summary>
        /// Get actions available for the current user.
        /// </summary>
        [When(@"Get actions for user")]
        public void WhenGetActionsForUser()
        {
            this.actionsManager = new ActionsManager(() => this.context.User);
            var userEntity = new ApplicationUser();
            this.actions = this.actionsManager.GetActions(userEntity);
        }

        /// <summary>
        /// Get actions available for the current user.
        /// </summary>
        /// <param name="clientId">Id of the client to which assign user.</param>
        [When(@"Get actions for user from client (\d+)")]
        public void WhenGetActionsForUserFromClient(int clientId)
        {
            this.actionsManager = new ActionsManager(() => this.context.User);
            var userEntity = new ApplicationUser();
            userEntity.ClientId = clientId;
            this.actions = this.actionsManager.GetActions(userEntity);
        }
            
        /// <summary>
        /// Get actions available for the client.
        /// </summary>
        [When(@"Get actions for client")]
        public void WhenGetActionsForClient()
        {
            this.actionsManager = new ActionsManager(() => this.context.User);
            var entity = new Client();
            this.actions = this.actionsManager.GetActions(entity);
        }

        /// <summary>
        /// Get actions available for the committee.
        /// </summary>
        [When(@"Get actions for committee")]
        public void WhenGetActionsForCommittee()
        {
            this.actionsManager = new ActionsManager(() => this.context.User);
            var entity = new Committee();
            this.actions = this.actionsManager.GetActions(entity);
        }

        /// <summary>
        /// Get actions available for the request.
        /// </summary>
        [When(@"Get actions for request")]
        public void WhenGetActionsForRequest()
        {
            this.actionsManager = new ActionsManager(() => this.context.User);
            var entity = this.requestContext.CurrentRequest;
            this.actions = this.actionsManager.GetActions(entity);
        }

        /// <summary>
        /// Tests that specific action is available for the user.
        /// </summary>
        /// <param name="actionId">Action Id to check for the presence in the action list.</param>
        /// <param name="contains">True if action should contains in the actions list; false otherwise.</param>
        [Then(@"(.*) action (True|False) in the actions list")]
        public void ThenActionContainsInTheActionsList(string actionId, bool contains)
        {
            var actionInList = this.actions.FirstOrDefault(_ => _.Id == actionId) != null;
                Assert.AreEqual(contains, actionInList, string.Format("Action {0} should {1} in the list of actions", actionId, contains ? "contains" : "not contains"));
        }
    }
}
