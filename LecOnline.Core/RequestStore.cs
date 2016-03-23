// -----------------------------------------------------------------------
// <copyright file="RequestStore.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Request store which persist request to the database.
    /// </summary>
    public class RequestStore : IRequestStore
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
        /// Initializes a new instance of the <see cref="RequestStore"/> class.
        /// </summary>
        /// <param name="context">Database context to use for persisting requests.</param>
        public RequestStore(LecOnlineDbEntities context)
        {
            this.context = context;
        }
        
        /// <summary>
        /// Gets requests in the store.
        /// </summary>
        public IQueryable<Request> Requests
        {
            get
            {
                return this.context.Requests.Include(r => r.Meetings.Select(m => m.MeetingAttendees));
            }
        }

        /// <summary>
        /// Gets messages for the meeting.
        /// </summary>
        public IQueryable<MeetingChatMessage> MeetingChatMessages
        {
            get
            {
                return this.context.MeetingChatMessages;
            }
        }

        /// <summary>
        /// Asynchronously inserts a new request.
        /// </summary>
        /// <param name="request">Request to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task CreateAsync(Request request)
        {
            this.ThrowIfDisposed();
            this.context.Requests.Add(request);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a request.
        /// </summary>
        /// <param name="request">Request to delete.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task DeleteAsync(Request request)
        {
            this.ThrowIfDisposed();
            this.context.Requests.Remove(request);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously deletes a request documentation.
        /// </summary>
        /// <param name="documentation">Request documentation to delete.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task DeleteAsync(RequestDocumentation documentation)
        {
            this.ThrowIfDisposed();
            this.context.RequestDocumentations.Remove(documentation);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates a request.
        /// </summary>
        /// <param name="request">Request to update.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task UpdateAsync(Request request)
        {
            this.ThrowIfDisposed();
            this.context.Entry(request).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously updates a attendee.
        /// </summary>
        /// <param name="attendee">Meeting attendee to update.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task UpdateAsync(MeetingAttendee attendee)
        {
            this.ThrowIfDisposed();
            this.context.Entry(attendee).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously inserts a new request documentation.
        /// </summary>
        /// <param name="documentation">Request documentation to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task CreateAsync(RequestDocumentation documentation)
        {
            this.ThrowIfDisposed();
            this.context.RequestDocumentations.Add(documentation);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously find request with given id.
        /// </summary>
        /// <param name="requestId">Find id of the request.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task<Request> FindById(int requestId)
        {
            this.ThrowIfDisposed();
            return await this.GetRequestAggregateAsync(_ => _.Id == requestId).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously find request documentation with given id.
        /// </summary>
        /// <param name="documentationId">Find id of the request documentation.</param>
        /// <returns>The task object representing the asynchronous operation for retrieving request.</returns>
        public virtual async Task<RequestDocumentation> FindDocumentationByIdAsync(int documentationId)
        {
            this.ThrowIfDisposed();
            return await this.GetRequestDocumentationAggregateAsync(_ => _.Id == documentationId).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets changes which is made to the entity from last retrieval from the database.
        /// </summary>
        /// <param name="entity">Entity for which get changes.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Dictionary<string, Tuple<object, object>> GetChanged(object entity)
        {
            var entry = this.context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }

            var result = new Dictionary<string, Tuple<object, object>>();
            foreach (var originalProperty in entry.CurrentValues.PropertyNames)
            {
                var originalValue = entry.State == EntityState.Added 
                    ? null 
                    : entry.OriginalValues[originalProperty];
                var currentValue = entry.State == EntityState.Unchanged
                    ? null
                    : entry.CurrentValues[originalProperty];
                if (originalValue == null)
                {
                    if (currentValue != null)
                    {
                        result.Add(originalProperty, Tuple.Create(originalValue, currentValue));
                    }
                } 
                else 
                {
                    if (!originalValue.Equals(currentValue))
                    {
                        result.Add(originalProperty, Tuple.Create(originalValue, currentValue));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Asynchronously inserts a new request meeting.
        /// </summary>
        /// <param name="meeting">Request meeting to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task CreateAsync(Meeting meeting)
        {
            this.ThrowIfDisposed();
            this.context.Meetings.Add(meeting);
            foreach (var attendee in meeting.MeetingAttendees)
            {
                this.context.MeetingAttendees.Add(attendee);
            }

            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously inserts a new request meeting.
        /// </summary>
        /// <param name="meeting">Request meeting to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task UpdateAsync(Meeting meeting)
        {
            this.ThrowIfDisposed();
            this.context.Entry(meeting).State = EntityState.Modified;

            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously inserts a new chat message.
        /// </summary>
        /// <param name="message">Message to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task CreateAsync(MeetingChatMessage message)
        {
            this.ThrowIfDisposed();
            this.context.MeetingChatMessages.Add(message);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Asynchronously inserts a new request action.
        /// </summary>
        /// <param name="requestAction">Request action to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual async Task CreateAsync(RequestAction requestAction)
        {
            this.ThrowIfDisposed();
            this.context.RequestActions.Add(requestAction);
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
        /// Get request with aggregated data asynchronously.
        /// </summary>
        /// <param name="filter">Filter which should be applied to requests.</param>
        /// <returns>The task object representing the asynchronous operation for retrieving request.</returns>
        private Task<Request> GetRequestAggregateAsync(Expression<Func<Request, bool>> filter)
        {
            return this.context.Requests
                .Include(u => u.RequestDocumentations)
                .Include(u => u.RequestActions)
                .Include(u => u.Meetings.Select(a => a.MeetingAttendees))
                .FirstOrDefaultAsync(filter);
        }

        /// <summary>
        /// Get request documentation with aggregated data asynchronously.
        /// </summary>
        /// <param name="filter">Filter which should be applied to request documentation.</param>
        /// <returns>The task object representing the asynchronous operation for retrieving request documentation.</returns>
        private Task<RequestDocumentation> GetRequestDocumentationAggregateAsync(Expression<Func<RequestDocumentation, bool>> filter)
        {
            return this.context.RequestDocumentations.Include(u => u.Request)
                .FirstOrDefaultAsync(filter);
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
