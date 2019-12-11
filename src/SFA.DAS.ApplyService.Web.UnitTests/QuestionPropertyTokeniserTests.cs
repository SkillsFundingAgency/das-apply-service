using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using System;

namespace SFA.DAS.ApplyService.Web.UnitTests
{
    [TestFixture]
    public class QuestionPropertyTokeniserTests
    {
        private Mock<IQnaApiClient> _qnaApiClient;
        private QuestionPropertyTokeniser _tokeniser;

        [SetUp]
        public void Before_each_test()
        {
            _qnaApiClient = new Mock<IQnaApiClient>();
            _tokeniser = new QuestionPropertyTokeniser(_qnaApiClient.Object);
        }

        [Test]
        public void Tokeniser_does_not_replace_if_question_tag_not_in_application_data()
        {
            var propertyData = "This is an {{unrecognised}} token";

            Answer nullAnswer = null;
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(nullAnswer);

            var tokenisedData = _tokeniser.GetTokenisedValue(Guid.NewGuid(), propertyData).GetAwaiter().GetResult();

            tokenisedData.Should().Be(propertyData);
        }

        [Test]
        public void Tokeniser_handles_single_token_in_source_text()
        {
            var questionTag = "Question-Tag";
            var propertyData = "This is an {{" + questionTag + "}} token";

            Answer previousAnswer = new Answer
            {
                QuestionId = "QID1",
                Value = "QuestionValue"
            };
            _qnaApiClient.Setup(x => x.GetAnswerByTag(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(previousAnswer);

            var tokenisedData = _tokeniser.GetTokenisedValue(Guid.NewGuid(), propertyData).GetAwaiter().GetResult();

            tokenisedData.Contains(previousAnswer.Value).Should().BeTrue();
        }
        
        [Test]
        public void Tokeniser_does_not_alter_source_text_if_no_tokens_present()
        {
            var propertyData = "This is no token";
                        
            var tokenisedData = _tokeniser.GetTokenisedValue(Guid.NewGuid(), propertyData).GetAwaiter().GetResult();

            tokenisedData.Should().Be(propertyData);
        }

        [Test]
        public void Tokeniser_ignores_start_of_token_with_no_end()
        {
            var propertyData = "This is no {{valid token";

            var tokenisedData = _tokeniser.GetTokenisedValue(Guid.NewGuid(), propertyData).GetAwaiter().GetResult();

            tokenisedData.Should().Be(propertyData);
        }

        [Test]
        public void Tokeniser_ignores_start_token_after_end_token()
        {
            var propertyData = "This is no}} valid {{ token";

            var tokenisedData = _tokeniser.GetTokenisedValue(Guid.NewGuid(), propertyData).GetAwaiter().GetResult();

            tokenisedData.Should().Be(propertyData);
        }

        [Test]
        public void Tokeniser_handles_nulls_correctly()
        {
            string propertyData = null;

            var tokenisedData = _tokeniser.GetTokenisedValue(Guid.NewGuid(), propertyData).GetAwaiter().GetResult();

            tokenisedData.Should().Be(string.Empty);
        }

        [Test]
        public void Tokeniser_handles_empty_strings_correctly()
        {
            string propertyData = string.Empty;

            var tokenisedData = _tokeniser.GetTokenisedValue(Guid.NewGuid(), propertyData).GetAwaiter().GetResult();

            tokenisedData.Should().Be(propertyData);
        }
    }
}
