// -----------------------------------------------------------------------
// <copyright file="IRequestStore.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Persistence store for the requests.
    /// </summary>
    public interface IRequestStore : IDisposable
    {
        /// <summary>
        /// Gets requests in the store.
        /// </summary>
        IQueryable<Request> Requests { get; }

        /// <summary>
        /// Gets messages for the meeting.
        /// </summary>
        IQueryable<MeetingChatMessage> MeetingChatMessages { get; }

        /// <summary>
        /// Asynchronously inserts a new request.
        /// </summary>
        /// <param name="request">Request to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CreateAsync(Request request);

        /// <summary>
        /// Asynchronously deletes a request.
        /// </summary>
        /// <param name="request">Request to delete.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task DeleteAsync(Request request);
        
        /// <summary>
        /// Asynchronously updates a request.
        /// </summary>
        /// <param name="request">Request to update.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task UpdateAsync(Request request);

        /// <summary>
        /// Asynchronously inserts a new request documentation.
        /// </summary>
        /// <param name="documentation">Request documentation to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CreateAsync(RequestDocumentation documentation);

        /// <summary>
        /// Asynchronously deletes a request documentation.
        /// </summary>
        /// <param name="documentation">Request documentation to delete.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task DeleteAsync(RequestDocumentation documentation);

        /// <summary>
        /// Asynchronously find request with given id.
        /// </summary>
        /// <param name="requestId">Find id of the request.</param>
        /// <returns>The task object representing the asynchronous operation for retrieving request.</returns>
        Task<Request> FindById(int requestId);

        /// <summary>
        /// Asynchronously find request documentation with given id.
        /// </summary>
        /// <param name="documentationId">Find id of the request documentation.</param>
        /// <returns>The task object representing the asynchronous operation for retrieving request.</returns>
        Task<RequestDocumentation> FindDocumentationByIdAsync(int documentationId);

        /// <summary>
        /// Gets changes which is made to the entity from last retrieval from the database.
        /// </summary>
        /// <param name="entity">Entity for which get changes.</param>
        /// <returns>The object representing the changes to the object.</returns>
        Dictionary<string, Tuple<object, object>> GetChanged(object entity);

        /// <summary>
        /// Asynchronously inserts a new request meeting.
        /// </summary>
        /// <param name="meeting">Request meeting to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CreateAsync(Meeting meeting);

        /// <summary>
        /// Asynchronously updates a meeting.
        /// </summary>
        /// <param name="meeting">Meeting to update.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task UpdateAsync(Meeting meeting);

        /// <summary>
        /// Asynchronously updates a attendee.
        /// </summary>
        /// <param name="attendee">Meeting attendee to update.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task UpdateAsync(MeetingAttendee attendee);

        /// <summary>
        /// Asynchronously inserts a new chat message.
        /// </summary>
        /// <param name="message">Message to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CreateAsync(MeetingChatMessage message);

        /// <summary>
        /// Asynchronously inserts a new request action.
        /// </summary>
        /// <param name="requestAction">Request action to add.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task CreateAsync(RequestAction requestAction);
    }
}
