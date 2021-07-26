using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    public class GetOversightDownloadRequest : IRequest<List<ApplicationOversightDownloadDetails>>
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
