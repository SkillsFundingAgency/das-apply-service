namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    using MediatR;
    using SFA.DAS.ApplyService.Domain.Entities;

    public class GetOrganisationByNameRequest : IRequest<Organisation>
    {
        public string Name { get; set; }
    }
}
