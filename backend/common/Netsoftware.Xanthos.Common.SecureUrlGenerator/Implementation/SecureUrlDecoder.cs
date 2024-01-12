using System;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.Common.SecureUrlGenerator;

namespace Netsoftware.Xanthos.Common.UrlTokenGenerator.Implementation;

public class SecureUrlDecoder : ISecureUrlDecoder
{
    private readonly ILogger<SecureUrlDecoder> _logger;
    private readonly ISecureStringProvider _secureStringProvider;

    public SecureUrlDecoder(ILogger<SecureUrlDecoder> logger, ISecureStringProvider secureStringProvider)
    {
        _logger = logger;
        _secureStringProvider = secureStringProvider;
    }

    /// <summary>
    ///     Return object with specific type.
    /// </summary>
    /// <param name="payloadToDecode"></param>
    /// <param name="purpose"></param>
    /// <typeparam name="TResult">Class should inherit from EncodablePayload</typeparam>
    /// <returns></returns>
    public TResult Decode<TResult>(string payloadToDecode, string purpose = "purpose") where TResult : EncodablePayload
    {
        try
        {
            _secureStringProvider.CreateProtector(purpose);
            return _secureStringProvider.Decode<TResult>(payloadToDecode);
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                $"Error during execute of Decode() method. Type of expected object is {typeof(TResult).FullName}");
            throw;
        }
    }

    /// <summary>
    ///     Return object as JSON
    /// </summary>
    /// <param name="urlToDecode"></param>
    /// <returns></returns>
    public string Decode(string payloadToDecode, string purpose = "purpose")
    {
        try
        {
            _secureStringProvider.CreateProtector(purpose);
            return _secureStringProvider.Decode(payloadToDecode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during execute of Decode() method.");
            throw;
        }
    }

    private static string GetPayload(string urlToDecode)
    {
        if (!urlToDecode.Contains("?payload=")) throw new ArgumentNullException("Payload not exist");

        return urlToDecode.Split("?payload=")[1];
    }
}