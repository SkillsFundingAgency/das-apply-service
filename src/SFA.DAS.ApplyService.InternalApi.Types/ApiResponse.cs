namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Response { get; set; }
    }
}
