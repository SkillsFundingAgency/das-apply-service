namespace SFA.DAS.ApplyService.Session
{
    public interface ISessionService
    {
        void Set(string key, object value);
        void Set(string key, string stringValue);
        void Remove(string key);
        string Get(string key);
        T Get<T>(string key);
        bool Exists(string key);
    }
}