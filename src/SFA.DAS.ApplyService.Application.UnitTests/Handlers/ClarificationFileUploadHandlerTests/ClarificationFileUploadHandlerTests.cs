using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Financial;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ClarificationFileUploadHandlerTests
{
    [TestFixture]
    public class ClarificationFileUploadHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private ClarificationFileUploadHandler _handler;
        private readonly Guid _applicationId = Guid.NewGuid();
        private ClarificationFileUploadRequest _request;
        private string _fileName = "file.pdf";
        [SetUp]
        public void TestSetup()
        {
            _request = new ClarificationFileUploadRequest(_applicationId,_fileName);
            _repository = new Mock<IApplyRepository>();
            _handler = new ClarificationFileUploadHandler(_repository.Object, Mock.Of<ILogger<ClarificationFileUploadHandler>>());
        }


        [Test]
        public void Update_Financial_Grade_with_clarification_Files_name_when_no_files_present()
        {
            var application = new Domain.Entities.Apply {FinancialGrade = new FinancialReviewDetails()};
            _repository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(application);
            _repository.Setup(x => x.UpdateFinancialReviewDetails(_applicationId, It.Is<FinancialReviewDetails>(g=>g.ClarificationFiles.First().Filename==_fileName))).ReturnsAsync(true);
            var result = _handler.Handle(_request, new CancellationToken()).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }


        [Test]
        public void Update_Financial_Grade_with_clarification_Files_name_when_one_file_present()
        {
            var application = new Domain.Entities.Apply { FinancialGrade = new FinancialReviewDetails {ClarificationFiles = new List<ClarificationFile> {new ClarificationFile {Filename = "first.pdf"}}} };
            _repository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(application);
            _repository.Setup(x => x.UpdateFinancialReviewDetails(_applicationId, It.Is<FinancialReviewDetails>(g => g.ClarificationFiles.Count==2))).ReturnsAsync(true);
            var result = _handler.Handle(_request, new CancellationToken()).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }
    }
}
