using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.ViewModels.Roatp
{
    public class ConfirmTrusteesDateOfBirthViewModel
    {
        public Guid ApplicationId { get; set; }
        public PeopleInControl Trustees { get; set; }
        public List<ValidationErrorDetail> ErrorMessages { get; set; }
    }
}
