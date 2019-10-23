using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public RoatpWhosInControlApplicationController(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
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
            var yourOrganisationSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, yourOrganisationSection.Id, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, answers);

            var verificationCharityAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity);
            if (verificationCharityAnswer.Value == "TRUE")
            {
                return RedirectToAction("ConfirmTrusteesNoDob", new { applicationId });
            }

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
            var answers = GetAnswersFromForm();
            var trusteesAnswer = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);
            var trusteesData = JsonConvert.DeserializeObject<TabularData>(trusteesAnswer.Value);

            trusteesData = MapAnswersToTrusteesDob(trusteesData, answers);

            model.TrusteeDatesOfBirth = MapTrusteesDataToViewModel(trusteesData);
            model.ErrorMessages = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteesData, answers);

            if (model.ErrorMessages != null & model.ErrorMessages.Count > 0)
            {
                return View("~/Views/Roatp/WhosInControl/ConfirmTrusteesDob.cshtml", model);
            }
            
            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var yourOrganisationSection =
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

            var updateResult = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, yourOrganisationSection.Id, RoatpWorkflowPageIds.WhosInControl.CharityCommissionStartPage, trusteeAnswers);

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

        private List<Answer> GetAnswersFromForm()
        {
            var answers = new List<Answer>();

            Dictionary<string, JObject> answerValues = new Dictionary<string, JObject>();

            foreach (var formVariable in HttpContext.Request.Form.Where(f => !f.Key.StartsWith("__")))
            {
                var answerKey = formVariable.Key.Split("_Key_");
                if (!answerValues.ContainsKey(answerKey[0]))
                {
                    answerValues.Add(answerKey[0], new JObject());
                }

                answerValues[answerKey[0]].Add(
                    answerKey.Count() == 1 ? string.Empty : answerKey[1],
                    formVariable.Value.ToString());
            }

            foreach (var answer in answerValues)
            {
                if (answer.Value.Count > 1)
                {
                    answers.Add(new Answer() { QuestionId = answer.Key, JsonValue = answer.Value });
                }
                else
                {
                    answers.Add(new Answer() { QuestionId = answer.Key, Value = answer.Value.Value<string>(string.Empty) });
                }
            }

            return answers;
        }

        private List<TrusteeDateOfBirth> MapTrusteesDataToViewModel(TabularData trusteeData)
        {
            var trusteeDatesOfBirth = new List<TrusteeDateOfBirth>();
            foreach(var trustee in trusteeData.DataRows)
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
            return trusteeDatesOfBirth;
        }

        private TabularData MapAnswersToTrusteesDob(TabularData trusteesData, List<Answer> answers)
        {
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
