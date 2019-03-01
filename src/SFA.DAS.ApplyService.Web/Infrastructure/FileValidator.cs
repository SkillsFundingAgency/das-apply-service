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
                    else
                    {
                        // Only add to answers if type validation passes.
                        answers.Add(new Answer() {QuestionId = file.Name, Value = file.FileName});
                    }
                }
                else
                {
                    // Only add to answers if type validation passes.
                    answers.Add(new Answer() {QuestionId = file.Name, Value = file.FileName});
                }
            }

            return fileValidationPassed;
        }
    }
}