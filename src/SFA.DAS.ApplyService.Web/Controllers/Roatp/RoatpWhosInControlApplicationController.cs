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

            var verificationPartnership = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationSoleTraderPartnership);
            if (verificationPartnership.Value == "TRUE")
            {
                return await SoleTraderOrPartnership(applicationId);
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
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDirectors,
                    TableData = directorsData
                },
                CompaniesHousePscs = new PeopleInControl
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHousePSCs,
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
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDetailsConfirmed,
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
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrustees,
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
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionDetailsConfirmed,
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
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrustees,
                    Value = JsonConvert.SerializeObject(trusteesData)
                },
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionDetailsConfirmed,
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

        public async Task<IActionResult> SoleTraderOrPartnership(Guid applicationId)
        {         
            var model = new SoleTraderOrPartnershipViewModel { ApplicationId = applicationId };
            var soleTraderPartnershipAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.SoleTraderOrPartnership);
            if (soleTraderPartnershipAnswer != null)
            {
                model.OrganisationType = soleTraderPartnershipAnswer.Value;
            }

            return View("~/Views/Roatp/WhosInControl/SoleTraderOrPartnership.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSoleTraderOrPartnership(SoleTraderOrPartnershipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Roatp/WhosInControl/SoleTraderOrPartnership.cshtml", model);
            }

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var organisationTypeAnswer = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.SoleTradeOrPartnership,
                    Value = model.OrganisationType
                }
            };

            var updateResult = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.SoleTraderPartnership, organisationTypeAnswer);
            
            if (model.OrganisationType == SoleTraderOrPartnershipViewModel.OrganisationTypePartnership)
            {
                var partnersData = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.AddPartners);

                if (partnersData != null && partnersData.Value != null)
                {
                    return RedirectToAction("ConfirmPartners", new { applicationId = model.ApplicationId });
                }

                return RedirectToAction("PartnershipType", new { applicationId = model.ApplicationId });
            }
            else
            {
                return RedirectToAction("AddSoleTradeDob", new { applicationId = model.ApplicationId });
            }
        }

        public async Task<IActionResult> PartnershipType(Guid applicationId)
        {
            var model = new ConfirmPartnershipTypeViewModel { ApplicationId = applicationId };
            
            return View("~/Views/Roatp/WhosInControl/PartnershipType.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> PartnershipTypeConfirmed(ConfirmPartnershipTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Roatp/WhosInControl/PartnershipType.cshtml", model);
            }

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var organisationTypeAnswer = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.PartnershipType,
                    Value = model.PartnershipType
                }
            };

            var updateResult = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.PartnershipType, organisationTypeAnswer);


            if (model.PartnershipType == ConfirmPartnershipTypeViewModel.PartnershipTypeIndividual)
            {
                return RedirectToAction("AddPartnerIndividual", new { applicationId = model.ApplicationId });
            }
            else
            {
                return RedirectToAction("AddPartnerOrganisation", new { applicationId = model.ApplicationId });
            }
        }

        public async Task<IActionResult> AddPeopleInControl(Guid applicationId)
        {
            return View("~/Views/Roatp/WhosInControl/AddPeopleInControl.cshtml");
        }

        public async Task<IActionResult> AddPartnerIndividual(Guid applicationId)
        {
            var model = new AddEditPartnerViewModel { ApplicationId = applicationId, PartnerTypeIndividual = true };

            return View("~/Views/Roatp/WhosInControl/AddPartnerIndividual.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddIndividualPartnerDetails(AddEditPartnerViewModel model)
        {
            var errorMessages = PartnerDetailsValidator.Validate(model);

            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/AddPartnerIndividual.cshtml", model);
            }

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            TabularData partnerTableData;
            var individualPartnerAnswer = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.AddPartners);
            if (individualPartnerAnswer != null && individualPartnerAnswer.Value != null)
            {
                partnerTableData = JsonConvert.DeserializeObject<TabularData>(individualPartnerAnswer.Value);
            }
            else
            {
                partnerTableData = new TabularData
                {
                    HeadingTitles = new List<string> { "Name", "Date of birth" },
                    DataRows = new List<TabularDataRow>()                
                };
            }
            partnerTableData.DataRows.Add(new TabularDataRow
            {
                Id = Guid.NewGuid().ToString(),
                Columns = new List<string>
                {
                    model.PartnerName, DateOfBirthFormatter.FormatDateOfBirth(model.PartnerDobMonth, model.PartnerDobYear)
                }
            });

            var individualPartnerJson = JsonConvert.SerializeObject(partnerTableData);

            var individualPartnerAnswers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.AddPartners,
                    Value = individualPartnerJson
                }
            };

            var result = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddPartners, individualPartnerAnswers);
            
            return RedirectToAction("ConfirmPartners", new { applicationId = model.ApplicationId });
        }

        public async Task<IActionResult> AddPartnerOrganisation(Guid applicationId)
        {
            var model = new AddEditPartnerViewModel { ApplicationId = applicationId };
            
            return View("~/Views/Roatp/WhosInControl/AddPartnerOrganisation.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganisationPartnerDetails(AddEditPartnerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Roatp/WhosInControl/AddPartnerOrganisation.cshtml", model);
            }

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            TabularData partnerTableData;
            var individualPartnerAnswer = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.AddPartners);
            if (individualPartnerAnswer != null && individualPartnerAnswer.Value != null)
            {
                partnerTableData = JsonConvert.DeserializeObject<TabularData>(individualPartnerAnswer.Value);
            }
            else
            {
                partnerTableData = new TabularData
                {
                    HeadingTitles = new List<string> { "Name", "Date of birth" },
                    DataRows = new List<TabularDataRow>()
                };
            }
            partnerTableData.DataRows.Add(new TabularDataRow
            {
                Id = Guid.NewGuid().ToString(),
                Columns = new List<string>
                {
                    model.PartnerName, string.Empty
                }
            });

            var organisationPartnerJson = JsonConvert.SerializeObject(partnerTableData);

            var organisationPartnerAnswer = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.AddPartners,
                    Value = organisationPartnerJson
                }
            };


            var result = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddPartners, organisationPartnerAnswer);

            return RedirectToAction("ConfirmPartners", new { applicationId = model.ApplicationId });
        }

        public async Task<IActionResult> ConfirmPartners(Guid applicationId)
        {
            var model = new ConfirmPartnersViewModel { ApplicationId = applicationId };

            var partnerType = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.PartnershipType);
            if (partnerType != null && partnerType.Value != null && partnerType.Value == ConfirmPartnershipTypeViewModel.PartnershipTypeIndividual)
            {
                model.BackAction = "AddPartnerIndividual";
            }
            else
            {
                model.BackAction = "AddPartnerOrganisation";
            }

            var partnerData = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.AddPartners);
            if (partnerData != null && partnerData.Value != null)
            {
                var partnerTableData = JsonConvert.DeserializeObject<TabularData>(partnerData.Value);
                model.PartnerData = partnerTableData;
            }

            return View("~/Views/Roatp/WhosInControl/ConfirmPartners.cshtml", model);
        }

        public async Task<IActionResult> EditPartner(Guid applicationId, int index)
        {
            var partnerData = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.AddPartners);
            if (partnerData != null && partnerData.Value != null)
            {
                var partnerTableData = JsonConvert.DeserializeObject<TabularData>(partnerData.Value);
                var partner = partnerTableData.DataRows[index];
                
                var model = new AddEditPartnerViewModel
                {
                    ApplicationId = applicationId,
                    PartnerName = partner.Columns[0]
                };
                if (partner.Columns.Count > 1 && !String.IsNullOrEmpty(partner.Columns[1]))
                {
                    var dateOfBirth = partner.Columns[1];
                    model.PartnerDobMonth = DateOfBirthFormatter.GetMonthNumberFromShortDateOfBirth(dateOfBirth);
                    model.PartnerDobYear = DateOfBirthFormatter.GetYearFromShortDateOfBirth(dateOfBirth);
                    model.Index = index;
                    model.PartnerTypeIndividual = true;
                }
                return View($"~/Views/Roatp/WhosInControl/EditPartner.cshtml", model);
            }
            return RedirectToAction("ConfirmPartners", new { applicationId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePartnerDetails(AddEditPartnerViewModel model)
        {
            var errorMessages = PartnerDetailsValidator.Validate(model);

            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/EditPartner.cshtml", model);
            }

            var partnerData = await _qnaApiClient.GetAnswerByTag(model.ApplicationId, RoatpWorkflowQuestionTags.AddPartners);
            if (partnerData != null && partnerData.Value != null)
            {
                var partnerTableData = JsonConvert.DeserializeObject<TabularData>(partnerData.Value);
                var partner = new TabularDataRow
                {
                    Columns = new List<string> { model.PartnerName }
                };
                if (model.PartnerTypeIndividual)
                {
                    partner.Columns.Add(DateOfBirthFormatter.FormatDateOfBirth(model.PartnerDobMonth, model.PartnerDobYear));
                }
                else
                {
                    partner.Columns.Add(string.Empty);
                }

                partnerTableData.DataRows[model.Index] = partner;
                
                var updatedPartnerJson = JsonConvert.SerializeObject(partnerTableData);

                var updatedPartnerAnswer = new List<Answer>
                {
                    new Answer
                    {
                        QuestionId = RoatpYourOrganisationQuestionIdConstants.AddPartners,
                        Value = updatedPartnerJson
                    }
                };

                var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
                var yourOrganisationSequence =
                    applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
                var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
                var whosInControlSection =
                    yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

                var result = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddPartners, updatedPartnerAnswer);

            }

            return RedirectToAction("ConfirmPartners", new { model.ApplicationId });
        }

        public async Task<IActionResult> AddSoleTradeDob(Guid applicationId)
        {
            var model = new SoleTradeDobViewModel { ApplicationId = applicationId };

            var soleTraderName = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpLegalName);
            if (soleTraderName != null)
            {
                model.SoleTraderName = soleTraderName.Value;
            }

            var soleTraderDob = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.SoleTradeDob);
            if (soleTraderDob != null && soleTraderDob.Value != null)
            {
                var answerValue = soleTraderDob.Value;
                var delimiterIndex = answerValue.IndexOf(",");
                if (delimiterIndex > 0)
                {
                    model.SoleTraderDobMonth = answerValue.Substring(0, delimiterIndex);
                    model.SoleTraderDobYear = answerValue.Substring(delimiterIndex+1);
                }
            }
            return View("~/Views/Roatp/WhosInControl/AddSoleTradeDob.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SoleTradeDobConfirmed(SoleTradeDobViewModel model)
        {
            var dobMonth = new Answer { Value = model.SoleTraderDobMonth };
            var dobYear = new Answer { Value = model.SoleTraderDobYear };

            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(dobMonth, dobYear, SoleTradeDobViewModel.DobFieldPrefix);
            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/AddSoleTradeDob.cshtml", model);
            }

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var answerValue = $"{model.SoleTraderDobMonth},{model.SoleTraderDobYear}";
            var soleTradeDobAnswer = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.AddSoleTradeDob,
                    Value = answerValue
                }
            };

            var result = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, whosInControlSection.Id, RoatpWorkflowPageIds.WhosInControl.AddSoleTraderDob, soleTradeDobAnswer);

            await _applicationApiClient.MarkSectionAsCompleted(model.ApplicationId, whosInControlSection.Id);

            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId = model.ApplicationId });
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
