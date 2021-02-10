using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Financial;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ClarificationFileUploadHandlerTests
{
    [TestFixture]
    public class RemoveClarificationFileUploadHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private RemoveClarificationFileUploadHandler _handler;
        private readonly Guid _applicationId = Guid.NewGuid();
        private RemoveClarificationFileUploadRequest _request;
        private const string FileName = "file.pdf";
        [SetUp]
        public void TestSetup()
        {
            _request = new RemoveClarificationFileUploadRequest(_applicationId,FileName);
            _repository = new Mock<IApplyRepository>();
            _handler = new RemoveClarificationFileUploadHandler(_repository.Object, Mock.Of<ILogger<RemoveClarificationFileUploadHandler>>());
        }


        [Test]
        public void Remove_File_From_Financial_Grade_Where_There_Is_Only_One_File()
        {
            var application = new Domain.Entities.Apply { FinancialGrade = new FinancialReviewDetails {ClarificationFiles = new List<ClarificationFile> {new ClarificationFile {Filename = FileName}}} };
            _repository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(application);
            _repository.Setup(x => x.UpdateFinancialReviewDetails(_applicationId, It.IsAny<FinancialReviewDetails>())).ReturnsAsync(true);
            var result = _handler.Handle(_request, new CancellationToken()).GetAwaiter().GetResult();
            _repository.Verify(x=>x.UpdateFinancialReviewDetails(_applicationId, It.Is<FinancialReviewDetails>(g => g.ClarificationFiles.Count == 0)));
            Assert.IsTrue(result);
        }

        [Test]
        public void Remove_File_From_Financial_Grade_Where_There_Is_Two_Files()
        {
            var application = new Domain.Entities.Apply { FinancialGrade = new FinancialReviewDetails { ClarificationFiles = new List<ClarificationFile> { new ClarificationFile { Filename = FileName }, new ClarificationFile {Filename = "test.pdf"} } } };
            _repository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(application);
            _repository.Setup(x => x.UpdateFinancialReviewDetails(_applicationId, It.IsAny<FinancialReviewDetails>())).ReturnsAsync(true);
            var result = _handler.Handle(_request, new CancellationToken()).GetAwaiter().GetResult();
            _repository.Verify(x => x.UpdateFinancialReviewDetails(_applicationId, It.Is<FinancialReviewDetails>(g => g.ClarificationFiles.Count == 1)));
            Assert.IsTrue(result);
        }
    }
}
