using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Financial;
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
        public void Remove_File_From_Financial_Review_Clarification_File()
        {
            _repository.Setup(x => x.RemoveFinancialReviewClarificationFile(_applicationId, FileName)).ReturnsAsync(true);
            var result = _handler.Handle(_request, new CancellationToken()).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }
        
    }
}
