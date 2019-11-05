using SFA.DAS.ApplyService.Domain.Apply;
using System;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmPeopleInControlViewModel
    {
        public Guid ApplicationId { get; set; }
        public TabularData PeopleInControlData { get; set; }
    }
}
