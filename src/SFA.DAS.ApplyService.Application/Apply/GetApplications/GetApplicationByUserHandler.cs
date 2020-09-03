using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationByUserHandler : IRequestHandler<GetApplicationByUserRequest, Domain.Entities.Apply>
    {
        private readonly IApplyRepository _applyRepository;

        public GetApplicationByUserHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Domain.Entities.Apply> Handle(GetApplicationByUserRequest request, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetApplicationByUser(request.ApplicationId, request.SigninId);
        }
    }
}
