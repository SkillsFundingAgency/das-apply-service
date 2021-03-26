using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Gateway;
using SFA.DAS.ApplyService.Application.Apply.Gateway.ExternalApiCheckDetails;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class RoatpGatewayControllerTests
    {
        private Mock<ILogger<RoatpGatewayController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IGatewayApiChecksService> _gatewayApiChecksService;
        private Mock<IFileStorageService> _fileStorage;
        private const string _pageId = "Page1";
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
                ApplyData = new ApplyData
                {
                    ApplyDetails = new ApplyDetails(),
                    GatewayReviewDetails = new ApplyGatewayDetails()
                }
            };

            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.GetConfig()).ReturnsAsync(new ApplyConfig { RoatpApiAuthentication = new RoatpApiAuthentication { ApiBaseAddress = "https://localhost" } });

            _logger = new Mock<ILogger<RoatpGatewayController>>();
            _fileStorage = new Mock<IFileStorageService>();
            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(_application);
            _mediator.Setup(x => x.Send(It.IsAny<UpdateGatewayReviewStatusAndCommentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => Unit.Value);

            _gatewayApiChecksService = new Mock<IGatewayApiChecksService>();
            _gatewayApiChecksService.Setup(x => x.GetExternalApiCheckDetails(_applicationId, _userName)).ReturnsAsync(new ApplyGatewayDetails());


            _controller = new RoatpGatewayController(_logger.Object, _mediator.Object, _gatewayApiChecksService.Object, _fileStorage.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task GetGatewayPageCommonDetails_When_GatewayReviewStatus_New_Starts_GatewayReview()
        {
            _application.GatewayReviewStatus = GatewayReviewStatus.New;

            var request = new Types.Requests.GatewayCommonDetailsRequest
            {
                UserId = _userId,
                UserName = _userName,
                PageId = _pageId
            };
            await _controller.GetGatewayCommonDetails(_applicationId, request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _mediator.Verify(x => x.Send(It.IsAny<StartGatewayReviewRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetGatewayPageCommonDetails_When_GatewayReviewStatus_New_Saves_ExternalApi_Checks()
        {
            _application.GatewayReviewStatus = GatewayReviewStatus.New;

            var request = new Types.Requests.GatewayCommonDetailsRequest
            {
                UserId = _userId,
                UserName = _userName,
                PageId = _pageId
            };
            await _controller.GetGatewayCommonDetails(_applicationId, request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _gatewayApiChecksService.Verify(x => x.GetExternalApiCheckDetails(_applicationId, _userName), Times.Once);
            _mediator.Verify(x => x.Send(It.IsAny<UpdateExternalApiCheckDetailsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetGatewayPageCommonDetails_When_GatewayReviewStatus_InProgress_Does_Not_Start_GatewayReview()
        {
            _application.GatewayReviewStatus = GatewayReviewStatus.InProgress;

            var request = new Types.Requests.GatewayCommonDetailsRequest
            {
                UserId = _userId,
                UserName = _userName,
                PageId = _pageId
            };
            await _controller.GetGatewayCommonDetails(_applicationId, request);

            _mediator.Verify(x => x.Send(It.IsAny<GetApplicationRequest>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            _mediator.Verify(x => x.Send(It.IsAny<StartGatewayReviewRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediator.Verify(x => x.Send(It.IsAny<UpdateExternalApiCheckDetailsRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task UpdateGatewayReviewStatusAndComment_Updates_Review_Status()
        {
            var gatewayReviewStatus = GatewayReviewStatus.Pass;
            var gatewayReviewComment = "Some comment";
            var gatewayReviewExternalComment = "some external comment";

            var request = new UpdateGatewayReviewStatusAndCommentCommand(_applicationId, gatewayReviewStatus, gatewayReviewComment, gatewayReviewExternalComment, _userId, _userName);
            await _controller.UpdateGatewayReviewStatusAndComment(request);

            _mediator.Verify(x => x.Send(request, It.IsAny<CancellationToken>()), Times.Once);
        }



        [Test]
        public async Task UploadClarificationFile_with_file_calls_file_upload()
        {
            var clarificationFileName = "file.pdf";
            var file = new FormFile(new MemoryStream(), 0, 0, clarificationFileName, clarificationFileName);
            var formFileCollection = new FormFileCollection { file };
            _controller.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), formFileCollection);

            var command = new SubcontractorDeclarationClarificationFileCommand
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

            var command = new SubcontractorDeclarationClarificationFileCommand
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

            var command = new SubcontractorDeclarationClarificationFileCommand
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
            
            var command = new SubcontractorDeclarationClarificationFileCommand
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

            var command = new SubcontractorDeclarationClarificationFileCommand
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
    }
}
