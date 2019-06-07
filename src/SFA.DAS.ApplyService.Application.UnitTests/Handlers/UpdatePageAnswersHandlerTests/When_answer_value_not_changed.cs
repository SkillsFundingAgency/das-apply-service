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
    public class When_answer_value_not_changed : UpdatePageAnswersHandlerTestBase
    {        
        [Test]
        public void Then_validation_succeeds_if_application_is_still_in_draft()
        {
            AnswerQ1 = new Answer() { QuestionId = "Q1", Value = "Yes" };
            AnswerQ1Dot1 = new Answer() { QuestionId = "Q1.1", Value = "SomeAnswer" };

            ApplyRepository.Setup(r => r.GetSection(ApplicationId, 1, 1, UserId)).ReturnsAsync(new ApplicationSection()
            {
                Status = ApplicationSectionStatus.Draft,
                QnAData = QnAData
            });

            Validator.Setup(v => v.Validate(It.Is<Answer>(p => p.QuestionId == AnswerQ1.QuestionId)))
                .Returns
                ((Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            Validator.Setup(v => v.Validate(It.Is<Answer>(p => p.QuestionId == AnswerQ1Dot1.QuestionId)))
                .Returns
                ((Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1", 
                new List<Answer>()
                {
                    AnswerQ1,
                    AnswerQ1Dot1
                }, true), new CancellationToken()).Result;

            result.ValidationPassed.Should().BeTrue();
            result.ValidationErrors.Should().BeNull();

            // Make sure it has verified the entirety of Question 1 
            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ1.QuestionId)));
            Validator.Verify(v => v.Validate(AnswerQ1));

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ1Dot1.QuestionId)));
            Validator.Verify(v => v.Validate(AnswerQ1Dot1));
        }

        [Test]
        public void Then_validation_fail_if_application_is_not_still_in_draft()
        {
            AnswerQ1 = new Answer() { QuestionId = "Q1", Value = "Yes" };
            AnswerQ1Dot1 = new Answer() { QuestionId = "Q1.1", Value = "SomeAnswer" };

            ApplyRepository.Setup(r => r.GetSection(ApplicationId, 1, 1, UserId)).ReturnsAsync(new ApplicationSection()
            {
                Status = ApplicationSectionStatus.Evaluated,
                QnAData = QnAData
            });

            Validator.Setup(v => v.Validate(It.Is<Answer>(p => p.QuestionId == AnswerQ1.QuestionId)))
                .Returns
                ((Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            Validator.Setup(v => v.Validate(It.Is<Answer>(p => p.QuestionId == AnswerQ1Dot1.QuestionId)))
                .Returns
                ((Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(answer.QuestionId, $"{answer.QuestionId} is required") });

            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1",
                new List<Answer>()
                {
                    AnswerQ1,
                    AnswerQ1Dot1
                }, true), new CancellationToken()).Result;

            result.ValidationPassed.Should().BeFalse();
            result.ValidationErrors.Select(v => v.Key).Distinct().Should().ContainSingle(AnswerQ1.QuestionId);

            // Make sure it has verified the entirety of Question 1 
            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ1.QuestionId)));
            Validator.Verify(v => v.Validate(AnswerQ1));

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ1Dot1.QuestionId)));
            Validator.Verify(v => v.Validate(AnswerQ1Dot1));
        }
    }
}