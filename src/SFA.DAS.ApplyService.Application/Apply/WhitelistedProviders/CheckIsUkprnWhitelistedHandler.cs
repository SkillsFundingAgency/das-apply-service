using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.WhitelistedProviders
{
    public class CheckIsUkprnWhitelistedHandler : IRequestHandler<CheckIsUkprnWhitelistedRequest, bool>
    {
        private IApplyRepository _repository;

        public CheckIsUkprnWhitelistedHandler(IApplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CheckIsUkprnWhitelistedRequest request, CancellationToken cancellationToken)
        {
            return await _repository.IsUkprnWhitelisted(request.UKPRN);
        }
    }
}
