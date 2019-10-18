using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    public class RoatpWhosInControlApplicationController : Controller
    {
        private readonly IQnaApiClient _qnaApiClient;

        public RoatpWhosInControlApplicationController(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        public async Task<IActionResult> StartPage(Guid applicationId)
        {
            var verificationCompanyAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany);
            if (verificationCompanyAnswer.Value == "TRUE")
            {
                return await ConfirmDirectorsPscs(applicationId);
            }
            var verificationCharityAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity);
            if (verificationCharityAnswer.Value == "TRUE")
            {
                return await ConfirmTrusteesNoDob(applicationId);
            }

            return await AddPeopleInControl(applicationId);
        }

        public async Task<IActionResult> ConfirmDirectorsPscs(Guid applicationId)
        {
            var companiesHouseDirectorsAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHouseDirectors);
            var directorsData = JsonConvert.DeserializeObject<dynamic>(companiesHouseDirectorsAnswer.Value);

            var companiesHousePscsAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs);
            var pscsData = JsonConvert.DeserializeObject<dynamic>(companiesHousePscsAnswer.Value);

            var model = new ConfirmDirectorsPscsViewModel
            {
                ApplicationId = applicationId,
                CompaniesHouseDirectors = new PeopleInControl
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDirectors,
                    TableData = directorsData
                },
                CompaniesHousePscs = new PeopleInControl
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHousePSCs,
                    TableData = pscsData
                }
            };

            return View("~/Views/Roatp/WhosInControl/ConfirmDirectorsPscs.cshtml", model);
        }

        public async Task<IActionResult> DirectorsPscsConfirmed(Guid applicationId)
        {
            var companiesHouseDirectorsAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHouseDirectors);
            var companiesHousePscsAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs);

            var answers = new List<Answer>()
            {
                companiesHouseDirectorsAnswer,
                companiesHousePscsAnswer,
                new Answer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.CompaniesHouseDetailsConfirmed,
                    Value = "Y"
                }
            };

            var applicationSequences = await _qnaApiClient.GetSequences(applicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(applicationId, yourOrganisationSequence.Id);
            var yourOrganisationSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, yourOrganisationSection.Id, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, answers);



            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
        }

        public async Task<IActionResult> ConfirmTrusteesNoDob(Guid applicationId)
        {
            return View("~/Views/Roatp/WhosInControl/ConfirmTrusteesNoDob.cshtml");
        }

        public async Task<IActionResult> ConfirmTrusteesDob(Guid applicationId)
        {
            return View("~/Views/Roatp/WhosInControl/ConfirmTrusteesDob.cshtml");
        }

        public async Task<IActionResult> AddTrustees(Guid applicationId)
        {
            return View("~/Views/Roatp/WhosInControl/AddTrustees.cshtml");
        }

        public async Task<IActionResult> AddPartner(Guid applicationId)
        {
            return View("~/Views/Roatp/WhosInControl/AddPartner.cshtml");
        }

        public async Task<IActionResult> AddSoleTradeDob(Guid applicationId)
        {
            return View("~/Views/Roatp/WhosInControl/AddSoleTradeDob.cshtml");
        }

        public async Task<IActionResult> AddPeopleInControl(Guid applicationId)
        {
            return View("~/Views/Roatp/WhosInControl/AddPeopleInControl.cshtml");
        }
    }
}
