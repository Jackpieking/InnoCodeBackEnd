using FastEndpoints;
using FluentValidation;

namespace InnoCode.Application.Features.Auth.Logout;

public sealed class LogoutRequestValidator : Validator<LogoutRequest>
{
    public LogoutRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;
    }
}
