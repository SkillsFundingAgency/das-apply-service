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

        [HttpGet]
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
                return View("~/Views/Roatp/ManagementHierarchy/AddManagementHierarchy.cshtml", model);
            }

            var managementHierarchySection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy);

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
                   model.IsPartOfOtherOrgThatGetsFunding,
                   model.IsPartOfOtherOrgThatGetsFunding=="Yes"? model.OtherOrgName : string.Empty
                }
            };

            if (managementHierarchyData == null)
            {
                managementHierarchyData = new TabularData
                {
                    HeadingTitles = new List<string> { "Name", "Job role","Years in role","Months in role","Part of another organisation","Organisation details"},
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
                var result = await _tabularDataRepository.UpsertTabularDataRecord(
                    model.ApplicationId,
                    managementHierarchySection.Id,
                    RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy,
                    RoatpDeliveringApprenticeshipTrainingQuestionIdConstants.ManagementHierarchy,
                    RoatpWorkflowQuestionTags.AddManagementHierarchy,
                    managementHierarchyPerson);
            }

            return RedirectToAction("ConfirmManagementHierarchy", new { model.ApplicationId });
        }


        [HttpPost]
        public async Task<IActionResult> RemoveManagementHierarchy(ConfirmRemoveManagementHierarchyViewModel model)
        {
            return await RemoveItemFromManagementHierarchy(
                model,
                RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy,
                RoatpDeliveringApprenticeshipTrainingQuestionIdConstants.ManagementHierarchy,
                RoatpWorkflowQuestionTags.AddManagementHierarchy,
                "ConfirmManagementHierarchy",
                model.BackAction);
        }

        private async Task<IActionResult> RemoveItemFromManagementHierarchy(ConfirmRemoveManagementHierarchyViewModel model, string pageId, string questionId, string questionTag, string redirectAction, string backAction)
        {
            if (String.IsNullOrEmpty(model.Confirmation))
            {
                model.ErrorMessages = new List<ValidationErrorDetail>
                {
                    new ValidationErrorDetail
                    {
                        Field = "Confirmation",
                        ErrorMessage = $"Tell us if you want to remove {model.Name}"
                    }
                };
                model.BackAction = backAction;

                return View("~/Views/Roatp/ManagementHierarchy/ConfirmManagementHierarchyRemoval.cshtml", model);
            }

            if (model.Confirmation != "Y")
            {
                return RedirectToAction(redirectAction, new { model.ApplicationId });
            }

            var managementHierarchyData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, questionTag);

            if ((managementHierarchyData == null) || model.Index < 0 || model.Index + 1 > managementHierarchyData.DataRows.Count)
            {
                return RedirectToAction(redirectAction, new { model.ApplicationId });
            }

            managementHierarchyData.DataRows.RemoveAt(model.Index);

            var managementHierarchySection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy);

            var result = await _tabularDataRepository.SaveTabularDataAnswer(
                model.ApplicationId,
                managementHierarchySection.Id,
                pageId,
                questionId,
                managementHierarchyData);

            return RedirectToAction(redirectAction, new { model.ApplicationId });
        }

        [HttpGet]
        public async Task<IActionResult> EditManagementHierarchy(Guid applicationId, int index)
        {
            var personTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);
            if (personTableData != null)
            {
                if (index >= personTableData.DataRows.Count)
                {
                    return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
                }

                var person = personTableData.DataRows[index];
                var name = person.Columns[0];
                var jobRole = person.Columns[1];

                var timeInRoleMonths = person.Columns[3];
                var timeInRoleYears = person.Columns[2];
                var isPartOfOtherOrg = person.Columns[4];
                var otherOrgName = person.Columns[5];
                var model = new AddEditManagementHierarchyViewModel
                {
                    ApplicationId = applicationId,
                    FullName = name,
                    Index = index,
                    Identifier = "person",
                    JobRole = jobRole,
                    TimeInRoleMonths = timeInRoleMonths,
                    TimeInRoleYears = timeInRoleYears,
                    IsPartOfOtherOrgThatGetsFunding = isPartOfOtherOrg,
                    OtherOrgName = otherOrgName,
                    GetHelpAction = "EditManagementHierarchy"
                };
                PopulateGetHelpWithQuestion(model, "EditManagementHierarchy");
                return View($"~/Views/Roatp/ManagementHierarchy/EditManagementHierarchy.cshtml", model);
            }
            return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateManagementHierarchyDetails(AddEditManagementHierarchyViewModel model)
        {
            var errorMessages = ManagementHierarchyValidator.Validate(model);

            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/ManagementHierarchy/EditManagementHierarchy.cshtml", model);
            }

            var peopleTableData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);
            if (peopleTableData != null)
            {
                var person = new TabularDataRow
                {
                    Columns = new List<string>
                    {
                        model.FullName,
                        model.JobRole,
                        model.TimeInRoleYears,
                        model.TimeInRoleMonths,
                        model.IsPartOfOtherOrgThatGetsFunding,
                        model.IsPartOfOtherOrgThatGetsFunding=="Yes"? model.OtherOrgName : string.Empty
                    }
                };

                var managementHierarchySection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy);

                var result = await _tabularDataRepository.EditTabularDataRecord(
                    model.ApplicationId,
                    managementHierarchySection.Id,
                    RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy,
                    RoatpDeliveringApprenticeshipTrainingQuestionIdConstants.ManagementHierarchy,
                    RoatpWorkflowQuestionTags.AddManagementHierarchy,
                    person,
                    model.Index);
            }

            return RedirectToAction("ConfirmManagementHierarchy", new { model.ApplicationId });
        }

        [HttpGet]
        public async Task<IActionResult> RemoveManagementHierarchy(Guid applicationId, int index)
        {
            var personTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

            if (index >= personTableData.DataRows.Count)
            {
                return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
            }

            var personName = personTableData.DataRows[index].Columns[0];

            return ConfirmRemovalOfManagementHierarchy(applicationId, personName, "RemoveManagementHierarchy", "ConfirmManagementHierarchy");
        }


        [HttpPost]
        public async Task<IActionResult> CompleteManagementHierarchySection(Guid applicationId)
        {           
            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
        }

        private IActionResult ConfirmRemovalOfManagementHierarchy(Guid applicationId, string name, string actionName, string backActionName)
        {
            var model = new ConfirmRemoveManagementHierarchyViewModel
            {
                ApplicationId = applicationId,
                Name = name,
                ActionName = actionName,
                BackAction = backActionName,
                GetHelpAction = actionName
            };
            PopulateGetHelpWithQuestion(model, actionName);

            return View("~/Views/Roatp/ManagementHierarchy/ConfirmManagementHierarchyRemoval.cshtml", model);
        }
    }
}
