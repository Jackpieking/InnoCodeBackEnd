using FastEndpoints;
using FluentValidation;
using InnoCode.Domain.Entities;

namespace InnoCode.Application.Features.Auth.Login;

public sealed class LoginRequestValidator : Validator<LoginRequest>
{
    public LoginRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(request => request.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(UserEntity.MetaData.Property.Email.MaxLength)
            .MinimumLength(UserEntity.MetaData.Property.Email.MinLength);

        RuleFor(request => request.Password)
            .NotEmpty()
            .MaximumLength(UserEntity.MetaData.Property.Password.MaxLength)
            .MinimumLength(UserEntity.MetaData.Property.Password.MinLength);

        RuleFor(request => request.IsRememberMe).NotNull();
    }
}
