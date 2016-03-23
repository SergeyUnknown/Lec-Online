// -----------------------------------------------------------------------
// <copyright file="Startup.Auth.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline
{
    using System;
    using LecOnline.Core;
    using LecOnline.Identity;
    using LecOnline.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;

    /// <summary>
    /// Configure application authentication.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configuration of the authentication the application.
        /// </summary>
        /// <param name="app">Application builder to use for configuration.</param>
        public void ConfigureAuth(IAppBuilder app)
        {
            var configuration = LecOnlineConfigurationSection.Instance ?? new LecOnlineConfigurationSection();
            System.Data.Entity.Database.SetInitializer<ApplicationDbContext>(null);

            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext(() => new LecOnlineDbEntities());
            app.CreatePerOwinContext<IUserStore<ApplicationUser>>(CreateUserStore);
            app.CreatePerOwinContext<ApplicationUserManager>(CreateUserManager);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ActionsManager>((options, context) =>
            {
                return new ActionsManager(() => context.Authentication.User);
            });
            app.CreatePerOwinContext<SidebarManager>((options, context) =>
            {
                return new SidebarManager(() => context.Authentication.User);
            });
            app.CreatePerOwinContext<ChangeManager>((options, context) =>
            {
                var dbContext = context.Get<LecOnlineDbEntities>();
                var store = new ChangeManagerStore(dbContext);
                return new ChangeManager(store);
            });
            app.CreatePerOwinContext<RequestManager>((options, context) =>
            {
                var dbContext = context.Get<LecOnlineDbEntities>();
                var changeManager = context.Get<ChangeManager>();
                var store = new RequestStore(dbContext);
                var requestManager = new RequestManager(store, changeManager);
                requestManager.VotingStarted += (request) =>
                {
                    LecOnline.Hubs.Chat.NotifyVotingStarted(request.Id);
                };
                requestManager.VoteMade += (request, user, status) =>
                {
                    LecOnline.Hubs.Chat.NotifyVote(
                        request.Id, 
                        user.FindFirst(System.Security.Claims.ClaimTypes.Sid).Value, 
                        (int)status);
                };
                return requestManager;
            });
            if (configuration.SendNotificationEmails)
            {
                app.CreatePerOwinContext<RequestNotifications>((options, context) =>
                {
                    var userManager = context.Get<ApplicationUserManager>();
                    var requestManager = context.Get<RequestManager>();
                    var notifications = new RequestNotifications(requestManager, userManager);
                    notifications.Subscribe();
                    return notifications;
                });
            }

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),

                    // Setting this property allows fixing issue https://katanaproject.codeplex.com/workitem/346.
                    // See discussion at https://katanaproject.codeplex.com/discussions/565294
                    OnException = context => { }
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            // app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            // app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            // app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            // app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            // {
            //     ClientId = "",
            //     ClientSecret = ""
            // });
        }

        /// <summary>
        /// Creates persistence user store.
        /// </summary>
        /// <param name="options">Options for the user store creation.</param>
        /// <param name="context">OWIN context in which user store created.</param>
        /// <returns>User store for given context.</returns>
        private static IUserStore<ApplicationUser> CreateUserStore(
            IdentityFactoryOptions<IUserStore<ApplicationUser>> options,
            IOwinContext context)
        {
            var dbContext = context.Get<ApplicationDbContext>();
            var userStore = new UserStore<ApplicationUser>(dbContext);
            return userStore;
        }

        /// <summary>
        /// Creates user manager.
        /// </summary>
        /// <param name="options">Options for the user manager creation.</param>
        /// <param name="context">OWIN context in which user manager created.</param>
        /// <returns>User manager for given context.</returns>
        private static ApplicationUserManager CreateUserManager(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var userStore = context.Get<IUserStore<ApplicationUser>>();
            var manager = new ApplicationUserManager(userStore);

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            var phoneProvider = new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            };
            var emailProvider = new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            };
            manager.RegisterTwoFactorProvider("Phone Code", phoneProvider);
            manager.RegisterTwoFactorProvider("Email Code", emailProvider);
            manager.EmailService = new EmailService(LecOnline.Properties.Resources.NoReplyName);
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }
}