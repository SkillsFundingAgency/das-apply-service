using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.ManagementHierarchy;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize(Policy = "AccessInProgressApplication")]
    public class RoatpManagementHierarchyController  : RoatpApplyControllerBase
    {
        private readonly IQnaApiClient _qnaApiClient;
        private readonly ITabularDataRepository _tabularDataRepository;

        public RoatpManagementHierarchyController(ISessionService sessionService, IQnaApiClient qnaApiClient, ITabularDataRepository tabularDataRepository) : base(sessionService)
        {
            _qnaApiClient = qnaApiClient;
            _tabularDataRepository = tabularDataRepository;
        }

        [HttpGet]
        [Route("management-hierarchy")]
        public async Task<IActionResult> StartPage(Guid applicationId)
        {
            var peopleData = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

            if (peopleData?.Value != null)
            {
                return await ConfirmManagementHierarchy(applicationId);
            }

            return AddManagementHierarchy(applicationId);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmManagementHierarchy(Guid applicationId)
        {
            var managementHierarchyData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

            if (managementHierarchyData?.DataRows == null || managementHierarchyData.DataRows.Count == 0)
            {
                return RedirectToAction("AddManagementHierarchy", new { applicationId });
            }

            var model = new ConfirmManagementHierarchyViewModel { ApplicationId = applicationId, ManagementHierarchyData = managementHierarchyData, GetHelpAction = "ConfirmManagementHierarchy" };
            PopulateGetHelpWithQuestion(model, "ConfirmManagementHierarchy");

            return View("~/Views/Roatp/ManagementHierarchy/ConfirmManagementHierarchy.cshtml", model);
        }

        [HttpGet]
        [ModelStatePersist(ModelStatePersist.RestoreEntry)]
        public IActionResult AddManagementHierarchy(Guid applicationId)
        {
            var model = new AddEditManagementHierarchyViewModel { ApplicationId = applicationId, Title = "Who is in your organisation's management hierarchy for apprenticeships?", GetHelpAction = "AddManagementHierarchy" };

            PopulateGetHelpWithQuestion(model, RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy);
            return View("~/Views/Roatp/ManagementHierarchy/AddManagementHierarchy.cshtml", model);
        }

        [HttpPost]
        [ModelStatePersist(ModelStatePersist.Store)]
        public async Task<IActionResult> AddManagementHierarchyDetails(AddEditManagementHierarchyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddManagementHierarchy", new {model.ApplicationId});
            }

            var managementHierarchySection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy);

            var managementHierarchyData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

            var managementHierarchyPerson = new TabularDataRow
            {
                Id = Guid.NewGuid().ToString(),
                Columns = new List<string>
                {
                   model.FirstName,
                   model.LastName,
                   model.JobRole,
                   model.TimeInRoleYears,
                   model.TimeInRoleMonths,
                   model.IsPartOfOtherOrgThatGetsFunding,
                   model.IsPartOfOtherOrgThatGetsFunding == "Yes" ? model.OtherOrgName : string.Empty,
                   model.DobMonth,
                   model.DobYear,
                   model.Email,
                   model.ContactNumber
                }
            };

            if (managementHierarchyData == null)
            {
                managementHierarchyData = new TabularData
                {
                    HeadingTitles = new List<string> { "First Name", "Last Name","Job role","Years in role","Months in role","Part of another organisation","Organisation details","Month","Year","Email","Contact number"},
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
            if (string.IsNullOrEmpty(model.Confirmation))
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
        [ModelStatePersist(ModelStatePersist.RestoreEntry)]
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

                var firstName = person.Columns[0];
                var lastName = person.Columns[1];
                var jobRole = person.Columns[2];

                var timeInRoleMonths = person.Columns[4];
                var timeInRoleYears = person.Columns[3];
                var isPartOfOtherOrg = person.Columns[5];
                var otherOrgName = person.Columns[6];
                var dobMonth = person.Columns[7];
                var dobYear = person.Columns[8];
                var email = person.Columns[9];
                var contactNumber = person.Columns[10];

                var model = new AddEditManagementHierarchyViewModel
                {
                    ApplicationId = applicationId,
                    FirstName = firstName,
                    LastName = lastName,
                    Index = index,
                    Identifier = "person",
                    JobRole = jobRole,
                    TimeInRoleMonths = timeInRoleMonths,
                    TimeInRoleYears = timeInRoleYears,
                    IsPartOfOtherOrgThatGetsFunding = isPartOfOtherOrg,
                    OtherOrgName = otherOrgName,
                    DobMonth = dobMonth,
                    DobYear = dobYear,
                    Email = email,
                    ContactNumber = contactNumber,
                    Title = "Enter the person's details",
                    GetHelpAction = "EditManagementHierarchy"
                };

                PopulateGetHelpWithQuestion(model, "EditManagementHierarchy");

                return View($"~/Views/Roatp/ManagementHierarchy/EditManagementHierarchy.cshtml", model);
            }
            return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
        }

        [HttpPost]
        [ModelStatePersist(ModelStatePersist.Store)]
        public async Task<IActionResult> UpdateManagementHierarchyDetails(AddEditManagementHierarchyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("EditManagementHierarchy", model);
            }

            var peopleTableData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);
            if (peopleTableData != null)
            {
                var person = new TabularDataRow
                {
                    Columns = new List<string>
                    {
                        model.FirstName,
                        model.LastName,
                        model.JobRole,
                        model.TimeInRoleYears,
                        model.TimeInRoleMonths,
                        model.IsPartOfOtherOrgThatGetsFunding,
                        model.IsPartOfOtherOrgThatGetsFunding=="Yes"? model.OtherOrgName : string.Empty,
                        model.DobMonth,
                        model.DobYear,
                        model.Email,
                        model.ContactNumber
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

            var personName = personTableData.DataRows[index].Columns[0] + " " + personTableData.DataRows[index].Columns[1];

            return ConfirmRemovalOfManagementHierarchy(applicationId, personName, "RemoveManagementHierarchy", "ConfirmManagementHierarchy");
        }

        [HttpPost]
        public IActionResult CompleteManagementHierarchySection(Guid applicationId)
        {
            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId }, "Sequence_7");
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
