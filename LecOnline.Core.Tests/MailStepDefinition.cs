// -----------------------------------------------------------------------
// <copyright file="MailStepDefinition.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TechTalk.SpecFlow;

    /// <summary>
    /// Mail related steps.
    /// </summary>
    [Binding]
    public class MailStepDefinition
    {
        /// <summary>
        /// User context for the step.
        /// </summary>
        private SmtpContext smtpContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailStepDefinition"/> class.
        /// </summary>
        /// <param name="smtpContext">SMTP context for the steps.</param>
        public MailStepDefinition(SmtpContext smtpContext)
        {
            this.smtpContext = smtpContext;
        }

        /// <summary>
        /// Run after test run.
        /// </summary>
        [BeforeScenario]
        public void BeforeTestRun()
        {
            this.smtpContext.Host.Start();
        }

        /// <summary>
        /// Run after test run.
        /// </summary>
        [AfterScenario]
        public void AfterTestRun()
        {
            if (this.smtpContext.Host.Status == Antix.Mail.Smtp.Impostor.HostStates.Started)
            {
                this.smtpContext.Host.Stop();
                this.smtpContext.Host.Dispose();
            }
        }

        /// <summary>
        /// Tests count of messages which was sent during test.
        /// </summary>
        /// <param name="messagesCount">Expected count of messages which was sent.</param>
        [Then(@"messages count equals (.*)")]
        public void ThenMessagesCountEquals(int messagesCount)
        {
            var messages = this.smtpContext.Host.Messages;
            Assert.AreEqual(messagesCount, messages.Count);
        }
    }
}
