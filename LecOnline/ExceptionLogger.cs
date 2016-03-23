// -----------------------------------------------------------------------
// <copyright file="ExceptionLogger.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.ExceptionHandling;
    using LecOnline.Core;

    /// <summary>
    /// Exception handler for the WebAPI
    /// </summary>
    public class ExceptionLogger : IExceptionLogger
    {
        /// <summary>
        /// Logs an unhandled exception.
        /// </summary>
        /// <param name="context">The exception logger context.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous exception logging operation.</returns>
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            return ExceptionHelper.PublishExceptionAsync(
                context.RequestContext.Principal.Identity.Name,
                context.Exception,
                cancellationToken);
        }
    }
}
