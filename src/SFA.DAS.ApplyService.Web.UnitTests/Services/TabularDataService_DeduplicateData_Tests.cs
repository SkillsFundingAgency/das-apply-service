using System.Collections.Generic;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class TabularDataServiceDeduplicateDataTests
    {

        [Test]
        public void Checking_No_FallOver_If_TabularData_Empty()
        {
            var tabularData = new TabularData { DataRows = new List<TabularDataRow>() };

            var tabularDataActual = new TabularDataService().DeduplicateData(tabularData);

            var isEqual = CompareTwoSources(tabularData, tabularDataActual);

            Assert.IsTrue(isEqual);
        }



        [Test]
        public void Checking_No_Change_If_TabularData_Has_No_Duplicates()
        {
            var tabularData = new TabularData
            {
                DataRows = new List<TabularDataRow> {
                    new TabularDataRow { Columns = new List<string> { "item 4", "item 3" } },
                    new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } } ,
                    new TabularDataRow { Columns = new List<string> { "item 5", "item 6" } } }
            };
       
            var tabularDataActual = new TabularDataService().DeduplicateData(tabularData);

            var isEqual = CompareTwoSources(tabularData, tabularDataActual);

            Assert.IsTrue(isEqual);
        }

        [Test]
        public void Checking_Expected_Change_If_TabularData_Has_Duplicates()
        {
            var tabularDataEntered = new TabularData
            {
                DataRows = new List<TabularDataRow> {
                    new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } },
                    new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } } ,
                    new TabularDataRow { Columns = new List<string> { "item 5", "item 6" } } }
            };


            var tabularDataExpected = new TabularData
            {
                DataRows = new List<TabularDataRow> {
                    new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } },
                    new TabularDataRow { Columns = new List<string> { "item 5", "item 6" } } }
            };

            var tabularDataActual = new TabularDataService().DeduplicateData(tabularDataEntered);

            var isEqual = CompareTwoSources(tabularDataExpected, tabularDataActual);

            Assert.IsTrue(isEqual);
        }


        private static bool? CompareTwoSources(TabularData tabularData, TabularData tabularDataActual)
        {

            var answerJsonActual = JsonConvert.SerializeObject(tabularDataActual);
            var answerJsonInput = JsonConvert.SerializeObject(tabularData);

            return answerJsonInput == answerJsonActual;

        }
    }
}
