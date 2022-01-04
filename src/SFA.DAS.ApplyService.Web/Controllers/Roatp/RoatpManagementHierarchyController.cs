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

            var model = new ConfirmManagementHierarchyViewModel 
            { 
                ApplicationId = applicationId, 
                ManagementHierarchyData = managementHierarchyData
            };

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/ManagementHierarchy/ConfirmManagementHierarchy.cshtml", model);
        }

        [HttpGet]
        public IActionResult AddManagementHierarchy(Guid applicationId)
        {
            var model = new AddEditManagementHierarchyViewModel 
            { 
                ApplicationId = applicationId, 
                Title = "Who is in your organisation's management hierarchy for apprenticeships?",
                PageId = RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy,
                GetHelpAction = "AddManagementHierarchy"
            };

            PopulateGetHelpWithQuestion(model);
            return View("~/Views/Roatp/ManagementHierarchy/AddManagementHierarchy.cshtml", model);
        }

        [HttpPost]
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
                    PageId = RoatpWorkflowPageIds.ManagementHierarchy.EditManagementHierarchy,
                    GetHelpAction = "EditManagementHierarchy"
                };

                PopulateGetHelpWithQuestion(model);

                return View($"~/Views/Roatp/ManagementHierarchy/EditManagementHierarchy.cshtml", model);
            }
            return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
        }

        [HttpPost]
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

            if (personTableData is null || index >= personTableData.DataRows.Count)
            {
                return RedirectToAction("ConfirmManagementHierarchy", new { applicationId });
            }

            var model = new ConfirmRemoveManagementHierarchyViewModel
            {
                ApplicationId = applicationId,
                Name = $"{personTableData.DataRows[index].Columns[0]} {personTableData.DataRows[index].Columns[1]}",
                ActionName = "RemoveManagementHierarchy",
                BackAction = "ConfirmManagementHierarchy"
            };

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/ManagementHierarchy/ConfirmManagementHierarchyRemoval.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveManagementHierarchy(ConfirmRemoveManagementHierarchyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("RemoveManagementHierarchy", new { model.ApplicationId, model.Index });
            }

            if (model.Confirmation == "Yes" && model.Index >= 0)
            {
                var managementHierarchyData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddManagementHierarchy);

                if (managementHierarchyData is null || model.Index + 1 > managementHierarchyData.DataRows.Count)
                {
                    return RedirectToAction("ConfirmManagementHierarchy", new { model.ApplicationId });
                }

                managementHierarchyData.DataRows.RemoveAt(model.Index);

                var managementHierarchySection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.DeliveringApprenticeshipTraining, RoatpWorkflowSectionIds.DeliveringApprenticeshipTraining.ManagementHierarchy);

                var result = await _tabularDataRepository.SaveTabularDataAnswer(
                    model.ApplicationId,
                    managementHierarchySection.Id,
                    RoatpWorkflowPageIds.ManagementHierarchy.AddManagementHierarchy,
                    RoatpDeliveringApprenticeshipTrainingQuestionIdConstants.ManagementHierarchy,
                    managementHierarchyData);
            }

            return RedirectToAction("ConfirmManagementHierarchy", new { model.ApplicationId });
        }


        [HttpPost]
        public IActionResult CompleteManagementHierarchySection(Guid applicationId)
        {
            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId }, "Sequence_7");
        }
    }
}
