using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetAnswers
{
    public class GetAnswersHandler : IRequestHandler<GetAnswersRequest, GetAnswersResponse>
    {
        private readonly IGetAnswersService _answersService;

        public GetAnswersHandler(IGetAnswersService answersService)
        {
            _answersService = answersService;
        }

        public async Task<GetAnswersResponse> Handle(GetAnswersRequest request, CancellationToken cancellationToken)
        {
            var answer = request.JsonAnswer
                ? await _answersService.GetJsonAnswersForQuestion(request.QuestionIdentifier, request.ApplicationId)                
                : await _answersService.GetAnswersForQuestion(request.QuestionIdentifier, request.ApplicationId);

            return new GetAnswersResponse {Answer=answer};
        }
    }
}
