// -----------------------------------------------------------------------
// <copyright file="RequestBaseInformationViewModel.cs" company="MDP-Soft">
// Copyright (c) MDP-Soft. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LecOnline.Models.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using LecOnline.Core;
    using LecOnline.Mvc;
    using LecOnline.Properties;

    /// <summary>
    /// Base information about request.
    /// </summary>
    public class RequestBaseInformationViewModel
    {
        /// <summary>
        /// Gets or sets id of the client to which this request belongs.
        /// </summary>
        [Display(Name = "FieldClient", ResourceType = typeof(Resources))]
        [UIHint("Client")]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets id of the committee to which this request belongs.
        /// </summary>
        [Display(Name = "FieldCommittee", ResourceType = typeof(Resources))]
        [UIHint("Committee")]
        public int? CommitteeId { get; set; }

        /// <summary>
        /// Gets or sets title for the request.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldTitle", Prompt = "FieldTitle", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [HelpPopup(HelpText = "HelpTextTitle", ResourceType = typeof(Resources))]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description for the request.
        /// </summary>
        [Display(Name = "FieldDescription", Prompt = "FieldDescription", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [HelpPopup(HelpText = "HelpTextDescription", ResourceType = typeof(Resources))]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets description of the earlier studies.
        /// </summary>
        [Display(Name = "FieldEarlierStudy", Prompt = "FieldEarlierStudy", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [HelpPopup(HelpText = "HelpEarlierStudy", ResourceType = typeof(Resources))]
        public string EarlierStudy { get; set; }

        /// <summary>
        /// Gets or sets description of the population in the study.
        /// </summary>
        [Display(Name = "FieldPopulationDescription", Prompt = "FieldPopulationDescription", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [HelpPopup(HelpText = "HelpPopulationDescription", ResourceType = typeof(Resources))]
        public string PopulationDescription { get; set; }

        /// <summary>
        /// Gets or sets description of the therapy methods in the study.
        /// </summary>
        [Display(Name = "FieldTherapyDescription", Prompt = "FieldTherapyDescription", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [HelpPopup(HelpText = "HelpTherapyDescription", ResourceType = typeof(Resources))]
        public string TherapyDescription { get; set; }

        /// <summary>
        /// Gets or sets other international studies related to that study.
        /// </summary>
        [Display(Name = "FieldInternationStudies", Prompt = "FieldInternationStudies", ResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [HelpPopup(HelpText = "HelpInternationStudies", ResourceType = typeof(Resources))]
        public string InternationStudies { get; set; }

        /// <summary>
        /// Gets or sets code for the study.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldStudyCode", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyCode", ResourceType = typeof(Resources))]
        public string StudyCode { get; set; }

        /// <summary>
        /// Gets or sets type of the study.
        /// </summary>
        [Display(Name = "FieldStudyType", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyType", ResourceType = typeof(Resources))]
        public int StudyType { get; set; }

        /// <summary>
        /// Gets or sets code for the study.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldStudyPhase", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyPhase", ResourceType = typeof(Resources))]
        [UIHint("StudyPhase")]
        public string StudyPhase { get; set; }

        /// <summary>
        /// Gets or sets count of centers in which study performed.
        /// </summary>
        [Range(1, 500)]
        [Display(Name = "FieldCentersQty", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextCentersQty", ResourceType = typeof(Resources))]
        public int CentersQty { get; set; }

        /// <summary>
        /// Gets or sets count of centers in which study performed.
        /// </summary>
        [Range(1, 500)]
        [Display(Name = "FieldLocalCentersQty", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextLocalCentersQty", ResourceType = typeof(Resources))]
        public int LocalCentersQty { get; set; }

        /// <summary>
        /// Gets or sets approximate duration of the study.
        /// </summary>
        [Display(Name = "FieldPlannedDuration", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextPlannedDuration", ResourceType = typeof(Resources))]
        public int PlannedDuration { get; set; }

        /// <summary>
        /// Gets or sets approximate date study's start date.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldStudyPlannedStartDate", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyPlannedStartDate", ResourceType = typeof(Resources))]
        [DataType(DataType.Date)]
        public DateTime? StudyPlannedStartDate { get; set; }

        /// <summary>
        /// Gets or sets approximate date study's end date.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationMessageRequired", ErrorMessageResourceType = typeof(Resources), ErrorMessage = null)]
        [Display(Name = "FieldStudyPlannedEndDate", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyPlannedEndDate", ResourceType = typeof(Resources))]
        [DataType(DataType.Date)]
        public DateTime? StudyPlannedFinishDate { get; set; }

        /// <summary>
        /// Gets or sets approximate count of patients which would be involved in the study.
        /// </summary>
        [Display(Name = "FieldPatientsCount", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextPatientsCount", ResourceType = typeof(Resources))]
        public int PatientsCount { get; set; }

        /// <summary>
        /// Gets or sets approximate count of randomized patients which would be involved in the study.
        /// </summary>
        [Display(Name = "FieldRandomizedPatientsCount", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextRandomizedPatientsCount", ResourceType = typeof(Resources))]
        public int RandomizedPatientsCount { get; set; }

        /// <summary>
        /// Gets or sets name of the company where study would be taken4.
        /// </summary>
        [Display(Name = "FieldStudyBase", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyBase", ResourceType = typeof(Resources))]
        public string StudyBase { get; set; }

        /// <summary>
        /// Gets or sets name of the company who is sponsoring study.
        /// </summary>
        [Display(Name = "FieldStudySponsor", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudySponsor", ResourceType = typeof(Resources))]
        public string StudySponsor { get; set; }

        /// <summary>
        /// Gets or sets name of the company who is producing or packaging drugs.
        /// </summary>
        [Display(Name = "FieldStudyProducer", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyProducer", ResourceType = typeof(Resources))]
        public string StudyProducer { get; set; }

        /// <summary>
        /// Gets or sets name of the company who is executing clinical trial.
        /// </summary>
        [Display(Name = "FieldStudyPerformer", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyPerformer", ResourceType = typeof(Resources))]
        public string StudyPerformer { get; set; }

        /// <summary>
        /// Gets or sets statutory address of the company who is executing clinical trial.
        /// </summary>
        [Display(Name = "FieldStudyPerformerStatutoryAddress", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyPerformerStatutoryAddress", ResourceType = typeof(Resources))]
        public string StudyPerformerStatutoryAddress { get; set; }

        /// <summary>
        /// Gets or sets registered address of the company who is executing clinical trial.
        /// </summary>
        [Display(Name = "FieldStudyPerformerRegisteredAddress", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyPerformerRegisteredAddress", ResourceType = typeof(Resources))]
        public string StudyPerformerRegisteredAddress { get; set; }

        /// <summary>
        /// Gets or sets name of the document which is approve study.
        /// </summary>
        [Display(Name = "FieldStudyApprovedBy", ResourceType = typeof(Resources))]
        [HelpPopup(HelpText = "HelpTextStudyApprovedBy", ResourceType = typeof(Resources))]
        public string StudyApprovedBy { get; set; }

        /// <summary>
        /// Gets or sets type of request.
        /// </summary>
        public RequestType RequestType { get; set; }
    }
}
