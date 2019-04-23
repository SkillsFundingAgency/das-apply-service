using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByUkprnRequest : IRequest<Organisation>
    {
        public string Ukprn { get; set; }
    }
}
