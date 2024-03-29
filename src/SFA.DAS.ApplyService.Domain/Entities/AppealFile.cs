﻿using System;
using SFA.DAS.ApplyService.Application.Interfaces;

namespace SFA.DAS.ApplyService.Domain.Entities
{
    public class AppealFile : IAuditable
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int Size { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }

        public AppealFile()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
        }
    }
}
