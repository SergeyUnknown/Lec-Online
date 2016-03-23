// -----------------------------------------------------------------------
// <copyright file="RoleNames.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    /// <summary>
    /// Constants for the role names
    /// </summary>
    public class RoleNames
    {
        /// <summary>
        /// Name of the administrator role.
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// Id of the administrator role.
        /// </summary>
        public const string AdministratorId = "d9317a83-834b-46c6-8df2-2c1438adf390";

        /// <summary>
        /// Name of the manager role.
        /// </summary>
        public const string Manager = "Manager";

        /// <summary>
        /// Id of the manager role.
        /// </summary>
        public const string ManagerId = "d9317a83-834b-46c6-8df2-2c1438adf391";

        /// <summary>
        /// Name of the ethical committee member role.
        /// </summary>
        public const string EthicalCommitteeMember = "EthicalCommitteeMember";

        /// <summary>
        /// Id of the ethical committee member role.
        /// </summary>
        public const string EthicalCommitteeMemberId = "d9317a83-834b-46c6-8df2-2c1438adf392";

        /// <summary>
        /// Name of the medical center role.
        /// </summary>
        public const string MedicalCenter = "MedicalCenter";

        /// <summary>
        /// Id of the medical center role.
        /// </summary>
        public const string MedicalCenterId = "d9317a83-834b-46c6-8df2-2c1438adf393";

        /// <summary>
        /// Administrator and manager roles together.
        /// </summary>
        public const string AdministratorAndManager = Administrator + "," + Manager;
    }
}
