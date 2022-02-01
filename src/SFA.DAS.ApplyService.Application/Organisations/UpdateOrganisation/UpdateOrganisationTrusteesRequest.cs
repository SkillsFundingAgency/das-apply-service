using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.InternalApi.Types.CharityCommission;


namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    public class UpdateOrganisationTrusteesRequest : IRequest<bool>
    {
        public string Ukprn { get; set; }
        public List<Trustee> Trustees { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}