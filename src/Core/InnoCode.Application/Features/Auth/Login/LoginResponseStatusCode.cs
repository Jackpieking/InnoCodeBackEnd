namespace InnoCode.Application.Features.Auth.Login;

public enum LoginResponseStatusCode
{
    USER_IS_NOT_FOUND,
    USER_IS_TEMPORARILY_LOCKED_OUT,
    PASSWORD_INCORRECT,
    INPUT_VALIDATION_FAIL,
    FORBIDDEN,
    OPERATION_SUCCESS,
    OPERATION_FAIL
}
