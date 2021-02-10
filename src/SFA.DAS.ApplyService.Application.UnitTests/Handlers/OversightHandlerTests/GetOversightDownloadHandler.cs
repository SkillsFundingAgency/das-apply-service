using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class GetOversightDownloadHandlerTests
    {
        private GetOversightDownloadHandler _handler;
        private Mock<IApplyRepository> _repository;
        private List<ApplicationOversightDownloadDetails> _data;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _data = _fixture.Create<List<ApplicationOversightDownloadDetails>>();

            _repository = new Mock<IApplyRepository>();
            _repository.Setup(x => x.GetOversightsForDownload(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(() => _data);

            _handler = new GetOversightDownloadHandler(_repository.Object);
        }

        [Test]
        public async Task Handler_returns_oversight_data()
        {
            var result = await _handler.Handle(new GetOversightDownloadRequest{ DateFrom = DateTime.Now, DateTo = DateTime.Now }, CancellationToken.None);
            Assert.AreSame(_data, result);
        }

        [Test]
        public async Task Handler_filters_oversight_data_by_date()
        {
            var fromDate = _fixture.Create<DateTime>();
            var toDate = _fixture.Create<DateTime>();
            await _handler.Handle(new GetOversightDownloadRequest { DateFrom = fromDate, DateTo = toDate }, CancellationToken.None);
            _repository.Verify(x => x.GetOversightsForDownload(It.Is<DateTime>(f => f == fromDate), It.Is<DateTime>(t => t == toDate)));
        }
    }
}
