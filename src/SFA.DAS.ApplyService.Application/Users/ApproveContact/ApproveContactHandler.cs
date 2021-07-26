using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.Users.ApproveContact
{
    public class ApproveContactHandler : IRequestHandler<ApproveContactRequest, bool>
    {
        private readonly IContactRepository _contactRepository;

        public ApproveContactHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<bool> Handle(ApproveContactRequest request, CancellationToken cancellationToken)
        {
            return await _contactRepository.UpdateIsApproved(request.ContactId, true);
        }
    }
}
