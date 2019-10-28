using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{    
    [Authorize]
    public class RoatpWhosInControlApplicationController : Controller
    {
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IAnswerFormService _answerFormService;

        public RoatpWhosInControlApplicationController(IQnaApiClient qnaApiClient, IApplicationApiClient applicationApiClient, IAnswerFormService answerFormService)
        {
            _qnaApiClient = qnaApiClient;
            _applicationApiClient = applicationApiClient;
            _answerFormService = answerFormService;
        }

        [Route("confirm-who-control")]
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

        [Route("confirm-directors-pscs")]
        public async Task<IActionResult> ConfirmDirectorsPscs(Guid applicationId)
        {
            var companiesHouseDirectorsAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHouseDirectors);
            var directorsData = JsonConvert.DeserializeObject<TabularData>(companiesHouseDirectorsAnswer.Value);

            var companiesHousePscsAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs);
            var pscsData = JsonConvert.DeserializeObject<TabularData>(companiesHousePscsAnswer.Value);

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

        [HttpPost]
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
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, answers);

            var verificationCharityAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity);
            if (verificationCharityAnswer.Value == "TRUE")
            {
                return RedirectToAction("ConfirmTrusteesNoDob", new { applicationId });
            }
            await _applicationApiClient.MarkSectionAsCompleted(applicationId, whosInControlSection.Id);

            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
        }

        [Route("confirm-trustees")]
        public async Task<IActionResult> ConfirmTrusteesNoDob(Guid applicationId)
        {
            var charityTrusteesAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);
            var trusteesData = JsonConvert.DeserializeObject<TabularData>(charityTrusteesAnswer.Value);
            
            var model = new ConfirmTrusteesViewModel
            {
                ApplicationId = applicationId,
                Trustees = new PeopleInControl
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                    TableData = trusteesData
                }
            };
            
            var verificationCompanyAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany);
            if (verificationCompanyAnswer.Value == "TRUE")
            {
                model.VerifiedCompaniesHouse = true;
            }

            return View("~/Views/Roatp/WhosInControl/ConfirmTrusteesNoDob.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> TrusteesConfirmed(Guid applicationId)
        {
            var trusteesAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);

            var answers = new List<Answer>()
            {
                trusteesAnswer,
                new Answer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionDetailsConfirmed,
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

            return RedirectToAction("ConfirmTrusteesDob", new { applicationId });
        }

        [Route("confirm-trustees-dob")]
        public async Task<IActionResult> ConfirmTrusteesDob(Guid applicationId)
        {
            var trusteesAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);
            var trusteesData = JsonConvert.DeserializeObject<TabularData>(trusteesAnswer.Value);

            var model = new ConfirmTrusteesDateOfBirthViewModel
            {
                ApplicationId = applicationId,
                TrusteeDatesOfBirth = MapTrusteesDataToViewModel(trusteesData)
            };

            return View("~/Views/Roatp/WhosInControl/ConfirmTrusteesDob.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> TrusteesDobsConfirmed(ConfirmTrusteesDateOfBirthViewModel model)
        {
            var answers = _answerFormService.GetAnswersFromForm(HttpContext);
            var trusteesAnswer = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);
            var trusteesData = JsonConvert.DeserializeObject<TabularData>(trusteesAnswer.Value);

            trusteesData = MapAnswersToTrusteesDob(trusteesData, answers);

            model.TrusteeDatesOfBirth = MapTrusteesDataToViewModel(trusteesData);

            if (model.TrusteeDatesOfBirth.Count == 0) // temporary code - manual trustee entry in future story
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { model.ApplicationId });
            }

            model.ErrorMessages = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteesData, answers);

            if (model.ErrorMessages != null & model.ErrorMessages.Count > 0)
            {
                return View("~/Views/Roatp/WhosInControl/ConfirmTrusteesDob.cshtml", model);
            }
            
            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var trusteeAnswers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionTrustees,
                    Value = JsonConvert.SerializeObject(trusteesData)
                },
                new Answer
                {
                    QuestionId = RoatpPreambleQuestionIdConstants.CharityCommissionDetailsConfirmed,
                    Value = "TRUE"
                }
            };

            var updateResult = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage, trusteeAnswers);

            await _applicationApiClient.MarkSectionAsCompleted(model.ApplicationId, whosInControlSection.Id);

            return RedirectToAction("TaskList", "RoatpApplication", new { model.ApplicationId });
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

        private List<TrusteeDateOfBirth> MapTrusteesDataToViewModel(TabularData trusteeData)
        {
            var trusteeDatesOfBirth = new List<TrusteeDateOfBirth>();
            if (trusteeData != null && trusteeData.DataRows != null)
            {
                foreach (var trustee in trusteeData.DataRows)
                {
                    var trusteeDob = new TrusteeDateOfBirth
                    {
                        Id = trustee.Id,
                        Name = trustee.Columns[0]
                    };
                    if (trustee.Columns.Count > 1)
                    {
                        var shortDob = trustee.Columns[1];
                        trusteeDob.DobMonth = DateOfBirthFormatter.GetMonthNumberFromShortDateOfBirth(shortDob);
                        trusteeDob.DobYear = DateOfBirthFormatter.GetYearFromShortDateOfBirth(shortDob);
                    }
                    trusteeDatesOfBirth.Add(trusteeDob);
                }
            }
            
            return trusteeDatesOfBirth;
        }

        private TabularData MapAnswersToTrusteesDob(TabularData trusteesData, List<Answer> answers)
        {
            if (trusteesData == null)
            {
                return null;
            }

            if (trusteesData.HeadingTitles.Count < 2)
            {
                trusteesData.HeadingTitles.Add("Date of birth");
            }

            foreach (var trustee in trusteesData.DataRows)
            {
                var dobMonthKey = $"{trustee.Id}_Month";
                var dobYearKey = $"{trustee.Id}_Year";
                var dobMonth = answers.FirstOrDefault(x => x.QuestionId == dobMonthKey);
                var dobYear = answers.FirstOrDefault(x => x.QuestionId == dobYearKey);
                if (dobMonth == null || dobYear == null)
                {
                    break;
                }
                if (trustee.Columns.Count < 2)
                {
                    trustee.Columns.Add(DateOfBirthFormatter.FormatDateOfBirth(dobMonth.Value, dobYear.Value));
                }
                else
                {
                    trustee.Columns[1] = DateOfBirthFormatter.FormatDateOfBirth(dobMonth.Value, dobYear.Value);
                }
            }

            return trusteesData;
        }

       
    }
}
