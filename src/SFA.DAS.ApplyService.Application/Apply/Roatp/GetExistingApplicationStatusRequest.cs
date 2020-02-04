using MediatR;
using SFA.DAS.ApplyService.Domain.Roatp;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetExistingApplicationStatusRequest : IRequest<IEnumerable<RoatpApplicationStatus>>
    {
        public string UKPRN { get; set; }

        public GetExistingApplicationStatusRequest(string ukprn)
        {
            UKPRN = ukprn;
        }
    }
}
