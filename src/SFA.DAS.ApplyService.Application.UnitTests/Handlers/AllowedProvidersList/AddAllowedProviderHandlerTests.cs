using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AllowedProvidersList
{
    [TestFixture]
    public class AddAllowedProviderHandlerTests
    {
        private const int UKPRN = 12345678;
        private readonly DateTime START_DATE = DateTime.MinValue;
        private readonly DateTime END_DATE = DateTime.MaxValue;

        protected Mock<IAllowedProvidersRepository> _repository;
        protected AddAllowedProviderHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAllowedProvidersRepository>();

            _repository.Setup(x => x.AddToAllowedProvidersList(UKPRN, START_DATE, END_DATE)).ReturnsAsync(true);

            _handler = new AddAllowedProviderHandler(_repository.Object);
        }

        [Test]
        public async Task AddAllowedProviderHandler_returns_expected_result()
        {
            var result = await _handler.Handle(new AddAllowedProviderRequest(UKPRN, START_DATE, END_DATE), new CancellationToken());

            Assert.IsTrue(result);
        }
    }
}
