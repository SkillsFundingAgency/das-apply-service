namespace SFA.DAS.ApplyService.InternalApi.Infrastructure
{
    public interface IRoatpTokenService
    {
        string GetToken(string url);
    }
}
