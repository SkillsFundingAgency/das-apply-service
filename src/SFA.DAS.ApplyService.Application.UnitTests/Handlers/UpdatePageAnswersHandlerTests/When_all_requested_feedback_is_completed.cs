using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.UpdatePageAnswersHandlerTests
{
    public class When_all_requested_feedback_is_completed : UpdatePageAnswersHandlerTestBase
    {
        public override void Arrange()
        {
            base.Arrange();

            // the QnAData is replaced for these tests
            ApplyRepository.Setup(r => r.GetSection(ApplicationId, 1, 1, UserId)).ReturnsAsync(new ApplicationSection()
            {
                Status = ApplicationSectionStatus.Evaluated,
                QnAData = new QnAData()
                {
                    Pages = new List<Page>
                    {
                        new Page
                        {
                            PageId = "1",
                            Questions = new List<Question>
                            {
                                new Question()
                                {
                                    QuestionId = "Q1",
                                    Input = new Input {Type = "Text"}
                                }
                            },
                            PageOfAnswers = new List<PageOfAnswers>(),
                            Next = new List<Next>()
                            {
                                new Next(){Action = "NextPage", ReturnId = "2"}
                            },
                            Feedback = new List<Feedback>()
                            {
                                new Feedback { IsNew = true, IsCompleted = true }
                            }
                        },
                        new Page
                        {
                            PageId = "2",
                            Feedback = new List<Feedback>()
                            {
                                new Feedback { IsNew = true, IsCompleted = true }
                            }
                        }
                    }
                }
            });

            AnswerQ1 = new Answer() { QuestionId = "Q1", Value = "QuestionAnswer" };

            Validator.Setup(v => v.Validate(It.Is<Answer>(p => p.QuestionId == AnswerQ1.QuestionId))).Returns(new List<KeyValuePair<string, string>>());
        }

        [Test]
        public void Then_RequestedFeedbackAnswered_IsTrue()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1",
                new List<Answer>()
                {
                    AnswerQ1
                },
                true), new CancellationToken()).Wait();


            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(section => section.QnAData.RequestedFeedbackAnswered.Value), UserId));
        }
    }
}
