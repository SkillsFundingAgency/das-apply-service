using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AllowedProvidersList
{
    [TestFixture]
    public class GetAllowedProviderDetailsHandlerTests
    {
        private const int UKPRN = 12345678;

        private AllowedProvider _allowedProvider;

        protected Mock<IAllowedProvidersRepository> _repository;
        protected GetAllowedProviderDetailsHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _allowedProvider=  new AllowedProvider
                                {
                                    Ukprn = UKPRN,
                                    StartDateTime = DateTime.MinValue,
                                    EndDateTime = DateTime.MaxValue,
                                    AddedDateTime = DateTime.Today
                                };

            _repository = new Mock<IAllowedProvidersRepository>();

            _repository.Setup(x => x.GetAllowedProviderDetails(UKPRN)).ReturnsAsync(_allowedProvider);

            _handler = new GetAllowedProviderDetailsHandler(_repository.Object);
        }

        [Test]
        public async Task GetAllowedProviderDetailsHandler_returns_expected_allowed_provider()
        {
            var result = await _handler.Handle(new GetAllowedProviderDetailsRequest(UKPRN), new CancellationToken());

            Assert.AreSame(_allowedProvider, result);
        }
    }
}
