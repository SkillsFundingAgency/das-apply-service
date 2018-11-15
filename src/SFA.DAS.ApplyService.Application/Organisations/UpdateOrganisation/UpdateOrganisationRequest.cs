namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    using MediatR;
    using SFA.DAS.ApplyService.Domain.Entities;

    public class UpdateOrganisationRequest : IRequest<Organisation>
    {
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public int? OrganisationUkprn { get; set; }

        public OrganisationDetails OrganisationDetails { get; set; }

        public bool RoEPAOApproved { get; set; }
        public bool RoATPApproved { get; set; }

        public string UpdatedBy { get; set; }
        public string PrimaryContactEmail { get; set; }
    }
}
