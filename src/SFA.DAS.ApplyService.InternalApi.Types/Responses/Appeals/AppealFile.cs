using System;

namespace SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals
{
    public class AppealFile
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
    }
}
