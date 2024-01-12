using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.Common.SecureUrlGenerator;

namespace Netsoftware.Xanthos.Common.UrlTokenGenerator.Implementation;

public class SecureUrlProvider : ISecureUrlProvider
{
    private readonly ILogger<SecureUrlProvider> _logger;
    private readonly ISecureStringProvider _secureStringProvider;

    public SecureUrlProvider(ILogger<SecureUrlProvider> logger, ISecureStringProvider secureStringProvider)
    {
        _logger = logger;
        _secureStringProvider = secureStringProvider;
    }

    public string GetSecureUrl(string url, string stringToEncode, string purpose = "purpose")
    {
        try
        {
            CheckUrl(url);
            CheckStringToEncode(stringToEncode);

            _secureStringProvider.CreateProtector(purpose);
            var protectedString = _secureStringProvider.Encode(stringToEncode);
            return BuildUrl(url, protectedString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during execute of GetSecureUrl() method.");
            throw;
        }
    }

    public string GetSecureUrl(string url, EncodablePayload objectToEncode, string purpose = "puropse")
    {
        try
        {
            CheckUrl(url);
            CheckObjectToDecode(objectToEncode);

            _secureStringProvider.CreateProtector(purpose);
            var protectedPayload = _secureStringProvider.Encode(objectToEncode);
            return BuildUrl(url, protectedPayload);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during execute of GetSecureUrl() method.");
            throw;
        }
    }

    private static string BuildUrl(string url, string protectedString)
    {
        var builder = new StringBuilder(url).Append("?payload=").Append(protectedString);
        return builder.ToString();
    }

    private void CheckStringToEncode(string stringToEncode)
    {
        if (string.IsNullOrWhiteSpace(stringToEncode))
        {
            _logger.LogError("StringToDecode parameter is null");
            throw new ArgumentNullException("\"StringToDecode\" cannot be null");
        }
    }

    private void CheckUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogError("Url parameter is null");
            throw new ArgumentNullException("\"Url\" cannot be null");
        }

        if (!UrlFormatIsValid(url))
        {
            _logger.LogError("Invalid format of url");
            throw new FormatException(
                "\"Url\" have invalid format. It shouldn't have \"/\" at last character");
        }
    }

    private bool UrlFormatIsValid(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;

        return !url.Last().Equals('/');
    }

    private void CheckObjectToDecode(EncodablePayload encodablePayload)
    {
        if (encodablePayload == null)
        {
            _logger.LogError("EncodablePayload parameter is null");
            throw new ArgumentNullException("\"EncodablePayload\" cannot be null");
        }
    }
}