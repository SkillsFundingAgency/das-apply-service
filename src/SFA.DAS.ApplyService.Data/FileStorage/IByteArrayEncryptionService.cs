namespace SFA.DAS.ApplyService.Data.FileStorage
{
    public interface IByteArrayEncryptionService
    {
        byte[] Encrypt(byte[] bytes);
        byte[] Decrypt(byte[] encryptedBytes);
    }
}