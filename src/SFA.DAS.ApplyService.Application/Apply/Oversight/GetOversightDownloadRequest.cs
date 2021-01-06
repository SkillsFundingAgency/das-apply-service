using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetOversightDownloadRequest : IRequest<List<ApplicationOversightDownloadDetails>>
    {
        //todo: date ranges
    }
}
