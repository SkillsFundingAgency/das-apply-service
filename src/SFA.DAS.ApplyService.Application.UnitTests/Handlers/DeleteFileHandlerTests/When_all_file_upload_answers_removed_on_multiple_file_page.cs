using System;
using System.Linq;
using System.Threading;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.DeleteFile;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.DeleteFileHandlerTests
{
    public class When_all_file_upload_answers_removed_on_multiple_file_page : DeleteFileHandlerTestBase
    {
        public override void Arrange()
        {
            base.Arrange();
        }

        [Test]
        public void Then_section_is_saved_with_answer_removed()
        {
            Handler.Handle(new DeleteFileRequest(ApplicationId, UserId, "3", 1, 1, "Q3", "MyFile.pdf"),
                new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section =>
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers.Count == 0),
                    UserId));
        }

        [Test]
        public void Then_section_is_saved_with_page_not_complete()
        {
            Handler.Handle(new DeleteFileRequest(ApplicationId, UserId, "3", 1, 1, "Q3", "MyFile.pdf"),
                new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section => 
                    section.QnAData.Pages.First(p => p.PageId == "3").Complete == false), 
                    UserId));

        }

        [Test]
        public void Then_file_is_deleted_from_storage()
        {
            Handler.Handle(new DeleteFileRequest(ApplicationId, UserId, "3", 1, 1, "Q3", "MyFile.pdf"),
                new CancellationToken()).Wait();

            StorageService.Verify(r => r.Delete(
                It.Is<Guid>(p => p == ApplicationId),
                It.Is<int>(p => p == 1),
                It.Is<int>(p => p == 1),
                It.Is<string>(p => p == "3"),
                It.Is<string>(p => p == "Q3"),
                It.Is<string>(p => p == "MyFile.pdf")));
        }
    }
}