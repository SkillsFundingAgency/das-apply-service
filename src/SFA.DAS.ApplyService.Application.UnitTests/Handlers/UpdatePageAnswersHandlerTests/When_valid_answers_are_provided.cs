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
    public class When_valid_answers_are_provided : UpdatePageAnswersHandlerTestBase
    {
        [Test]
        public void Then_section_is_saved_with_those_answers()
        {
            Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1", 
                new List<Answer>()
                {
                    new Answer() {QuestionId = "Q1", Value = "QuestionAnswer"}
                }), new CancellationToken()).Wait();


            ApplyRepository.Verify(r => r.SaveSection(It.Is<ApplicationSection>(section => section.QnAData.Pages[0].PageOfAnswers[0].Answers[0].QuestionId == "Q1"
                                                                                           && section.QnAData.Pages[0].PageOfAnswers[0].Answers[0].Value == "QuestionAnswer"), UserId));
        }
        
        [Test]
        public void Then_page_is_returned_with_correct_next_page()
        {
            var result = Handler.Handle(new UpdatePageAnswersRequest(ApplicationId, UserId, 1, 1, "1", 
                new List<Answer>()
                {
                    new Answer() {QuestionId = "Q1", Value = "QuestionAnswer"}
                }), new CancellationToken()).Result;

            result.Page.Next[0].ConditionMet.Should().BeTrue();
            result.Page.Next[0].Action.Should().Be("NextPage");
            result.Page.Next[0].ReturnId.Should().Be("2");
        }

        [Test]
        public void Then_validation_is_carried_out()
        {
            var validator = new Mock<IValidator>();
            validator.Setup(v => v.Validate(It.IsAny<Question>(), It.IsAny<Answer>())).Returns(new List<KeyValuePair<string, string>>());

            ValidatorFactory.Setup(vf => vf.Build(It.IsAny<Question>()))
                .Returns(new List<IValidator> {validator.Object});

            var answer = new Answer() {QuestionId = "Q1", Value = "QuestionAnswer"};
            
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