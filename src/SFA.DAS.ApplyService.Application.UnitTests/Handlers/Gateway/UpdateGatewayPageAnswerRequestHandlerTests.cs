using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.Gateway;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.Gateway
{
    [TestFixture]
    public class UpdateGatewayPageAnswerRequestHandlerTests
    {
        private UpdateGatewayPageAnswerRequestHandler _handler;
        private Mock<IApplyRepository> _repository;
        private Mock<IAuditService> _auditService;
        private Fixture _autoFixture;
        private Guid _applicationId;
        private GatewayPageAnswer _existingGatewayPageAnswer;
        private string _pageId;
        private string _userId;
        private string _userName;
        private string _status;
        private string _comments;


        [SetUp]
        public void SetUp()
        {
            _autoFixture = new Fixture();
            _repository = new Mock<IApplyRepository>();
            _auditService = new Mock<IAuditService>();

            _applicationId = Guid.NewGuid();
            _pageId = _autoFixture.Create<string>();
            _userId = _autoFixture.Create<string>();
            _userName = _autoFixture.Create<string>();
            _status = _autoFixture.Create<string>();
            _comments = _autoFixture.Create<string>();

            _existingGatewayPageAnswer = _autoFixture.Create<GatewayPageAnswer>();

            _repository.Setup(x => x.InsertGatewayPageAnswer(It.IsAny<GatewayPageAnswer>(), _userId, _userName)).Returns(() => Task.CompletedTask);
            _repository.Setup(x => x.UpdateGatewayPageAnswer(It.IsAny<GatewayPageAnswer>(), _userId, _userName)).Returns(() => Task.CompletedTask);

            _handler = new UpdateGatewayPageAnswerRequestHandler(_repository.Object, _auditService.Object);
        }

        [Test]
        public async Task Handle_Inserts_New_GatewayPageAnswer()
        {
            await _handler.Handle(
                new UpdateGatewayPageAnswerRequest(_applicationId, _pageId, _status, _comments, _userId, _userName),
                CancellationToken.None);

            _repository.Verify(x => x.InsertGatewayPageAnswer(It.Is<GatewayPageAnswer>(a => 
                a.ApplicationId == _applicationId
                && a.PageId == _pageId
                && a.Status == _status
                && a.Comments == _comments), _userId, _userName));
        }

        [Test]
        public async Task Handle_Updates_Existing_GatewayPageAnswer()
        {
            _repository.Setup(x => x.GetGatewayPageAnswer(_applicationId, _pageId))
                .ReturnsAsync(_existingGatewayPageAnswer);

            await _handler.Handle(
                new UpdateGatewayPageAnswerRequest(_applicationId, _pageId, _status, _comments, _userId, _userName),
                CancellationToken.None);

            _repository.Verify(x => x.UpdateGatewayPageAnswer(It.Is<GatewayPageAnswer>(a => 
                a.ApplicationId == _existingGatewayPageAnswer.ApplicationId
                && a.PageId == _existingGatewayPageAnswer.PageId
                && a.Comments == _comments
                && a.Status == _status
            ), _userId, _userName));
        }

        [Test]
        public async Task Handle_Insert_Is_Subject_To_Audit()
        {
            await _handler.Handle(
                new UpdateGatewayPageAnswerRequest(_applicationId, _pageId, _status, _comments, _userId, _userName),
                CancellationToken.None);

            _auditService.Verify(x => x.StartTracking(UserAction.UpdateGatewayPageOutcome, _userId, _userName));
            _auditService.Verify(x => x.AuditInsert(It.Is<GatewayPageAnswer>(a => a.ApplicationId == _applicationId && a.PageId == _pageId)));
            _auditService.Verify(x => x.Save());
        }

        [Test]
        public async Task Handle_Update_Is_Subject_To_Audit()
        {
            _repository.Setup(x => x.GetGatewayPageAnswer(_applicationId, _pageId))
                .ReturnsAsync(_existingGatewayPageAnswer);

            await _handler.Handle(
                new UpdateGatewayPageAnswerRequest(_applicationId, _pageId, _status, _comments, _userId, _userName),
                CancellationToken.None);

            _auditService.Verify(x => x.StartTracking(UserAction.UpdateGatewayPageOutcome, _userId, _userName));
            _auditService.Verify(x => x.AuditUpdate(It.Is<GatewayPageAnswer>(a => a == _existingGatewayPageAnswer)));
            _auditService.Verify(x => x.Save());
        }
    }
}
