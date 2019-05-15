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
    public class When_answer_with_further_answer_is_provided : UpdatePageAnswersHandlerTestBase
    {
        public override void Arrange()
        {
            base.Arrange();

            AnswerQ1 = new Answer() { QuestionId = "Q1", Value = "Yes" };
            AnswerQ1Dot1 = new Answer() { QuestionId = "Q1.1", Value = "SomeAnswer" };

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

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section =>
                    section.QnAData.Pages.First(p => p.PageId == "1").PageOfAnswers[0].Answers[0].QuestionId == AnswerQ1.QuestionId &&
                    section.QnAData.Pages.First(p => p.PageId == "1").PageOfAnswers[0].Answers[0].Value == AnswerQ1.Value), UserId));

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section =>
                    section.QnAData.Pages.First(p => p.PageId == "1").PageOfAnswers[0].Answers[1].QuestionId == AnswerQ1Dot1.QuestionId &&
                    section.QnAData.Pages.First(p => p.PageId == "1").PageOfAnswers[0].Answers[1].Value == AnswerQ1Dot1.Value), UserId));
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

            result.Page.Next.Should().ContainSingle(p => p.ConditionMet);
            result.Page.Next.Should().Contain(p => p.ConditionMet && p.ReturnId == "2");
            result.Page.Next.Should().Contain(p => !p.ConditionMet && p.ReturnId == "3");

            result.ValidationErrors.Should().BeNull();
            result.ValidationPassed.Should().BeTrue();
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
            Validator.Verify(v => v.Validate(It.Is<Question>(question => question.QuestionId == "Q1"), AnswerQ1));

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == "Q1.1")));
            Validator.Verify(v => v.Validate(It.Is<Question>(question => question.QuestionId == "Q1.1"), AnswerQ1Dot1));
        }
    }
}