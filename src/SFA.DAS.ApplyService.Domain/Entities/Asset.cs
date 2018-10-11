namespace SFA.DAS.ApplyService.Domain.Entities
{
    class Asset : EntityBase
    {
        public string Reference { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Format { get; set; }
    }
}