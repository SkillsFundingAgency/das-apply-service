using System.Collections.Generic;
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
    public class When_valid_answer_for_conditional_question_is_provided : UpdatePageAnswersHandlerTestBase
    {
        [Test]
        public void Then_page_is_returned_with_first_next_condition_met()
        {
            AnswerQ1 = new Answer() { QuestionId = "Q1", Value = "Yes" };
            AnswerQ1Dot1 = new Answer() { QuestionId = "Q1.1", Value = "SomeNewAnswer" };

            Validator.Setup(v => v.Validate(It.IsAny<Question>(), It.Is<Answer>(p => p.QuestionId == AnswerQ1.QuestionId)))
                .Returns
                ((Question question, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            Validator.Setup(v => v.Validate(It.IsAny<Question>(), It.Is<Answer>(p => p.QuestionId == AnswerQ1Dot1.QuestionId)))
                .Returns
                ((Question question, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1",
                new List<Answer>()
                {
                    AnswerQ1,
                    AnswerQ1Dot1
                },
                true), new CancellationToken()).Result;

            result.Page.Next.Should().ContainSingle(p => p.ConditionMet);
            result.Page.Next.Should().Contain(p => p.ConditionMet && p.ReturnId == "2");
            result.Page.Next.Should().Contain(p => !p.ConditionMet && p.ReturnId == "3");

            result.ValidationErrors.Should().BeNull();
            result.ValidationPassed.Should().BeTrue();
        }

        [Test]
        public void Then_page_is_returned_with_second_next_condition_met()
        {
            AnswerQ1 = new Answer() { QuestionId = "Q1", Value = "No" };
            Validator.Setup(v => v.Validate(It.IsAny<Question>(), It.Is<Answer>(p => p.QuestionId == AnswerQ1.QuestionId))).Returns(new List<KeyValuePair<string, string>>());

            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1",
                new List<Answer>()
                {
                    AnswerQ1
                },
                true), new CancellationToken()).Result;

            result.Page.Next.Should().ContainSingle(p => p.ConditionMet);
            result.Page.Next.Should().Contain(p => !p.ConditionMet && p.ReturnId == "2");
            result.Page.Next.Should().Contain(p => p.ConditionMet && p.ReturnId == "3");
        }
    }
}