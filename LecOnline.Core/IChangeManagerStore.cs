// -----------------------------------------------------------------------
// <copyright file="IChangeManagerStore.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for the persisting audit log changes.
    /// </summary>
    public interface IChangeManagerStore : IDisposable
    {
        /// <summary>
        /// Gets audit log entries in the store.
        /// </summary>
        IQueryable<ChangesLog> ChangesLogs { get; }

        /// <summary>
        /// Asynchronously inserts a new audit log entry.
        /// </summary>
        /// <param name="logEntry">Log entry to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task RegisterAsync(ChangesLog logEntry);
    }
}
