using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AllowedProvidersList
{
    [TestFixture]
    public class RemoveAllowedProviderHandlerTests
    {
        private const int UKPRN = 12345678;

        protected Mock<IAllowedProvidersRepository> _repository;
        protected RemoveAllowedProviderHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAllowedProvidersRepository>();

            _repository.Setup(x => x.RemoveFromAllowedProvidersList(UKPRN)).ReturnsAsync(true);

            _handler = new RemoveAllowedProviderHandler(_repository.Object);
        }

        [Test]
        public async Task RemoveAllowedProviderHandler_returns_expected_result()
        {
            var result = await _handler.Handle(new RemoveAllowedProviderRequest(UKPRN), new CancellationToken());

            Assert.IsTrue(result);
        }
    }
}
