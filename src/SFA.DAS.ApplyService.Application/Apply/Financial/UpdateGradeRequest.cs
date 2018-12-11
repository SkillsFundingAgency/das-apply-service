using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Financial
{
    public class UpdateGradeRequest : IRequest
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