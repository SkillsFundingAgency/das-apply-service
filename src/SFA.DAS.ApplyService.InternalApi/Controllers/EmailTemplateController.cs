using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Email.GetEmailTemplate;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Route("emailTemplates/")]
    [Authorize]
    public class EmailTemplateController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmailTemplateController> _logger;

        public EmailTemplateController(IMediator mediator, ILogger<EmailTemplateController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{templateName}", Name = "GetEmailTemplate")]
        public async Task<IActionResult> GetEmailTemplate(string templateName)
        {
            var emailTemplate = await _mediator.Send(new GetEmailTemplateRequest { TemplateName = templateName });

            if (emailTemplate is null)
            {
                return NotFound();
            }

            return Ok(emailTemplate);
        }
    }
}
