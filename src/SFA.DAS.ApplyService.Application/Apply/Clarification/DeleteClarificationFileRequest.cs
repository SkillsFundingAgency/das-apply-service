using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Clarification
{
    public class DeleteClarificationFileRequest : IRequest<Unit>
    {
        public DeleteClarificationFileRequest(Guid applicationId,
                                                        int sequenceNumber,
                                                        int sectionNumber,
                                                        string pageId,
                                                        string clarificationFile)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
            ClarificationFile = clarificationFile;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
        public string ClarificationFile { get; }
    }
}
