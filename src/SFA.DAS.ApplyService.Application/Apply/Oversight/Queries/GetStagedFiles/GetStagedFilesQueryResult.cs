using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetStagedFiles
{
    public class GetStagedFilesQueryResult
    {
        public List<AppealFile> Files { get; set; } = new List<AppealFile>();

        public class AppealFile
        {
            public Guid Id { get; set; }
            public string Filename { get; set; }
        }
    }
}
