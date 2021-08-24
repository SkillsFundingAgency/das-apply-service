using System;

namespace SFA.DAS.ApplyService.Domain.QueryResults
{
    public class AppealFile
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
    }
}
