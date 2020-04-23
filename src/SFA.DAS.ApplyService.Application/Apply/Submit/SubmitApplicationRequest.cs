using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Submit
{
    public class SubmitApplicationRequest : IRequest<bool>
    {
        public Guid ApplicationId { get; set; }
        public Guid SubmittingContactId { get; set; }
        public int ProviderRoute { get; set; }
        public string ProviderRouteName { get; set; }

        public ApplyData ApplyData { get; set; }
    }
}
