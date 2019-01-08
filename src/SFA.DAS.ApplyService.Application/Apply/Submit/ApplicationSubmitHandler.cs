using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Submit
{
    public class ApplicationSubmitHandler : IRequestHandler<ApplicationSubmitRequest>
    {
        private readonly IApplyRepository _applyRepository;

        public ApplicationSubmitHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }
        
        public async Task<Unit> Handle(ApplicationSubmitRequest request, CancellationToken cancellationToken)
        {
            var sections = await _applyRepository.GetSections(request.ApplicationId);

            foreach (var section in sections)
            {
                if (section.QnAData.HasFeedback)
                {
                    foreach (var feedback in section.QnAData.Feedback)
                    {
                        feedback.IsNew = false;
                        feedback.IsCompleted = true;
                    }
                }

                foreach (var page in section.QnAData.Pages)
                {
                    if (page.HasFeedback)
                    {
                        foreach (var feedback in page.Feedback)
                        {
                            feedback.IsNew = false;
                        }
                    }
                }
            }

            await _applyRepository.UpdateSections(sections);
            
            await _applyRepository.SubmitApplicationSequence(request);
            return Unit.Value;
        }
    }
}