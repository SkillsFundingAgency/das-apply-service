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
    public class ClarificationFileUploadHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private AddClarificationFileUploadHandler _handler;
        private readonly Guid _applicationId = Guid.NewGuid();
        private AddClarificationFileUploadRequest _request;
        private const string FileName = "file.pdf";
        [SetUp]
        public void TestSetup()
        {
            _request = new AddClarificationFileUploadRequest(_applicationId,FileName);
            _repository = new Mock<IApplyRepository>();
            _handler = new AddClarificationFileUploadHandler(_repository.Object, Mock.Of<ILogger<AddClarificationFileUploadHandler>>());
        }

        [Test]
        public void Update_Financial_Grade_with_clarification_FileName()
        {
            _repository.Setup(x => x.AddFinancialReviewClarificationFile(_applicationId,FileName)).ReturnsAsync(true);
            var result = _handler.Handle(_request, new CancellationToken()).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }
    }
}
