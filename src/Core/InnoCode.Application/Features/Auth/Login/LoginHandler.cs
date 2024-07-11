using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Application.Features.Auth.Login;
using InnoCode.Application.Share.Features;
using InnoCode.Application.Share.Tokens;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

namespace ODour.Application.Feature.Auth.Login;

internal sealed class LoginHandler : IFeatureHandler<LoginRequest, LoginResponse>
{
    private readonly Lazy<ILoginRepository> _loginRepository;
    private readonly Lazy<IRefreshTokenHandler> _refreshTokenHandler;
    private readonly Lazy<IAccessTokenHandler> _accessTokenHandler;

    public LoginHandler(
        Lazy<IMainUnitOfWork> unitOfWork,
        Lazy<IRefreshTokenHandler> refreshTokenHandler,
        Lazy<IAccessTokenHandler> accessTokenHandler
    )
    {
        _loginRepository = unitOfWork.Value.LoginRepository;
        _refreshTokenHandler = refreshTokenHandler;
        _accessTokenHandler = accessTokenHandler;
    }

    public async Task<LoginResponse> ExecuteAsync(LoginRequest command, CancellationToken ct)
    {
        // Find user by email.
        var foundUser = await _loginRepository.Value.FindUserByEmailAsync(command.Email, ct);

        // User with email does not exist.
        if (Equals(foundUser, default))
        {
            return new() { StatusCode = LoginResponseStatusCode.USER_IS_NOT_FOUND };
        }

        // Does password belong to user.
        var signInResult = await _loginRepository.Value.CheckPasswordSignInAsync(
            foundUser,
            command.Password,
            true,
            ct
        );

        // Number of login attempts is exceeded.
        if (!signInResult.Succeeded && signInResult.IsLockedOut)
        {
            return new() { StatusCode = LoginResponseStatusCode.USER_IS_TEMPORARILY_LOCKED_OUT };
        }
        // User password is uncorrect still can try to login again.
        else if (!signInResult.Succeeded)
        {
            return new() { StatusCode = LoginResponseStatusCode.PASSWORD_INCORRECT };
        }

        var userRole = "USER";

        // Get found user roles.
        var isUserInRole = await _loginRepository.Value.IsUserInRoleAsync(foundUser, userRole, ct);

        if (!isUserInRole)
        {
            return new() { StatusCode = LoginResponseStatusCode.FORBIDDEN };
        }

        // Init list of user claims.
        var userClaims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new("sub", foundUser.Id.ToString()),
            new("role", userRole)
        };

        // Create new refresh token.
        var newRefreshToken = InitNewRefreshToken(userClaims, command.IsRememberMe);

        // Add new refresh token to the database.
        var dbResult = await _loginRepository.Value.CreateRefreshTokenCommandAsync(
            newRefreshToken,
            ct
        );

        // Cannot add new refresh token to the database.
        if (!dbResult)
        {
            return new() { StatusCode = LoginResponseStatusCode.OPERATION_FAIL };
        }

        // Generate access token.
        var newAccessToken = _accessTokenHandler.Value.GenerateSigningToken(userClaims, 600);

        return new()
        {
            StatusCode = LoginResponseStatusCode.OPERATION_SUCCESS,
            Body = new()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Value,
                User = new() { Email = foundUser.Email, EmailConfirmed = foundUser.EmailConfirmed }
            }
        };
    }

    #region Others
    private UserTokenEntity InitNewRefreshToken(List<Claim> userClaims, bool isRememberMe)
    {
        return new()
        {
            LoginProvider = Guid.Parse(
                    userClaims.First(claim => claim.Type.Equals(value: "jti")).Value
                )
                .ToString(),
            ExpiredAt = isRememberMe ? DateTime.UtcNow.AddDays(15) : DateTime.UtcNow.AddDays(3),
            UserId = Guid.Parse(userClaims.First(claim => claim.Type.Equals("sub")).Value),
            Value = _refreshTokenHandler.Value.Generate(15),
            Name = "RefreshToken"
        };
    }
    #endregion
}
