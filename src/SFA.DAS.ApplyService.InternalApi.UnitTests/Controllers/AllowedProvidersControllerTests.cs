using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.AllowedProviders;
using SFA.DAS.ApplyService.Domain.Apply.AllowedProviders;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using System;
using System.Collections.Generic;
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
        public async Task CanUkprnStartApplication_returns_expected_result()
        {
            const int ukprn = 12345678;
            var expectedResult = true;

            _mediator.Setup(x => x.Send(It.Is<CanUkprnStartApplicationRequest>(r => r.UKPRN == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.CanUkprnStartApplication(ukprn);

            _mediator.Verify(x => x.Send(It.Is<CanUkprnStartApplicationRequest>(y => y.UKPRN == ukprn), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public async Task GetAllowedProvidersList_returns_expected_result()
        {
            const string sortColumn = null;
            const string sortOrder = null;

            var expectedResult = new List<AllowedProvider>
            {
                new AllowedProvider
                {
                    Ukprn = 12345678,
                    StartDateTime = DateTime.MinValue,
                    EndDateTime = DateTime.MaxValue,
                    AddedDateTime = DateTime.Today
                }
            };

            _mediator.Setup(x => x.Send(It.Is<GetAllowedProvidersListRequest>(r => r.SortColumn == sortColumn && r.SortOrder == sortOrder), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.GetAllowedProvidersList(sortColumn, sortOrder);

            _mediator.Verify(x => x.Send(It.Is<GetAllowedProvidersListRequest>(y => y.SortColumn == sortColumn && y.SortOrder == sortOrder), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreSame(expectedResult, actualResult);
        }

        [Test]
        public async Task AddAllowedProvider_returns_expected_result()
        {
            var entry = new AllowedProvider
            {
                Ukprn = 12345678,
                StartDateTime = DateTime.MinValue,
                EndDateTime = DateTime.MaxValue
            };

            var expectedResult = true;

            _mediator.Setup(x => x.Send(It.Is<AddAllowedProviderRequest>(r => r.Ukprn == entry.Ukprn && r.StartDateTime == entry.StartDateTime && r.EndDateTime == entry.EndDateTime), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

            var actualResult = await _controller.AddAllowedProvider(entry);

            _mediator.Verify(x => x.Send(It.Is<AddAllowedProviderRequest>(y => y.Ukprn == entry.Ukprn && y.StartDateTime == entry.StartDateTime && y.EndDateTime == entry.EndDateTime), It.IsAny<CancellationToken>()), Times.Once);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
