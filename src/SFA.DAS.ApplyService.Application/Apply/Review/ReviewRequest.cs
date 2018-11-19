using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review
{
    public class ReviewRequest : IRequest<List<Domain.Entities.Application>>
    {
    }
}