using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Assessor;
using SFA.DAS.ApplyService.Application.Apply.Financial;
using SFA.DAS.ApplyService.Domain.Apply.Assessor;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.ClarificationFileUploadHandlerTests
{
    [TestFixture]
    public class ClarificationFileUploadHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private ClarificationFileUploadHandler _handler;
        private Guid _applicationId = Guid.NewGuid();
        private FinancialReviewDetails _financialGrade;
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
        public async Task Update_Financial_Grade_with_clarification_Files_name_when_no_files_present()
        {
            var financialGrade = new FinancialReviewDetails();
            var application = new Domain.Entities.Apply {FinancialGrade = new FinancialReviewDetails()};
            var clarificationFiles = new List<ClarificationFile> { new ClarificationFile { Filename = _fileName } };
            financialGrade.ClarificationFiles = clarificationFiles;
            _repository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(application);
         
            _repository.Setup(x => x.UpdateFinancialReviewDetails(_applicationId, It.Is<FinancialReviewDetails>(g=>g.ClarificationFiles.First().Filename==_fileName))).ReturnsAsync(true);
            var result = _handler.Handle(_request, new CancellationToken()).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }


        [Test]
        public async Task Update_Financial_Grade_with_clarification_Files_name_when_one_file_present()
        {
            var financialGrade = new FinancialReviewDetails();
            var application = new Domain.Entities.Apply { FinancialGrade = new FinancialReviewDetails {ClarificationFiles = new List<ClarificationFile> {new ClarificationFile {Filename = "first.pdf"}}} };
            var clarificationFiles = new List<ClarificationFile> { new ClarificationFile { Filename = _fileName } };
            financialGrade.ClarificationFiles = clarificationFiles;
            _repository.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(application);

            _repository.Setup(x => x.UpdateFinancialReviewDetails(_applicationId, It.Is<FinancialReviewDetails>(g => g.ClarificationFiles.Count==2))).ReturnsAsync(true);
            var result = _handler.Handle(_request, new CancellationToken()).GetAwaiter().GetResult();
            Assert.IsTrue(result);
        }
    }
}
