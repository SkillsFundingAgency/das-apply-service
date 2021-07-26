using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.AllowedProviders
{
    public class CanUkprnStartApplicationRequest : IRequest<bool>
    {
        public int UKPRN { get; }

        public CanUkprnStartApplicationRequest(int ukprn)
        {
            UKPRN = ukprn;
        }
    }
}
