using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands.CancelAppeal;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;


namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Appeals.CancelAppealCommandHandlerTests
{
    [TestFixture]
    public class CancelAppealCommandHandlerTests
    {
        private CancelAppealCommandHandler _handler;
        private Mock<IAppealRepository> _appealRepository;
        private Mock<IAppealFileRepository> _appealFileRepository;
        private Mock<IAuditService> _auditService;

        private CancelAppealCommand _command;
        private readonly Guid _applicationId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _command = new CancelAppealCommand
            {
                ApplicationId = _applicationId,
                UserId = "userId",
                UserName = "userName"
            };

            var appeal = new Appeal { Id = Guid.NewGuid(), ApplicationId = _applicationId };

            _appealRepository = new Mock<IAppealRepository>();
            _appealRepository.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(appeal);
            _appealRepository.Setup(x => x.Remove(It.IsAny<Guid>()));

            var appealFiles = new List<AppealFile> { new AppealFile { Id = Guid.NewGuid(), ApplicationId = _applicationId } };

            _appealFileRepository = new Mock<IAppealFileRepository>();
            _appealFileRepository.Setup(x => x.GetAllForApplication(_applicationId)).ReturnsAsync(appealFiles);
            _appealFileRepository.Setup(x => x.Remove(It.IsAny<Guid>()));

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.CancelAppeal, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditDelete(It.IsAny<AppealFile>()));
            _auditService.Setup(x => x.AuditDelete(It.IsAny<Appeal>()));

            _handler = new CancelAppealCommandHandler(_appealRepository.Object, _appealFileRepository.Object, _auditService.Object);
        }

        [Test]
        public async Task Handle_Records_Removal_of_Appeal_Files()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealFileRepository.Verify(x => x.Remove(It.IsAny<Guid>()), Times.AtLeastOnce);

            _auditService.Verify(x => x.Save());
        }

        [Test]
        public async Task Handle_Audits_Removed_Appeal_Files()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x => x.StartTracking(UserAction.CancelAppeal, _command.UserId, _command.UserName), Times.Once);

            _auditService.Verify(x => x.AuditDelete(It.IsAny<AppealFile>()), Times.AtLeastOnce);

            _auditService.Verify(x => x.Save());
        }

        [Test]
        public async Task Handle_Records_Removal_of_Appeal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _appealRepository.Verify(x => x.Remove(It.IsAny<Guid>()), Times.Once);

            _auditService.Verify(x => x.Save());
        }

        [Test]
        public async Task Handle_Audits_Removed_Appeal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            _auditService.Verify(x => x.StartTracking(UserAction.CancelAppeal, _command.UserId, _command.UserName), Times.Once);

            _auditService.Verify(x => x.AuditDelete(It.IsAny<Appeal>()), Times.Once);

            _auditService.Verify(x => x.Save());
        }
    }
}
