using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.QueryResults
{
    public class Appeal
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<AppealUpload> Uploads { get; set; }

        public class AppealUpload
        {
            public Guid Id { get; set; }
            public string Filename { get; set; }
            public string ContentType { get; set; }
        }
    }
}
