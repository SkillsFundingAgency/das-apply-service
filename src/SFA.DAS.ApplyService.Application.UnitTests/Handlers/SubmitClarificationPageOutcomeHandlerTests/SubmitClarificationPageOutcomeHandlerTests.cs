﻿using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.SubmitClarificationPageOutcomeHandlerTests
{
    [TestFixture]
    public class SubmitClarificationPageOutcomeHandlerTests
    {
        private Mock<IClarificationRepository> _repository;
        private SubmitClarificationPageOutcomeHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IClarificationRepository>();
            _handler = new SubmitClarificationPageOutcomeHandler(_repository.Object, Mock.Of<ILogger<SubmitClarificationPageOutcomeHandler>>());
        }

        [Test]
        public async Task SubmitModeratorPageOutcome_is_stored()
        {
            var applicationId = Guid.NewGuid();
            var sequenceNumber = 1;
            var sectionNumber = 2;
            var pageId = "30";
            var userId = "4fs7f-userId-7gfhh";
            var status = "Fail";
            var comment = "Very bad";
            var clarificaitonReponse = "A good response";

            await _handler.Handle(new SubmitClarificationPageOutcomeRequest(applicationId, sequenceNumber, sectionNumber, pageId, userId, status, comment, clarificaitonReponse), new CancellationToken());

            _repository.Verify(x => x.SubmitClarificationPageOutcome(applicationId, sequenceNumber, sectionNumber, pageId, userId, status, comment, clarificaitonReponse), Times.Once);
        }
    }
}