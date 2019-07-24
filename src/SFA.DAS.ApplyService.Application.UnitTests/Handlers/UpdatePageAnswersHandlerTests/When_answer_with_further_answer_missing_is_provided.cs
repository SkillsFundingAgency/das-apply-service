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
    public class When_answer_with_further_answer_missing_is_provided : UpdatePageAnswersHandlerTestBase
    {
        public override void Arrange()
        {
            base.Arrange();

            AnswerQ1.Value = "Yes";
            AnswerQ1Dot1.Value = string.Empty;

            Validator.Setup(v => v.Validate(It.Is<string>(p => p == QuestionIdQ1), It.Is<Answer>(p => p.QuestionId == AnswerQ1.QuestionId)))
                .Returns
                ((string questionId, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(questionId, $"{questionId} is required") });

            Validator.Setup(v => v.Validate(It.Is<string>(p => p == QuestionIdQ1Dot1), It.Is<Answer>(p => p.QuestionId == AnswerQ1Dot1.QuestionId)))
                .Returns
                ((string questionId, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(questionId, $"{questionId} is required") });
        }

        [Test]
        public void Then_section_is_not_saved_with_those_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1", 
                new List<Answer>()
                {
                    AnswerQ1,
                    AnswerQ1Dot1
                },
                true), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.IsAny<ApplicationSection>(), UserId), Times.Never);
        }
        
        [Test]
        public void Then_page_is_returned_with_no_next_conditions_met()
        {
            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1",
                new List<Answer>()
                {
                    AnswerQ1,
                    AnswerQ1Dot1
                },
                true), new CancellationToken()).Result;

            result.Page.Next.Should().NotContain(p => p.ConditionMet);
            result.Page.Next.Should().Contain(p => p.ReturnId == "2");
            result.Page.Next.Should().Contain(p => p.ReturnId == "3");

            result.ValidationErrors.Should().NotBeEmpty();
            result.ValidationPassed.Should().BeFalse();
        }

        [Test]
        public void Then_validation_is_carried_out()
        {             
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1", 
                new List<Answer>()
                {
                    AnswerQ1,
                    AnswerQ1Dot1
                },
                false), new CancellationToken()).Wait();

            ValidatorFactory.Verify(v=>v.Build(It.Is<Question>(question => question.QuestionId == "Q1")));
            Validator.Verify(v => v.Validate(QuestionIdQ1, AnswerQ1));

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == "Q1.1")));
            Validator.Verify(v => v.Validate(QuestionIdQ1Dot1, AnswerQ1Dot1));
        }
    }
}