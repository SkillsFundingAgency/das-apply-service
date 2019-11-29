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
        private readonly ITabularDataRepository _tabularDataRepository;

        public RoatpWhosInControlApplicationController(IQnaApiClient qnaApiClient, IApplicationApiClient applicationApiClient, 
                                                       IAnswerFormService answerFormService, ITabularDataRepository tabularDataRepository)
        {
            _qnaApiClient = qnaApiClient;
            _applicationApiClient = applicationApiClient;
            _answerFormService = answerFormService;
            _tabularDataRepository = tabularDataRepository;
        }

        [Route("confirm-who-control")]
        public async Task<IActionResult> StartPage(Guid applicationId)
        {
            var verificationCompanyAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany);
            var companiesHouseManualEntryAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ManualEntryRequiredCompaniesHouse);
            if ((verificationCompanyAnswer.Value == "TRUE") 
                && (companiesHouseManualEntryAnswer.Value != "TRUE"))
            {
                return await ConfirmDirectorsPscs(applicationId);
            }
            var verificationCharityAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity);
            var charityCommissionManualEntryAnswer = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.ManualEntryRequiredCharityCommission);
            if ((verificationCharityAnswer.Value == "TRUE")
                && (charityCommissionManualEntryAnswer.Value != "TRUE"))
            {
                return await ConfirmTrusteesNoDob(applicationId);
            }

            var verificationPartnership = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationSoleTraderPartnership);
            if (verificationPartnership.Value == "TRUE")
            {
                return await SoleTraderOrPartnership(applicationId);
            }

            var peopleData = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);

            if (peopleData != null && peopleData.Value != null)
            {
                return await ConfirmPeopleInControl(applicationId);
            }

            return await AddPeopleInControl(applicationId);
        }

        [Route("confirm-directors-pscs")]
        public async Task<IActionResult> ConfirmDirectorsPscs(Guid applicationId)
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

            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
        }

        [Route("confirm-trustees")]
        public async Task<IActionResult> ConfirmTrusteesNoDob(Guid applicationId)
        {
            var trusteesData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);

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
            var trusteesData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);
            
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
            
            return RedirectToAction("TaskList", "RoatpApplication", new { model.ApplicationId });
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
                return RedirectToAction("AddPartner", new { applicationId = model.ApplicationId, partnerIndividual = true });
            }
            else
            {
                return RedirectToAction("AddPartner", new { applicationId = model.ApplicationId, partnerIndividual = false });
            }
        }
        
        public async Task<IActionResult> AddPartner(Guid applicationId, bool partnerIndividual)
        {
            var model = new AddEditPeopleInControlViewModel
            {
                ApplicationId = applicationId,
                DateOfBirthOptional = !partnerIndividual                
            };

            if (partnerIndividual)
            {
                model.Identifier = "individual";
            }
            else
            {
                model.Identifier = "organisation";
            }

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

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

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

            model.PartnerData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPartners);

            if (model.PartnerData == null || model.PartnerData.DataRows == null || model.PartnerData.DataRows.Count == 0)
            {
                return RedirectToAction("PartnershipType", new { applicationId });
            }

            return View("~/Views/Roatp/WhosInControl/ConfirmPartners.cshtml", model);
        }

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
                    DateOfBirthOptional = true
                };
                if (partner.Columns.Count > 1 && !String.IsNullOrEmpty(partner.Columns[1]))
                {
                    var dateOfBirth = partner.Columns[1];
                    model.PersonInControlDobMonth = DateOfBirthFormatter.GetMonthNumberFromShortDateOfBirth(dateOfBirth);
                    model.PersonInControlDobYear = DateOfBirthFormatter.GetYearFromShortDateOfBirth(dateOfBirth);
                    model.DateOfBirthOptional = false;
                    model.Identifier = "individual";
                }
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
                
                var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
                var yourOrganisationSequence =
                    applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
                var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
                var whosInControlSection =
                    yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

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
            var errorMessages = DateOfBirthAnswerValidator.ValidateDateOfBirth(model.SoleTraderDobMonth, model.SoleTraderDobYear, SoleTradeDobViewModel.DobFieldPrefix);
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
            
            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId = model.ApplicationId });
        }

        public async Task<IActionResult> AddPeopleInControl(Guid applicationId)
        {
            var model = new AddEditPeopleInControlViewModel { ApplicationId = applicationId };

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

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

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
                var result = await _tabularDataRepository.AddTabularDataRecord(
                    model.ApplicationId,
                    whosInControlSection.Id,
                    RoatpWorkflowPageIds.WhosInControl.AddPeopleInControl, 
                    RoatpYourOrganisationQuestionIdConstants.AddPeopleInControl, 
                    RoatpWorkflowQuestionTags.AddPeopleInControl,
                    personInControl);
            }

            return RedirectToAction("ConfirmPeopleInControl", new { model.ApplicationId });
        }

        public async Task<IActionResult> ConfirmPeopleInControl(Guid applicationId)
        {
            var peopleInControlData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);

            if (peopleInControlData == null || peopleInControlData.DataRows == null || peopleInControlData.DataRows.Count == 0)
            {
                return RedirectToAction("AddPeopleInControl", new { applicationId });
            }

            var model = new ConfirmPeopleInControlViewModel { ApplicationId = applicationId, PeopleInControlData = peopleInControlData };

            return View("~/Views/Roatp/WhosInControl/ConfirmPeopleInControl.cshtml", model);
        }


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
                    DateOfBirthOptional = false
                };
                
                return View($"~/Views/Roatp/WhosInControl/EditPeopleInControl.cshtml", model);
            }
            return RedirectToAction("ConfirmPeopleInControl", new { applicationId });
        }

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

                var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
                var yourOrganisationSequence =
                    applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
                var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
                var whosInControlSection =
                    yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

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

        public async Task<IActionResult> RemovePartner(Guid applicationId, int index)
        {
            var partnerTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPartners);

            if (index >= partnerTableData.DataRows.Count)
            {
                return RedirectToAction("ConfirmPartners", new { applicationId });
            }

            var partnerName = partnerTableData.DataRows[index].Columns[0];

            return await ConfirmRemovalOfPersonInControl(applicationId, partnerName, "RemovePartnerDetails", "ConfirmPartners");
        }

        public async Task<IActionResult> RemovePeopleInControl(Guid applicationId, int index)
        {
            var personTableData = await _tabularDataRepository.GetTabularDataAnswer(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);

            if (index >= personTableData.DataRows.Count)
            {
                return RedirectToAction("ConfirmPeopleInControl", new { applicationId });
            }

            var personName = personTableData.DataRows[index].Columns[0];

            return await ConfirmRemovalOfPersonInControl(applicationId, personName, "RemovePscDetails", "ConfirmPeopleInControl");
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
        public async Task<IActionResult> CompletePeopleInControlSection(Guid applicationId)
        {
            // tech debt - this will be reworked by the changes to the QnA config JSON and method
            // for determining that application section completed is derived (APR-1008)

            var applicationSequences = await _qnaApiClient.GetSequences(applicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(applicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);
            
            return RedirectToAction("TaskList", "RoatpApplication", new { applicationId });
        }

        private async Task<IActionResult> ConfirmRemovalOfPersonInControl(Guid applicationId, string name, string actionName, string backActionName)
        {
            var model = new ConfirmRemovePersonInControlViewModel
            {
                ApplicationId = applicationId,
                Name = name,
                ActionName = actionName,
                BackAction = backActionName
            };

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

            if ((pscData == null) || (model.Index < 0 || model.Index > pscData.DataRows.Count))
            {
                return RedirectToAction(redirectAction, new { model.ApplicationId });
            }

            pscData.DataRows.RemoveAt(model.Index);

            var applicationSequences = await _qnaApiClient.GetSequences(model.ApplicationId);
            var yourOrganisationSequence =
                applicationSequences.FirstOrDefault(x => x.SequenceId == RoatpWorkflowSequenceIds.YourOrganisation);
            var yourOrganisationSections = await _qnaApiClient.GetSections(model.ApplicationId, yourOrganisationSequence.Id);
            var whosInControlSection =
                yourOrganisationSections.FirstOrDefault(x => x.SectionId == RoatpWorkflowSectionIds.YourOrganisation.WhosInControl);

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
