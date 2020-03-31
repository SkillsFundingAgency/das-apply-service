using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Web.LayoutRenderers;
using NPOI.SS.Util;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Domain.Apply;
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

            var TRUE = "TRUE";
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
        [Route(("DirectorData/{applicationId}"))]
        public async Task<IActionResult> GetDirectorData(Guid applicationId)
        {
            var peopleInControl = new List<PersonInControl>();

            var directorsData =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHouseDirectors);

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
        [Route(("Apply/DirectorData/{applicationId}"))]
        public async Task<IActionResult> GetDirectorDataFromApply(Guid applicationId)
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
                var directorName = director?.Name.ToUpper();
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
        [Route(("PscData/{applicationId}"))]
        public async Task<IActionResult> GetPscs(Guid applicationId)
        {
            var pscs =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHousePscs);

            return Ok(pscs);
        }

        [HttpGet]
        [Route(("TrusteeData/{applicationId}"))]
        public async Task<IActionResult> GetTrustees(Guid applicationId)
        {
            var trustees =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.CharityCommissionTrustees);

            return Ok(trustees);
        }


        [HttpGet]
        [Route(("PeopleInControlData/{applicationId}"))]
        public async Task<IActionResult> GetPeopleInControl(Guid applicationId)
        {
            var peopleInControl =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.AddPeopleInControl);

            return Ok(peopleInControl);
        }


        [HttpGet]
        [Route(("PartnersData/{applicationId}"))]
        public async Task<IActionResult> GetPartners(Guid applicationId)
        {
            var directorData =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.AddPartners);
            return Ok(directorData);
        }


        [HttpGet]
        [Route(("SoleTraderDob/{applicationId}"))]
        public async Task<IActionResult> GetSoleTraderDob(Guid applicationId)
        {
            var soleTraderDob =
                await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.SoleTradeDob);

            var dateToProcess = string.IsNullOrEmpty(soleTraderDob?.Value) ? null : soleTraderDob.Value;

            var result = string.Empty;
            if (dateToProcess != null)
                result = DateOfBirthFormatter.GetMonthYearDescription(dateToProcess);


            return Ok(result);
        }
    }


}
