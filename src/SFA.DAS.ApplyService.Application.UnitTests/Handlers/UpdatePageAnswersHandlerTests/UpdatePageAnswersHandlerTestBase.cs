using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.UpdatePageAnswers;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.UpdatePageAnswersHandlerTests
{
    [TestFixture]
    public class UpdatePageAnswersHandlerTestBase
    {
        protected Guid ApplicationId;
        protected Guid UserId;
        protected UpdatePageAnswersHandler Handler;
        protected Mock<IApplyRepository> ApplyRepository;
        protected Mock<IValidatorFactory> ValidatorFactory;

        [SetUp]
        public void SetUp()
        {
            ApplicationId = Guid.NewGuid();
            UserId = new Guid();
            
            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetSection(ApplicationId, 1, 1, UserId)).ReturnsAsync(new ApplicationSection()
            {
                QnAData = new QnAData()
                {
                    Pages = new List<Page>
                    {
                        new Page
                        {
                            PageId = "1",
                            Questions = new List<Question>
                            {
                                new Question()
                                {
                                    QuestionId = "Q1", 
                                    Input = new Input {Type = "Text"}
                                }
                            },
                            PageOfAnswers = new List<PageOfAnswers>(),
                            Next = new List<Next>()
                            {
                                new Next(){Action = "NextPage", ReturnId = "2"}
                            }
                        }
                    }
                }
            });
            
            
            ValidatorFactory = new Mock<IValidatorFactory>();
            ValidatorFactory.Setup(vf => vf.Build(It.IsAny<Question>())).Returns(new List<IValidator>());
            
            
            Handler = new UpdatePageAnswersHandler(ApplyRepository.Object, ValidatorFactory.Object);
        }
    }
}