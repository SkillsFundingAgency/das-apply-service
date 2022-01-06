using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NPOI.HPSF;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Gateway.Applications;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Domain.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.GetFinancialDetailsHandlerTests
{



    [TestFixture]
    public class GetFinancialReviewDetailsHandlerTests
    {
        private GetFinancialReviewDetailsHandler _handler;
        private Mock<IApplyRepository> _repository;
        private Guid _applicationId;
        private FinancialReviewDetails _financialReviewDetails;
        private List<ClarificationFile> _clarificationFiles;
        private ClarificationFile _clarificationFile;

        [SetUp]
        public void TestSetup()
        {
            _applicationId = Guid.NewGuid();
            _financialReviewDetails = new FinancialReviewDetails {ApplicationId = _applicationId}; 
            _clarificationFile = new ClarificationFile {Filename = "file.pdf"};
            _clarificationFiles = new List<ClarificationFile> {_clarificationFile};
            _repository = new Mock<IApplyRepository>();
            _repository.Setup(x => x.GetFinancialReviewDetails(_applicationId)).ReturnsAsync(_financialReviewDetails);
            _repository.Setup(x => x.GetFinancialReviewClarificationFiles(_applicationId)).ReturnsAsync(_clarificationFiles);

            _handler = new GetFinancialReviewDetailsHandler(_repository.Object);
        }

        [Test]
        public async Task GetFinancialReviewDetailsHandler_returns_financial_details_with_clarification_files()
        {
            var result = await _handler.Handle(new GetFinancialReviewDetailsRequest(_applicationId), new CancellationToken());

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ApplicationId,_applicationId);
            Assert.AreEqual(result.ClarificationFiles.First().Filename,_clarificationFile.Filename);
        }
    }
}
