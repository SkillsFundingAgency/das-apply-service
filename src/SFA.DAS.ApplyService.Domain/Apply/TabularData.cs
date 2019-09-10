using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Apply
{
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
