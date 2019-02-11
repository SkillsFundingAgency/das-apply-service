using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Organisations;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    public class DataFedAnswersController : Controller
    {
        private readonly IOrganisationRepository _organisationRepository;

        public DataFedAnswersController(IOrganisationRepository organisationRepository)
        {
            _organisationRepository = organisationRepository;
        }

        [HttpGet("DataFed/CompanyNumber")]
        public async Task<ActionResult<DataFedAnswerResult>> CompanyNumber(Guid applicationId)
        {
            var organisation = await _organisationRepository.GetOrganisationByApplicationId(applicationId);
            if (organisation == null)
            {
                return BadRequest("Could not find Organisation for supplied applicationId");
            }
            return Ok(new DataFedAnswerResult() {Answer = organisation?.OrganisationDetails?.CompanyNumber});
        }
    }

    public class DataFedAnswerResult
    {
        public string Answer { get; set; }
    }
}