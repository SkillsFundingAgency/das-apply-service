using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class UploadAppealFileCommandHandler : IRequestHandler<UploadAppealFileCommand>
    {
        public Task<Unit> Handle(UploadAppealFileCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
