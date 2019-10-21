using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.Web.Configuration;
using System.Linq;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Validations
{
    public class FileSizeMaxMbValidator : ICustomValidator
    {
        private const long BytesInMb = 1048576;

        private readonly CustomValidationConfiguration _config;
        private readonly IFormFileCollection _files;

        public FileSizeMaxMbValidator(CustomValidationConfiguration config, IFormFileCollection files)
        {
            _config = config;
            _files = files;
        }

        public CustomValidationResult Validate()
        {
            var questionFiles = _files.Where(f => f.Name == _config.QuestionId).ToList();

            foreach (var file in questionFiles)
            {
                var maxSizeBytes = long.Parse(_config.Value) * BytesInMb;

                if (file.Length > maxSizeBytes)
                {
                    return new CustomValidationResult(_config.QuestionId, _config.ErrorMessage);
                }
            }

            return new CustomValidationResult(_config.QuestionId);
        }
    }
}
