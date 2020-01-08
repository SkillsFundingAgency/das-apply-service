using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Submit
{
    public class SubmitApplicationRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public Guid SubmittingContactId { get; set; }
        public int ProviderRoute { get; set; }
    }
}
