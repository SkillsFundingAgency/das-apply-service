using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Controllers
{
    [TestFixture]
    public class AllowedProvidersControllerTests
    {
        private Mock<IMediator> _mediator;

        private AllowedProvidersController _controller;

        [SetUp]
        public void TestSetup()
        {
            _mediator = new Mock<IMediator>();

            _controller = new AllowedProvidersController(_mediator.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task IsUkprnOnAllowedProviderList_returns_expected_result()
        {
            const int ukprn = 12345678;
            var expectedResult = true;

            _mediator.Setup(x => x.Send(It.Is<IsUkprnOnAllowedProvidersListRequest>(r => r.UKPRN == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.IsUkprnOnAllowedProviderList(ukprn);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
