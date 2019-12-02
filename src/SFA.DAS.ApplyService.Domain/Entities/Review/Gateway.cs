﻿using SFA.DAS.ApplyService.Domain.Review;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Entities.Review
{
    public class Gateway
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime AssignedAt { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public List<Outcome> Outcomes { get; set; }
    }
}
