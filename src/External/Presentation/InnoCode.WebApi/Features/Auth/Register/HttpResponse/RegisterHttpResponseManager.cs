using System;
using System.Collections.Generic;
using InnoCode.Application.Features.Auth.Register;
using Microsoft.AspNetCore.Http;

namespace InnoCode.WebApi.Features.Auth.Register.HttpResponse;

internal static class RegisterHttpResponseManager
{
    private static Dictionary<
        RegisterResponseStatusCode,
        Func<RegisterRequest, RegisterResponse, RegisterHttpResponse>
    > _dictionary;

    private static void Init()
    {
        _dictionary = new();

        _dictionary.TryAdd(
            key: RegisterResponseStatusCode.INPUT_VALIDATION_FAIL,
            value: (_, response) =>
                new()
                {
                    HttpCode = StatusCodes.Status400BadRequest,
                    AppCode = response.StatusCode.ToAppCode()
                }
        );

        _dictionary.TryAdd(
            key: RegisterResponseStatusCode.OPERATION_FAIL,
            value: (_, response) =>
                new()
                {
                    HttpCode = StatusCodes.Status500InternalServerError,
                    AppCode = response.StatusCode.ToAppCode()
                }
        );

        _dictionary.TryAdd(
            key: RegisterResponseStatusCode.OPERATION_SUCCESS,
            value: (_, response) =>
                new()
                {
                    HttpCode = StatusCodes.Status200OK,
                    AppCode = response.StatusCode.ToAppCode(),
                }
        );

        _dictionary.TryAdd(
            key: RegisterResponseStatusCode.USER_IS_EXISTED,
            value: (_, response) =>
                new()
                {
                    HttpCode = StatusCodes.Status409Conflict,
                    AppCode = response.StatusCode.ToAppCode(),
                }
        );
    }

    internal static Func<RegisterRequest, RegisterResponse, RegisterHttpResponse> Resolve(
        RegisterResponseStatusCode statusCode
    )
    {
        if (Equals(objA: _dictionary, objB: default))
        {
            Init();
        }

        return _dictionary[statusCode];
    }
}
