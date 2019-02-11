using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.GetAnswers
{
    public class GetAnswersHandler : IRequestHandler<GetAnswersRequest, string>
    {
        private readonly IGetAnswersService _answersService;

        public GetAnswersHandler(IGetAnswersService answersService)
        {
            _answersService = answersService;
        }


        public async Task<string> Handle(GetAnswersRequest request, CancellationToken cancellationToken)
        {

            return await _answersService.GetAnswers(request.QuestionIdentifier, request.ApplicationId);

        }
    }
}
