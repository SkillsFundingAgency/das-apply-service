namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class ValidationDefinition
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public string ErrorMessage { get; set; }
    }
}