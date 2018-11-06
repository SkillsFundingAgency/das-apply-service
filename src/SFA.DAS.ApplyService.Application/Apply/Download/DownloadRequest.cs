using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Download
{
    public class DownloadRequest : IRequest<DownloadResponse>
    {
        public Guid ApplicationId { get; }
        public Guid UserId { get; }
        public string PageId { get; }
        public string QuestionId { get; }
        public string Filename { get; }

        public DownloadRequest(Guid applicationId, Guid userId, string pageId, string questionId, string filename)
        {
            ApplicationId = applicationId;
            UserId = userId;
            PageId = pageId;
            QuestionId = questionId;
            Filename = filename;
        }
    }
}