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
    public class When_answer_is_not_provided : UpdatePageAnswersHandlerTestBase
    {
        public override void Arrange()
        {
            base.Arrange();

            AnswerQ7 = null;

            Validator.Setup(v => v.Validate(It.Is<string>(p => p == QuestionIdQ7), It.Is<Answer>(p => p == AnswerQ7)))
                .Returns
                ((string questionId, Answer answer) => new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(questionId, $"{questionId} is required") });
        }

        [Test]
        public void Then_section_is_not_saved_with_those_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "5", 
                new List<Answer>()
                {
                    /* no answers are provided */
                },
                true), new CancellationToken()).Wait();

            ApplyRepository.Verify(r => r.SaveSection(It.IsAny<ApplicationSection>(), UserId), Times.Never);
        }
        
        [Test]
        public void Then_page_is_returned_with_no_next_conditions_met()
        {
            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "5",
                new List<Answer>()
                {
                    /* no answers are provided */
                },
                true), new CancellationToken()).Result;

            result.Page.Next.Should().NotContain(p => p.ConditionMet);
            result.Page.Next.Should().Contain(p => p.ReturnId == "1");

            result.ValidationErrors.Count.Should().Be(1);
            result.ValidationPassed.Should().BeFalse();
        }

        [Test]
        public void Then_validation_is_carried_out()
        {             
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "5", 
                new List<Answer>()
                {
                    /* no answers are provided */
                },
                true), new CancellationToken()).Wait();

            ValidatorFactory.Verify(v=>v.Build(It.Is<Question>(question => question.QuestionId == QuestionIdQ7)));
            Validator.Verify(v => v.Validate(QuestionIdQ7, null));
        }
    }
}