using NUnit.Framework;
using AutoMapper;
using SFA.DAS.ApplyService.InternalApi.AutoMapper;
using SFA.DAS.ApplyService.Application.Apply;
using Moq;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Models.Ukrlp;
using SFA.DAS.ApplyService.Domain.Ukrlp;
using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.InternalApi.Types.CompaniesHouse;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
   public class ExperienceAndAccreditationControllerTests

    {
        private Mock<IInternalQnaApiClient> _qnaApiClient;
        
        private ExperienceAndAccreditationController _controller;

        private const string ValueOfQuestion = "swordfish";

        [SetUp]
        public void Before_each_test()
        {

            _qnaApiClient = new Mock<IInternalQnaApiClient>();
            _controller = new ExperienceAndAccreditationController(_qnaApiClient.Object);
        }

        [Test]
        public void get_office_for_students_returns_expected_value_when_present()
        {
            
            var page = new Page
            {
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId =
                                    RoatpYourOrganisationQuestionIdConstants.OfficeForStudents,
                                Value = ValueOfQuestion
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x =>
                    x.GetPageBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(page);

            var actualResult = _controller.GetOfficeForStudents(new Guid()).Result;

            Assert.AreEqual(ValueOfQuestion,actualResult);
            _qnaApiClient.Verify(x=>x.GetPageBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()),Times.Once);
        }

        [Test]
        public void get_office_for_students_returns_no_value_when_not_present()
        {

            var page = new Page
            {
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId =
                                    "123",
                                Value = ValueOfQuestion
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x =>
                    x.GetPageBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(page);

            var actualResult = _controller.GetOfficeForStudents(new Guid()).Result;

            Assert.AreEqual(null, actualResult);
            _qnaApiClient.Verify(x => x.GetPageBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }


        [Test]
        public void get_initial_teacher_training_returns_expected_value_when_present()
        {

            var page = new Page
            {
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId =
                                    RoatpYourOrganisationQuestionIdConstants.InitialTeacherTraining,
                                Value = ValueOfQuestion
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x =>
                    x.GetPageBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(page);

            var actualResult = _controller.GetInitialTeacherTraining(new Guid()).Result;

            Assert.AreEqual(ValueOfQuestion, actualResult);
            _qnaApiClient.Verify(x => x.GetPageBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void get_initial_teacher_training_returns_no_value_when_not_present()
        {

            var page = new Page
            {
                PageOfAnswers = new List<PageOfAnswers>
                {
                    new PageOfAnswers
                    {
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                QuestionId =
                                    "123",
                                Value = ValueOfQuestion
                            }
                        }
                    }
                }
            };

            _qnaApiClient.Setup(x =>
                    x.GetPageBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(page);

            var actualResult = _controller.GetInitialTeacherTraining(new Guid()).Result;

            Assert.AreEqual(null, actualResult);
            _qnaApiClient.Verify(x => x.GetPageBySectionNo(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }
    }
}
