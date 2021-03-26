using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure.Validations;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.UnitTests.Infrastructure.Validations
{
    [TestFixture]
    public class CustomValidatorFactoryTests
    {
        [Test]
        public void GetCustomValidationsForPage_should_return_validations_for_config()
        {
            var config = new Mock<IOptions<List<CustomValidationConfiguration>>>();
            config.Setup(c => c.Value).Returns(new List<CustomValidationConfiguration>
            {
                new CustomValidationConfiguration
                {
                    PageId = "P1",
                    Name = "FileUploadRequired",
                    ErrorMessage = "Please upload a file",
                    Value = "Q1"
                },
            });

            var page1 = new Page
            {
                PageId = "P1",
                Questions = new List<Question> { new Question { QuestionId = "Q1", Input = new Input { Type = "FileUpload" } } },
                PageOfAnswers = new List<PageOfAnswers>()
            };

            var page2 = new Page
            {
                PageId = "P2",
                Questions = new List<Question> { new Question { QuestionId = "Q2", Input = new Input { Type = "FileUpload" } } },
                PageOfAnswers = new List<PageOfAnswers>()
            };

            var factory = new CustomValidatorFactory(config.Object);

            var result1 = factory.GetCustomValidationsForPage(page1, null);
            result1.Count().Should().Be(1);
            result1.ToList()[0].Should().BeOfType<FileUploadRequiredValidator>();

            var result2 = factory.GetCustomValidationsForPage(page2, null);
            result2.Count().Should().Be(0);

            var result3 = factory.GetCustomValidationsForPage(null, null);
            result3.Count().Should().Be(0);
        }

        [Test]
        public void GetCustomValidationsForQuestion_should_return_validations_for_config()
        {
            var config = new Mock<IOptions<List<CustomValidationConfiguration>>>();
            config.Setup(c => c.Value).Returns(new List<CustomValidationConfiguration>
            {
                new CustomValidationConfiguration
                {
                    PageId = "P1",
                    QuestionId = "Q1",
                    Name = "FileSizeMaxMb",
                    ErrorMessage = "File is too big",
                    Value  = "10"
                },
            });

            var factory = new CustomValidatorFactory(config.Object);

            var result1 = factory.GetCustomValidationsForQuestion("P1", "Q1", null);
            result1.Count().Should().Be(1);
            result1.ToList()[0].Should().BeOfType<FileSizeMaxMbValidator>();

            var result2 = factory.GetCustomValidationsForQuestion("P2", "Q1", null);
            result2.Count().Should().Be(0);

            var result3 = factory.GetCustomValidationsForQuestion("P1", "Q2", null);
            result3.Count().Should().Be(0);
        }
    }
}
