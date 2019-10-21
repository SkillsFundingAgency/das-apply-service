using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure.Validations;

namespace SFA.DAS.ApplyService.Web.UnitTests.Infrastructure.Validations
{
    [TestFixture]
    public class FileSizeMaxMbValidatorTests
    {
        [TestCase((1024 * 1024) - 1)]
        [TestCase(1024 * 1024)]
        public void Validate_should_be_valid_if_less_than_or_equal_to_max_size(long fileSizeBytes)
        {
            const string questionId = "QId";

            var config = new CustomValidationConfiguration
            {
                Name = "FileSizeMaxMb",
                QuestionId = questionId,
                Value = "1",
                ErrorMessage = "The file is larger than 1 Mb"
            };

            var files = new FormFileCollection();
            files.Add(CreateFile("This file should be ignored", 5000000));
            files.Add(CreateFile("QId", fileSizeBytes));

            var validator = new FileSizeMaxMbValidator(config, files);

            var result = validator.Validate();

            result.IsValid.Should().BeTrue();
            result.QuestionId.Should().Be(questionId);
            result.ErrorMessage.Should().BeNull();
        }

        [Test]
        public void Validate_should_be_invalid_if_more_than_max_size()
        {
            const string questionId = "QId";
            const string errorMessage = "The file is larger than 1 Mb";

            var config = new CustomValidationConfiguration
            {
                Name = "FileSizeMaxMb",
                QuestionId = "QId",
                Value = "1",
                ErrorMessage = errorMessage
            };

            var files = new FormFileCollection();
            files.Add(CreateFile("This file should be ignored", 5000000));
            files.Add(CreateFile("QId", 1024 * 1024 + 1));

            var validator = new FileSizeMaxMbValidator(config, files);

            var result = validator.Validate();

            result.IsValid.Should().BeFalse();
            result.QuestionId.Should().Be(questionId);
            result.ErrorMessage.Should().Be(errorMessage);
        }

        private IFormFile CreateFile(string questionId, long sizeBytes)
        {
            var file = new Mock<IFormFile>();
            file.Setup(f => f.Name).Returns(questionId);
            file.Setup(f => f.Length).Returns(sizeBytes);
            return file.Object;
        }
    }
}
