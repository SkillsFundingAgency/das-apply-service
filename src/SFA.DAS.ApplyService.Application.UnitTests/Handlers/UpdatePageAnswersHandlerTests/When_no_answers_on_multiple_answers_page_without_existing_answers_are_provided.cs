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
    public class When_no_answers_on_multiple_answers_page_without_existing_answers_are_provided : UpdatePageAnswersHandlerTestBase
    {
        public override void Arrange()
        {
            base.Arrange();

            AnswerQ4 = new Answer() { QuestionId = "Q4", Value = string.Empty };
            AnswerQ5 = new Answer() { QuestionId = "Q5", Value = string.Empty };

            Validator.Setup(v => v.Validate(It.Is<Answer>(p => p.QuestionId == AnswerQ4.QuestionId)))
                .Returns
                ((Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            Validator.Setup(v => v.Validate(It.Is<Answer>(p => p.QuestionId == AnswerQ5.QuestionId)))
                .Returns
                ((Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });
        }

        [Test]
        public void Then_section_is_not_saved_with_any_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3",
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                true), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.IsAny<ApplicationSection>(), UserId), Times.Never());
        }

        [Test]
        public void Then_page_is_returned_with_correct_same_page()
        {
            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3", 
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                false), new CancellationToken()).Result;

            result.Page.Next.Should().NotContain(p => p.ConditionMet);
            result.Page.Next[0].Action.Should().Be("NextPage");
            result.Page.Next.Should().Contain(p => p.ReturnId == "4");

            result.ValidationErrors.Should().NotBeEmpty();
            result.ValidationPassed.Should().BeFalse();
        }

        [Test]
        public void Then_validation_is_carried_out_on_new_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3", 
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                false), new CancellationToken()).Wait();

            Validator.Verify(v => v.Validate(AnswerQ4), Times.Once());
            Validator.Verify(v => v.Validate(AnswerQ5), Times.Once());
        }
    }
}