using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Interfaces;
using Moq;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AllowedProvidersList
{
    [TestFixture]
    public class CanUkprnStartApplicationRequestHandlerTests
    {
        private const int ALLOWED_UKPRN = 12345678;

        protected Mock<IAllowedProvidersRepository> _repository;
        protected CanUkprnStartApplicationRequestHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IAllowedProvidersRepository>();

            _repository.Setup(x => x.CanUkprnStartApplication(It.IsAny<int>())).ReturnsAsync(false);
            _repository.Setup(x => x.CanUkprnStartApplication(ALLOWED_UKPRN)).ReturnsAsync(true);

            _handler = new CanUkprnStartApplicationRequestHandler(_repository.Object);
        }

        [Test]
        public async Task IsUkprnOnAllowedProvidersListHandler_returns_true_if_UKPRN_on_allowed_list()
        {
            var result = await _handler.Handle(new CanUkprnStartApplicationRequest(12345678), new CancellationToken());

            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsUkprnOnAllowedProvidersListHandler_returns_false_if_UKPRN_not_on_allowed_list()
        {
            var result = await _handler.Handle(new CanUkprnStartApplicationRequest(87654321), new CancellationToken());

            Assert.IsFalse(result);
        }
    }
}
