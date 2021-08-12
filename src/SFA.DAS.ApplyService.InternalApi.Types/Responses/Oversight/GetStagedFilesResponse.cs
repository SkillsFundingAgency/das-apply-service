using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class GetStagedFilesResponse
    {
        public List<AppealFile> Files { get; set; } = new List<AppealFile>();

        public class AppealFile
        {
            public Guid Id { get; set; }
            public string Filename { get; set; }
        }
    }
}
