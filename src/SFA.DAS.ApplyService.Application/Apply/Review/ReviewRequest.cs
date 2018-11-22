using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Review
{
    public class ReviewRequest : IRequest<List<Domain.Entities.Application>>
    {
    }
}