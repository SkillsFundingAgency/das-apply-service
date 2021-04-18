using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.RemoveSubcontractorDeclarationFileHandlerTests
{
    [TestFixture]
    public class AddSubcontractorDeclarationFileUploadHandlerTests
    {
        private Mock<IApplyRepository> _repository;
        private Mock<IGatewayRepository> _gatewayRepository;
        private RemoveSubcontractorDeclarationFileHandler _handler;

        [SetUp]
        public void TestSetup()
        {
            _repository = new Mock<IApplyRepository>();
            _gatewayRepository = new Mock<IGatewayRepository>();
            _handler = new RemoveSubcontractorDeclarationFileHandler(_repository.Object, _gatewayRepository.Object, Mock.Of<ILogger<RemoveSubcontractorDeclarationFileHandler>>());
        }

        [Test]
        public async Task RemoveSubcontractorDeclarationFile_is_updated()
        {
            var applicationId = Guid.NewGuid();
            var clarificationFile = "file.pdf";
            var userId = "user id";
            var userName = "user name";

            var application = new Domain.Entities.Apply {ApplyData = new ApplyData {GatewayReviewDetails = new ApplyGatewayDetails
            {
                GatewaySubcontractorDeclarationClarificationUpload = clarificationFile
            }}};


            _repository.Setup(x => x.GetApplication(applicationId)).ReturnsAsync(application);

            await _handler.Handle(new RemoveSubcontractorDeclarationFileRequest(applicationId, clarificationFile, userId, userName), new CancellationToken());

            _gatewayRepository.Verify(x => x.UpdateGatewayApplyData(applicationId, It.IsAny<ApplyData>(), userId, userName), Times.Once);
        }


        [Test]
        public async Task RemoveSubcontractorDeclarationFile_swith_wrong_name_is_not_updated()
        {
            var applicationId = Guid.NewGuid();
            var clarificationFile = "file.pdf";
            var wrongFileName = "file2.pdf";
            var userId = "user id";
            var userName = "user name";

            var application = new Domain.Entities.Apply
            {
                ApplyData = new ApplyData
                {
                    GatewayReviewDetails = new ApplyGatewayDetails
                    {
                        GatewaySubcontractorDeclarationClarificationUpload = wrongFileName
                    }
                }
            };

            _repository.Setup(x => x.GetApplication(applicationId)).ReturnsAsync(application);

            await _handler.Handle(new RemoveSubcontractorDeclarationFileRequest(applicationId, clarificationFile, userId, userName), new CancellationToken());

            _gatewayRepository.Verify(x => x.UpdateGatewayApplyData(applicationId, It.IsAny<ApplyData>(), userId, userName), Times.Never);
        }
    }
}
