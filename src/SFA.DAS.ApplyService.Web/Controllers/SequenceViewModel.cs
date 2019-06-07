using System;
using System.Collections.Generic;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Web.Controllers
{
    public class SequenceViewModel
    {
        public SequenceViewModel(ApplicationSequence sequence, Guid applicationId, List<ValidationErrorDetail> errorMessages)
        {
            ApplicationId = applicationId;
            Sections = sequence.Sections;
            SequenceId = (int)sequence.SequenceId;
            Status = sequence.Status;
            ErrorMessages = errorMessages;
        }

        public string Status { get; }
        public List<ApplicationSection> Sections { get; }

        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public List<ValidationErrorDetail> ErrorMessages { get; }
    }
}