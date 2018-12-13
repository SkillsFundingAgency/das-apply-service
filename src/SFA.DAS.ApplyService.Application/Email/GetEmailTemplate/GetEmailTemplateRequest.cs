namespace SFA.DAS.ApplyService.Application.Email.GetEmailTemplate
{
    using MediatR;
    using SFA.DAS.ApplyService.Domain.Entities;

    public class GetEmailTemplateRequest : IRequest<EmailTemplate>
    {
        public string TemplateName { get; set; }
    }
}
