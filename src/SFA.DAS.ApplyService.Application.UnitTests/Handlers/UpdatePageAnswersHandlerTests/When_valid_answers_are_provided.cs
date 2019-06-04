using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.UpdatePageAnswersHandlerTests
{
    public class When_valid_answers_are_provided : UpdatePageAnswersHandlerTestBase
    {
        public override void Arrange()
        {
            base.Arrange();

            AnswerQ2 = new Answer() { QuestionId = "Q2", Value = "SomeAnswer" };
            AnswerQ3 = new Answer() { QuestionId = "Q3", Value = "SomeOtherAnswer" };

            Validator.Setup(v => v.Validate(It.IsAny<Question>(), It.Is<Answer>(p => p.QuestionId == AnswerQ2.QuestionId)))
                .Returns
                ((Question question, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            Validator.Setup(v => v.Validate(It.IsAny<Question>(), It.Is<Answer>(p => p.QuestionId == AnswerQ3.QuestionId)))
                .Returns
                ((Question question, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });
        }

        [Test]
        public void Then_section_is_saved_with_those_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "2", 
                new List<Answer>()
                {
                    AnswerQ2,
                    AnswerQ3
                },
                true), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section => 
                    section.QnAData.Pages.First(p => p.PageId == "2").PageOfAnswers[0].Answers[0].QuestionId == AnswerQ2.QuestionId && 
                    section.QnAData.Pages.First(p => p.PageId == "2").PageOfAnswers[0].Answers[0].Value == AnswerQ2.Value), UserId));

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section =>
                    section.QnAData.Pages.First(p => p.PageId == "2").PageOfAnswers[0].Answers[1].QuestionId == AnswerQ3.QuestionId &&
                    section.QnAData.Pages.First(p => p.PageId == "2").PageOfAnswers[0].Answers[1].Value == AnswerQ3.Value), UserId));
        }
        
        [Test]
        public void Then_page_is_returned_with_correct_next_page()
        {
            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "2", 
                new List<Answer>()
                {
                    AnswerQ2,
                    AnswerQ3
                },
                false), new CancellationToken()).Result;

            result.Page.Next.Should().ContainSingle(p => p.ConditionMet);
            result.Page.Next[0].Action.Should().Be("NextPage");
            result.Page.Next[0].ReturnId.Should().Be("3");

            result.ValidationErrors.Should().BeNull();
            result.ValidationPassed.Should().BeTrue();
        }

        [Test]
        public void Then_validation_is_carried_out()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "2", 
                new List<Answer>()
                {
                    AnswerQ2,
                    AnswerQ3
                },
                false), new CancellationToken()).Wait();

            ValidatorFactory.Verify(v=>v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ2.QuestionId)));
            Validator.Verify(v => v.Validate(It.Is<Question>(question => question.QuestionId == AnswerQ2.QuestionId), AnswerQ2));
        }
    }
}