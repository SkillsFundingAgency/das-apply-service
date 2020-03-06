using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applyRepository"></param>
        public RoatpGatewayController(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        [Route("Gateway/Page/Submit")]
        [HttpPost()]
         public async Task<ActionResult<bool>> GatewayPageSubmit([FromBody] UpsertGatewayPageAnswerRequest request)
        {
            return await _applyRepository.SubmitGatewayPageAnswer(request.ApplicationId, request.PageId, request.Status, request.UserName, request.GatewayPageData);
        }


         [Route("Gateway/Page")]
         [HttpGet]
         public async Task<ActionResult<GatewayPageAnswer>> GetGatewayPage(Guid applicationId, string pageId)
         {
             var res = await _applyRepository.GetGatewayPageAnswer(applicationId, pageId);
             return res;
         }

         [Route("Gateway/Pages")]
         [HttpGet]
         public async Task<ActionResult<List<GatewayPageAnswer>>> GetGatewayPages(Guid applicationId)
        {
             var res = await _applyRepository.GetGatewayPageAnswers(applicationId);
             return res;
         }
    }
}
