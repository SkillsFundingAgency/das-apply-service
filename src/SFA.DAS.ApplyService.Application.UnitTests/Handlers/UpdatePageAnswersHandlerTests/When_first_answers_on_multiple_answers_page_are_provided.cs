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
    public class When_first_answers_on_multiple_answers_page_are_provided : UpdatePageAnswersHandlerTestBase
    {
        public override void Arrange()
        {
            base.Arrange();

            AnswerQ4 = new Answer() { QuestionId = "Q4", Value = "SomeAnswer" };
            AnswerQ5 = new Answer() { QuestionId = "Q5", Value = "SomeOtherAnswer" };

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
        public void Then_section_is_saved_with_those_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3", 
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                true), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section => 
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[0].QuestionId == AnswerQ4.QuestionId && 
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[0].Value == AnswerQ4.Value), UserId));

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section =>
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[1].QuestionId == AnswerQ5.QuestionId &&
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[1].Value == AnswerQ5.Value), UserId));
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

            result.Page.Next.Should().ContainSingle(p => p.ConditionMet);
            result.Page.Next[0].Action.Should().Be("NextPage");
            result.Page.Next[0].ReturnId.Should().Be("4");

            result.ValidationErrors.Should().BeNull();
            result.ValidationPassed.Should().BeTrue();
        }

        [Test]
        public void Then_validation_is_carried_out()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3", 
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                true), new CancellationToken()).Wait();

            ValidatorFactory.Verify(v=>v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ4.QuestionId)));
            Validator.Verify(v => v.Validate(AnswerQ4));

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == AnswerQ5.QuestionId)));
            Validator.Verify(v => v.Validate(AnswerQ5));
        }
    }
}