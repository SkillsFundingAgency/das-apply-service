using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetApplications
{
    public class GetApplicationByUserIdHandler : IRequestHandler<GetApplicationByUserIdRequest, Domain.Entities.Apply>
    {
        private readonly IApplyRepository _applyRepository;

        public GetApplicationByUserIdHandler(IApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
        }

        public async Task<Domain.Entities.Apply> Handle(GetApplicationByUserIdRequest idRequest, CancellationToken cancellationToken)
        {
            return await _applyRepository.GetApplicationByUserId(idRequest.ApplicationId, idRequest.SigninId);
        }
    }
}
