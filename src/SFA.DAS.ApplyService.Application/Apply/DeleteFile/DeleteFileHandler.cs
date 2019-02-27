using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Application.Apply.DeleteFile
{
    public class DeleteFileHandler : IRequestHandler<DeleteFileRequest>
    {
        private readonly IMediator _mediator;
        private readonly IApplyRepository _applyRepository;
        private readonly IStorageService _storageService;

        public DeleteFileHandler(IMediator mediator, IApplyRepository applyRepository, IStorageService storageService)
        {
            _mediator = mediator;
            _applyRepository = applyRepository;
            _storageService = storageService;
        }
        
        public async Task<Unit> Handle(DeleteFileRequest request, CancellationToken cancellationToken)
        {
            var section = await _applyRepository.GetSection(request.ApplicationId, request.SequenceId, request.SectionId,
                request.UserId);

            var qnaDataObject = section.QnAData;
            
            var page = await _mediator.Send(new GetPageRequest(request.ApplicationId, request.SequenceId, request.SectionId, request.PageId, request.UserId), CancellationToken.None);

            var updatedPageOfAnswers = page.PageOfAnswers.SingleOrDefault(poa => poa.Answers.Any(a => a.QuestionId == request.QuestionId));

            var answer = updatedPageOfAnswers.Answers.SingleOrDefault(a => a.QuestionId == request.QuestionId && a.Value == request.FileName);

            var fileName = request.FileName;
            
            updatedPageOfAnswers.Answers.Remove(answer);

            var question = page.Questions.Single(q => q.QuestionId == request.QuestionId);

            if (question.Input.Validations.Any(v => v.Name == "Required") || (question.Input.FileUploadInfo.NumberOfUploadsRequired != null && question.Input.FileUploadInfo.NumberOfUploadsRequired.Value > updatedPageOfAnswers.Answers.Count))
            {
                page.Complete = false;
            }
                     
            qnaDataObject.Pages.ForEach(p =>
            {
                if (p.PageId == page.PageId)
                {
                    p.PageOfAnswers = page.PageOfAnswers;
                    p.Complete = page.Complete;
                }
            });

            section.QnAData = qnaDataObject;
            
            await _applyRepository.SaveSection(section, request.UserId);

            await _storageService.Delete(request.ApplicationId, request.SequenceId, request.SectionId, request.PageId,
                request.QuestionId, fileName);
            
            return Unit.Value;
        }
    }
}