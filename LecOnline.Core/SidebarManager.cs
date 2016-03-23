// -----------------------------------------------------------------------
// <copyright file="SidebarManager.cs" company="MDP-Soft">
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
    /// Manages the sidebar.
    /// </summary>
    public class SidebarManager : IDisposable
    {
        /// <summary>
        /// Root sidebar item.
        /// </summary>
        private SidebarItem rootItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="SidebarManager"/> class.
        /// </summary>
        /// <param name="userBuilder">User for which sidebar manager will provide results.</param>
        public SidebarManager(Func<ClaimsPrincipal> userBuilder)
        {
            this.User = new Lazy<ClaimsPrincipal>(userBuilder);
        }

        /// <summary>
        /// Gets user for which sidebar manager will provide information.
        /// </summary>
        public Lazy<ClaimsPrincipal> User { get; private set; }

        /// <summary>
        /// Get root item for sidebar.
        /// </summary>
        /// <returns>Root sidebar items.</returns>
        public SidebarItem GetRootSidebarItem()
        {
            if (this.rootItem != null)
            {
                return this.rootItem;
            }

            var rootItem = new SidebarItem();
            /*rootItem.Items.Add(new SidebarItem
                {
                    Icon = "fa-tachometer",
                    Title = Resources.SidebarDashboard,
                    Action = "Index",
                    Controller = "Home"
                });*/
            var requestsItem = new SidebarItem
            {
                Id = "Requests",
                Icon = "fa-file-text",
                Title = Resources.SidebarRequests,
            };
            this.BuildRequestsItem(requestsItem);
            rootItem.Items.Add(requestsItem);
            if (this.User.Value.IsInRole(RoleNames.Administrator) && false)
            {
                var centersItem = new SidebarItem
                {
                    Id = "MedicalCenters",
                    Icon = "fa-hospital-o",
                    Title = Resources.SidebarMedicalCenters,
                };
                this.BuildMedicalCenterItem(centersItem);
                rootItem.Items.Add(centersItem);
            }

            if (this.User.Value.IsInRole(RoleNames.Administrator)
                || this.User.Value.IsInRole(RoleNames.Manager))
            {
                var usersItem = new SidebarItem
                {
                    Id = "Users",
                    Icon = "fa-users",
                    Title = Resources.SidebarUsers,
                };
                this.BuildUserItem(usersItem);
                rootItem.Items.Add(usersItem);
            }

            if (this.User.Value.IsInRole(RoleNames.Administrator))
            {
                var securityItem = new SidebarItem
                {
                    Id = "Security",
                    Icon = "fa-shield",
                    Title = Resources.SidebarSecurity,
                };
                BuildSecurityItem(securityItem);
                rootItem.Items.Add(securityItem);

                var settingsItem = new SidebarItem
                {
                    Id = "Settings",
                    Icon = "fa-cogs",
                    Title = Resources.SidebarSettings,
                };
                BuildSettingsItem(settingsItem);
                rootItem.Items.Add(settingsItem);
            }
            
            this.rootItem = rootItem;
            return rootItem;
        }

        /// <summary>
        /// Get sidebar item by id.
        /// </summary>
        /// <param name="id">Id of the sidebar item to find</param>
        /// <returns>Sidebar item with requested id if exists, null otherwise.</returns>
        public SidebarItem FindById(string id)
        {
            var root = this.GetRootSidebarItem();
            return FindById(id, root);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Find item with given id within 
        /// </summary>
        /// <param name="id">Id of the item too look for.</param>
        /// <param name="parent">Parent item where to look item with given id.</param>
        /// <returns>Sidebar item with requested id if exists, null otherwise.</returns>
        private static SidebarItem FindById(string id, SidebarItem parent)
        {
            foreach (var item in parent.Items)
            {
                if (item.Id == id)
                {
                    return item;
                }

                var insideNested = FindById(id, item);
                if (insideNested != null)
                {
                    return insideNested;
                }
            }

            return null;
        }

        /// <summary>
        /// Build settings item.
        /// </summary>
        /// <param name="parent">Parent item for all created items.</param>
        private static void BuildSecurityItem(SidebarItem parent)
        {
            var auditItem = new SidebarItem
            {
                Id = "Security.Audit",
                Title = "Аудит",
                Action = "AuditLog",
                Controller = "Security",
            };
            parent.Items.Add(auditItem);

            var errorsItem = new SidebarItem
            {
                Id = "Security.Errors",
                Title = "Ошибки",
                Action = "Errors",
                Controller = "Security",
            };
            parent.Items.Add(errorsItem);
        }

        /// <summary>
        /// Build settings item.
        /// </summary>
        /// <param name="parent">Parent item for all created items.</param>
        private static void BuildSettingsItem(SidebarItem parent)
        {
            var clientsItem = new SidebarItem
            {
                Id = "Clients",
                Title = "Клиенты",
                Action = "Index",
                Controller = "Client",
            };
            parent.Items.Add(clientsItem);

            var committeesItem = new SidebarItem
            {
                Id = "Committees",
                Title = "Комитеты",
                Action = "Index",
                Controller = "Committee",
            };
            parent.Items.Add(committeesItem);
        }

        /// <summary>
        /// Build request related items.
        /// </summary>
        /// <param name="parent">Parent item for all request items.</param>
        private void BuildRequestsItem(SidebarItem parent)
        {
            parent.Items.Add(new SidebarItem
            {
                Id = "Requests.All",
                Title = "Все заявки",
                Action = "Index",
                Controller = "Request"
            });
        }

        /// <summary>
        /// Build medical center related items.
        /// </summary>
        /// <param name="parent">Parent item for all items.</param>
        private void BuildMedicalCenterItem(SidebarItem parent)
        {
            /*parent.Items.Add(new SidebarItem
            {
                Icon = "fa-hospital-o",
                Title = "Все медцентры",
                Action = "Index",
                Controller = "MedicalCenter"
            });*/
        }

        /// <summary>
        /// Build user related items.
        /// </summary>
        /// <param name="parent">Parent item for all items.</param>
        private void BuildUserItem(SidebarItem parent)
        {
            parent.Items.Add(new SidebarItem
            {
                Id = "Users.All",
                Title = "Все пользователи",
                Action = "Index",
                Controller = "User"
            });
            parent.Items.Add(new SidebarItem
            {
                Id = "Users.Pending",
                Title = "Не подтвержденные",
                Action = "Pending",
                Controller = "User"
            });
        }
    }
}
