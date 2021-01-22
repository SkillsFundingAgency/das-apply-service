using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Application.Apply.GetApplications;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.AutoMapper;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using SFA.DAS.ApplyService.InternalApi.Types.Requests;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpGatewayControllerTests
    {
        private Mock<IApplyRepository> _applyRepository;
        private Mock<ILogger<RoatpGatewayController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IGatewayApiChecksService> _gatewayApiChecksService;
        private Mock<IFileStorageService> _fileStorage;
        private const string _twoInTwelveMonthsPageId = "TwoInTwelveMonths";
        private const string _userName = "John Smith";
        private const string _userId = "user id 123";

        private Guid _applicationId;
        private Apply _application;
        private RoatpGatewayController _controller;

        [OneTimeSetUp]
        public void Before_all_tests()
        {
            Mapper.Reset();

            Mapper.Initialize(cfg =>
            {

                cfg.AddProfile<UkrlpCharityCommissionProfile>();
                cfg.AddProfile<UkrlpCompaniesHouseProfile>();
            });

            Mapper.AssertConfigurationIsValid();
        }

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();

            _application = new Apply
            {
                ApplicationId = _applicationId,
                GatewayReviewStatus = GatewayReviewStatus.New,
                ApplyData = new ApplyData()
            };

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.GetConfig()).ReturnsAsync(new ApplyConfig { RoatpApiAuthentication = new RoatpApiAuthentication { ApiBaseAddress = "https://localhost" } });

            _applyRepository = new Mock<IApplyRepository>();
            _logger = new Mock<ILogger<RoatpGatewayController>>();
            _fileStorage = new Mock<IFileStorageService>();
            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(_application);

            _gatewayApiChecksService = new Mock<IGatewayApiChecksService>();
            _gatewayApiChecksService.Setup(x => x.GetExternalApiCheckDetails(_applicationId, _userName)).ReturnsAsync(new ApplyGatewayDetails());

            _controller = new RoatpGatewayController(_applyRepository.Object, _logger.Object, _mediator.Object, _gatewayApiChecksService.Object, _fileStorage.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task GatewayPageSubmit_When_TwoInTwelveMonths_and_GatewayReviewStatus_New_Starts_GatewayReview()
        {
            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";

            var request = new Types.Requests.UpdateGatewayPageAnswerRequest
            {
                ApplicationId = _applicationId,
                Comments = gatewayReviewComment,
                Status = gatewayReviewStatus,
                UserId = _userId,
                UserName = _userName,
                PageId = _twoInTwelveMonthsPageId
            };
            await _controller.GatewayPageSubmit(request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediator.Verify(x => x.Send(It.IsAny<StartGatewayReviewRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GatewayPageSubmit_When_TwoInTwelveMonths_and_GatewayReviewStatus_InProgress_Does_Not_Start_GatewayReview()
        {
            _application.GatewayReviewStatus = GatewayReviewStatus.InProgress;

            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";

            var request = new Types.Requests.UpdateGatewayPageAnswerRequest
            {
                ApplicationId = _applicationId,
                Comments = gatewayReviewComment,
                Status = gatewayReviewStatus,
                UserId = _userId,
                UserName = _userName,
                PageId = _twoInTwelveMonthsPageId
            };

            await _controller.GatewayPageSubmit(request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediator.Verify(x => x.Send(It.IsAny<StartGatewayReviewRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task GatewayPageSubmit_When_Pass_TwoInTwelveMonths_And_GatewayReviewDetails_Null_Saves_ExternalApi_Checks()
        {
            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";

            var request = new Types.Requests.UpdateGatewayPageAnswerRequest
            {
                ApplicationId = _applicationId,
                Comments = gatewayReviewComment,
                Status = gatewayReviewStatus,
                UserId = _userId,
                UserName = _userName,
                PageId = _twoInTwelveMonthsPageId
            };

            await _controller.GatewayPageSubmit(request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _gatewayApiChecksService.Verify(x => x.GetExternalApiCheckDetails(_applicationId, _userName), Times.Once);
            _applyRepository.Verify(x => x.UpdateApplyData(_applicationId, It.IsAny<ApplyData>(), _userName), Times.Once);
        }

        [Test]
        public async Task UpdateGatewayReviewStatusAndComment_executes()
        {
            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";
            var gatewayReviewExternalComment = "Some external comment";

            _applyRepository.Setup(x => x.UpdateGatewayReviewStatusAndComment(_applicationId, It.IsAny<ApplyData>(), gatewayReviewStatus, _userId, _userName)).ReturnsAsync(true); 

            var request = new UpdateGatewayReviewStatusAndCommentRequest(_applicationId, gatewayReviewStatus, gatewayReviewComment, gatewayReviewExternalComment, _userId, _userName);
            await _controller.UpdateGatewayReviewStatusAndComment(request);

            _applyRepository.Verify(x => x.UpdateGatewayReviewStatusAndComment(_applicationId, It.IsAny<ApplyData>(), gatewayReviewStatus, _userId, _userName), Times.Once);
        }

        [Test]
        public async Task UploadClarificationFile_with_file_calls_file_upload()
        {
            var clarificationFileName = "file.pdf";
            var file = new FormFile(new MemoryStream(), 0, 0, clarificationFileName, clarificationFileName);
            var formFileCollection = new FormFileCollection { file };
            _controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);

            var command = new RoatpGatewayController.SubcontractorDeclarationClarificationFileCommand
            {
                UserId = "user id", UserName = "user name"
            };
            _fileStorage.Setup(x => x.UploadFiles(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, formFileCollection,
                ContainerType.Gateway,It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.UploadClarificationFile(_applicationId, command);

            var expectedResult = new OkResult();

            Assert.AreEqual(expectedResult.ToString(),result.ToString());
            _mediator.Verify(x => x.Send(It.IsAny<AddSubcontractorDeclarationFileUploadRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task UploadClarificationFile_without_file_calls_file_upload()
        { 
            var formFileCollection = new FormFileCollection();
            _controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);

            var command = new RoatpGatewayController.SubcontractorDeclarationClarificationFileCommand
            {
                UserId = "user id",
                UserName = "user name"
            };
            _fileStorage.Setup(x => x.UploadFiles(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, formFileCollection,
                ContainerType.Gateway, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.UploadClarificationFile(_applicationId, command);

            var expectedResult = new OkResult();

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            _mediator.Verify(x => x.Send(It.IsAny<AddSubcontractorDeclarationFileUploadRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task UploadClarificationFile_with_file_calls_file_upload_which_fails_returns_bad_request()
        {
            var clarificationFileName = "file.pdf";
            var file = new FormFile(new MemoryStream(), 0, 0, clarificationFileName, clarificationFileName);
            var formFileCollection = new FormFileCollection { file };
            _controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);

            var command = new RoatpGatewayController.SubcontractorDeclarationClarificationFileCommand
            {
                UserId = "user id",
                UserName = "user name"
            };
            _fileStorage.Setup(x => x.UploadFiles(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, formFileCollection,
                ContainerType.Gateway, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var result = await _controller.UploadClarificationFile(_applicationId, command);

            var expectedResult = new BadRequestResult();

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            _mediator.Verify(x => x.Send(It.IsAny<AddSubcontractorDeclarationFileUploadRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        [Test]
        public async Task RemoveClarificationFile_removed_file_successfully()
        {
            var clarificationFileName = "file.pdf";
            
            var command = new RoatpGatewayController.SubcontractorDeclarationClarificationFileCommand
            {
                UserId = "user id",
                UserName = "user name",
                FileName = clarificationFileName
            };

            _fileStorage.Setup(x => x.DeleteFile(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, clarificationFileName,
                ContainerType.Gateway, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.RemoveClarificationFile(_applicationId, command);
            
            var expectedResult = new OkResult();

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            _mediator.Verify(x => x.Send(It.IsAny<RemoveSubcontractorDeclarationFileRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task RemoveClarificationFile_removed_file_unsuccessfully()
        {
            var clarificationFileName = "file.pdf";

            var command = new RoatpGatewayController.SubcontractorDeclarationClarificationFileCommand
            {
                UserId = "user id",
                UserName = "user name",
                FileName = clarificationFileName
            };

            _fileStorage.Setup(x => x.DeleteFile(_applicationId, RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, clarificationFileName,
                ContainerType.Gateway, It.IsAny<CancellationToken>())).ReturnsAsync(false);

            var result = await _controller.RemoveClarificationFile(_applicationId, command);

            var expectedResult = new BadRequestResult();

            Assert.AreEqual(expectedResult.ToString(), result.ToString());
            _mediator.Verify(x => x.Send(It.IsAny<RemoveSubcontractorDeclarationFileRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        [Test]
        public async Task GatewayPageSubmitClarification_When_GatewayReviewStatus_New_Starts_GatewayReview()
        {
            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";

            var request = new Types.Requests.UpdateGatewayPageAnswerRequest
            {
                ApplicationId = _applicationId,
                Comments = gatewayReviewComment,
                Status = gatewayReviewStatus,
                UserId = _userId,
                UserName = _userName,
                PageId = _twoInTwelveMonthsPageId
            };
            await _controller.GatewayPageSubmitClarification(request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediator.Verify(x => x.Send(It.IsAny<StartGatewayReviewRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Test]
        public async Task GatewayPageSubmitClarification_When_GatewayReviewStatus_InProgress_Does_Not_Start_GatewayReview()
        {
            _application.GatewayReviewStatus = GatewayReviewStatus.InProgress;

            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";

            var request = new Types.Requests.UpdateGatewayPageAnswerRequest
            {
                ApplicationId = _applicationId,
                Comments = gatewayReviewComment,
                Status = gatewayReviewStatus,
                UserId = _userId,
                UserName = _userName,
                PageId = _twoInTwelveMonthsPageId
            };

            await _controller.GatewayPageSubmitClarification(request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediator.Verify(x => x.Send(It.IsAny<StartGatewayReviewRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }


        [Test]
        public async Task GatewayPageSubmitPostClarification_When_GatewayReviewStatus_InProgress_Does_Not_Start_GatewayReview()
        {
            _application.GatewayReviewStatus = GatewayReviewStatus.InProgress;

            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";

            var request = new Types.Requests.UpdateGatewayPageAnswerRequest
            {
                ApplicationId = _applicationId,
                Comments = gatewayReviewComment,
                Status = gatewayReviewStatus,
                UserId = _userId,
                UserName = _userName,
                PageId = _twoInTwelveMonthsPageId,
                ClarificationAnswer = "answer"
            };

            await _controller.GatewayPageSubmitPostClarification(request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediator.Verify(x => x.Send(It.IsAny<StartGatewayReviewRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

    }
}
