using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Authorization;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.UnitTests.Authorisation
{
    [TestFixture]
    public class AccessApplicationRequirementHandlerTests
    {
        private AuthorizationHandler _handler;

        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private Mock<IApplicationApiClient> _applicationApiClient;
        private Mock<IUserService> _userService;

        private Guid _applicationId;
        private Guid _userSignInId;
        private Apply _application;

        [SetUp]
        public void Setup()
        {
            _userSignInId = Guid.NewGuid();
            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetSignInId()).ReturnsAsync(_userSignInId);

            _applicationId = Guid.NewGuid();
            _application = new Apply { ApplicationId = _applicationId };
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _applicationApiClient.Setup(x => x.GetApplicationByUserId(_applicationId, _userSignInId))
                .ReturnsAsync(_application);

            var queryCollection = new QueryCollection(new Dictionary<string, StringValues> {{ "ApplicationId", _applicationId.ToString()}});
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(x => x.HttpContext.Request.Query).Returns(() => queryCollection);

            _handler = new AuthorizationHandler(_httpContextAccessor.Object,
                _applicationApiClient.Object,
                _userService.Object,
                Mock.Of<ILogger<AuthorizationHandler>>());
        }

        [Test]
        public async Task AccessApplicationRequirement_Succeeds_If_User_Has_Access_To_Application()
        {
            var handlerContext = CreateHandlerContext(new AccessApplicationRequirement());
            await _handler.HandleAsync(handlerContext);
            Assert.IsTrue(handlerContext.HasSucceeded);
        }

        [Test]
        public async Task AccessApplicationRequirement_Does_Not_Succeed_If_User_Does_Not_Have_Access_To_Application()
        {
            _applicationApiClient.Setup(x => x.GetApplicationByUserId(_applicationId, _userSignInId)).ReturnsAsync(() => null);
            var handlerContext = CreateHandlerContext(new AccessApplicationRequirement());
            await _handler.HandleAsync(handlerContext);
            Assert.IsFalse(handlerContext.HasSucceeded);
        }

        [TestCase(ApplicationStatus.InProgress, ApplicationStatus.New, false)]
        [TestCase(ApplicationStatus.InProgress, ApplicationStatus.Withdrawn, false)]
        [TestCase(ApplicationStatus.InProgress, ApplicationStatus.Rejected, false)]
        [TestCase(ApplicationStatus.New, ApplicationStatus.New, true)]
        [TestCase(ApplicationStatus.InProgress, ApplicationStatus.InProgress, true)]
        [TestCase(ApplicationStatus.Withdrawn, ApplicationStatus.Withdrawn, true)]
        [TestCase(ApplicationStatus.Rejected, ApplicationStatus.Rejected, true)]
        public async Task ApplicationStatusRequirement_Succeeds_If_Application_Is_In_A_Valid_State(string applicationStatus, string requiredStatus, bool expectSucceeds)
        {
            _application.ApplicationStatus = applicationStatus;
            var handlerContext = CreateHandlerContext(new ApplicationStatusRequirement(requiredStatus));
            await _handler.HandleAsync(handlerContext);
            Assert.AreEqual(expectSucceeds, handlerContext.HasSucceeded);
        }

        private AuthorizationHandlerContext CreateHandlerContext(IAuthorizationRequirement requirement)
        {
            var handlerContext = new AuthorizationHandlerContext(new[]{ requirement }, new ClaimsPrincipal(), "");
            return handlerContext;
        }
    }
}
