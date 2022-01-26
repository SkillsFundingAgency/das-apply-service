namespace SFA.DAS.ApplyService.Application.Organisations.ManageOrganisation
{
    using MediatR;
    using SFA.DAS.ApplyService.Domain.Entities;
    using System;

    public class ManageOrganisationRequest : IRequest<Organisation>
    {
        public string Name { get; set; }
        public string OrganisationType { get; set; }
        public int? OrganisationUkprn { get; set; }

        public bool RoATPApproved { get; set; }

        public OrganisationDetails OrganisationDetails { get; set; }

        public Guid CreatedBy { get; set; }
        public string PrimaryContactEmail { get; set; }
    }
}
