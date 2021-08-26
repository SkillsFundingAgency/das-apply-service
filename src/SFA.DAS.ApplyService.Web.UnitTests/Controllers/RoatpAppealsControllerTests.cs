using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpAppealsControllerTests
    {
        private Guid _applicationId;
        private DateTime _applicationDeterminedDate;

        private RoatpAppealsController _controller;
        private Mock<IOutcomeApiClient> _outcomeApiClient;
        private Mock<IAppealsApiClient> _appealsApiClient;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _applicationDeterminedDate = DateTime.Today;

            _outcomeApiClient = new Mock<IOutcomeApiClient>();
            _appealsApiClient = new Mock<IAppealsApiClient>();

            var signInId = Guid.NewGuid();
            var givenNames = "Test";
            var familyName = "User";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, $"{givenNames} {familyName}"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("Email", "test@test.com"),
                new Claim("sub", signInId.ToString()),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = _applicationDeterminedDate };
            _outcomeApiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(oversightReview);

            _outcomeApiClient.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(_applicationDeterminedDate.AddDays(10));

            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync(default(GetAppealResponse));

            _appealsApiClient.Setup(x => x.GetAppealFileList(_applicationId)).ReturnsAsync(default(GetAppealFileListResponse));

            _controller = new RoatpAppealsController(_outcomeApiClient.Object, _appealsApiClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
        }

        [Test]
        public async Task MakeAppeal_shows_make_appeal_page_if_within_appeal_window()
        {
            var model = new MakeAppealViewModel { ApplicationId = _applicationId };

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("MakeAppeal.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task MakeAppeal_shows_Tasklist_page_if_outside_appeal_window()
        {
            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = _applicationDeterminedDate };
            _outcomeApiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(oversightReview);
            _outcomeApiClient.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(_applicationDeterminedDate.AddDays(-10));

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public async Task MakeAppeal_shows_Tasklist_page_if_appeal_already_submitted()
        {
            var appeal = new GetAppealResponse { Status = AppealStatus.Submitted };
            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync(appeal);

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public async Task POST_MakeAppeal_shows_GroundsOfAppeal_page_when_valid_input_submitted()
        {
            var model = new MakeAppealViewModel
            {
                ApplicationId = _applicationId,
                AppealOnEvidenceSubmitted = true,
                AppealOnPolicyOrProcesses = true
            };

            var result = await _controller.MakeAppeal(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("GroundsOfAppeal");
        }

        [Test]
        public async Task GroundsOfAppeal_shows_make_appeal_page_if_within_appeal_window()
        {
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var model = new GroundsOfAppealViewModel { ApplicationId = _applicationId, AppealOnPolicyOrProcesses = _appealOnPolicyOrProcesses, AppealOnEvidenceSubmitted = _appealOnEvidenceSubmitted };

            var result = await _controller.GroundsOfAppeal(_applicationId, _appealOnPolicyOrProcesses, _appealOnEvidenceSubmitted);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("GroundsOfAppeal.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task GroundsOfAppeal_shows_Tasklist_page_if_outside_appeal_window()
        {
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = _applicationDeterminedDate };
            _outcomeApiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(oversightReview);
            _outcomeApiClient.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(_applicationDeterminedDate.AddDays(-10));

            var result = await _controller.GroundsOfAppeal(_applicationId, _appealOnPolicyOrProcesses, _appealOnEvidenceSubmitted);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public async Task GroundsOfAppeal_shows_Tasklist_page_if_appeal_already_submitted()
        {
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var appeal = new GetAppealResponse { Status = AppealStatus.Submitted };
            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync(appeal);

            var result = await _controller.GroundsOfAppeal(_applicationId, _appealOnPolicyOrProcesses, _appealOnEvidenceSubmitted);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("TaskList");
        }

        [Test]
        public async Task POST_GroundsOfAppeal_shows_AppealSubmitted_page_when_valid_input_submitted()
        {
            var howFailedOnEvidenceSubmitted = "valid input";
            var howFailedOnPolicyOrProcesses = "valid input";

            var model = new GroundsOfAppealViewModel
            {
                ApplicationId = _applicationId,
                AppealOnEvidenceSubmitted = !string.IsNullOrEmpty(howFailedOnEvidenceSubmitted),
                HowFailedOnEvidenceSubmitted = howFailedOnEvidenceSubmitted,
                AppealOnPolicyOrProcesses = !string.IsNullOrEmpty(howFailedOnPolicyOrProcesses),
                HowFailedOnPolicyOrProcesses = howFailedOnPolicyOrProcesses
            };

            var result = await _controller.GroundsOfAppeal(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("IsAppealSubmitted");
        }

        [Test]
        public async Task POST_GroundsOfAppeal_verify_MakeAppeal_api_call_when_valid_input_submitted()
        {
            var howFailedOnEvidenceSubmitted = "valid input";
            var howFailedOnPolicyOrProcesses = "valid input";

            var model = new GroundsOfAppealViewModel
            {
                ApplicationId = _applicationId,
                AppealOnEvidenceSubmitted = !string.IsNullOrEmpty(howFailedOnEvidenceSubmitted),
                HowFailedOnEvidenceSubmitted = howFailedOnEvidenceSubmitted,
                AppealOnPolicyOrProcesses = !string.IsNullOrEmpty(howFailedOnPolicyOrProcesses),
                HowFailedOnPolicyOrProcesses = howFailedOnPolicyOrProcesses
            };

            var result = await _controller.GroundsOfAppeal(model);

            _appealsApiClient.Verify(x => x.MakeAppeal(_applicationId, howFailedOnPolicyOrProcesses, howFailedOnEvidenceSubmitted, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task POST_GroundsOfAppeal_shows_GroundsOfAppeal_page_when_valid_UPLOAD_APPEALFILE_FORMACTION()
        {
            var appealFile = Mock.Of<IFormFile>();

            var model = new GroundsOfAppealViewModel
            {
                ApplicationId = _applicationId,
                AppealOnEvidenceSubmitted = true,
                AppealOnPolicyOrProcesses = true,
                FormAction = GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION,
                AppealFileToUpload = appealFile
            };

            var result = await _controller.GroundsOfAppeal(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("GroundsOfAppeal");
        }

        [Test]
        public async Task POST_GroundsOfAppeal_verify_UploadFile_api_call_when_valid_UPLOAD_APPEALFILE_FORMACTION()
        {
            var appealFile = Mock.Of<IFormFile>();

            var model = new GroundsOfAppealViewModel
            {
                ApplicationId = _applicationId,
                AppealOnEvidenceSubmitted = true,
                AppealOnPolicyOrProcesses = true,
                FormAction = GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION,
                AppealFileToUpload = appealFile
            };

            var result = await _controller.GroundsOfAppeal(model);

            _appealsApiClient.Verify(x => x.UploadFile(_applicationId, appealFile, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }


        [Test]
        public void AppealSubmitted_shows_appeal_submitted_page()
        {
            var model = new AppealSubmittedViewModel { ApplicationId = _applicationId };

            var result = _controller.AppealSubmitted(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("IsAppealSubmitted.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task DownloadAppealFile_when_file_exists_downloads_the_requested_file()
        {
            string fileName = "test.pdf";
            string contentType = "application/pdf";

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new MemoryStream())
            { 
                Headers = 
                { 
                    ContentLength = 0,
                    ContentType = new MediaTypeHeaderValue(contentType),
                    ContentDisposition = new ContentDispositionHeaderValue("attachment"){ FileName = fileName, FileNameStar = fileName }
                }
            };

            _appealsApiClient.Setup(x => x.DownloadFile(_applicationId, fileName)).ReturnsAsync(response);

            var result = await _controller.DownloadAppealFile(_applicationId, fileName) as FileStreamResult;
            
            Assert.AreEqual(fileName, result.FileDownloadName);
            Assert.AreEqual(contentType, result.ContentType);
        }

        [Test]
        public async Task DownloadAppealFile_when_file_does_not_exists_then_gives_NotFound_result()
        {
            string fileName = "test.pdf";

            var response = new HttpResponseMessage(HttpStatusCode.NotFound);

            _appealsApiClient.Setup(x => x.DownloadFile(_applicationId, fileName)).ReturnsAsync(response);

            var result = await _controller.DownloadAppealFile(_applicationId, fileName) as NotFoundResult;
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteAppealFile_deletes_the_file_and_redirects_to_GroundsOfAppeal()
        {
            string fileName = "test.pdf";

            _appealsApiClient.Setup(x => x.DeleteFile(_applicationId, fileName, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var result = await _controller.DeleteAppealFile(_applicationId, fileName, true, true);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("GroundsOfAppeal");
        }
    }
}
