using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure.Validations;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Web.UnitTests.Infrastructure.Validations
{
    [TestFixture]
    public class FileUploadRequiredValidatorTests
    {
        private const string pageId = "page1";
        private const string questionId = "Q1";

        private readonly CustomValidationConfiguration config = new CustomValidationConfiguration
        {
            Name = "FileUploadRequired",
            PageId = pageId,
            ErrorMessage = "Please upload file"
        };

        private readonly Page page = new Page
        {
            PageId = pageId,
            Questions = new List<Question> { new Question { QuestionId = questionId, Input = new Input { Type = "FileUpload" } } },
            PageOfAnswers = new List<PageOfAnswers>()
        };

        [Test]
        public void Validate_should_be_valid_if_a_file_is_being_uploaded()
        {
            var files = new FormFileCollection();
            files.Add(CreateFile(questionId, 1024));

            var validator = new FileUploadRequiredValidator(config, page, files);

            var result = validator.Validate();

            result.IsValid.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
        }

        [Test]
        public void Validate_should_be_valid_if_a_fileupload_already_Exists()
        {
            page.PageOfAnswers = new List<PageOfAnswers> { new PageOfAnswers { Answers = new List<Answer> { new Answer { QuestionId = questionId } } } };

            var validator = new FileUploadRequiredValidator(config, page, null);

            var result = validator.Validate();

            result.IsValid.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
        }

        [Test]
        public void Validate_should_be_invalid_if_no_file_uploaded()
        {
            page.PageOfAnswers = new List<PageOfAnswers>();

            var validator = new FileUploadRequiredValidator(config, page, null);

            var result = validator.Validate();

            result.IsValid.Should().BeFalse();
            result.QuestionId.Should().Be(questionId);
            result.ErrorMessage.Should().Be(config.ErrorMessage);
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
