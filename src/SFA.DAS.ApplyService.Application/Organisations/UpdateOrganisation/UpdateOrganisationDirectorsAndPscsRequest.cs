using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.CompaniesHouse;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.UpdateOrganisation
{
    public class UpdateOrganisationDirectorsAndPscsRequest : IRequest<bool>
    {
        public string Ukprn { get; set; }
        public List<DirectorInformation> Directors { get; set; }
        public List<PersonSignificantControlInformation> PersonsSignificantControl { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}