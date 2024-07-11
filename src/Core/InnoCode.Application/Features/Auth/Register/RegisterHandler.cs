using System;
using System.Threading;
using System.Threading.Tasks;
using InnoCode.Application.Share.Features;
using InnoCode.Domain.Entities;
using InnoCode.Domain.UnitOfWorks.Main;
using InnoCode.Domain.UnitOfWorks.Main.Repositories.Auth;

namespace InnoCode.Application.Features.Auth.Register;

internal sealed class RegisterHandler : IFeatureHandler<RegisterRequest, RegisterResponse>
{
    private readonly Lazy<IRegisterRepository> _registerRepository;

    public RegisterHandler(Lazy<IMainUnitOfWork> unitOfWork)
    {
        _registerRepository = unitOfWork.Value.RegisterRepository;
    }

    /// <summary>
    ///     Entry of new request handler.
    /// </summary>
    /// <param name="command">
    ///     Request model.
    /// </param>
    /// <param name="ct">
    ///     A token that is used for notifying system
    ///     to cancel the current operation when user stop
    ///     the request.
    /// </param>
    /// <returns>
    ///     A task containing the response.
    /// </returns>
    public async Task<RegisterResponse> ExecuteAsync(RegisterRequest command, CancellationToken ct)
    {
        #region InputValidation
        // Init new user.
        UserEntity newUser = new() { Id = Guid.NewGuid() };

        // Is new user password valid.
        var isPasswordValid = await _registerRepository.Value.ValidateUserPasswordAsync(
            newUser,
            command.Password
        );

        // Password is not valid.
        if (!isPasswordValid)
        {
            return new() { StatusCode = RegisterResponseStatusCode.INPUT_VALIDATION_FAIL };
        }
        #endregion

        // Does user exist by email.
        var isUserFound = await _registerRepository.Value.IsUserFoundByNormalizedEmailQueryAsync(
            command.Email,
            ct
        );

        // User with email already exists.
        if (isUserFound)
        {
            return new() { StatusCode = RegisterResponseStatusCode.USER_IS_EXISTED };
        }

        // Completing new user.
        FinishFillingUser(newUser, command);

        // Create and add user to role.
        var dbResult = await _registerRepository.Value.CreateAndAddUserToRoleCommandAsync(
            newUser,
            command.Password,
            ct
        );

        // Cannot create or add user to role.
        if (!dbResult)
        {
            return new() { StatusCode = RegisterResponseStatusCode.OPERATION_FAIL };
        }

        return new() { StatusCode = RegisterResponseStatusCode.OPERATION_SUCCESS };
    }

    /// <summary>
    ///     Finishes filling the user with default
    ///     values for the newly created user.
    /// </summary>
    /// <param name="newUser">
    ///     The newly created user.
    /// </param>
    /// <param name="command">
    ///     Request model.
    /// </param>
    /// <param name="newAccountStatus">
    ///     The new account status.
    /// </param>
    /// <returns>
    ///     Nothing
    /// </returns>
    private static void FinishFillingUser(UserEntity newUser, RegisterRequest command)
    {
        newUser.Email = command.Email;
        newUser.UserName = command.Email;
    }
}
