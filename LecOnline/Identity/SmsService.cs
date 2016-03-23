// -----------------------------------------------------------------------
// <copyright file="SmsService.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Identity
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Service for sending SMS
    /// </summary>
    public class SmsService : IIdentityMessageService
    {
        /// <summary>
        /// This method should send the message
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <returns>Asynchronous task which sends the message.</returns>
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
