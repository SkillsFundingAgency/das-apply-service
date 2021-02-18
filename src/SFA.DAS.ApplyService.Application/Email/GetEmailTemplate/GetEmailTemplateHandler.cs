using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Email.GetEmailTemplate
{
    public class GetEmailTemplateHandler : IRequestHandler<GetEmailTemplateRequest, EmailTemplate>
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        public GetEmailTemplateHandler(IEmailTemplateRepository emailTemplateRepository)
        {
            _emailTemplateRepository = emailTemplateRepository;
        }

        public async Task<EmailTemplate> Handle(GetEmailTemplateRequest request, CancellationToken cancellationToken)
        {
            return await _emailTemplateRepository.GetEmailTemplate(request.TemplateName);
        }
    }
}
