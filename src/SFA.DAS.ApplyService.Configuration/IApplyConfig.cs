using System.Runtime.InteropServices;

namespace SFA.DAS.ApplyService.Configuration
{
    public interface IApplyConfig
    {
        InternalApiConfig InternalApi { get; set; }
        string SignInPage { get; set; }
        string SessionRedisConnectionString { get; set; }
        DfeSignInConfig DfeSignIn { get; set; }
        string SqlConnectionString { get; set; }
        FileStorageConfig FileStorage { get; set; }
        EmailConfig Email { get; set; }

        AssessorServiceApiAuthentication AssessorServiceApiAuthentication { get; set; }
        ProviderRegisterApiAuthentication ProviderRegisterApiAuthentication { get; set; }
        ReferenceDataApiAuthentication ReferenceDataApiAuthentication { get; set; }
    }

    public class FileStorageConfig
    {
        public string FileEncryptionKey { get; set; }
        public string StorageConnectionString { get; set; }
        public string ContainerName { get; set; }
    }

    public class InternalApiConfig
    {
        public string Uri { get; set; }
    }
}