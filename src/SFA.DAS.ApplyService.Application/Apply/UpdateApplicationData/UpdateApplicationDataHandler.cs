using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.UpdateApplicationData
{
    public class UpdateApplicationDataHandler : IRequestHandler<UpdateApplicationDataRequest>
    {
        private readonly IApplyRepository _applyRepository;
        private readonly IContactRepository _contactRepository;

        public UpdateApplicationDataHandler(IApplyRepository applyRepository, IContactRepository contactRepository)
        {
            _applyRepository = applyRepository;
            _contactRepository = contactRepository;
        }
        
        public async Task<Unit> Handle(UpdateApplicationDataRequest request, CancellationToken cancellationToken)
        {
            if (request.ApplicationData == null) return Unit.Value;
            var standardAppData = JsonConvert.DeserializeObject<StandardApplicationData>(request.ApplicationData.ToString()); 
            var application = await _applyRepository.GetApplication(request.ApplicationId);
            //application data entry must exist in application table
            if (application== null) return Unit.Value;
            if (application?.ApplicationData == null)
                application.ApplicationData = new ApplicationData(); 
            application.ApplicationData.StandardName = standardAppData.StandardName;
            application.ApplicationData.StandardCode = standardAppData.StandardCode;

            if (application.ApplicationData.StandardSubmissions == null)
            {
                application.ApplicationData.StandardSubmissions = new List<StandardSubmission>
                {
                    new StandardSubmission
                    {
                        SubmittedAt = DateTime.UtcNow,
                        SubmittedBy = standardAppData.UserEmail
                    }
                };
            }
            else
            {
                application.ApplicationData.StandardSubmissions.Add(new StandardSubmission
                {
                    SubmittedAt = DateTime.UtcNow,
                    SubmittedBy = ""
                });
            }
            await _applyRepository.UpdateApplicationData(request.ApplicationId, application.ApplicationData);

            return Unit.Value;
        }
    }
}