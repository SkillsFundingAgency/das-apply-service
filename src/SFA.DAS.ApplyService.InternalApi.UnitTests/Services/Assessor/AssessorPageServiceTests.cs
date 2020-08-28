﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
{
    [TestFixture]
    public class AssessorPageServiceTests
    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        private AssessorLookupService _assessorLookupService;
        private AssessorPageService _assessorPageService;

        private readonly Guid _applicationId = Guid.NewGuid();
        private const int _sequenceNumber = 5;
        private const int _sectionNumber = 99;
        private const string _pageId = "PageId";
        private const string _nextPageId = "NextPageId";
        private const string _bodyText = "some body text";

        [SetUp]
        public void TestSetup()
        {
            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _assessorLookupService = new AssessorLookupService();
            _assessorPageService = new AssessorPageService(_qnaApiClient.Object, _assessorLookupService);
        }

        [Test]
        public async Task Get_mapped_assessor_page_with_next_page_id_from_application_sequence_section_page()
        {
            var preambleSection = new ApplicationSection
            {
                Id = Guid.NewGuid(),
                QnAData =  new QnAData { Pages = new List<Page>
                    {
                        new Page
                        {
                            PageId = _pageId,
                            BodyText = _bodyText
                        }
                    }
                }
            };

            var skip = new SkipPageResponse {NextAction = "NextPage",NextActionId = _nextPageId};

            _qnaApiClient.Setup(x => x.GetSectionBySectionNo(_applicationId, _sequenceNumber,
                _sectionNumber)).ReturnsAsync(preambleSection).Verifiable();

            _qnaApiClient.Setup(x => x.SkipPageBySectionNo(_applicationId, _sequenceNumber,
                _sectionNumber, _pageId)).ReturnsAsync(skip).Verifiable();

            var returnedAssessorPage = await _assessorPageService.GetPage(_applicationId, _sequenceNumber, _sectionNumber, _pageId);

            Assert.AreEqual(_nextPageId,returnedAssessorPage.NextPageId);
            Assert.AreEqual(_bodyText,returnedAssessorPage.BodyText);
        }
    }
}