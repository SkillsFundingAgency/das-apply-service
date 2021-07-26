using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Data.UnitOfWork;

namespace SFA.DAS.ApplyService.Application.Behaviours
{
    public class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehaviour(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var result = await next();

            if (IsCommand(request))
            {
                await _unitOfWork.Commit();
            }

            return result;
        }

        private bool IsCommand(TRequest request)
        {
            return request.GetType().Name.EndsWith("Command");
        }
    }
}
