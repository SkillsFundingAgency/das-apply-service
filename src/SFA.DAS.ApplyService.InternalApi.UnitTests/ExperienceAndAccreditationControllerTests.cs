using NUnit.Framework;
using Moq;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;

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
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync(ValueOfQuestion);
            var actualResult = _controller.GetOfficeForStudents(new Guid()).Result;

            Assert.AreEqual(ValueOfQuestion,actualResult);
            _qnaApiClient.Verify(x=>x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()),Times.Once);
        }

        [Test]
        public void get_office_for_students_returns_no_value_when_not_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync((string)null);

            var actualResult = _controller.GetOfficeForStudents(new Guid()).Result;

            Assert.AreEqual(null, actualResult);
            _qnaApiClient.Verify(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }


        [Test]
        public void get_initial_teacher_training_returns_expected_value_when_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync(ValueOfQuestion);


            var actualResult = _controller.GetInitialTeacherTraining(new Guid()).Result;

            Assert.AreEqual(ValueOfQuestion, actualResult);
            _qnaApiClient.Verify(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void get_initial_teacher_training_returns_no_value_when_not_present()
        {
            _qnaApiClient
                .Setup(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
                    It.IsAny<string>())).ReturnsAsync((string)null);

            var actualResult = _controller.GetInitialTeacherTraining(new Guid()).Result;

            Assert.AreEqual(null, actualResult);
            _qnaApiClient.Verify(x => x.GetAnswerValue(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
