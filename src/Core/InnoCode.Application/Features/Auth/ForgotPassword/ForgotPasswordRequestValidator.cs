using FastEndpoints;
using FluentValidation;
using InnoCode.Domain.Entities;

namespace InnoCode.Application.Features.Auth.ForgotPassword;

public sealed class ForgotPasswordRequestValidator : Validator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(expression: request => request.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(maximumLength: UserEntity.MetaData.Property.Email.MaxLength)
            .MinimumLength(minimumLength: UserEntity.MetaData.Property.Email.MinLength);
    }
}
