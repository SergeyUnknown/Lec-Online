// -----------------------------------------------------------------------
// <copyright file="SmtpContext.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System;
    using Antix.Mail.Smtp.Impostor;

    /// <summary>
    /// Context for the SMTP server.
    /// </summary>
    public class SmtpContext : IDisposable
    {
        /// <summary>
        /// SMTP server.
        /// </summary>
        private Server server;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpContext"/> class.
        /// </summary>
        public SmtpContext()
        {
            this.server = new Server();
            this.Host = this.server.CreateHost(new HostConfiguration
            {
                IPAddress = System.Net.IPAddress.Loopback,
                Port = 25
            });
            this.Host.Messages.DeleteAll();
        }

        /// <summary>
        /// Gets SMTP host which is receiving messages.
        /// </summary>
        public Host Host { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.Host != null)
            {
                this.Host.Stop();
                this.Host.Dispose();
                this.Host = null;
            }

            if (this.server != null)
            {
                this.server.Dispose();
                this.server = null;
            }
        }
    }
}
