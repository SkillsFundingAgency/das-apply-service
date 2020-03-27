using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
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
    }
}
