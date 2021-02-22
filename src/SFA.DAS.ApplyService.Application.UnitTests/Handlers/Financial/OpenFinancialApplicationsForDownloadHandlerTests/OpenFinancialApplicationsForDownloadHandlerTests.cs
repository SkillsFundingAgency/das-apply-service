using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Financial.Applications;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Financial.OpenFinancialApplicationsForDownloadHandlerTests
{
    [TestFixture]
    public class OpenFinancialApplicationsForDownloadHandlerTests
    {
        private Mock<IApplyRepository> _applyRepository;
        private OpenFinancialApplicationsForDownloadRequestHandler _handler;

        private List<RoatpFinancialSummaryDownloadItem> _openApplications;

        [SetUp]
        public void Setup()
        {
            _openApplications = new List<RoatpFinancialSummaryDownloadItem>();

            _applyRepository = new Mock<IApplyRepository>();
            _applyRepository.Setup(r => r.GetOpenFinancialApplicationsForDownload()).ReturnsAsync(_openApplications);

            _handler = new OpenFinancialApplicationsForDownloadRequestHandler(_applyRepository.Object);
        }

        [Test]
        public async Task OpenFinancialApplicationsForDownloadHandler_returns_expected_applications()
        {
            var request = new OpenFinancialApplicationsForDownloadRequest();
            var result = await _handler.Handle(request, new CancellationToken());

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(_openApplications);
        }
    }
}
