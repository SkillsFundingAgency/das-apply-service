using MediatR;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.ApplyService.Application.Import
{
    public class ImportWorkflowRequest : IRequest
    {
        public IFormFile ImportFile { get; }

        public ImportWorkflowRequest(IFormFile importFile)
        {
            ImportFile = importFile;
        }
    }
}