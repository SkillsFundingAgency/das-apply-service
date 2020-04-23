using System.Collections.Generic;
using System.Linq;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class TabularDataService: ITabularDataService
    {
        public bool IsRowAlreadyPresent(TabularData currentTabularData, TabularDataRow newRow)
        {
            var isPresent = false;
            foreach (var row in currentTabularData.DataRows)
            {
                isPresent = true;
                if (row.Columns.Count != newRow.Columns.Count)
                {
                    isPresent = false;
                }
                else
                {
                    for (var i = 0; i < row.Columns.Count; i++)
                    {
                        if (newRow.Columns[i] == row.Columns[i]) continue;
                        isPresent = false;
                        break;
                    }

                    if (isPresent)
                        return isPresent;
                }
            }

            return isPresent;
        }

        public TabularData DeduplicateData(TabularData tabularData)
        {
            var tabularDataDeduplicated = new TabularData {DataRows = new List<TabularDataRow>()};

            foreach (var row in tabularData.DataRows.Where(row => !IsRowAlreadyPresent(tabularDataDeduplicated,row)))
           {
               tabularDataDeduplicated.DataRows.Add(row);
           }

           tabularData.DataRows = tabularDataDeduplicated.DataRows;
           return tabularData;
        }
    }
}
