using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Web.Infrastructure;
using System.Collections.Generic;
using System.IO;

namespace SFA.DAS.ApplyService.Web.UnitTests.FileUploadValidatorTests
{
    [TestFixture]
    public class FileUpload_FileContent_Validation_Tests
    {
        [TestCase("pdf,application/pdf", "This file has correct content.pdf", new byte[] { 0x25, 0x50, 0x44, 0x46 }, true)]
        [TestCase("pdf,application/pdf", "This file has bad or corrupted content.pdf", new byte[] { 0x00, 0x50, 0x44, 0x00 }, false)]
        [TestCase("pdf,application/pdf", "This file has content for a different file type.pdf", new byte[] { 0x89, 0x50, 0x4E, 0x47 }, false)]
        public void Then_validation_should_return_correct_result(string validationValue, string filename, byte[] content, bool validationResult)
        {
            var page = new Page()
            {
                Questions = new List<Question>
                {
                    new Question()
                    {
                        QuestionId = "Q1",
                        Input = new Input()
                        {
                            Type = "FileUpload",
                            Validations = new List<ValidationDefinition>()
                            {
                                new ValidationDefinition() {Name = "FileType", Value = validationValue, ErrorMessage = "File must be a PDF"}
                            }
                        }
                    }
                }
            };
            var files = new FormFileCollection();

            var ms = new MemoryStream(content);
            ms.Read(content, 0, content.Length);

            files.Add(new FormFile(ms, 0, ms.Length, "Q1", filename)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf",
            });
            var result = FileValidator.FileValidationPassed(new List<Answer>(), page, new List<ValidationErrorDetail>(), new ModelStateDictionary(), files);

            result.Should().Be(validationResult);
        }
    }
}
