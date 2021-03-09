using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.CreateAppeal
{
	public class CreateAppealCommandHandler : IRequestHandler<CreateAppealCommand>
	{
        private readonly IAppealUploadRepository _appealUploadRepository;
        private readonly IAppealsFileStorage _appealsFileStorage;
        private readonly IAuditService _auditService;

		public CreateAppealCommandHandler(IAppealUploadRepository appealUploadRepository, IAppealsFileStorage appealsFileStorage, IAuditService auditService)
        {
            _appealUploadRepository = appealUploadRepository;
            _appealsFileStorage = appealsFileStorage;
            _auditService = auditService;
        }

		public async Task<Unit> Handle(CreateAppealCommand request, CancellationToken cancellationToken)
		{
			return Unit.Value;
		}
	}
}
