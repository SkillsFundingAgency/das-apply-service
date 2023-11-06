using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Domain.Entities;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Domain.Interfaces;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.CreateAccountHandlerTests
{
    [TestFixture]
    public class WhenHandlingCreateAccountRequest
    {
        private Mock<IContactRepository> _contactRepository;
        private CreateAccountHandler _handler;
        private Mock<IDfeSignInService> _dfeSignInService;

        private const string EMAIL = "name@email.com";
        private const string GIVEN_NAME = "James";
        private const string FAMILY_NAME = "Jones";
        private const string GovUkIdentifier = "test";
        private readonly Guid ContactId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _contactRepository = new Mock<IContactRepository>();
            _contactRepository.Setup(r => r.CreateContact(EMAIL, GIVEN_NAME, FAMILY_NAME, GovUkIdentifier)).ReturnsAsync(new Contact { Id = ContactId, GivenNames = GIVEN_NAME, FamilyName = FAMILY_NAME, Email = EMAIL });

            _dfeSignInService = new Mock<IDfeSignInService>();
            _dfeSignInService.Setup(dfe => dfe.InviteUser(EMAIL, GIVEN_NAME, FAMILY_NAME, ContactId)).ReturnsAsync(new InviteUserResponse { IsSuccess = true });

            _handler = new CreateAccountHandler(_contactRepository.Object, _dfeSignInService.Object, new Mock<ILogger<CreateAccountHandler>>().Object);
        }
        
        [Test]
        public async Task ThenANewUserIsCreated()
        {
            await _handler.Handle(new CreateAccountRequest(EMAIL, GIVEN_NAME, FAMILY_NAME,GovUkIdentifier), new CancellationToken());

            _contactRepository.Verify(r => r.CreateContact(EMAIL, GIVEN_NAME, FAMILY_NAME,GovUkIdentifier));
        }

        [Test]
        public async Task ThenANewUserIsInvitedToDfeSignin()
        {
            await _handler.Handle(new CreateAccountRequest(EMAIL, GIVEN_NAME, FAMILY_NAME, GovUkIdentifier), new CancellationToken());

            _dfeSignInService.Verify(dfe => dfe.InviteUser(EMAIL, GIVEN_NAME, FAMILY_NAME, ContactId));
        }

        [Test]
        public async Task ThenAnExistingUserIsNotCreated()
        {
            _contactRepository.Setup(r => r.GetContactByEmail(EMAIL)).ReturnsAsync(new Contact { Id = ContactId, GivenNames = GIVEN_NAME, FamilyName = FAMILY_NAME, Email = EMAIL });

            await _handler.Handle(new CreateAccountRequest(EMAIL, GIVEN_NAME, FAMILY_NAME, GovUkIdentifier), new CancellationToken());
            
            _contactRepository.Verify(r => r.CreateContact(EMAIL, GIVEN_NAME, FAMILY_NAME,GovUkIdentifier), Times.Never);
        }

        [Test]
        public async Task ThenAnExistingUserIsInvitedToDfeSignin()
        {
            _contactRepository.Setup(r => r.GetContactByEmail(EMAIL)).ReturnsAsync(new Contact { Id = ContactId, GivenNames = GIVEN_NAME, FamilyName = FAMILY_NAME, Email = EMAIL });

            await _handler.Handle(new CreateAccountRequest(EMAIL, GIVEN_NAME, FAMILY_NAME,GovUkIdentifier), new CancellationToken());

            _dfeSignInService.Verify(dfe => dfe.InviteUser(EMAIL, GIVEN_NAME, FAMILY_NAME, It.IsAny<Guid>()));
        }

        [Test]
        public async Task And_AnyErrorReturnedFromDfeService_ReturnsFalse()
        {
            _dfeSignInService.Setup(dfe => dfe.InviteUser(EMAIL, GIVEN_NAME, FAMILY_NAME, ContactId)).ReturnsAsync(new InviteUserResponse { IsSuccess = false });
            
            var result = await _handler.Handle(new CreateAccountRequest(EMAIL, GIVEN_NAME, FAMILY_NAME,GovUkIdentifier), new CancellationToken());

            result.Should().Be(false);
        }             
    }
}