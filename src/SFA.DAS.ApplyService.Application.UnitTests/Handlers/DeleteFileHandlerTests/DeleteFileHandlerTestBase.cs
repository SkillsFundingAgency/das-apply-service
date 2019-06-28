using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply;
using SFA.DAS.ApplyService.Application.Apply.DeleteFile;
using SFA.DAS.ApplyService.Application.Apply.GetPage;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.Handlers.DeleteFileHandlerTests
{
    [TestFixture]
    public class DeleteFileHandlerTestBase
    {
        protected Guid ApplicationId;
        protected Guid UserId;
        protected DeleteFileHandler Handler;
        protected Mock<IMediator> Mediator;
        protected Mock<IApplyRepository> ApplyRepository;
        protected Mock<IStorageService> StorageService;
        protected Mock<IValidator> Validator;
        
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
                        Complete = true,
                        Questions = new List<Question>
                        {
                            new Question()
                            {
                                QuestionId = "Q1",
                                Input = new Input
                                {
                                    Type = "FileUpload",
                                    Validations = new List<ValidationDefinition>
                                    {
                                        new ValidationDefinition
                                        {
                                            Name = "Required",
                                            ErrorMessage = "Upload a file"
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
                                    new Answer { QuestionId = "Q1", Value = "MyFile.pdf" },
                                }
                            }
                        },
                        Next = new List<Next>
                        {
                            new Next
                            {
                                Action = "NextPage",
                                ReturnId = "2"
                            }
                        }
                    },
                    new Page
                    {
                        PageId = "2",
                        Complete = true,
                        Questions = new List<Question>
                        {
                            new Question()
                            {
                                QuestionId = "Q2",
                                Input = new Input
                                {
                                    Type = "FileUpload",
                                    Validations = new List<ValidationDefinition>
                                    {
                                        new ValidationDefinition
                                        {
                                            Name = "Required",
                                            ErrorMessage = "Upload 1 to 3 files"
                                        }
                                    },
                                    FileUploadInfo = new FileUploadInfo
                                    {
                                        NumberOfUploadsRequired = 1,
                                        MaximumNumberOfUploads = 3
                                    }
                                }
                            },
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers {
                                Answers = new List<Answer>
                                {
                                    new Answer { QuestionId = "Q2", Value = "MyFile.pdf" },
                                    new Answer { QuestionId = "Q2", Value = "MyFileTwo.pdf" },
                                    new Answer { QuestionId = "Q2", Value = "MyFileThree.pdf" },
                                }
                            }
                        },
                        Next = new List<Next>
                        {
                            new Next
                            {
                                Action = "NextPage",
                                ReturnId = "2"
                            }
                        }
                    },
                    new Page
                    {
                        PageId = "3",
                        Complete = true,
                        Questions = new List<Question>
                        {
                            new Question()
                            {
                                QuestionId = "Q3",
                                Input = new Input
                                {
                                    Type = "FileUpload",
                                    Validations = new List<ValidationDefinition>
                                    {
                                        new ValidationDefinition
                                        {
                                            Name = "Required",
                                            ErrorMessage = "Upload 1 to 3 files"
                                        }
                                    },
                                    FileUploadInfo = new FileUploadInfo
                                    {
                                        NumberOfUploadsRequired = 1,
                                        MaximumNumberOfUploads = 3
                                    }
                                }
                            },
                        },
                        PageOfAnswers = new List<PageOfAnswers>
                        {
                            new PageOfAnswers {
                                Answers = new List<Answer>
                                {
                                    new Answer { QuestionId = "Q3", Value = "MyFile.pdf" },
                                }
                            }
                        },
                        Next = new List<Next>
                        {
                            new Next
                            {
                                Action = "ReturnToSection",
                                ReturnId = "1"
                            }
                        }
                    }
                },
                FinancialApplicationGrade = null
            };
            
            ApplyRepository = new Mock<IApplyRepository>();
            ApplyRepository.Setup(r => r.GetSection(ApplicationId, 1, 1, UserId)).ReturnsAsync(new ApplicationSection()
            {
                Status = ApplicationSectionStatus.Draft,
                QnAData = QnAData
            });

            Mediator = new Mock<IMediator>();
            Mediator.Setup(r => r.Send(
                    It.Is<GetPageRequest>(p =>
                        p.ApplicationId == ApplicationId &&
                        p.SequenceId == 1 &&
                        p.SectionId == 1 &&
                        p.UserId == UserId),
                    It.IsAny<CancellationToken>())).ReturnsAsync((GetPageRequest p, CancellationToken c) => QnAData.Pages.FirstOrDefault(q => q.PageId == p.PageId));

            StorageService = new Mock<IStorageService>();
            Handler = new DeleteFileHandler(Mediator.Object, ApplyRepository.Object, StorageService.Object);
        }
    }
}