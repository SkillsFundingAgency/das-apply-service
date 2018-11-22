using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetSection
{
    public class GetSectionRequest : IRequest<ApplicationSection>
    {
        public Guid ApplicationId { get; }
        public Guid? UserId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }

        public GetSectionRequest(Guid applicationId, Guid? userId, int sequenceId, int sectionId)
        {
            ApplicationId = applicationId;
            UserId = userId;
            SequenceId = sequenceId;
            SectionId = sectionId;
        }
    }
}