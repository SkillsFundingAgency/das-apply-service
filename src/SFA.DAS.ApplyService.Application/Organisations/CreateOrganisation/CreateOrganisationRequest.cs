namespace SFA.DAS.ApplyService.Application.Organisations.CreateOrganisation
{
    using MediatR;
    using SFA.DAS.ApplyService.Domain.Entities;
    using System;

    public class CreateOrganisationRequest : IRequest<Organisation>
    {
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public int? OrganisationUkprn { get; set; }

        public OrganisationDetails OrganisationDetails { get; set; }

        public Guid CreatedByUserId { get; set; }
        public string CreatedBy { get; set; }
        public string PrimaryContactEmail { get; set; }
    }
}
