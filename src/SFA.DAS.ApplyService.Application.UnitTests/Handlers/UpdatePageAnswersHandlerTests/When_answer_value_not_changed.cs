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
        public void Then_validation_fails()
        {
            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1", 
                new List<Answer>()
                {
                    new Answer() {QuestionId = "Q1", Value = ""}
                }), new CancellationToken()).Result;

            result.ValidationPassed.Should().BeFalse();
            result.ValidationErrors.Select(v => v.Key).Distinct().Should().ContainSingle("Q1");
        }

        [Test]
        public void Then_validation_is_carried_out()
        {
            var validator = new Mock<IValidator>();
            validator.Setup(v => v.Validate(It.IsAny<Question>(), It.IsAny<Answer>())).Returns(new List<KeyValuePair<string, string>>());

            ValidatorFactory.Setup(vf => vf.Build(It.IsAny<Question>()))
                .Returns(new List<IValidator> {validator.Object});

            var answer = new Answer() {QuestionId = "Q1", Value = ""};
            
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1", 
                new List<Answer>()
                {
                    answer
                }), new CancellationToken()).Wait();

            ValidatorFactory.Verify(v=>v.Build(It.Is<Question>(question => question.QuestionId == "Q1")));
            validator.Verify(v => v.Validate(It.Is<Question>(question => question.QuestionId == "Q1"), answer));
        }
    }
}