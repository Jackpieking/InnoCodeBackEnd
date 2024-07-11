using System;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Application.Share.Features;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

namespace InnoCode.Application.Features.Auth.ForgotPassword;

internal sealed class ForgotPasswordHandler
    : IFeatureHandler<ForgotPasswordRequest, ForgotPasswordResponse>
{
    private readonly Lazy<IForgotPasswordRepository> _forgotPasswordRepository;

    public ForgotPasswordHandler(Lazy<IMainUnitOfWork> unitOfWork)
    {
        _forgotPasswordRepository = unitOfWork.Value.ForgotPasswordRepository;
    }

    public async Task<ForgotPasswordResponse> ExecuteAsync(
        ForgotPasswordRequest command,
        CancellationToken ct
    )
    {
        // Does user exist by email.
        var isUserFound =
            await _forgotPasswordRepository.Value.IsUserFoundByNormalizedEmailQueryAsync(
                command.Email,
                ct
            );

        // User with email is not found.
        if (!isUserFound)
        {
            return new() { StatusCode = ForgotPasswordResponseStatusCode.USER_IS_NOT_FOUND };
        }

        // Get user with user id only.
        var user = await _forgotPasswordRepository.Value.FindUserByEmailAsync(command.Email);

        // Generate password changing token.
        var passwordChangingToken = await GenerateUserPasswordChangingTokenAsync(user);

        // Add token to the database.
        var dbResult =
            await _forgotPasswordRepository.Value.AddUserPasswordChangingTokenCommandAsync(
                passwordChangingToken,
                ct
            );

        // Cannot add token to the database.
        if (!dbResult)
        {
            return new() { StatusCode = ForgotPasswordResponseStatusCode.OPERATION_FAIL };
        }

        // // Send email.
        // await SendingUserPasswordChangingMailAsync(passwordChangingToken.Value, command, ct);

        return new() { StatusCode = ForgotPasswordResponseStatusCode.OPERATION_SUCCESS };
    }

    private async Task<UserTokenEntity> GenerateUserPasswordChangingTokenAsync(UserEntity user)
    {
        var tokenId = Guid.NewGuid();

        return new()
        {
            UserId = user.Id,
            Name = "PasswordChanghingToken",
            Value = await _forgotPasswordRepository.Value.GeneratePasswordResetTokenAsync(user),
            ExpiredAt = DateTime.UtcNow.AddMinutes(1),
            LoginProvider = tokenId.ToString()
        };
    }

    /// <summary>
    ///     Sending user confirmation mail.
    /// </summary>
    /// <param name="command">
    ///     Request model.
    /// </param>
    /// <param name="passwordChangingTokenValue">
    ///     Password changing token value.
    /// </param>
    /// <param name="ct">
    ///     The token to monitor cancellation requests.
    /// </param>
    /// <returns>
    ///     Nothing
    /// </returns>
    // private async Task SendingUserPasswordChangingMailAsync(
    //     ForgotPasswordRequest command,
    //     string passwordChangingTokenValue,
    //     CancellationToken ct
    // )
    // {
    //     //Try to send mail.
    //     var sendingEmailCommand = new BackgroundJob.SendingUserPasswordChangingEmailCommand
    //     {
    //         MainTokenValue = passwordChangingTokenValue,
    //         Email = command.Email
    //     };

    //     await _queueHandler.Value.QueueAsync(
    //         sendingEmailCommand,
    //         default,
    //         DateTime.UtcNow.AddSeconds(60),
    //         ct
    //     );
    // }
}
