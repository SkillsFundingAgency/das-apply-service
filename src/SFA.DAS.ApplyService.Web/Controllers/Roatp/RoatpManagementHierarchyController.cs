using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.ManagementHierarchy;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    public class RoatpManagementHierarchyController  : RoatpApplyControllerBase
    {
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IAnswerFormService _answerFormService;
        private readonly ITabularDataRepository _tabularDataRepository;

        public RoatpManagementHierarchyController(ISessionService sessionService, IQnaApiClient qnaApiClient, IApplicationApiClient applicationApiClient, IAnswerFormService answerFormService, ITabularDataRepository tabularDataRepository) : base(sessionService)
        {
            _qnaApiClient = qnaApiClient;
            _applicationApiClient = applicationApiClient;
            _answerFormService = answerFormService;
            _tabularDataRepository = tabularDataRepository;
        }

        [Route("management-hierarchy")]
        public async Task<IActionResult> StartPage(Guid applicationId)
        {
            var peopleData = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

            if (peopleData != null && peopleData.Value != null)
            {
                return await ConfirmManagementHierarchy(applicationId);
            }

            return AddManagementHierarchy(applicationId);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmManagementHierarchy(Guid applicationId)
        {
            var ManagementHierarchyData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

            if (ManagementHierarchyData == null || ManagementHierarchyData.DataRows == null || ManagementHierarchyData.DataRows.Count == 0)
            {
                return RedirectToAction("AddManagementHierarchy", new { applicationId });
            }

            var model = new ConfirmManagementHierarchyViewModel { ApplicationId = applicationId, ManagementHierarchyData = ManagementHierarchyData };
            PopulateGetHelpWithQuestion(model, "ConfirmManagementHierarchy");

            return View("~/Views/Roatp/ManagementHierarchy/ConfirmManagementHierarchy.cshtml", model);
        }



        [HttpGet]
        public IActionResult AddManagementHierarchy(Guid applicationId)
        {
            var model = new AddEditManagementHierarchyViewModel { ApplicationId = applicationId, GetHelpAction = "AddManagementHierarchy" };
            
            //MFCMFC not sure about the variable being injected here.....
            PopulateGetHelpWithQuestion(model, RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy);

            return View("~/Views/Roatp/ManagementHierarchy/AddManagementHierarchy.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddManagementHierarchyDetails(AddEditManagementHierarchyViewModel model)
        {
            var errorMessages = ManagementHierarchyValidator.Validate(model);

            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/AddManagementHierarchy.cshtml", model);
            }

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var managementHierarchySection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy);

            var managementHierarchyData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

            var managementHierarchyPerson = new TabularDataRow
            {
                Id = Guid.NewGuid().ToString(),
                Columns = new List<string>
                {
                    model.FullName,
                   model.JobRole,
                   model.TimeInRoleYears,
                   model.TimeInRoleMonths,
                   model.IsPartOfOtherOrgThatGetsFunding.ToString(),
                   model.OtherOrgName
                }
            };

            if (managementHierarchyData == null)
            {
                managementHierarchyData = new TabularData
                {
                    HeadingTitles = new List<string> { "Name", "Job role"},
                    DataRows = new List<TabularDataRow>
                    {
                        managementHierarchyPerson
                    }
                };

                var result = await _tabularDataRepository.SaveTabularDataAnswer(
                    model.ApplicationId,
                    managementHierarchySection.Id,
                    RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy,
                    RoatpDeliveringApprenticeshipTrainingQuestionIdConstants.ManagementHierarchy,
                    managementHierarchyData);
            }
            else
            {
                var result = await _tabularDataRepository.AddTabularDataRecord(
                    model.ApplicationId,
                    managementHierarchySection.Id,
                    RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy,
                    RoatpDeliveringApprenticeshipTrainingQuestionIdConstants.ManagementHierarchy,
                    RoatpWorkflowQuestionTags.AddManagementHierarchy,
                    managementHierarchyPerson);
            }

            return RedirectToAction("ConfirmManagementHierarchy", new { model.ApplicationId });
        }

        //[HttpGet]
        //public async Task<IActionResult> EditManagementHierarchy(Guid applicationId, int index)
        //{
        //    var personTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);
        //    if (personTableData != null)
        //    {
        //        if (index >= personTableData.DataRows.Count)
        //        {
        //            return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
        //        }

        //        var person = personTableData.DataRows[index];
        //        var name = person.Columns[0];
        //        var dateOfBirth = person.Columns[1];
        //        var model = new AddEditManagementHierarchyViewModel
        //        {
        //            ApplicationId = applicationId,
        //            PersonInControlName = name,
        //            Index = index,
        //            Identifier = "person",
        //            PersonInControlDobMonth = DateOfBirthFormatter.GetMonthNumberFromShortDateOfBirth(dateOfBirth),
        //            PersonInControlDobYear = DateOfBirthFormatter.GetYearFromShortDateOfBirth(dateOfBirth),
        //            DateOfBirthOptional = false,
        //            GetHelpAction = "EditManagementHierarchy"
        //        };
        //        PopulateGetHelpWithQuestion(model, "EditManagementHierarchy");
        //        return View($"~/Views/Roatp/WhosInControl/EditManagementHierarchy.cshtml", model);
        //    }
        //    return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateManagementHierarchyDetails(AddEditManagementHierarchyViewModel model)
        //{
        //    var errorMessages = ManagementHierarchyValidator.Validate(model);

        //    if (errorMessages.Any())
        //    {
        //        model.ErrorMessages = errorMessages;
        //        return View("~/Views/Roatp/WhosInControl/EditManagementHierarchy.cshtml", model);
        //    }

        //    var peopleTableData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);
        //    if (peopleTableData != null)
        //    {
        //        var person = new TabularDataRow
        //        {
        //            Columns = new List<string>
        //            {
        //                model.PersonInControlName,
        //                DateOfBirthFormatter.FormatDateOfBirth(model.PersonInControlDobMonth, model.PersonInControlDobYear)
        //            }
        //        };

        //        var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
        //        var yourOrganisationSequence =
        //            applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
        //        var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
        //        var whosInControlSection =
        //            yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

        //        var result = await _tabularDataRepository.EditTabularDataRecord(
        //            model.ApplicationId,
        //            whosInControlSection.Id,
        //            RoatpWorkflowPageIds.WhosInControl.AddManagementHierarchy,
        //            RoatpYourOrganisationQuestionIdConstants.AddManagementHierarchy,
        //            RoatpWorkflowQuestionTags.AddManagementHierarchy,
        //            person,
        //            model.Index);
        //    }

        //    return RedirectToAction("ConfirmManagementHierarchy", new { model.ApplicationId });
        //}

        //[HttpGet]
        //public async Task<IActionResult> RemoveManagementHierarchy(Guid applicationId, int index)
        //{
        //    var personTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

        //    if (index >= personTableData.DataRows.Count)
        //    {
        //        return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
        //    }

        //    var personName = personTableData.DataRows[index].Columns[0];

        //    return ConfirmRemovalOfPersonInControl(applicationId, personName, "RemovePscDetails", "ConfirmManagementHierarchy");
        //}


        [HttpPost]
        public async Task<IActionResult> CompleteManagementHierarchySection(Guid applicationId)
        {
            // tech debt - this will be reworked by the changes to the QnA config JSON and method
            // for determining that application section completed is derived (APR-1008)

            var applicationSequences = await _qnaApiClient.GetSequences(applicationId);
            var deliveringApprenticeshipTrainingSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining);
            var deliveringApprenticeshipTrainingSections = await _qnaApiClient.GetSections(applicationId, deliveringApprenticeshipTrainingSequence.Id);
            var managemetnHierarchySection
                =
                deliveringApprenticeshipTrainingSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy);

            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
        }

        //private IActionResult ConfirmRemovalOfManagementHierarchy(Guid applicationId, string name, string actionName, string backActionName)
        //{
        //    var model = new ConfirmRemoveManagementHierarchyViewModel
        //    {
        //        ApplicationId = applicationId,
        //        Name = name,
        //        ActionName = actionName,
        //        BackAction = backActionName,
        //        GetHelpAction = actionName
        //    };
        //    PopulateGetHelpWithQuestion(model, actionName);

        //    return View("~/Views/Roatp/WhosInControl/ConfirmPscRemoval.cshtml", model);
        //}
    }
}
