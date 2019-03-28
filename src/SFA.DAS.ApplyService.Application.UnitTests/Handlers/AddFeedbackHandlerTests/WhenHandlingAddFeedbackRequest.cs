using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Review.Feedback;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.AddFeedbackHandlerTests
{
    [TestFixture]
    public class WhenHandlingAddFeedbackRequest
    {
        private const string pageId = "1";

        private Mock<IApplyRepository> _applyRepository;
        private AddFeedbackHandler _handler;     

        [SetUp]
        public void Setup()
        {
            _applyRepository = new Mock<IApplyRepository>();

            var page = new Page { PageId = pageId };
            var section = new ApplicationSection { QnAData = new QnAData { Pages = new List<Page> { page } } };

            _applyRepository.Setup(r => r.GetSection(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(section);

            _handler = new AddFeedbackHandler(_applyRepository.Object);
        }

        [Test]
        public async Task ThenNewFeedbackIsAdded()
        {
            var feedback = new Feedback { From = "Test", Message = "Test", Date = DateTime.Now.Date };
            await _handler.Handle(new AddFeedbackRequest(Guid.Empty, 1, 1, pageId, feedback), new CancellationToken());

            _applyRepository.Verify(r => r.UpdateSections(
                It.Is<List<ApplicationSection>>(sections => sections[0].QnAData.Pages[0].HasNewFeedback)
                ));
        }

        [Test]
        public async Task ThenRequestedFeedbackAnsweredIsFalse()
        {
            var feedback = new Feedback { From = "Test", Message = "Test", Date = DateTime.Now.Date };
            await _handler.Handle(new AddFeedbackRequest(Guid.Empty, 1, 1, pageId, feedback), new CancellationToken());

            _applyRepository.Verify(r => r.UpdateSections(
                It.Is<List<ApplicationSection>>(sections => !sections[0].QnAData.RequestedFeedbackAnswered)
                ));
        }
    }
}
