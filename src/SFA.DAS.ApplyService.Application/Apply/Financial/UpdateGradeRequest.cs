using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class UpdateGradeRequest : IRequest<Organisation>
    {
        public Guid ApplicationId { get; }
        public FinancialApplicationGrade UpdatedGrade { get; }

        public UpdateGradeRequest(Guid applicationId, FinancialApplicationGrade updatedGrade)
        {
            ApplicationId = applicationId;
            UpdatedGrade = updatedGrade;
        }
    }
}