﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.Validators;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize(Policy = "AccessInProgressApplication")]
    public class RoatpWhosInControlApplicationController : RoatpApplyControllerBase
    {
        private readonly IQnaApiClient _qnaApiClient;
        private readonly IApplicationApiClient _applicationApiClient;
        private readonly IOrganisationApiClient _organisationApiClient;
        private readonly IAnswerFormService _answerFormService;
        private readonly ITabularDataRepository _tabularDataRepository;
        private readonly ICompaniesHouseApiClient _companiesHouseApiClient;
        private readonly ITrusteeExemptionService _trusteeExemptionService;
        private readonly ILogger<RoatpWhosInControlApplicationController> _logger;

        public RoatpWhosInControlApplicationController(IQnaApiClient qnaApiClient, IApplicationApiClient applicationApiClient,
                                                       IAnswerFormService answerFormService, ITabularDataRepository tabularDataRepository,
                                                       ISessionService sessionService, ICompaniesHouseApiClient companiesHouseApiClient,
                                                       IOrganisationApiClient organisationApiClient,
                                                       ITrusteeExemptionService trusteeExemptionService,
                                                       ILogger<RoatpWhosInControlApplicationController> logger)
            : base(sessionService)
        {
            _qnaApiClient = qnaApiClient;
            _applicationApiClient = applicationApiClient;
            _answerFormService = answerFormService;
            _tabularDataRepository = tabularDataRepository;
            _companiesHouseApiClient = companiesHouseApiClient;
            _organisationApiClient = organisationApiClient;
            _trusteeExemptionService = trusteeExemptionService;
            _logger = logger;
        }

        [Route("confirm-who-control")]
        public async Task<IActionResult> StartPage(Guid applicationId)
        {
            var qnaApplicationData = await _qnaApiClient.GetApplicationData(applicationId);

            var companyNumber = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UKRLPVerificationCompanyNumber)?.Value<string>();
            var ukprn = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UKPRN)?.Value<string>();
            var verificationCompanyAnswer = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UkrlpVerificationCompany)?.Value<string>();
            var companiesHouseManualEntryAnswer = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.ManualEntryRequiredCompaniesHouse)?.Value<string>();
            if (verificationCompanyAnswer == "TRUE"
                && companiesHouseManualEntryAnswer != "TRUE")
            {
                return RedirectToAction("ConfirmDirectorsPscs", new { applicationId, ukprn, companyNumber });
            }

            var verificationCharityAnswer = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UkrlpVerificationCharity)?.Value<string>();
            var charityCommissionManualEntryAnswer = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.ManualEntryRequiredCharityCommission)?.Value<string>();
            if (verificationCharityAnswer == "TRUE"
                && charityCommissionManualEntryAnswer != "TRUE")
            {
                return RedirectToAction("ConfirmTrustees", new { applicationId });
            }

            var verificationPartnership = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UkrlpVerificationSoleTraderPartnership)?.Value<string>();
            if (verificationPartnership == "TRUE")
            {
                return RedirectToAction("SoleTraderOrPartnership", new { applicationId });
            }

            var peopleData = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.AddPeopleInControl)?.Value<string>();
            if (peopleData != null)
            {
                return RedirectToAction("ConfirmPeopleInControl", new { applicationId });
            }

            return RedirectToAction("AddPeopleInControl", new { applicationId });
        }

        [HttpGet("refresh-directors-pscs")]
        public async Task<IActionResult> RefreshDirectorsPscs(Guid applicationId, string ukprn, string companyNumber)
        {
            try
            {
                _logger.LogInformation($"RefreshDirectorsPscs: Retrieving company details applicationId {applicationId} | Company Number : {companyNumber}");
                var timer = new Stopwatch();
                timer.Start();
                var companyDetails = await _companiesHouseApiClient.GetCompanyDetails(companyNumber);
                var timeToCallCompanyDetails = $"{timer.ElapsedMilliseconds} ms";

                switch (companyDetails.Status)
                {
                    case CompaniesHouseSummary.ServiceUnavailable:
                        _logger.LogInformation($"Issue refreshing directors/pscs - applicationId {applicationId} | Company Number : {companyNumber} | Status : Service Unavailable");
                        return RedirectToAction("CompaniesHouseNotAvailable", "RoatpShutterPages");
                    case CompaniesHouseSummary.CompanyStatusNotFound:
                        _logger.LogInformation(
                            $"Issue refreshing directors/pscs - applicationId {applicationId} | Company Number : {companyNumber} | Status : Company Status Not Found");
                        return RedirectToAction("CompanyNotFoundRefresh", "RoatpShutterPages", new { companyNumber });
                }

                if (!CompaniesHouseValidator.CompaniesHouseStatusValid(ukprn, companyDetails.Status, _logger))
                {
                    _logger.LogInformation($"Issue refreshing directors/pscs - applicationId {applicationId} | UKPRN : {ukprn} | Status : Companies House status not valid: {companyDetails.Status}");
                    return RedirectToAction("CompanyNotFoundRefresh", "RoatpShutterPages", new { companyNumber });
                }

                var applicationDetails = new Domain.Roatp.ApplicationDetails { CompanySummary = companyDetails };

                _logger.LogInformation($"RefreshDirectorsPscs: updating organisation directors/pscs applicationId {applicationId}");

                var success = await _organisationApiClient.UpdateDirectorsAndPscs(ukprn, companyDetails.Directors, companyDetails.PersonsWithSignificantControl, User.GetUserId());

                if (!success)
                {
                    _logger.LogInformation($"Organisation director/pscs update failed - applicationId {applicationId}");
                    return RedirectToAction("CompaniesHouseNotAvailable", "RoatpShutterPages");
                }

                var directorsAnswers = RoatpPreambleQuestionBuilder.CreateCompaniesHouseWhosInControlQuestions(applicationDetails);

                _logger.LogInformation($"RefreshDirectorsPscs: resetting page answers for companies, applicationId {applicationId}");
                var resetSection1_3 = _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage);
                var resetSection3_4 = _qnaApiClient.ResetPageAnswersBySection(applicationId, RoatpWorkflowSequenceIds.CriminalComplianceChecks, RoatpWorkflowSectionIds.CriminalComplianceChecks.CheckOnWhosInControl);
                await Task.WhenAll(resetSection1_3, resetSection3_4);

                _logger.LogInformation($"RefreshDirectorsPscs: updating page answers for companies, applicationId {applicationId}");
                await _qnaApiClient.UpdatePageAnswers(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, directorsAnswers.ToList<Answer>());

                var timeToDoEntireCall = $"{timer.ElapsedMilliseconds} ms";
                timer.Stop();
                _logger.LogInformation($"RefreshDirectorsPscs: all updates completed for {applicationId} - entire call timespan: {timeToDoEntireCall}, Company call timespan: {timeToCallCompanyDetails}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when processing directors/pscs - applicationId {applicationId}");
                return RedirectToAction("CompaniesHouseNotAvailable", "RoatpShutterPages");
            }

            return RedirectToAction("StartPage", "RoatpWhosInControlApplication", new { applicationId });
        }




        [Route("confirm-directors-pscs")]
        public async Task<IActionResult> ConfirmDirectorsPscs(Guid applicationId, string ukprn, string companyNumber)
        {
            var directorsData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CompaniesHouseDirectors);
            var pscsData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs);

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
                },
                CompanyNumber = companyNumber,
                Ukprn = ukprn
            };

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/ConfirmDirectorsPscs.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> DirectorsPscsConfirmed(Guid applicationId)
        {
            var companiesHouseDirectorsAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHouseDirectors, RoatpYourOrganisationQuestionIdConstants.CompaniesHouseDirectors);
            var companiesHousePscsAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs, RoatpYourOrganisationQuestionIdConstants.CompaniesHousePSCs);

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

            var updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CompaniesHouseStartPage, answers);

            if (!updateResult.ValidationPassed)
            {
                return RedirectToAction("ConfirmDirectorsPscs", new { applicationId });
            }
            else
            {
                var verificationCharityAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity);
                if (verificationCharityAnswer.Value == "TRUE")
                {
                    return RedirectToAction("ConfirmTrustees", new { applicationId });
                }

                return RedirectToAction("TaskList", "RoatpApplication", new { applicationId }, "Sequence_1");
            }
        }

        [HttpGet("confirm-trustees")]
        public async Task<IActionResult> ConfirmTrustees(Guid applicationId)
        {
            var trusteesData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);

            var qnaApplicationData = await _qnaApiClient.GetApplicationData(applicationId);
            var ukprn = qnaApplicationData.GetValue(RoatpWorkflowQuestionTags.UKPRN)?.Value<string>();
            var charityExcluded = await _trusteeExemptionService.IsProviderExempted(ukprn);

            if (charityExcluded)
            {
                return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
            }

            if (trusteesData is null)
            {
                return RedirectToAction("AddPeopleInControl", new { applicationId });
            };

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
            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/ConfirmTrustees.cshtml", model);
        }

        [HttpPost("confirm-trustees")]
        public async Task<IActionResult> TrusteesConfirmed(Guid applicationId)
        {
            var trusteesAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees, RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrustees);

            var answers = new List<Answer>()
            {
                trusteesAnswer,
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrusteesConfirmed,
                    Value = "Y"
                }
            };

            var updateResult = await _qnaApiClient.UpdatePageAnswers(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees, answers);

            if (!updateResult.ValidationPassed)
            {
                return RedirectToAction("ConfirmTrustees", new { applicationId });
            }
            else
            {
                return RedirectToAction("ConfirmTrusteesDob", new { applicationId });
            }
        }

        [HttpGet("confirm-trustees-dob")]
        public async Task<IActionResult> ConfirmTrusteesDob(Guid applicationId)
        {
            var trusteesData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);

            var model = new ConfirmTrusteesDateOfBirthViewModel
            {
                ApplicationId = applicationId,
                TrusteeDatesOfBirth = MapTrusteesDataToViewModel(trusteesData)
            };
            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/ConfirmTrusteesDob.cshtml", model);
        }

        [HttpPost("confirm-trustees-dob")]
        public async Task<IActionResult> TrusteesDobsConfirmed(ConfirmTrusteesDateOfBirthViewModel model)
        {
            var answers = _answerFormService.GetAnswersFromForm(HttpContext);

            var trusteesData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);

            trusteesData = MapAnswersToTrusteesDob(trusteesData, answers);

            model.TrusteeDatesOfBirth = MapTrusteesDataToViewModel(trusteesData);

            if (model.TrusteeDatesOfBirth.Count == 0)
            {
                return RedirectToAction("AddPeopleInControl", new { model.ApplicationId });
            }

            model.ErrorMessages = TrusteeDateOfBirthValidator.ValidateTrusteeDatesOfBirth(trusteesData, answers);

            if (model.ErrorMessages != null & model.ErrorMessages.Count > 0)
            {
                return View("~/Views/Roatp/WhosInControl/ConfirmTrusteesDob.cshtml", model);
            }

            var trusteeAnswers = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrustees,
                    Value = JsonConvert.SerializeObject(trusteesData)
                },
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrusteesConfirmed,
                    Value = "Y"
                }
            };

            var updateTrusteesResult = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrustees, trusteeAnswers);

            if (!updateTrusteesResult.ValidationPassed)
            {
                return RedirectToAction("ConfirmTrusteesDob", new { model.ApplicationId });
            }
            else
            {
                var trusteeDobConfirmed = new List<Answer>
                {
                    new Answer
                    {
                        QuestionId = RoatpYourOrganisationQuestionIdConstants.CharityCommissionTrusteesDobConfirmed,
                        Value = "Y"
                    }
                };

                await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.CharityCommissionTrusteesDob, trusteeDobConfirmed);
            }

            return RedirectToAction("TaskList", "RoatpApplication", new { model.ApplicationId });
        }

        [HttpGet]
        public async Task<IActionResult> SoleTraderOrPartnership(Guid applicationId)
        {
            var model = new SoleTraderOrPartnershipViewModel { ApplicationId = applicationId };
            var soleTraderPartnershipAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.SoleTraderOrPartnership);
            if (soleTraderPartnershipAnswer != null)
            {
                model.OrganisationType = soleTraderPartnershipAnswer.Value;
            }

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/SoleTraderOrPartnership.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmSoleTraderOrPartnership(SoleTraderOrPartnershipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessages = new List<ValidationErrorDetail>();
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "OrganisationType",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }
                return View("~/Views/Roatp/WhosInControl/SoleTraderOrPartnership.cshtml", model);
            }

            var organisationTypeAnswer = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.SoleTradeOrPartnership,
                    Value = model.OrganisationType
                }
            };

            var updateResult = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.SoleTraderPartnership, organisationTypeAnswer);

            if (!updateResult.ValidationPassed)
            {
                return RedirectToAction("SoleTraderOrPartnership", new { applicationId = model.ApplicationId });
            }
            else if (model.OrganisationType == RoatpOrganisationTypes.Partnership)
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

        [HttpGet]
        public async Task<IActionResult> PartnershipType(Guid applicationId)
        {
            var model = new ConfirmPartnershipTypeViewModel { ApplicationId = applicationId };

            var partnerTypeAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.PartnershipType);

            if (partnerTypeAnswer != null && partnerTypeAnswer.Value != null)
            {
                model.PartnershipType = partnerTypeAnswer.Value;
            }

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/PartnershipType.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> PartnershipTypeConfirmed(ConfirmPartnershipTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessages = new List<ValidationErrorDetail>();
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var modelError in modelErrors)
                {
                    model.ErrorMessages.Add(new ValidationErrorDetail
                    {
                        Field = "PartnershipType",
                        ErrorMessage = modelError.ErrorMessage
                    });
                }
                return View("~/Views/Roatp/WhosInControl/PartnershipType.cshtml", model);
            }

            var organisationTypeAnswer = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.PartnershipType,
                    Value = model.PartnershipType
                }
            };

            var updateResult = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.PartnershipType, organisationTypeAnswer);
            if (!updateResult.ValidationPassed)
            {
                return RedirectToAction("PartnershipType", new { applicationId = model.ApplicationId });
            }

            if (model.PartnershipType == ConfirmPartnershipTypeViewModel.PartnershipTypeIndividual)
            {
                return RedirectToAction("AddPartner", new { applicationId = model.ApplicationId, partnerIndividual = true });
            }
            else
            {
                return RedirectToAction("AddPartner", new { applicationId = model.ApplicationId, partnerIndividual = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddPartner(Guid applicationId)
        {
            var partnerTypeAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.PartnershipType);

            var partnerIndividual = (partnerTypeAnswer != null && partnerTypeAnswer.Value == ConfirmPartnershipTypeViewModel.PartnershipTypeIndividual);

            var model = new AddEditPeopleInControlViewModel
            {
                ApplicationId = applicationId,
                DateOfBirthOptional = !partnerIndividual,
                GetHelpAction = "AddPartner",
                PageId = RoatpWorkflowPageIds.WhosInControl.AddPartners
            };

            if (partnerIndividual)
            {
                model.Identifier = "individual";
            }
            else
            {
                model.Identifier = "organisation";
            }

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/AddPartner.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPartnerDetails(AddEditPeopleInControlViewModel model)
        {
            var errorMessages = PeopleInControlValidator.Validate(model);

            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/AddPartner.cshtml", model);
            }

            var whosInControlSection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var partnerTableData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddPartners);

            if (partnerTableData == null)
            {
                partnerTableData = new TabularData
                {
                    HeadingTitles = new List<string> { "Name", "Date of birth" },
                    DataRows = new List<TabularDataRow>()
                };
            }

            var partnerData = new TabularDataRow
            {
                Id = Guid.NewGuid().ToString(),
                Columns = new List<string>
                {
                    model.PersonInControlName
                }

            };
            if (!model.DateOfBirthOptional)
            {
                partnerData.Columns.Add(DateOfBirthFormatter.FormatDateOfBirth(model.PersonInControlDobMonth, model.PersonInControlDobYear));
            }
            else
            {
                partnerData.Columns.Add(string.Empty);
            }
            partnerTableData.DataRows.Add(partnerData);

            var result = await _tabularDataRepository.SaveTabularDataAnswer(
                model.ApplicationId,
                whosInControlSection.Id,
                RoatpWorkflowPageIds.WhosInControl.AddPartners,
                RoatpYourOrganisationQuestionIdConstants.AddPartners,
                partnerTableData);

            return RedirectToAction("ConfirmPartners", new { applicationId = model.ApplicationId });
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmPartners(Guid applicationId)
        {
            var model = new ConfirmPartnersViewModel { ApplicationId = applicationId };

            model.BackAction = "AddPartner";
            model.PartnerData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPartners);

            if (model.PartnerData == null || model.PartnerData.DataRows == null || model.PartnerData.DataRows.Count == 0)
            {
                return RedirectToAction("PartnershipType", new { applicationId });
            }

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/ConfirmPartners.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPartner(Guid applicationId, int index)
        {
            var partnerTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPartners);
            if (partnerTableData != null)
            {
                if (index >= partnerTableData.DataRows.Count)
                {
                    return RedirectToAction("ConfirmPartners", new { applicationId });
                }

                var partner = partnerTableData.DataRows[index];

                var model = new AddEditPeopleInControlViewModel
                {
                    ApplicationId = applicationId,
                    PersonInControlName = partner.Columns[0],
                    Index = index,
                    Identifier = "organisation",
                    DateOfBirthOptional = true,
                    GetHelpAction = "EditPartner",
                    PageId = "EditPartner"
                };

                if (partner.Columns.Count > 1 && !string.IsNullOrEmpty(partner.Columns[1]))
                {
                    var dateOfBirth = partner.Columns[1];
                    model.PersonInControlDobMonth = DateOfBirthFormatter.GetMonthNumberFromShortDateOfBirth(dateOfBirth);
                    model.PersonInControlDobYear = DateOfBirthFormatter.GetYearFromShortDateOfBirth(dateOfBirth);
                    model.DateOfBirthOptional = false;
                    model.Identifier = "individual";
                }

                PopulateGetHelpWithQuestion(model);

                return View($"~/Views/Roatp/WhosInControl/EditPartner.cshtml", model);
            }
            return RedirectToAction("ConfirmPartners", new { applicationId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePartnerDetails(AddEditPeopleInControlViewModel model)
        {
            var errorMessages = PeopleInControlValidator.Validate(model);

            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/EditPartner.cshtml", model);
            }

            var partnerTableData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddPartners);
            if (partnerTableData != null)
            {
                var partner = new TabularDataRow
                {
                    Columns = new List<string> { model.PersonInControlName }
                };
                if (!model.DateOfBirthOptional)
                {
                    partner.Columns.Add(DateOfBirthFormatter.FormatDateOfBirth(model.PersonInControlDobMonth, model.PersonInControlDobYear));
                }
                else
                {
                    partner.Columns.Add(string.Empty);
                }

                var whosInControlSection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

                var result = await _tabularDataRepository.EditTabularDataRecord(
                    model.ApplicationId,
                    whosInControlSection.Id,
                    RoatpWorkflowPageIds.WhosInControl.AddPartners,
                    RoatpYourOrganisationQuestionIdConstants.AddPartners,
                    RoatpWorkflowQuestionTags.AddPartners,
                    partner,
                    model.Index);
            }

            return RedirectToAction("ConfirmPartners", new { model.ApplicationId });
        }

        [HttpGet]
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
                    model.SoleTraderDobYear = answerValue.Substring(delimiterIndex + 1);
                }
            }

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/AddSoleTradeDob.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> SoleTradeDobConfirmed(SoleTradeDobViewModel model)
        {
            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(model.SoleTraderDobMonth, model.SoleTraderDobYear, SoleTradeDobViewModel.DobFieldPrefix);
            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/AddSoleTradeDob.cshtml", model);
            }

            var answerValue = $"{model.SoleTraderDobMonth},{model.SoleTraderDobYear}";
            var soleTradeDobAnswer = new List<Answer>
            {
                new Answer
                {
                    QuestionId = RoatpYourOrganisationQuestionIdConstants.AddSoleTradeDob,
                    Value = answerValue
                }
            };

            var result = await _qnaApiClient.UpdatePageAnswers(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl, RoatpWorkflowPageIds.WhosInControl.AddSoleTraderDob, soleTradeDobAnswer);

            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId = model.ApplicationId }, "Sequence_1");
        }

        [HttpGet]
        public async Task<IActionResult> AddPeopleInControl(Guid applicationId)
        {
            var organisationType = await _applicationApiClient.GetOrganisationTypeFromApplication(applicationId);
            var model = new AddEditPeopleInControlViewModel
            {
                ApplicationId = applicationId,
                OrganisationType = organisationType,
                GetHelpAction = "AddPeopleInControl",
                PageId = RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl
            };

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/AddPeopleInControl.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPeopleInControlDetails(AddEditPeopleInControlViewModel model)
        {
            var errorMessages = PeopleInControlValidator.Validate(model);

            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/AddPeopleInControl.cshtml", model);
            }

            var whosInControlSection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var personInControlData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);

            var personInControl = new TabularDataRow
            {
                Id = Guid.NewGuid().ToString(),
                Columns = new List<string>
                {
                    model.PersonInControlName,
                    DateOfBirthFormatter.FormatDateOfBirth(model.PersonInControlDobMonth, model.PersonInControlDobYear)
                }
            };

            if (personInControlData == null)
            {
                personInControlData = new TabularData
                {
                    HeadingTitles = new List<string> { "Name", "Date of birth" },
                    DataRows = new List<TabularDataRow>
                    {
                        personInControl
                    }
                };

                var result = await _tabularDataRepository.SaveTabularDataAnswer(
                    model.ApplicationId,
                    whosInControlSection.Id,
                    RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl,
                    RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl,
                    personInControlData);
            }
            else
            {
                var result = await _tabularDataRepository.UpsertTabularDataRecord(
                        model.ApplicationId,
                        whosInControlSection.Id,
                        RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl,
                        RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl,
                        RoatpWorkflowQuestionTags.AddPeopleInControl,
                        personInControl);
            }

            return RedirectToAction("ConfirmPeopleInControl", new { model.ApplicationId });
        }



        [HttpGet]
        public async Task<IActionResult> ConfirmPeopleInControl(Guid applicationId)
        {
            var peopleInControlData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);

            if (peopleInControlData == null || peopleInControlData.DataRows == null || peopleInControlData.DataRows.Count == 0)
            {
                return RedirectToAction("AddPeopleInControl", new { applicationId });
            }

            var model = new ConfirmPeopleInControlViewModel
            {
                ApplicationId = applicationId,
                PeopleInControlData = peopleInControlData
            };

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/ConfirmPeopleInControl.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPeopleInControl(Guid applicationId, int index)
        {
            var personTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);
            if (personTableData != null)
            {
                if (index >= personTableData.DataRows.Count)
                {
                    return RedirectToAction("ConfirmPeopleInControl", new { applicationId });
                }

                var person = personTableData.DataRows[index];
                var name = person.Columns[0];
                var dateOfBirth = person.Columns[1];
                var model = new AddEditPeopleInControlViewModel
                {
                    ApplicationId = applicationId,
                    PersonInControlName = name,
                    Index = index,
                    Identifier = "person",
                    PersonInControlDobMonth = DateOfBirthFormatter.GetMonthNumberFromShortDateOfBirth(dateOfBirth),
                    PersonInControlDobYear = DateOfBirthFormatter.GetYearFromShortDateOfBirth(dateOfBirth),
                    DateOfBirthOptional = false,
                    GetHelpAction = "EditPeopleInControl",
                    PageId = "EditPeopleInControl"
                };

                PopulateGetHelpWithQuestion(model);
                return View($"~/Views/Roatp/WhosInControl/EditPeopleInControl.cshtml", model);
            }
            return RedirectToAction("ConfirmPeopleInControl", new { applicationId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePeopleInControlDetails(AddEditPeopleInControlViewModel model)
        {
            var errorMessages = PeopleInControlValidator.Validate(model);

            if (errorMessages.Any())
            {
                model.ErrorMessages = errorMessages;
                return View("~/Views/Roatp/WhosInControl/EditPeopleInControl.cshtml", model);
            }

            var peopleTableData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);
            if (peopleTableData != null)
            {
                var person = new TabularDataRow
                {
                    Columns = new List<string>
                    {
                        model.PersonInControlName,
                        DateOfBirthFormatter.FormatDateOfBirth(model.PersonInControlDobMonth, model.PersonInControlDobYear)
                    }
                };

                var whosInControlSection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

                var result = await _tabularDataRepository.EditTabularDataRecord(
                    model.ApplicationId,
                    whosInControlSection.Id,
                    RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl,
                    RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl,
                    RoatpWorkflowQuestionTags.AddPeopleInControl,
                    person,
                    model.Index);
            }

            return RedirectToAction("ConfirmPeopleInControl", new { model.ApplicationId });
        }

        [HttpGet]
        public async Task<IActionResult> RemovePartner(Guid applicationId, int index)
        {
            var partnerTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPartners);

            if (index >= partnerTableData.DataRows.Count)
            {
                return RedirectToAction("ConfirmPartners", new { applicationId });
            }

            var partnerName = partnerTableData.DataRows[index].Columns[0];

            return ConfirmRemovalOfPersonInControl(applicationId, partnerName, "RemovePartnerDetails", "ConfirmPartners");
        }

        [HttpGet]
        public async Task<IActionResult> RemovePeopleInControl(Guid applicationId, int index)
        {
            var personTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);

            if (index >= personTableData.DataRows.Count)
            {
                return RedirectToAction("ConfirmPeopleInControl", new { applicationId });
            }

            var personName = personTableData.DataRows[index].Columns[0];

            return ConfirmRemovalOfPersonInControl(applicationId, personName, "RemovePscDetails", "ConfirmPeopleInControl");
        }

        [HttpPost]
        public async Task<IActionResult> RemovePartnerDetails(ConfirmRemovePersonInControlViewModel model)
        {
            return await RemoveItemFromPscsList(
                model,
                RoatpWorkflowPageIds.WhosInControl.AddPartners,
                RoatpYourOrganisationQuestionIdConstants.AddPartners,
                RoatpWorkflowQuestionTags.AddPartners,
                "ConfirmPartners",
                model.BackAction);
        }

        [HttpPost]
        public async Task<IActionResult> RemovePscDetails(ConfirmRemovePersonInControlViewModel model)
        {
            return await RemoveItemFromPscsList(
                model,
                RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl,
                RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl,
                RoatpWorkflowQuestionTags.AddPeopleInControl,
                "ConfirmPeopleInControl",
                model.BackAction);
        }

        [HttpPost]
        public IActionResult CompletePeopleInControlSection(Guid applicationId)
        {
            // tech debt - this will be reworked by the changes to the QnA config JSON and method
            // for determining that application section completed is derived (APR-1008)
            // It only act as redirector
            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId }, "Sequence_1");
        }

        private IActionResult ConfirmRemovalOfPersonInControl(Guid applicationId, string name, string actionName, string backActionName)
        {
            var model = new ConfirmRemovePersonInControlViewModel
            {
                ApplicationId = applicationId,
                Name = name,
                ActionName = actionName,
                BackAction = backActionName,
                GetHelpAction = backActionName,
                PageId = actionName
            };

            PopulateGetHelpWithQuestion(model);

            return View("~/Views/Roatp/WhosInControl/ConfirmPscRemoval.cshtml", model);
        }

        private async Task<IActionResult> RemoveItemFromPscsList(ConfirmRemovePersonInControlViewModel model, string pageId, string questionId, string questionTag, string redirectAction, string backAction)
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

                return View("~/Views/Roatp/WhosInControl/ConfirmPscRemoval.cshtml", model);
            }

            if (model.Confirmation != "Y")
            {
                return RedirectToAction(redirectAction, new { model.ApplicationId });
            }

            var pscData = await _tabularDataRepository.GetTabularDataAnswer(model.ApplicationId, questionTag);

            if ((pscData == null) || (model.Index < 0 || model.Index + 1 > pscData.DataRows.Count))
            {
                return RedirectToAction(redirectAction, new { model.ApplicationId });
            }

            pscData.DataRows.RemoveAt(model.Index);

            var whosInControlSection = await _qnaApiClient.GetSectionBySectionNo(model.ApplicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

            var result = await _tabularDataRepository.SaveTabularDataAnswer(
                model.ApplicationId,
                whosInControlSection.Id,
                pageId,
                questionId,
                pscData);

            return RedirectToAction(redirectAction, new { model.ApplicationId });
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