// -----------------------------------------------------------------------
// <copyright file="RequestController.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Controllers
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using LecOnline.Core;
    using LecOnline.Models.Request;
    using LecOnline.Properties;
    using Microsoft.AspNet.Identity.Owin;

    /// <summary>
    /// Controller for managing requests.
    /// </summary>
    [Authorize(Roles = RoleNames.AdministratorAndManager + "," + RoleNames.EthicalCommitteeMember + "," + RoleNames.MedicalCenter)]
    public class RequestController : Controller
    {
        /// <summary>
        /// Initializes static members of the <see cref="RequestController"/> class.
        /// </summary>
        static RequestController()
        {
            Mapper.CreateMap<Request, RequestBaseInformationViewModel>();
            Mapper.CreateMap<RequestBaseInformationViewModel, Request>();
            Mapper.CreateMap<Request, RequestContactInformationViewModel>();
            Mapper.CreateMap<RequestContactInformationViewModel, Request>();
            Mapper.CreateMap<RequestDocumentation, RequestDocumentationFileViewModel>();
            Mapper.CreateMap<Request, RequestDocumentationViewModel>();
        }

        /// <summary>
        /// Displays list of all requests available for current user.
        /// </summary>
        /// <param name="filter">Filter which should be applied to the requests.</param>
        /// <returns>Result of the action.</returns>
        public ActionResult Index([Bind(Prefix = "Filter")]RequestsListFilter filter)
        {
            var context = HttpContext.GetOwinContext();
            var manager = context.Get<RequestManager>();
            var principal = context.Authentication.User;
            var requests = manager.GetRequests(principal);
            var model = new RequestsListViewModel(requests, filter);
            return this.View(model);
        }

        /// <summary>
        /// Displays page with information about application.
        /// </summary>
        /// <param name="id">Id of the application for which show the data</param>
        /// <returns>Task which asynchronously returns result of the action.</returns>
        public async Task<ActionResult> Details(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var model = new RequestDetailsViewModel();
            model.Id = request.Id;
            model.Documentation.Id = id;
            Mapper.Map(request, model.BaseInformation);
            Mapper.Map(request, model.ContactInformation);
            Mapper.Map(request.RequestDocumentations, model.Documentation.Files);
            Mapper.Map(request.RequestActions, model.Actions.Actions);
            if (!this.User.IsInRole(RoleNames.Administrator))
            {
                model.BaseInformation.ClientId = claimPrincipal.GetClient() ?? request.ClientId;
                model.BaseInformation.CommitteeId = null;
            }

            model.OriginalRequest = request;
            return this.View(model);
        }

        /// <summary>
        /// Downloads the request PDF.
        /// </summary>
        /// <param name="id">Id of the application for which show the request PDF</param>
        /// <returns>Task which asynchronously returns result of the action.</returns>
        public async Task<ActionResult> DetailsDoc(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var userManager = context.Get<ApplicationUserManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var builder = new LecOnline.Reporting.DocumentBuilder(dbContext, userManager);
            var document = await builder.GetRequest(request);
            var renderer = new MigraDoc.Rendering.PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            var stream = new System.IO.MemoryStream();
            renderer.PdfDocument.Save(stream, false);
            return this.File(stream, "application/pdf");
        }

        /// <summary>
        /// Displays page with information about application.
        /// </summary>
        /// <param name="id">Id of the application for which show the data</param>
        /// <returns>Task which asynchronously returns result of the action.</returns>
        public async Task<ActionResult> ResolutionDoc(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var dbContext = context.Get<LecOnlineDbEntities>();
            var userManager = context.Get<ApplicationUserManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var builder = new LecOnline.Reporting.DocumentBuilder(dbContext, userManager);
            var document = await builder.GetResolution(request);
            var renderer = new MigraDoc.Rendering.PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            var stream = new System.IO.MemoryStream();
            renderer.PdfDocument.Save(stream, false);
            return this.File(stream, "application/pdf");
        }

        /// <summary>
        /// Show edit user page by id.
        /// </summary>
        /// <returns>Result of the action.</returns>
        public ActionResult Create()
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            if (!RequestManager.CouldCreateRequest(claimPrincipal))
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new CreateRequestViewModel();
            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the user.
        /// </summary>
        /// <param name="model">Data to save about user.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRequestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = new Request();
            Mapper.Map(model.BaseInformation, request);
            Mapper.Map(model.ContactInformation, request);

            await requestManager.CreatePrimaryRequestAsync(claimPrincipal, request);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show edit user page by id.
        /// </summary>
        /// <param name="id">Id of the base request.</param>
        /// <returns>Result of the action.</returns>
        public async Task<ActionResult> CreateSecondary(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var baseRequest = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (!RequestManager.CouldCreateSecondaryRequest(baseRequest, claimPrincipal))
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new CreateSecondaryRequestViewModel();
            Mapper.Map(baseRequest, model.BaseRequestInformation);
            Mapper.Map(baseRequest, model.BaseInformation);
            Mapper.Map(baseRequest, model.ContactInformation);
            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the user.
        /// </summary>
        /// <param name="id">Id of the base request.</param>
        /// <param name="model">Data to save about user.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSecondary(int id, CreateSecondaryRequestViewModel model)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var baseRequest = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (!this.ModelState.IsValid)
            {
                Mapper.Map(baseRequest, model.BaseRequestInformation);
                return this.View(model);
            }

            var request = new Request();
            Mapper.Map(model.BaseInformation, request);
            Mapper.Map(model.ContactInformation, request);

            await requestManager.CreateSecondaryRequestAsync(claimPrincipal, request, baseRequest);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show edit user page by id.
        /// </summary>
        /// <param name="id">Id of the base request.</param>
        /// <returns>Result of the action.</returns>
        public async Task<ActionResult> CreateNotification(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var baseRequest = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (!RequestManager.CouldCreateNotificationRequest(baseRequest, claimPrincipal))
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new CreateSecondaryRequestViewModel();
            Mapper.Map(baseRequest, model.BaseRequestInformation);
            Mapper.Map(baseRequest, model.BaseInformation);
            Mapper.Map(baseRequest, model.ContactInformation);
            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the user.
        /// </summary>
        /// <param name="id">Id of the base request.</param>
        /// <param name="model">Data to save about user.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNotification(int id, CreateSecondaryRequestViewModel model)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var baseRequest = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (!this.ModelState.IsValid)
            {
                Mapper.Map(baseRequest, model.BaseRequestInformation);
                return this.View(model);
            }

            var request = new Request();
            Mapper.Map(model.BaseInformation, request);
            Mapper.Map(model.ContactInformation, request);

            await requestManager.CreateNotificationRequestAsync(claimPrincipal, request, baseRequest);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show edit user page by id.
        /// </summary>
        /// <param name="id">Id of the user to get</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Edit(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (request == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new EditRequestViewModel();
            model.Id = request.Id;
            Mapper.Map(request, model.BaseInformation);
            Mapper.Map(request, model.ContactInformation);
            Mapper.Map(request, model.Documentation);
            Mapper.Map(request.RequestDocumentations, model.Documentation.Files);
            if (!this.User.IsInRole(RoleNames.Administrator))
            {
                model.BaseInformation.ClientId = claimPrincipal.GetClient() ?? request.ClientId;
                model.BaseInformation.CommitteeId = null;
            }

            return this.View(model);
        }

        /// <summary>
        /// Saves changes during editing of the user.
        /// </summary>
        /// <param name="model">Data to save about user.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditRequestViewModel model)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, model.Id);
            if (!this.ModelState.IsValid)
            {
                Mapper.Map(request, model.Documentation);
                Mapper.Map(request.RequestDocumentations, model.Documentation.Files);
                return this.View(model);
            }

            Mapper.Map(model.BaseInformation, request);
            Mapper.Map(model.ContactInformation, request);
            Mapper.Map(request.RequestDocumentations, model.Documentation.Files);

            if (!this.User.IsInRole(RoleNames.Administrator))
            {
                request.ClientId = claimPrincipal.GetClient() ?? request.ClientId;
                request.CommitteeId = null;
            }

            await requestManager.UpdateAsync(claimPrincipal, request);
            foreach (HttpPostedFileBase file in Request.Files)
            {
                // file.SaveAs("myPath");
            }

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show delete user page.
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Delete(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (request == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new EditRequestViewModel();
            model.Id = request.Id;
            Mapper.Map(request, model.BaseInformation);
            Mapper.Map(request, model.ContactInformation);
            Mapper.Map(request.RequestDocumentations, model.Documentation.Files);
            if (!this.User.IsInRole(RoleNames.Administrator))
            {
                model.BaseInformation.ClientId = claimPrincipal.GetClient() ?? request.ClientId;
                model.BaseInformation.CommitteeId = null;
            }

            return this.View(model);
        }

        /// <summary>
        /// Deletes the request.
        /// </summary>
        /// <param name="model">Data about deleting user.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(EditRequestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, model.Id);
            await requestManager.DeleteAsync(claimPrincipal, request);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show submit request page.
        /// </summary>
        /// <param name="id">Id of the request to submit</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Submit(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (request == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            if (!requestManager.IsDocumentationSufficient(request))
            {
                this.ModelState.AddModelError(string.Empty, Resources.NotSufficientDocumentationForSubmission);
            }

            var model = new SubmitRequestViewModel();
            model.Id = request.Id;
            model.Title = request.Title;
            
            return this.View(model);
        }

        /// <summary>
        /// Submit the request.
        /// </summary>
        /// <param name="model">Data for request submission.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Submit(SubmitRequestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, model.Id);
            if (!requestManager.IsDocumentationSufficient(request) && !model.SuppressMissingFiles)
            {
                this.ModelState.AddModelError(string.Empty, Resources.NotSufficientDocumentationForSubmission);
                return this.View(model);
            }

            await requestManager.SubmitAsync(claimPrincipal, model.Id, model.CommitteeId.Value, model.Comment);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show revoke request page.
        /// </summary>
        /// <param name="id">Id of the request to revoke</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Revoke(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (request == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new RevokeRequestViewModel();
            model.Id = request.Id;
            model.Title = request.Title;

            return this.View(model);
        }

        /// <summary>
        /// Revoke the application.
        /// </summary>
        /// <param name="model">Data for application revocation.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Revoke(RevokeRequestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, model.Id);
            await requestManager.RevokeAsync(claimPrincipal, model.Id, model.Comment);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Accept application for processing by committee.
        /// </summary>
        /// <param name="id">Id of the request to accept</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Accept(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            await requestManager.AcceptAsync(claimPrincipal, id);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Show reject request page.
        /// </summary>
        /// <param name="id">Id of the request to reject</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> Reject(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (request == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new RejectRequestViewModel();
            model.Id = request.Id;
            model.Title = request.Title;

            return this.View(model);
        }

        /// <summary>
        /// Reject the application.
        /// </summary>
        /// <param name="model">Data for application rejection.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reject(RejectRequestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, model.Id);
            await requestManager.RejectAsync(claimPrincipal, model.Id, model.RejectionReason);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Upload documentation for the application.
        /// </summary>
        /// <param name="id">Id of the request for which upload documentation.</param>
        /// <param name="fileType">Type of documentation which is uploaded.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadDocumentation(int id, int fileType)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            foreach (string fileName in this.Request.Files)
            {
                var file = this.Request.Files[fileName];
                await requestManager.UploadDocumentationAsync(
                    claimPrincipal, 
                    id, 
                    (DocumentationType)fileType, 
                    file.FileName, 
                    file.ContentType, 
                    file.InputStream);
            }
            
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Download documentation file.
        /// </summary>
        /// <param name="id">Id of the file to download.</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<FileResult> DownloadFile(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var documentation = await requestManager.FindDocumentationByIdAsync(claimPrincipal, id);
            return this.File(documentation.Content, System.Net.Mime.MediaTypeNames.Application.Octet, documentation.Name);
        }

        /// <summary>
        /// Download archive of documentation files.
        /// </summary>
        /// <param name="id">Id of the request for which download archive of submission files.</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> SubmissionArchive(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            if (request.RequestDocumentations.Count == 0)
            {
                return this.View("NoSubmission", request);
            }

            return this.File(
                requestManager.GetSubmissionArchive(claimPrincipal, request), 
                System.Net.Mime.MediaTypeNames.Application.Zip, 
                string.Format(Resources.SubmissionBaseFileName, request.Id));
        }

        /// <summary>
        /// Show delete request documentation page.
        /// </summary>
        /// <param name="id">Id of the request documentation to delete</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> DeleteFile(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var documentation = await requestManager.FindDocumentationByIdAsync(claimPrincipal, id);
            if (documentation == null)
            {
                // Add notification that user does not found.
                return this.RedirectToAction("Index");
            }

            var model = new RequestDocumentationFileViewModel();
            model.Id = documentation.Id;
            Mapper.Map(documentation, model);
            return this.View(model);
        }

        /// <summary>
        /// Deletes the request documentation.
        /// </summary>
        /// <param name="model">Data about deleting documentation.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteFile(RequestDocumentationFileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var documentation = await requestManager.FindDocumentationByIdAsync(claimPrincipal, model.Id);
            await requestManager.DeleteAsync(claimPrincipal, documentation);
            return this.RedirectToAction("Edit", new { id = documentation.RequestId });
        }

        /// <summary>
        /// Shows the page for setting meeting date and time.
        /// </summary>
        /// <param name="id">Id of the request for which set meeting date.</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> SetMeeting(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var userManager = context.Get<ApplicationUserManager>();

            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var model = new SetMeetingViewModel();
            model.Id = id;
            model.Title = request.Title;
            model.MeetingDate = DateTime.UtcNow;
            var committeeId = request.CommitteeId.Value;
            model.Members = from m in userManager.GetCommitteeMembers(claimPrincipal, committeeId)
                            select m.Id;
            model.AvailableMembers = this.GetCommitteeMembers(committeeId);
            return this.View(model);
        }

        /// <summary>
        /// Sets request meeting.
        /// </summary>
        /// <param name="id">Id of the request.</param>
        /// <param name="model">Data about setting meeting.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetMeeting(int id, SetMeetingViewModel model)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var committeeId = request.CommitteeId.Value;
            if (!this.ModelState.IsValid)
            {
                model.AvailableMembers = this.GetCommitteeMembers(committeeId);
                return this.View(model);
            }

            var meeting = await requestManager.SetupMeetingAsync(claimPrincipal, request, model.MeetingDate, model.Members);
            return this.RedirectToAction("Edit", new { id = meeting.RequestId });
        }

        /// <summary>
        /// Start meeting.
        /// </summary>
        /// <param name="id">Id of the request for which starts meeting</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> StartMeeting(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            await requestManager.StartMeetingAsync(claimPrincipal, request);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Stop meeting.
        /// </summary>
        /// <param name="id">Id of the request for which stops meeting</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> StopMeeting(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            await requestManager.StopMeetingAsync(claimPrincipal, request);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Finish meeting.
        /// </summary>
        /// <param name="id">Id of the request for which finish meeting</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> FinishMeeting(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            await requestManager.FinishMeetingAsync(claimPrincipal, request);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Enter meeting resolution.
        /// </summary>
        /// <param name="id">Id of the request for which enter meeting resolution.</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> EnterMeetingResolution(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Completed);
            var model = new MeetingResolutionViewModel();
            model.Id = request.Id;
            model.Protocol = meeting.Protocol;
            model.MeetingLog = meeting.MeetingLog;
            model.Resolution = (MeetingResolution)meeting.Resolution;
            return this.View(model);
        }

        /// <summary>
        /// Enter meeting resolution.
        /// </summary>
        /// <param name="model">Id of the request for which enter meeting resolution.</param>
        /// <returns>Task which returns result of the action.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnterMeetingResolution(MeetingResolutionViewModel model)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, model.Id);
            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Completed);
            meeting.Resolution = (int)model.Resolution;
            meeting.MeetingLog = model.MeetingLog;
            meeting.Protocol = model.Protocol;
            if (model.Resolution != MeetingResolution.NotProvided)
            {
                await requestManager.FinalizeMeetingAsync(claimPrincipal, request);
                return this.RedirectToAction("Index");
            }

            this.ModelState.AddModelError("Resolution", Resources.ValidationMessageResolutionRequired);
            return this.View(model);
        }

        /// <summary>
        /// Accepts meeting.
        /// </summary>
        /// <param name="id">Id of the request for which accept meeting</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> AcceptMeeting(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Pending);
            await requestManager.AcceptMeetingAsync(claimPrincipal, meeting);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Decline meeting.
        /// </summary>
        /// <param name="id">Id of the request for which decline meeting</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> DeclineMeeting(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Pending);
            await requestManager.DeclineMeetingAsync(claimPrincipal, meeting);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// View meeting attendees.
        /// </summary>
        /// <param name="id">Id of the request for which view meeting members</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> ViewAttendees(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var meeting = request.Meetings.FirstOrDefault();
            var model = new ViewAttendeesViewModel();
            model.Id = request.Id;
            model.Title = request.Title;
            model.Attendees = meeting.MeetingAttendees;
            return this.View(model);
        }

        /// <summary>
        /// View meeting room.
        /// </summary>
        /// <param name="id">Id of the request for which view meeting room</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> MeetingRoom(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Started
                || _.Status == (int)MeetingStatus.Voting);
            var model = new MeetingRoomViewModel();
            model.Id = request.Id;
            model.Title = request.Title;
            model.MeetingId = meeting.Id;
            model.Attendees = meeting.MeetingAttendees;
            model.IsChatEnabled = meeting.Status == (int)MeetingStatus.Started;
            model.VotingStarted = meeting.Status == (int)MeetingStatus.Voting;
            var userId = claimPrincipal.FindFirst(ClaimTypes.Sid).Value;
            var attendee = meeting.MeetingAttendees.FirstOrDefault(_ => _.UserId == userId);
            if (attendee != null) 
            {
                model.VotePlaced = attendee.Vote != null;
            }

            return this.View(model);
        }

        /// <summary>
        /// Decline meeting.
        /// </summary>
        /// <param name="id">Id of the request for which decline meeting</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> StartVoting(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            await requestManager.StartVotingAsync(claimPrincipal, request);
            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// Bing accept vote.
        /// </summary>
        /// <param name="id">Id of the request for which vote bind</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> VoteAccept(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Voting);
            await requestManager.VoteAcceptAsync(claimPrincipal, meeting);
            return this.RedirectToAction("MeetingRoom", new { id = id });
        }

        /// <summary>
        /// Bing accept vote.
        /// </summary>
        /// <param name="id">Id of the request for which vote bind</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> VoteAbstain(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Voting);
            await requestManager.VoteAbstainAsync(claimPrincipal, meeting);
            return this.RedirectToAction("MeetingRoom", new { id = id });
        }

        /// <summary>
        /// Bing reject vote.
        /// </summary>
        /// <param name="id">Id of the request for which vote bind</param>
        /// <returns>Task which returns result of the action.</returns>
        public async Task<ActionResult> VoteReject(int id)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var requestManager = context.Get<RequestManager>();
            var request = await requestManager.FindByIdAsync(claimPrincipal, id);
            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Voting);
            await requestManager.VoteRejectAsync(claimPrincipal, meeting);
            return this.RedirectToAction("MeetingRoom", new { id = id });
        }

        /// <summary>
        /// Gets committee members.
        /// </summary>
        /// <param name="committeeId">Id of the committee members.</param>
        /// <returns>List of committee members.</returns>
        private IQueryable<SelectListItem> GetCommitteeMembers(int committeeId)
        {
            var claimPrincipal = (System.Security.Claims.ClaimsPrincipal)this.User;
            var context = HttpContext.GetOwinContext();
            var userManager = context.Get<ApplicationUserManager>();
            return from m in userManager.GetCommitteeMembers(claimPrincipal, committeeId)
                   select new SelectListItem()
                   {
                       Text = m.LastName + " " + m.FirstName,
                       Value = m.Id,
                       Selected = true
                   };
        }
    }
}