namespace SFA.DAS.ApplyService.Domain.Audit
{
    public class DiffItem
    {
        public string PropertyName { get; set; }
        public object InitialValue { get; set; }
        public object UpdatedValue { get; set; }
    }
}
