using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Interfaces
{
    public interface IEncryptionService
    {
        Task<Stream> Encrypt(Stream fileStream);
        Task<Stream> Decrypt(Stream encryptedFileStream);
    }
}