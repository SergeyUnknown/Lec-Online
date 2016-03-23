// -----------------------------------------------------------------------
// <copyright file="DocumentBuilder.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Reporting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using LecOnline.Core;
    using LecOnline.Reporting.Properties;
    using MigraDoc.DocumentObjectModel;
    using NPetrovich;

    /// <summary>
    /// Class which builds the documents.
    /// </summary>
    public class DocumentBuilder
    {
        /// <summary>
        /// Database context.
        /// </summary>
        private LecOnlineDbEntities dbContext;

        /// <summary>
        /// User manager.
        /// </summary>
        private ApplicationUserManager userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentBuilder"/> class.
        /// </summary>
        /// <param name="dbContext">Database context for accessing missing data.</param>
        /// <param name="userManager">User manager for accessing missing data.</param>
        public DocumentBuilder(LecOnlineDbEntities dbContext, ApplicationUserManager userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        /// <summary>
        /// Gets format provider used for strings formatting.
        /// </summary>
        public CultureInfo Formatter
        {
            get
            {
                return CultureInfo.CurrentUICulture;
            }
        }

        /// <summary>
        /// Gets document which represents the request.
        /// </summary>
        /// <param name="request">Request object for which should be created request document.</param>
        /// <returns>The created <see cref="Document"/>.</returns>
        public async Task<Document> GetRequest(Request request)
        {
            var document = new Document();
            document.Info.Title = "Заявка на рассмотрение исследования " + request.StudyCode;
            document.Info.Subject = "Заявка на рассмотрение исследования " + request.StudyCode + 
                " по исследованию " + request.Title;
            document.Info.Author = "LecOnline.ru";

            document.Styles.Normal.ParagraphFormat.SpaceAfter = new Unit(0.5, UnitType.Centimeter);
            var chairman = await this.userManager.FindByIdAsync(request.Committee.Chairman);
            var submitter = await this.userManager.FindByNameAsync(request.SubmittedBy ?? request.CreatedBy);

            var section = document.AddSection();
            this.CreateHeader(section, request.Committee, request.Client, chairman, submitter);

            this.FillRequestHeader(section.AddParagraph(), request.Committee, chairman, submitter);
            this.CreateRequestIntroduction(section, request, chairman);
            this.FillRequestMainBody(section.AddParagraph(), request);
            this.FillRequestAttachments(section.AddParagraph(), request);
            this.FillRequestSignOffSection(section, request, submitter);

            this.CreateFooter(section, request.Client);
            return document;
        }

        /// <summary>
        /// Gets document which represents the request resolution by committee.
        /// </summary>
        /// <param name="request">Request object for which should be created request document.</param>
        /// <returns>The created <see cref="Document"/>.</returns>
        public async Task<Document> GetResolution(Request request)
        {
            var document = new Document();
            document.Info.Title = "Решение по заявке на рассмотрение исследования " + request.StudyCode;
            document.Info.Subject = "Решение по заявке на рассмотрение исследования " + request.StudyCode + 
                " по исследованию " + request.Title;
            document.Info.Author = "LecOnline.ru";

            var meeting = request.Meetings.FirstOrDefault(_ => _.Status == (int)MeetingStatus.Completed
                || _.Status == (int)MeetingStatus.HasResolution);
            
            document.Styles.Normal.ParagraphFormat.SpaceAfter = new Unit(0.5, UnitType.Centimeter);
            var chairman = await this.userManager.FindByIdAsync(request.Committee.Chairman);
            var secretary = await this.userManager.FindByIdAsync(request.Committee.Secretary);
            var submitter = await this.userManager.FindByNameAsync(request.SubmittedBy ?? request.CreatedBy);

            var section = document.AddSection();
            this.CreateHeader(section, request.Committee, request.Client, chairman, submitter);

            this.FillResolutionHeader(section, request.Committee, meeting);
            await this.CreateResolutionIntroduction(section, request, meeting);
            this.CreateResolutionRequestTopic(section, request, submitter);
            this.CreateResolutionRequestAttachments(section, request);
            this.CreateMeetingTalks(section, meeting);
            this.CreateMeetingVotes(section, meeting);
            this.CreateMeetingDecision(section, meeting);

            this.AddSignOff(section, "Председатель этического комитета", chairman);
            this.AddSignOff(section, "Секретарь этического комитета", secretary);

            this.CreateFooter(section, request.Client);
            return document;
        }

        /// <summary>
        /// Fill sign-off section
        /// </summary>
        /// <param name="section">Section where to fill information about attachments.</param>
        /// <param name="request">Request with information about attachments.</param>
        /// <param name="submitter">User which submit application.</param>
        private void FillRequestSignOffSection(Section section, Request request, ApplicationUser submitter)
        {
            var paragraph = section.AddParagraph();
            paragraph.AddText("С уважением");
            paragraph.AddLineBreak();

            this.AddSignOff(section, "Главный исследователь", submitter);

            paragraph = section.AddParagraph();
            var submissionDate = request.SentDate ?? DateTime.UtcNow;
            paragraph.AddText(string.Format(this.Formatter, @"Дата: {0}", submissionDate.ToString("dd MMMM yyyy", this.Formatter)));
        }

        /// <summary>
        /// Add line where user could sign document.
        /// </summary>
        /// <param name="section">Section where to add signing line.</param>
        /// <param name="title">Title of the user which signature should be put on the document.</param>
        /// <param name="signer">User who have to sign document.</param>
        private void AddSignOff(Section section, string title, ApplicationUser signer)
        {
            var paragraph = section.AddParagraph();
            var messageFormat = @"{0}: {1} _____________";
            var degreeString = signer == null ? string.Empty : (signer.Degree ?? string.Empty);
            if (!string.IsNullOrWhiteSpace(degreeString))
            {
                degreeString = string.Join(string.Empty, degreeString.ToLower(this.Formatter).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(_ => _[0] + "."));
            }

            paragraph.AddText(string.Format(messageFormat, title, degreeString));
            paragraph.AddTab();
            var submitterName = signer == null ? Resources.UnknownSubmitter : signer.LastName + " " + signer.Initials;
            paragraph.AddText(submitterName);
        }

        /// <summary>
        /// Creates footer for the specific client.
        /// </summary>
        /// <param name="section">Section where footer should be added.</param>
        /// <param name="client">Client for which footer should be generated.</param>
        private void CreateFooter(Section section, Client client)
        {
            var footer = section.Footers.Primary;
            var text = footer.AddParagraph();
        }

        /// <summary>
        /// Creates header for the specific client.
        /// </summary>
        /// <param name="section">Section where header should be added.</param>
        /// <param name="committee">Committee to which request is sent.</param>
        /// <param name="client">Client for which header should be generated.</param>
        /// <param name="chairman">Chairman of the committee.</param>
        /// <param name="submitterId">Id of the user which submit application.</param>
        private void CreateHeader(Section section, Committee committee, Client client, ApplicationUser chairman, ApplicationUser submitterId)
        {
            var header = section.Headers.Primary;
            var text = section.AddParagraph();
        }

        /// <summary>
        /// Create request introduction body section
        /// </summary>
        /// <param name="section">Section where to create information about attachments.</param>
        /// <param name="request">Request with information about attachments.</param>
        /// <param name="chairman">Chairman for the committee.</param>
        private void CreateRequestIntroduction(Section section, Request request, ApplicationUser chairman)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Format.SpaceBefore = new Unit(1, UnitType.Centimeter);
            paragraph.Format.SpaceAfter = new Unit(1, UnitType.Centimeter);
            var gender = NPetrovich.Utils.GenderUtils.Detect(chairman.PatronymicName);
            var messageFormat = "Глубокоуважаем{2} {0} {1},";
            var message = string.Format(
                this.Formatter,
                messageFormat, 
                chairman.FirstName, 
                chairman.PatronymicName, 
                gender == Gender.Female ? "ая" : "ый");
            paragraph.AddFormattedText(message, TextFormat.Bold);

            paragraph = section.AddParagraph();
            paragraph.AddText("Свидетельствуем Вам свое почтение и настоящим ");
            paragraph.AddText("просим Вас разрешить проведение международного ");
            paragraph.AddText("клинического исследования препарата ");
            paragraph.AddFormattedText(string.Format("по протоколу №{0}:", request.StudyCode), TextFormat.Bold);
            paragraph.AddText(string.Format("«{0}». Фаза {1}.", request.Title, request.StudyPhase));

            paragraph = section.AddParagraph();
            paragraph.AddText("на базе учреждения: ");
            paragraph.AddFormattedText(request.StudyBase ?? string.Empty, TextFormat.Bold);
            paragraph.AddLineBreak();
            messageFormat = @"Спонсор исследования: {0}
Производитель/упаковщик: {1}
Клиническое исследование в России проводит компания: {2}
Юридический адрес: {3}
Фактический адрес: {4}
Проведение исследования одобрено: {5}
";
            message = string.Format(
                this.Formatter,
                messageFormat,
                request.StudySponsor ?? string.Empty,
                request.StudyProducer ?? string.Empty,
                request.StudyPerformer ?? string.Empty,
                request.StudyPerformerRegisteredAddress ?? string.Empty,
                request.StudyPerformerStatutoryAddress ?? string.Empty,
                request.StudyApprovedBy ?? string.Empty);
            paragraph.AddText(message);
        }

        /// <summary>
        /// Fill main request body section
        /// </summary>
        /// <param name="paragraph">Paragraph where to fill information about attachments.</param>
        /// <param name="request">Request with information about attachments.</param>
        private void FillRequestMainBody(Paragraph paragraph, Request request)
        {
            paragraph.AddFormattedText("Описание цели клинического исследования в соотвествии с ФЗ \"Об обращении лекарственных средств\":", TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddText(request.Description ?? string.Empty);
            paragraph.AddLineBreak();
            
            paragraph.AddLineBreak();
            var dateFormat = "MMMM yyyy";
            var messageFormat = @"Планируемое начало иследований в России - {0}
Планируемое окончание иследований в России - {1}
Планируемая длительность иследований в России - {2} дней
Планируемое количество пациентов в России - {3}
Планируемое количество рандомизированных пациентов в России - {4}
Планируемое количество медицинских организаций участников исследований в России - {5}
";
            var message = string.Format(
                this.Formatter,
                messageFormat,
                DateTime.UtcNow.ToString(dateFormat, this.Formatter),
                DateTime.UtcNow.AddDays(request.PlannedDuration).ToString(dateFormat, this.Formatter),
                request.PlannedDuration,
                request.PatientsCount,
                request.RandomizedPatientsCount,
                request.CentersQty);
            paragraph.AddText(message);
            paragraph.AddLineBreak();
            paragraph.AddText(@"Просим выдать разрешенне на проведение клинического исследования с приложением
результатов этической экспертизы.
");
            paragraph.AddLineBreak();
        }
        
        /// <summary>
        /// Fill attachments section
        /// </summary>
        /// <param name="paragraph">Paragraph where to fill information about attachments.</param>
        /// <param name="request">Request with information about attachments.</param>
        private void FillRequestAttachments(Paragraph paragraph, Request request)
        {
            paragraph.AddFormattedText("К настоящему письму прилагаются:", TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            var i = 1;
            var docs = request.RequestDocumentations.Select(x => x.Name).Distinct();
            foreach (var doc in docs)
            {
                paragraph.AddText(string.Format("{0} {1}", i, doc));
                paragraph.AddLineBreak();
                i++;
            }

            if (i == 0)
            {
                paragraph.AddText(Resources.NoAtachmentProvidedForRequest);
                paragraph.AddLineBreak();
            }
        }

        /// <summary>
        /// Fill paragraph with request header.
        /// </summary>
        /// <param name="text">Paragraph to fill.</param>
        /// <param name="committee">Committee for which prepare request header.</param>
        /// <param name="chairman">Chairman of the committee.</param>
        /// <param name="submitter">Id of the submitter.</param>
        private void FillRequestHeader(Paragraph text, Committee committee, ApplicationUser chairman, ApplicationUser submitter)
        {
            text.Format.Alignment = ParagraphAlignment.Right;
            var chairmanName = chairman.ShortFullName;
            var submitterName = submitter == null ? Resources.UnknownSubmitter : submitter.ShortFullName;
            var messageFormat = @"Председателю Локального Этического Комитета
при {0}
{1}

От главного исследователя
{2}
Дата {3:D}";
            var message = string.Format(this.Formatter, messageFormat, committee.Name, chairmanName, submitterName, DateTime.UtcNow);
            text.AddText(message);
        }

        /// <summary>
        /// Header for the resolution.
        /// </summary>
        /// <param name="section">Section where put resolution header.</param>
        /// <param name="committee">Committee which perform meeting.</param>
        /// <param name="meeting">Meeting where resolution was happens.</param>
        private void FillResolutionHeader(Section section, Committee committee, Meeting meeting)
        {
            var text = section.AddParagraph();
            var messageFormat = @"Выписка №1 из протокола заседания №{1} Комитета по этике {0}";
            var message = string.Format(this.Formatter, messageFormat, committee.Name, meeting.Id);
            text.AddText(message);

            text = section.AddParagraph();
            text.Format.TabStops.AddTabStop(new Unit(16, UnitType.Centimeter), TabAlignment.Right);
            text.AddText(committee.City ?? string.Empty);
            text.AddTab();
            text.AddText(DateTime.UtcNow.ToString("dd MMMM yyyy", this.Formatter));
            section.AddParagraph();
        }

        /// <summary>
        /// Creates resolution introduction.
        /// </summary>
        /// <param name="section">Section to which add introduction.</param>
        /// <param name="request">Information about request.</param>
        /// <param name="meeting">Information about meeting held.</param>
        /// <returns>Asynchronous task which performs operation.</returns>
        private async Task CreateResolutionIntroduction(Section section, Request request, Meeting meeting)
        {
            var text = section.AddParagraph();
            text.AddText("3аседание состоялось с использованием системы связи и документооборота LЕС-опliпе.ru.");
            
            text = section.AddParagraph();
            text.AddFormattedText("Присутсвовали:", TextFormat.Bold | TextFormat.Underline);
            
            text = section.AddParagraph();
            var attendees = await this.GetMeetingAttendees(request.Committee, meeting);
            foreach (var attendee in attendees.OrderBy(_ => _.Item3))
            {
                var name = attendee.Item1;
                var status = attendee.Item2;
                text.AddText(name + " \u2013 " + status);
                text.AddLineBreak();
            }
        }

        /// <summary>
        /// Create part which describe topic of request.
        /// </summary>
        /// <param name="section">Section where to put information.</param>
        /// <param name="request">Request with information.</param>
        /// <param name="submitter">User who submit application.</param>
        private void CreateResolutionRequestTopic(Section section, Request request, ApplicationUser submitter)
        {
            var text = section.AddParagraph();
            text.AddFormattedText("Рассматривали:", TextFormat.Bold | TextFormat.Underline);

            text = section.AddParagraph();
            var messageFormat = @"Вопрос об одобрении международного клинического исследования препарата по протоколу {0}:";
            var message = string.Format(
                messageFormat,
                request.StudyCode);
            text.AddText(message);
            text.AddFormattedText(request.Title, TextFormat.Italic);
            messageFormat = " {0} на базе {1} (главный исследователь {2}).";
            message = string.Format(
                messageFormat,
                request.StudyPhase,
                request.StudyBase,
                submitter.LastName + " " + submitter.Initials);
            text.AddText(message);
        }

        /// <summary>
        /// Create part which describe talks which was on meeting.
        /// </summary>
        /// <param name="section">Section where to put information.</param>
        /// <param name="meeting">Meeting for which information should be displayed.</param>
        private void CreateMeetingTalks(Section section, Meeting meeting)
        {
            var text = section.AddParagraph();
            text.AddFormattedText("Выступили:", TextFormat.Bold | TextFormat.Underline);

            text = section.AddParagraph();
            text.AddText(meeting.MeetingLog);
        }

        /// <summary>
        /// Create part which describe voting results on the given meeting.
        /// </summary>
        /// <param name="section">Section where to put information.</param>
        /// <param name="meeting">Meeting for which information should be displayed.</param>
        private void CreateMeetingVotes(Section section, Meeting meeting)
        {
            var text = section.AddParagraph();
            text.AddFormattedText("Результаты голосования:", TextFormat.Bold | TextFormat.Underline);

            text = section.AddParagraph();
            var positiveVoteBinding = meeting.MeetingAttendees.Count(_ => _.Vote == (int)VoteStatus.AcceptStudy);
            var negativeVoteBinding = meeting.MeetingAttendees.Count(_ => _.Vote == (int)VoteStatus.RejectStudy);
            var abstainVoteBinding = meeting.MeetingAttendees.Count(_ => _.Vote == (int)VoteStatus.Abstain);
            text.AddText("Одобрение исследования: " + positiveVoteBinding);
            text.AddLineBreak();
            text.AddText("Запрещение исследования: " + negativeVoteBinding);
            text.AddLineBreak();
            text.AddText("Воздержались: " + abstainVoteBinding);
        }

        /// <summary>
        /// Create part which describe decision which was made on the given meeting.
        /// </summary>
        /// <param name="section">Section where to put information.</param>
        /// <param name="meeting">Meeting for which information should be displayed.</param>
        private void CreateMeetingDecision(Section section, Meeting meeting)
        {
            var text = section.AddParagraph();
            text.AddFormattedText("Постановили:", TextFormat.Bold | TextFormat.Underline);

            text = section.AddParagraph();
            text.AddText(meeting.Protocol);
        }

        /// <summary>
        /// Creates section inside resolution where list of attached documents are presented.
        /// </summary>
        /// <param name="section">Section where to fill information about attachments.</param>
        /// <param name="request">Request with information about attachments.</param>
        private void CreateResolutionRequestAttachments(Section section, Request request)
        {
            var paragraph = section.AddParagraph();
            paragraph.AddFormattedText("Предоставленные документы:", TextFormat.Bold | TextFormat.Underline);
            
            paragraph = section.AddParagraph();
            var i = 1;
            var docsNames = request.RequestDocumentations.Select(x => x.Name).Distinct();
            foreach (var doc in docsNames)
            {
                paragraph.AddText(string.Format("{0} {1}", i, doc));
                paragraph.AddLineBreak();
                i++;
            }

            if (i == 0)
            {
                paragraph.AddText(Resources.NoAtachmentProvidedForRequest);
                paragraph.AddLineBreak();
            }
        }

        /// <summary>
        /// Gets sequence of the attendees.
        /// </summary>
        /// <param name="committee">Committee which held the meeting.</param>
        /// <param name="meeting">Meeting for which get attendees.</param>
        /// <returns>Task which returns information about meeting attendees.</returns>
        private Task<Tuple<string, string, int>[]> GetMeetingAttendees(Committee committee, Meeting meeting)
        {
            var ass = meeting.MeetingAttendees.Where(_ => _.Status == (int)AttendanceStatus.InvitationAccepted).ToList();
            return Task.WhenAll(from a in ass select this.ConvertAttendeeItem(committee, a));
        }

        /// <summary>
        /// Convert attendee item.
        /// </summary>
        /// <param name="committee">Committee which held the meeting.</param>
        /// <param name="attendee">Attendee for which short representation should be constructed.</param>
        /// <returns>Task which returns short information about user.</returns>
        private Task<Tuple<string, string, int>> ConvertAttendeeItem(Committee committee, MeetingAttendee attendee)
        {
            var user = this.userManager.FindByIdAsync(attendee.UserId).Result;
            var name = user.LastName + " " + user.Initials;
            var status = Resources.EthicalCommitteeMember;
            var sortOrder = int.MaxValue;
            if (committee.Chairman == user.Id)
            {
                sortOrder = 0;
                status = Resources.Chairman;
            } 
            else if (committee.ViceChairman == user.Id)
            {
                sortOrder = 1;
                status = Resources.ViceChairman;
            }
            else if (committee.Secretary == user.Id)
            {
                sortOrder = 2;
                status = Resources.Secretary;
            }

            return Task.FromResult(Tuple.Create(name, status, sortOrder));
        }
    }
}
