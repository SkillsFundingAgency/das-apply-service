using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Services;
using SFA.DAS.ApplyService.Domain.Audit;

namespace SFA.DAS.ApplyService.Application.UnitTests.Services
{
    [TestFixture]
    public class DiffServiceTests
    {
        private DiffServiceTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new DiffServiceTestsFixture();
        }

        [Test]
        public void IdenticalItemsProducesEmptyDiff()
        {
            _fixture.WithIdenticalItems().GenerateDiff();
            Assert.IsEmpty(_fixture.Result);
        }

        [Test]
        public void DifferentItemsAreReturned()
        {
            _fixture.WithRandomInitialItem().WithDifferentUpdatedValues().GenerateDiff();

            foreach (var item in _fixture.InitialItem)
            {
                var resultItem = _fixture.Result.Single(x => x.PropertyName == item.Key);
                Assert.AreEqual(item.Value, resultItem.InitialValue);
                Assert.AreEqual(_fixture.UpdatedItem[item.Key], resultItem.UpdatedValue);
            }
        }

        [Test]
        public void ComparisonToNullInitialStateReturnsAllItemsInUpdated()
        {
            _fixture.WithNullInitialItem().WithRandomUpdatedItem().GenerateDiff();

            Assert.AreEqual(_fixture.UpdatedItem.Count, _fixture.Result.Count);
            foreach (var item in _fixture.UpdatedItem)
            {
                var resultItem = _fixture.Result.Single(x => x.PropertyName == item.Key);
                Assert.IsNull(resultItem.InitialValue);
                Assert.AreEqual(item.Value, resultItem.UpdatedValue);
            }
        }

        [Test]
        public void ComparisonToInitialItemWithNullValuesReturnsAllItemsInUpdated()
        {
            _fixture.WithInitialItemsWithNullValues().WithDifferentUpdatedValues().GenerateDiff();

            Assert.AreEqual(_fixture.UpdatedItem.Count, _fixture.Result.Count);
            foreach (var item in _fixture.UpdatedItem)
            {
                var resultItem = _fixture.Result.Single(x => x.PropertyName == item.Key);
                Assert.IsNull(resultItem.InitialValue);
                Assert.AreEqual(item.Value, resultItem.UpdatedValue);
            }
        }

        [Test]
        public void ComparisonToNullUpdatedStateReturnsAllItemsInInitial()
        {
            _fixture.WithRandomInitialItem().WithNullUpdatedItem().GenerateDiff();

            Assert.AreEqual(_fixture.InitialItem.Count, _fixture.Result.Count);
            foreach (var item in _fixture.InitialItem)
            {
                var resultItem = _fixture.Result.Single(x => x.PropertyName == item.Key);
                Assert.IsNull(resultItem.UpdatedValue);
                Assert.AreEqual(item.Value, resultItem.InitialValue);
            }
        }

        private class DiffServiceTestsFixture
        {
            private readonly Fixture _autoFixture;
            private readonly DiffService _diffService;
            public IReadOnlyList<DiffItem> Result { get; private set; }
            public Dictionary<string, object> InitialItem;
            public Dictionary<string, object> UpdatedItem;

            public DiffServiceTestsFixture()
            {
                _autoFixture = new Fixture();
                _diffService = new DiffService();

                InitialItem = null;
                UpdatedItem = null;
            }

            public DiffServiceTestsFixture WithNullInitialItem()
            {
                InitialItem = null;
                return this;
            }

            public DiffServiceTestsFixture WithInitialItemsWithNullValues()
            {
                InitialItem = GenerateRandomDataWithNullValues();
                return this;
            }

            public DiffServiceTestsFixture WithNullUpdatedItem()
            {
                UpdatedItem = null;
                return this;
            }

            public DiffServiceTestsFixture WithRandomInitialItem()
            {
                InitialItem = GenerateRandomData();
                return this;
            }

            public DiffServiceTestsFixture WithRandomUpdatedItem()
            {
                UpdatedItem = GenerateRandomData();
                return this;
            }

            public DiffServiceTestsFixture WithDifferentUpdatedValues()
            {
                UpdatedItem = GenerateModifiedData(InitialItem);
                return this;
            }

            public DiffServiceTestsFixture WithIdenticalItems()
            {
                InitialItem = GenerateRandomData();
                //UpdatedItem = TestHelper.Clone(InitialItem);
                UpdatedItem = InitialItem;
                return this;
            }

            public void GenerateDiff()
            {
                Result = _diffService.GenerateDiff(InitialItem, UpdatedItem);
            }

            private Dictionary<string, object> GenerateRandomData()
            {
                var result = new Dictionary<string, object>();
                for (var i = 0; i < 10; i++)
                {
                    result.Add(_autoFixture.Create<string>(), _autoFixture.Create<string>());
                    result.Add(_autoFixture.Create<string>(), _autoFixture.Create<long>());
                    result.Add(_autoFixture.Create<string>(), _autoFixture.Create<DateTime>());
                }
                return result;
            }

            private Dictionary<string, object> GenerateRandomDataWithNullValues()
            {
                var result = new Dictionary<string, object>();
                for (var i = 0; i < 10; i++)
                {
                    result.Add(_autoFixture.Create<string>(), null);
                }
                return result;
            }

            private Dictionary<string, object> GenerateModifiedData(Dictionary<string, object> source)
            {
                var result = new Dictionary<string, object>();

                foreach (var sourceItem in source)
                {
                    switch (sourceItem.Value)
                    {
                        case null:
                            result.Add(sourceItem.Key, "modified");
                            continue;
                        case string stringValue:
                            result.Add(sourceItem.Key, stringValue + "_modified");
                            continue;
                        case long longValue:
                            result.Add(sourceItem.Key, longValue + 1);
                            continue;
                        case DateTime dateTimeValue:
                            result.Add(sourceItem.Key, dateTimeValue.AddDays(1));
                            continue;
                    }
                }

                return result;
            }
        }
    }
}
