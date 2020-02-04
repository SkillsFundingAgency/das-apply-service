using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class PageNavigationTrackingServiceTests
    {
        private Mock<ISessionService> _sessionService;
        private Mock<IQnaApiClient> _qnaApiClient;
        private PageNavigationTrackingService _service;
        private const int SequenceId = 1;
        private const int SectionId = 1;
        private ApplicationSection _section;

        [SetUp]
        public void Before_each_test()
        {
            _sessionService = new Mock<ISessionService>();
            _qnaApiClient = new Mock<IQnaApiClient>();
            var sequence = new ApplicationSequence { Id = Guid.NewGuid(), SequenceId = SequenceId };
            var sequences = new List<ApplicationSequence>
            {
                sequence
            };
            _qnaApiClient.Setup(x => x.GetSequences(It.IsAny<Guid>())).ReturnsAsync(sequences);
            _section = new ApplicationSection { Id = Guid.NewGuid(), SectionId = SectionId };
            _section.QnAData = new QnAData
            {
                Pages = new List<Page>()
            };
            var firstPage = new Page { PageId = "100" };
            _section.QnAData.Pages.Add(firstPage);
            var secondPage = new Page { PageId = "110" };
            _section.QnAData.Pages.Add(firstPage);
            var sections = new List<ApplicationSection>
            {
                _section
            };
            _qnaApiClient.Setup(x => x.GetSections(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(sections);
            _service = new PageNavigationTrackingService(_sessionService.Object, _qnaApiClient.Object);
        }

        [Test]
        public void Service_returns_no_prev_page_if_first_in_section()
        {
            var previousPageId = _service.GetBackNavigationPageId(Guid.NewGuid(), SequenceId, SectionId, "100").GetAwaiter().GetResult();

            previousPageId.Should().BeNull();
        }

        [Test]
        public void Service_returns_previous_page_if_more_than_one_in_stack()
        {
            _service.AddPageToNavigationStack("100");
            _service.AddPageToNavigationStack("110");

            var stack = new Stack<string>();
            stack.Push("100");
            stack.Push("110");

            _sessionService.Setup(x => x.Get<Stack<string>>(It.IsAny<string>())).Returns(stack);

            var previousPageId = _service.GetBackNavigationPageId(Guid.NewGuid(), SequenceId, SectionId, "110").GetAwaiter().GetResult();

            previousPageId.Should().Be("100");
        }

        [Test]
        public void Service_returns_previous_page_if_current_page_has_been_navigated_to_after_showing_an_error()
        {
            _service.AddPageToNavigationStack("100");
            _service.AddPageToNavigationStack("110");
            _service.AddPageToNavigationStack("110");

            var stack = new Stack<string>();
            stack.Push("100");
            stack.Push("110");  
            stack.Push("110"); 

            _sessionService.Setup(x => x.Get<Stack<string>>(It.IsAny<string>())).Returns(stack);

            var previousPageId = _service.GetBackNavigationPageId(Guid.NewGuid(), SequenceId, SectionId, "110").GetAwaiter().GetResult();

            previousPageId.Should().Be("100");
        }
    }
}
