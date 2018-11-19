using System;
using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> Store(string applicationId, string pageId, string questionId, string fileName, Stream fileStream);
        Task<Tuple<string, Stream>> Retrieve(string applicationId, string pageId, string questionId,
            string filename);
    }
}