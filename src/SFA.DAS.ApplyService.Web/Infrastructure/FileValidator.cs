using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.ApplyService.Application.Apply.Validation;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class FileValidator
    {
        public static bool FileValidationPassed(List<Answer> answers, Page page, List<ValidationErrorDetail> errorMessages, ModelStateDictionary modelState, IFormFileCollection files)
        {
            var fileValidationPassed = true;
            if (!files.Any()) return true;

            foreach (var file in files)
            {

                var typeValidation = page.Questions.First(q => q.QuestionId == file.Name).Input.Validations.FirstOrDefault(v => v.Name == "FileType");
                if (typeValidation != null)
                {
                    var allowedExtension = typeValidation.Value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries)[0];
                    var mimeType = typeValidation.Value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries)[1];

                    var fileNameParts = file.FileName.Split(".", StringSplitOptions.RemoveEmptyEntries);
                    var fileNameExtension = fileNameParts[fileNameParts.Length - 1];

                    if (fileNameExtension != allowedExtension || file.ContentType.ToLower() != mimeType)
                    {
                        modelState.AddModelError(file.Name, typeValidation.ErrorMessage);
                        errorMessages.Add(new ValidationErrorDetail(file.Name, typeValidation.ErrorMessage));
                        fileValidationPassed = false;
                    }
                    else if (!FileContentIsValidForMimeType(fileNameExtension, file.OpenReadStream()))
                    {
                        var errorMessage = "File content is incorrect for the specified file type";
                        modelState.AddModelError(file.Name, errorMessage);
                        errorMessages.Add(new ValidationErrorDetail(file.Name, errorMessage));
                        fileValidationPassed = false;
                    }
                    else
                    {
                        // Only add to answers if type validation passes.
                        answers.Add(new Answer() { QuestionId = file.Name, Value = file.FileName });
                    }
                }
                else
                {
                    // Only add to answers if type validation passes.
                    answers.Add(new Answer() { QuestionId = file.Name, Value = file.FileName });
                }
            }

            return fileValidationPassed;
        }

        // Refer to this: https://www.garykessler.net/library/file_sigs.html
        private static Dictionary<string, byte[]> _knownHeaders = new Dictionary<string, byte[]>
        {
            {"JPG",  new byte[] { 0xFF, 0xD8, 0xFF }},
            {"JPEG", new byte[] { 0xFF, 0xD8, 0xFF }},
            {"TIF",  new byte[] { 0x49, 0x49, 0x2A, 0x00 }},
            {"TIFF", new byte[] { 0x49, 0x49, 0x2A, 0x00 }},
            {"GIF",  new byte[] { 0x47, 0x49, 0x46, 0x38 }},
            {"BMP",  new byte[] { 0x42, 0x4D }},
            {"ICO",  new byte[] { 0x00, 0x00, 0x01, 0x00 }},
            {"PDF",  new byte[] { 0x25, 0x50, 0x44, 0x46 }}
        };

        public static bool FileContentIsValidForMimeType(string fileExtension, System.IO.Stream fileContents)
        {
            if (fileExtension is null || fileContents is null) return false;

            var headerForFileExtension = _knownHeaders[fileExtension.ToUpper()];

            if (headerForFileExtension != null)
            {
                var headerOfActualFile = new byte[headerForFileExtension.Length];
                fileContents.Read(headerOfActualFile, 0, headerOfActualFile.Length);
                fileContents.Position = 0;

                return headerOfActualFile.SequenceEqual(headerForFileExtension);
            }

            return true;
        }
    }
}