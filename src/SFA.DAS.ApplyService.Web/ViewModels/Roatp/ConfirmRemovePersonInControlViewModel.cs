using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmRemovePersonInControlViewModel
    {
        public Guid ApplicationId { get; set; }
        public string Name { get; set; }
        public string Confirmation { get; set; }
        public int Index { get; set; }
        public string ActionName { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
