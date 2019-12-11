using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class SubmitApplicationRequest : IRequest<bool>
    {
        public RoatpApplicationData ApplicationData { get; set; }
        
        public SubmitApplicationRequest(RoatpApplicationData applicationData)
        {
            ApplicationData = applicationData;
        }
    }
}
