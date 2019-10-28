using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmTrusteesDateOfBirthViewModel
    {
        public Guid ApplicationId { get; set; }
        public List<TrusteeDateOfBirth> TrusteeDatesOfBirth { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }

    public class TrusteeDateOfBirth
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DobMonth { get; set; }
        public string DobYear { get; set; }
    }
}
