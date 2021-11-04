using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationsByUkprnRequest : IRequest<List<Domain.Entities.Apply>>
    {
        public string Ukprn { get; }

      
        public GetApplicationsByUkprnRequest(string ukprn)
        {
            Ukprn = ukprn;
        }
    }
}