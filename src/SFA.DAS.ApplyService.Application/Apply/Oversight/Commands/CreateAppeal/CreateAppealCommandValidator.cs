using FluentValidation;
using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Models;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight.Commands.CreateAppeal
{
    public class CreateAppealCommandValidator : AbstractValidator<CreateAppealCommand>
    {
        public CreateAppealCommandValidator()
        {
            RuleFor(x => x.OversightReviewId).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}