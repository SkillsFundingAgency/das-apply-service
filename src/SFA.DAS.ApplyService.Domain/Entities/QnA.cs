namespace SFA.DAS.ApplyService.Domain.Entities
{
    class QnA : EntityBase
    {
        public string Description { get; set; }
        public string Version { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}