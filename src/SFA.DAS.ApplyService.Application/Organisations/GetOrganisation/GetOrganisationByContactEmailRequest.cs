namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    using MediatR;
    using SFA.DAS.ApplyService.Domain.Entities;

    public class GetOrganisationByContactEmailRequest : IRequest<Organisation>
    {
        public string Email { get; set; }
    }
}
