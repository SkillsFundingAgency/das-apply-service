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
    public class When_partial_answers_on_multiple_answers_page_with_existing_answers_are_provided : UpdatePageAnswersHandlerTestBase
    {
        private Answer _existingAnswerQ4;
        private Answer _existingAnswerQ5;

        public override void Arrange()
        {
            base.Arrange();

            _existingAnswerQ4 = new Answer() { QuestionId = "Q4", Value = "SomePreviousAnswer" };
            _existingAnswerQ5 = new Answer() { QuestionId = "Q5", Value = "SomeOtherPreviousAnswer" };

            QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers.Add(new PageOfAnswers
            {
                Answers = new List<Answer>
                {
                    _existingAnswerQ4,
                    _existingAnswerQ5
                }
            });

            AnswerQ4 = new Answer() { QuestionId = "Q4", Value = "SomeNewAnswer" };
            AnswerQ5 = new Answer() { QuestionId = "Q5", Value = string.Empty };

            Validator.Setup(v => v.Validate(It.IsAny<Question>(), It.Is<Answer>(p => p.QuestionId == AnswerQ4.QuestionId)))
                .Returns
                ((Question question, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            Validator.Setup(v => v.Validate(It.IsAny<Question>(), It.Is<Answer>(p => p.QuestionId == AnswerQ5.QuestionId)))
                .Returns
                ((Question question, Answer answer) => !string.IsNullOrEmpty(answer.Value)
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
        public void Then_page_is_returned_with_correct_next_page()
        {
            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3", 
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                true), new CancellationToken()).Result;

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
                true), new CancellationToken()).Wait();

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ4.QuestionId)));
            Validator.Verify(v => v.Validate(It.Is<Question>(question => question.QuestionId == AnswerQ4.QuestionId), AnswerQ4));

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ5.QuestionId)));
            Validator.Verify(v => v.Validate(It.Is<Question>(question => question.QuestionId == AnswerQ5.QuestionId), AnswerQ5));
        }

        [Test]
        public void Then_validation_is_not_carried_out_on_existing_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3",
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                true), new CancellationToken()).Wait();

            Validator.Verify(v => v.Validate(It.Is<Question>(question => question.QuestionId == _existingAnswerQ4.QuestionId), _existingAnswerQ4), Times.Never());
            Validator.Verify(v => v.Validate(It.Is<Question>(question => question.QuestionId == _existingAnswerQ5.QuestionId), _existingAnswerQ5), Times.Never());
        }
    }
}