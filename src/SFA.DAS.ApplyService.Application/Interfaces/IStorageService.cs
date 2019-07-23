using System;
using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> Store(string applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename, Stream fileStream, string fileContentType);
        Task<Tuple<string, Stream, string>> Retrieve(string applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename);

        Task Delete(Guid applicationId, int sequenceId, int sectionId, string pageId, string questionId, string filename);
    }
}