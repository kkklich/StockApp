using System;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.Common.SecureUrlGenerator;

namespace Netsoftware.Xanthos.Common.UrlTokenGenerator.Implementation;

public class SecureStringProvider : ISecureStringProvider
{
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly ILogger<SecureStringProvider> _logger;
    private IDataProtector _protector;

    public SecureStringProvider()
    {
    }

    public SecureStringProvider(ILogger<SecureStringProvider> logger,
        IDataProtectionProvider dataProtectionProvider)
    {
        _logger = logger;
        _dataProtectionProvider = dataProtectionProvider;
    }

    public void CreateProtector(string purpose)
    {
        if (string.IsNullOrWhiteSpace(purpose))
        {
            _logger.LogError("The \"purpose\" should have value");
            throw new ArgumentException("The \"purpose\" should have value");
        }

        _protector = _dataProtectionProvider.CreateProtector(purpose);
    }

    public string Encode(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            _logger.LogError("The \"data\" should have value");
            throw new ArgumentException("The \"data\" should have value");
        }

        if (_protector == null)
        {
            _logger.LogError("Protector not exist. Create protector by using \"CreateProtector()\" method");
            throw new ArgumentNullException(
                "Protector not exist. Create protector by using \"CreateProtector()\" method");
        }

        return _protector.Protect(data);
    }

    public string Encode(EncodablePayload data)
    {
        if (data == null)
        {
            _logger.LogError("The \"data\" should have value");
            throw new ArgumentException("The \"data\" should have value");
        }

        return Encode(data.Serialize());
    }

    public string Decode(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            _logger.LogError("The \"data\" should have value");
            throw new ArgumentException("The \"data\" should have value");
        }

        if (_protector == null)
        {
            _logger.LogError("Protector not exist. Create protector by using \"CreateProtector()\" method");
            throw new ArgumentNullException(
                "Protector not exist. Create protector by using \"CreateProtector()\" method");
        }

        return _protector.Unprotect(data);
    }

    public TOutput Decode<TOutput>(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            _logger.LogError("The \"data\" should have value");
            throw new ArgumentException("The \"data\" should have value");
        }

        var json = Decode(data);
        return JsonSerializer.Deserialize<TOutput>(json);
    }

    public bool ProtectorIsReady()
    {
        return _protector != null;
    }
}