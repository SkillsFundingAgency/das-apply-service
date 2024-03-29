﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Appeals.Commands.DeleteAppealFile;
using SFA.DAS.ApplyService.Application.Appeals.Commands.UploadAppealFile;
using SFA.DAS.ApplyService.Application.Appeals.Queries.GetAppealFile;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.CancelAppeal;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.MakeAppeal;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppeal;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Queries.GetAppealFileList;
using SFA.DAS.ApplyService.Domain.QueryResults;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Appeals;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Controllers
{
    public class AppealsControllerTests
    {
        private Mock<ILogger<AppealsController>> _logger;
        private Mock<IMediator> _mediator;
        private Mock<IFileStorageService> _fileStorageService;

        private AppealsController _controller;
        private static readonly Fixture AutoFixture = new Fixture();

        [SetUp]
        public void Before_each_test()
        {
            _logger = new Mock<ILogger<AppealsController>>();
            _mediator = new Mock<IMediator>();
            _fileStorageService = new Mock<IFileStorageService>();

            _controller = new AppealsController(_logger.Object, _mediator.Object, _fileStorageService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task GetAppeal_gets_Appeal_for_Application()
        {
            var request = new GetAppealRequest();
            var queryResult = AutoFixture.Create<Appeal>();

            _mediator.Setup(x => x.Send(It.IsAny<GetAppealQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);

            var result = await _controller.GetAppeal(request);
            result.Should().BeOfType<ActionResult<GetAppealResponse>>();

            var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
            var comparisonResult = compareLogic.Compare(queryResult, result);
            Assert.IsTrue(comparisonResult.AreEqual);
        }

        [Test]
        public async Task MakeAppeal_sends_Command_to_make_a_new_Appeal()
        {
            var applicationId = AutoFixture.Create<Guid>();
            var request = AutoFixture.Create<MakeAppealRequest>();

            _mediator.Setup(x => x.Send(It.IsAny<MakeAppealCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var result = await _controller.MakeAppeal(applicationId, request);
            Assert.IsInstanceOf<OkResult>(result);

            _mediator.Verify(x => x.Send(It.Is<MakeAppealCommand>(c =>
                c.ApplicationId == applicationId &&
                c.HowFailedOnPolicyOrProcesses == request.HowFailedOnPolicyOrProcesses &&
                c.HowFailedOnEvidenceSubmitted == request.HowFailedOnEvidenceSubmitted &&
                c.UserId == request.UserId &&
                c.UserName == request.UserName), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task CancelAppeal_removes_all_AppealFiles_from_Application_Appeal()
        {
            var applicationId = AutoFixture.Create<Guid>();

            _fileStorageService.Setup(x => x.DeleteApplicationDirectory(applicationId, It.IsAny<ContainerType>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var request = AutoFixture.Create<CancelAppealRequest>();

            var result = await _controller.CancelAppeal(applicationId, request);
            result.Should().BeOfType<OkResult>();

            _fileStorageService.Verify(x => x.DeleteApplicationDirectory(applicationId, It.IsAny<ContainerType>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task CancelAppeal_sends_Command_to_cancel_Appeal()
        {
            var applicationId = AutoFixture.Create<Guid>();
            var request = AutoFixture.Create<CancelAppealRequest>();

            _mediator.Setup(x => x.Send(It.IsAny<CancelAppealCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var result = await _controller.CancelAppeal(applicationId, request);
            Assert.IsInstanceOf<OkResult>(result);

            _mediator.Verify(x => x.Send(It.Is<CancelAppealCommand>(c =>
                c.ApplicationId == applicationId &&
                c.UserId == request.UserId &&
                c.UserName == request.UserName), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task GetAppealFileList_gets_Files_for_Application_Appeal()
        {
            var request = new GetAppealFileListRequest();
            var queryResult = new List<Domain.QueryResults.AppealFile>
            {
                new Domain.QueryResults.AppealFile { Id = Guid.NewGuid(), FileName = AutoFixture.Create<string>() }
            };

            _mediator.Setup(x => x.Send(It.IsAny<GetAppealFileListQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);

            var result = await _controller.GetAppealFileList(request);
            result.Should().BeOfType<ActionResult<GetAppealFileListResponse>>();

            var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
            var comparisonResult = compareLogic.Compare(queryResult, result.Value.AppealFiles);
            Assert.IsTrue(comparisonResult.AreEqual);
        }

        [Test]
        public async Task UploadAppealFile_adds_Uploaded_File_to_Application_Appeal()
        {
            var request = new UploadAppealFileRequest
            {
                ApplicationId = AutoFixture.Create<Guid>(),
                AppealFile = GenerateFile(),
                UserId = AutoFixture.Create<string>(),
                UserName = AutoFixture.Create<string>()
            };

            _fileStorageService.Setup(x => x.UploadApplicationFiles(request.ApplicationId, It.IsAny<IFormFileCollection>(), It.IsAny<ContainerType>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            _mediator.Setup(x => x.Send(It.IsAny<UploadAppealFileCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var result = await _controller.UploadAppealFile(request);
            result.Should().BeOfType<OkResult>();

            _fileStorageService.Verify(x => x.UploadApplicationFiles(request.ApplicationId, It.IsAny<IFormFileCollection>(), It.IsAny<ContainerType>(), It.IsAny<CancellationToken>()), Times.Once);

            string expectedFileData;
            using (var reader = new StreamReader(request.AppealFile.OpenReadStream()))
            {
                expectedFileData = await reader.ReadToEndAsync();
            }

            _mediator.Verify(x => x.Send(It.Is<UploadAppealFileCommand>(c =>
                    c.ApplicationId == request.ApplicationId
                    && c.UserId == request.UserId
                    && c.UserName == request.UserName
                    && c.AppealFile.FileName == request.AppealFile.FileName
                    && Encoding.UTF8.GetString(c.AppealFile.Data) == expectedFileData),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task GetAppealFile_gets_AppealFile_for_Application_Appeal()
        {
            var applicationId = AutoFixture.Create<Guid>();
            var fileName = AutoFixture.Create<string>();
            var contentType = "application/pdf";

            var request = new GetAppealFileRequest { ApplicationId = applicationId, FileName = fileName };
            var appealFile = new Domain.QueryResults.AppealFile { ApplicationId = applicationId, FileName = fileName, ContentType = contentType };
            _mediator.Setup(x => x.Send(It.IsAny<GetAppealFileQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(appealFile);

            var storageFile = new DownloadFile { Stream = new MemoryStream(), ContentType = contentType, FileName = fileName };
            _fileStorageService.Setup(x => x.DownloadApplicationFile(applicationId, storageFile.FileName, It.IsAny<ContainerType>(), It.IsAny<CancellationToken>())).ReturnsAsync(storageFile);

            var result = await _controller.GetAppealFile(request);
            result.Should().BeOfType<FileStreamResult>();

            var fileResult = result as FileStreamResult;

            Assert.AreEqual(storageFile.FileName, fileResult.FileDownloadName);
            Assert.AreEqual(storageFile.ContentType, fileResult.ContentType);
        }

        [TestCase(AppealStatus.Successful)]
        [TestCase(AppealStatus.Unsuccessful)]
        public async Task Record_appeal_outcome_updates_appeal_status(string appealStatus)
        {
            var command = new RecordAppealOutcomeCommand
            {
                AppealStatus = appealStatus,
                ApplicationId = Guid.NewGuid(),
                UserId = "User Id",
                UserName = "Test user"
            };

            _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.RecordAppealOutcome(command);
            result.Should().NotBeNull();
            result.Value.Should().BeTrue();
        }

        [Test]
        public async Task DeleteAppealFile_removes_AppealFile_from_Application_Appeal()
        {
            var applicationId = AutoFixture.Create<Guid>();
            var fileName = AutoFixture.Create<string>();

            var request = new DeleteAppealFileRequest
            {
                UserId = AutoFixture.Create<string>(),
                UserName = AutoFixture.Create<string>()
            };

            _mediator.Setup(x => x.Send(It.IsAny<DeleteAppealFileCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            _fileStorageService.Setup(x => x.DeleteApplicationFile(applicationId, fileName, It.IsAny<ContainerType>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.DeleteAppealFile(applicationId, fileName, request);
            result.Should().BeOfType<OkResult>();

            _mediator.Verify(
                x => x.Send(It.Is<DeleteAppealFileCommand>(c =>
                    c.ApplicationId == applicationId
                    && c.FileName == fileName
                    && c.UserId == request.UserId
                    && c.UserName == request.UserName),
                    It.IsAny<CancellationToken>()), Times.Once);

            _fileStorageService.Verify(x => x.DeleteApplicationFile(applicationId, fileName, It.IsAny<ContainerType>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private static IFormFile GenerateFile()
        {
            var fileName = AutoFixture.Create<string>();
            var content = AutoFixture.Create<string>();
            return new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(content)),
                0,
                content.Length,
                fileName,
                fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };
        }
    }
}
