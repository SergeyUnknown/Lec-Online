// -----------------------------------------------------------------------
// <copyright file="DateTimeTransforms.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core.Tests
{
    using System;
    using TechTalk.SpecFlow;

    /// <summary>
    /// SpecFlow transforms related to the dates.
    /// </summary>
    [Binding]
    public class DateTimeTransforms
    {
        /// <summary>
        /// Generate date which was ago by specified amount of days.
        /// </summary>
        /// <param name="daysAgo">Amount of days ago from now.</param>
        /// <returns>Date and time ago from now.</returns>
        [StepArgumentTransformation(@"(\d+) days ago")]
        public DateTime DaysAgoTransform(int daysAgo)
        {
            return DateTime.Today.AddDays(-daysAgo);
        }

        /// <summary>
        /// Generate date which was ago by specified amount of hours.
        /// </summary>
        /// <param name="hoursAgo">Amount of hours ago from now.</param>
        /// <returns>Date and time ago from now.</returns>
        [StepArgumentTransformation(@"(\d+) minutes ago")]
        public DateTime HoursAgoTransform(int hoursAgo)
        {
            return DateTime.Today.AddHours(-hoursAgo);
        }

        /// <summary>
        /// Generate date which was ago by specified amount of minutes.
        /// </summary>
        /// <param name="minutesAgo">Amount of minutes ago from now.</param>
        /// <returns>Date and time ago from now.</returns>
        [StepArgumentTransformation(@"(\d+) minutes ago")]
        public DateTime MinutesAgoTransform(int minutesAgo)
        {
            return DateTime.Today.AddMinutes(-minutesAgo);
        }
    }
}
