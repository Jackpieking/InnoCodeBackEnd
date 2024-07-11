using System;
using InnoCode.Application.Share.DataProtection;
using InnoCode.Configuration.Presentation.WebApi.SecurityKey;
using Microsoft.AspNetCore.DataProtection;

namespace ODour.AppIdentityService.Handlers;

public sealed class AppDataProtectionHandler : IDataProtectionHandler
{
    private readonly IDataProtector _protector;

    public AppDataProtectionHandler(
        Lazy<IDataProtectionProvider> dataProtectionProvider,
        Lazy<AppBaseProtectionSecurityKeyOption> options
    )
    {
        _protector = dataProtectionProvider.Value.CreateProtector(purpose: options.Value.Value);
    }

    public string Protect(string plaintext)
    {
        if (string.IsNullOrWhiteSpace(value: plaintext))
        {
            return default;
        }

        return _protector.Protect(plaintext: plaintext);
    }

    public string Unprotect(string ciphertext)
    {
        if (string.IsNullOrWhiteSpace(value: ciphertext))
        {
            return default;
        }

        try
        {
            return _protector.Unprotect(protectedData: ciphertext);
        }
        catch
        {
            return default;
        }
    }
}
