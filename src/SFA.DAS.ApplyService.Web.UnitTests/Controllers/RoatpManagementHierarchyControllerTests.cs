using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Controllers.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.UnitTests.Controllers
{
    [TestFixture]
    public class RoatpManagementHierarchyControllerTests
    {
        private  RoatpManagementHierarchyController _controller;
        private  Mock<ISessionService> _sessionService;
        private  Mock<IQnaApiClient> _qnaApiClient;
        private  Mock<ITabularDataRepository> _tabularDataRepository;
        private  Guid _applicationId;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();

            _sessionService = new Mock<ISessionService>();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _tabularDataRepository = new Mock<ITabularDataRepository>();

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
            _controller = new RoatpManagementHierarchyController(_sessionService.Object, _qnaApiClient.Object, _tabularDataRepository.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = user }
                }
            };
        }

        [Test]
        public  void AddManagementHierarchy_ReturnsAddManagementHierarchyPage()
        {
            var result =  _controller.AddManagementHierarchy(_applicationId);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("AddManagementHierarchy.cshtml");
        }

        [Test]
        public async Task AddManagementHierarchyDetails_HasValidationErrors_ReturnsView()
        {
            var model = new AddEditManagementHierarchyViewModel { ApplicationId = _applicationId };
            _controller.ModelState.AddModelError("Testing", "test message");

            var result = await _controller.AddManagementHierarchyDetails(model);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("AddManagementHierarchy.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task EditManagementHierarchy_Get_ReturnsConfirmManagementHierarchy()
        {
            var index = 0;
            TabularData personTableData = null;
            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddManagementHierarchy)).ReturnsAsync(personTableData);
            var result = await _controller.EditManagementHierarchy(_applicationId, index);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ConfirmManagementHierarchy");

        }

        [Test]
        public async Task EditManagementHierarchy_InvalidIndex_ReturnsEditPage()
        {
            var index = 0;
            var personTableData = new TabularData
            {
                HeadingTitles = new List<string> 
                { "First Name", "Last Name","Job role", "TimeInRoleMonths", "TimeInRoleYears", "isPartOfOtherOrg", "otherOrgName", "dobMonth","dobYear","email","contactNumber"},
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "T1", "T1","Test","1","1","","","1", "1900","t1@t1.com","1234567890" }
                    },
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "T2", "T2","Test","1","1","","","1", "1900","t2@t2.com","1234567890" }
                    }
                }
            };
            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddManagementHierarchy)).ReturnsAsync(personTableData);
            var result = await _controller.EditManagementHierarchy(_applicationId, index);
            
            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("EditManagementHierarchy.cshtml");
        }

        [Test]
        public async Task EditManagementHierarchy_ValidIndex_ReturnsConfirmPage()
        {
            var index = 1;
            var personTableData = new TabularData
            {
                HeadingTitles = new List<string>
                { "First Name", "Last Name","Job role", "TimeInRoleMonths", "TimeInRoleYears", "isPartOfOtherOrg", "otherOrgName", "dobMonth","dobYear","email","contactNumber"},
                DataRows = new List<TabularDataRow>
                {
                    new TabularDataRow
                    {
                        Id = Guid.NewGuid().ToString(),
                        Columns = new List<string> { "T1", "T1","Test","1","1","","","1", "1900","t1@t1.com","1234567890" }
                    },
                }
            };
            _tabularDataRepository.Setup(x => x.GetTabularDataAnswer(It.IsAny<Guid>(), RoatpWorkflowQuestionTags.AddManagementHierarchy)).ReturnsAsync(personTableData);
            var result = await _controller.EditManagementHierarchy(_applicationId, index);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ConfirmManagementHierarchy");
        }

        [Test]
        public async Task UpdateManagementHierarchyDetails_HasValidationErrors_ReturnsEditManagementHierarchy()
        {
            var model = new AddEditManagementHierarchyViewModel { ApplicationId = _applicationId };
            _controller.ModelState.AddModelError("Testing", "test message");

            var result = await _controller.UpdateManagementHierarchyDetails(model);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("EditManagementHierarchy.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task RemoveManagementHierarchy_HasValidationErrors_ReturnsConfirmManagementHierarchyRemoval()
        {
            var model = new ConfirmRemoveManagementHierarchyViewModel { ApplicationId = _applicationId };
            _controller.ModelState.AddModelError("Testing", "test message");

            var result = await _controller.RemoveManagementHierarchy(model);

            var viewResult = result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult.ViewName.Should().Contain("ConfirmManagementHierarchyRemoval.cshtml");
            viewResult.Model.Should().BeEquivalentTo(model);
        }

        [Test]
        public async Task RemoveManagementHierarchy_Post_ReturnsConfirmManagementHierarchy()
        {
            var model = new ConfirmRemoveManagementHierarchyViewModel { ApplicationId = _applicationId , Confirmation="No"};

            var result = await _controller.RemoveManagementHierarchy(model);

            var viewResult = result as RedirectToActionResult;
            viewResult.Should().NotBeNull();
            viewResult.ActionName.Should().Be("ConfirmManagementHierarchy");
        }
    }
}
