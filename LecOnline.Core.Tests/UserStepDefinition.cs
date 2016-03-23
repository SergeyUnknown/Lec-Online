// -----------------------------------------------------------------------
// <copyright file="UserStepDefinition.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Steps related to the user.
    /// </summary>
    [Binding]
    public class UserStepDefinition
    {
        /// <summary>
        /// User context for the step.
        /// </summary>
        private UserContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStepDefinition"/> class.
        /// </summary>
        /// <param name="context">User context in which step should be executed.</param>
        public UserStepDefinition(UserContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates user in the context.
        /// </summary>
        /// <param name="roleName">Role in which user should be created.</param>
        [Given(@"User with role (.*)")]
        public void GivenUserWithRole(string roleName)
        {
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim(identity.RoleClaimType, roleName));
            var user = new ClaimsPrincipal();
            user.AddIdentity(identity);
            this.context.User = user;
        }

        /// <summary>
        /// Creates user in the context.
        /// </summary>
        /// <param name="clientId">Id of the client to which user assigned.</param>
        [Given(@"Belongs to client (.*)")]
        public void GivenUserBelongsToClient(int clientId)
        {
            var identity = (ClaimsIdentity)this.context.User.Identity;
            identity.AddClaim(new Claim(WellKnownClaims.ClientClaim, clientId.ToString()));
        }

        /// <summary>
        /// Creates user in the context.
        /// </summary>
        /// <param name="committeeId">Id of the committee to which user assigned.</param>
        [Given(@"Belongs to committee (.*)")]
        public void GivenUserBelongsToCommittee(int committeeId)
        {
            var identity = (ClaimsIdentity)this.context.User.Identity;
            identity.AddClaim(new Claim(WellKnownClaims.CommitteeClaim, committeeId.ToString()));
        }

        /// <summary>
        /// Ensures that user is secretary for given committee.
        /// </summary>
        /// <param name="committeeId">Id of the committee.</param>
        [Given(@"User is secretary for committee (\d+)")]
        public void GivenUserIsSecretaryForCommittee(int committeeId)
        {
            var identity = (ClaimsIdentity)this.context.User.Identity;
            if (!identity.HasClaim(WellKnownClaims.CommitteeClaim, committeeId.ToString()))
            {
                identity.AddClaim(new Claim(WellKnownClaims.CommitteeClaim, committeeId.ToString()));
            }

            identity.AddClaim(new Claim(WellKnownClaims.CommitteeSecretaryClaim, committeeId.ToString()));
        }

        /// <summary>
        /// Ensures that user is chairman for given committee.
        /// </summary>
        /// <param name="committeeId">Id of the committee.</param>
        [Given(@"User is chairman for committee (\d+)")]
        public void GivenUserIsChairmanForCommittee(int committeeId)
        {
            var identity = (ClaimsIdentity)this.context.User.Identity;
            if (!identity.HasClaim(WellKnownClaims.CommitteeClaim, committeeId.ToString()))
            {
                identity.AddClaim(new Claim(WellKnownClaims.CommitteeClaim, committeeId.ToString()));
            }

            identity.AddClaim(new Claim(WellKnownClaims.CommitteeChairmanClaim, committeeId.ToString()));
        }
    }
}
