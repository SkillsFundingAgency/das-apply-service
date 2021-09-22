using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Types;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;
using AppealFile = SFA.DAS.ApplyService.InternalApi.Types.Responses.Appeals.AppealFile;

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
        private Mock<IApplicationApiClient> _applicationApiClient;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _applicationDeterminedDate = DateTime.Today;

            _outcomeApiClient = new Mock<IOutcomeApiClient>();
            _appealsApiClient = new Mock<IAppealsApiClient>();
            _applicationApiClient = new Mock<IApplicationApiClient>();  

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

            _controller = new RoatpAppealsController(_outcomeApiClient.Object, _appealsApiClient.Object,_applicationApiClient.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user },
                },
                TempData = Mock.Of<ITempDataDictionary>()
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
        public async Task MakeAppeal_shows_ProcessApplicationStatus_page_if_outside_appeal_window()
        {
            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = _applicationDeterminedDate };
            _outcomeApiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(oversightReview);
            _outcomeApiClient.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(_applicationDeterminedDate.AddDays(-10));

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ProcessApplicationStatus");
        }

        [Test]
        public async Task MakeAppeal_shows_ProcessApplicationStatus_page_if_appeal_already_submitted()
        {
            var appeal = new GetAppealResponse { Status = AppealStatus.Submitted, AppealSubmittedDate = DateTime.UtcNow };
            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync(appeal);

            var result = await _controller.MakeAppeal(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ProcessApplicationStatus");
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
        public async Task GroundsOfAppeal_shows_ProcessApplicationStatus_page_if_outside_appeal_window()
        {
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var oversightReview = new GetOversightReviewResponse { Status = OversightReviewStatus.Unsuccessful, ApplicationDeterminedDate = _applicationDeterminedDate };
            _outcomeApiClient.Setup(x => x.GetOversightReview(_applicationId)).ReturnsAsync(oversightReview);
            _outcomeApiClient.Setup(x => x.GetWorkingDaysAheadDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(_applicationDeterminedDate.AddDays(-10));

            var result = await _controller.GroundsOfAppeal(_applicationId, _appealOnPolicyOrProcesses, _appealOnEvidenceSubmitted);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ProcessApplicationStatus");
        }

        [Test]
        public async Task GroundsOfAppeal_shows_ProcessApplicationStatus_page_if_appeal_already_submitted()
        {
            var _appealOnPolicyOrProcesses = false;
            var _appealOnEvidenceSubmitted = false;

            var appeal = new GetAppealResponse { Status = AppealStatus.Submitted, AppealSubmittedDate = DateTime.UtcNow };
            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync(appeal);

            var result = await _controller.GroundsOfAppeal(_applicationId, _appealOnPolicyOrProcesses, _appealOnEvidenceSubmitted);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ProcessApplicationStatus");
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
                HowFailedOnPolicyOrProcesses = howFailedOnPolicyOrProcesses,
                FormAction = GroundsOfAppealViewModel.SUBMIT_APPEAL_FORMACTION
            };

            var result = await _controller.GroundsOfAppeal(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("AppealSubmitted");
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
                HowFailedOnPolicyOrProcesses = howFailedOnPolicyOrProcesses,
                FormAction = GroundsOfAppealViewModel.SUBMIT_APPEAL_FORMACTION
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
        public async Task POST_GroundsOfAppeal_shows_GroundsOfAppeal_page_when_valid_DELETE_APPEALFILE_FORMACTION()
        {
            string fileName = "test.pdf";

            var model = new GroundsOfAppealViewModel
            {
                ApplicationId = _applicationId,
                AppealOnEvidenceSubmitted = true,
                AppealOnPolicyOrProcesses = true,
                FormAction = $"{GroundsOfAppealViewModel.DELETE_APPEALFILE_FORMACTION}{GroundsOfAppealViewModel.FORMACTION_SEPERATOR}{fileName}",
            };

            var result = await _controller.GroundsOfAppeal(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("GroundsOfAppeal");
        }

        [Test]
        public async Task POST_GroundsOfAppeal_verify_DeleteFile_api_call_when_valid_DELETE_APPEALFILE_FORMACTION()
        {
            string fileName = "test.pdf";

            var model = new GroundsOfAppealViewModel
            {
                ApplicationId = _applicationId,
                AppealOnEvidenceSubmitted = true,
                AppealOnPolicyOrProcesses = true,
                FormAction = $"{GroundsOfAppealViewModel.DELETE_APPEALFILE_FORMACTION}{GroundsOfAppealViewModel.FORMACTION_SEPERATOR}{fileName}",
            };

            var result = await _controller.GroundsOfAppeal(model);

            _appealsApiClient.Verify(x => x.DeleteFile(_applicationId, fileName, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task AppealSubmitted_shows_ProcessApplicationStatus_page_if_no_appeal_submitted()
        {
            var result = await _controller.AppealSubmitted(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ProcessApplicationStatus");
        }

        [Test]
        public async Task AppealSubmitted_shows_Submitted_page_if_appeal_submitted()
        {
            var model = new AppealSubmittedViewModel
            {
                ApplicationId = _applicationId,
                AppealSubmittedDate = DateTime.UtcNow,
                HowFailedOnEvidenceSubmitted = "valid input",
                HowFailedOnPolicyOrProcesses = "valid input",
                AppealFiles = new List<AppealFile>()
            };

            var appeal = new GetAppealResponse 
            { 
                Status = AppealStatus.Submitted,
                ApplicationId = model.ApplicationId,
                AppealSubmittedDate = model.AppealSubmittedDate,
                HowFailedOnEvidenceSubmitted = model.HowFailedOnEvidenceSubmitted,
                HowFailedOnPolicyOrProcesses = model.HowFailedOnPolicyOrProcesses,
                AppealFiles = model.AppealFiles
            };
            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync(appeal);

            var result = await _controller.AppealSubmitted(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("AppealSubmitted.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task CancelAppeal_shows_ProcessApplicationStatus_page()
        {
            var result = await _controller.CancelAppeal(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ProcessApplicationStatus");
        }

        [Test]
        public async Task CancelAppeal_verify_CancelAppeal_api_call()
        {
            var result = await _controller.CancelAppeal(_applicationId);

            _appealsApiClient.Verify(x => x.CancelAppeal(_applicationId, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task AppealUnsuccessful_shows_ProcessApplicationStatus_page_if_appeal_is_not_deemed_unsuccessful()
        {
            var result = await _controller.AppealUnsuccessful(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ProcessApplicationStatus");
        }

        [Test]
        public async Task AppealSubmitted_shows_Unsuccessful_page_if_appeal_deemed_unsuccessful()
        {
            var model = new AppealUnsuccessfulViewModel
            {
                ApplicationId = _applicationId,
                AppealSubmittedDate = DateTime.UtcNow.AddDays(-1),
                AppealDeterminedDate = DateTime.UtcNow,
                AppealedOnEvidenceSubmitted = true,
                AppealedOnPolicyOrProcesses = true,
                ExternalComments = "You were unsuccessful"
            };

            var appeal = new GetAppealResponse
            {
                Status = AppealStatus.Unsuccessful,
                ApplicationId = model.ApplicationId,
                AppealSubmittedDate = model.AppealSubmittedDate,
                AppealDeterminedDate = model.AppealDeterminedDate,
                HowFailedOnEvidenceSubmitted = "valid input",
                HowFailedOnPolicyOrProcesses = "valid input",
                ExternalComments = model.ExternalComments
            };
            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync(appeal);

            var result = await _controller.AppealUnsuccessful(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("AppealUnsuccessful.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [TestCase(AppealStatus.Successful, false, "AppealSuccessful.cshtml")]
        [TestCase(AppealStatus.SuccessfulAlreadyActive, false,"AppealSuccessfulAlreadyActive.cshtml")]
        [TestCase(AppealStatus.SuccessfulFitnessForFunding, false,"AppealSuccessfulFitnessForFunding.cshtml")]
        [TestCase(AppealStatus.Successful, true, "AppealSuccessfulSupporting.cshtml")]
        [TestCase(AppealStatus.SuccessfulAlreadyActive, true, "AppealSuccessfulSupportingAlreadyActive.cshtml")]
        [TestCase(AppealStatus.SuccessfulFitnessForFunding, true, "AppealSuccessfulSupportingFitnessForFunding.cshtml")]

        public async Task AppealSubmitted_shows_Successful_page_if_appeal_deemed_Successful(string appealStatus, bool isSupporting,string expectedPage)
        {
            var model = new AppealSuccessfulViewModel
            {
                ApplicationId = _applicationId,
                AppealSubmittedDate = DateTime.UtcNow.AddDays(-1),
                AppealDeterminedDate = DateTime.UtcNow,
                AppealedOnEvidenceSubmitted = true,
                AppealedOnPolicyOrProcesses = true,
                ExternalComments = "You were successful"
            };

            var appeal = new GetAppealResponse
            {
                Status = appealStatus,
                ApplicationId = model.ApplicationId,
                AppealSubmittedDate = model.AppealSubmittedDate,
                AppealDeterminedDate = model.AppealDeterminedDate,
                HowFailedOnEvidenceSubmitted = "valid input",
                HowFailedOnPolicyOrProcesses = "valid input",
                ExternalComments = model.ExternalComments
            };

            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync(appeal);

            var route = Domain.Roatp.ApplicationRoute.MainProviderApplicationRoute;

            if (isSupporting)
                route = Domain.Roatp.ApplicationRoute.SupportingProviderApplicationRoute;

            var application = new Apply
            {
                ApplyData = new ApplyData {ApplyDetails = new ApplyDetails {ProviderRoute = route}}
            };

            _applicationApiClient.Setup(x => x.GetApplication(_applicationId)).ReturnsAsync(application);
            var result = await _controller.AppealSuccessful(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain(expectedPage);
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task AppealSubmitted_shows_tasklist_if_appeal_does_not_exist()
        {
            _appealsApiClient.Setup(x => x.GetAppeal(_applicationId)).ReturnsAsync((GetAppealResponse)null);

            var result = await _controller.AppealSuccessful(_applicationId);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ProcessApplicationStatus");
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
    }
}
