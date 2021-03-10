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
        private readonly IAppealRepository _appealRepository;
        private readonly IAppealUploadRepository _appealUploadRepository;
        private readonly IAuditService _auditService;

		public CreateAppealCommandHandler(IAppealRepository appealRepository, IAppealUploadRepository appealUploadRepository, IAuditService auditService)
        {
            _appealRepository = appealRepository;
            _appealUploadRepository = appealUploadRepository;
            _auditService = auditService;
        }

		public async Task<Unit> Handle(CreateAppealCommand request, CancellationToken cancellationToken)
		{
            //get the oversight review in question (in part, so that we know its application Id)

            //add an appeal entity to the appeal repo
            //audit of new appeal

            //mark any and all uploaded files as belonging to the appeal now, not the application
            //audit of uploads




			return Unit.Value;
		}
	}
}
