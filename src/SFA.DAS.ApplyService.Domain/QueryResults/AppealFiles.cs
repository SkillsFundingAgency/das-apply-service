using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.QueryResults
{
    public class AppealFiles
    {
        public List<AppealFile> Files { get; set; }
    }

    public class AppealFile
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
    }
}
