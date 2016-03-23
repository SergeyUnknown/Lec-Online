// -----------------------------------------------------------------------
// <copyright file="UserActionProvider.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Core
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using LecOnline.Core.Properties;

    /// <summary>
    /// Action provider for the user.
    /// </summary>
    public class UserActionProvider : IActionProvider
    {
        /// <summary>
        /// Checks whether entity type is supported by this provider.
        /// </summary>
        /// <param name="entityType">Type of the entity to check.</param>
        /// <returns>True if action type is supported.</returns>
        public bool IsTypeSuported(Type entityType)
        {
            return entityType.FullName == "LecOnline.Core.ApplicationUser"
                || entityType.FullName.StartsWith("System.Data.Entity.DynamicProxies.ApplicationUser");
        }

        /// <summary>
        /// Gets actions which action provider could add for given entity.
        /// </summary>
        /// <param name="principal">Principal which attempts to retrieve list of actions.</param>
        /// <param name="item">Entity for which actions could be provided.</param>
        /// <returns>Sequence of <see cref="ActionDescription"/> objects which represents actions.</returns>
        public IEnumerable<ActionDescription> GetActions(ClaimsPrincipal principal, object item)
        {
            dynamic ditem = item;
            var couldManagerOtherClients = principal.IsInRole(RoleNames.Administrator);
            if (principal.IsInRole(RoleNames.Administrator)
                || principal.IsInRole(RoleNames.Manager))
            {
                yield return new ActionDescription
                {
                    Id = "Common.Create",
                    CssClass = "info",
                    SmallCssClass = "blue",
                    Icon = "fa-edit",
                    Text = Resources.ActionEdit,
                    SortOrder = 10,
                    Action = "Create",
                    NotItemOperation = true,
                };
                
                if (!couldManagerOtherClients)
                {
                    int? clientId = ditem.ClientId;
                    int? editorClientId = principal.GetClient();
                    if (clientId != editorClientId)
                    {
                        yield break;
                    }
                }

                yield return new ActionDescription
                {
                    Id = "Common.Edit",
                    CssClass = "info",
                    SmallCssClass = "blue",
                    Icon = "fa-edit",
                    Text = Resources.ActionEdit,
                    SortOrder = 10,
                    Action = "Edit",
                    RouteParameters = new { id = ditem.Id },
                };
                yield return new ActionDescription
                {
                    Id = "Common.Delete",
                    CssClass = "danger",
                    SmallCssClass = "red",
                    Icon = "fa-remove",
                    Text = Resources.ActionDelete,
                    SortOrder = 20,
                    Action = "Delete",
                    RouteParameters = new { id = ditem.Id },
                };
                if (!ditem.EmailConfirmed)
                {
                    yield return new ActionDescription
                    {
                        Id = "User.Activate",
                        CssClass = "default",
                        SmallCssClass = "blue",
                        Icon = "fa-accept",
                        Text = Resources.ActionActivate,
                        SortOrder = 15,
                        Action = "Activate",
                        RouteParameters = new { id = ditem.Id },
                    };
                }
                else
                {
                    if (principal.IsInRole(RoleNames.Administrator))
                    {
                        yield return new ActionDescription
                        {
                            Id = "User.Deactivate",
                            CssClass = "default",
                            SmallCssClass = "blue",
                            Icon = "fa-accept",
                            Text = Resources.ActionDeactivate,
                            SortOrder = 15,
                            Action = "Deactivate",
                            RouteParameters = new { id = ditem.Id },
                        };
                        yield return new ActionDescription
                        {
                            Id = "User.PaswordReset",
                            CssClass = "default",
                            SmallCssClass = "blue",
                            Icon = "fa-accept",
                            Text = Resources.ActionPasswordReset,
                            SortOrder = 15,
                            Action = "ResetPassword",
                            RouteParameters = new { id = ditem.Id },
                        };
                    }
                }
            }
        }
    }
}
