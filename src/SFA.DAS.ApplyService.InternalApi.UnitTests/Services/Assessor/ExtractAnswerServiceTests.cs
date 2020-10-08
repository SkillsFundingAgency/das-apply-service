using System.Collections.Generic;
using NUnit.Framework;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests.Services.Assessor
{
    [TestFixture]
    public class ExtractAnswerValueServiceTests
    {
        private ExtractAnswerValueService _extractService;
        private List<AssessorAnswer> _answers;
        private readonly string _questionId = "111";
        private readonly string _value = "value returned";
        private AssessorPage _assessorPage = new AssessorPage();

        [SetUp]
        public void TestSetup()
        {
            _extractService = new ExtractAnswerValueService();
            _answers = new List<AssessorAnswer>();
        }

        [Test]
        public void ExtractEmptyStringValueFromEmptyListOfAnswers()
        {
            var result = _extractService.ExtractAnswerValueFromQuestionId(_answers, _questionId);
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ExtractEmptyStringValueFromNullListOfAnswers()
        {
            var result = _extractService.ExtractAnswerValueFromQuestionId(null, _questionId);
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ExtractEmptyStringValueFromListOfAnswersWithoutPageId()
        {
            _answers = new List<AssessorAnswer> {new AssessorAnswer {QuestionId = $"{_questionId}x", Value = _value}};
            var result = _extractService.ExtractAnswerValueFromQuestionId(_answers, _questionId);
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ExtractEmptyStringValueFromListOfAnswersWithEmptyPageId()
        {
            _answers = new List<AssessorAnswer>
            {
                new AssessorAnswer {QuestionId = _questionId, Value = _value},
                new AssessorAnswer {QuestionId = $"{_questionId}x", Value = "other value"}
            };
            var result = _extractService.ExtractAnswerValueFromQuestionId(_answers, null);
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void ExtractExpectedValueFromListOfAnswersWithMatchingPageId()
        {
            _answers = new List<AssessorAnswer>
            {
                new AssessorAnswer {QuestionId = _questionId, Value = _value},
                new AssessorAnswer {QuestionId = $"{_questionId}x", Value = "other value"}
            };
            var result = _extractService.ExtractAnswerValueFromQuestionId(_answers, _questionId);
            Assert.AreEqual(_value, result);
        }

        [Test]
        public void ExtractFurtherQuestainsNoValueFromListOfAnswersWithMatchingPageIdWithoutFurtherQuestion()
        {
            _answers = new List<AssessorAnswer>
            {
                new AssessorAnswer {QuestionId = _questionId, Value = _value},
                new AssessorAnswer {QuestionId = $"{_questionId}x", Value = "other value"}
            };
            _assessorPage.Answers = _answers;
            var result = _extractService.ExtractFurtherQuestionAnswerValueFromQuestionId(_assessorPage, _questionId);
            Assert.AreEqual(string.Empty, result);
        }


        [Test]
        public void ExtractFurtherQuestainsValueFromListOfAnswersWithMatchingPageIdWithFurtherQuestion()
        {
            var furtherQuestionId = "123";
            _assessorPage.Answers = new List<AssessorAnswer>
            {
                new AssessorAnswer {QuestionId = _questionId, Value = furtherQuestionId},
                new AssessorAnswer {QuestionId = $"{_questionId}x", Value = "other value"},
                new AssessorAnswer {QuestionId = furtherQuestionId, Value = _value},
            };

            var question = new AssessorQuestion
            {
                QuestionId = _questionId,
                Options = new List<AssessorOption>
                {
                    new AssessorOption
                    {
                        Value = furtherQuestionId,
                        FurtherQuestions = new List<AssessorQuestion>
                        {
                            new AssessorQuestion {QuestionId = furtherQuestionId}
                        }
                    }
                }
            };

            _assessorPage.Questions = new List<AssessorQuestion> {question};

            var result = _extractService.ExtractFurtherQuestionAnswerValueFromQuestionId(_assessorPage, _questionId);
            Assert.AreEqual(_value, result);
        }
    }
}
