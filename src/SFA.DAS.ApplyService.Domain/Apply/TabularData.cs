
namespace SFA.DAS.ApplyService.Domain.Apply
{
    using System.Collections.Generic;
    
    public class TabularData
    {
        public string Caption { get; set; }
        public List<string> HeadingTitles { get; set; }
        public List<TabularDataRow> DataRows { get; set; }
    }

    public class TabularDataRow
    {
        public List<string> Columns { get; set; }
    }
}
