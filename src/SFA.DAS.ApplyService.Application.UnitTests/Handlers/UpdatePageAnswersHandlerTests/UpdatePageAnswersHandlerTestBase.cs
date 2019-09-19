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
        protected Mock<IValidator> Validator;

        protected const string QuestionIdQ1 = "Q1";
        protected const string QuestionIdQ1Dot1 = "Q1.1";
        protected const string QuestionIdQ2 = "Q2";
        protected const string QuestionIdQ3 = "Q3";
        protected const string QuestionIdQ4 = "Q4";
        protected const string QuestionIdQ5 = "Q5";
        protected const string QuestionIdQ6 = "Q6";
        protected const string QuestionIdQ7 = "Q7";

        protected Answer AnswerQ1 = new Answer { QuestionId = QuestionIdQ1 };
        protected Answer AnswerQ1Dot1 = new Answer { QuestionId = QuestionIdQ1Dot1 };
        protected Answer AnswerQ2 = new Answer { QuestionId = QuestionIdQ2 };
        protected Answer AnswerQ3 = new Answer { QuestionId = QuestionIdQ3 };
        protected Answer AnswerQ4 = new Answer { QuestionId = QuestionIdQ4 };
        protected Answer AnswerQ5 = new Answer { QuestionId = QuestionIdQ5 };
        protected Answer AnswerQ6 = new Answer { QuestionId = QuestionIdQ6 };
        protected Answer AnswerQ7 = new Answer { QuestionId = QuestionIdQ7 };

        protected QnAData QnAData;

        [SetUp]
        public virtual void Arrange()
        {
            ApplicationId = Guid.NewGuid();
            UserId = new Guid();

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
                                Input = new Input
                                {
                                    Type = "ComplexRadio",
                                    Options = new List<Option>
                                    {
                                        new Option
                                        {
                                            Label = "Yes",
                                            Value = "Yes",
                                            FurtherQuestions = new List<Question>
                                            {
                                                new Question
                                                {
                                                    QuestionId = "Q1.1",
                                                    Input = new Input
                                                    {
                                                        Type = "Text",
                                                        Validations = new List<ValidationDefinition>
                                                        {
                                                            new ValidationDefinition
                                                            {
                                                                Name = "Required",
                                                                ErrorMessage = "Enter Q1.1"
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new Option
                                        {
                                            Label = "No",
                                            Value = "No"
                                        }
                                    }
                                }
                            },
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers {
                                Answers = new List<Answer>
                                {
                                    new Answer { QuestionId = "Q1", Value = "Yes" },
                                    new Answer { QuestionId = "Q1.1", Value = "SomeAnswer" }
                                }
                            }
                        },
                        Next = new List<Next>()
                        {
                            new Next
                            {
                                Action = "NextPage",
                                ReturnId = "2",
                                Condition = new Condition()
                                {
                                    QuestionId = "Q1",
                                    MustEqual = "Yes"
                                },
                                ConditionMet = false

                            },
                            new Next
                            {
                                Action = "NextPage",
                                ReturnId = "3",
                                Condition = new Condition()
                                {
                                    QuestionId = "Q1",
                                    MustEqual = "No"
                                },
                                ConditionMet = false
                            }
                        }
                    },
                    new Page
                    {
                        PageId = "2",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "Q2",
                                Input = new Input
                                {
                                    Type = "Text"
                                }
                            },
                            new Question
                            {
                                QuestionId = "Q3",
                                Input = new Input
                                {
                                    Type = "Text"
                                }
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>(),
                        Next = new List<Next>
                        {
                            new Next
                            {
                                Action = "NextPage",
                                ReturnId = "3"
                            }
                        }
                    },
                    new Page
                    {
                        PageId = "3",
                        AllowMultipleAnswers = true,
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "Q4",
                                Input = new Input
                                {
                                    Type = "Text"
                                }
                            },
                            new Question
                            {
                                QuestionId = "Q5",
                                Input = new Input
                                {
                                    Type = "Text"
                                }
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>(),
                        Next = new List<Next>
                        {

                            new Next
                            {
                                Action = "NextPage",
                                ReturnId = "4"
                            }
                        }
                    },
                    new Page
                    {
                        PageId = "4",
                        Questions = new List<Question>
                        {
                            new Question
                            {
                                QuestionId = "Q6",
                                Input = new Input
                                {
                                    Type = "Text"
                                }
                            }
                        },
                        PageOfAnswers = new List<PageOfAnswers>(),
                        Next = new List<Next>
                        {
                            new Next
                            {
                                Action = "NextPage",
                                ReturnId = "5"
                            }
                        }
                    },
                    new Page
                    {
                        PageId = "5",
                        Questions = new List<Question>
                        {
                            new Question()
                            {
                                QuestionId = "Q7",
                                Input = new Input
                                {
                                    Type = "ComplexRadio",
                                    Options = new List<Option>
                                    {
                                        new Option
                                        {
                                            Label = "Yes",
                                            Value = "Yes",
                                        },
                                        new Option
                                        {
                                            Label = "No",
                                            Value = "No"
                                        }
                                    }
                                }
                            },
                        },
                        PageOfAnswers = new List<PageOfAnswers>(),
                        Next = new List<Next>
                        {
                            new Next
                            {
                                Action = "ReturnToSection",
                                ReturnId = "1"
                            }
                        }
                    },
                },
                FinancialApplicationGrade = null
            };
            

        ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetSection(ApplicationId, 1, 1, UserId)).ReturnsAsync(new ApplicationSection()
            {
                Status = ApplicationSectionStatus.Draft,
                QnAData = QnAData
            });
              
            ValidatorFactory = new Mock<IValidatorFactory>();
            ValidatorFactory.Setup(vf => vf.Build(It.IsAny<Question>())).Returns(new List<IValidator>());

            Validator = new Mock<IValidator>();
            ValidatorFactory.Setup(vf => vf.Build(It.IsAny<Question>()))
                .Returns(new List<IValidator> { Validator.Object });

            Handler = new UpdatePageAnswersHandler(ApplyRepository.Object, ValidatorFactory.Object);
        }
    }
}