// -----------------------------------------------------------------------
// <copyright file="ChangeManager.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using LecOnline.Core.Properties;

    /// <summary>
    /// Register changes.
    /// </summary>
    public class ChangeManager : IDisposable
    {
        /// <summary>
        /// A value indicating that object is disposed.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Requests audit log store.
        /// </summary>
        private IChangeManagerStore store;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeManager"/> class using specified database context.
        /// </summary>
        /// <param name="store">Requests persistence store to save log entries.</param>
        public ChangeManager(IChangeManagerStore store)
        {
            this.store = store;
        }

        /// <summary>
        /// Registers request creation.
        /// </summary>
        /// <param name="request">Request which created.</param>
        /// <param name="changed">Changed data about request.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestCreatedAsync(Request request, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.CurrentProperties(changed);
            var description = string.Format(Resources.RequestCreated, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Registers change in the request.
        /// </summary>
        /// <param name="request">Request where change appears.</param>
        /// <param name="changed">Changed data about request.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestChangedAsync(Request request, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.ChangedProperties(changed);
            var description = string.Format(Resources.RequestChanged, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Registers change in the request.
        /// </summary>
        /// <param name="request">Request where change appears.</param>
        /// <param name="changed">Changed data about request.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestDeletedAsync(Request request, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.OriginalProperties(changed);
            var description = string.Format(Resources.RequestDeleted, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Registers change in the request.
        /// </summary>
        /// <param name="request">Request where change appears.</param>
        /// <param name="changed">Changed data about request.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestSubmittedAsync(Request request, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.ChangedProperties(changed);
            var description = string.Format(Resources.RequestSubmitted, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Registers change in the request.
        /// </summary>
        /// <param name="request">Request where change appears.</param>
        /// <param name="changed">Changed data about request.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestRevokedAsync(Request request, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.ChangedProperties(changed);
            var description = string.Format(Resources.RequestRevoked, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Registers application acceptance.
        /// </summary>
        /// <param name="request">Request where change appears.</param>
        /// <param name="changed">Changed data about request.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestAcceptedAsync(Request request, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.ChangedProperties(changed);
            var description = string.Format(Resources.RequestAccepted, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Registers application rejection.
        /// </summary>
        /// <param name="request">Request where change appears.</param>
        /// <param name="changed">Changed data about request.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestRejectedAsync(Request request, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.ChangedProperties(changed);
            var description = string.Format(Resources.RequestRejected, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Register document upload.
        /// </summary>
        /// <param name="request">Request to which document was uploaded.</param>
        /// <param name="documentation">Document which was uploaded.</param>
        /// <param name="changed">Changed information.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestDocumentationCreatedAsync(Request request, RequestDocumentation documentation, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.ChangedProperties(changed);
            var description = string.Format(Resources.RequestDocumentationCreated, request.Id, request.Title, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Register document deletion.
        /// </summary>
        /// <param name="request">Request to which document was deleted.</param>
        /// <param name="documentation">Document which was deleted.</param>
        /// <param name="changed">Changed information.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterRequestDocumentationDeletedAsync(Request request, RequestDocumentation documentation, Dictionary<string, Tuple<object, object>> changed, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = request.ClientId;
            logEntry.CommitteeId = request.CommitteeId;
            logEntry.ObjectId = request.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Request;
            var changedFieldsDescription = this.ChangedProperties(changed);
            var description = string.Format(Resources.RequestDocumentationDeleted, request.Id, request.Title, changedFieldsDescription);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Register starting of voting.
        /// </summary>
        /// <param name="meeting">Meeting where voting started.</param>
        /// <param name="principal">Principal which perform change.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterVotingStartedAsync(Meeting meeting, ClaimsPrincipal principal)
        {
            var logEntry = new ChangesLog();
            logEntry.Changed = DateTime.UtcNow;
            logEntry.ChangedBy = principal.Identity.Name;
            logEntry.ClientId = meeting.Request.ClientId;
            logEntry.CommitteeId = meeting.Request.CommitteeId;
            logEntry.ObjectId = meeting.Id;
            logEntry.ObjectType = (int)ChangedObjectType.Meeting;
            var description = string.Format(Resources.MeetingVotingStarted, meeting.Id, meeting.Request.Id, meeting.Request.Title);
            logEntry.ChangeDescription = description;
            await this.RegisterAsync(logEntry);
        }

        /// <summary>
        /// Registers the log entry.
        /// </summary>
        /// <param name="logEntry">Log entry to register.</param>
        /// <returns>Task which asynchronously register log entry.</returns>
        public async Task RegisterAsync(ChangesLog logEntry)
        {
            this.ThrowIfDisposed();
            await this.store.RegisterAsync(logEntry);
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
                this.store.Dispose();
                this.disposed = true;
            }
        }

        /// <summary>
        /// Filter log entries based on permissions.
        /// </summary>
        /// <param name="logEntries">Log entries to filter.</param>
        /// <param name="principal">Principal for which filter log entries.</param>
        /// <returns>Filtered log entries based on the permissions.</returns>
        private static IQueryable<ChangesLog> FilterRequests(IQueryable<ChangesLog> logEntries, ClaimsPrincipal principal)
        {
            return logEntries;
        }

        /// <summary>
        /// Collect information about changed properties.
        /// </summary>
        /// <param name="changed">Information about changed properties.</param>
        /// <returns>String representing changed properties.</returns>
        private string ChangedProperties(Dictionary<string, Tuple<object, object>> changed)
        {
            var list = new List<string>();
            foreach (var propertyChangedData in changed)
            {
                var propertyName = propertyChangedData.Key;
                var original = propertyChangedData.Value.Item1;
                var current = propertyChangedData.Value.Item2;
                list.Add(string.Format("{0}: {1} => {2}", propertyName, original, current));
            }

            return string.Join("\n", list);
        }

        /// <summary>
        /// Collect information about changed properties.
        /// </summary>
        /// <param name="changed">Information about changed properties.</param>
        /// <returns>String representing changed properties.</returns>
        private string CurrentProperties(Dictionary<string, Tuple<object, object>> changed)
        {
            var list = new List<string>();
            foreach (var propertyChangedData in changed)
            {
                var propertyName = propertyChangedData.Key;
                var current = propertyChangedData.Value.Item2;
                list.Add(string.Format("{0}: {1}", propertyName, current));
            }

            return string.Join("\n", list);
        }

        /// <summary>
        /// Collect information about changed properties.
        /// </summary>
        /// <param name="changed">Information about changed properties.</param>
        /// <returns>String representing changed properties.</returns>
        private string OriginalProperties(Dictionary<string, Tuple<object, object>> changed)
        {
            var list = new List<string>();
            foreach (var propertyChangedData in changed)
            {
                var propertyName = propertyChangedData.Key;
                var original = propertyChangedData.Value.Item1;
                list.Add(string.Format("{0}: {1}", propertyName, original));
            }

            return string.Join("\n", list);
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
