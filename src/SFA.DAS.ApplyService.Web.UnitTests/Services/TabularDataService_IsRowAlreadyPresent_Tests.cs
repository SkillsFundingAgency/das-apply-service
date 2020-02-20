using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class TabularDataServiceIsRowAlreadyPresentTests
    {


        [Test]
        public void Checking_Row_Already_Present_When_Table_And_Row_Empty_Returns_False()
        {
            var tabularData = new TabularData {DataRows = new List<TabularDataRow>()};
            var tabularDataRow = new TabularDataRow();

            var result = new TabularDataService().IsRowAlreadyPresent(tabularData, tabularDataRow);

            Assert.IsFalse(result);
        }


        [Test]
        public void Checking_Row_Already_Present_When_Table_Empty_And_Row_Filled_Returns_False()
        {
            var tabularData = new TabularData {DataRows = new List<TabularDataRow>()};
            var tabularDataRow = new TabularDataRow {Columns = new List<string> {"item 1", "item 2"}};

            var result = new TabularDataService().IsRowAlreadyPresent(tabularData, tabularDataRow);

            Assert.IsFalse(result);
        }

        [Test]
        public void Checking_Row_Empty_When_Table_Filled_Returns_False()
        {
            var tabularData = new TabularData { DataRows = new List<TabularDataRow> {new TabularDataRow{ Columns = new List<string> { "item 1", "item 2" }}}};
            var tabularDataRow = new TabularDataRow { Columns = new List<string>( )};

            var result = new TabularDataService().IsRowAlreadyPresent(tabularData, tabularDataRow);

            Assert.IsFalse(result);
        }

        [Test]
        public void Checking_Row_Filled_Slightly_Different_When_Table_Filled_Returns_False()
        {
            var tabularData = new TabularData { DataRows = new List<TabularDataRow> { new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } } } };
            var tabularDataRow = new TabularDataRow { Columns = new List<string> { "item 1", "item 2b" } };

            var result = new TabularDataService().IsRowAlreadyPresent(tabularData, tabularDataRow);

            Assert.IsFalse(result);
        }

        [Test]
        public void Checking_Row_Filled_Same_But_With_Extra_Elements_When_Table_Filled_Returns_False()
        {
            var tabularData = new TabularData { DataRows = new List<TabularDataRow> { new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } } } };
            var tabularDataRow = new TabularDataRow { Columns = new List<string> { "item 1", "item 2", "item 3" } };

            var result = new TabularDataService().IsRowAlreadyPresent(tabularData, tabularDataRow);

            Assert.IsFalse(result);
        }

        [Test]
        public void Checking_Row_Filled_Same_But_With_Less_Elements_When_Table_Filled_Returns_False()
        {
            var tabularData = new TabularData { DataRows = new List<TabularDataRow> { new TabularDataRow { Columns = new List<string> { "item 1", "item 2", "item 3" } } } };
            var tabularDataRow = new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } };

            var result = new TabularDataService().IsRowAlreadyPresent(tabularData, tabularDataRow);

            Assert.IsFalse(result);
        }

        [Test]
        public void Checking_Row_Filled_Same_When_Table_Filled_Returns_True()
        {
            var tabularData = new TabularData { DataRows = new List<TabularDataRow> {
                new TabularDataRow { Columns = new List<string> { "item 4", "item 3" } },
                new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } } ,
                new TabularDataRow { Columns = new List<string> { "item 5", "item 6" } } } };
            var tabularDataRow = new TabularDataRow { Columns = new List<string> { "item 1", "item 2" } };

            var result = new TabularDataService().IsRowAlreadyPresent(tabularData, tabularDataRow);

            Assert.IsTrue(result);
        }
    }
}
