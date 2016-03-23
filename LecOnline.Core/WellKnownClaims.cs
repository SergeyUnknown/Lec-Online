// -----------------------------------------------------------------------
// <copyright file="WellKnownClaims.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Security.Claims;

    /// <summary>
    /// Well known claims
    /// </summary>
    public static class WellKnownClaims
    {
        /// <summary>
        /// Company claim
        /// </summary>
        public const string ClientClaim = "http://schemas.lec-online.ru/claims/Client";

        /// <summary>
        /// Committee claim
        /// </summary>
        public const string CommitteeClaim = "http://schemas.lec-online.ru/claims/Committee";

        /// <summary>
        /// Committee chairman claim
        /// </summary>
        public const string CommitteeChairmanClaim = "http://schemas.lec-online.ru/claims/Committee/Chairman";

        /// <summary>
        /// Committee secretary claim
        /// </summary>
        public const string CommitteeSecretaryClaim = "http://schemas.lec-online.ru/claims/Committee/Secretary";

        /// <summary>
        /// Get company for the principal.
        /// </summary>
        /// <param name="principal">Principal for which get company id.</param>
        /// <returns>Id of the company which associated with given principal.</returns>
        public static int? GetClient(this ClaimsPrincipal principal)
        {
            var claim = WellKnownClaims.ClientClaim;
            return GetInt32(principal, claim);
        }

        /// <summary>
        /// Get committee for the principal.
        /// </summary>
        /// <param name="principal">Principal for which get committee id.</param>
        /// <returns>Id of the committee which associated with given principal.</returns>
        public static int? GetCommittee(this ClaimsPrincipal principal)
        {
            var claim = WellKnownClaims.CommitteeClaim;
            return GetInt32(principal, claim);
        }

        /// <summary>
        /// Get committee for the principal where it is chairman.
        /// </summary>
        /// <param name="principal">Principal for which get committee chairman status.</param>
        /// <returns>Id of the committee where this principal is chairman.</returns>
        public static int? GetCommitteeChairman(this ClaimsPrincipal principal)
        {
            var claim = WellKnownClaims.CommitteeChairmanClaim;
            return GetInt32(principal, claim);
        }

        /// <summary>
        /// Get committee for the principal where it is secretary.
        /// </summary>
        /// <param name="principal">Principal for which get committee secretary status.</param>
        /// <returns>Id of the committee where this principal is secretary.</returns>
        public static int? GetCommitteeSecretary(this ClaimsPrincipal principal)
        {
            var claim = WellKnownClaims.CommitteeSecretaryClaim;
            return GetInt32(principal, claim);
        }

        /// <summary>
        /// Get integer value for the claim.
        /// </summary>
        /// <param name="principal">Principal for which get claim value.</param>
        /// <param name="claim">Name of the claim for which to get value.</param>
        /// <returns>Value of the claim if present, null otherwise.</returns>
        private static int? GetInt32(ClaimsPrincipal principal, string claim)
        {
            var companyClaim = principal.FindFirst(claim);
            if (companyClaim == null)
            {
                return null;
            }

            return Convert.ToInt32(companyClaim.Value);
        }
    }
}
