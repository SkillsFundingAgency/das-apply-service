using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class RoatpGatewayController: Controller
    {
        private readonly IApplyRepository _applyRepository;
        private readonly ILogger<RoatpGatewayController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applyRepository"></param>
        public RoatpGatewayController(IApplyRepository applyRepository, ILogger<RoatpGatewayController> logger)
        {
            _applyRepository = applyRepository;
            _logger = logger;
        }

        [Route("Gateway/Page/Submit")]
        [HttpPost()]
         public async Task GatewayPageSubmit([FromBody] UpsertGatewayPageAnswerRequest request)
        {
            _logger.LogInformation($"RoatpGatewayController-GatewayPageSubmit - ApplicationId '{request.ApplicationId}' - PageId '{request.PageId}' - Status '{request.Status}' - UserName '{request.UserName}' - PageData '{request.GatewayPageData}'");
            try
            {
                await _applyRepository.SubmitGatewayPageAnswer(request.ApplicationId, request.PageId, request.Status, request.UserName, request.GatewayPageData);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "RoatpGatewayController-GatewayPageSubmit - Error: '" + ex.Message + "'");
            }
            
        }

         [Route("Gateway/Page")]
         [HttpGet]
         public async Task<ActionResult<GatewayPageAnswer>> GetGatewayPage(Guid applicationId, string pageId)
         {
             return await _applyRepository.GetGatewayPageAnswer(applicationId, pageId);
         }

         [Route("Gateway/Pages")]
         [HttpGet]
         public async Task<ActionResult<List<GatewayPageAnswerSummary>>> GetGatewayPages(Guid applicationId)
        {
            return await _applyRepository.GetGatewayPageAnswers(applicationId);
        }
    }
}
