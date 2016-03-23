// -----------------------------------------------------------------------
// <copyright file="RequestActionsViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System.Collections.Generic;
    using System.Linq;
    using LecOnline.Core;

    /// <summary>
    /// Model for the request actions.
    /// </summary>
    public class RequestActionsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestActionsViewModel"/> class.
        /// </summary>
        public RequestActionsViewModel()
        {
            this.Actions = new List<RequestAction>();
        }

        /// <summary>
        /// Gets actions.
        /// </summary>
        public IList<RequestAction> Actions { get; private set; }
    }
}
