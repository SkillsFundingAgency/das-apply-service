
namespace SFA.DAS.ApplyService.Application.Apply
{
    public class StartQnaApplicationRequest 
    {
        public string UserReference { get; set; }
        public string WorkflowType { get; set; }
        public string ApplicationData { get; set; }
    }
}