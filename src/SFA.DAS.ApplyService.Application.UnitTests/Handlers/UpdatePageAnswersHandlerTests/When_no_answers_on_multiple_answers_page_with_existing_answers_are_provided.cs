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
    public class When_no_answers_on_multiple_answers_page_with_existing_answers_are_provided : UpdatePageAnswersHandlerTestBase
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

            AnswerQ4 = new Answer() { QuestionId = "Q4", Value = string.Empty };
            AnswerQ5 = new Answer() { QuestionId = "Q5", Value = string.Empty };

            Validator.Setup(v => v.Validate(It.Is<string>(p => p == QuestionIdQ4), It.Is<Answer>(p => p.QuestionId == AnswerQ4.QuestionId)))
                .Returns
                ((string questionId, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(questionId, $"{questionId} is required") });

            Validator.Setup(v => v.Validate(It.Is<string>(p => p == QuestionIdQ5), It.Is<Answer>(p => p.QuestionId == AnswerQ5.QuestionId)))
                .Returns
                ((string questionId, Answer answer) => !string.IsNullOrEmpty(answer.Value)
                    ? new List<KeyValuePair<string, string>>()
                    : new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(questionId, $"{questionId} is required") });
        }

        [Test]
        public void Then_section_is_not_saved_with_the_new_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3", 
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                false), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section => 
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[0].QuestionId == AnswerQ4.QuestionId && 
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[0].Value == AnswerQ4.Value), UserId), Times.Never());

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section =>
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[1].QuestionId == AnswerQ5.QuestionId &&
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[1].Value == AnswerQ5.Value), UserId), Times.Never());
        }

        [Test]
        public void Then_section_is_saved_with_the_existing_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3",
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                false), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section =>
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[0].QuestionId == _existingAnswerQ4.QuestionId &&
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[0].Value == _existingAnswerQ4.Value), UserId), Times.Once());

            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(
                section =>
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[1].QuestionId == _existingAnswerQ5.QuestionId &&
                    section.QnAData.Pages.First(p => p.PageId == "3").PageOfAnswers[0].Answers[1].Value == _existingAnswerQ5.Value), UserId), Times.Once());
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
                false), new CancellationToken()).Result;

            result.Page.Next.Should().ContainSingle(p => p.ConditionMet);
            result.Page.Next[0].Action.Should().Be("NextPage");
            result.Page.Next[0].ReturnId.Should().Be("4");

            result.ValidationErrors.Should().BeNull();
            result.ValidationPassed.Should().BeTrue();
        }

        [Test]
        public void Then_validation_is_not_carried_out_on_new_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3", 
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                false), new CancellationToken()).Wait();

            Validator.Verify(v => v.Validate(QuestionIdQ4, AnswerQ4), Times.Never());
            Validator.Verify(v => v.Validate(QuestionIdQ5, AnswerQ5), Times.Never());
        }

        [Test]
        public void Then_validation_is_carried_out_on_existing_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "3",
                new List<Answer>()
                {
                    AnswerQ4,
                    AnswerQ5
                },
                false), new CancellationToken()).Wait();

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == _existingAnswerQ4.QuestionId)));
            Validator.Verify(v => v.Validate(QuestionIdQ4, _existingAnswerQ4));

            ValidatorFactory.Verify(v => v.Build(It.Is<Question>(question => question.QuestionId == _existingAnswerQ5.QuestionId)));
            Validator.Verify(v => v.Validate(QuestionIdQ5, _existingAnswerQ5));
        }
    }
}