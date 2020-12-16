using System.Collections.Generic;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Services;

namespace SFA.DAS.ApplyService.Application.UnitTests.Services
{
    [TestFixture]
    public class StateServiceTests
    {
        private WhenGettingStateTestFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenGettingStateTestFixture();
        }

        [Test]
        public void PublicInstancePropertiesAreCaptured()
        {
            var result = _fixture.GetState();
            Assert.IsTrue(result.ContainsKey(nameof(GetStateTestClass.TestPublicProperty)));
            Assert.AreEqual(_fixture.TestObject.TestPublicProperty, result[nameof(GetStateTestClass.TestPublicProperty)]);
        }

        [Test]
        public void PrivateInstancePropertiesAreNotCaptured()
        {
            var result = _fixture.GetState();
            Assert.IsFalse(result.ContainsKey("TestPrivateProperty"));
        }

        [Test]
        public void StaticPropertiesAreNotCaptured()
        {
            var result = _fixture.GetState();
            Assert.IsFalse(result.ContainsKey(nameof(GetStateTestClass.TestStaticProperty)));
        }

        [Test]
        public void EnumerablePropertiesAreNotCaptured()
        {
            var result = _fixture.GetState();
            Assert.IsFalse(result.ContainsKey(nameof(GetStateTestClass.TestEnumerable)));
        }

        [Test]
        public void ClassesAreNotCaptured()
        {
            var result = _fixture.GetState();
            Assert.IsFalse(result.ContainsKey(nameof(GetStateTestClass.TestObject)));
        }

        [Test]
        public void AbstractsAreNotCaptured()
        {
            var result = _fixture.GetState();
            Assert.IsFalse(result.ContainsKey(nameof(GetStateTestClass.TestInterfaceImplementation)));
        }


        private class WhenGettingStateTestFixture
        {
            private readonly StateService _stateService;
            public readonly GetStateTestClass TestObject;
            private Dictionary<string, object> _result;
            public readonly long PrivatePropertyValue;

            public WhenGettingStateTestFixture()
            {
                var autoFixture = new Fixture();
                _stateService = new StateService();
                PrivatePropertyValue = autoFixture.Create<long>();
                TestObject = new GetStateTestClass(PrivatePropertyValue);
            }

            public Dictionary<string, object> GetState()
            {
                _result = _stateService.GetState(TestObject);
                return _result;
            }
        }

        private class GetStateTestClass
        {
            public GetStateTestClass(long privatePropertyValue)
            {
                var autoFixture = new Fixture();
                TestPrivateProperty = privatePropertyValue;
                TestPublicProperty = autoFixture.Create<string>();
                TestObject = new GetStateTestSubClass();
                TestEnumerable = new List<GetStateTestSubClass>();
                TestInterfaceImplementation = new StateTestInterfaceImplementation();
            }

            public string TestPublicProperty { get; }
            private long TestPrivateProperty { get; }
            public GetStateTestSubClass TestObject { get; }
            public List<GetStateTestSubClass> TestEnumerable { get; }
            public static string TestStaticProperty { get; }
            public IStateTestInterface TestInterfaceImplementation { get; }
        }

        private class GetStateTestSubClass
        {
        }

        private interface IStateTestInterface
        {
        }

        private class StateTestInterfaceImplementation : IStateTestInterface
        {
        }
    }
}
