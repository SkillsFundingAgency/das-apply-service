using System;
using System.Net.Security;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Application.Users.CreateAccount;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests
{
    [TestFixture]
    public class WhenHandlingCreateAccountRequest
    {
        private Mock<IContactRepository> _userRepository;
        private CreateAccountHandler _handler;
        private Mock<IDfeSignInService> _dfeSignInService;
        private Mock<IEmailService> _emailService;

        [SetUp]
        public void Setup()
        {
            _userRepository = new Mock<IContactRepository>();
            _dfeSignInService = new Mock<IDfeSignInService>();
            _emailService = new Mock<IEmailService>();
            
            _handler = new CreateAccountHandler(_userRepository.Object, _dfeSignInService.Object, _emailService.Object);
        }
        
        [Test]
        public void ThenANewUserIsCreated()
        {
            _handler.Handle(new CreateAccountRequest("name@email.com", "James", "Jones"), new CancellationToken());
            
            _userRepository.Verify(r => r.CreateContact("name@email.com", "James", "Jones"));
        }

        [Test]
        public void ThenAnExistingUserIsNotCreated()
        {
            _userRepository.Setup(r => r.GetContact("name@email.com")).ReturnsAsync(new Contact());
            _handler.Handle(new CreateAccountRequest("name@email.com", "James", "Jones"), new CancellationToken());
            
            _userRepository.Verify(r => r.CreateContact("name@email.com", "James", "Jones"), Times.Never);
        }

        [Test]
        public void ThenANewUserIsInvitedToDfeSignin()
        {
            var newUserId = Guid.NewGuid();

            _userRepository.Setup(r => r.CreateContact("name@email.com", "James", "Jones"))
                .ReturnsAsync(new Contact() {Id = newUserId});
            
            _handler.Handle(new CreateAccountRequest("name@email.com", "James", "Jones"), new CancellationToken());

            _dfeSignInService.Verify(s => s.InviteUser("name@email.com", "James", "Jones", newUserId));
        }

        [Test]
        public void ThenAnExistingUserIsNotInvitedToDfeSignin()
        {
            _userRepository.Setup(r => r.GetContact("name@email.com")).ReturnsAsync(new Contact());
            _handler.Handle(new CreateAccountRequest("name@email.com", "James", "Jones"), new CancellationToken());

            _dfeSignInService.Verify(s => s.InviteUser("name@email.com", "James", "Jones", It.IsAny<Guid>()), Times.Never);
        }

        [Test]
        public void ThenAnExistingUserIsSentASignInHereEmail()
        {
            _userRepository.Setup(r => r.GetContact("name@email.com")).ReturnsAsync(new Contact());
            _handler.Handle(new CreateAccountRequest("name@email.com", "James", "Jones"), new CancellationToken());

            _emailService.Verify(s => 
                s.SendEmail("name@email.com", 1, It.IsAny<object>()));
        }
        
    }
}