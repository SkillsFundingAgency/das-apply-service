using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Validations
{
    public class FileUploadRequiredValidator : ICustomValidator
    {
        private readonly CustomValidationConfiguration _config;
        private readonly Page _page;
        private readonly IFormFileCollection _files;

        public FileUploadRequiredValidator(CustomValidationConfiguration config, Page page, IFormFileCollection files)
        {
            _config = config;
            _page = page;
            _files = files;
        }

        public CustomValidationResult Validate()
        {
            if (_page?.Questions != null)
            {
                var fileUploadQuestionIds = _page.Questions.Where(p => p.Input.Type == QuestionType.FileUpload).Select(q => q.QuestionId);

                var existingAnswers = _page.PageOfAnswers?.SelectMany(poa => poa.Answers) ?? new List<Answer>();

                var fileUploadAnswers = existingAnswers.Where(ans => fileUploadQuestionIds.Contains(ans.QuestionId));
                var fileUploadFiles = _files?.Where(f => fileUploadQuestionIds.Contains(f.Name)) ?? new FormFileCollection();

                if(!fileUploadAnswers.Any() && !fileUploadFiles.Any())
                {
                    return new CustomValidationResult(fileUploadQuestionIds.First(), _config.ErrorMessage);
                }
            }

            return new CustomValidationResult(string.Empty);
        }
    }
}
