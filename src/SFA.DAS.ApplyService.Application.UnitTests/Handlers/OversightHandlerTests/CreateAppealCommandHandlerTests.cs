using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.CreateAppeal;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.UploadAppealFile;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Data.FileStorage;
using SFA.DAS.ApplyService.Domain.Audit;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.OversightHandlerTests
{
    [TestFixture]
    public class CreateAppealCommandHandlerTests
    {
        private CreateAppealCommandHandler _handler;
        private Mock<IAppealRepository> _appealRepository;
        private Mock<IAppealUploadRepository> _appealUploadRepository;
        private Mock<IAuditService> _auditService;

        private CreateAppealCommand _command;
        private readonly Guid _oversightReviewId = Guid.NewGuid();
        private readonly Guid _applicationId = Guid.NewGuid();
        private OversightReview _oversightReview;
        private List<AppealUpload> _appealUploads;

        public CreateAppealCommandHandlerTests()
        {
            _oversightReview = new OversightReview
            {
                Id = _oversightReviewId,
                ApplicationId = _applicationId
            };

            _appealUploads = new List<AppealUpload>
            {
                new AppealUpload
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = _applicationId
                },
                new AppealUpload
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = _applicationId
                }
            };

            _appealRepository = new Mock<IAppealRepository>();
            _appealRepository.Setup(x => x.Add(It.IsAny<Appeal>()));

            _appealUploadRepository = new Mock<IAppealUploadRepository>();
            _appealUploadRepository.Setup(x => x.GetByApplicationId(_applicationId)).ReturnsAsync(_appealUploads);

            _auditService = new Mock<IAuditService>();
            _auditService.Setup(x => x.StartTracking(UserAction.UploadAppealFile, _command.UserId, _command.UserName));
            _auditService.Setup(x => x.AuditInsert(It.IsAny<AppealUpload>()));

            _command = new CreateAppealCommand
            {

            }

            _handler = new CreateAppealCommandHandler(_appealRepository.Object, _appealUploadRepository.Object, _auditService.Object);
        }


        [Test]
        public async Task Handle_Adds_Appeal()
        {
            await _handler.Handle(_command, CancellationToken.None);

            throw new NotImplementedException();
        }

        [Test]
        public void Handle_Audits_New_Appeal()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Handle_Updates_Staged_Files_To_The_New_Appeal()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Handle_Audits_Updates_To_Staged_Files()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Handle_Throws_If_Oversight_Review_Is_Not_Unsuccessful()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void Handle_Throws_If_Oversight_Review_Has_Already_Been_Appealed()
        {
            throw new NotImplementedException();
        }
    }
}
