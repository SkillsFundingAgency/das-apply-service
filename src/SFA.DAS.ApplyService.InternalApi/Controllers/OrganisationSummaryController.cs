using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Web.LayoutRenderers;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    [Route("organisation/")]
    public class OrganisationSummaryController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;

        public OrganisationSummaryController(IInternalQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
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
            var directorData =
                await _qnaApiClient.GetTabularDataByTag(applicationId, RoatpWorkflowQuestionTags.CompaniesHouseDirectors);

            return Ok(directorData);
        }

        [HttpGet]
        [Route(("Pscs/{applicationId}"))]
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
        [Route(("soleTraderDob/{applicationId}"))]
        public async Task<IActionResult> GetSoleTraderDob(Guid applicationId)
        {
            var soleTraderDob =
                await _qnaApiClient.GetAnswerByTag(applicationId, RoatpWorkflowQuestionTags.SoleTradeDob);

            return soleTraderDob?.Value == null ? null : Ok(soleTraderDob.Value);
        }
    }
}
