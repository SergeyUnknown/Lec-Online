// -----------------------------------------------------------------------
// <copyright file="ChangeManagerStore.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Persists audit log entries in the database using Entity Framework
    /// </summary>
    public class ChangeManagerStore : IChangeManagerStore
    {
        /// <summary>
        /// A value indicating that object is disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Database context to use for persisting requests.
        /// </summary>
        private LecOnlineDbEntities context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeManagerStore"/> class.
        /// </summary>
        /// <param name="context">Database context to use for persisting log entries.</param>
        public ChangeManagerStore(LecOnlineDbEntities context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets audit log entries in the store.
        /// </summary>
        public IQueryable<ChangesLog> ChangesLogs
        {
            get
            {
                return this.context.ChangesLogs;
            }
        }

        /// <summary>
        /// Asynchronously inserts a new audit log entry.
        /// </summary>
        /// <param name="logEntry">Log entry to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task RegisterAsync(ChangesLog logEntry)
        {
            this.ThrowIfDisposed();
            this.context.ChangesLogs.Add(logEntry);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Dispose this object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.context.Dispose();
                this.disposed = true;
            }
        }

        /// <summary>
        /// Throws exception if object is disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }
    }
}
