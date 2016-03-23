// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Controllers
{
    using System.Linq;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using LecOnline.Core;
    using LecOnline.Identity;
    using LecOnline.Models;
    using LecOnline.Models.Account;
    using LecOnline.Properties;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;

    /// <summary>
    /// Account controller.
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// Sign-in manager.
        /// </summary>
        private ApplicationSignInManager signInManager;

        /// <summary>
        /// User manager.
        /// </summary>
        private ApplicationUserManager userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">User manager for this controller.</param>
        /// <param name="signInManager">Sign-in manager for this controller.</param>
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }

        /// <summary>
        /// Gets sign-in manager.
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return this.signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }

            private set 
            {
                this.signInManager = value; 
            }
        }

        /// <summary>
        /// Gets user manager.
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set
            {
                this.userManager = value;
            }
        }

        /// <summary>
        /// Gets authentication manager.
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Shows login page for entering login information.
        /// </summary>
        /// <param name="returnUrl">Url to which user should return after login.</param>
        /// <returns>Action result.</returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        /// <summary>
        /// Performs login based on entered information from user.
        /// </summary>
        /// <param name="model">Login parameters.</param>
        /// <param name="returnUrl">Url to which user should return after login.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await this.SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return this.RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return this.View("Lockout");
                case SignInStatus.RequiresVerification:
                    return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    this.ModelState.AddModelError(string.Empty, Resources.InvalidLoginAttempt);
                    return this.View(model);
            }
        }

        /// <summary>
        /// Shows screen for verification confirmation code in two-factor authorization scenarios.
        /// </summary>
        /// <param name="provider">Provider for which verification happens.</param>
        /// <param name="returnUrl">Url to which user should return after verification.</param>
        /// <param name="rememberMe">True if authenticated user should be persisted for future visits; false otherwise.</param>
        /// <returns>Action result.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await this.SignInManager.HasBeenVerifiedAsync())
            {
                return this.View("Error");
            }

            return this.View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// Performs verification of security code generated during two-factor authentication.
        /// </summary>
        /// <param name="model">Data for verify security code.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await this.SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return this.RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return this.View("Lockout");
                case SignInStatus.Failure:
                default:
                    this.ModelState.AddModelError(string.Empty, Resources.InvalidVerificationCode);
                    return this.View(model);
            }
        }

        /// <summary>
        /// Show page for entering registration information.
        /// </summary>
        /// <returns>Action result.</returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return this.View();
        }

        /// <summary>
        /// Performs registration of the user based on entered information.
        /// </summary>
        /// <param name="model">Information about user to register.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (!model.UserAgreementAccepted)
                {
                    this.ModelState.AddModelError(string.Empty, Resources.ValidationMessageLicenseAgreementNotAccepted);
                    return this.View(model);
                }

                var user = new ApplicationUser
                { 
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PatronymicName = model.PatronymicName,
                    ContactPhone = model.ContactPhone,
                    Company = model.Company
                };
                var result = await this.UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {                   
                    // Send an email with confirmation link
                    try
                    {
                        await this.UserManager.SendEmailAsync(
                            user.Id,
                            Resources.MailRegistrationSuccessfulSubject,
                            Resources.MailRegistrationSuccessfulBody);
                    }
                    catch (SmtpException)
                    {
                    }

                    return this.RedirectToAction("RegistrationSuccess");
                }

                this.AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        /// <summary>
        /// Show page for success registration.
        /// </summary>
        /// <returns>Action result.</returns>
        [AllowAnonymous]
        public ActionResult RegistrationSuccess()
        {
            return this.View();
        }

        /// <summary>
        /// Confirm the user's email with confirmation token
        /// </summary>
        /// <param name="userId">User identifier to confirm.</param>
        /// <param name="code">Confirmation code.</param>
        /// <returns>Action result.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.View("Error");
            }

            var result = await this.UserManager.ConfirmEmailAsync(userId, code);
            return this.View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        /// <summary>
        /// Show page for the start forgot password procedure.
        /// </summary>
        /// <returns>Result of the action.</returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return this.View();
        }

        /// <summary>
        /// Initiates the forgot password procedure.
        /// </summary>
        /// <param name="model">Information for the forgot password.</param>
        /// <returns>Result of the action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await this.UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return this.View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await this.UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = this.Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await this.UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking [here](" + callbackUrl + ")");
                return this.RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        /// <summary>
        /// Show page which displays that forgot password procedure finished success.
        /// </summary>
        /// <returns>Result of the action.</returns>
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return this.View();
        }

        /// <summary>
        /// Show page for resetting password.
        /// </summary>
        /// <param name="userId">Id of the user which request password reset.</param>
        /// <param name="code">Security code which allows reset password.</param>
        /// <returns>Result of the action.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string userId, string code)
        {
            if (code == null || userId == null)
            {
                return this.View("Error");
            }

            var user = await this.UserManager.FindByIdAsync(userId);
            var model = new ResetPasswordViewModel();
            model.Code = code;
            model.Email = user.Email;
            return this.View(model);
        }

        /// <summary>
        /// Performs reset password
        /// </summary>
        /// <param name="model">Model with information about password reset.</param>
        /// <returns>Result of the action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return this.RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            var result = await this.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return this.RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            this.AddErrors(result);
            return this.View();
        }

        /// <summary>
        /// Show page that password reset procedure finished successfully.
        /// </summary>
        /// <returns>Result of the action.</returns>
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return this.View();
        }

        /// <summary>
        /// Show page for performing external login.
        /// </summary>
        /// <param name="provider">Authentication provider to use for login.</param>
        /// <param name="returnUrl">Url to which user should be returned after authorization.</param>
        /// <returns>Result of the action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, this.Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        /// <summary>
        /// Send security code in two-factor authentication.
        /// </summary>
        /// <param name="returnUrl">Url to which user should be returned after authorization.</param>
        /// <param name="rememberMe">True to persist user for next visits; false otherwise.</param>
        /// <returns>Result of the action.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await this.SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return this.View("Error");
            }

            var userFactors = await this.UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return this.View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        /// <summary>
        /// Sends code in two-factor authorization.
        /// </summary>
        /// <param name="model">Model with information about sending security code.</param>
        /// <returns>Result of the action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            // Generate the token and send it
            if (!await this.SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return this.View("Error");
            }

            return this.RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        /// <summary>
        /// Callback for external providers.
        /// </summary>
        /// <param name="returnUrl">Return url to which user should be redirected.</param>
        /// <returns>Result of the action.</returns>
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return this.RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await this.SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return this.RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return this.View("Lockout");
                case SignInStatus.RequiresVerification:
                    return this.RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    this.ViewBag.ReturnUrl = returnUrl;
                    this.ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        /// <summary>
        /// Confirm external login.
        /// </summary>
        /// <param name="model">Model for confirming authorization using external login.</param>
        /// <param name="returnUrl">Return url to which return user.</param>
        /// <returns>Result of the action.</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Manage");
            }

            if (this.ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await this.AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return this.View("ExternalLoginFailure");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await this.UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await this.UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await this.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return this.RedirectToLocal(returnUrl);
                    }
                }

                this.AddErrors(result);
            }

            this.ViewBag.ReturnUrl = returnUrl;
            return this.View(model);
        }

        /// <summary>
        /// Log-off user and terminate his authenticated session.
        /// </summary>
        /// <returns>Result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Shows failure to authenticate using external login.
        /// </summary>
        /// <returns>Result of the action.</returns>
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return this.View();
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.userManager != null)
                {
                    this.userManager.Dispose();
                    this.userManager = null;
                }

                if (this.signInManager != null)
                {
                    this.signInManager.Dispose();
                    this.signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Adds errors to model state from authorization results.
        /// </summary>
        /// <param name="result">Authorization result which errors should be added to model state.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }
        }

        /// <summary>
        /// Redirect to local url if possible.
        /// </summary>
        /// <param name="returnUrl">Return url to which user should be redirected.</param>
        /// <returns>Result of the action.</returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Http result which represents challenge procedure.
        /// </summary>
        internal class ChallengeResult : HttpUnauthorizedResult
        {
            /// <summary>
            /// Used for XSRF protection when adding external logins
            /// </summary>
            private const string XsrfKey = "XsrfId";

            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class using authorization provider and redirect uri.
            /// </summary>
            /// <param name="provider">Authorization provider to use in the challenge</param>
            /// <param name="redirectUri">Redirect uri to which user should be redirected after challenge succeed.</param>
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ChallengeResult"/> class using authorization provider, redirect uri and specific user.
            /// </summary>
            /// <param name="provider">Authorization provider to use in the challenge</param>
            /// <param name="redirectUri">Redirect uri to which user should be redirected after challenge succeed.</param>
            /// <param name="userId">Id of the user for which challenge performed.</param>
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                this.LoginProvider = provider;
                this.RedirectUri = redirectUri;
                this.UserId = userId;
            }

            /// <summary>
            /// Gets or sets authorization provider to use in the challenge
            /// </summary>
            public string LoginProvider { get; set; }

            /// <summary>
            /// Gets or sets redirect uri to which user should be redirected after challenge succeed.
            /// </summary>
            public string RedirectUri { get; set; }
            
            /// <summary>
            /// Gets or sets id of the user for which challenge performed.
            /// </summary>
            public string UserId { get; set; }

            /// <summary>
            /// Add information into the response environment that will cause the authentication
            /// middleware to challenge the caller to authenticate. This also changes the
            /// status code of the response to 401. The nature of that challenge varies greatly,
            /// and ranges from adding a response header or changing the 401 status code
            /// to a 302 redirect.
            /// </summary>
            /// <param name="context">
            /// The context in which the result is executed. The context information includes
            /// the controller, HTTP content, request context, and route data.
            /// </param>
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
                if (this.UserId != null)
                {
                    properties.Dictionary[XsrfKey] = this.UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
            }
        }
    }
}