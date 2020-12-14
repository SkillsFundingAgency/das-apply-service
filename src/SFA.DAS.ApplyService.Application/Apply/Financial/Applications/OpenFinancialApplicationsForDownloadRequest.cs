using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Financial.Applications
{
    public class OpenFinancialApplicationsForDownloadRequest : IRequest<List<RoatpFinancialSummaryDownloadItem>>
    {
    }
}
