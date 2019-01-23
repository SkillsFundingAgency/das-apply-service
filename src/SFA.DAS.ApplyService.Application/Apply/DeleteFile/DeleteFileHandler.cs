using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetPage;

namespace SFA.DAS.ApplyService.Application.Apply.DeleteFile
{
    public class DeleteFileHandler : IRequestHandler<DeleteFileRequest>
    {
        private readonly IMediator _mediator;

        public DeleteFileHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<Unit> Handle(DeleteFileRequest request, CancellationToken cancellationToken)
        {
            // Get page.
            // Remove question answer.
            // Save page.
            
            // Delete file from storage

            var page = await _mediator.Send(new GetPageRequest(request.ApplicationId, request.SequenceId, request.SectionId, request.PageId, request.UserId), CancellationToken.None);
            
            
            
            return Unit.Value;
        }
    }
}