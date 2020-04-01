using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    [Route("organisation/")]
    public class OrganisationSummaryController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IApplyRepository _applyRepository;

        public OrganisationSummaryController(IInternalQnaApiClient qnaApiClient, IApplyRepository applyRepository)
        {
            _qnaApiClient = qnaApiClient;
            _applyRepository = applyRepository;
        } 

        [HttpGet]
        [Route("TypeOfOrganisation/{applicationId}")]
        public async Task<IActionResult> GetTypeOfOrganisation(Guid applicationId)
        {

            const string TRUE = "TRUE";
                 var companyVerification = await
                _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCompany);

            var charityVerification = await
                _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.UkrlpVerificationCharity);


            if (companyVerification == TRUE && charityVerification == TRUE)
            {
                return Ok(GatewayOrganisationTypes.CompanyAndCharity);
            }

            if (companyVerification == TRUE)
            {
                return Ok(GatewayOrganisationTypes.Company);
            }

            if (charityVerification == TRUE)
            {
                return Ok(GatewayOrganisationTypes.Charity);
            }

            var soleTraderPartnership = await
                _qnaApiClient.GetQuestionTag(applicationId, RoatpWorkflowQuestionTags.SoleTraderOrPartnership);

            if (!string.IsNullOrEmpty(soleTraderPartnership))
                return Ok(soleTraderPartnership);

            return Ok(GatewayOrganisationTypes.StatutoryInstitute);
        }

        [HttpGet]
        [Route(("DirectorData/Submitted/{applicationId}"))]
        public async Task<IActionResult> GetDirectorsFromSubmitted(Guid applicationId)
        {
            var peopleInControl = new List<PersonInControl>();

            var directorsData =
                await _qnaApiClient.GetTabularDataByTag(applicationId,
                    RoatpWorkflowQuestionTags.CompaniesHouseDirectors);

            if (directorsData?.DataRows == null || !directorsData.DataRows.Any()) return Ok(peopleInControl);

            foreach (var director in directorsData.DataRows.Where(x => x.Columns.Any()).OrderBy(x => x.Columns[0]))
            {
                var directorName = director.Columns[0];
                var directorDob = string.Empty;
                if (director.Columns.Any() && director.Columns.Count >= 2)
                {
                    directorDob = director.Columns[1];
                }

                peopleInControl.Add(new PersonInControl {Name = directorName, DateOfBirth = directorDob});
            }
            
            return Ok(peopleInControl);
        }

        [HttpGet]
        [Route(("DirectorData/CompaniesHouse/{applicationId}"))]
        public async Task<IActionResult> GetDirectorsFromCompaniesHouse(Guid applicationId)
        {
            var peopleInControl = new List<PersonInControl>();

            var applyData = await _applyRepository.GetApplyData(applicationId);

            if (applyData?.GatewayReviewDetails?.CompaniesHouseDetails == null)
            {
                return Ok(peopleInControl);
            }

            var companyData = applyData.GatewayReviewDetails.CompaniesHouseDetails;
            if (companyData?.Directors == null || !companyData.Directors.Any())
                return Ok(peopleInControl);

            foreach (var director in companyData.Directors.OrderBy(x => x.Name))
            {
                var directorName = director?.Name;
                var directorDob = string.Empty;

                if (director.DateOfBirth!=null)
                {
                    directorDob = $"{director.DateOfBirth:MMM yyyy}";
                }
                peopleInControl.Add(new PersonInControl { Name = directorName, DateOfBirth = directorDob });
            }

            return Ok(peopleInControl);
        }

        [HttpGet]
        [Route(("PscData/Submitted/{applicationId}"))]
        public async Task<IActionResult> GetPscsFromSubmitted(Guid applicationId)
        {
            var peopleInControl = new List<PersonInControl>();

            var pics =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs);

            if (pics?.DataRows == null || !pics.DataRows.Any()) return Ok(peopleInControl);

            foreach (var pic in pics.DataRows.Where(x => x.Columns.Any()).OrderBy(x => x.Columns[0]))
            {
                var picName = pic.Columns[0];
                var picDob = string.Empty;
                if (pic.Columns.Any() && pic.Columns.Count >= 2)
                {
                    picDob = pic.Columns[1];
                }

                peopleInControl.Add(new PersonInControl { Name = picName, DateOfBirth = picDob });
            }

            return Ok(peopleInControl);
        }


        [HttpGet]
        [Route(("PscData/CompaniesHouse/{applicationId}"))]
        public async Task<IActionResult> GetPscsFromCompaniesHouse(Guid applicationId)
        {
            var peopleInControl = new List<PersonInControl>();

            var applyData = await _applyRepository.GetApplyData(applicationId);

            if (applyData?.GatewayReviewDetails?.CompaniesHouseDetails == null)
            {
                return Ok(peopleInControl);
            }

            var companyData = applyData.GatewayReviewDetails.CompaniesHouseDetails;
            if (companyData?.PersonsWithSignificantControl == null || !companyData.PersonsWithSignificantControl.Any())
                return Ok(peopleInControl);

            foreach (var pic in companyData.PersonsWithSignificantControl.OrderBy(x => x.Name))
            {
                var picName = pic?.Name;
                var picDob = string.Empty;

                if (pic.DateOfBirth != null)
                {
                    picDob = $"{pic.DateOfBirth:MMM yyyy}";
                }
                peopleInControl.Add(new PersonInControl { Name = picName, DateOfBirth = picDob });
            }

            return Ok(peopleInControl);
        }
        [HttpGet]
        [Route(("TrusteeData/Submitted/{applicationId}"))]
        public async Task<IActionResult> GetTrusteesFromSubmitted(Guid applicationId)
        {
            var peopleInControl = new List<PersonInControl>();

            var pics =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);

            if (pics?.DataRows == null || !pics.DataRows.Any()) return Ok(peopleInControl);

            foreach (var pic in pics.DataRows.Where(x => x.Columns.Any()).OrderBy(x => x.Columns[0]))
            {
                var picName = pic.Columns[0];
                var picDob = string.Empty;
                if (pic.Columns.Any() && pic.Columns.Count >= 2)
                {
                    picDob = pic.Columns[1];
                }

                peopleInControl.Add(new PersonInControl { Name = picName, DateOfBirth = picDob });
            }

            return Ok(peopleInControl);
        }


        [HttpGet]
        [Route(("TrusteeData/CharityCommission/{applicationId}"))]
        public async Task<IActionResult> GetTrusteesFromCharityCommission(Guid applicationId)
        {
            var peopleInControl = new List<PersonInControl>();

            var applyData = await _applyRepository.GetApplyData(applicationId);

            if (applyData?.GatewayReviewDetails?.CharityCommissionDetails == null)
            {
                return Ok(peopleInControl);
            }

            var charityCommissionData = applyData.GatewayReviewDetails.CharityCommissionDetails;
            if (charityCommissionData?.Trustees == null || !charityCommissionData.Trustees.Any())
                return Ok(peopleInControl);

            foreach (var pic in charityCommissionData.Trustees.OrderBy(x => x.Name))
            {
                var picName = pic?.Name;
                peopleInControl.Add(new PersonInControl { Name = picName, DateOfBirth = null });
            }

            return Ok(peopleInControl);
        }

        [HttpGet]
        [Route(("WhosInControlData/Submitted/{applicationId}"))]
        public async Task<IActionResult> GetWhosInControlFromSubmitted(Guid applicationId)
        {
            var peopleInControl = new List<PersonInControl>();

            var pics =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl) ??
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.AddPartners);

            if (pics != null)
            {
                if (pics?.DataRows == null || !pics.DataRows.Any()) return Ok(peopleInControl);

                foreach (var pic in pics.DataRows.Where(x => x.Columns.Any()).OrderBy(x => x.Columns[0]))
                {
                    var picName = pic.Columns[0];
                    var picDob = string.Empty;
                    if (pic.Columns.Any() && pic.Columns.Count >= 2)
                    {
                        picDob = pic.Columns[1];
                    }

                    peopleInControl.Add(new PersonInControl { Name = picName, DateOfBirth = picDob });
                }

            }
            else
            {
                var soleTraderDob = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.SoleTradeDob);
                var legalName = await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.UkrlpLegalName);

                if (!string.IsNullOrEmpty(soleTraderDob?.Value))
                {
                    var formattedDob = DateOfBirthFormatter.GetMonthYearDescription(soleTraderDob.Value);
                    peopleInControl.Add(new PersonInControl { Name = legalName?.Value, DateOfBirth = formattedDob });
                }
            }

            return Ok(peopleInControl);
        }
    }
}
