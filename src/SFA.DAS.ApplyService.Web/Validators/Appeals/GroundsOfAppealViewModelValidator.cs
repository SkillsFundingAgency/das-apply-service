using System;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp.Appeals;

namespace SFA.DAS.ApplyService.Web.Validators.Appeals
{
    public class GroundsOfAppealViewModelValidator : AbstractValidator<GroundsOfAppealViewModel>
    {
        public const string NoHowFailedOnPolicyOrProcessesEntered = "Tell us how the DfE failed to follow its own policy or processes";
        public const string NoHowFailedOnEvidenceSubmittedEntered = "Tell us how the DfE failed to understand or recognise the evidence submitted";

        public const int MaxLength = 10000;
        public const string MaxLengthError = "Your answer must be 10,000 characters or less";

        public const string AppealFileRequired = "Upload a file";
        public const string RequestedFileToDeleteRequired = "Select a file to remove";

        public const int MaxFileSizeInBytes = 5 * 1024 * 1024;
        public const string MaxFileSizeExceeded = "The selected file must be smaller than 5MB";
        public const string FileMustBePdf = "The selected file must be a PDF";

        public GroundsOfAppealViewModelValidator()
        {
            When(x => x.RequestedFormAction == GroundsOfAppealViewModel.DELETE_APPEALFILE_FORMACTION, () =>
            {
                RuleFor(x => x.RequestedFileToDelete)
                        .NotEmpty().WithMessage(RequestedFileToDeleteRequired);
            });

            When(x => x.RequestedFormAction == GroundsOfAppealViewModel.UPLOAD_APPEALFILE_FORMACTION, () =>
            {
                RuleFor(x => x.AppealFileToUpload)
                        .NotEmpty().WithMessage(AppealFileRequired);
            });

            When(x => x.RequestedFormAction == GroundsOfAppealViewModel.SUBMIT_APPEAL_FORMACTION, () =>
            {
                When(x => x.AppealOnPolicyOrProcesses, () =>
                {
                    RuleFor(x => x.HowFailedOnPolicyOrProcesses)
                        .NotEmpty().WithMessage(NoHowFailedOnPolicyOrProcessesEntered)
                        .MaximumLength(MaxLength).WithMessage(MaxLengthError);
                });

                When(x => x.AppealOnEvidenceSubmitted, () =>
                {
                    RuleFor(x => x.HowFailedOnEvidenceSubmitted)
                        .NotEmpty().WithMessage(NoHowFailedOnEvidenceSubmittedEntered)
                        .MaximumLength(MaxLength).WithMessage(MaxLengthError);
                });
            });

            When(x => x.AppealFileToUpload != null, () =>
            {
                RuleFor(x => x.AppealFileToUpload).Custom((appealFile, context) =>
                {
                    if (!FileContentIsValidForPdfFile(appealFile))
                    {
                        context.AddFailure(FileMustBePdf);
                    }
                    else if (appealFile.Length > MaxFileSizeInBytes)
                    {
                        context.AddFailure(MaxFileSizeExceeded);
                    }
                });
            });
        }

        private static bool FileContentIsValidForPdfFile(IFormFile file)
        {
            var pdfHeader = new byte[] { 0x25, 0x50, 0x44, 0x46 };

            using (var fileContents = file.OpenReadStream())
            {
                var headerOfActualFile = new byte[pdfHeader.Length];
                fileContents.Read(headerOfActualFile, 0, headerOfActualFile.Length);
                fileContents.Position = 0;

                return headerOfActualFile.SequenceEqual(pdfHeader);
            }
        }
    }
}
