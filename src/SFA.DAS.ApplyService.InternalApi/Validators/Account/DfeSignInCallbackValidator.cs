using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SFA.DAS.ApplyService.InternalApi.Types;

namespace SFA.DAS.ApplyService.InternalApi.Validators.Account
{
    public class DfeSignInCallbackValidator : AbstractValidator<SignInCallback>
    {
        public DfeSignInCallbackValidator(IStringLocalizer<NewContact> localizer)
        {
            RuleFor(m => m.Sub)
                .NotEmpty().WithMessage(localizer["Sub must not be empty"])
                .Must(m => Guid.TryParse(m, out _)).WithMessage("Sub must be a Guid");
            
            RuleFor(m => m.SourceId)
                .NotEmpty().WithMessage(localizer["SourceId must not be empty"])
                .Must(m => Guid.TryParse(m, out _)).WithMessage("SourceId must be a Guid");
        }
    }
}