namespace SFA.DAS.ApplyService.Configuration
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class FileStorageConfig
    {
        public string FileEncryptionKey { get; set; }
        public string StorageConnectionString { get; set; }
        public string GatewayContainerName { get; set; }
        public string FinancialContainerName { get; set; }
        public string AssessorContainerName { get; set; }
        public string AppealsContainerName { get; set; }
    }
}
